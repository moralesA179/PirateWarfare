using UnityEngine;
using UnityEngine.UI;

public class PirateAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    private int counter = 0;
    private int timer = 0;
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        timer += 1;
        if(counter == sprites.Length)
        {
            counter = 0;
        }
        if(timer > 30)
        {
            image.sprite = sprites[counter++];
            timer = 0;
        }

    }
}
