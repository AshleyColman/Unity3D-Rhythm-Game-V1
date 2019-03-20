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
    public float songAudioSourceTime;
    public int songClipChosenIndex; // The song clip chosen for this beatmap

    // Get the reference to the beatmap setup to disable starting the song when space is pressed whilst in the editor
    public BeatmapSetup beatmapSetup;
    // Required  for only disabling the songProgressBar in the editor and not gameplay
    public LevelChanger levelChanger;
    // Required for getting the song list from the songDatabase
    public SongDatabase songDatabase;

    void Start()
    {
        // Get the reference
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
        // Get the reference
        levelChanger = FindObjectOfType<LevelChanger>();
        // Get the reference
        songDatabase = FindObjectOfType<SongDatabase>();
    }

    // Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
    void Update()
    {
    
        if (levelChanger.currentLevelIndex == 2)
        {
            if (beatmapSetup.settingUp == false)
            {
                // Play song when user press Space button
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Play song
                    songAudioSource.clip = songDatabase.songClip[songClipChosenIndex];
                    songAudioSource.volume = songVolume;
                    songAudioSource.Play();
                    playing = true;
                    active = true;
                }
            }
        }
        else
        {
            // Play song when user press Space button in gameplay
            if (levelChanger.currentLevelIndex == 4)
            {
                // Set the song to the song loaded
                songClipChosenIndex = Database.database.loadedSongClipChosenIndex;
                songAudioSource.clip = songDatabase.songClip[songClipChosenIndex];
                songAudioSource.volume = songVolume;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Play song

                    songAudioSource.Play();
                    playing = true;
                    active = true;
                }
            }

        }
        


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
        songClipChosenIndex = songChosenIndexPass;
    }
}



