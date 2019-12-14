using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;
using System.Collections.Generic;

public class DestroyTimelineObject : MonoBehaviour
{
    // UI
    public TextMeshProUGUI numberText;
    public Slider timelineSlider;
    public Image selectedImage, colorImage;

    // Integers
    private int timelineObjectListIndex;
    private float timelineHitObjectSpawnTime;
    private float hitObjectSliderValue;
    private float lastSavedSliderValue; // Last saved slider value before being moved on the timeline
    private float nearest;

    // Bools
    private bool previousFrameMouseHeldDown;
    private bool previousFrameBeatsnapValueTaken;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public float TimelineHitObjectSpawnTime
    {
        set { timelineHitObjectSpawnTime = value; }
        get { return timelineHitObjectSpawnTime; }
    }

    public int TimelineObjectListIndex
    {
        get { return timelineObjectListIndex; }
        set { timelineObjectListIndex = value; }
    }

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        // If the previous frame had the left mouse button pressed down, and there is no longer mouse button down on this frame
        if (previousFrameMouseHeldDown == true && (!Input.GetMouseButton(0)) && previousFrameBeatsnapValueTaken == true)
        {
            // Reset the current slider value to the last saved beat slider value
            ResetSliderValueToLastSavedBeat();
        }

        // If the previous frame had left mouse button down, and there is no longer the mouse button being pressed down, and the previous beatsnap value was not taken
        if (previousFrameMouseHeldDown == true && (!Input.GetMouseButton(0) && previousFrameBeatsnapValueTaken == false))
        {
            // Update timeline hit object spawn time
            UpdateTimelineHitObjectSpawnTime();
            // Update the last saved slider values
            lastSavedSliderValue = timelineSlider.value;
        }
    }

    // Reset the current slider value to the last saved beat slider value
    private void ResetSliderValueToLastSavedBeat()
    {
        // Reset the current slider value to the last saved beat slider value
        timelineSlider.value = lastSavedSliderValue;
        // Update last saved slider values
        lastSavedSliderValue = timelineSlider.value;

        // Reset variables 
        previousFrameMouseHeldDown = false;
        previousFrameBeatsnapValueTaken = false;
    }

    // Update the hierarchy position for this timeline object
    public void UpdateHierarchyPosition()
    {
        this.gameObject.transform.SetAsLastSibling();
    }

    // Enable and update the editable hit object
    public void SpawnEditableHitObject(string _trigger)
    {
        // If left click
        if (Input.GetMouseButton(0) || _trigger == "TIMELINEOBJECT")
        {
            // If exists
            if (scriptManager.editableHitObject)
            {
                // Activate the editable hit object
                scriptManager.editableHitObject.gameObject.SetActive(true);

                // Update the object index with the timeline object index
                scriptManager.editableHitObject.ObjectIndex = timelineObjectListIndex;

                // Setup the position and color of the hit object
                scriptManager.editableHitObject.SetupEditorObject();

                // Select this timeline object
                selectedImage.color = scriptManager.colorManager.selectedColor;
            }
        }
    }

    // Update the number text on the timeline hit object
    public void UpdateNumberText(int _value)
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        // If the value is different than what the text is already set to 
        if (numberText.text != _value.ToString())
        {
            // Update the text
            numberText.text = _value.ToString();

            // If the instantiated editable hit object is active for this timeline object
            if (scriptManager.editableHitObject.ObjectIndex == timelineObjectListIndex)
            {
                // Update the text for the editable hit object
                scriptManager.editableHitObject.UpdateEditableObjectIndex(_value);
            }
        }
    }

    // Deactivate the instantiated editable hit object
    public void DeactivateEditableHitObject()
    {
        scriptManager.editableHitObject.gameObject.SetActive(false);
    }

    public void DestroyEditorTimelineObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Deactivate editable hit object
            DeactivateEditableHitObject();
            // Check null timeline objects and update the list order/remove null objects from all lists
            scriptManager.placedObject.RemoveTimelineObject(timelineObjectListIndex);
            // Update the list orders
            scriptManager.placedObject.SortListOrders();
            // Update all timeline objects
            scriptManager.placedObject.UpdateTimelineObjects();
            // Destroy the game object
            Destroy(this.gameObject);
        }
    }

    // Update the timelines spawn time
    public void UpdateTimelineHitObjectSpawnTime()
    {
        timelineHitObjectSpawnTime = scriptManager.metronomePro_Player.UpdateTimelineHitObjectSpawnTimes(timelineSlider);
        // Send the new spawn time and the editorHitObject index to update
        scriptManager.placedObject.UpdateEditorHitObjectSpawnTime(timelineHitObjectSpawnTime, timelineObjectListIndex);
    }

    // Update the last saved slider value before the slider was moved on the timeline
    public void UpdateLastSavedSliderValue()
    {
        if (timelineSlider == null)
        {
            timelineSlider = this.gameObject.GetComponent<Slider>(); // Get the reference to the timelines own slider
        }

        lastSavedSliderValue = timelineSlider.value;
    }

    // Snap the timeline hit object to the nearsest beat snap
    public void SnapToNearestBeatsnap()
    {
        if (Input.GetMouseButton(0))
        {
            // Set to true as mouse is held down this frame
            previousFrameMouseHeldDown = true;

            // Get the slider value for this timeline hit object
            hitObjectSliderValue = timelineSlider.value;

            // Detect which beatsnap slider value the hit object slider value is closest to
            nearest = scriptManager.beatsnapManager.beatsnapSliderValueList.Select(p => new { Value = p, Difference = Math.Abs(p - hitObjectSliderValue) })
                      .OrderBy(p => p.Difference)
                      .First().Value;

            bool nearestBeatTaken = false;

            // Check if another hit object has that value
            for (int i = 0; i < scriptManager.placedObject.instantiatedTimelineObjectList.Count; i++)
            {
                Slider timelineObjectSlider = scriptManager.placedObject.instantiatedTimelineObjectList[i].GetComponent<Slider>();

                if (timelineObjectSlider != this.timelineSlider)
                {
                    // Check if the slider value has already been taken by another timeline object
                    if (timelineObjectSlider.value == nearest)
                    {
                        nearestBeatTaken = true;
                        break;
                    }
                    else
                    {
                        nearestBeatTaken = false;
                    }
                }
            }

            // Change the slider value to the nearest beatsnap slider value
            timelineSlider.value = nearest;

            // If the nearest beat has been taken
            if (nearestBeatTaken == true)
            {
                //Debug.Log("taken");

                previousFrameBeatsnapValueTaken = true;
            }
            else
            {
                //Debug.Log("not");

                previousFrameBeatsnapValueTaken = false;
            }
        }
    }
}
