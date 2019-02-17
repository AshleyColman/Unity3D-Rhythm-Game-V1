using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource clickSound; // The sound that plays when the button is pressed

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayHitSound()
    {
        clickSound.Play();
    }
}
