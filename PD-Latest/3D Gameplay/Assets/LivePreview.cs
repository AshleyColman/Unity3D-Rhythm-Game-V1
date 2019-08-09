using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LivePreview : MonoBehaviour {

    // UI
    public GameObject previewHitObject; // Preview hit object prefabs
    public List<GameObject> instantiatedPreviewHitObjects = new List<GameObject>(); // All spawned preview hit objects
    public TextMeshProUGUI loopingOnText, loopingOffText; // Looping on and off text
    public TMP_InputField startTimeInputField, endTimeInputField; // Input fields for getting the time values
    public TextMeshProUGUI previewDurationText; // Duration of the preview 
    // Gameobject
    public GameObject previewHitObjectGlow; // Glow part of the preview hit object
    public GameObject previewHitObjectInner; // Inner part of the preview hit object


    // Animation
    public List<Animator> previewHitObjectAnimatorList = new List<Animator>();

    // Integers
    private int objectType; // Object type index
    public int spawnHitObjectIndex; // Index of the hit object to spawn next
    public int currentHitObjectIndex; // Calculated hit object index based on the current time of the timeline
    private float previewStartTime; // Time that starts the preview
    private float previewEndTime; // Time that ends the preview

    // Vectors
    Vector3 objectPosition; // Hit object positions

    // Bools
    public bool previewOn, previewPaused, previewOff, previewLooping;

    // Transform
    public Transform canvas; // Spawn location

    // Scripts
    private PlacedObject placedObject;
    private MetronomePro metronomePro;
    private MetronomePro_Player metronomePro_Player;

    public List<UIPreviewHitObject> UIPreviewHitObjectScriptList = new List<UIPreviewHitObject>();

    private void Start()
    {
        // Initalize
        previewOn = false;
        previewOff = true;
        previewPaused = false;

        // Reference
        placedObject = FindObjectOfType<PlacedObject>();
        metronomePro = FindObjectOfType<MetronomePro>();
        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();
    }

    private void Update()
    {
        // Preview is active
        if (previewOn == true && previewOff == false && previewPaused == false)
        {
            // Check if a preview hit object is to be spawned based on the current time of the song
            CheckIfReadyToSpawn();
        }

        // Check the loop if active and reset the preview if it has reached the end
        CheckPreviewLoop();
    }

    // Calculate the current preview object index based on the current time in the timeline
    private void CalculateCurrentPreviewObjectIndex()
    {
        // List starts at 0
        // Calculate the index we're at based on song time
        // Song time is 60 seconds
        // current index is 60
        // clear list/information
        // instantiate object 0 with the editorhitobject information from "current index: 60"



        // Check how many hit object spawn times we're ahead of
        // Only spawn the hit object for the NEXT hit object not the others 
        // Prevents spawning 50 or more etc hit objects if the user has clicked ahead in the timeline
        // For example clicking 1 minute in = calculate which hit object index we are to spawn next and don't spawn the previous objects



        // Object 1 time: 2
        // song time = 7
        // object 2 time = 10

        // check if greater
        // sets index to 1


        // Check the list if not empty
        if (placedObject.editorHitObjectList.Count != 0)
        {
            // Loop through all the editor hit objects saved
            for (int i = 0; i < placedObject.editorHitObjectList.Count; i++)
            {
                // Get the index of the next hit object
                if (metronomePro.songAudioSource.time > (placedObject.editorHitObjectList[i].hitObjectSpawnTime - 1))
                {
                    // Update the spawn hit object index to the index of the hit object where the current song time is not greater than = next note to spawn
                    currentHitObjectIndex = (i + 1);
                }
                else
                {
                    // Break to prevent the index from increasing after finding the next index to spawn
                    break;
                }                
            }
        }
    }

    // Update the current hit objects that should be displayed on the screen
    public void UpdatePreviewHitObjects()
    {
        // Clear all information
        ClearPreviewInformation();

        // Calculate the current preview object index based on the current time in the timeline
        CalculateCurrentPreviewObjectIndex();
    }

    // Start/Resume/Pause the live preview - toggle
    public void StartOrPauseLivePreview()
    {

        // If the preview is paused or the preview is currently off
        if (previewPaused == true || previewOff == true)
        {
            // Resume the live preview
            ResumeLivePreview();
        }
        // If preview is currently active
        else if (previewPaused == false)
        {
            // Pause the live preview
            PauseLivePreview();
        }
    }

    // Resume the live preview
    private void ResumeLivePreview()
    {
        // Resume the preview animations
        ResumePreviewAnimations();

        // Preview is now active
        previewOff = false;
        // Turn pause off
        previewPaused = false;
        // Resume the preview spawning
        previewOn = true;

        // Turn on the song
        metronomePro_Player.PlayOrPauseSong();
    }

    // Pause the live preview
    private void PauseLivePreview()
    {
        // Pause the preview hit object animations
        PausePreviewAnimations();

        // Turn pause on
        previewPaused = true;
        // Turn preview spawning off
        previewOn = false;

        // Pause the song
        metronomePro_Player.PlayOrPauseSong();
    }

    // Clear all preview information
    public void ResetPreviewFromStart()
    {
        // Clear all preview object information
        ClearPreviewInformation();

        // Play the song
        metronomePro_Player.StopSong();

        // Reset the time
        metronomePro.songAudioSource.time = 0f;

        // Calculate the next hit object to spawn based on the current song time
        CalculateCurrentPreviewObjectIndex();

        // Resume song
        StartOrPauseLivePreview();

        // Ensure preview is on and not paused
        previewOn = true;
        previewPaused = false;
    }

    // Clear all preview information
    private void ClearPreviewInformation()
    {
        spawnHitObjectIndex = 0;
        currentHitObjectIndex = 0;

        /*
        previewOn = false;
        previewPaused = false;
        previewOff = true;
        */

        DestroyAllPreviewObjects();
        previewHitObjectAnimatorList.Clear();
        instantiatedPreviewHitObjects.Clear();
        UIPreviewHitObjectScriptList.Clear();
    }

    // Destroy all preview objects on screen
    private void DestroyAllPreviewObjects()
    {
        for (int i = 0; i < instantiatedPreviewHitObjects.Count; i++)
        {
            Destroy(instantiatedPreviewHitObjects[i]);
        }
    }

    // Check if a preview hit object is to be spawned based on the current time of the song
    private void CheckIfReadyToSpawn()
    {
        // Check if hit objects exist to preview
        if (placedObject.editorHitObjectList.Count != 0)
        {
            // If the audio is playing
            if (metronomePro.songAudioSource.isPlaying == true)
            {
                // Only spawn if there are objects to spawn
                if (currentHitObjectIndex < placedObject.editorHitObjectList.Count)
                {
                    // Check spawn time for hit object at the current index (-1 to consider spawn in time)
                    if (metronomePro.songAudioSource.time >= (placedObject.editorHitObjectList[currentHitObjectIndex].hitObjectSpawnTime - 1))
                    {
                        // Get the object type for the preview object
                        objectType = placedObject.editorHitObjectList[currentHitObjectIndex].hitObjectType;

                        // Get the object position for the preview object
                        objectPosition = placedObject.editorHitObjectList[currentHitObjectIndex].hitObjectPosition;

                        // Instantiate a preview hit object and add to the list
                        SpawnPreviewHitObject(spawnHitObjectIndex, objectType, objectPosition);

                        // Increment for the next preview object to spawn
                        spawnHitObjectIndex++;
                        currentHitObjectIndex++;
                    }
                }
            }
        }

    }

    // Instantiate a preview hit object and add to the list
    private void SpawnPreviewHitObject(int _index, int _previewObjectType, Vector3 _previewObjectPosition)
    {
        // Instantiate
        GameObject obj = Instantiate(previewHitObject, Vector3.zero, Quaternion.identity);

        // Update the parent spawn to be in the canvas
        obj.transform.SetParent(canvas, false);

        // Update the position
        obj.transform.position = _previewObjectPosition;

        // Update the rotation
        obj.transform.rotation = Quaternion.Euler(-90, 0, 45);

        // Add to instantiated objects list
        instantiatedPreviewHitObjects.Add(obj);

        // Add the preview hit objects animator to the list to control animation playback
        previewHitObjectAnimatorList.Add(obj.GetComponent<Animator>());

        // Add the UIPreviewHitObject script attached to the preview hit object to the list for controlling pausing deactivation
        UIPreviewHitObjectScriptList.Add(obj.GetComponent<UIPreviewHitObject>());

        // Update the color of the glow and inner components
        ChangePreviewObjectColor(_index, _previewObjectType);
    }

    // Pause preview animations
    private void PausePreviewAnimations()
    {
        // Check if there are instantiated objects to pause their animations
        if (instantiatedPreviewHitObjects.Count != 0)
        {
            // Loop through all preview hit object animators and pause the animation
            for (int i = 0; i < previewHitObjectAnimatorList.Count; i++)
            {
                // If the game object is currently active 
                if (instantiatedPreviewHitObjects[i].gameObject.activeSelf == true)
                {
                    // Set speed to 0 to pause the animation
                    previewHitObjectAnimatorList[i].speed = 0;

                    // Pause deactivation of the preview hit object
                    UIPreviewHitObjectScriptList[i].Paused = true;
                }
            }
        }
    }

    // Resume preview animations
    private void ResumePreviewAnimations()
    {
        // Check if objects have been instantiated to resume their animations
        if (instantiatedPreviewHitObjects.Count != 0)
        {
            // Loop through all preview hit object animators and resume the animation
            for (int i = 0; i < previewHitObjectAnimatorList.Count; i++)
            {
                // If the game object is currently active 
                if (instantiatedPreviewHitObjects[i].gameObject.activeSelf == true)
                {
                    // Set speed to 0 to pause the animation
                    previewHitObjectAnimatorList[i].speed = 1;

                    // Resume activation of the preview hit object to allow the timer to continue for deactivation
                    UIPreviewHitObjectScriptList[i].Paused = false;
                }
            }
        }
    }

    // Change the preview object color
    public void ChangePreviewObjectColor(int _index, int _objectType)
    {
        // Blue, purple, red, green, yellow, orange

        // If the glow and inner component exists
        if (UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage != null && UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage
            != null)
        {
            // Change the preview hit object component colors based on the object type value
            switch (_objectType)
            {
                case 0:
                    // Change the glow and inner components of the preview hit object color to blue
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.blueObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.blueObjectColor;
                    break;
                case 1:
                    // Purple
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.purpleObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.purpleObjectColor;
                    break;
                case 2:
                    // Red
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.redObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.redObjectColor;
                    break;
                case 3:
                    // Green
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.greenObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.greenObjectColor;
                    break;
                case 4:
                    // Yellow
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.yellowObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.yellowObjectColor;
                    break;
                case 5:
                    // Orange
                    UIPreviewHitObjectScriptList[_index].previewHitObjectGlowImage.color = placedObject.orangeObjectColor;
                    UIPreviewHitObjectScriptList[_index].previewHitObjectInnerImage.color = placedObject.orangeObjectColor;
                    break;
            }
        }
    }

    // Reset preview start time from the beginning of the song
    public void ResetPreviewStartTime()
    {
        previewStartTime = 0f;

        // Update the preview duration text
        UpdatePreviewDurationText();
    }

    // Reset preview end time to the end of the song
    public void ResetPreviewEndTime()
    {
        if (metronomePro.songAudioSource.clip != null)
        {
            previewEndTime = metronomePro.songAudioSource.clip.length;

            // Update the preview duration text
            UpdatePreviewDurationText();
        }
    }

    // Update the preview duration text
    private void UpdatePreviewDurationText()
    {
        previewDurationText.text = "Start: " + previewStartTime.ToString("F2") + " - End: " + previewEndTime.ToString("F2");
    }

    // Manually enter the preview start time
    public void ManuallySetPreviewStartTime()
    {
        // Convert the text to float value
        var newStartTimeFloat = float.Parse(startTimeInputField.text);

        // Check the time passed is within the song time limits
        if (newStartTimeFloat < metronomePro.songAudioSource.clip.length)
        {
            // Update the previewStartTime with the value passed
            previewStartTime = newStartTimeFloat;

            // Update the preview duration text
            UpdatePreviewDurationText();
        }
    }

    // Set the preview start time based on the current time of the song
    public void SetPreviewStartTime()
    {
        // Check if a song clip has been assigned
        if (metronomePro.songAudioSource.clip != null)
        {
            // Check the time is within the song time limits
            if (metronomePro.songAudioSource.time < metronomePro.songAudioSource.clip.length)
            {
                // Update the previewStartTime with the current song time
                previewStartTime = metronomePro.songAudioSource.time;

                // Update the preview duration text
                UpdatePreviewDurationText();
            }
        }
    }

    // Manually enter the preview end time
    public void ManuallySetPreviewEndTime()
    {
        // Convert the text to float value
        var newEndTimeFloat = float.Parse(endTimeInputField.text);


        // Check the time passed is within the song time limits
        if (newEndTimeFloat < metronomePro.songAudioSource.clip.length)
        {
            // Update the previewEndTime with the value passed
            previewEndTime = newEndTimeFloat;

            // Update the preview duration text
            UpdatePreviewDurationText();
        }
    }

    // Set the preview end time based on the current time of the song
    public void SetPreviewEndTime()
    {
        // Check if a song clip has been assigned
        if (metronomePro.songAudioSource.clip != null)
        {
            // Check the time is within the song time limits
            if (metronomePro.songAudioSource.time < metronomePro.songAudioSource.clip.length)
            {
                // Update the previewStartTime with the current song time
                previewEndTime = metronomePro.songAudioSource.time;

                // Update the preview duration text
                UpdatePreviewDurationText();
            }
        }
    }

    // Allow the preview to loop continously
    private void TurnOnPreviewLoop()
    {
        previewLooping = true;

        // Update the text
        loopingOffText.gameObject.SetActive(false);
        loopingOnText.gameObject.SetActive(true);
    }

    // Turn off preview looping
    private void TurnOffPreviewLoop()
    {
        previewLooping = false;


        // Update the text
        loopingOffText.gameObject.SetActive(true);
        loopingOnText.gameObject.SetActive(false);
    }

    // Turn on or off looping
    public void ToggleLoop()
    {
        // If looping is currently off, set to true
        if (previewLooping == false)
        {
            // Allow the preview to loop continously
            TurnOnPreviewLoop();
        }
        else if (previewLooping == true)
        {
            // Turn off preview looping
            TurnOffPreviewLoop();
        }
    }

    // Play the preview from the manually entered start time 
    public void PlayPreviewFromSetStartTime()
    {
        // Clear all preview object information
        ClearPreviewInformation();

        // Stop the song
        metronomePro_Player.StopSong();

        // Reset the time
        metronomePro.songAudioSource.time = previewStartTime;

        // Calculate the next hit object to spawn based on the current song time
        CalculateCurrentPreviewObjectIndex();

        // Resume song
        StartOrPauseLivePreview();

        // Ensure preview is on and not paused
        previewOn = true;
        previewPaused = false;
    }

    // Check the loop if active and reset the preview if it has reached the end
    private void CheckPreviewLoop()
    {
        // If preview looping is enabled
        if (previewLooping == true)
        {
            // If the current song time has gone past the preview end time
            if (metronomePro.songAudioSource.time >= previewEndTime)
            {
                // Restart the preview from the start time
                PlayPreviewFromSetStartTime();
            }
        }
    }

}
