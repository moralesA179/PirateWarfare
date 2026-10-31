using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "World Map";
    [Tooltip("How many seconds to wait after the last kill before loading the next scene")]
    public float delayBeforeLoading = 3f;

    // Global counters that any script can access without needing a reference
    public static int activeSpawners = 0;
    public static int activeEnemies = 0;

    private bool isLoading = false;
    private float safetyTimer = 1f;

    void Awake()
    {
        // Reset counters every time a new level loads
        activeSpawners = 0;
        activeEnemies = 0;
    }

    void Update()
    {
        if (isLoading) return;

        // 1. Wait 1 second when the game starts to let all spawners register themselves
        if (safetyTimer > 0)
        {
            safetyTimer -= Time.deltaTime;
            return;
        }

        // 2. Win Condition: No spawners are running AND no enemies are alive
        if (activeSpawners <= 0 && activeEnemies <= 0)
        {
            isLoading = true;
            Debug.Log("Level Complete! Loading next scene...");
            Invoke("LoadNextMenu", delayBeforeLoading); // Use Invoke to create a brief delay
        }
    }

    private void LoadNextMenu()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}