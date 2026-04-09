using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public enum HealthState 
    { 
        Full = 0, 
        Half = 1, 
        Low = 2
    }

    public static int maxHealth = 100, currentHealth = maxHealth, score = 0;
    public static float speedMult = 1f;
    //public static ItemList[] <-- Future(We need to keep track of current items player has)

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

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
        HealthUIManager.UpdateHealthUI(damage);
    }

}
