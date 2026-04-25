using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

public class MusicPlayer : MonoBehaviour

{
    public AudioClip[] MusicSections;
    public bool playIntro = true;
    private AudioSource audioSource;
    private int lastPlayed;
    private bool preloadBufferActive = true;
    public AudioSource sailSource;   // looping movement sound


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sailSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        audioSource.clip = MusicSections[0];
        audioSource.Play();

    }

    public void Damage()
    {
        audioSource.PlayOneShot(MusicSections[1]);
    }

    public void SailStart()
    {
        if (!sailSource.isPlaying)
        {
            sailSource.clip = MusicSections[2];
            sailSource.loop = true;
            sailSource.Play();
        }
    }

    public void SailStop()
    {
        if (sailSource.isPlaying)
        {
            sailSource.Stop();
        }
    }

    public void Shop()
    {
        audioSource.clip = MusicSections[3];
    }

}

