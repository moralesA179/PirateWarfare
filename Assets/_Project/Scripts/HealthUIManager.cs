using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public HeartIcon heart;

    private static List<HeartIcon> hearts = new List<HeartIcon>();
    void Start()
    {
        GetComponentsInChildren<HeartIcon>(hearts);
        if(PlayerData.maxHealth > 100)
        {
            for (int i = 0; i < ShopManager.healthLevelCounter - 1; i++)
            {
                hearts.Add(Instantiate(heart, transform));
            }
        }
        if(PlayerData.currentHealth < PlayerData.maxHealth)
        {
            Debug.Log($"{PlayerData.currentHealth} vs {PlayerPrefs.GetInt("health")}");
            UpdateHealthUI(PlayerData.maxHealth - PlayerData.currentHealth, false);
        }
    }

    public void LevelUpHealth()
    {
        hearts.Add(Instantiate(heart, transform));
    }


    public static void UpdateHealthUI(int healthDiff, bool healing)
    {
        if (!healing)
        {
            Debug.Log("health diff: " + healthDiff);
            Debug.Log("COUNT: " + hearts.Count);
            Debug.Log("IS NULL: " + hearts[hearts.Count - 1] == null);
            for (int i = hearts.Count - 1; i >= 0; i--) //for each heart (working backwards right -> left)
            {
                if (healthDiff <= 0)
                    break;
                if (hearts[i].IsEmpty()) //skip to next heart if this is already empty
                    continue;
                int heartDamage = healthDiff >= 20 ? 20 : healthDiff;
                int rem = hearts[i].UpdateHeart(heartDamage);
                if (rem < 0)
                    healthDiff += -rem;  //if it was overkill (negative health remaining for this heart), add this back into the health difference pool 
                healthDiff -= 20;
            }
        }
        else
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                //Debug.Log(healthDiff);
                if (healthDiff <= 0)
                    break;
                if (hearts[i].IsFull())
                    continue;
                int heal = healthDiff >= 20 ? 20 : healthDiff;
                int excess = hearts[i].UpdateHeart(-heal);
                //Debug.Log(excess);
                healthDiff -= 20 - excess;
            }
        }
    }
}
