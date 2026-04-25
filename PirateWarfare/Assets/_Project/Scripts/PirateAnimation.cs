using UnityEngine;
using UnityEngine.UI;

public class PirateAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    private int counter = 0;
    private int timer = 0;
    public int maxTime = 50;
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        timer += 1;
        if(counter == sprites.Length)
        {
            counter = 0;
        }
        if(timer > maxTime)
        {
            image.sprite = sprites[counter++];
            timer = 0;
        }

    }
}
