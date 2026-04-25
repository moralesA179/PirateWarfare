using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    void Start()
    {
        // When this enemy is spawned, increase the global count
        LevelManager.activeEnemies++;
    }

    void OnDestroy()
    {
        // When this enemy dies (or is removed), decrease the global count
        LevelManager.activeEnemies--;
        GameUtilities.currentEnemiesLeft--;
    }
}