using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class MetronomeForEffects : MonoBehaviour
{

    [Header("Variables")]

    public GameObject mainMenuCanvas;

    public bool active = false;

    private Animator metronomeEffectsMainMenuCanvasAnimator;
    private LevelChanger levelChanger;

    public double Bpm = 140.0f;
    public double OffsetMS = 0;

    public int Step = 4;
    public int Base = 4;

    public int CurrentMeasure = 0;
    public int CurrentStep = 0;
    public int CurrentTick;

    public List<Double> songTickTimes;

    double interval;

    public bool neverPlayed = false;

    private AudioSource songAudioSource;
    private GameObject songAudioGameObject;

    float timer;

    int currentTick;

    private void Start()
    {
        levelChanger = FindObjectOfType<LevelChanger>();

        songAudioGameObject = GameObject.FindGameObjectWithTag("AudioSource");
        songAudioSource = songAudioGameObject.GetComponent<AudioSource>();
        CalculateIntervals();
    }

    public void GetSongData(double _bpm, double _offsetMS, int _base, int _step)
    {
        Bpm = _bpm;
        OffsetMS = _offsetMS;
        Base = _base;
        Step = _step;
    }

    // Calculate Time Intervals for the song
    public void CalculateIntervals()
    {
        try
        {
            var multiplier = Base / Step;
            var tmpInterval = 60f / Bpm;
            interval = tmpInterval / multiplier;

            int i = 0;

            songTickTimes.Clear();

            while (interval * i <= songAudioSource.clip.length)
            {
                songTickTimes.Add((interval * i) + (OffsetMS / 1000f));
                i++;
            }
        }
        catch
        {
            Debug.LogWarning("There isn't an Audio Clip assigned in the Player.");
        }
    }


    // Tick Time (execute here all what you want)
    void OnTick()
    {

        // Find the animator game object
        metronomeEffectsMainMenuCanvasAnimator = mainMenuCanvas.GetComponent<Animator>();

        // Only play canvas animation if the animator is not null
        if (metronomeEffectsMainMenuCanvasAnimator != null)
        {
            // Play canvas animation on start scene
            metronomeEffectsMainMenuCanvasAnimator.Play("MetronomeEffectsMainMenuCanvasAnimation");
        }
    }

    private void Update()
    {
        levelChanger = FindObjectOfType<LevelChanger>();

        // Check if the menu song is playing 
        if (songAudioSource.isPlaying)
        {
            // Increment the itmer
            timer += Time.deltaTime;

            // If the song has reached the end, check if it has started playing again, reset the timer to loop the animation
            if (songAudioSource.time >= songAudioSource.clip.length || songAudioSource.time == 0f || songAudioSource.isPlaying == false)
            {
                // Reset the menu animation that plays with the song
                ResetMenuAnimation();
            }

            // Check the timer against the tick times for the song
            if (currentTick < (songTickTimes.Count - 1))
            {
                if (timer >= songTickTimes[currentTick])
                {
                    // Play the animation
                    OnTick();
                    // Check for next tick next time
                    currentTick++;
                }
            }
        }
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
