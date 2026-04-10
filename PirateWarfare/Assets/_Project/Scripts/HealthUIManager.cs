using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Image heart;
    private static HeartIcon[] hearts;
    void Start()
    {
        hearts = GetComponentsInChildren<HeartIcon>();
        if(PlayerPrefs.GetInt("health") < PlayerData.maxHealth)
        {
            UpdateHealthUI(PlayerData.maxHealth - PlayerPrefs.GetInt("health"), false);
        }
    }


    public static void UpdateHealthUI(int healthDiff, bool healing)
    {
        if (!healing)
        {
            for (int i = hearts.Length - 1; i >= 0; i--) //for each heart (working backwards right -> left)
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
            for (int i = 0; i < hearts.Length; i++)
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
