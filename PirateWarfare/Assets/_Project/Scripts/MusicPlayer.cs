 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

public class MusicPlayer : MonoBehaviour                                     

{
    public AudioClip[] MusicSections;
    // Music files which share the identifier 'Section' go in here. Example track: 'Adventure Inn Section 2.wav'.
    // In the editor, changing the 'Size' of the array increases/decreases the available slots. This is useful for removing unwanted music sections.
    public bool playIntro = true;
    // Disabling this in the Unity Inspector will skip Element 1 in MusicSections Array (by default this should be reserved for files with the 'Intro' identifier. Example track: 'Adventure Inn Section 1 Intro.wav').

    private AudioSource audioSource;
    // The audiosource is responsible for playing all of our 'MusicSections'.
    private int lastPlayed;
    // This keeps a log of the last played music section. Leave this alone unless you know what you are doing!
    private bool preloadBufferActive = true;
    // Necessary Preload buffer, leave this alone unless you know what you are doing!.
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
  

