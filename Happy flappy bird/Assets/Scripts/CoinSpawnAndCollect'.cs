using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinSpawnAndCollectWithSound : MonoBehaviour
{
    [Header("Coin Pool Settings")]
    [Tooltip("The Coin prefab to spawn (should have a trigger collider and tag = 'Coin').")]
    public GameObject coinPrefab;

    [Tooltip("How many coins to keep in the pool.")]
    public int poolSize = 10;

    [Header("Spawn Settings")]
    [Tooltip("How often (seconds) a new coin is spawned.")]
    public float spawnInterval = 2f;

    [Tooltip("How far in front of the player the coin is spawned (along +Z).")]
    public float forwardDistance = 15f;

    [Header("Random X/Y Ranges for Coins")]
    [Tooltip("Minimum X world position for coin spawn.")]
    public float minX = -3f;
    [Tooltip("Maximum X world position for coin spawn.")]
    public float maxX = 3f;
    [Tooltip("Minimum Y world position for coin spawn.")]
    public float minY = 1f;
    [Tooltip("Maximum Y world position for coin spawn.")]
    public float maxY = 1f;

    [Header("Coin Collection Tracking")]
    [Tooltip("Total coins collected (visible in Inspector).")]
    public int coinCount = 0;

    [Header("Audio Settings")]
    [Tooltip("Sound that plays when a coin is collected.")]
    public AudioClip coinCollectSound;

    // Internals
    private List<GameObject> coinPool = new List<GameObject>();
    private Coroutine spawnRoutine;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component on the same GameObject (the player)
        audioSource = GetComponent<AudioSource>();

        InitializePool();
        spawnRoutine = StartCoroutine(SpawnCoinsOverTime());
    }

    void Update()
    {
        // Check if any active coin is behind the player by 10 units on Z
        for (int i = 0; i < coinPool.Count; i++)
        {
            GameObject coin = coinPool[i];
            if (coin.activeInHierarchy)
            {
                // If player is 10 units past the coin in Z
                if (transform.position.z - coin.transform.position.z > 10f)
                {
                    DeactivateCoin(coin);
                }
            }
        }
    }

    /// <summary>
    /// Initialize the coin pool with 'poolSize' coins (inactive).
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);  // Start inactive
            coinPool.Add(coin);
        }
    }

    /// <summary>
    /// Coroutine: every 'spawnInterval' seconds, spawn a coin.
    /// </summary>
    private IEnumerator SpawnCoinsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCoin();
        }
    }

    /// <summary>
    /// Grabs an inactive coin, positions it, sets active.
    /// </summary>
    private void SpawnCoin()
    {
        GameObject coin = GetInactiveCoin();
        if (coin == null)
            return; // no free coin in pool

        // Decide a random X/Y within your specified range
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // We'll place coin forward of the player on Z
        // but randomize X and Y within the set ranges.
        Vector3 spawnPos = new Vector3(randomX, randomY, transform.position.z + forwardDistance);

        coin.transform.position = spawnPos;
        coin.SetActive(true);
    }

    /// <summary>
    /// Find first inactive coin in pool.
    /// </summary>
    private GameObject GetInactiveCoin()
    {
        for (int i = 0; i < coinPool.Count; i++)
        {
            if (!coinPool[i].activeInHierarchy)
            {
                return coinPool[i];
            }
        }
        return null; // no inactive coin
    }

    /// <summary>
    /// Deactivate so it can be reused.
    /// </summary>
    private void DeactivateCoin(GameObject coin)
    {
        coin.SetActive(false);
    }

    /// <summary>
    /// Detect collision (trigger) with coin. 
    /// - The coin must have a collider with 'Is Trigger' = true
    /// - The coin must have the tag 'Coin'
    /// - The player must have a non-trigger collider + Rigidbody
    ///   (and an AudioSource to play the sound).
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Debug.Log($"Coin collected! Total coins = {coinCount}");

            // Deactivate the coin so it can be reused
            DeactivateCoin(other.gameObject);

            // Play the coin sound if assigned
            if (coinCollectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinCollectSound);
            }
        }
    }
}