using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SongSelectPreview : MonoBehaviour
{
    public TextMeshProUGUI actualPosition;
    public TextMeshProUGUI songTotalDuration;
    public Image playAndPauseButton;
    public Image songPlayerBar;
    public Slider songPlayerSlider;
    public AudioSource songAudioSource;
    public bool active;
    bool playing = false;
    float songVolume = 0.4f;
    float amount;
    public int songClipChosenIndex;
    public float songAudioSourceTime;
    bool choseSong;
    public SongDatabase songDatabase; // Required for loading all the songs in the game

    void Start()
    {
        choseSong = true;
        // Get the reference
        songDatabase = FindObjectOfType<SongDatabase>();
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
                    actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.time);
                    songTotalDuration.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.clip.length);
                }
                else
                {

                }
            }
        }

        // Plays song when chosen
        if (choseSong == true)
        {
            PlaySongPreview();
        }

       

    }

    public static class UtilityMethods
    {
        public static string FromSecondsToMinutesAndSeconds(float seconds)
        {
            int sec = (int)(seconds % 60f);
            int min = (int)((seconds / 60f) % 60f);

            string minSec = min.ToString("D2") + ":" + sec.ToString("D2");
            return minSec;
        }
    }

    // Get the song chosen to load 
    public void GetSongChosen(int songChosenIndexPass)
    {
        // Get the index of the song chosens
        songClipChosenIndex = songChosenIndexPass;
        // Play the song
        PlaySongPreview();
    }

    // Play the song preview
    public void PlaySongPreview()
    {
        // Play song
        songAudioSource.clip = songDatabase.songClip[songClipChosenIndex];
        songAudioSource.volume = songVolume;
        songAudioSource.Play();
        playing = true;
        active = true;
        choseSong = false;
    }
}