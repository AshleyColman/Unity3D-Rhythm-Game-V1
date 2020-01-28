using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class MetronomePro_Player : MonoBehaviour
{
    // Text
    public TextMeshProUGUI actualPositionText;

    // Dropdown
    public TMP_Dropdown velocityScale;

    // Slider
    public Slider songSlider, timelineSlider, reversedTimelineSlider, positionSlider;

    private float amount;

    // Scripts
    private ScriptManager scriptManager;

    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Check input to change the song play back speed
    private void CheckSongPlaybackSpeedInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (velocityScale.value == 0)
            {
                // Reset
                velocityScale.value = 12;
            }
            else
            {
                // Decrease the velocity playback speed value
                velocityScale.value--;
            }

            // Error check and reset if too high
            if (velocityScale.value > 12)
            {
                // Reset
                velocityScale.value = 0;
            }


            // Change the song play back speed
            ChangeSongPlaybackSpeed();
        }
    }

    // Change the song play back speed
    public void ChangeSongPlaybackSpeed()
    {
        switch (velocityScale.value)
        {
            case 0:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 2f;
                break;
            case 1:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 1.75f;
                break;
            case 2:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 1.5f;
                break;
            case 3:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 1.25f;
                break;
            case 4:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 1f;
                break;
            case 5:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 0.75f;
                break;
            case 6:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 0.5f;
                break;
            case 7:
                scriptManager.rhythmVisualizatorPro.audioSource.pitch = 0.25f;
                break;
        }
    }

    // Sets a New Song Position if the user clicked on Song Player Slider
    public void SetNewSongPosition()
    {
        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            if (positionSlider.value * scriptManager.rhythmVisualizatorPro.audioSource.clip.length <
                scriptManager.rhythmVisualizatorPro.audioSource.clip.length)
            {
                scriptManager.rhythmVisualizatorPro.audioSource.time = (positionSlider.value *
                    scriptManager.rhythmVisualizatorPro.audioSource.clip.length);
            }
            else if ((positionSlider.value * scriptManager.rhythmVisualizatorPro.audioSource.clip.length >=
                scriptManager.rhythmVisualizatorPro.audioSource.clip.length))
            {
                StopSong();
            }

            if (scriptManager.metronomePro.NeverPlayed)
            {
                scriptManager.metronomePro.CalculateIntervals();
            }

            scriptManager.metronomePro.CalculateActualStep();

            UpdateSongProgressUI();
        }
    }

    // Update the hit objects time from the slider passed and set it to a song time
    public float UpdateTimelineHitObjectSpawnTimes(Slider _timelineSlider)
    {
        float newTimelineHitObjectSpawnTime = 0;
        // Set the new objects spawn time based on the slider value
        newTimelineHitObjectSpawnTime = (_timelineSlider.value * scriptManager.rhythmVisualizatorPro.audioSource.clip.length);

        return newTimelineHitObjectSpawnTime;
    }

    // Play or Pause the Song and Metronome
    public void PlayOrPauseSong()
    {

        if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
        {
            PauseSong();
            // Sort beatsnaps
            scriptManager.beatsnapManager.SortBeatsnaps();
        }
        else
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.time == 0 ||
                scriptManager.rhythmVisualizatorPro.audioSource.time == scriptManager.rhythmVisualizatorPro.audioSource.clip.length)
            {
                // Reset
                StopSong();
                // Sort beatsnaps
                scriptManager.beatsnapManager.SortBeatsnaps();
            }

            // Play
            scriptManager.rhythmVisualizatorPro.audioSource.Play();
            scriptManager.metronomePro.Play();
        }
    }

    public void PauseSong()
    {
        scriptManager.rhythmVisualizatorPro.audioSource.Pause();
        scriptManager.metronomePro.Pause();
        scriptManager.timelineScript.SnapToClosestTickOnTimeline();
        scriptManager.rotatorManager.ResetLerpVariables();
    }

    // Stop Song and Metronome, Resets all too.
    public void StopSong()
    {
        StopAllCoroutines();

        scriptManager.rhythmVisualizatorPro.audioSource.Stop();
        scriptManager.rhythmVisualizatorPro.audioSource.time = 0;

        actualPositionText.text = "00:00";
        scriptManager.metronomePro.Stop();

        amount = 0f;
        songSlider.value = 0f;
        songSlider.value = 0f;
        reversedTimelineSlider.value = 0f;

        scriptManager.beatsnapManager.SortBeatsnaps();
    }

    // Update the song progress bar ui 
    public void UpdateSongProgressUI()
    {
        amount = (scriptManager.rhythmVisualizatorPro.audioSource.time) / (scriptManager.rhythmVisualizatorPro.audioSource.clip.length);
        timelineSlider.value = amount;
        reversedTimelineSlider.value = amount;
        songSlider.value = amount;

        actualPositionText.text = UtilityMethods.FromSecondsToMinutesAndSeconds(scriptManager.rhythmVisualizatorPro.audioSource.time);
    }

    // Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
    void Update()
    {
        // Check input to change the song play back speed
        //CheckSongPlaybackSpeedInput();

        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying)
            {
                UpdateSongProgressUI();
            }
        }
        // Play song when user press Space button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if the live preview panel is open
            // If live preview is active play live preview
            // Else play the song normally

            /*
            if (levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex)
            {
                if (editorUIManager.previewPanel.activeSelf == true)
                {
                    livePreview.StartOrPauseLivePreview();
                }
                else
                {
                    // Unmute metronome
                    metronomePro.UnmuteMetronome();

                    PlayOrPauseSong();
                }
            }
            else
            {
                PlayOrPauseSong();
            }
            */

            PlayOrPauseSong();
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

