using UnityEngine;
using System.Collections.Generic;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // Assign your rock prefab in the inspector
    public int poolSize = 10; // Number of rocks to pre-instantiate
    public float minSpawnRate = 1f; // Minimum spawn rate
    public float maxSpawnRate = 5f; // Maximum spawn rate
    public int numRocas = 1;
    public Vector2 spawnSizeRange = new Vector2(0.5f, 2f); // Min and max scale for the rocks

    private float timer;
    private float currentSpawnRate;
    private int currentRockIndex = 0;

    private List<GameObject> rockPool = new List<GameObject>();

    void Start()
    {
        // Pre-instantiate rocks and disable them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity, transform);
            rock.SetActive(false);
            rockPool.Add(rock);
        }

        // Set initial spawn rate
        SetRandomSpawnRate();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > currentSpawnRate)
        {
            int numRocksToSpawn = Random.Range(1, numRocas); // Random number of rocks to spawn between 1 and 3
            for (int i = 0; i < numRocksToSpawn; i++)
            {
                SpawnRock();
            }

            // Reset timer and set a new random spawn rate
            timer = 0f;
            SetRandomSpawnRate();
        }
    }

    void SpawnRock()
    {
        GameObject currentRock = rockPool[currentRockIndex];
        currentRock.transform.position = CalculateSpawnPosition();
        currentRock.transform.localScale = CalculateSpawnScale();

        // Set the spawner object as the parent of the rock
        currentRock.transform.SetParent(transform);

        currentRock.SetActive(true);

        // Move to the next rock in the pool
        currentRockIndex = (currentRockIndex + 1) % poolSize;
    }

    void SetRandomSpawnRate()
    {
        currentSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
    }

    Vector3 CalculateSpawnPosition()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("No collider found on the spawner object. Cannot calculate spawn position.");
            return Vector3.zero;
        }

        // Calculate spawn position within the bounds of the container
        Vector3 center = collider.bounds.center;
        Vector3 extents = collider.bounds.extents;

        float spawnX = Random.Range(center.x - extents.x, center.x + extents.x);
        float spawnZ = Random.Range(center.z - extents.z, center.z + extents.z);
        float spawnY = center.y + extents.y; // Set spawn position at the top of the container

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        return spawnPosition;
    }

    Vector3 CalculateSpawnScale()
    {
        // Random scale within the specified range
        float scale = Random.Range(spawnSizeRange.x, spawnSizeRange.y);
        return new Vector3(scale, scale, scale);
    }

    // Method to check if an object is pooled
    public bool IsObjectPooled(GameObject obj)
    {
        return rockPool.Contains(obj);
    }

    // Method to repool an object
    public void RepoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}