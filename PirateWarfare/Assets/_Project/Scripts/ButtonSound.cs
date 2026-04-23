using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;

    public void PlayClick()
    {
        audioSource.Play();
    }
}

