using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    
    // Tracks total active enemies to know when to end the level
    void Start()
    {
        LevelManager.activeEnemies++;
    }

    // If an enemy dies it tracks, once it hits 0 the round is over
    void OnDestroy()
    {
        LevelManager.activeEnemies--;
        GameUtilities.currentEnemiesLeft--;
    }
}