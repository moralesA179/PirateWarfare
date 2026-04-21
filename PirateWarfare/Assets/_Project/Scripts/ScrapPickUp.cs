using UnityEngine;

public class ScrapPickUp : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerData.currentHealth < PlayerData.maxHealth)
            {
                int oldHealth = PlayerData.currentHealth;

                PlayerData.currentHealth += healAmount;

                if (PlayerData.currentHealth > PlayerData.maxHealth)
                    PlayerData.currentHealth = PlayerData.maxHealth;

                int actualHeal = PlayerData.currentHealth - oldHealth;

                if (actualHeal > 0)
                {
                    HealthUIManager.UpdateHealthUI(actualHeal, true);
                    Destroy(gameObject);
                }
            }
        }
    }
}
