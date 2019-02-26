// Created by Carlos Arturo Rodriguez Silva (Legend)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MetronomePro_Player : MonoBehaviour {

	[Header ("Variables")]
	private bool active = true;
	private bool playing = true;

	[Space(5)]

	public AudioSource songAudioSource;
	public Sprite playSprite;
	public Sprite pauseSprite;

	[Space (5)]
	public Text txtSongName;
	public Text actualPosition;
	public Text songTotalDuration;
	public Image playAndPauseButton;
	public Image songPlayerBar;
	public Dropdown velocityScale;
	public Slider songPlayerSlider;

	[Header ("Song Data")]
	public AudioClip songClip;

	public string songName;
	public string songArtist;

	[Space(5)]

	public double Bpm = 128;
	public double OffsetMS = 100;

	public int Step = 4;
	public int Base = 4;

	float amount;

	void Start () {

		// Assign the clip to the AudioSource
		songAudioSource.clip = songClip;

		// Display Song Data
		DisplaySongDuration ();

		// Send Song Data to Metronome
		SendSongData();

        songAudioSource.Play();
    }

	// Sends Song Data to Metronome Pro script
	public void SendSongData () {
		FindObjectOfType<MetronomePro> ().GetSongData (Bpm, OffsetMS, Base, Step);
	}

	// Set New Song Name and Artist
	public void SetSongName (string SongName, string SongArtist) {
		txtSongName.text = SongName + " - " + SongArtist;
	}

	// Sets a New Song and Metronome Velocity using Velocity Scale Dropdown Value
	public void SetNewVelocity () {
		if (velocityScale.value == 4) {
			songAudioSource.pitch = 1;
		} else if (velocityScale.value == 5) {
			songAudioSource.pitch = 0.75f;
		} else if (velocityScale.value == 6) {
			songAudioSource.pitch = 0.50f;
		} else if (velocityScale.value == 7) {
			songAudioSource.pitch = 0.25f;
		} else if (velocityScale.value == 3) {
			songAudioSource.pitch = 1.25f;
		} else if (velocityScale.value == 2) {
			songAudioSource.pitch = 1.50f;
		} else if (velocityScale.value == 1) {
			songAudioSource.pitch = 1.75f;
		} else if (velocityScale.value == 0) {
			songAudioSource.pitch = 2.00f;
		} else {
			songAudioSource.pitch = 1;
		}
	}

	// Calculate Song Total Duration
	public void DisplaySongDuration () {
		try {
		songTotalDuration.text = UtilityMethods.FromSecondsToMinutesAndSeconds (songAudioSource.clip.length);
		} catch {
			Debug.LogWarning ("Please assign an Audio Clip to the Player!");
		}
	}


	// Play or Pause the Song and Metronome
	public void PlayOrPauseSong() {
        songAudioSource.Play ();
		FindObjectOfType<MetronomePro> ().Play ();
		Debug.Log ("Song Playing");
		playing = true;
		active = true;
	}

    /*
	// Stop Song and Metronome, Resets all too.
	public void StopSong () {
		Debug.Log ("Song Stoped");
		StopAllCoroutines ();
		active = false;
		playing = false;

		songAudioSource.Stop ();
		songAudioSource.time = 0;
		amount = 0f;
		songPlayerSlider.value = 0f;
		songPlayerBar.fillAmount = 0f;
		actualPosition.text = "00:00";

		FindObjectOfType<MetronomePro> ().Stop ();
	}
    */

	// Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
	void Update () {
		if (active) {
            Debug.Log("is active");
			if (playing) {
                Debug.Log("is playing");
                if (songAudioSource.isPlaying) {
                    Debug.Log("song running");
                    amount = (songAudioSource.time) / (songAudioSource.clip.length);
					songPlayerBar.fillAmount = amount;
					actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds (songAudioSource.time);
				} else {
					//StopSong ();
				}
			}
		}
	}
}
	
public static class UtilityMethods {
	public static string FromSecondsToMinutesAndSeconds (float seconds) {
		int sec = (int)(seconds % 60f); 
		int min = (int)((seconds / 60f) % 60f);

		string minSec = min.ToString ("D2") + ":" + sec.ToString ("D2");
		return minSec;
	}
}
