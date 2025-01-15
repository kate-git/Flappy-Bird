using UnityEngine;
using System.Collections.Generic;

public class TerrainSpawner : MonoBehaviour
{
    [Header("Terrain Settings")]
    public GameObject[] terrainPrefabs; // Array to hold all terrain prefabs
    public int initialBlocks = 3; // Number of blocks to spawn at the start
    public float blockLength = 20f; // Length of each terrain block
    public int maxBlocks = 5; // Maximum number of active terrain blocks

    [Header("Player Settings")]
    public Transform player; // Transform of the player or camera

    [Header("Spawn Settings")]
    public float safeZone = 40f; // Distance before a block is recycled

    private Queue<GameObject> activeBlocks = new Queue<GameObject>(); // Queue to track active terrain blocks
    private float spawnZ = 0f; // Z-position for the next spawn

    void Start()
    {
        // Spawn the initial blocks
        for (int i = 0; i < initialBlocks; i++)
        {
            SpawnBlock();
        }
    }

    void Update()
    {
        // Check if new blocks need to be spawned
        if (player.position.z - safeZone > (spawnZ - initialBlocks * blockLength))
        {
            SpawnBlock();
            RecycleBlock();
        }
    }

    void SpawnBlock()
    {
        GameObject block;

        if (activeBlocks.Count >= maxBlocks)
        {
            // Reuse the oldest block if the maximum number of blocks is reached
            block = activeBlocks.Dequeue();
            block.transform.position = Vector3.forward * spawnZ;
            block.SetActive(true);
            Debug.Log($"Reusing block at position: {block.transform.position}");
        }
        else
        {
            // Instantiate a new block
            GameObject randomPrefab = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];
            block = Instantiate(randomPrefab, Vector3.forward * spawnZ, Quaternion.identity);
        }

        spawnZ += blockLength; // Update the spawn position
        activeBlocks.Enqueue(block);
    }

    void RecycleBlock()
    {
        if (activeBlocks.Count > maxBlocks)
        {
            GameObject blockToRecycle = activeBlocks.Dequeue();
            blockToRecycle.SetActive(false); // Deactivate the block for performance optimization
            Debug.Log($"Recycling block at position: {blockToRecycle.transform.position}");
        }
    }
}
