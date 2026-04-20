using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SaveManager : MonoBehaviour
{

    public static List<int> currentTypes = new();
    public static List<BasicProjectile> currentProjectiles = new();

    public Cannon[] availableCannons;
    public BasicProjectile[] availableProjectiles;

    //private void Start()
    //{
    //    //loading current cannons (integers signify type of cannon to be loaded)
    //    currentTypes.Add(PlayerPrefs.GetInt("leftCannon", 0));
    //    currentTypes.Add(PlayerPrefs.GetInt("rightCannon", 0));

    //    //current projectiles as strings
    //    string leftProj = PlayerPrefs.GetString("leftProjectile", "Base");
    //    string rightProj = PlayerPrefs.GetString("rightProjectile", "Base");
    //    switch(leftProj)
    //    {
    //        case "Base":
    //            currentProjectiles.Add(availableProjectiles[0]);
    //            break;
    //        case "Richochet":
    //            currentProjectiles.Add(availableProjectiles[1]);
    //            break;
    //        case "Homing":
    //            currentProjectiles.Add(availableProjectiles[2]);
    //            break;
    //    }

    //    switch (rightProj)
    //    {
    //        case "Base":
    //            currentProjectiles.Add(availableProjectiles[0]);
    //            break;
    //        case "Richochet":
    //            currentProjectiles.Add(availableProjectiles[1]);
    //            break;
    //        case "Homing":
    //            currentProjectiles.Add(availableProjectiles[2]);
    //            break;
    //    }
    //}
    

    public static void ResetGame()
    {
        //Default vals
        SaveGame(100, 100, 1, 5, 0, new string[] { "Base" }, new string[] {"Base"}, true);

        //resetting shop
        ShopManager.healthCost = 50;
        ShopManager.speedCost = 50;
        ShopManager.damageCost = 50;
        ShopManager.healthLevelCounter = 1;
        ShopManager.speedLevelCounter = 1;
        ShopManager.damageLevelCounter = 1;

        

    }

    public static void SaveGame(int currentHealth, int maxHealth, float speed, int damage, int score, string[] cannons, string[] projectiles, bool reset=false)
    {

        //Basic Stats
        PlayerPrefs.SetInt("cHealth", currentHealth);
        PlayerPrefs.SetInt("mHealth", maxHealth);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetInt("damage", damage);
        PlayerPrefs.SetInt("score", score);

        //Inventory
        int counter = 0;

        //Reset logic
        if(reset)
        {
            for(int i = 0; i < 3; i++)
            {
                if(PlayerPrefs.HasKey($"cannon{i}"))
                {
                    PlayerPrefs.DeleteKey($"cannon{i}");
                }
            }
        }
        foreach (string cannon in cannons)
        {
            PlayerPrefs.SetString($"cannon{counter++}", cannon);
        }
        if (reset)
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.HasKey($"projectile{i}"))
                {
                    PlayerPrefs.DeleteKey($"projectile{i}");
                }
            }
        }


        counter = 0;
        foreach (string projectile in projectiles)
        {
            PlayerPrefs.SetString($"projectile{counter++}", projectile);
        }

        //Current loadout
        PlayerPrefs.SetInt("leftCannon", reset ? 0 : currentTypes[0]);
        PlayerPrefs.SetInt("rightCannon", reset ? 0 : currentTypes[1]);
        PlayerPrefs.SetString("leftProjectile", reset ? "Base" : currentProjectiles[0].GetProjectileTypeStr());
        PlayerPrefs.SetString("rightProjectile", reset ? "Base" : currentProjectiles[1].GetProjectileTypeStr());


        //Shop things
        PlayerPrefs.SetInt("healthLevel", ShopManager.healthLevelCounter);
        PlayerPrefs.SetInt("speedLevel", ShopManager.speedLevelCounter);
        PlayerPrefs.SetInt("damageLevel", ShopManager.damageLevelCounter);
        PlayerPrefs.SetInt("healthCost", ShopManager.healthCost);
        PlayerPrefs.SetInt("speedCost", ShopManager.speedCost);
        PlayerPrefs.SetInt("damageCost", ShopManager.damageCost);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        //Basic Stats
        PlayerData.currentHealth = PlayerPrefs.GetInt("cHealth", 100);
        PlayerData.maxHealth = PlayerPrefs.GetInt("mHealth", 100);
        PlayerData.speedMult = PlayerPrefs.GetFloat("speed", 1f);
        PlayerData.baseDamage = PlayerPrefs.GetInt("damage", 5);
        PlayerData.score = PlayerPrefs.GetInt("score", 0);

        ShopManager.healthLevelCounter = PlayerPrefs.GetInt("healthLevel", 1);

        //Inventory
        for(int i = 0; i < 3; i++) //3 b.c there only 3 cannons in the game...
        {
            if(PlayerPrefs.HasKey($"cannon{i}")) //if this was present
            {
                PlayerData.CannonInventory.TryAdd(PlayerPrefs.GetString($"cannon{i}"), availableCannons[GetRightCannon(PlayerPrefs.GetString($"cannon{i}"))]);
            }
        }

        for(int i = 0; i < 3; i++)
        {
            if(PlayerPrefs.HasKey($"projectile{i}"))
            {
                PlayerData.ProjectileInventory.TryAdd(PlayerPrefs.GetString($"projectile{i}"), availableProjectiles[GetRightProjectile(PlayerPrefs.GetString($"projectile{i}"))]);
            }
        }

        //current loadout
        //loading current cannons (integers signify type of cannon to be loaded)
        currentTypes.Add(PlayerPrefs.GetInt("leftCannon", 0));
        currentTypes.Add(PlayerPrefs.GetInt("rightCannon", 0));

        //current projectiles as strings
        string leftProj = PlayerPrefs.GetString("leftProjectile", "Base");
        string rightProj = PlayerPrefs.GetString("rightProjectile", "Base");
        switch (leftProj)
        {
            case "Base":
                currentProjectiles.Add(availableProjectiles[0]);
                break;
            case "Richochet":
                currentProjectiles.Add(availableProjectiles[1]);
                break;
            case "Homing":
                currentProjectiles.Add(availableProjectiles[2]);
                break;
        }

        switch (rightProj)
        {
            case "Base":
                currentProjectiles.Add(availableProjectiles[0]);
                break;
            case "Richochet":
                currentProjectiles.Add(availableProjectiles[1]);
                break;
            case "Homing":
                currentProjectiles.Add(availableProjectiles[2]);
                break;
        }

        //Shop Loading
        ShopManager.healthLevelCounter = PlayerPrefs.GetInt("healthLevel", 1);
        ShopManager.speedLevelCounter = PlayerPrefs.GetInt("speedLevel", 1);
        ShopManager.damageLevelCounter = PlayerPrefs.GetInt("damageLevel", 1);
        ShopManager.healthCost = PlayerPrefs.GetInt("healthCost", 50);
        ShopManager.speedCost = PlayerPrefs.GetInt("speedCost", 50);
        ShopManager.damageCost = PlayerPrefs.GetInt("damageCost", 50);
    }

    int GetRightCannon(string type)
    {
        for(int i = 0; i < availableCannons.Length; i++)
        {
            if (Cannon.GetCannonTypeStr(availableCannons[i].GetCannonTypeInt()) == type)
            {
                return i;
            }

        }
        return -1; //no need to worry about index out of bounds since this should find something
    }

    int GetRightProjectile(string type)
    {
        for (int i = 0; i < availableProjectiles.Length; i++)
        {
            if (availableProjectiles[i].GetProjectileTypeStr() == type)
            {
                return i;
            }

        }
        return -1; //no need to worry about index out of bounds since this should find something
    }

     
}
