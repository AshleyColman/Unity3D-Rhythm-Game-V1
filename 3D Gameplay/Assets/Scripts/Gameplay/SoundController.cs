using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource audioSource; // The sound that plays when the button is pressed
    public AudioClip clickSound;
    public AudioClip missSound;
    private float songVolume = 0.3f;
    // Use this for initialization
    void Start () {
        audioSource.volume = songVolume;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Play hit sound
    public void PlayHitSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }

    // Play miss sound
    public void PlayMissSound()
    {
        audioSource.clip = missSound;
        audioSource.Play();
    }
}
