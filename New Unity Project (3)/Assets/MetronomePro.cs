using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class MetronomePro : MonoBehaviour
{
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
    private bool neverPlayed, metronomeIsMuted, active, metronomeMuted;

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

    public int Division
    {
        get { return division; }
        set { division = value; }
    }

    void Start()
    {
        metronomeIsMuted = false;
        active = false;
        neverPlayed = true;

        imgBeat1.color = Color.gray;
        imgBeat2.color = Color.gray;
        imgBeat3.color = Color.gray;
        imgBeat4.color = Color.gray;

        scriptManager = FindObjectOfType<ScriptManager>();

        OffsetText.text = "OFFSET: " + OffsetMS.ToString("F2");
        BPMText.text = "BPM: " + Bpm.ToString("F2");
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

    // Update the beatsnap division
    public void UpdateBeatsnapDivision()
    {
        switch (divisionDropdown.value)
        {
            case 0:
                division = 0;
                break;
            case 1:
                division = 8;
                break;
            case 2:
                division = 16;
                break;
            case 3:
                division = 32;
                break;
        }

        // Sort the beatsnaps with the new division
        scriptManager.beatsnapManager.SortBeatsnapsWithDivision();
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

    // Calculate Actual Step when the user changes song position in the UI
    public void CalculateActualStep()
    {
        active = false;

        // Get the Actual Step searching the closest Song Tick Time using the Actual Song Time
        for (int i = 0; i < songTickTimes.Count; i++)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.time < songTickTimes[i])
            {
                currentMeasure = (i / Base);
                currentStep = (int)((((float)i / (float)Base) - (i / Base)) * 4);
                if (currentStep == 0)
                {
                    currentMeasure = 0;
                    currentStep = 4;
                }
                else
                {
                    currentMeasure++;
                }

                currentTick = i;
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

        if (scriptManager.rhythmVisualizatorPro.audioSource != null)
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

                            // Only change the sound if the metronome is muted (so preview notes don't play the wrong sound)
                            if (metronomeIsMuted == false)
                            {
                                metronomeAudioSource.clip = highClip;
                            }
                        }
                        else
                        {
                            currentStep++;

                            // Only change the sound if the metronome is muted (so preview notes don't play the wrong sound)
                            if (metronomeIsMuted == false)
                            {
                                metronomeAudioSource.clip = lowClip;
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
        /*
        if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
        {
            beatsnapmanager.SortBeatsnaps();
        }
        */


        // Play Audio Tick
        if (metronomeIsMuted == false)
        {
            metronomeAudioSource.Play();
        }


        // Change all colors in the UI to gray
        imgBeat1.color = Color.gray;
        imgBeat2.color = Color.gray;
        imgBeat3.color = Color.gray;
        imgBeat4.color = Color.gray;

        // Change the color from the Actual Step Image in the UI
        if (currentStep == 1)
        {
            imgBeat1.color = Color.yellow;
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

        yield return null;
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
}