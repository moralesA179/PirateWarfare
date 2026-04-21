using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //All the fields we need to keep track of and update in the shop...
    public Button levelUpHealth, levelUpSpeed, levelUpDamage, equipLeft, equipRight, buyCannon, buyBall, closeShop;
    public TMP_Dropdown cannonTypes, projectileTypes;
    public TMP_Text healthLvText, speedLv, damageLv, statInfo, notEnoughScore, buyCannonText, buyProjectileText;
    //Aesthetics, don't actually get stored in player data...
    public static int healthLevelCounter = 1, speedLevelCounter = 1, damageLevelCounter = 1;
    public HealthUIManager health;
    public Cannon[] cannonPrefabs;
    public BasicProjectile[] projectilePrefabs;
    public static int healthCost = 50, speedCost = 50, damageCost = 50;

    public int[] cannonCosts = { 0, 100, 150};
    public int[] projectileCosts = { 0, 100, 300000000};
    private bool timerActive = false;
    private int timer = 0;
    public int max_time = 200;

    private void Start()
    {
        //Level Health Up By 1
        levelUpHealth.onClick.AddListener(() =>
        {
            if(EnoughScore(healthCost))
            {
                PlayerData.maxHealth += 20;
                PlayerData.Heal(PlayerData.maxHealth);
                PlayerPrefs.SetInt("cHealth", PlayerData.maxHealth);
                health.LevelUpHealth();
                healthLevelCounter++;
                PlayerData.score -= healthCost;
                healthCost *= 2;
            }
            else
            {
                timerActive = true;
            }
        });

        //Level Speed Up By 1
        levelUpSpeed.onClick.AddListener(() =>
        {
            if (EnoughScore(speedCost))
            {
                PlayerData.speedMult += 0.5f;
                speedLevelCounter++;
                PlayerData.score -= speedCost;
                speedCost *= 2;
            } else
            {
                timerActive = true;
            }
        });

        //Level Damage Up By 1
        levelUpDamage.onClick.AddListener(() =>
        {
            if(EnoughScore(damageCost))
            {
                PlayerData.baseDamage += 5;
                damageLevelCounter++;
                PlayerData.score -= damageCost;
                damageCost *= 2;
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
            if (EnoughScore(cannonCosts[cannonTypes.value]))
            {
                if(!PlayerData.CannonInventory.ContainsKey(cannonTypes.options[cannonTypes.value].text)) //if you dont already own the cannon
                {
                    PlayerData.CannonInventory.Add(cannonTypes.options[cannonTypes.value].text, cannonPrefabs[cannonTypes.value]); //buying it
                    PlayerData.score -= cannonCosts[cannonTypes.value];
                    //Settting the equip buttons back on b/c they were disabled due to you not owning this
                    if (PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if the projectile currently selected is also owned
                    {
                        equipLeft.interactable = true;
                        equipRight.interactable = true;
                    }
                    buyCannonText.text = "OWNED";
                } //else do nothing (you already have it, no need to do anything)
            }
            else
                timerActive = true;
        });

        buyBall.onClick.AddListener(() =>
        {
            if (EnoughScore(projectileCosts[projectileTypes.value]))
            {
                if (!PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if you dont already own the cannon
                {
                    PlayerData.ProjectileInventory.Add(projectileTypes.options[projectileTypes.value].text, projectilePrefabs[projectileTypes.value]); //buying it
                    PlayerData.score -= projectileCosts[projectileTypes.value];
                    //Settting the equip buttons back on b/c they were disabled due to you not owning this
                    if (PlayerData.CannonInventory.ContainsKey(cannonTypes.options[cannonTypes.value].text)) //if the cannon currently selected is also owned
                    {
                        equipLeft.interactable = true;
                        equipRight.interactable = true;
                    }
                    buyProjectileText.text = "OWNED";
                } //else do nothing (you already have it, no need to do anything)
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
            PlayerData.UpdateProjectile(projectileTypes.options[projectileTypes.value].text, 1);
        });

        cannonTypes.onValueChanged.AddListener((i) => { 
            if(!PlayerData.CannonInventory.ContainsKey(cannonTypes.options[i].text) && cannonTypes.options[i].text != "Base") //Base is never added b/c its never bought...
            {
                Debug.Log("Setting buttons to off");
                equipLeft.interactable = false;
                equipRight.interactable = false;
                buyCannonText.text = $"BUY ({cannonCosts[i]})";
            }
            else
            {
                buyCannonText.text = "OWNED";
                if (PlayerData.ProjectileInventory.ContainsKey(projectileTypes.options[projectileTypes.value].text)) //if the projectile currently selected is also owned
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
                buyProjectileText.text = $"BUY ({projectileCosts[i]})";
            }
            else
            {
                buyProjectileText.text = "OWNED";
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
        statInfo.text = $"Health: {PlayerData.maxHealth} HP\nSpeed: {PlayerData.speedMult} m/s\nBase Damage: {PlayerData.baseDamage}\nSCORE: {PlayerData.score}\n" +
            $"LEFT CANNON: {Cannon.GetCannonTypeStr(SaveManager.currentTypes[0])} with ammo: {SaveManager.currentProjectiles[0].GetProjectileTypeStr()}\n" +
            $"RIGHT CANNON: {Cannon.GetCannonTypeStr(SaveManager.currentTypes[1])} with ammo: {SaveManager.currentProjectiles[1].GetProjectileTypeStr()}";
        healthLvText.text = $"Lv: {healthLevelCounter} \nCost: {healthCost}";
        speedLv.text = $"Lv: {speedLevelCounter} \nCost: {speedCost}";
        damageLv.text = $"Lv: {damageLevelCounter} \nCost: {damageCost}";
        //Debug.Log(cannonTypes.options[0].text);
        notEnoughScore.enabled = timerActive;

    }

    bool EnoughScore(int price)
    {
        return PlayerData.score >= price; 
    }
}
