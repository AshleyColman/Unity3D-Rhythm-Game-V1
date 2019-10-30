using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    public GameObject timeline, reverseTimeline;

    Vector3 timelineCurrentPosition;
    float x;

    private float currentTimelineWidth;

    public GameObject reversedTimelineHandle;
    Vector3 reversedTimelineHandlePosition;
    Vector3 reversedTimelineHandleStartingPosition;

    MetronomePro metronomePro;
    MetronomePro_Player metronomePro_Player;
    BeatsnapManager beatsnapManager;
    PlacedObject placedObject;
    EditorUIManager editorUIManager;
    LivePreview livePreview;

    void Start()
    {
        metronomePro = FindObjectOfType<MetronomePro>();
        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();
        beatsnapManager = FindObjectOfType<BeatsnapManager>();
        placedObject = FindObjectOfType<PlacedObject>();
        editorUIManager = FindObjectOfType<EditorUIManager>();
        livePreview = FindObjectOfType<LivePreview>();

        // Get the starting position for the reversed timeline handle
        reversedTimelineHandleStartingPosition = reversedTimelineHandle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // Get the position of the reversed timeline handle
        reversedTimelineHandlePosition = reversedTimelineHandle.transform.position;
        // Set the main timeline to the position of the reversed timeline handle
        timeline.transform.position = reversedTimelineHandlePosition;

        // Timeline tick navigation through keyboard input
        TimelineTickNavigation();

        // Change timeline object sizes based on key input
        //ChangeTimelineSize();
    }

    // Timeline tick navigation through keyboard input
    private void TimelineTickNavigation()
    {
        // Check for left and right arrow input to move timeline back and forward 1 tick
        if (metronomePro.songAudioSource.clip != null)
        {
            // If the song is not playing/paused
            if (metronomePro.songAudioSource.isPlaying == false)
            {
                var mouseScroll = Input.GetAxis("Mouse ScrollWheel");

                // Move the song and timeline back 1 tick if left arrow key is pressed
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Comma) || mouseScroll < 0f)
                {
                    // Navigate the timeline 1 tick back
                    TimelineNavigationBackwardOneTick();
                }

                // Move the song and timeline forward 1 tick if right arrow key is pressed
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Period) || mouseScroll > 0f)
                {
                    // Navigate the timeline forward one tick
                    TimelineNavigationForwardOneTick();
                }
            }
        }
    }

    // Navigate the timeline 1 tick back
    public void TimelineNavigationBackwardOneTick()
    {
        if (metronomePro.songTickTimes.Count != 0)
        {
            if (metronomePro.CurrentTick != 0)
            {
                // Move the song and timeline back 1 tick
                metronomePro.songAudioSource.time = (float)metronomePro.songTickTimes[metronomePro.CurrentTick - 1];

                // Calculate new metronome values
                metronomePro.CalculateDecrementMetronomeValues();

                // Update text and update timeline position
                metronomePro_Player.UpdateSongProgressUI();

                // Sort beatsnaps
                beatsnapManager.SortBeatsnaps();

                // If live preview is off
                if (editorUIManager.previewPanel.gameObject.activeSelf == false)
                {
                    // Displays the hit object for the beat currently selected
                    DisplaySelectedBeatTimelineObject();
                }
                else
                {
                    // Update the preview hit objects on scroll wheel 
                    livePreview.UpdatePreviewHitObjects();
                }
            }
        }
    }

    // Navigate the timeline forward one tick
    public void TimelineNavigationForwardOneTick()
    {
        if (metronomePro.songTickTimes.Count != 0)
        {
            if (metronomePro.songAudioSource.time < metronomePro.songTickTimes[metronomePro.songTickTimes.Count - 1])
            {
                // Move the song and timeline back 1 tick
                metronomePro.songAudioSource.time = (float)metronomePro.songTickTimes[metronomePro.CurrentTick + 1];

                // Update text and update timeline position
                metronomePro_Player.UpdateSongProgressUI();

                metronomePro.CalculateIncrementMetronomeValues();

                // Sort beatsnaps
                beatsnapManager.SortBeatsnaps();

                // If live preview is off
                if (editorUIManager.previewPanel.gameObject.activeSelf == false)
                {
                    // Displays the hit object for the beat currently selected
                    DisplaySelectedBeatTimelineObject();
                }
                else
                {
                    // Update the preview hit objects on scroll wheel 
                    livePreview.UpdatePreviewHitObjects();
                }
            }
        }
    }


    // Displays the hit object for the beat currently selected
    public void DisplaySelectedBeatTimelineObject()
    {
        // Check all timeline objects spawn times to see if a preview should be spawned/current selected object
        for (int i = 0; i < placedObject.destroyTimelineObjectList.Count; i++)
        {
            if (metronomePro.songAudioSource.time >= (placedObject.destroyTimelineObjectList[i].TimelineHitObjectSpawnTime - 0.0001)
                && metronomePro.songAudioSource.time <= (placedObject.destroyTimelineObjectList[i].TimelineHitObjectSpawnTime + 0.0001))
            {
                placedObject.destroyTimelineObjectList[i].SpawnEditableHitObject("TIMELINE_NAVIGATION");
                break;
            }
        }
    }

    // Reset the reversed Timeline sliders position
    public void ResetReversedTimelinePosition()
    {
        reversedTimelineHandlePosition = reversedTimelineHandleStartingPosition;
    }

    /*
    // Change the timeline size smaller or bigger
    public void ChangeTimelineSize()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            var rectTransform = timeline.transform as RectTransform;

            // Increment width
            currentTimelineWidth = rectTransform.sizeDelta.x;
            currentTimelineWidth += 1000;

            rectTransform.sizeDelta = new Vector2(currentTimelineWidth, rectTransform.sizeDelta.y);

            var reversedRectTransform = reverseTimeline.transform as RectTransform;
            reversedRectTransform.sizeDelta = new Vector2(currentTimelineWidth, reversedRectTransform.sizeDelta.y);

            // Update the width of all the beatsnap objects
            for (int i = 0; i < beatsnapManager.beatsnapGameObjectList.Count; i++)
            {
                var beatsnapRectTransform = beatsnapManager.beatsnapGameObjectList[i].transform as RectTransform;

                beatsnapRectTransform.sizeDelta = new Vector2(currentTimelineWidth, beatsnapRectTransform.sizeDelta.y);
            }

            // Update the width of all the timeline objects
            for (int i = 0; i < placedObject.instantiatedTimelineObjectList.Count; i++)
            {
                var timelineListObjectRectTransform = placedObject.instantiatedTimelineObjectList[i].transform as RectTransform;

                timelineListObjectRectTransform.sizeDelta = new Vector2(currentTimelineWidth, timelineListObjectRectTransform.sizeDelta.y);
            }
        }
    }
    */

}
