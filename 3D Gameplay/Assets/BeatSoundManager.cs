using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using System.Linq;

public class BeatSoundManager : MonoBehaviour
{
    public bool active = false;

    public AudioSource songAudioSource;
    public AudioSource audioSource;
    public AudioClip highClip;
    public AudioClip lowClip;

    public double Bpm = 140.0f;
    public double OffsetMS = 0;

    public int Step = 4;
    public int Base = 4;

    public int CurrentMeasure = 0;
    public int CurrentStep = 0;
    public int CurrentTick;

    public List<Double> songTickTimes;

    double interval;

    public bool neverPlayed = true;

    private int division = 32;

    public bool playTickSound = false;

    // Hit sound preview manager for playing the selected hit sound by the player
    HitSoundPreview hitSoundPreview;

    // |SongProgressBar for getting the current song time
    SongProgressBar songProgressBar;


    public Text songTimeText;
    public Text nextTickTimeText;
    public Text hitTimeText;

    public float timeHit;

    private void Start()
    {
        // Get the song data/bpm/offset
        GetSongData();
        // Get the reference to the hit sound preview object
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();
        // get the reference to the songProgressBar object
        songProgressBar = FindObjectOfType<SongProgressBar>();
    }

    public void GetSongData()
    {
        Bpm = Database.database.loadedBPM;
        OffsetMS = Database.database.loadedOffsetMS;
    }

    // Play Metronome
    public void Play()
    {
        if (neverPlayed)
        {
            CalculateIntervals();
        }

        neverPlayed = false;
        active = true;
    }

    // Calculate Time Intervals for the song
    public void CalculateIntervals()
    {
        try
        {
            active = false;
            var multiplier = Base / Step;
            var tmpInterval = 60f / Bpm;
            interval = tmpInterval / multiplier;

            // Check the division, based on this calculate the intervals
            switch (division)
            {
                case 0:
                    // No division
                    break;
                case 8:
                    interval = interval / 2;
                    break;
                case 16:
                    interval = interval / 4;
                    break;
                case 32:
                    interval = interval / 6;
                    break;
            }

            int i = 0;

            songTickTimes.Clear();

            while (interval * i <= songAudioSource.clip.length)
            {
                songTickTimes.Add((interval * i) + (OffsetMS / 1000f));
                i++;


            }

            active = true;
        }
        catch
        {
            Debug.LogWarning("There isn't an Audio Clip assigned in the Player.");
        }
    }

    // Calculate Actual Step when the user changes song position in the UI
    public void CalculateActualStep()
    {
        active = false;

        // Get the Actual Step searching the closest Song Tick Time using the Actual Song Time
        for (int i = 0; i < songTickTimes.Count; i++)
        {
            if (songAudioSource.time < songTickTimes[i])
            {
                CurrentMeasure = (i / Base);
                CurrentStep = (int)((((float)i / (float)Base) - (i / Base)) * 4);
                if (CurrentStep == 0)
                {
                    CurrentMeasure = 0;
                    CurrentStep = 4;
                }
                else
                {
                    CurrentMeasure++;
                }

                CurrentTick = i;
                Debug.Log("Metronome Synchronized at Tick: " + i + " Time: " + songTickTimes[i]);
                break;
            }
        }
        active = true;
    }

    // Read Audio (this function executes from Unity Audio Thread)
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!active)
            return;

        // You can't execute any function of Unity here because this function is working on Unity Audio Thread (this ensure the Metronome Accuracy)
        // To Fix that you need to execute your function on Main Thread again, don't worry i created an easy way to do that :D
        // There are so much other fixes to do this, like Ninja Thread.
        ToMainThread.AssignNewAction().ExecuteOnMainThread(CalculateTicks());
    }

    public void GetTimeHit()
    {
        timeHit = songProgressBar.songTimePosition;
        playTickSound = true;
    }

    // Metronome Main function, this calculates the times to make a Tick, Step Count, Metronome Sounds, etc.
    IEnumerator CalculateTicks()
    {
        if (!active)
            yield return null;


        if (hitSoundPreview.hitSoundAudioSource.isPlaying == false)
        {
            hitSoundPreview.MuteHitSound();
        }

        nextTickTimeText.text = songTickTimes[CurrentTick].ToString();

        /*
        if (timeHit > songTickTimes[CurrentTick])
        {
            playTickSound = false;
        }
        */

        if (songAudioSource != null)
        {
            // Check if the song time is greater than the current tick Time
            if (songProgressBar.songTimePosition >= songTickTimes[CurrentTick])
            {
                CurrentTick++;

                if (CurrentTick >= songTickTimes.Count)
                {
                    active = false;
                }

                // If the Current Step is greater than the Step, reset it and increment the Measure
                if (CurrentStep >= Step)
                {
                    CurrentStep = 1;
                    CurrentMeasure++;
                }
                else
                {
                    CurrentStep++;
                }

                if (playTickSound == true)
                {
                    playTickSound = false;
                    OnTick();
                }

            }
        }
        yield return null;
    }

    // Tick Time (execute here all what you want)
    void OnTick()
    {
        hitSoundPreview.UnMuteHitSound();

        // Play the hit sound
        hitSoundPreview.PlayHitSound();

        hitTimeText.text = songProgressBar.songTimePosition.ToString();
    }
}
