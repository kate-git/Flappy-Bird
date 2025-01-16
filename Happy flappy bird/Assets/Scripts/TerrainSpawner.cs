using UnityEngine;
using System.Collections.Generic;

public class TerrainSpawner : MonoBehaviour
{
    [Header("Terrain Settings")]
    [Tooltip("Alla Terrain-prefabs (varje prefab måste ha en Terrain-komponent i något child-objekt).")]
    public GameObject[] terrainPrefabs;

    [Tooltip("Max antal aktiva block i scenen innan vi återanvänder det äldsta.")]
    public int maxBlocks = 5;

    [Header("Player Settings")]
    [Tooltip("Spelarens/kamerans transform som rör sig i Z-axeln.")]
    public Transform player;

    [Header("Spawn/Recycling Settings")]
    [Tooltip("Hur långt framför spelaren vi ska trigga nästa block-spawn (dvs hur tidigt vi spawnar).")]
    public float spawnTriggerDistance = 50f;

    [Tooltip("Hur mycket i Z-led vi ska offseta blockets position när det spawnas.")]
    public float spawnOffset = 0f;

    // Interna variabler
    private Queue<GameObject> activeBlocks = new Queue<GameObject>();
    private float currentSpawnZ = 0f;
    private int nextPrefabIndex = 0;
    private Dictionary<int, float> prefabLengths = new Dictionary<int, float>();

    void Start()
    {
        // Kolla att prefabs har Terrain-komponent i sina barn och hämta dess längd
        for (int i = 0; i < terrainPrefabs.Length; i++)
        {
            float length = GetTerrainLength(terrainPrefabs[i]);
            if (length <= 0f)
            {
                Debug.LogError(
                    $"Prefab '{terrainPrefabs[i].name}' saknar Terrain-komponent " +
                    "eller har ogiltig TerrainData i child-objekt!"
                );
                continue;
            }
            prefabLengths[i] = length;
        }

        // Spawnar en första omgång: alla prefabs en gång + 2 extra.
        // -> Då ligger redan minst två block "i förväg".
        for (int i = 0; i < terrainPrefabs.Length + 2; i++)
        {
            SpawnBlock();
        }
    }

    void Update()
    {
        // Om spelaren är så pass nära 'currentSpawnZ' (alltså den punkt där nästa block tar vid)
        // minus en viss "framför-distans" (spawnTriggerDistance),
        // så spawnar vi en ny bit i god tid innan spelaren når dit.
        if (player.position.z + spawnTriggerDistance > currentSpawnZ)
        {
            SpawnBlock();
            RecycleOldestIfNeeded();
        }
    }

    /// <summary>
    /// Hämtar Terrain-komponent (i child-objekt) och returnerar Z-storleken.
    /// </summary>
    float GetTerrainLength(GameObject prefab)
    {
        Terrain terrain = prefab.GetComponentInChildren<Terrain>();
        if (terrain != null && terrain.terrainData != null)
        {
            return terrain.terrainData.size.z;
        }
        return 0f;
    }

    /// <summary>
    /// Spawnar (eller återanvänder) nästa block i ordningen,
    /// och lägger det på 'currentSpawnZ + spawnOffset' i Z-led.
    /// Stegar sedan 'currentSpawnZ' med längden på blocket.
    /// </summary>
    void SpawnBlock()
    {
        int index = nextPrefabIndex;
        if (!prefabLengths.ContainsKey(index) || prefabLengths[index] <= 0f)
        {
            Debug.LogWarning($"Prefab med index {index} är ogiltig. Avbryter SpawnBlock.");
            return;
        }

        float length = prefabLengths[index];
        GameObject prefab = terrainPrefabs[index];
        GameObject block;

        if (activeBlocks.Count >= maxBlocks)
        {
            // Återanvänd äldsta blocket om vi redan har för många aktiva
            block = activeBlocks.Dequeue();
            block.SetActive(true);
            block.transform.position = new Vector3(0f, 0f, currentSpawnZ + spawnOffset);
            Debug.Log($"[Reuse] Lägger {block.name} på Z: {currentSpawnZ + spawnOffset}");
        }
        else
        {
            // Annars instansierar vi ett nytt
            block = Instantiate(prefab, new Vector3(0f, 0f, currentSpawnZ + spawnOffset), Quaternion.identity);
            Debug.Log($"[New] Lägger {block.name} på Z: {currentSpawnZ + spawnOffset}");
        }

        activeBlocks.Enqueue(block);

        // Flytta fram 'currentSpawnZ' med blockets längd
        currentSpawnZ += length;

        // Välj nästa prefab i rundtur
        nextPrefabIndex = (nextPrefabIndex + 1) % terrainPrefabs.Length;
    }

    /// <summary>
    /// Om vi har fler aktiva block än max, inaktiverar vi det äldsta.
    /// </summary>
    void RecycleOldestIfNeeded()
    {
        if (activeBlocks.Count > maxBlocks)
        {
            GameObject oldestBlock = activeBlocks.Dequeue();
            oldestBlock.SetActive(false);
            Debug.Log($"Inaktiverar block [{oldestBlock.name}] som är för långt bak.");
        }
    }
}