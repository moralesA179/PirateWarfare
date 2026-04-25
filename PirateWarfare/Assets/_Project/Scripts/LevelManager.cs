using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneToLoad = "World Map";
    public float delayBeforeLoading = 3f;

    // Global counters
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

        if (safetyTimer > 0)
        {
            safetyTimer -= Time.deltaTime;
            return;
        }

        // Level ends when all spawners are deactivated, and all enemies are defeated
        if (activeSpawners <= 0 && activeEnemies <= 0)
        {
            isLoading = true;
            Debug.Log("Level Complete! Loading next scene...");
            Invoke("LoadNextMenu", delayBeforeLoading); 
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