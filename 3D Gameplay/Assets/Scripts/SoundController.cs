using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource audioSource; // The sound that plays when the button is pressed
    public AudioClip clickSound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayHitSound()
    {
        audioSource.Play();
    }
}
