using UnityEngine;

public class RammerSpawner : MonoBehaviour
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

    private void Start()
    {
        LevelManager.activeSpawners++; 
        timer = timeBetweenWaves; 
    }

    void Update()
    {
        // Shut down the spawner logic  when finished
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
                EnemyMovement movementScript = spawnedEnemy.GetComponent<EnemyMovement>();
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
