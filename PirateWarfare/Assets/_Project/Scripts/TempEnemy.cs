using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum HealthState
    {
        Full = 0,
        Half = 1,
        Low = 2
    }

    int maxHealth = 100;
    int currentHealth = 100;
    Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
}
