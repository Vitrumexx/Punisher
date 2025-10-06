using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepSound : MonoBehaviour
{
    public AudioClip[] stepSounds;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    public void playStepSound()
    {
        audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
    }
}
