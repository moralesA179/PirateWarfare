using UnityEngine;

public class RangerSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float timeBetweenWaves = 15f;
    public int maxWaves = 3;
    public int enemiesPerWave = 3;

    public float spawnRadius = 5f;

    public Transform targetPlayer;

    private float timer;
    private int currentWave = 0; 
    private bool isDoneSpawning = false;

    void Start()
    {
        LevelManager.activeSpawners++;
        timer = timeBetweenWaves;
    }

    void Update()
    {
        // Shut down the spawner when done
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
            return;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            GameObject spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            if (targetPlayer != null)
            {
                // Assigning to CircularMovement for the Ranger
                CircularMovement movementScript = spawnedEnemy.GetComponent<CircularMovement>();
                if (movementScript != null)
                {
                    movementScript.player = targetPlayer;
                }
            }
        }
    }
    
    // Show where the spawner is 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}