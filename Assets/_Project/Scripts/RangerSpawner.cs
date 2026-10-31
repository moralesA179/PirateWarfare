using UnityEngine;

public class RangerSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject prefabToSpawn;
    [Tooltip("How long to wait between each wave in seconds")]
    public float timeBetweenWaves = 15f;
    [Tooltip("Total number of waves to spawn before stopping")]
    public int maxWaves = 3;
    [Tooltip("How many enemies spawn at the exact same time per wave")]
    public int enemiesPerWave = 3;

    [Header("Position Settings")]
    public float spawnRadius = 5f;

    [Header("Targeting")]
    [Tooltip("Drag the Player from your scene into this slot")]
    public Transform targetPlayer;

    private float timer;
    private int currentWave = 0;
    private bool isDoneSpawning = false;

    void Start()
    {
        // Announce to LevelManager that this spawner is active
        LevelManager.activeSpawners++;

        // Start the timer at the max value so the first wave spawns immediately 
        timer = timeBetweenWaves;
    }

    void Update()
    {
        // Shut down the spawner and notify LevelManager when all waves are out
        if (currentWave >= maxWaves)
        {
            if (!isDoneSpawning)
            {
                LevelManager.activeSpawners--;
                isDoneSpawning = true;
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= timeBetweenWaves)
        {
            SpawnWave();
            currentWave++;
            timer = 0f;
        }
    }

    void SpawnWave()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Spawner doesn't have a prefab assigned!");
            return;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            GameObject spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            if (targetPlayer != null)
            {
                // Assigning to CircularMovement specifically for the Ranger
                CircularMovement movementScript = spawnedEnemy.GetComponent<CircularMovement>();
                if (movementScript != null)
                {
                    movementScript.player = targetPlayer;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}