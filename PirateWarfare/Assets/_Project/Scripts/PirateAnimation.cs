using UnityEngine;
using UnityEngine.UI;

public class PirateAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    private int counter = 0;
    private int timer = 0;
    [Tooltip("How many frames must pass before switching to next sprite.")]
    public int maxTime = 50;
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
        if(timer > maxTime)
        {
            image.sprite = sprites[counter++];
            timer = 0;
        }

    }
}
