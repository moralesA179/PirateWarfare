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
    //public static ItemList[] <-- Future(We need to keep track of current items player has)

    Animator animator;

    private void Awake()
    {
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

}
