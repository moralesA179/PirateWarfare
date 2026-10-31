using UnityEngine;
using UnityEngine.SceneManagement;

public class tempLevelCompleter : MonoBehaviour
{
    public int level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (PlayerData.levelsComplete < level)
            {
                PlayerData.levelsComplete += 1;
            }
            Debug.Log("Levels complete: " + PlayerData.levelsComplete);
            SceneManager.LoadScene("World Map");
        }
    }
}
