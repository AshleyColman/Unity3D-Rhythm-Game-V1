// Created by Carlos Arturo Rodriguez Silva (Legend)
// Video: https://www.youtube.com/watch?v=LXYWPNltY0s
// Contact: carlosarturors@gmail.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class MusicPlayer : MonoBehaviour {

	[HideInInspector]
	public bool PrimaryMusicPlayer;

	[Header ("Assignation")]
	public AudioSource audioSource;

	[Header ("Music Player UI")]
	public Text songName;
	public Image playerBar;
	public Slider sliderBar;
	public Text actualTime;
	public Text totalTime;

    public int songIndex;

	[Header ("Songs List")]

    public SongDatabase songDatabase;

	int actualPos = 0;

	float amount = 0f;
	bool playing;
	bool active;

	public bool animateSearch = true;
	public Animator contentAnim;

	public static MusicPlayer instance;

	void Awake () {

        if (audioSource == null) {
            audioSource = FindObjectOfType<RhythmVisualizatorPro>().audioSource;
        }

        if (audioSource == null) {
            Debug.Log("Assign an Audio Source to the Music Player script");
            enabled = false;
            return;
        }

        if (instance == null) {
			PrimaryMusicPlayer = true;
			instance = this;
		} else {
			PrimaryMusicPlayer = false;
			return;
		}

		active = true;
		playing = true;

		actualPos = UnityEngine.Random.Range (0, songDatabase.songClip.Length - 1);
		ChangeSong (actualPos);
	}

    private void Start()
    {
        songDatabase = FindObjectOfType<SongDatabase>();
    }
    

	public void ChangeSong (int pos) {
		if (!PrimaryMusicPlayer) {
			instance.ChangeSong (pos);
			return;
		}

		StopSong ();

		actualPos = pos;

		PrepareToLoadSong (pos);
	}

	void Update () {

		if (!PrimaryMusicPlayer) {
			songName.text = instance.songName.text;
			actualTime.text = instance.actualTime.text;
			totalTime.text = instance.totalTime.text;
			playerBar.fillAmount = instance.playerBar.fillAmount;
			playing = instance.playing;
			amount = instance.amount;
			actualPos = instance.actualPos;
			return;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			PlayOrPauseSong ();
		} else if (Input.GetKeyDown (KeyCode.Backspace)) {
			StopSong ();
		}

		if (active) {
			if (playing) {
				if (audioSource.isPlaying) {
					amount = (audioSource.time / audioSource.clip.length);
					playerBar.fillAmount = amount;

					CalculateActualTime ();
		
				} else {
					active = false;
					playing = false;
					NextSong ();
				}
			}
		}
	}

	void CalculateActualTime () {
		// Calculate duration to show in 00:00 format
		var totalSeconds = audioSource.time;
		int seconds = (int)(totalSeconds % 60f); 
		int minutes = (int)((totalSeconds / 60f) % 60f); 

		actualTime.text = minutes + ":" + seconds.ToString ("D2");
	}

	/// <summary>
	/// Changes the playback position in the song.
	/// </summary>
	public void ChangePosition () {
		if (audioSource.clip != null) {
			active = false;
			audioSource.time = sliderBar.value * audioSource.clip.length;
			playerBar.fillAmount = sliderBar.value;
			active = true;

			CalculateActualTime ();
		}
	}

	/// <summary>
	/// Stops the song.
	/// </summary>
	public void StopSong () {
		if (!PrimaryMusicPlayer) {
			instance.StopSong ();
			return;
		}

		Debug.Log ("Stop");
		StopAllCoroutines ();
		active = false;
		playing = false;

		actualTime.text = "0:00";

		audioSource.Stop ();
		audioSource.time = 0;

		amount = 0f;
		sliderBar.value = 0f;
		playerBar.fillAmount = 0f;
	}

	/// <summary>
	/// Play or pause the song.
	/// </summary>
	public void PlayOrPauseSong () {
		if (!PrimaryMusicPlayer) {
			instance.PlayOrPauseSong ();
			return;
		}

		if (playing) {
			//	Debug.Log ("Pause");

			active = false;
			playing = false;
			audioSource.Pause ();

		} else {

			//	Debug.Log ("Play");

			audioSource.Play ();
			playing = true;
			active = true;
		}
	}

	/// <summary>
	/// Plays the next song.
	/// </summary>
	public void NextSong () {
		if (!PrimaryMusicPlayer) {
			instance.NextSong ();
			return;
		}

		ChangeSong (actualPos);

	}

	/// <summary>
	/// Plays the previous song.
	/// </summary>
	public void PreviousSong () {
		if (!PrimaryMusicPlayer) {
			instance.PreviousSong ();
			return;
		}

		--actualPos;

		if (actualPos < 0) {
			actualPos = songDatabase.songClip.Length - 1;
		}
						
		ChangeSong (actualPos);
	}

	/// <summary>
	/// Prepares to load the song.
	/// </summary>
	/// <param name="pos">Position.</param>
	void PrepareToLoadSong (int pos) {
		StopCoroutine ("LoadSong");
		//StartCoroutine (LoadSong ());
	}

	/// <summary>
	/// Loads the song.
	/// </summary>
	/// <returns>The song.</returns>
	/// <param name="song">Song.</param>
	IEnumerator LoadSong (int songIndexPass) {

        // Rename the clip
        AudioClip a = songDatabase.songClip[songIndexPass];

		// Loads and wait for song load
		#pragma warning disable 618
		while (!a.isReadyToPlay)
			#pragma warning restore 618
		{
			//	Debug.Log("Loading Song...");
			yield return null; 
		}

		// Assign the clip, and play
		audioSource.clip = a;

		// Calculate duration to show in 00:00 format
		var totalSeconds = audioSource.clip.length;
		int seconds = (int)(totalSeconds % 60f); 
		int minutes = (int)((totalSeconds / 60f) % 60f); 

		totalTime.text = minutes + ":" + seconds.ToString ("D2");

		PlayOrPauseSong ();
	}
}

public class OrderSongs : IComparer {

	int IComparer.Compare (System.Object x, System.Object y) {
		return((new CaseInsensitiveComparer ()).Compare (((GameObject)x).name, ((GameObject)y).name));
	}
}
