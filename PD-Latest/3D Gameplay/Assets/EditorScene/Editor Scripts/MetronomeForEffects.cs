using System.Collections.Generic;
using UnityEngine;

public class MetronomeForEffects : MonoBehaviour
{

    // UI
    public GameObject mainMenuCanvas;

    // Audio
    public GameObject songAudioGameObject;
    public AudioSource songAudioSource;

    // Bools
    private bool active;
    private bool neverPlayed;
    private bool hasCalculatedCurrentTick; // Used for calculating the current tick in the song
    private bool hasSongInformationForGameplay; // For getting the bpm and offset information in-gameplay

    // Strings
    private string beatmapDifficultySelected; // Difficulty selected, changes which animations are played
    
    // Animation
    public Animator metronomeEffectsMainMenuCanvasAnimator;
    public Animator mainMenuCanvasFlashAnimator;
    // Song Select Animators
    public Animator titleAnimator;
    public Animator difficultyTextAnimator;
    public Animator flashImageAnimator;
    // Gameplay difficulty panel animators
    public Animator easyDifficultyPanelAnimator, advancedDifficultyPanelAnimator, extraDifficultyPanelAnimator;
    // Gameplay healthbar animator
    public Animator healthbarAnimator;
    // Blur flash animator
    public Animator blurFlashAnimator;
    // Gameplay fever time bar animators
    public Animator bar25PercentAnimator, bar50PercentAnimator, bar75PercentAnimator, bar100PercentAnimator;

    // Integers
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
    private LevelChanger levelChanger;
    private Healthbar healthbar;
    private LoadAndRunBeatmap loadAndRunBeatmap;
    private FeverTimeManager feverTimeManager;


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

        // Reference
        levelChanger = FindObjectOfType<LevelChanger>();
        songAudioGameObject = GameObject.FindGameObjectWithTag("AudioSource");
        songAudioSource = songAudioGameObject.GetComponent<AudioSource>();

