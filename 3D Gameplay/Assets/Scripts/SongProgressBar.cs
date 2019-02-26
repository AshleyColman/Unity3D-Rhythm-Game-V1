using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongProgressBar : MonoBehaviour
{


    private bool active = true;
    private bool playing = true;

    public AudioSource songAudioSource;

    public Text actualPosition;
    public Text songTotalDuration;
    public Image playAndPauseButton;
    public Image songPlayerBar;
    public Slider songPlayerSlider;
    public AudioClip songClip;

    float amount;

    void Start()
    {

        // Assign the clip to the AudioSource
        songAudioSource.clip = songClip;

        // Display Song Data
        DisplaySongDuration();

        songAudioSource.Play();
    }


    // Calculate Song Total Duration
    public void DisplaySongDuration()
    {
        try
        {
            songTotalDuration.text = UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.clip.length);
        }
        catch
        {
            Debug.LogWarning("Please assign an Audio Clip to the Player!");
        }
    }


    // Play or Pause the Song and Metronome
    public void PlayOrPauseSong()
    {
        songAudioSource.Play();
        Debug.Log("Song Playing");
        playing = true;
        active = true;
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
                }
                else
                {
                    //StopSong ();
                }
            }
        }
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
