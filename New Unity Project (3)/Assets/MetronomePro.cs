using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class MetronomePro : MonoBehaviour
{
    // Animator
    public Animator flashGlassAnimator;
    public Animator backgroundImageAnimator, backgroundImageAnimator2, videoPlayerImageAnimator, videoPlayerImageAnimator2, 
        topColorPanelGlowAnimator, bottomColorPanelGlowAnimator;

    // Audio
    public AudioSource metronomeAudioSource;
    public AudioClip highClip;
    public AudioClip lowClip;

    // Image
    public Image imgBeat1;
    public Image imgBeat2;
    public Image imgBeat3;
    public Image imgBeat4;

    // Input field
    public TMP_InputField BPMInputField, OffsetInputField;
    public TextMeshProUGUI BPMText, OffsetText;
    public TMP_Dropdown divisionDropdown;

    // Float
    public float Bpm = 120f;
    public float OffsetMS = 0f;

    // Int
    public int Step = 4;
    public int Base = 4;
    public int currentMeasure = 0;
    public int currentStep = 0;
    public int currentTick = 0;
    public int division = 0;

    // Double
    public List<Double> songTickTimes;
    private double interval;

    // Bool
    private bool neverPlayed, metronomeIsMuted, active, metronomeMuted, playTopPanelGlowAnimation;
    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public int CurrentMeasure
    {
        get { return currentMeasure; }
        set { currentMeasure = value; }
    }

    public int CurrentStep
    {
        get { return currentStep; }
        set { currentStep = value; }
    }

    public int CurrentTick
    {
        get { return currentTick; }
        set { currentTick = value; }
    }

    public bool MetronomeIsMuted
    {
        set { metronomeIsMuted = value; }
    }

    public bool NeverPlayed
    {
        get { return neverPlayed; }
    }

    void Start()
    {
        metronomeIsMuted = false;
        active = false;
        neverPlayed = true;

        ReferenceScriptManager();

        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.EditorSceneIndex)
        {
            // Change all colors in the UI to white
            ResetImgBeatColors();
            OffsetText.text = "OFFSET: " + OffsetMS.ToString("F2");
            BPMText.text = "BPM: " + Bpm.ToString("F2");
        }
    }

    // Set the new BPM when is playing
    public void UpdateBPM()
    {
        float bpmValue = 0;

        if (BPMInputField.text != "" && BPMInputField.text != "-")
        {
            bpmValue = float.Parse(BPMInputField.text);

            if (bpmValue < 60 || bpmValue > 500)
            {
                bpmValue = 60;
            }
        }
        else
        {
            bpmValue = 60;
        }

        Bpm = bpmValue;
        SetDelay();

        BPMText.text = "BPM: " + Bpm.ToString("F2");
    }

    // Set the new Offset when is playing
    public void UpdateOffset()
    {
        float offsetValue;

        if (OffsetInputField.text != "" && OffsetInputField.text != "-")
        {
            offsetValue = float.Parse(OffsetInputField.text);

            if (offsetValue < 0)
            {
                offsetValue = 0;
            }
        }
        else
        {
            offsetValue = 0;
        }

        OffsetMS = offsetValue;

        SetDelay();

        OffsetText.text = "OFFSET: " + OffsetMS.ToString("F2");
    }

    void SetDelay()
    {
        bool isPlaying = false;

        if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
        {
            isPlaying = true;
        }

        scriptManager.rhythmVisualizatorPro.audioSource.Pause();

        CalculateIntervals();
        CalculateActualStep();

        if (isPlaying)
        {
            scriptManager.rhythmVisualizatorPro.audioSource.Play();
        }
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

    // Pause Metronome
    public void Pause()
    {
        active = false;
    }

    // Stop Metronome
    public void Stop()
    {
        active = false;

        currentMeasure = 0;
        currentStep = 4;
        currentTick = 0;
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

            if (divisionDropdown == null)
            {
                division = 0;
            }
            else
            {
                division = divisionDropdown.value;
            }

            // Check the division, based on this calculate the intervals
            switch (division)
            {
                case 0:
                    // 1/1
                    // Default interval;
                    break;
                case 1:
                    // 1/2
                    interval = interval / 2;
                    break;
                case 2:
                    // 1/3
                    interval = interval / 3;
                    break;
                case 3:
                    // 1/4
                    interval = interval / 4;
                    break;
            }

            int i = 0;

            songTickTimes.Clear();

            ReferenceScriptManager();

            while (interval * i <= scriptManager.rhythmVisualizatorPro.audioSource.clip.length)
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
  
    // Get reference to the scriptManager
    private void ReferenceScriptManager()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
    }

    // Calculate Actual Step when the user changes song position in the UI
    public void CalculateActualStep()
    {
        // Get the Actual Step searching the closest Song Tick Time using the Actual Song Time
        // Loop through all song tick times

        for (int i = 0; i < songTickTimes.Count; i++)
        {
            // Get the closest tick time up to the current song position
            if (scriptManager.rhythmVisualizatorPro.audioSource.time <= songTickTimes[i])
            {
                currentMeasure = (i / 4);

                currentTick = i;
                break;
            }
        }

        // Reset current step
        currentStep = 0;

        // Loop through to the current tick, calculating the current step
        for (int i = 0; i < currentTick; i++)
        {
            // If the Current Step is greater than the Step, reset it and increment the Measure
            if (currentStep >= Step)
            {
                currentStep = 1;
            }
            else
            {
                currentStep++;
            }

            // Update metronome UI colors
            UpdateMetronomeUIColors();
        }
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


    // Calculate decremented metronome values
    public void CalculateDecrementMetronomeValues()
    {
        if (currentTick != 0)
        {
            // Decrement tick
            currentTick--;
        }

        if (currentStep != 0)
        {
            // Decrement current step
            currentStep--;
        }

        // Decrement current step
        if (currentStep < 1)
        {
            currentStep = 4;
        }

        // If the Current Step is greater than the Step, reset it and increment the Measure
        if (currentStep >= Step)
        {
            metronomeAudioSource.clip = highClip;
        }
        else
        {
            metronomeAudioSource.clip = lowClip;
        }

        // Call OnTick functions
        StartCoroutine(OnTick());
    }

    // Calculate Incremented metronome values
    public void CalculateIncrementMetronomeValues()
    {
        // Increment current tick
        currentTick++;

        // If the Current Step is greater than the Step, reset it and increment the Measure
        if (currentStep >= Step)
        {
            currentStep = 1;
            currentMeasure++;
            metronomeAudioSource.clip = highClip;
        }
        else
        {
            currentStep++;
            metronomeAudioSource.clip = lowClip;
        }

        // Call OnTick functions
        StartCoroutine(OnTick());
    }


    // Metronome Main function, this calculates the times to make a Tick, Step Count, Metronome Sounds, etc.
    public IEnumerator CalculateTicks()
    {
        if (!active)
            yield return null;

        {

                /*
                // Live preview UI object hit sounds
                if (livePreview != null)
                {
                    if (livePreview.PreviewOn == true)
                    {
                        if (livePreview.hasCalculatedOldestHitObjectIndex == true)
                        {
                            if (livePreview.oldestHitObjectIndex < placedObject.editorHitObjectList.Count)
                            {
                                if (scriptManager.rhythmVisualizatorPro.audioSource.time >= placedObject.editorHitObjectList[livePreview.oldestHitObjectIndex].hitObjectSpawnTime)
                                {
                                    // Increment to check next spawned UI preview hit object
                                    livePreview.oldestHitObjectIndex++;
                                    // Play hit sound
                                    metronomeAudioSource.PlayOneShot(highClip);
                                }
                            }
                        }
                    }
                }
                */

                if (currentTick < songTickTimes.Count)
            {
                {
                    // Check if the song time is greater than the current tick Time
                    if (scriptManager.rhythmVisualizatorPro.audioSource.time >= songTickTimes[currentTick])
                    {
                        currentTick++;

                        if (currentTick >= songTickTimes.Count)
                        {
                            active = false;
                        }

                        // If the Current Step is greater than the Step, reset it and increment the Measure
                        if (currentStep >= Step)
                        {
                            currentStep = 1;
                            currentMeasure++;

                            if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.EditorSceneIndex)
                            {
                                // Only change the sound if the metronome is muted (so preview notes don't play the wrong sound)
                                if (metronomeIsMuted == false)
                                {
                                    metronomeAudioSource.clip = highClip;
                                }
                            }
                        }
                        else
                        {
                            currentStep++;

                            if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.EditorSceneIndex)
                            {
                                // Only change the sound if the metronome is muted (so preview notes don't play the wrong sound)
                                if (metronomeIsMuted == false)
                                {
                                    metronomeAudioSource.clip = lowClip;
                                }
                            }
                        }

                        // Call OnTick functions
                        StartCoroutine(OnTick());
                    }
                }
            }
        }

        yield return null;
    }

    // Tick Time (execute here all what you want)
    IEnumerator OnTick()
    {
        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.EditorSceneIndex)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
            {
                // Sort latest beatsnap and push it to the back
                scriptManager.beatsnapManager.SortLatestBeatsnap("FORWARD");

                // Disable the timeline objects 
                scriptManager.placedObject.DisableTimelineObjects();
            }

            // Play Audio Tick
            if (metronomeIsMuted == false)
            {
                metronomeAudioSource.Play();
            }
            UpdateMetronomeUIColors();

            if (currentStep == 1)
            {
                EditorSceneOnMeasure();
            }

            EditorSceneOnMeasure();
        }


        if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
        {
            // Reset the rotating line lerp variables
            //scriptManager.rotatorManager.ResetLerpVariables();
            scriptManager.follower.UpdateLerpToNextObject();
        }

        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.GameplaySceneIndex)
        {
            GameplaySceneOnMeasure();
        }

        yield return null;
    }

    // Change all colors in the UI to white
    private void ResetImgBeatColors()
    {
        imgBeat1.color = Color.white;
        imgBeat2.color = Color.white;
        imgBeat3.color = Color.white;
        imgBeat4.color = Color.white;
    }

    private void UpdateMetronomeUIColors()
    {
        // Change all colors in the UI to white
        ResetImgBeatColors();

        // Change the color from the Actual Step Image in the UI
        if (currentStep == 1)
        {
            imgBeat1.color = scriptManager.colorManager.selectedColor;
        }
        else if (currentStep == 2)
        {
            imgBeat2.color = Color.cyan;
        }
        else if (currentStep == 3)
        {
            imgBeat3.color = Color.cyan;
        }
        else if (currentStep == 4)
        {
            imgBeat4.color = Color.cyan;
        }
    }

    // Mute the metronome so no sound plays on click
    public void MuteMetronome()
    {
        metronomeIsMuted = true;
    }

    // Unmute the metronome so sound plays on click
    public void UnmuteMetronome()
    {
        metronomeIsMuted = false;
    }

    // Editor scene on measure animations
    void EditorSceneOnMeasure()
    {
        flashGlassAnimator.Play("FlashGlass_Animation", 0, 0f);

        PlayColorPanelGlowAnimation();

        PlayBackgroundBeatAnimation();
    }

    // Gameplay scene on measure animations
    void GameplaySceneOnMeasure()
    {
        flashGlassAnimator.Play("FlashGlass_Animation", 0, 0f);

        PlayBackgroundBeatAnimation();
    }

    // Play the background beat animation for the current active background or video
    private void PlayBackgroundBeatAnimation()
    {
        // Background and video player images
        if (scriptManager.backgroundManager.ActiveBackgroundImageIndex == 1)
        {
            backgroundImageAnimator.Play("BackgroundImageBeat_Animation", 0, 0f);
        }
        else if (scriptManager.backgroundManager.ActiveBackgroundImageIndex == 2)
        {
            backgroundImageAnimator2.Play("BackgroundImageBeat_Animation", 0, 0f);
        }
        else if (scriptManager.backgroundManager.ActiveVideoPlayerIndex == 1)
        {
            videoPlayerImageAnimator.Play("BackgroundImageBeat_Animation", 0, 0f);
        }
        else if (scriptManager.backgroundManager.ActiveVideoPlayerIndex == 2)
        {
            videoPlayerImageAnimator2.Play("BackgroundImageBeat_Animation", 0, 0f);
        }
    }

    // Play the color panel glow animations
    private void PlayColorPanelGlowAnimation()
    {
        // Top and bottom panel glow animations
        if (playTopPanelGlowAnimation == true)
        {
            topColorPanelGlowAnimator.Play("TopColorPanelGlow_Animation", 0, 0f);

            playTopPanelGlowAnimation = false;
        }
        else
        {
            bottomColorPanelGlowAnimator.Play("BottomColorPanelGlow_Animation", 0, 0f);

            playTopPanelGlowAnimation = true;
        }
    }

}