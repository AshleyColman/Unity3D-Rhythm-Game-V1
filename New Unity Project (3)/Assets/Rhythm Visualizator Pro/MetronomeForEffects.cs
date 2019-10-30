using System.Collections.Generic;
using UnityEngine;

public class MetronomeForEffects : MonoBehaviour
{
    // Animator
    public Animator flashGlassAnimator;
    public Animator backgroundImageAnimator, backgroundImageAnimator2, videoPlayerImageAnimator, videoPlayerImageAnimator2, lightPanelBeatAnimator;

    // Audio
    public AudioSource songAudioSource;

    // Bools
    private bool active;
    private bool neverPlayed;
    private bool hasCalculatedCurrentTick; // Used for calculating the current tick in the song

    // Integers
    private int currentBeatShapeAnimatorIndex;

    public List<float> songTickTimes;
    private float interval;
    private float bpm;
    private float offsetMS;
    private float timer;
    public float tickTimeDifference; // Time difference between ticks
    public float measureDuration; // Time that makes up a measure in the song
    private int currentTick;
    private int flashTick;
    private int Step;
    private int Base;
    private int CurrentMeasure;
    private int CurrentStep;

    // Scripts
    private MenuManager menuManager;
    private BackgroundManager backgroundManager;

    private void Start()
    {
        // Initialize
        bpm = 170.0f;
        offsetMS = 1000;
        Step = 4;
        Base = 4;
        CurrentMeasure = 0;
        CurrentStep = 0;
        flashTick = 2;
        active = false;
        neverPlayed = false;

        bpm = 175f;
        offsetMS = 280;

        // Do not calculate intervals for gameplay immediately
        // Calculate intervals for other scenes immediately however
        CalculateIntervals();

        // Reference
        menuManager = FindObjectOfType<MenuManager>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
    }

    // Get the bpm and offset information
    public void GetSongData(float _bpm, float _offsetMS)
    {
        bpm = _bpm;
        offsetMS = _offsetMS;
    }

    // Calculate Time Intervals for the song
    public void CalculateIntervals()
    {
        try
        {
            var multiplier = Base / Step;
            var tmpInterval = 60f / bpm;
            interval = tmpInterval / multiplier;

            int i = 0;

            songTickTimes.Clear();

            while (interval * i <= songAudioSource.clip.length)
            {
                songTickTimes.Add((interval * i) + (offsetMS / 1000f));
                i++;
            }
        }
        catch
        {
            Debug.LogWarning("There isn't an Audio Clip assigned in the Player.");
        }
    }

    private void Update()
    {
        // If the song audio source has been found
        if (songAudioSource != null)
        {
            // Check if the menu song is playing 
            if (songAudioSource.isPlaying)
            {
                // Increment the timer based on the current song time
                timer = songAudioSource.time;

                // Check the timer against the tick times for the song
                if (currentTick < (songTickTimes.Count - 1))
                {
                    if (timer >= songTickTimes[currentTick])
                    {
                        // Play ontick animations
                        StartMenuSceneOnTick();
                        SongSelectSceneOnTick();

                        // Check for next tick next time
                        currentTick++;


                        // Difficulty Flash Animations
                        if (currentTick >= flashTick)
                        {
                            // Play the difficuly flash animation
                            flashTick += 4;

                            // ON MEASURE

                            StartMenuOnMeasure();
                            SongSelectSceneOnMeasure();
                        }
                    }
                }
            }
        }
    }

    // Start menu tick functions
    void StartMenuSceneOnTick()
    {

    }

    void StartMenuOnMeasure()
    {

    }

    private void SongSelectSceneOnMeasure()
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            flashGlassAnimator.Play("FlashGlass_Animation", 0, 0f);

            lightPanelBeatAnimator.Play("LightPanelBeat_Animation", 0, 0f);


            // Background and video player images
            if (backgroundManager.ActiveBackgroundImageIndex == 1)
            {
                backgroundImageAnimator.Play("BackgroundImageBeat_Animation", 0, 0f);
            }
            else if (backgroundManager.ActiveBackgroundImageIndex == 2)
            {
                backgroundImageAnimator2.Play("BackgroundImageBeat_Animation", 0, 0f);
            }
            else if (backgroundManager.ActiveVideoPlayerIndex == 1)
            {
                videoPlayerImageAnimator.Play("BackgroundImageBeat_Animation", 0, 0f);
            }
            else if (backgroundManager.ActiveVideoPlayerIndex == 2)
            {
                videoPlayerImageAnimator2.Play("BackgroundImageBeat_Animation", 0, 0f);
            }
        }
    }

    private void SongSelectSceneOnTick()
    {

    }

    // Get the measure duration for the song
    public float GetMeasureDuration()
    {
        return CalculateMeasureDuration();
    }

    // Calculate duration for measure
    private float CalculateMeasureDuration()
    {
        // Get the difference for 1 tick
        tickTimeDifference = (float)songTickTimes[1] - (float)songTickTimes[0];

        // Measure = 4 ticks
        measureDuration = tickTimeDifference * 4;

        return measureDuration;
    }

   // Reset and allow current tick to be recalculated when the song has changed
    public void ResetCalculateCurrentTick()
    {
        hasCalculatedCurrentTick = false;
    }


    // Calculate the current tick past based on the current song time
    public void CalculateCurrentTick()
    {
        int latestTick = 0;

        for (int iCount = 0; iCount < songTickTimes.Count; iCount++)
        {
            // Check the current song time against the tick times, see which tick time we're at
            if (timer >= songTickTimes[iCount] && timer < songTickTimes[iCount + 1])
            {
                // Get the latest tick
                latestTick = iCount;
            }
        }

        currentTick = latestTick;
    }

    // Reset the menu animation that plays with the song
    private void ResetMenuAnimation()
    {
        // Reset the timer
        timer = 0f;
        // Reset the current tick time to check for animation
        currentTick = 0;
    } 
}
