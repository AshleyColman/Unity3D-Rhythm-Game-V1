using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSoundController : MonoBehaviour {

    public AudioSource audioSource; // The sound that plays when the button is pressed
    public AudioClip placedSound; // The soudn effect that plays when an object is placed
    public AudioClip specialTimeStartPlacedSound; // The sound effect that is played when the user presses the special time button for the first time
    public AudioClip specialTimeEndPlacedSound; // The sound effect that is played when the user presses the special time button for the first time
    private float songVolume = 1f;

    // Use this for initialization
    void Start()
    {
        audioSource.volume = songVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Play placed sound
    public void PlayPlacedSound()
    {
        audioSource.clip = placedSound;
        audioSource.Play();
    }

    // Play placed sound when the user has pressed the key for the first time
    public void PlaySpecialTimeStartPlacedSound()
    {
        audioSource.clip = specialTimeStartPlacedSound;
        audioSource.Play();
    }

    // Play placed sound when the user has pressed the key for the second time
    public void PlaySpecialTimeEndPlacedSound()
    {
        audioSource.clip = specialTimeEndPlacedSound;
        audioSource.Play();
    }

}
