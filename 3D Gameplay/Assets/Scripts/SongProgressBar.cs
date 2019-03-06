using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SongProgressBar : MonoBehaviour {

    public Text txtSongName;
    public Text actualPosition;
    public Text songTotalDuration;
    public Image playAndPauseButton;
    public Image songPlayerBar;
    public Dropdown velocityScale;
    public Slider songPlayerSlider;
    public AudioSource songAudioSource;
    public bool active;
    bool playing = false;
    float songVolume = 0.4f;
    float amount;
    public AudioClip songClip;

    // Calculate Song Total Duration
    public void DisplaySongDuration()
    {
        try
        {
            songTotalDuration.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.clip.length);
        }
        catch
        {
            FindObjectOfType<MetronomePro>().txtState.text = "Please assign an Audio Clip to the Player!";
            Debug.LogWarning("Please assign an Audio Clip to the Player!");
        }
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
                    Debug.Log("Song Playing");
                    amount = (songAudioSource.time) / (songAudioSource.clip.length);
                    songPlayerBar.fillAmount = amount;
                    actualPosition.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.time);
                }
                else
                {
                    
                }
            }
        }

        // Play song when user press Space button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Play song
            songAudioSource.Play();
            songAudioSource.clip = songClip;
            songAudioSource.volume = songVolume;
            playing = true;
            active = true;
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
}



