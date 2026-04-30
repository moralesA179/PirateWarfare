using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUtilities : MonoBehaviour
{
    [Range(0, 100)]
    public int damage = 50;

    [Range(0, 100)]
    public int heal = 50;


    public RangerSpawner[] rangerSpawners;
    public RammerSpawner[] rammerSpawners;
    private int totalEnemiesToSpawn = 0;
    public static int currentEnemiesLeft = 0;
    public Button button, resumeButton, saveButton,quitButton, healButton;
    public GameObject pauseMenu;
    public TMP_Text scoreText, waveText;



    public void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        waveText.transform.gameObject.SetActive(currentSceneName.Contains("Level")); //dont show wave counter in world map or other menus
        if (rangerSpawners != null)
        {
            foreach (RangerSpawner rS in rangerSpawners)
            {
                totalEnemiesToSpawn += rS.enemiesPerWave * rS.maxWaves;
            }
        }

        if (rammerSpawners != null)
        {
            foreach(RammerSpawner rS in rammerSpawners)
            {
                totalEnemiesToSpawn += rS.enemiesPerWave * rS.maxWaves;
            }
        }
        currentEnemiesLeft = totalEnemiesToSpawn;
        if (button != null)
            button.onClick.AddListener(() => { PlayerData.TakeDamage(damage); });
        if(resumeButton != null)
            resumeButton.onClick.AddListener(() => { Time.timeScale = 1f; pauseMenu.SetActive(false); });
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(() =>
            {
                string[] cannons = PlayerData.CannonInventory.Keys.Count != 0 ? PlayerData.CannonInventory.Keys.ToArray() : new string[] { "Base" };
                string[] projectiles = PlayerData.ProjectileInventory.Keys.Count != 0 ? PlayerData.ProjectileInventory.Keys.ToArray() : new string[] { "Base" };
                SaveManager.SaveGame(PlayerData.currentHealth, PlayerData.maxHealth, PlayerData.speedMult, PlayerData.baseDamage, PlayerData.score, cannons, projectiles, false, PlayerData.levelsComplete);
            });
        }
        if (quitButton != null)
            quitButton.onClick.AddListener(() => {
                SceneManager.LoadScene("MainMenu");
            });
    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score: {PlayerData.score}";
        }

        if(waveText.IsActive())
            waveText.text = $"Enemies Left: {currentEnemiesLeft} / {totalEnemiesToSpawn}";
    }
}
