using System.Collections;
using UnityEngine;
using TMPro;

public class Timeline : MonoBehaviour
{
    // Gameobject
    public GameObject reversedTimelineHandle, timeline, timelineSlider;

    // Text
    public TextMeshProUGUI timelineSizeText;

    // Vector
    private Vector3 timelineCurrentPosition, reversedTimelineHandlePosition, reversedTimelineHandleStartingPosition;
    private Vector3 defaultTimelinePosition, timingSetupTimelinePosition;

    // Float
    private float x, currentTimelineWidth;
    private const float SIZE_VALUE = 2000, MIN_TIMELINE_SIZE = 15000, MAX_TIMELINE_SIZE = 70000;

    // Int
    private int currentTimelineSizeCount;

    // String
    private const string ADD_OPERATOR = "+", SUBTRACT_OPERATOR = "-";

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public float CurrentTimelineWidth
    {
        get { return currentTimelineWidth; }
    }

    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        defaultTimelinePosition = timeline.transform.localPosition;

        currentTimelineSizeCount = 1;

        // Setup timeline position is -450 on y axis so is positioned in the middle of screen
        timingSetupTimelinePosition = new Vector3(0, 0, 0);

        // Get the starting position for the reversed timeline handle
        reversedTimelineHandleStartingPosition = reversedTimelineHandle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the reversed timeline handle
        reversedTimelineHandlePosition = reversedTimelineHandle.transform.position;
        // Set the main timeline to the position of the reversed timeline handle
        scriptManager.metronomePro_Player.timelineSlider.transform.position = reversedTimelineHandlePosition;

        // Timeline tick navigation through keyboard input
        TimelineTickNavigation();

