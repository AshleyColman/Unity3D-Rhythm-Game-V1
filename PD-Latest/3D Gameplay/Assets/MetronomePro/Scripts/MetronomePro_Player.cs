// Created by Carlos Arturo Rodriguez Silva (Legend)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class MetronomePro_Player : MonoBehaviour
{

    [Header("Variables")]
    public bool active;
    public bool playing = false;

    [Space(5)]

    public AudioSource songAudioSource;
    public Sprite playSprite;
    public Sprite pauseSprite;

    [Space(5)]
    public TextMeshProUGUI actualPosition;
    public Image songPlayerBar;
    public TMP_Dropdown velocityScale;
    public Slider songPlayerSlider;
    public GameObject songPointSliderHandle;
    public Slider handleSlider;


    // Timeline position slider 
    public Slider timelinePositionSlider;
    public Image timelinePositionSliderImage;
    public GameObject timelinePositionSliderHandle;
    public Slider timelinePositionHandleSlider;

    // Reversed timeline slider
    public Slider reversedTimelineSlider;
    public GameObject reversedTimelineSliderHandle;
    public Slider reversedTimelineHandleSlider;

    [Header("Song Data")]
    public AudioClip songClip;

    public string songName;
    public string songArtist;

    [Space(5)]

    public float Bpm = 128;
    public float OffsetMS = 100;

    private int Step = 4;
    private int Base = 4;

    float amount;

    // Reference for the songDatabase for selecting and getting the song selected
    private SongDatabase songDatabase;

    // The song selected
    private int songSelectedIndex = 0;

    private MetronomePro metronomePro;


    void Start()
    {

        // Find the reference to the songDatabase
        songDatabase = FindObjectOfType<SongDatabase>();

        metronomePro = FindObjectOfType<MetronomePro>();

        // Stop any song and reset values
        StopSong();

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
    public void SendSongData()
    {
        metronomePro.GetSongData(Bpm, OffsetMS, Base, Step);
    }


    // Sets a New Song and Metronome Velocity using Velocity Scale Dropdown Value
    public void SetNewVelocity()
    {
        if (velocityScale.value == 4)
        {
            songAudioSource.pitch = 1;
        }
        else if (velocityScale.value == 5)
        {
            songAudioSource.pitch = 0.75f;
        }
        else if (velocityScale.value == 6)
        {
            songAudioSource.pitch = 0.50f;
        }
        else if (velocityScale.value == 7)
        {
            songAudioSource.pitch = 0.25f;
        }
        else if (velocityScale.value == 3)
        {
            songAudioSource.pitch = 1.25f;
        }
        else if (velocityScale.value == 2)
        {
            songAudioSource.pitch = 1.50f;
        }
        else if (velocityScale.value == 1)
        {
            songAudioSource.pitch = 1.75f;
        }
        else if (velocityScale.value == 0)
        {
            songAudioSource.pitch = 2.00f;
        }
        else
        {
            songAudioSource.pitch = 1;
        }
    }

    // Sets a New Song Position if the user clicked on Song Player Slider
    public void SetNewSongPosition()
    {
        active = false;

        if (songAudioSource.clip != null)
        {
            if (timelinePositionSlider.value * songAudioSource.clip.length < songAudioSource.clip.length)
            {
                songAudioSource.time = (timelinePositionSlider.value * songAudioSource.clip.length);
            }
            else if ((timelinePositionSlider.value * songAudioSource.clip.length >= songAudioSource.clip.length))
            {
                StopSong();
            }

            if (metronomePro.neverPlayed)
            {
                metronomePro.CalculateIntervals();
            }

            metronomePro.CalculateActualStep();

            actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.time);

            songPlayerBar.fillAmount = timelinePositionSlider.value;
            timelinePositionSliderImage.fillAmount = timelinePositionSlider.value;

            active = true;
        }
    }

    // Update the hit objects time from the slider passed and set it to a song time
    public float UpdateTimelineHitObjectSpawnTimes(Slider timelineSliderPass)
    {
        float newTimelineHitObjectSpawnTime = 0;
        // Set the new objects spawn time based on the slider value
        newTimelineHitObjectSpawnTime = (timelineSliderPass.value* songAudioSource.clip.length);

        return newTimelineHitObjectSpawnTime;
    }

    // Play or Pause the Song and Metronome
    public void PlayOrPauseSong()
    {
        if (playing)
        {
            active = false;
            playing = false;
            songAudioSource.Pause();
            metronomePro.Pause();

        }
        else
        {
            songAudioSource.Play();
            metronomePro.Play();
            playing = true;
            active = true;
        }
    }


    // Stop Song and Metronome, Resets all too.
    public void StopSong()
    {
        StopAllCoroutines();
        active = false;
        playing = false;

        songAudioSource.Stop();
        songAudioSource.time = 0;
        amount = 0f;
        songPlayerSlider.value = 0f;
        songPlayerBar.fillAmount = 0f;
        timelinePositionSlider.value = 0f;
        timelinePositionSliderImage.fillAmount = 0f;
        reversedTimelineSlider.value = 0f;


        actualPosition.text = "00:00";

        metronomePro.Stop();
    }

    // Next Song
    public void NextSong()
    {
        StopSong();

        // Load next song data
        // //

        // songAudioSource.clip = songClip;
        // SendSongData ();
        // PlayOrPauseSong();
    }

    // Previous Song
    public void PreviousSong()
    {
        StopSong();

        // Load previous song data
        // //

        // songAudioSource.clip = songClip;
        // SendSongData ();
        // PlayOrPauseSong();
    }

    // Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
    void Update()
    {
        if (active)
        {
            if (playing)
            {
                if (songAudioSource.isPlaying)
                {
                    amount = (songAudioSource.time) / (songAudioSource.clip.length);
                    songPlayerBar.fillAmount = amount;
                    timelinePositionSliderImage.fillAmount = amount;
                    handleSlider.value = amount;
                    timelinePositionHandleSlider.value = amount;
                    reversedTimelineHandleSlider.value = amount;

                    actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.time);
                }
                else
                {
                    StopSong();
                }
            }
        }

        // Play song when user press Space button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayOrPauseSong();
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

