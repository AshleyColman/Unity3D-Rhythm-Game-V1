using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlacedObject : MonoBehaviour {

    public GameObject[] editorPlacedHitObjects = new GameObject[3];
    public MouseFollow mouseFollow; // Get the position of the mouse when pressed for placement
    int totalEditorHitObjects = 0;
    List<Vector3> editorHitObjectPositions = new List<Vector3>();
    List<float> editorHitObjectSpawnTimes = new List<float>();
    List<int> editorPlacedHitObjectTypeList = new List<int>();
    int editorPlacedHitObjectType;
    
    Vector3 instantiatePosition;
    public bool hasClickedUIButton = false;
    public SongProgressBar songProgressBar;
    
    private int specialTimeKeyPresses;
    public Image backgroundImage; // To spawn during special time
    public TextMeshProUGUI instructionButtonText; // The instruction button text
    public Animator instructionButtonAnimation; // Animate the instruction button text
    public bool startSongTimer;
    public float songTimer;
    private EditorSoundController editorSoundController; // The editorSoundController

    public GameObject editorHitObject; // Editor hit object used for tracking the position and saving the position

    // Get the reference to the beatmap setup to disable starting the song when space is pressed whilst in the editor
    public BeatmapSetup beatmapSetup;

    // Used for tracking when the spacebar has been pressed to start the song and timer
    private bool hasPressedSpacebar;

    // Used for only allowing the user to create a leaderboard once
    private bool hasCreatedLeaderboard;

    // Used for only allowing the user to save the beatmap once
    private bool hasSaved;

    // Save button for enabling and disabling when a leaderboard has been created
    public Button saveButton;

    // Other UI elements for disabling during live mapping
    public Button instructionButton;
    public Button resetButton;
    public Button placeButton;

    // Keys used bools
    public bool pressedKeyS;
    public bool pressedKeyD;
    public bool pressedKeyF;
    public bool pressedKeyJ;
    public bool pressedKeyK;
    public bool pressedKeyL;


    public Vector3 timelineObjectPosition;

    public GameObject[] instantiatedTimelineObject = new GameObject[6];
    // List of instantiated timeline objects that are added to the list when instantiated
    public List<GameObject> instantiatedTimelineObjectList = new List<GameObject>();
    private int instantiatedTimelineObjectType;
    // The index for all editor objects, increases by 1 everytime one is instantiated
    private int timelineObjectIndex;


    float handlePositionX;
    float handlePositionY = 9999;
    float handlePositionZ;
    private Vector3 handlePosition;

    // Reference to the destroy timeline script for assigning the index to the timeline object and removing it 
    DestroyTimelineObject destroyTimelineObject;

    // Reference to the metronome player to get the current song time, position of the handle and slide value for placed diamond bars on the timeline
    MetronomePro_Player metronomePro_Player;



    // Use this for initialization
    void Start () {

        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();

        pressedKeyS = false;
        pressedKeyD = false;
        pressedKeyF = false;
        pressedKeyJ = false;
        pressedKeyK = false;
        pressedKeyL = false;


        hasPressedSpacebar = false;
        startSongTimer = false;
        songTimer = 0f;
        specialTimeKeyPresses = 0;
        backgroundImage.enabled = false;
        mouseFollow = FindObjectOfType<MouseFollow>();
        songProgressBar = FindObjectOfType<SongProgressBar>();
        editorSoundController = FindObjectOfType<EditorSoundController>(); // Reference to the editor sound controller
        beatmapSetup = FindObjectOfType<BeatmapSetup>(); // Required for disabling the animation whilst the spacebar is pressed in the setup screen
    }
	
	// Update is called once per frame
	void Update () {

        // Find the editor hit object in the editor scene once instantiated
        if (editorHitObject == null)
        {
            editorHitObject = GameObject.FindGameObjectWithTag("EditorHitObject");

        }

        if (beatmapSetup.settingUp == false && hasPressedSpacebar == false)
        {
            // Check if live mapping has started
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Has pressed the spacebar
                hasPressedSpacebar = true;

                // Update the instruction button text and play animation
                UpdateInstructionButtonText("SpacebarPressed");

                // If the space key has been pressed we start the song and song timer
                startSongTimer = true;
            }
        }


        // Update the song timer with the current song time
        if (startSongTimer == true)
        {
            songTimer += Time.deltaTime;
        }

        // Place a hit object only if the mouse has been clicked and the UI button has been clicked
        if (hasClickedUIButton == true)
        {
            // Blue Key Pressed
            if (Input.GetKeyDown(KeyCode.J))
            {
                // Set has pressed J to true
                pressedKeyJ = true;

                // Set the type to BLUE as the J key has been pressed
                editorPlacedHitObjectType = 0;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 3;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }
            // Purple Key Pressed
            else if (Input.GetKeyDown(KeyCode.K))
            {
                // Set has pressed K to true
                pressedKeyK = true;

                // Set the type to PURPLE as the K key has been pressed
                editorPlacedHitObjectType = 1;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 4;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }
            // Red Key Pressed
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Set has pressed L to true
                pressedKeyL = true;

                // Set the type to RED as the L key has been pressed
                editorPlacedHitObjectType = 2;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 5;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }
            // Green Key Pressed
            if (Input.GetKeyDown(KeyCode.U))
            {
                // Set has pressed S to true
                pressedKeyS = true;

                // Set the type to GREEN as the U key has been pressed
                editorPlacedHitObjectType = 1;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 0;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }
            // Yellow Key Pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Set has pressed D to true
                pressedKeyD = true;

                // Set the type to YELLOW as the I key has been pressed
                editorPlacedHitObjectType = 4;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 1;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }
            // Orange Key Pressed
            if (Input.GetKeyDown(KeyCode.O))
            {
                // Set has pressed F to true
                pressedKeyF = true;

                // Set the type to ORANGE as the O key has been pressed
                editorPlacedHitObjectType = 5;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();

                // Assign the timeline type and instantiate it on the timeline
                instantiatedTimelineObjectType = 2;
                InstantiateTimelineObject(instantiatedTimelineObjectType);
            }

            // Special Time Key Press Set Times
            if (Input.GetKeyDown(KeyCode.H))
            {
                // Increase the special time key presses to know when it's started and ended
                specialTimeKeyPresses++;

                // If the first press set the start time for special time
                if (specialTimeKeyPresses == 1)
                {
                    // Activate the border image
                    ActivateBorder();
                    // Mark the special time start
                    SetSpecialTimeStart();
                    // Update the instruction button text and play animation
                    UpdateInstructionButtonText("HKeyPressedOnce");
                    // Play the specialTimeFirstPlaced sound effect
                    editorSoundController.PlaySpecialTimeStartPlacedSound();
                }
                // The the second time set the special time end
                if (specialTimeKeyPresses == 2)
                {
                    // Deactive the border image
                    DeActivateBorder();
                    // Mark the special time end
                    SetSpecialTimeEnd();
                    // Update the instruction button text and play animation
                    UpdateInstructionButtonText("HKeyPressedTwice");
                    // Play the second specialTimeSecondPlaced sound effect
                    editorSoundController.PlaySpecialTimeEndPlacedSound();
                }
            }
        }
    }


    // Removes the timeline object from the list
    public void RemoveTimelineObject()
    {

        // Do a check on the list and find the null object, store the index and remove from all the lists
        int nullTimelineObjectIndex = 0;
        bool nullWasFound = false; 

        for (int i = 0; i < instantiatedTimelineObjectList.Count; i++)
        {

            // Check if any objects are null in the list
            if (instantiatedTimelineObjectList[i] == null)
            {
                // Set the index to the null object index found
                nullTimelineObjectIndex = i;
                // Set null was found to true
                nullWasFound = true;
            }
        }

        // If a null object was found remove from the list
        if (nullWasFound == true)
        {
            // Remove the
            instantiatedTimelineObjectList.RemoveAt(nullTimelineObjectIndex);
            // Remove the object positions tied to the timeline object
            editorHitObjectPositions.RemoveAt(nullTimelineObjectIndex);
            // Remove the spawn time tied to the timeline object
            editorHitObjectSpawnTimes.RemoveAt(nullTimelineObjectIndex);
            // Remove the type tied to the timeline object
            editorPlacedHitObjectTypeList.RemoveAt(nullTimelineObjectIndex);

            Debug.Log("Removed at: " + nullTimelineObjectIndex);
        }
    }

    // Instantiate a timeline object at the current song time
    public void InstantiateTimelineObject(int instantiatedTimelineObjectTypePass)
    {
        // Get the handle position currently in the song to spawn the timeline object at
        handlePositionX = metronomePro_Player.songPointSliderHandle.transform.position.x;
        // Decrease the Y position to prevent overlap
        handlePositionY = 0;
        handlePositionZ = metronomePro_Player.songPointSliderHandle.transform.position.z;

        // Assign the new position
        handlePosition = new Vector3(handlePositionX, handlePositionY, handlePositionZ);

        // Instantiate the type of object
        GameObject timelineObject = Instantiate(instantiatedTimelineObject[instantiatedTimelineObjectTypePass], handlePosition,
        Quaternion.Euler(90, 0, 0), GameObject.FindGameObjectWithTag("Timeline").transform);

        Slider timelineSlider = timelineObject.GetComponent<Slider>();

        
        // Add the instantiated timeline object to the list of instantiated timeline objects
        instantiatedTimelineObjectList.Add(timelineObject);
        timelineSlider.value = metronomePro_Player.handleSlider.value;

        // Increase the timeline object index
        timelineObjectIndex++;


    }

    // Instantiate placed hit object at the position on the mouse
    public void InstantiateEditorPlacedHitObject(Vector3 instantiatePositionPass, int editorHitObjectTypePass)
    {
        Instantiate(editorPlacedHitObjects[editorHitObjectTypePass], instantiatePositionPass, Quaternion.Euler(0, 45, 0));
    }

    public void HasClickedUIButton()
    {
        // The button has been clicked enable positioning of objects
        hasClickedUIButton = true;
    }

    public void SpawnAndSavePlacedObject(int editorPlacedHitObjectType)
    {
        // Set a new vector 3 based off the editor hit object position in the scene
        float x = editorHitObject.transform.position.x;
        float y = 10;
        float z = editorHitObject.transform.position.z;


        // Set the instantiate position to the editor hit object position but with a Y of 0
        instantiatePosition = new Vector3(x, y, z);
        InstantiateEditorPlacedHitObject(instantiatePosition, editorPlacedHitObjectType);
        // Store the time spawned and position of the object
        editorHitObjectPositions.Add(instantiatePosition);
        // Add to total
        totalEditorHitObjects += 1;

        // Add the current song timer (when the user clicked) as the spawn time for the instantiated editor object
        editorHitObjectSpawnTimes.Add(songTimer);

        // Add the object type to the object type list
        editorPlacedHitObjectTypeList.Add(editorPlacedHitObjectType);




        // Save object position to the list?
        // Database.database.PositionX.Add(instantiatePosition.x);
        // Database.database.PositionY.Add(instantiatePosition.y);
        // Database.database.PositionZ.Add(instantiatePosition.z);

        // Save object spawn time
        //Database.database.HitObjectSpawnTime.Add(songTimer);

        // Save object type
        //Database.database.ObjectType.Add(editorPlacedHitObjectType);
    }

    // Set the special time start when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeStart()
    {
        Database.database.SpecialTimeStart = songTimer;
    }

    // Set the special time end when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeEnd()
    {
        Database.database.SpecialTimeEnd = songTimer;
    }

    // Display the special time border during mapping when the key has been pressed
    public void ActivateBorder()
    {
        backgroundImage.enabled = true;
    }

    // Deactivate the special time border 
    public void DeActivateBorder()
    {
        backgroundImage.enabled = false;
    }

    // When the save button has been clicked update the instruction button text and play animation
    public void UpdateInstructionButtonText(string actionPass)
    {
        if (actionPass == "SpacebarPressed")
        {
            // Update the instruction button text
            instructionButtonText.text = "PRESS H TO START SPECIAL TIME";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "HKeyPressedOnce")
        {
            // Update the instruction button text
            instructionButtonText.text = "Press H TO END SPECIAL TIME";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "HKeyPressedTwice")
        {
            // Update the instruction button text
            instructionButtonText.text = "PRESS 'FINISHED' WHEN COMPLETE";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "LeaderboardCreated")
        {
            // Update the instruction button text
            instructionButtonText.text = "SELECT A DIFFICULTY TYPE";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "DifficultyTypeSelected")
        {
            // Update the instruction button text
            instructionButtonText.text = "SELECT A DIFFICULTY LEVEL";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "DifficultyLevelSelected")
        {
            // Update the instruction button text
            instructionButtonText.text = "SAVE YOUR BEATMAP";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "SaveButtonPressed")
        {
            // Update the instruction button text
            instructionButtonText.text = "BEATMAP SAVED";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
    }

    // Reset the song timer when clear button has been pressed in the editor
    public void ResetSongTimer()
    {
        // Reset the song time and spacebar pressed
        hasPressedSpacebar = false;
        songTimer = 0f;
        startSongTimer = false;
        // Update the instruction button text
        instructionButtonText.text = "PRESS SPACE TO START LIVE MAPPING";
        // Do instruction button animation
        instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        // Reset specialTimeKeyPresses
        specialTimeKeyPresses = 0;
    }

    // Disable the save button
    public void DisableSaveButton()
    {
        saveButton.interactable = false;
    }

    // Reset keys pressed if the map has been reset
    public void ResetKeysPressed()
    {
        pressedKeyS = false;
        pressedKeyD = false;
        pressedKeyF = false;
        pressedKeyJ = false;
        pressedKeyK = false;
        pressedKeyL = false;
    }
}