        // Change timeline object sizes based on key input
        //ChangeTimelineSize();
    }

    // Reset the timeline to the default position
    public void SetDefaultTimelinePosition()
    {
        timeline.transform.localPosition = defaultTimelinePosition;
    }

    // Set the timeline to the setup timing panel position
    public void SetTimingSetupTimlinePosition()
    {
        timeline.transform.localPosition = timingSetupTimelinePosition;
    }

    // Timeline tick navigation through keyboard input
    private void TimelineTickNavigation()
    {
        // Check for left and right arrow input to move timeline back and forward 1 tick
        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            // If the song is not playing/paused
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying == false)
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
        if (scriptManager.metronomePro.songTickTimes.Count != 0)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying == false)
            {
                if (scriptManager.metronomePro.CurrentTick != 0)
                {
                    // Move the song and timeline back 1 tick
                    scriptManager.rhythmVisualizatorPro.audioSource.time =
                        (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick - 1];

                    // Calculate new metronome values
                    scriptManager.metronomePro.CalculateDecrementMetronomeValues();

                    // Sort beatsnaps
                    //scriptManager.beatsnapManager.SortBeatsnaps();

                    StartCoroutine(DelayUpdateLatestBeatsnap(0f, "BACKWARD"));

                    // Update text and update timeline position
                    scriptManager.metronomePro_Player.UpdateSongProgressUI();

                    /*
                    // If live preview is off
                    if (scriptManager.editorUIManager.previewPanel.gameObject.activeSelf == false)
                    {
                        // Displays the hit object for the beat currently selected
                        DisplaySelectedBeatTimelineObject();
                    }
                    else
                    {
                        // Update the preview hit objects on scroll wheel 
                        scriptManager.livePreview.UpdatePreviewHitObjects();
                    }
                    */
                }
            }
        }
    }

    // Navigate the timeline forward one tick
    public void TimelineNavigationForwardOneTick()
    {
        if (scriptManager.metronomePro.songTickTimes.Count != 0)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying == false)
            {
                if (scriptManager.rhythmVisualizatorPro.audioSource.time <
                    scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.songTickTimes.Count - 1])
                {
                    // Move the song and timeline forward 1 tick
                    scriptManager.rhythmVisualizatorPro.audioSource.time =
                        (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick + 1];

                    // Calculate increment metronome values
                    scriptManager.metronomePro.CalculateIncrementMetronomeValues();

                    // Sort beatsnaps
                    //scriptManager.beatsnapManager.SortBeatsnaps();

                    StartCoroutine(DelayUpdateLatestBeatsnap(0f, "FORWARD"));

                    // Update text and update timeline position
                    scriptManager.metronomePro_Player.UpdateSongProgressUI();

                    /*
                    // If live preview is off
                    if (scriptManager.editorUIManager.previewPanel.gameObject.activeSelf == false)
                    {
                        // Displays the hit object for the beat currently selected
                        DisplaySelectedBeatTimelineObject();
                    }
                    else
                    {
                        // Update the preview hit objects on scroll wheel 
                        scriptManager.livePreview.UpdatePreviewHitObjects();
                    }
                    */
                }
            }
        }
    }


    // Navigate to the current tick on the timeline when the song is paused
    public void SnapToClosestTickOnTimeline()
    {
        if (scriptManager.metronomePro.songTickTimes.Count != 0)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.time <
                scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.songTickTimes.Count - 1])
            {
                // Get the closest tick time, comparing previous and current tick time
                float closestTickTime = scriptManager.placedObject.GetCurrentBeatsnapTime(scriptManager.rhythmVisualizatorPro.audioSource.time);

                // Update audio source time to the closest tick
                scriptManager.rhythmVisualizatorPro.audioSource.time = closestTickTime;

                // If the closest tick is the current tick
                if (closestTickTime == scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick])
                {
                    
                }
                else if (closestTickTime == scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick - 1])
                {
                    StartCoroutine(DelayUpdateLatestBeatsnap(0f, "BACKWARD"));
                }


                // Calculate increment metronome values
                scriptManager.metronomePro.CalculateActualStep();

                /*
                // Update all beatsnaps
                StartCoroutine(DelayUpdateSortBeatsnaps(0f));
                */

                // Update text and update timeline position
                scriptManager.metronomePro_Player.UpdateSongProgressUI();
            }
        }
    }

    // Delay and update the beatsnaps, prevents beatsnap frame display bug issue when scrolling timeline
    private IEnumerator DelayUpdateLatestBeatsnap(float _time, string _navigation)
    {
        yield return new WaitForSeconds(_time);

        // Sort beatsnaps
        scriptManager.beatsnapManager.SortLatestBeatsnap(_navigation);
    }

    // Sort all beatsnaps with delay for timeline when moving backwards via mouse scroll
    private IEnumerator DelayUpdateSortBeatsnaps(float _time)
    {
        yield return new WaitForSeconds(_time);

        // Sort beatsnaps
        scriptManager.beatsnapManager.SortBeatsnaps();
    }

    // Displays the hit object for the beat currently selected
    public void DisplaySelectedBeatTimelineObject()
    {
        // Check all timeline objects spawn times to see if a preview should be spawned/current selected object
        for (int i = 0; i < scriptManager.placedObject.destroyTimelineObjectList.Count; i++)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.time >= 
                (scriptManager.placedObject.destroyTimelineObjectList[i].TimelineHitObjectSpawnTime - 0.0001)
                && scriptManager.rhythmVisualizatorPro.audioSource.time <= 
                (scriptManager.placedObject.destroyTimelineObjectList[i].TimelineHitObjectSpawnTime + 0.0001))
            {
                scriptManager.placedObject.destroyTimelineObjectList[i].SpawnEditableHitObject();
                break;
            }
        }
    }

    // Reset the reversed Timeline sliders position
    public void ResetReversedTimelinePosition()
    {
        reversedTimelineHandlePosition = reversedTimelineHandleStartingPosition;
    }

    // Change the timeline size smaller or bigger
    public void ChangeTimelineSize(string _operator)
    {
        var rectTransform = scriptManager.timelineScript.timelineSlider.transform as RectTransform;
        currentTimelineWidth = rectTransform.sizeDelta.x;

        switch (_operator)
        {
            case ADD_OPERATOR:

                // Increment width
                if ((currentTimelineWidth + SIZE_VALUE) > MAX_TIMELINE_SIZE)
                {
                    currentTimelineWidth = MAX_TIMELINE_SIZE;
                }
                else
                {
                    currentTimelineWidth += SIZE_VALUE;
                    currentTimelineSizeCount++;
                }

                break;
            case SUBTRACT_OPERATOR:

                // Decrement width
                currentTimelineWidth = rectTransform.sizeDelta.x;

                if ((currentTimelineWidth - SIZE_VALUE) < MIN_TIMELINE_SIZE)
                {
                    currentTimelineWidth = MIN_TIMELINE_SIZE;
                }
                else
                {
                    currentTimelineWidth -= SIZE_VALUE;
                    currentTimelineSizeCount--;
                }

                break;
        }

        rectTransform.sizeDelta = new Vector2(currentTimelineWidth, rectTransform.sizeDelta.y);

        var reversedRectTransform = scriptManager.metronomePro_Player.reversedTimelineSlider.transform as RectTransform;
        reversedRectTransform.sizeDelta = new Vector2(currentTimelineWidth, reversedRectTransform.sizeDelta.y);

        timelineSizeText.text = "TIMELINE SIZE " + currentTimelineSizeCount.ToString();

        /*

        // Update the width of all the beatsnap objects
        for (int i = 0; i < scriptManager.beatsnapManager.beatsnapGameObjectList.Count; i++)
        {
            var beatsnapRectTransform = scriptManager.beatsnapManager.beatsnapGameObjectList[i].transform as RectTransform;

            beatsnapRectTransform.sizeDelta = new Vector2(currentTimelineWidth, beatsnapRectTransform.sizeDelta.y);
        }

        // Update the width of all the timeline objects
        for (int i = 0; i < scriptManager.placedObject.instantiatedTimelineObjectList.Count; i++)
        {
            var timelineListObjectRectTransform = scriptManager.placedObject.instantiatedTimelineObjectList[i].transform as RectTransform;

            timelineListObjectRectTransform.sizeDelta = new Vector2(currentTimelineWidth, timelineListObjectRectTransform.sizeDelta.y);
        }

        */
    }

}
