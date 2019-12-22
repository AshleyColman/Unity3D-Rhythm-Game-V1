using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DestroyTimelineObject : MonoBehaviour
{
    // UI
    public TextMeshProUGUI numberText;
    public Slider timelineSlider;

    // Integers
    private int timelineObjectListIndex, timelineHitObjectType, timelineHitObjectAnimationType, timelineHitObjectSoundType;
    private float timelineHitObjectSpawnTime;
    private float hitObjectSliderValue;
    private float lastSavedSliderValue; // Last saved slider value before being moved on the timeline
    private float nearest;

    // Bool
    private bool toggleOn;

    // Vector3
    private Vector3 timelineHitObjectPosition;

    // Color block
    private ColorBlock colorBlock;

    // Color 
    private Color color;

    // Bools
    private bool previousFrameMouseHeldDown;
    private bool previousFrameBeatsnapValueTaken;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public Vector3 TimelineHitObjectPosition
    {
        get { return timelineHitObjectPosition; }
        set { timelineHitObjectPosition = value; }
    }

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

    public int TimelineHitObjectType
    {
        get { return timelineHitObjectType; }
        set { timelineHitObjectType = value; }
    }

    public int TimelineHitObjectAnimationType
    {
        get { return timelineHitObjectAnimationType; }
        set { timelineHitObjectAnimationType = value; }
    }

    public int TimelineHitObjectSoundType
    {
        get { return timelineHitObjectSoundType; }
        set { timelineHitObjectSoundType = value; }
    }

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        color = scriptManager.colorManager.whiteColor;
        colorBlock.colorMultiplier = 1f;
    }

    private void Update()
    {
        /*
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
        */
    }

    public void SetToggleOff()
    {
        toggleOn = false;
    }

    public void SetToggleOn()
    {
        toggleOn = true;
    }

    // Check toggle and make object selected/unselected
    public void CheckToggle()
    {
        if (toggleOn == true)
        {
            color.a = 0.25f;
            colorBlock.normalColor = color;

            color.a = 0.5f;
            colorBlock.highlightedColor = color;
            colorBlock.selectedColor = color;

            color.a = 0.75f;
            colorBlock.pressedColor = color;

            colorBlock.disabledColor = color;

            timelineSlider.colors = colorBlock;
        }
        else
        {
            color.a = 0f;
            colorBlock.normalColor = color;

            color.a = 0.25f;
            colorBlock.highlightedColor = color;
            colorBlock.selectedColor = color;

            color.a = 0.5f;
            colorBlock.pressedColor = color;

            colorBlock.disabledColor = color;

            timelineSlider.colors = colorBlock;
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
    public void SpawnEditableHitObject()
    {
        // Activate the editable hit object
        scriptManager.editableHitObject.gameObject.SetActive(true);

        // Reset the previous timeline object if there was one or if it still exists
        scriptManager.editableHitObject.ResetTimelineObject();

        // Pass reference to the timeline object tying it together with this script
        scriptManager.editableHitObject.UpdateReferenceToTimelineObject(this.gameObject);

        // Update the object index with the timeline object index
        scriptManager.editableHitObject.ObjectIndex = timelineObjectListIndex;

        // Setup the position and color of the hit object
        scriptManager.editableHitObject.SetupEditorObject();

        // Update ui editable hit object menu text
        scriptManager.editorBottomMenu.UpdateBottomMenu(timelineObjectListIndex, timelineHitObjectSpawnTime, 
            timelineHitObjectAnimationType, timelineHitObjectType, timelineHitObjectSoundType);
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

    // Destroy this timeline object and remove all information from the lists
    public void DestroyEditorTimelineObject()
    {
        if (Input.GetMouseButton(1))
        {
            // Deactivate editable hit object
            DeactivateEditableHitObject();
            // Check null timeline objects and update the list order/remove null objects from all lists
            scriptManager.placedObject.RemoveTimelineObject(timelineObjectListIndex);
            // Update the list orders
            scriptManager.placedObject.SortListOrders();
            // Update all timeline objects
            scriptManager.placedObject.UpdateTimelineObjects();
            // Default the editor bottom menu
            scriptManager.editorBottomMenu.ResetBottomMenu();
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
                previousFrameBeatsnapValueTaken = true;
            }
            else
            {
                previousFrameBeatsnapValueTaken = false;
            }
        }
    }

    // Check if new slider value has been taken, save changes and update objects if new slider value has not been taken
    public void UpdateTimelineObjectOrder()
    {
        switch (previousFrameBeatsnapValueTaken)
        {
            case true:
                // Reset the current slider value to the last saved beat slider value
                ResetSliderValueToLastSavedBeat();
                break;
            case false:
                // Update timeline hit object spawn time
                UpdateTimelineHitObjectSpawnTime();
                // Update the last saved slider values
                lastSavedSliderValue = timelineSlider.value;
                // Update the list orders
                scriptManager.placedObject.SortListOrders();
                // Update all timeline objects
                scriptManager.placedObject.UpdateTimelineObjects();
                // Update the bottom UI
                scriptManager.editorBottomMenu.UpdateSpawnTimeText(timelineHitObjectSpawnTime);
                scriptManager.editorBottomMenu.UpdateIDText(timelineObjectListIndex);
                break;
        }
    }

}
