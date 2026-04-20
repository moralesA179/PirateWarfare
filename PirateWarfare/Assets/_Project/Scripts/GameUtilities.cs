using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUtilities : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0, 100)]
    public int damage = 50;

    [Range(0, 100)]
    public int heal = 50;


    public Button button, resumeButton, saveButton,quitButton, healButton;
    public GameObject pauseMenu;
    public TMP_Text scoreText;


    public void Start()
    {
        if(button != null)
            button.onClick.AddListener(() => { PlayerData.TakeDamage(damage); });
        if(resumeButton != null)
            resumeButton.onClick.AddListener(() => { Time.timeScale = 1f; pauseMenu.SetActive(false); });
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(() =>
            {
                string[] cannons = PlayerData.CannonInventory.Keys.Count != 0 ? PlayerData.CannonInventory.Keys.ToArray() : new string[] { "Base" };
                string[] projectiles = PlayerData.ProjectileInventory.Keys.Count != 0 ? PlayerData.ProjectileInventory.Keys.ToArray() : new string[] { "Base" };
                SaveManager.SaveGame(PlayerData.currentHealth, PlayerData.maxHealth, PlayerData.speedMult, PlayerData.baseDamage, PlayerData.score, cannons, projectiles);
            });
        }
        if (quitButton != null)
            quitButton.onClick.AddListener(() => {
                SceneManager.LoadScene("MainMenu");
            });
        if (healButton != null)
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

        if (scoreText != null)
        {
            scoreText.text = $"Score: {PlayerData.score}";
        }
    }
}
