using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

public class MetronomeForEffects : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public bool active = false;
    private LevelChanger levelChanger;
    public double Bpm = 170.0f;
    public double OffsetMS = 1000;
    public int Step = 4;
    public int Base = 4;
    public int CurrentMeasure = 0;
    public int CurrentStep = 0;
    public List<Double> songTickTimes;
    double interval;
    public bool neverPlayed = false;
    public AudioSource songAudioSource;
    public GameObject songAudioGameObject;
    public float timer;
    public int currentTick;
    public int flashTick;

    // Main Menu Animators
    private Animator metronomeEffectsMainMenuCanvasAnimator;
    public Animator mainMenuCanvasFlashAnimator;

    // Song Select Animators
    public Animator titleAnimator;
    public Animator difficultyTextAnimator;
    public Animator flashImageAnimator;


    // Used for calculating the current tick in the song
    bool hasCalculatedCurrentTick;

    // Difficulty selected, changes which animations are played
    string beatmapDifficultySelected;

    // Special time manager for special time animations
    public SpecialTimeManager specialTimeManager;

    // The previous random number used for special time animations
    int previousRandomNumber;
    // For getting the bpm and offset information in-gameplay
    bool hasSongInformationForGameplay;

    private void Start()
    {
        levelChanger = FindObjectOfType<LevelChanger>();

        songAudioGameObject = GameObject.FindGameObjectWithTag("AudioSource");
        songAudioSource = songAudioGameObject.GetComponent<AudioSource>();

        // Do not calculate internvals for gameplay immediately
        if (levelChanger.currentLevelIndex != levelChanger.gameplaySceneIndex)
        {
            CalculateIntervals();
        }


        flashTick = 2;
    }

    // Update the beatmap difficulty selected 
    public void UpdateDifficultyAnimations(string beatmapDifficultyPass)
    {
        beatmapDifficultySelected = beatmapDifficultyPass;
    }

    public void GetSongData(double _bpm, double _offsetMS)
    {
        Bpm = _bpm;
        OffsetMS = _offsetMS;
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

    private void Update()
    {
        // Get the reference to the level changer
        levelChanger = FindObjectOfType<LevelChanger>();

        // Check if the menu song is playing 
        if (songAudioSource.isPlaying)
        {
            // Increment the timer based on the current song time
            timer = songAudioSource.time;

            // Main Menu Animation Reset
            if (levelChanger.currentLevelIndex == levelChanger.mainMenuSceneIndex)
            {
                // If the song has reached the end, check if it has started playing again, reset the timer to loop the animation
                if (songAudioSource.time >= songAudioSource.clip.length || songAudioSource.time == 0f || songAudioSource.isPlaying == false)
                {
                    // Reset the menu animation that plays with the song
                    ResetMenuAnimation();
                }
            }


            // Calculate the current tick for song select scene preview time differences effecting animations
            // Current tick needs to be calculated to ensure animations are played in sync
            if (levelChanger.currentLevelIndex == levelChanger.songSelectSceneIndex)
            {
                if (hasCalculatedCurrentTick == false)
                {
                    // Calculate and update the current tick
                    currentTick = CalculateCurrentTick();

                    // Reset flash tick
                    flashTick = currentTick;
                    // Set to true to prevent recalculations
                    hasCalculatedCurrentTick = true;
                }
            }

            // Main Menu Scene Animations
            if (levelChanger.currentLevelIndex == levelChanger.mainMenuSceneIndex)
            {
                // Check the timer against the tick times for the song
                if (currentTick < (songTickTimes.Count - 1))
                {
                    if (timer >= songTickTimes[currentTick])
                    {
                        // Play ontick animations
                        MainMenuSceneOnTick();
                        // Check for next tick next time
                        currentTick++;

                        if (currentTick >= flashTick)
                        {
                            // Play the flash animation on the main menu
                            flashTick += 4;
                            PlayMainMenuSceneFlashAnimation();
                        }
                    }
                }
            }

            // Song Select Scene Animations
            if (levelChanger.currentLevelIndex == levelChanger.songSelectSceneIndex)
            {
                // Check the timer against the tick times for the song
                if (currentTick < (songTickTimes.Count - 1))
                {
                    if (timer >= songTickTimes[currentTick])
                    {
                        // Play ontick animations
                        SongSelectSceneOnTick();
                        // Check for next tick next time
                        currentTick++;

                        PlayDifficultyTextAnimation();

                        // Difficulty Flash Animations
                        if (currentTick >= flashTick)
                        {
                            // Play the difficuly flash animation
                            flashTick += 4;
                            PlayBeatFlashAnimation();

                        }
                    }
                }
            }


            // Gameplay special time Animations
            if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex)
            {
                if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex)
                {
                    // Get reference to the special time manager
                    specialTimeManager = FindObjectOfType<SpecialTimeManager>();

                    // Get bpm and offset information
                    if (hasSongInformationForGameplay == false)
                    {
                        Bpm = Database.database.loadedBPM;
                        OffsetMS = Database.database.loadedOffsetMS;

                        if (songAudioSource.clip != null)
                        {
                            CalculateIntervals();
                        }

                        hasSongInformationForGameplay = true;
                    }
                }



                // Check the timer against the tick times for the song
                if (currentTick < (songTickTimes.Count - 1))
                {
                    if (timer >= songTickTimes[currentTick])
                    {
                        // Play ontick animations
                        GameplaySceneOnTick();
                        // Check for next tick next time
                        currentTick++;

                        /*
                        // Difficulty Flash Animations
                        if (currentTick >= flashTick)
                        {
                            // Play the difficuly flash animation
                            flashTick += 4;
                            PlayBeatFlashAnimation();
                            PlayDifficultyTextAnimation();
                        }
                        */
                    }
                }
            }
        }
    }

    // Main Menu Scene tick functions
    void MainMenuSceneOnTick()
    {
        // Find the animator game object
        metronomeEffectsMainMenuCanvasAnimator = mainMenuCanvas.GetComponent<Animator>();

        // Only play canvas animation if the animator is not null
        if (metronomeEffectsMainMenuCanvasAnimator != null)
        {
            // Play canvas animation on start scene
            metronomeEffectsMainMenuCanvasAnimator.Play("MetronomeEffectsMainMenuCanvasAnimation", 0, 0f);
        }
    }

    // Song Select Scene tick functions
    void SongSelectSceneOnTick()
    {
        // Only play the title animation if the title animator exists
        if (titleAnimator != null)
        {
            // Play title animation
            titleAnimator.Play("TitleAnimation", 0, 0f);
        }
    }

    // Gameplay Scene tick functions
    void GameplaySceneOnTick()
    {
        // If special time is activate play special time animation
        // Get random number from 1-7
        int randomNumber = Random.Range(1, 6);

        // Check random number against the the previous random number to ensure no doubles
        if (randomNumber == previousRandomNumber)
        {
            if (randomNumber == 6)
            {
                // Reset random number
                randomNumber = 1;
            }
            else
            {
                // Increment random number for different color animation
                randomNumber++;
            }
        }

        if (specialTimeManager.isSpecialTime == true)
        {
            switch (randomNumber)
            {
                case 1:
                    specialTimeManager.BlueBackgroundAnimation();
                    break;
                case 2:
                    specialTimeManager.PurpleBackgroundAnimation();
                    break;
                case 3:
                    specialTimeManager.RedBackgroundAnimation();
                    break;
                case 4:
                    specialTimeManager.YellowBackgroundAnimation();
                    break;
                case 5:
                    specialTimeManager.OrangeBackgroundAnimation();
                    break;
                case 6:
                    specialTimeManager.GreenBackgroundAnimation();
                    break;
            }

            // Update the previous random number
            previousRandomNumber = randomNumber;
        }
    }

    // Reset and allow current tick to be recalculated when the song has changed
    public void ResetCalculateCurrentTick()
    {
        hasCalculatedCurrentTick = false;
    }


    // Calculate the current tick past based on the current song time
    private int CalculateCurrentTick()
    {
        int latestTick = 0;

        for (int iCount = 0; iCount < songTickTimes.Count; iCount++)
        {
            // Check the current song time against the tick times, see which tick time we're at
            if (timer >= songTickTimes[iCount] && timer <songTickTimes[iCount + 1])
            {
                // Get the latest tick
                latestTick = iCount;
            }
        }

        return latestTick;
    }

    // Main Menu Scene flash animation
    private void PlayMainMenuSceneFlashAnimation()
    {
        if (levelChanger.currentLevelIndex == levelChanger.mainMenuSceneIndex)
        {
            mainMenuCanvasFlashAnimator.Play("MainMenuFlash", 0, 0f);
        }
    }

    // Play beat flash animation
    private void PlayBeatFlashAnimation()
    {
        switch (beatmapDifficultySelected)
        {
            case "easy":
                flashImageAnimator.Play("EASYBeatFlashImage", 0, 0f);
                break;
            case "advanced":
                flashImageAnimator.Play("ADVANCEDBeatFlashImage", 0, 0f);
                break;
            case "extra":
                flashImageAnimator.Play("EXTRABeatFlashImage", 0, 0f);
                break;
        }


    }

    // Song Select Scene difficulty beat flash animation
    private void PlayDifficultyTextAnimation()
    {
        switch (beatmapDifficultySelected)
        {
            case "easy":
                difficultyTextAnimator.Play("EASYDifficultyText", 0, 0f);
                break;
            case "advanced":
                difficultyTextAnimator.Play("ADVANCEDDifficultyText", 0, 0f);
                break;
            case "extra":
                difficultyTextAnimator.Play("EXTRADifficultyText", 0, 0f);
                break;
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