        // Gameplay Scene References
        if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex)
        {
            // Get the reference to the healthbar gameobject
            healthbar = FindObjectOfType<Healthbar>();

            // Get the reference to the loadAndRunBeatmap gameobject
            loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>();

            feverTimeManager = FindObjectOfType<FeverTimeManager>();
        }
        else
        {
            // Do not calculate intervals for gameplay immediately
            // Calculate intervals for other scenes immediately however
            CalculateIntervals();
        }
    }

    // Update the beatmap difficulty selected 
    public void UpdateDifficultyAnimations(string _beatmapDifficulty)
    {
        beatmapDifficultySelected = _beatmapDifficulty;
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
        // If the song audio game object has not been found
        if (songAudioGameObject == null)
        {
            // Get the reference to the audio source
            songAudioGameObject = GameObject.FindGameObjectWithTag("AudioSource");
            songAudioSource = songAudioGameObject.GetComponent<AudioSource>();
        }

        // If the song audio source has been found
        if (songAudioSource != null)
        {
            // Check if the menu song is playing 
            if (songAudioSource.isPlaying)
            {
                // Increment the timer based on the current song time
                timer = songAudioSource.time;

                // Main Menu Scene Animation Play / Reset
                if (levelChanger.CurrentLevelIndex == levelChanger.MainMenuSceneIndex)
                {
                    // If the song has reached the end, check if it has started playing again, reset the timer to loop the animation
                    if (songAudioSource.time >= songAudioSource.clip.length || songAudioSource.time == 0f || songAudioSource.isPlaying == false)
                    {
                        // Reset the menu animation that plays with the song
                        ResetMenuAnimation();
                    }
                }

                // Main Menu Scene Animations
                if (levelChanger.CurrentLevelIndex == levelChanger.MainMenuSceneIndex)
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
                if (levelChanger.CurrentLevelIndex == levelChanger.SongSelectSceneIndex)
                {
                    // Calculate the current tick for song select scene preview time differences effecting animations
                    // Current tick needs to be calculated to ensure animations are played in sync
                    if (hasCalculatedCurrentTick == false)
                    {
                        // Calculate and update the current tick
                        currentTick = CalculateCurrentTick();

                        // Reset flash tick
                        flashTick = currentTick;
                        // Set to true to prevent recalculations
                        hasCalculatedCurrentTick = true;
                    }

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
                if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex)
                {
                    // Get bpm and offset information
                    if (hasSongInformationForGameplay == false)
                    {
                        bpm = Database.database.LoadedBPM;
                        offsetMS = Database.database.LoadedOffsetMS;

                        if (songAudioSource.clip != null)
                        {
                            CalculateIntervals();
                            CalculateMeasureDuration();
                        }

                        hasSongInformationForGameplay = true;
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

                            
                            // Difficulty Flash Animations
                            if (currentTick >= flashTick)
                            {
                                // Play the difficuly flash animation
                                flashTick += 4;
                                // PlayBeatFlashAnimation();
                                // PlayDifficultyTextAnimation();

                                // Play the blur flash animation
                                PlayBlurFlashAnimation();
                            }
                            
                        }
                    }
                }
            }
        }
    }

    // Get the beatmap difficulty for the beatmap being played
    private string GetBeatmapDifficulty()
    {
        return Database.database.LoadedBeatmapDifficulty;
    }

    // Play the gameplay difficulty panel animation
    private void PlayDifficultyPanelAnimation()
    {
        // Get the beatmap difficulty
        string beatmapDifficulty = GetBeatmapDifficulty();

        // Play the animation based on the beatmap difficulty
        switch (beatmapDifficulty)
        {
            case "easy":
                if (easyDifficultyPanelAnimator.isActiveAndEnabled)
                {
                    easyDifficultyPanelAnimator.Play("Gameplay_EasyDifficultyPanel", 0, 0f);
                }
                break;
            case "advanced":
                if (advancedDifficultyPanelAnimator.isActiveAndEnabled)
                {
                    advancedDifficultyPanelAnimator.Play("Gameplay_AdvancedDifficultyPanel", 0, 0f);
                }
                break;
            case "extra":
                if (extraDifficultyPanelAnimator.isActiveAndEnabled)
                {
                    extraDifficultyPanelAnimator.Play("Gameplay_ExtraDifficultyPanel", 0, 0f);
                }
                break;
        }
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

    // Play the fever time bar animations based on the percentage reached
    private void PlayFeverTimeBarAnimations()
    {
        if (feverTimeManager.HasSet25Percent == true)
        {
            bar25PercentAnimator.Play("FeverTimeBarPercent");
        }

        if (feverTimeManager.HasSet50Percent == true)
        {
            bar50PercentAnimator.Play("FeverTimeBarPercent");
        }

        if (feverTimeManager.HasSet75Percent == true)
        {
            bar75PercentAnimator.Play("FeverTimeBarPercent");
        }

        if (feverTimeManager.HasSet100Percent == true)
        {
            bar100PercentAnimator.Play("FeverTimeBarPercent");
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
        // Play healthbar beat animation
        PlayGameplayHealthbarAnimation();

        // Play the difficulty panel animation
        PlayDifficultyPanelAnimation();

        // Play fever time bar animations
        PlayFeverTimeBarAnimations();
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

    // Play gameplay healthbar animation
    private void PlayGameplayHealthbarAnimation()
    {
        // Play the color animation based on the current color of the healthbar
        switch (healthbar.CurrentHealthColor)
        {
            case "GREEN":
                healthbarAnimator.Play("Healthbar_Green", 0, 0f);
                break;
            case "YELLOW":
                healthbarAnimator.Play("Healthbar_Yellow", 0, 0f);
                break;
            case "RED":
                healthbarAnimator.Play("Healthbar_Red", 0, 0f);
                break;
        }
    }

    // Main Menu Scene flash animation
    private void PlayMainMenuSceneFlashAnimation()
    {
        if (levelChanger.CurrentLevelIndex == levelChanger.MainMenuSceneIndex)
        {
            //mainMenuCanvasFlashAnimator.Play("MainMenuFlash", 0, 0f);
            PlayBlurFlashAnimation();
        }
    }

    // Play the blur flash animation
    private void PlayBlurFlashAnimation()
    {
        blurFlashAnimator.Play("BlurFlash", 0, 0f);
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
