using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HeartIcon : MonoBehaviour
{
    public Sprite[] states;
    private Image uiImage;
    private int heartHealth = 20;
    public void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    public void Update()
    {
        //Debug.Log($"Heart HP: {heartHealth}");
    }

    public int UpdateHeart(int damage)
    {
        heartHealth -= damage;
        if(heartHealth <= 0)
        {
            uiImage.sprite = states[2]; //empty heart
            return heartHealth; //returning remainder for bugs (negative health)
        } else if(heartHealth  <= 10)
        {
            uiImage.sprite = states[1]; //half heart
        } else //greater than 10 HP
        {
            Debug.Log("half heart");
            uiImage.sprite = states[0]; //full heart
            if(heartHealth > 20)
            {
                int excess = heartHealth - 20;
                heartHealth = 20;
                return excess; //excess healing
            }
        }
        return 0;
    }

    public bool IsEmpty()
    {
        Debug.Log($"In the heart code: {uiImage == null}");
        return uiImage.sprite == states[2];
    }
    public bool IsFull()
    {
        return heartHealth == 20; //important to do it this way since just checking the sprite isn't helpful given a full heart is displayed at any HP above 10
    }
}
