using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [Range(0, 4)]
    public static int levelsComplete = 0;
    public static int maxHealth = 100, currentHealth = 80, score = 0;
    public static float speedMult = 1f;
    public static int baseDamage = 5;
    public static Dictionary<string, Cannon> CannonInventory = new();
    public static Dictionary<string, BasicProjectile> ProjectileInventory = new();
    static Cannon[] cannons;
    public static bool superState = false;
    public static int superTime = 10000; //time you stay in super state
    private static int currentSuperTime = 0;

    Animator animator;

    private void Awake()
    {
        //make sure reloading this resets super state always
        superState = false; 
        currentSuperTime = 0;
        cannons = GetComponentsInChildren<Cannon>();
        if (SaveManager.currentTypes.Count > 0) //first run
        {
            for (int i = 0; i < cannons.Length; i++)
            {
                Debug.Log("current types: " + SaveManager.currentTypes[i]);
                cannons[i].SetCannonType(SaveManager.currentTypes[i]);
            }
        }
        else
        {
            foreach(Cannon cannon in cannons)
            {
                SaveManager.currentTypes.Add(cannon.GetCannonTypeInt());
                CannonInventory.TryAdd("Base", cannon); //Base never gets added since its never bought
            }
        }

        if(SaveManager.currentProjectiles.Count > 0)
        {
            for (int i = 0; i < cannons.Length; i++)
            {
                cannons[i].projectile = SaveManager.currentProjectiles[i];
            }
        }
        else
        {
            foreach (Cannon cannon in cannons)
            {
                SaveManager.currentProjectiles.Add(cannon.projectile);
                ProjectileInventory.TryAdd("Base", cannon.projectile); //Base never gets added since its never bought
            }
        }
            animator = GetComponent<Animator>();
        
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

        if (superState)
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
            Debug.Log($"SUPER STATE ACTIVE: {cannons[0].maxProjectileCount}");
            if (currentSuperTime <= 0)
                ExitSuperState();
            currentSuperTime--;
        }

    }

    public static void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthUIManager.UpdateHealthUI(damage, false);
        if (currentHealth <= 0)
        { 

            SceneManager.LoadScene("GameOver");
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
            SaveManager.currentTypes[cannonToUpgrade] = CannonInventory[type].GetCannonTypeInt();
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
            SaveManager.currentProjectiles[cannonToUpgrade] = ProjectileInventory[type];
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Colliding with any ships is a guarentee 10 damage
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }

    public static void EnterSuperState(int projectileCount)
    {
        foreach(Cannon cannon in cannons)
        {
            cannon.maxProjectileCount = projectileCount;
        }
        superState = true;
        currentSuperTime = superTime;
    }

    public static void ExitSuperState()
    {
        foreach (Cannon cannon in cannons)
        {
            cannon.maxProjectileCount = 3; //default
        }

        superState = false;
    }
}
