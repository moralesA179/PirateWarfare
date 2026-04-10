using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameUtilities : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0, 100)]
    public int damage = 50;

    [Range(0, 100)]
    public int heal = 50;


    public Button button, resumeButton, quitButton, healButton;
    public GameObject pauseMenu;


    public void Start()
    {
        if(button != null)
            button.onClick.AddListener(() => { PlayerData.TakeDamage(damage); });
        resumeButton.onClick.AddListener(() => { Time.timeScale = 1f; pauseMenu.SetActive(false); });
        quitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("health", PlayerData.currentHealth);
            PlayerPrefs.Save();
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
        });

        if(healButton != null)
            healButton.onClick.AddListener(() => { PlayerData.Heal(heal); });
    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("I was in here!!!");
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
