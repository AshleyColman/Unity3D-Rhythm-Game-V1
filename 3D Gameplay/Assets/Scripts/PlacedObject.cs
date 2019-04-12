using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedObject : MonoBehaviour {

    public GameObject[] editorPlacedHitObjects = new GameObject[3];
    public MouseFollow mouseFollow; // Get the position of the mouse when pressed for placement
    int totalEditorHitObjects = 0;
    List<Vector3> editorHitObjectPositions = new List<Vector3>();
    Vector3 instantiatePosition;
    public bool hasClickedUIButton = false;
    public SongProgressBar songProgressBar;
    public int editorPlacedHitObjectType;
    private int specialTimeKeyPresses;
    public Image backgroundImage; // To spawn during special time
    public Text instructionButtonText; // The instruction button text
    public Animator instructionButtonAnimation; // Animate the instruction button text
    public bool startSongTimer;
    public float songTimer;
    private EditorSoundController editorSoundController; // The editorSoundController

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

    private bool uiActive; // Used for controlling the UI hide and show

    // Use this for initialization
    void Start () {
        uiActive = true;
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (uiActive == true)
            {
                // If the tab key has been pressed disable all UI buttons to increase the charting area space
                placeButton.gameObject.SetActive(false);
                saveButton.gameObject.SetActive(false);
                instructionButton.gameObject.SetActive(false);
                resetButton.gameObject.SetActive(false);
                // Set uiActive to false
                uiActive = false;
            }
            else if (uiActive == false)
            {
                // If the tab key has been pressed enable all UI buttons
                placeButton.gameObject.SetActive(true);
                saveButton.gameObject.SetActive(true);
                instructionButton.gameObject.SetActive(true);
                resetButton.gameObject.SetActive(true);
                // Set uiActive to true
                uiActive = true;
            }
        }

        // Place a hit object only if the mouse has been clicked and the UI button has been clicked
        if (hasClickedUIButton == true)
        {
            // Blue Key Pressed
            if (Input.GetKeyDown(KeyCode.J))
            {
                // Set the type to BLUE as the J key has been pressed
                editorPlacedHitObjectType = 0;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }
            // Purple Key Pressed
            else if (Input.GetKeyDown(KeyCode.K))
            {
                // Set the type to PURPLE as the K key has been pressed
                editorPlacedHitObjectType = 1;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }
            // Red Key Pressed
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Set the type to RED as the L key has been pressed
                editorPlacedHitObjectType = 2;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }
            // Green Key Pressed
            if (Input.GetKeyDown(KeyCode.U))
            {
                // Set the type to GREEN as the U key has been pressed
                editorPlacedHitObjectType = 3;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }
            // Yellow Key Pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Set the type to YELLOW as the I key has been pressed
                editorPlacedHitObjectType = 4;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }
            // Orange Key Pressed
            if (Input.GetKeyDown(KeyCode.O))
            {
                // Set the type to ORANGE as the O key has been pressed
                editorPlacedHitObjectType = 5;
                // Spawn and save the placed object information in the beatmap file
                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
                // Play the placed sound effect
                editorSoundController.PlayPlacedSound();
            }

            if (Input.GetKeyDown(KeyCode.Q) && hasCreatedLeaderboard == false)
            {
                // Set created leaderboard to true
                hasCreatedLeaderboard = true;
                // Update the instruction button text and play animation
                UpdateInstructionButtonText("LeaderboardCreated");
                // Play the specialTimeFirstPlaced sound effect
                editorSoundController.PlaySpecialTimeStartPlacedSound();
                // Set the save button to interactable
                saveButton.interactable = true;
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
        instantiatePosition = mouseFollow.pos;
        InstantiateEditorPlacedHitObject(instantiatePosition, editorPlacedHitObjectType);
        // Store the time spawned and position of the object
        editorHitObjectPositions.Add(mouseFollow.pos);
        // Add to total
        totalEditorHitObjects += 1;


        // Save object position to the list?
        Database.database.PositionX.Add(mouseFollow.pos.x);
        Database.database.PositionY.Add(mouseFollow.pos.y);
        Database.database.PositionZ.Add(mouseFollow.pos.z);

        // Save object spawn time
        Database.database.HitObjectSpawnTime.Add(songTimer);

        // Save object type
        Database.database.ObjectType.Add(editorPlacedHitObjectType);
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
            instructionButtonText.text = "Press H to Start Special Time";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "HKeyPressedOnce")
        {
            // Update the instruction button text
            instructionButtonText.text = "Press H to End Special Time";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "HKeyPressedTwice")
        {
            // Update the instruction button text
            instructionButtonText.text = "Press Q to create a leaderboard";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "LeaderboardCreated")
        {
            // Update the instruction button text
            instructionButtonText.text = "Save Your Beatmap When Finished";
            // Do instruction button animation
            instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        }
        else if (actionPass == "SaveButtonPressed")
        {
            // Update the instruction button text
            instructionButtonText.text = "Beatmap is Saved";
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
        instructionButtonText.text = "Press Space to Start Live Mapping";
        // Do instruction button animation
        instructionButtonAnimation.Play("EditorInstructionButtonAnimation");
        // Reset specialTimeKeyPresses
        specialTimeKeyPresses = 0;
    }
}
