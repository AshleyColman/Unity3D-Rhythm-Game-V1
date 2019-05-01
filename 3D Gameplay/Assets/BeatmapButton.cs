using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapButton : MonoBehaviour {

    private int beatmapButtonIndex;

    private SongSelectMenuFlash songSelectMenuFlash;

    private GameObject menuSFXGameObject;
    private AudioSource menuSFXAudioSource;

   // public AudioClip click;
   // public AudioClip onPointerEnter;

	// Use this for initialization
	void Start () {
        songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        //menuSFXGameObject= GameObject.FindWithTag("MenuSFXAudioSource");
        //menuSFXAudioSource = menuSFXGameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Load the beatmap assigned to the button when clicked
    public void LoadBeatmap()
    {
        songSelectMenuFlash.LoadBeatmapButtonSong(beatmapButtonIndex);
    }

    // Set the beatmap butotn index during instantiation
    public void SetBeatmapButtonIndex(int beatmapButtonIndexPass)
    {
        beatmapButtonIndex = beatmapButtonIndexPass;
    }

    /*
    // Play click sound when the button has been pressed
    public void PlayClickSound()
    {
        menuSFXAudioSource.clip = click;
        menuSFXAudioSource.Play();
    }

    // Play onPointerEnter sound for the button
    public void PlayOnPointerEnterSound()
    {
        menuSFXAudioSource.clip = onPointerEnter;
        menuSFXAudioSource.Play();
    }
    */
}
