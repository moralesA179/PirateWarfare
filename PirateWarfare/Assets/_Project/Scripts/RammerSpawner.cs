using UnityEngine;

public class RammerSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefabToSpawn;
    public float spawnInterval = 3f;

    [Header("Position Settings")]
    public float spawnRadius = 5f;

    [Header("Targeting")]
    [Tooltip("Drag the Player from your scene into this slot")]
    public Transform targetPlayer;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    void SpawnObject()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Spawner doesn't have a prefab assigned!");
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        // 1. Store the instantiated object in a variable
        GameObject spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // 2. Assign the target player to the spawned enemy's movement script
        if (targetPlayer != null)
        {
            EnemyMovement movementScript = spawnedEnemy.GetComponent<EnemyMovement>();
            if (movementScript != null)
            {
                movementScript.player = targetPlayer;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}