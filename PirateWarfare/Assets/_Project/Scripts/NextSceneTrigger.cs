using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    public string sceneName;
    public int level;

    // Changes to a scene based on the variable set
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerData.levelsComplete >= (level - 1))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
