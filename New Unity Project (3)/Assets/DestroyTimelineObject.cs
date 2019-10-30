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
    public GameObject instantiatedEditableHitObject;
    public Image selectedDiamondImage, diamondImage;

    // Integers
    public int timelineObjectListIndex;
    private float timelineHitObjectSpawnTime;
    private float hitObjectSliderValue;
    public float lastSavedSliderValue; // Last saved slider value before being moved on the timeline
    private float nearest;

    // Bools
    private bool previousFrameMouseHeldDown;
    private bool previousFrameBeatsnapValueTaken;

    // Audio
    private AudioSource menuSFXAudioSource;

    // Scripts
    private PlacedObject placedObject;
    private MetronomePro_Player metronome_Player;
    private BeatsnapManager beatsnapManager;
    private EditableHitObject editableHitObject;
    private EditorUIManager editorUIManager;


    // Properties

    public float TimelineHitObjectSpawnTime
    {
        set { timelineHitObjectSpawnTime = value; }
        get { return timelineHitObjectSpawnTime; }
    }

    private void Start()
    {
        // Initialize
        // Reference
        beatsnapManager = FindObjectOfType<BeatsnapManager>();
        placedObject = FindObjectOfType<PlacedObject>();
        metronome_Player = FindObjectOfType<MetronomePro_Player>();
        editorUIManager = FindObjectOfType<EditorUIManager>();

        menuSFXAudioSource = GameObject.FindGameObjectWithTag("MenuSFXAudioSource").GetComponentInChildren<AudioSource>();

        timelineSlider = this.gameObject.GetComponent<Slider>(); // Get the reference to the timelines own slider
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
        // Get reference if null
        GetReferenceToEditorUIManager();

        // If the preview panel is not active
        if (editorUIManager.previewPanel.gameObject.activeSelf == false)
        {
            // If left click
            if (Input.GetMouseButton(0) || _trigger == "TIMELINE_NAVIGATION")
            {
                // Check if the editable hit object has been found
                if (instantiatedEditableHitObject == null)
                {

                    // Get the reference
                    instantiatedEditableHitObject = editorUIManager.instantiatedEditableHitObject;
                }

                // If exists
                if (instantiatedEditableHitObject != null)
                {
                    // Activate
                    instantiatedEditableHitObject.gameObject.SetActive(true);

                    // Check if the script has been referenced
                    if (editableHitObject == null)
                    {
                        // Get the reference
                        editableHitObject = FindObjectOfType<EditableHitObject>();
                    }

                    // If script exists
                    if (editableHitObject != null)
                    {
                        // Update the object index with the timeline object index
                        editableHitObject.ObjectIndex = timelineObjectListIndex;

                        // Setup the position and color of the hit object
                        editableHitObject.SetupEditorObject();

                        // Change the current selected timeline object button diamond color to be selected
                        editableHitObject.ChangeTimelineObjectSelectedColor(selectedDiamondImage);

                        // Update the current timeline object image to change when changing object colors
                        editableHitObject.UpdateTimelineObjectDiamondImage(diamondImage);
                    }
                }

                // Assign the timeline object reference for the editable hit object
                editableHitObject.timelineObject = this.gameObject;

                // Activate the properties panel
                editorUIManager.ActivateObjectPropertiesPanel();
            }
        }
    }

    // Update the number text on the timeline hit object
    public void UpdateNumberText(int _value)
    {
        // If the value is different than what the text is already set to 
        if (numberText.text != _value.ToString())
        {
            // Update the text
            numberText.text = _value.ToString();

            // Check if exists
            if (editableHitObject != null)
            {
                // If the instantiated editable hit object is active for this timeline object
                if (editableHitObject.ObjectIndex == timelineObjectListIndex)
                {
                    // Update the text for the editable hit object
                    editableHitObject.UpdateEditableObjectIndex(_value);
                }
            }
        }
    }


    // Deactivate the instantiated editable hit object
    public void DeactivateEditableHitObject()
    {
        instantiatedEditableHitObject.gameObject.SetActive(false);

        // Deactive the editable object properties panel
        editorUIManager.DisplayCurrentIndexPanel();
    }

    // Get the reference to the editor ui manager
    public void GetReferenceToEditorUIManager()
    {
        if (editorUIManager == null)
        {
            editorUIManager = FindObjectOfType<EditorUIManager>();
        }
    }

    // Play deleted sound
    public void PlayDeletedSound()
    {
        GetReferenceToEditorUIManager()
            ;
        menuSFXAudioSource.PlayOneShot(editorUIManager.timelineObjectDeletedClip);
    }

    // Play the selected sound
    public void PlaySelectedSound()
    {
        GetReferenceToEditorUIManager()
    ;
        menuSFXAudioSource.PlayOneShot(editorUIManager.timelineObjectSelectedClip);
    }

    // Play clicked sound
    public void PlayClickedSound()
    {
        GetReferenceToEditorUIManager()
    ;
        menuSFXAudioSource.PlayOneShot(editorUIManager.timelineObjectClickedClip);
    }

    public void DestroyEditorTimelineObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (instantiatedEditableHitObject != null)
            {
                // Deactivate editable hit object
                DeactivateEditableHitObject();
            }

            // Play deleted sound effect
            PlayDeletedSound();
            // Check null timeline objects and update the list order/remove null objects from all lists
            placedObject.RemoveTimelineObject(timelineObjectListIndex);
            // Update the list orders
            placedObject.SortListOrders();
            // Update all timeline objects
            placedObject.UpdateTimelineObjects();
            // Destroy the game object
            Destroy(this.gameObject);
        }
    }

    // Update the timelines spawn time
    public void UpdateTimelineHitObjectSpawnTime()
    {
        if (metronome_Player != null)
        {
            timelineHitObjectSpawnTime = metronome_Player.UpdateTimelineHitObjectSpawnTimes(timelineSlider);
            // Send the new spawn time and the editorHitObject index to update
            placedObject.UpdateEditorHitObjectSpawnTime(timelineHitObjectSpawnTime, timelineObjectListIndex);
        }
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
            nearest = beatsnapManager.beatsnapSliderValueList.Select(p => new { Value = p, Difference = Math.Abs(p - hitObjectSliderValue) })
                      .OrderBy(p => p.Difference)
                      .First().Value;

            bool nearestBeatTaken = false;

            // Check if another hit object has that value
            for (int i = 0; i < placedObject.instantiatedTimelineObjectList.Count; i++)
            {
                Slider timelineObjectSlider = placedObject.instantiatedTimelineObjectList[i].GetComponent<Slider>();

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
