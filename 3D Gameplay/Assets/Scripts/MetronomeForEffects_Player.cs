
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MetronomeForEffects_Player : MonoBehaviour
{

    [Header("Variables")]
    public bool active;
    bool playing = false;

    [Space(5)]

    public AudioSource songAudioSource;


    [Header("Song Data")]
    public AudioClip songClip;

    public double Bpm = 128;
    public double OffsetMS = 100;

    public int Step = 4;
    public int Base = 4;

    private bool previouslyPaused = false;

    void Start()
    {

        // Assign the clip to the AudioSource
        songAudioSource.clip = songClip;

        StopSong();

        // Send Song Data to Metronome
        SendSongData();
    }

    // Sends Song Data to Metronome Pro script
    public void SendSongData()
    {
        FindObjectOfType<MetronomeForEffects>().GetSongData(Bpm, OffsetMS, Base, Step);
    }

    // Sets a New Song Position if the user clicked on Song Player Slider
    public void SetNewSongPosition()
    {
        if (FindObjectOfType<MetronomeForEffects>().neverPlayed)
        {
            FindObjectOfType<MetronomeForEffects>().CalculateIntervals();
        }

        FindObjectOfType<MetronomeForEffects>().CalculateActualStep();
    }

    // Play or Pause the Song and Metronome
    public void PlayOrPauseSong()
    {
        if (playing)
        {
            Debug.Log("Song Paused");
            active = false;
            playing = false;
            songAudioSource.Pause();
            FindObjectOfType<MetronomeForEffects>().Pause();

        }
        else
        {
            songAudioSource.Play();
            FindObjectOfType<MetronomeForEffects>().Play();
            Debug.Log("Song Playing");
            playing = true;
            active = true;
        }
    }


    // Stop Song and Metronome, Resets all too.
    public void StopSong()
    {
        Debug.Log("Song Stoped");
        StopAllCoroutines();
        active = false;
        playing = false;

        songAudioSource.Stop();
        songAudioSource.time = 0;

        FindObjectOfType<MetronomeForEffects>().Stop();
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

                }
                else
                {
                    StopSong();
                }
            }
        }

        // Play song when user press Space button
        if (playing == false)
        {
            PlayOrPauseSong();
        }

    }

}


