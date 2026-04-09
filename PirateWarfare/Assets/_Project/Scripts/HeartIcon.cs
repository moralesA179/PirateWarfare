using UnityEngine;
using UnityEngine.UI;

public class HeartIcon : MonoBehaviour
{
    public Sprite[] states;
    private Image uiImage;
    private int heartHealth = 20;
    public void Start()
    {
        uiImage = GetComponent<Image>();
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
            uiImage.sprite = states[0]; //full heart
        }
        return 0;
    }

    public bool IsEmpty()
    {
        return uiImage.sprite == states[2];
    }
}
