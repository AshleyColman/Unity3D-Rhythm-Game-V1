// Created by Carlos Arturo Rodriguez Silva (Legend)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class MetronomePro_Player : MonoBehaviour {

	[Header ("Variables")]
	public bool active;
	bool playing = false;

	[Space(5)]

	public AudioSource songAudioSource;
	public Sprite playSprite;
	public Sprite pauseSprite;

	[Space (5)]
	public TextMeshProUGUI actualPosition;
	public Image playAndPauseButton;
	public Image songPlayerBar;
	public TMP_Dropdown velocityScale;
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

    // Reference for the songDatabase for selecting and getting the song selected
    private SongDatabase songDatabase;

    // The song selected
    private int songSelectedIndex = 0;


    // HIT OBJECT TESTING
    public Vector3 timelineObjectPosition;

    public GameObject[] instantiatedTimelineObject = new GameObject[6];
    private int instantiatedTimelineObjectType;
    public GameObject songPointSliderHandle;

    public Vector3 handlePosition;
    public Slider handleSlider;

    float handlePositionX;
    float handlePositionY = 9999;
    float handlePositionZ;
    float x;

    void Start () {

        // Find the reference to the songDatabase
        songDatabase = FindObjectOfType<SongDatabase>();

		// Stop any song and reset values
		StopSong ();

		// Send Song Data to Metronome
		SendSongData();
	}

    // Gets the song selected from the song button list
    public void GetSongSelected(int songSelectedIndexPass)
    {
        songSelectedIndex = songSelectedIndexPass;
        // Assign the clip to the AudioSource
        songClip = songDatabase.songClip[songSelectedIndex];
        songAudioSource.clip = songClip;
    }

	// Sends Song Data to Metronome Pro script
	public void SendSongData () {
		FindObjectOfType<MetronomePro> ().GetSongData (Bpm, OffsetMS, Base, Step);
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

	// Sets a New Song Position if the user clicked on Song Player Slider
	public void SetNewSongPosition () {
		active = false;

		if (songPlayerSlider.value * songAudioSource.clip.length < songAudioSource.clip.length) {
			songAudioSource.time = (songPlayerSlider.value * songAudioSource.clip.length);
		} else if ((songPlayerSlider.value * songAudioSource.clip.length >= songAudioSource.clip.length)) {
			StopSong ();
		}

        if (FindObjectOfType<MetronomePro> ().neverPlayed) {
			FindObjectOfType<MetronomePro> ().CalculateIntervals ();
		}

		FindObjectOfType<MetronomePro> ().CalculateActualStep ();

		actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds (songAudioSource.time);

		songPlayerBar.fillAmount = songPlayerSlider.value;
		active = true;
	}


	// Play or Pause the Song and Metronome
	public void PlayOrPauseSong() {
		if (playing) {
			Debug.Log ("Song Paused");
			active = false;
			playing = false;
			songAudioSource.Pause ();
			FindObjectOfType<MetronomePro> ().Pause ();
			playAndPauseButton.sprite = playSprite;

		} else {
			songAudioSource.Play ();
			FindObjectOfType<MetronomePro> ().Play ();
			Debug.Log ("Song Playing");
			playAndPauseButton.sprite = pauseSprite;
			playing = true;
			active = true;
		}
	}


	// Stop Song and Metronome, Resets all too.
	public void StopSong () {
		Debug.Log ("Song Stoped");
		StopAllCoroutines ();
		active = false;
		playing = false;

		songAudioSource.Stop ();
		songAudioSource.time = 0;
		playAndPauseButton.sprite = playSprite;
		amount = 0f;
		songPlayerSlider.value = 0f;
		songPlayerBar.fillAmount = 0f;
		actualPosition.text = "00:00";

		FindObjectOfType<MetronomePro> ().Stop ();
	}

	// Next Song
	public void NextSong () {
		StopSong ();

		// Load next song data
		// //

		// songAudioSource.clip = songClip;
		// SendSongData ();
		// PlayOrPauseSong();
	}

	// Previous Song
	public void PreviousSong () {
		StopSong ();

		// Load previous song data
		// //

		// songAudioSource.clip = songClip;
		// SendSongData ();
		// PlayOrPauseSong();
	}

	// Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
	void Update () {
		if (active) {
			if (playing) {
				if (songAudioSource.isPlaying) {
					amount = (songAudioSource.time) / (songAudioSource.clip.length);
					songPlayerBar.fillAmount = amount;
                    handleSlider.value = amount;
					actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds (songAudioSource.time);
				} else {
					StopSong ();
				}
			}
		}

		// Play song when user press Space button
		if (Input.GetKeyDown(KeyCode.Space)) {
			PlayOrPauseSong ();
		}



        // Testing


        if (Input.GetKeyDown(KeyCode.U))
        {
            // Assign the type of GREEN
            instantiatedTimelineObjectType = 0;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Assign the type of YELLOW
            instantiatedTimelineObjectType = 1;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Assign the type of ORANGE
            instantiatedTimelineObjectType = 2;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Assign the type of BLUE
            instantiatedTimelineObjectType = 3;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Assign the type of PURPLE
            instantiatedTimelineObjectType = 4;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Assign the type of RED
            instantiatedTimelineObjectType = 5;
            InstantiateTimelineObject(instantiatedTimelineObjectType);
        }
    }

    // Instantiate a timeline object at the current song time
    public void InstantiateTimelineObject(int instantiatedTimelineObjectTypePass)
    {
        // Get the handle position currently in the song to spawn the timeline object at
        handlePositionX = songPointSliderHandle.transform.position.x;
        // Decrease the Y position to prevent overlap
        handlePositionY -= 10;
        handlePositionZ = songPointSliderHandle.transform.position.z;

        // Assign the new position
        handlePosition = new Vector3(handlePositionX, handlePositionY, handlePositionZ);

        // Instantiate the type of object
        GameObject timelineObject = GameObject.Instantiate(instantiatedTimelineObject[instantiatedTimelineObjectTypePass], handlePosition,
        Quaternion.Euler(0, 45, 0), GameObject.FindGameObjectWithTag("Timeline").transform);
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
