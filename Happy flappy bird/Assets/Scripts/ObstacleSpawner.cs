using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Player & Prefabs")]
    [Tooltip("Reference to the player (e.g., VR Rig). Used for determining spawn positions along the Z axis.")]
    public Transform player;

    [Tooltip("Exactly 5 different obstacle prefabs. Each will appear at least once.")]
    public GameObject[] obstaclePrefabs; // 5 prefabs total

    [Header("Which Are Ground-Only?")]
    [Tooltip("For each prefab (same index), check true if it must be on the ground at groundY.")]
    public bool[] groundObjects; // Must have the same length as 'obstaclePrefabs'

    [Header("Pool Settings")]
    [Tooltip("Total number of obstacles in the pool. Must be >= 5.")]
    public int poolSize = 10;

    [Header("Spawn Settings")]
    [Tooltip("Time (in seconds) between spawns.")]
    public float spawnInterval = 15f;

    [Tooltip("Distance along +Z (in front of the player) to spawn obstacles.")]
    public float forwardDistance = 10f;

    [Header("Random Offset Ranges (for non-ground objects)")]
    [Tooltip("Minimum X offset from the player's X position.")]
    public float minXOffset = -5f;
    [Tooltip("Maximum X offset from the player's X position.")]
    public float maxXOffset = 5f;

    [Tooltip("Minimum Y offset from the player's Y position.")]
    public float minYOffset = 0f;
    [Tooltip("Maximum Y offset from the player's Y position.")]
    public float maxYOffset = 3f;

    [Header("Ground Settings (no raycast)")]
    [Tooltip("If the object is ground-only, place it here on the Y-axis.")]
    public float groundY = 0f;

    [Header("Absolute X Range for Ground Objects")]
    [Tooltip("Lowest (leftmost) X world position for ground-only spawns.")]
    public float groundMinXPos = -5f;
    [Tooltip("Highest (rightmost) X world position for ground-only spawns.")]
    public float groundMaxXPos = 5f;

    // The obstacle pool
    private List<GameObject> obstaclePool = new List<GameObject>();

    // Map: "this GameObject" -> "which index of obstaclePrefabs"
    private Dictionary<GameObject, int> obstacleIndexMap = new Dictionary<GameObject, int>();

    // Coroutine for continuous spawning
    private Coroutine spawnRoutine;

    void Start()
    {
        // Make sure we have at least 5 in the pool
        if (poolSize < obstaclePrefabs.Length)
        {
            poolSize = obstaclePrefabs.Length;
            Debug.LogWarning("[ObstacleSpawner] Pool size increased to match the number of unique prefabs.");
        }

        // Also ensure groundObjects[] matches obstaclePrefabs[] length
        if (groundObjects.Length != obstaclePrefabs.Length)
        {
            Debug.LogError("[ObstacleSpawner] 'groundObjects' must be the same length as 'obstaclePrefabs'!");
        }

        InitializePool();
        spawnRoutine = StartCoroutine(SpawnObstacles());
    }

    /// <summary>
    /// Checks if the player is 10 units ahead of any active obstacle; if so, return it to the pool.
    /// </summary>
    void Update()
    {
        for (int i = 0; i < obstaclePool.Count; i++)
        {
            GameObject obstacle = obstaclePool[i];
            if (obstacle.activeInHierarchy)
            {
                // If player's Z-pos is at least 10 greater than the obstacle's Z-pos
                if (player.position.z - obstacle.transform.position.z > 10f)
                {
                    DeactivateObstacle(obstacle);
                }
            }
        }
    }

    /// <summary>
    /// Creates the pool by first instantiating one of each prefab,
    /// then filling up the rest (if poolSize > 5) with random picks.
    /// </summary>
    void InitializePool()
    {
        // First, create one instance of each prefab
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            GameObject obstacleInstance = Instantiate(obstaclePrefabs[i]);
            obstacleInstance.SetActive(false);

            obstaclePool.Add(obstacleInstance);
            // Map object -> index
            obstacleIndexMap[obstacleInstance] = i;
        }

        // Fill remaining slots if poolSize > number of prefabs
        for (int i = obstaclePrefabs.Length; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstacleInstance = Instantiate(obstaclePrefabs[randomIndex]);
            obstacleInstance.SetActive(false);

            obstaclePool.Add(obstacleInstance);
            // Map object -> randomIndex
            obstacleIndexMap[obstacleInstance] = randomIndex;
        }
    }

    /// <summary>
    /// Spawns obstacles every 'spawnInterval' seconds indefinitely.
    /// </summary>
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnSingleObstacle();
        }
    }

    /// <summary>
    /// Spawns a single obstacle from the pool at the correct position.
    /// </summary>
    void SpawnSingleObstacle()
    {
        // Get a random inactive obstacle
        GameObject obstacle = GetRandomInactiveObstacle();
        if (obstacle == null)
        {
            // No inactive obstacle available—nothing spawned this time
            return;
        }

        // Find which prefab index this object originally came from
        int prefabIndex = obstacleIndexMap[obstacle];

        // Check if it's ground-only
        bool mustBeOnGround = groundObjects[prefabIndex];

        // Base position in front of the player (along +Z)
        Vector3 spawnPosition = new Vector3(
            player.position.x,
            player.position.y,
            player.position.z + forwardDistance
        );

        if (mustBeOnGround)
        {
            // Y is fixed at groundY
            spawnPosition.y = groundY;

            // Instead of offset from player.x, 
            // we pick a random world-space X between groundMinXPos and groundMaxXPos
            float randomX = Random.Range(groundMinXPos, groundMaxXPos);
            spawnPosition.x = randomX;
        }
        else
        {
            // If not ground-only, randomize X and Y offset from player's position
            float offsetX = Random.Range(minXOffset, maxXOffset);
            float offsetY = Random.Range(minYOffset, maxYOffset);

            spawnPosition.x += offsetX;
            spawnPosition.y += offsetY;
        }

        obstacle.transform.position = spawnPosition;
        obstacle.transform.rotation = Quaternion.identity;
        obstacle.SetActive(true);
    }

    /// <summary>
    /// Finds a random obstacle that isn't currently active.
    /// </summary>
    GameObject GetRandomInactiveObstacle()
    {
        List<GameObject> inactiveObstacles = obstaclePool.FindAll(o => !o.activeInHierarchy);
        if (inactiveObstacles.Count == 0)
            return null;

        int randomIndex = Random.Range(0, inactiveObstacles.Count);
        return inactiveObstacles[randomIndex];
    }

    /// <summary>
    /// Deactivate an obstacle so it returns to the pool.
    /// </summary>
    public void DeactivateObstacle(GameObject obstacle)
    {
        obstacle.SetActive(false);
    }
}