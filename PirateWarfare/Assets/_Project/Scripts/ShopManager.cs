using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //All the fields we need to keep track of and update in the shop...
    public Button levelUpHealth, levelUpSpeed, levelUpDamage, equipLeft, equipRight, buyCannon, buyBall, closeShop;
    public TMP_Dropdown cannonTypes, projectileTypes;
    public TMP_Text healthLvText, speedLv, damageLv, statInfo, notEnoughScore;
    //Aesthetics, don't actually get stored in player data...
    public static int healthLevelCounter = 1, speedLevelCounter = 1, damageLevelCounter = 1;
    public HealthUIManager health;
    public Cannon[] cannonPrefabs;
    public BasicProjectile[] projectilePrefabs;

    private bool timerActive = false;
    private int timer = 0;
    public int max_time = 200;

    private void Start()
    {
        //Level Health Up By 1
        levelUpHealth.onClick.AddListener(() =>
        {
            if(EnoughScore(50))
            {
                PlayerData.maxHealth += 20;
                PlayerData.Heal(PlayerData.maxHealth);
                PlayerPrefs.SetInt("health", PlayerData.maxHealth);
                health.LevelUpHealth();
                healthLevelCounter++;
                PlayerData.score -= 50;
            }
            else
            {
                timerActive = true;
            }
        });

        //Level Speed Up By 1
        levelUpSpeed.onClick.AddListener(() =>
        {
            if (EnoughScore(50))
            {
                PlayerData.speedMult += 0.5f;
                speedLevelCounter++;
                PlayerData.score -= 50;
            } else
            {
                timerActive = true;
            }
        });

        //Level Damage Up By 1
        levelUpDamage.onClick.AddListener(() =>
        {
            if(EnoughScore(50))
            {
                PlayerData.baseDamage += 5;
                damageLevelCounter++;
                PlayerData.score -= 50;
            }
            else
            {
                timerActive = true;
            }
        });

        closeShop.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("World Map");
        });

        buyCannon.onClick.AddListener(() => 
        {
            if (EnoughScore(100))
            {
                if(!PlayerData.CannonInventory.ContainsKey(cannonTypes.options[cannonTypes.value].text)) //if you dont already own the cannon
                {
                    PlayerData.CannonInventory.Add(cannonTypes.options[cannonTypes.value].text, cannonPrefabs[cannonTypes.value]); //buying it
                    PlayerData.score -= 100;
                    //Settting the equip buttons back on b/c they were disabled due to you not owning this
                    if (PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if the projectile currently selected is also owned
                    {
                        equipLeft.interactable = true;
                        equipRight.interactable = true;
                    }
                } //else do nothing (you already have it, no need to do anything)
            }
            else
                timerActive = true;
        });

        buyBall.onClick.AddListener(() =>
        {
            if (EnoughScore(100))
            {
                if (!PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if you dont already own the cannon
                {
                    PlayerData.ProjectileInventory.Add(projectileTypes.options[projectileTypes.value].text, projectilePrefabs[projectileTypes.value]); //buying it
                    PlayerData.score -= 100;
                    //Settting the equip buttons back on b/c they were disabled due to you not owning this
                    if (PlayerData.CannonInventory.ContainsKey(cannonTypes.options[cannonTypes.value].text)) //if the cannon currently selected is also owned
                    {
                        equipLeft.interactable = true;
                        equipRight.interactable = true;
                    }
                } //else do nothing (you already have it, no need to do anythin
            }
            else
                timerActive = true;
        });

        equipLeft.onClick.AddListener(() => {
            Debug.Log("This ran");
            PlayerData.UpdateCannon(cannonTypes.options[cannonTypes.value].text, 0);
            PlayerData.UpdateProjectile(projectileTypes.options[projectileTypes.value].text, 0);
        });

        equipRight.onClick.AddListener(() => {
            PlayerData.UpdateCannon(cannonTypes.options[cannonTypes.value].text, 1);
            PlayerData.UpdateProjectile(projectileTypes.options[projectileTypes.value].text, 0);
        });

        cannonTypes.onValueChanged.AddListener((i) => { 
            if(!PlayerData.CannonInventory.ContainsKey(cannonTypes.options[i].text) && cannonTypes.options[i].text != "Base") //Base is never added b/c its never bought...
            {
                Debug.Log("Setting buttons to off");
                equipLeft.interactable = false;
                equipRight.interactable = false;
            }
            else
            {
                if(PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if the projectile currently selected is also owned
                {
                    equipLeft.interactable = true;
                    equipRight.interactable = true;
                }
            }
        });

        projectileTypes.onValueChanged.AddListener((i) => {
            if (!PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[i].text) && projectileTypes.options[i].text != "Base") //Base is never added b/c its never bought...
            {
                equipLeft.interactable = false;
                equipRight.interactable = false;
            }
            else
            {
                if (PlayerData.CannonInventory.ContainsKey(cannonTypes.options[cannonTypes.value].text)) //if the cannon currently selected is also owned
                {
                    equipLeft.interactable = true;
                    equipRight.interactable = true;
                }
            }
        });


    }

    private void Update()
    {
        if (timerActive)
        {
            if(timer >= max_time)
            {
                timer = 0;
                timerActive = false;
            }
            else
            {
                timer++;
            }
        }
        statInfo.text = $"Health: {PlayerData.maxHealth} HP\nSpeed: {PlayerData.speedMult} m/s\nBase Damage: {PlayerData.baseDamage}\nSCORE: {PlayerData.score}";
        healthLvText.text = $"Lv: {healthLevelCounter}";
        speedLv.text = $"Lv: {speedLevelCounter}";
        damageLv.text = $"Lv: {damageLevelCounter}";
        //Debug.Log(cannonTypes.options[0].text);
        notEnoughScore.enabled = timerActive;

    }

    bool EnoughScore(int price)
    {
        return PlayerData.score >= price; 
    }
}
