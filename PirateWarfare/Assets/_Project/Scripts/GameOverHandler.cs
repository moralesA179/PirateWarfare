using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Button retryButton, quitButton;

    private void Start()
    {
        retryButton.onClick.AddListener(() => {
            PlayerData.currentHealth = PlayerData.maxHealth;
            SceneManager.LoadScene("World Map");
        });

        quitButton.onClick.AddListener(() => {
            PlayerPrefs.SetInt("health", PlayerData.maxHealth);
            PlayerPrefs.Save();
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        });
    }
}
