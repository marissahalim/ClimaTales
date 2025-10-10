using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataButtonsAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioSource[] audioSources;
    private AudioSource whistleUp;
    private AudioSource whistleDown;
    private AudioSource pop;
    private AudioSource done;

    void Start()
    {
        audioSources = audioSource.gameObject.GetComponents<AudioSource>();
        pop = audioSources[0];
        done = audioSources[1];
    }
    public void playWhistleUp()
    {
        whistleUp.Play();
    }

    public void playWhistleDown()
    {
        whistleDown.Play();
    }

    public void playPop()
    {
        pop.Play();
    }
    
    public void playDone()
    {
        done.Play();
    }
}
