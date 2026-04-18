using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public enum HealthState 
    { 
        Full = 0, 
        Half = 1, 
        Low = 2
    }

    public static int maxHealth = 100, currentHealth, score = 0;
    public static float speedMult = 1f;
    public static int baseDamage = 5;
    //public static ItemList[] <-- Future(We need to keep track of current items player has)
    public static Dictionary<string, Cannon> CannonInventory = new();
    public static Dictionary<string, BasicProjectile> ProjectileInventory = new();
    static Cannon[] cannons;


    Animator animator;

    private void Awake()
    {
        cannons = GetComponentsInChildren<Cannon>();
        if (TempShopTester.currentTypes.Count > 0) //first run
        {
            for (int i = 0; i < cannons.Length; i++)
            {
                cannons[i].SetCannonType(TempShopTester.currentTypes[i]);
            }
        }
        else
        {
            foreach(Cannon cannon in cannons)
            {
                TempShopTester.currentTypes.Add(cannon.GetCannonTypeInt());
                CannonInventory.TryAdd("Base", cannon); //Base never gets added since its never bought
            }
        }

        if(TempShopTester.currentProjectiles.Count > 0)
        {
            for (int i = 0; i < cannons.Length; i++)
            {
                cannons[i].projectile = TempShopTester.currentProjectiles[i];
            }
        }
        else
        {
            foreach (Cannon cannon in cannons)
            {
                TempShopTester.currentProjectiles.Add(cannon.projectile);
                ProjectileInventory.TryAdd("Base", cannon.projectile); //Base never gets added since its never bought
            }
        }
            animator = GetComponent<Animator>();
        currentHealth = PlayerPrefs.GetInt("health", maxHealth); //when loading into the game either start with previous health or the maximum starting health (100)
    }

    private void Update()
    {
        // Modifies the ships sprite based on their health
       if (currentHealth <= (maxHealth * 0.25))
        {
            animator.SetInteger("HealthState", (int)HealthState.Low);
        }
       else if (currentHealth <= (maxHealth * 0.5))
        {
            animator.SetInteger("HealthState", (int)HealthState.Half);
        }
        else if (currentHealth == maxHealth)
        {
            animator.SetInteger("HealthState", (int)HealthState.Full);
        }

        Debug.Log(currentHealth);

    }

    public static void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthUIManager.UpdateHealthUI(damage, false);
        if (currentHealth == 0)
        { 

            SceneManager.LoadScene("GameOver");
            //Debug.Log("This ran");
        }
    }

    public static void Heal(int healAmount)
    {
        healAmount = Mathf.Clamp(healAmount, 0, maxHealth);
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthUIManager.UpdateHealthUI(healAmount, true);
    }

    public static bool UpdateCannon(string type, int cannonToUpgrade)
    {
        if(CannonInventory.ContainsKey(type))
        {
            cannons[cannonToUpgrade].type = CannonInventory[type].type;
            Debug.Log($"HELLO WORLD: {CannonInventory[type].type}; is-present: {CannonInventory.ContainsKey(type)}");
            TempShopTester.currentTypes[cannonToUpgrade] = CannonInventory[type].GetCannonTypeInt();
            return true;
        } else
        {  
            return false;
        }
    }

    public static bool UpdateProjectile(string type, int cannonToUpgrade)
    {
        if (ProjectileInventory.ContainsKey(type))
        {
            cannons[cannonToUpgrade].projectile = ProjectileInventory[type];
            TempShopTester.currentProjectiles[cannonToUpgrade] = ProjectileInventory[type];
            return true;
        }
        return false;
    }
}
