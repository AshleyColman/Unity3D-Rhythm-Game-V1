using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class PlacedObject : MonoBehaviour {

    public GameObject[] editorPlacedHitObjects = new GameObject[3];
    public MouseFollow mouseFollow; // Get the position of the mouse when pressed for placement

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

    // Get the reference to the PlaceObject script, used for disabling the spawned editor hit object ghost
    private PlaceObject placeObject;

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

    // The handle position on the current song play bar
    float handlePositionX;
    float handlePositionY = 9999;
    float handlePositionZ;
    private Vector3 handlePosition;

    // Reference to the metronome player to get the current song time, position of the handle and slide value for placed diamond bars on the timeline
    MetronomePro_Player metronomePro_Player;

    // The timeline object that was clicked on in the timeline bar
    private GameObject raycastTimelineObject;

    // The index of the timeline bar clicked in the editor, used to delete and update existing notes spawn times, position etc by getting the index on click
    public int raycastTimelineObjectListIndex;

    // Used to check if a timeline bar has been clicked, instantiating a hitobject to appear on screen, if another timeline bar is pressed
    public bool instantiatedEditorHitObjectExists;

    // The instantiated editor hit object that is added to the scene when a timeline bar has been clicked
    GameObject instantiatedEditorHitObject;

    // The materials to change to when the instantiatedEditorHitObject's color button has been pressed
    public Material greenEditorHitObjectMaterial;
    public Material yellowEditorHitObjectMaterial;
    public Material orangeEditorHitObjectMaterial;
    public Material blueEditorHitObjectMaterial;
    public Material purpleEditorHitObjectMaterial;
    public Material redEditorHitObjectMaterial;

    // Instantiated timeline bar image colors
    public Color greenTimelineBarColor;
    public Color yellowTimelineBarColor;
    public Color orangeTimelineBarColor;
    public Color blueTimelineBarColor;
    public Color purpleTimelineBarColor;
    public Color redTimelineBarColor;

    // The list of preview hit objects that have been spawned when the preview button has been pressed and the song timer has reached the spawn time for the hit object
    private List<GameObject> previewHitObjectList = new List<GameObject>();
    // The preview hit objects to instantiate
    public GameObject[] previewHitObjects = new GameObject[6];
    // Index used for tracking and spawning the preview hit objects
    private int previewHitObjectIndex;
    // The type of the preview hit object to be spawned
    private int previewHitObjectType;
    // The position of the preview hit object to be spawned
    private Vector3 previewHitObjectPosition;
    // The spawn time of the preview hit object to be spawned
    private float previewHitObjectSpawnTime;
    // Used for controlling when to play the beatmap preview
    public bool playBeatmapPreview;

    // The special start time timeline object that is instantiated
    public GameObject instantiatedSpecialTimeStartObject;
    // The special end time timeline object that is instantiated 
    public GameObject instantiatedSpecialTimeEndObject;

    // List of editorHitObjects (includes spawn time, object type and positions)
    public List<EditorHitObject> editorHitObjectList = new List<EditorHitObject>();
    // List of positions, spawn times and type for all hit objects
    public List<Vector3> editorHitObjectPositionList = new List<Vector3>(); // The editorHitObject positions
    List<float> xPositionList = new List<float>(); // The editorHitObject x position
    List<float> yPositionList = new List<float>(); // The editorHitObject y position
    List<float> zPositionList = new List<float>(); // The editorHitObject z position
    public List<float> spawnTimeList = new List<float>(); // The editorHitObject spawntime
    public List<int> objectTypeList = new List<int>(); // The editorHitObject type

    bool hasSpawnedAllPreviewHitObjects; // Used for stopping the spawn of preview hit objects

    private MetronomePro metronomePro;

    private EditorUIManager editorUIManager; // UI manager for controlling UI elements

    public Button moveHitObjectPositionInstructionButton; // The instructions on how to move a hit object in edit mode
    public Button changeHitObjectTypeInstructionButton; // The instructinos on how to change a hit objects color in edit mode

    public AudioSource menuSFXAudioSource;
    public AudioClip colorChangedSound;

    // Use this for initialization
    void Start () {

        songTimer = 0;

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
        placeObject = FindObjectOfType<PlaceObject>();
        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();
        metronomePro = FindObjectOfType<MetronomePro>();
        editorUIManager = FindObjectOfType<EditorUIManager>();
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
            }
        }

        // Check for key color change input if an instantiated editor hit object exists
        if (instantiatedEditorHitObjectExists == true)
        {
            // Check for color change input
            CheckForColorChangeInput();

            // Show UI elements
            ActivateEditorHitObjectToolTips();
        }
        else
        {
            // Hide UI elements and disable color change input
            DeactivateEditorHitObjectToolTips();
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

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
            }
            // Purple Key Pressed
            else if (Input.GetKeyDown(KeyCode.K))
            {
                // Set has pressed K to true
                pressedKeyK = true;

                // Set the type to PURPLE as the K key has been pressed
                editorPlacedHitObjectType = 1;

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
            }
            // Red Key Pressed
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Set has pressed L to true
                pressedKeyL = true;

                // Set the type to RED as the L key has been pressed
                editorPlacedHitObjectType = 2;

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
            }
            // Green Key Pressed
            if (Input.GetKeyDown(KeyCode.U) || Input.GetKey(KeyCode.S))
            {
                // Set has pressed S to true
                pressedKeyS = true;

                // Set the type to GREEN as the U key has been pressed
                editorPlacedHitObjectType = 3;

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
            }
            // Yellow Key Pressed
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKey(KeyCode.D))
            {
                // Set has pressed D to true
                pressedKeyD = true;

                // Set the type to YELLOW as the I key has been pressed
                editorPlacedHitObjectType = 4;

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
            }
            // Orange Key Pressed
            if (Input.GetKeyDown(KeyCode.O) || Input.GetKey(KeyCode.F))
            {
                // Set has pressed F to true
                pressedKeyF = true;

                // Set the type to ORANGE as the O key has been pressed
                editorPlacedHitObjectType = 5;

                // Add a new editor hit object to the editorHitObjectList, and instantiate a new timeline object for this hit object on the timeline
                AddEditorHitObjectToList(editorPlacedHitObjectType);
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
                    // Instantiate the start time timeline object on the timeline at the current song position
                    InstantiateSpecialTimeTimelineObject("START");
                    // Mark the special time start
                    SetSpecialTimeStart();
                    // Play the specialTimeFirstPlaced sound effect
                    editorSoundController.PlaySpecialTimeStartPlacedSound();
                }
                // The the second time set the special time end
                if (specialTimeKeyPresses == 2)
                {
                    // Deactive the border image
                    DeActivateBorder();
                    // Instantiate the end time timeline object on the timeline at the current song position
                    InstantiateSpecialTimeTimelineObject("END");
                    // Mark the special time end
                    SetSpecialTimeEnd();
                    // Play the second specialTimeSecondPlaced sound effect
                    editorSoundController.PlaySpecialTimeEndPlacedSound();
                }
            }
        }

        if (playBeatmapPreview == true)
        {
            // Play the beatmap preview
            PlayBeatmapPreview();
        }

    }

    private void DestroyAllPreviewHitObjects()
    {
        // Destroy all previewHitObjects that appear on screen
        for (int i = 0; i < previewHitObjectList.Count; i++)
        {
            Destroy(previewHitObjectList[i]);
        }
    }

    // Reset the editor
    public void ResetEditor()
    {
        // Turn metronome back on
        metronomePro.UnmuteMetronome();

        // Turn off preview
        ResetBeatmapPreview();
        StopBeatmapPreview();

        // Reset lists
        editorHitObjectPositionList.Clear();
        editorHitObjectList.Clear();
        xPositionList.Clear();
        yPositionList.Clear();
        zPositionList.Clear();
        objectTypeList.Clear();
        spawnTimeList.Clear();

        // Destroy all preview hit objects
        DestroyAllPreviewHitObjects();
        // Clear the previewHitObject list
        previewHitObjectList.Clear();

        // Delete all objects in the instantiated timeline list then clear it
        for (int i = 0; i < instantiatedTimelineObjectList.Count; i++)
        {
            Destroy(instantiatedTimelineObjectList[i]);
        }

        // Clear the instantaitedTimelineObjectList also
        instantiatedTimelineObjectList.Clear();

        raycastTimelineObjectListIndex = 0;

        // Reset the song to 0 and the metronome
        metronomePro_Player.StopSong();

        // Reset keys pressed
        ResetKeysPressed();

        // Reset the song timer
        ResetSongTimer();

        // Deactivate the border
        DeActivateBorder();

        // DestroyInstantiatedEditorHitObject
        DestroyInstantiatedEditorHitObject();

        // Set editor hit object to false
        instantiatedEditorHitObjectExists = false;

        // Destroy the special timeline objects
        // Destroy(instantiatedSpecialTimeStartObject);
        // Destroy(instantiatedSpecialTimeEndObject);

        // Reset the special time key presses
        // specialTimeKeyPresses = 0;
    }

    // Destroy the instantiatedEditorHitObject that is spawned when a timeline object has been clicked
    public void DestroyInstantiatedEditorHitObject()
    {
        Destroy(instantiatedEditorHitObject);
    }

    // Get the index of the timeline object clicked on
    public int GetIndexOfRaycastTimelineObject(GameObject gameObjectPass)
    {
        // Check if an editor hit object already exists on the map, if it does delete it before adding a new one to the scene to edit
        if (instantiatedEditorHitObjectExists == true)
        {
            // Destroy the current hit editor object
            Destroy(instantiatedEditorHitObject);
            // Set back to false
            instantiatedEditorHitObjectExists = false;
        }

        // The timeline object reference is passed and assigned
        raycastTimelineObject = gameObjectPass;

        // Get the index number for the timeline object passed in the instantiated list it was inserted into when instantiated
        raycastTimelineObjectListIndex = instantiatedTimelineObjectList.IndexOf(raycastTimelineObject);
        Debug.Log(raycastTimelineObject.transform.name);
        Debug.Log(raycastTimelineObjectListIndex);

        // Instantiate the hit object saved with this timeline index
        InstantiateEditorHitObject();

        return raycastTimelineObjectListIndex;

    }

    // Activate the instantiateEditorHitObjectEdit UI elements
    private void ActivateEditorHitObjectToolTips()
    {
        // Activate the move hit object position button
        moveHitObjectPositionInstructionButton.gameObject.SetActive(true);

        // Activate the change hit object color button
        changeHitObjectTypeInstructionButton.gameObject.SetActive(true);
    }

    // Hide the instantiateEditorHitObjectEdit UI elements
    private void DeactivateEditorHitObjectToolTips()
    {
        // Activate the move hit object position button
        moveHitObjectPositionInstructionButton.gameObject.SetActive(false);

        // Activate the change hit object color button
        changeHitObjectTypeInstructionButton.gameObject.SetActive(false);
    }

    // Save the changed instantiated editor objects position
    public void SaveNewInstantiatedEditorObjectsPosition()
    {
        // Set the saved editor position to the new position of the current object/replace the old value
        editorHitObjectList[raycastTimelineObjectListIndex].hitObjectPosition = instantiatedEditorHitObject.transform.position;
        // Update the positions list for preview notes
        editorHitObjectPositionList[raycastTimelineObjectListIndex] = instantiatedEditorHitObject.transform.position;
    }

    // Removes the timeline object from the list
    public void RemoveTimelineObject()
    {
        // The index of null objects found
        int nullTimelineObjectIndex = 0;

        // Create a list for holding all the indexes of null objects found
        List<int> nullObjectsList = new List<int>();

        // Check if any objects are null in the instantiatedTimelineObjectList
        for (int i = 0; i < instantiatedTimelineObjectList.Count; i++)
        {
            // Check if any objects are null in the list
            if (instantiatedTimelineObjectList[i] == null)
            {
                // Set the index to the null object index found
                nullTimelineObjectIndex = i;
                // Add the index of a null object to the null object list
                nullObjectsList.Add(nullTimelineObjectIndex);
            }
        }
    
        // Remove all null objects and their associated information lists for the hit objects
        for (int i = nullObjectsList.Count - 1; i > -1; i--)
        {
            // Get the null object index from the list
            nullTimelineObjectIndex = nullObjectsList[i];

            // Remove the timeline object from the list
            instantiatedTimelineObjectList.RemoveAt(nullTimelineObjectIndex);

            // Remove the editorHitObject tied to the timeline from the list
            editorHitObjectList.RemoveAt(nullTimelineObjectIndex);

            // Remove the positions, type and spawn time information tied to this index in other lists
            spawnTimeList.RemoveAt(nullTimelineObjectIndex);
            editorHitObjectPositionList.RemoveAt(nullTimelineObjectIndex);
            objectTypeList.RemoveAt(nullTimelineObjectIndex);


            Debug.Log("Removed at: " + nullTimelineObjectIndex);
        }
    }


    
    // Update editorHitObject in the list's spawn time to the new value
    public void UpdateEditorHitObjectSpawnTime(float spawnTimePass, int editorHitObjectIndexPass)
    {
        // Slider change updates the spawn time for this editor hit object
        editorHitObjectList[editorHitObjectIndexPass].hitObjectSpawnTime = spawnTimePass;
        spawnTimeList[editorHitObjectIndexPass] = spawnTimePass;
        Debug.Log("object: " + editorHitObjectIndexPass + "now: " + spawnTimePass);
    }


    // Get the information for the editor hit object which is instantiated when the timeline bar has been clicked
    public Vector3 GetHitObjectPositionInformation()
    {
        // Get the position for the hit object saved
        Vector3 hitObjectSavedPosition = editorHitObjectList[raycastTimelineObjectListIndex].hitObjectPosition;
        // Return the position back to the instantiation function for the hit object
        return hitObjectSavedPosition;
    }

    public int GetHitObjectTypeInformation()
    {
        // Get the object type for the hit object saved
        int hitObjectSavedType = editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType;
        // Return the type back to the instantiation function for the hit object
        return hitObjectSavedType;
    }

    // Instantiate the editor hit object in the editor scene for the timeline object selected with its correct positioning, disable the fade script
    public void InstantiateEditorHitObject()
    {
        // Run functions to get the position, type and spawn time of this editor object based off the timelineobjectindex
        // Get the hit object position saved
        Vector3 hitObjectSavedPosition = GetHitObjectPositionInformation();
        // Get the hit object type saved
        int hitObjectSavedType = GetHitObjectTypeInformation();

        // Instantiate the editor hit object with its loaded information previously saved
        instantiatedEditorHitObject = Instantiate(editorPlacedHitObjects[hitObjectSavedType], hitObjectSavedPosition, Quaternion.Euler(0, 45, 0));
        // Get the fadeout script attached to the child of the editor hit object and disable it so it remains on screen
        FadeOut fadeOut = instantiatedEditorHitObject.GetComponentInChildren<FadeOut>();
        // Get the destroyEditorPlacedHitObject script and disable it so it doesn't get destroyed
        DestroyEditorPlacedHitObject destroyEditorPlacedHitObject = instantiatedEditorHitObject.GetComponentInChildren<DestroyEditorPlacedHitObject>();
        // Disable fadeout script for the hit object
        fadeOut.enabled = false;
        // Disable destroyEditorPlacedHitObject script for the hit object
        destroyEditorPlacedHitObject.enabled = false;

        instantiatedEditorHitObjectExists = true;
    }

    // Instantiate a timeline object at the current song time
    public float InstantiateTimelineObject(int instantiatedTimelineObjectTypePass)
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

        // Get the reference to the destroy timeline object script attached to the timeline object
        DestroyTimelineObject destroyTimelineObject = timelineObject.GetComponent<DestroyTimelineObject>();

        // Set the timeline objects spawn time to the current time in the song
        float timelineObjectSpawnTime = 0;
        timelineObjectSpawnTime = metronomePro_Player.songAudioSource.time;
        destroyTimelineObject.timelineHitObjectSpawnTime = timelineObjectSpawnTime;

        // Increase the timeline object index
        timelineObjectIndex++;

        // Return the timelineObjectSpawnTime
        return timelineObjectSpawnTime;
    }

    // Instantiate a special time start timeline object on the timeline
    public void InstantiateSpecialTimeTimelineObject(string specialTimeTypePass)
    {

        // Get the handle position currently in the song to spawn the timeline object at
        handlePositionX = metronomePro_Player.songPointSliderHandle.transform.position.x;
        // Decrease the Y position to prevent overlap
        handlePositionY = 0;
        handlePositionZ = metronomePro_Player.songPointSliderHandle.transform.position.z;

        // Assign the new position
        handlePosition = new Vector3(handlePositionX, handlePositionY, handlePositionZ);


        if (specialTimeTypePass == "START")
        {
            // Instantiate the start time object on the timeline
            instantiatedSpecialTimeStartObject = Instantiate(instantiatedSpecialTimeStartObject, handlePosition,
            Quaternion.Euler(90, 0, 0), GameObject.FindGameObjectWithTag("Timeline").transform);

            // Get the slider component from the game object instantiated
            Slider timelineSlider = instantiatedSpecialTimeStartObject.GetComponent<Slider>();

            // Get the reference to the destroy timeline object script attached to the timeline object
            SpecialTimelineObject specialTimelineObject = instantiatedSpecialTimeStartObject.GetComponent<SpecialTimelineObject>();


            // Set the timeline slider value to the current song time handles value
            timelineSlider.value = metronomePro_Player.handleSlider.value;


            // Set the timeline objects spawn time to the current time in the song
           specialTimelineObject.timelineHitObjectSpawnTime = metronomePro_Player.songAudioSource.time;
        }
        
        if (specialTimeTypePass == "END")
        {
            // Instantiate the end time object on the timeline
            instantiatedSpecialTimeEndObject = Instantiate(instantiatedSpecialTimeEndObject, handlePosition,
            Quaternion.Euler(90, 0, 0), GameObject.FindGameObjectWithTag("Timeline").transform);

            // Get the slider component from the game object instantiated
            Slider timelineSlider = instantiatedSpecialTimeEndObject.GetComponent<Slider>();

            // Get the reference to the destroy timeline object script attached to the timeline object
            SpecialTimelineObject specialTimelineObject = instantiatedSpecialTimeEndObject.GetComponent<SpecialTimelineObject>();


            // Set the timeline slider value to the current song time handles value
            timelineSlider.value = metronomePro_Player.handleSlider.value;

            // Set the timeline objects spawn time to the current time in the song
            specialTimelineObject.timelineHitObjectSpawnTime = metronomePro_Player.songAudioSource.time;
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

    // Add a new editor hit object to the editorHitObjectList saving the spawn times, positions and object type. Instantiate a timeline object for this object also
    public void AddEditorHitObjectToList(int objectTypePass)
    {
        // Set a new vector 3 based off the editor hit object position in the scene
        float x = editorHitObject.transform.position.x;
        float y = 10;
        float z = editorHitObject.transform.position.z;

        // Set the instantiate position to the editor hit object position but with a Y of 0
        instantiatePosition = new Vector3(x, y, z);
        InstantiateEditorPlacedHitObject(instantiatePosition, editorPlacedHitObjectType);

        // Call the instantiateTimelineObject function and pass the object type to instantiate a timeline object of the correct note color type
        // Retrieve the spawn time from the current time in the song player
        float hitObjectSpawnTime = InstantiateTimelineObject(objectTypePass);

        // Create a new editor hit object (class object) and assign all the variables such as position, spawn time and type
        EditorHitObject newEditorHitObject = new EditorHitObject();
        newEditorHitObject.hitObjectPosition = instantiatePosition;
        newEditorHitObject.hitObjectType = editorPlacedHitObjectType;
        newEditorHitObject.hitObjectSpawnTime = hitObjectSpawnTime;

        // Add the information to the individual lists
        editorHitObjectPositionList.Add(instantiatePosition);
        spawnTimeList.Add(hitObjectSpawnTime);
        objectTypeList.Add(editorPlacedHitObjectType);

        // Add the newEditorHitObject to the editorHitObjectList
        editorHitObjectList.Add(newEditorHitObject);

        // Reorder the editorHitObject list
        SortListOrders();
    }

    // Sort all lists based on spawn time so they're in order
    public void SortListOrders()
    {
        editorHitObjectList = editorHitObjectList.OrderBy(w => w.hitObjectSpawnTime).ToList();
    }

    // Update the editor hit object list information such as spawn times, type and positions
    public void EditorHitObjectListInformation()
    {


    }

    public void SaveListsToDatabase()
    {
        // Clear the lists before rewriting back to them with the sorted information
        editorHitObjectPositionList.Clear();
        spawnTimeList.Clear();
        objectTypeList.Clear();

        // Sort the editorHitObjects based on the spawn time
        SortListOrders();

        
        for (int i = 0; i < editorHitObjectList.Count; i++)
        {
            // Get the hit object positions, spawn times and type and assign to variables
            float xPosition = editorHitObjectList[i].hitObjectPosition.x;
            float yPosition = editorHitObjectList[i].hitObjectPosition.y;
            float zPosition = editorHitObjectList[i].hitObjectPosition.z;
            float spawnTime = editorHitObjectList[i].hitObjectSpawnTime;
            int objectType = editorHitObjectList[i].hitObjectType;

            // Combine x y z for complete position of the editor hit object before adding to the list
            Vector3 editorHitObjectPosition = new Vector3(xPosition, yPosition, zPosition);

            // Add the positions, spawn time and type to their own lists for all hit objects
            editorHitObjectPositionList.Add(editorHitObjectPosition);
            xPositionList.Add(xPosition);
            yPositionList.Add(yPosition);
            zPositionList.Add(zPosition);
            spawnTimeList.Add(spawnTime);
            objectTypeList.Add(objectType);
        }
        
    

        // Save to the database everything - spawn times, object type, positions
        for (int i = 0; i < editorHitObjectList.Count; i++)
        {
            // Add the positions to the database
            Database.database.PositionX.Add(xPositionList[i]);
            Database.database.PositionY.Add(yPositionList[i]);
            Database.database.PositionZ.Add(zPositionList[i]);
            // Add the spawn times to the database
            Database.database.HitObjectSpawnTime.Add(spawnTimeList[i]);
            // Add the object type to the database
            Database.database.ObjectType.Add(objectTypeList[i]);
        }

    }

    // Set the special time start when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeStart()
    {
        Database.database.SpecialTimeStart = metronomePro_Player.songAudioSource.time;
    }

    // Update the special time start if the instantiated special time start timeline slider has been changed
    public void UpdateSpecialTimeStart(float newSpecialTimeStartPass)
    {
        Database.database.SpecialTimeStart = newSpecialTimeStartPass;
    }

    // Set the special time end when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeEnd()
    {
        Database.database.SpecialTimeEnd = metronomePro_Player.songAudioSource.time;
    }

    // Update the special time start if the instantiated special time start timeline slider has been changed
    public void UpdateSpecialTimeEnd(float newSpecialTimeEndPass)
    {
        Database.database.SpecialTimeEnd = newSpecialTimeEndPass;
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

    // Reset the song timer when clear button has been pressed in the editor
    public void ResetSongTimer()
    {
        // Reset the song time and spacebar pressed
        hasPressedSpacebar = false;
        songTimer = 0f;
        startSongTimer = false;
    }

    // Disable the save button
    public void DisableSaveButton()
    {
        saveButton.interactable = false;
    }

    // Reset keys pressed if the map has been reset
    public void ResetKeysPressed()
    {
        // Reset keys pressed
        pressedKeyS = false;
        pressedKeyD = false;
        pressedKeyF = false;
        pressedKeyJ = false;
        pressedKeyK = false;
        pressedKeyL = false;

        // Reset specialTimeKeyPresses
        //specialTimeKeyPresses = 0;
    }

    // Enable the beatmap preview starting with note 0
    public void EnableBeatmapPreview()
    {
        // Start the beatmap preview
        playBeatmapPreview = true;
    }

    // Start the beatmap preview from where it last was left at
    public void StartBeatmapPreview()
    {
        // Reset
        ResetBeatmapPreview();
        // Start the beatmap preview
        playBeatmapPreview = true;
    }

    // Reset beatmap preview
    public void ResetBeatmapPreview()
    {
        // Remove all null objects in lists before playing the preview
        RemoveTimelineObject();
        hasSpawnedAllPreviewHitObjects = false;
        previewHitObjectIndex = 0;
        songTimer = 0;
        // DestroyInstantiatedEditorHitObject
        DestroyInstantiatedEditorHitObject();

        // Set instantiatedEditorHitObject to false
        instantiatedEditorHitObjectExists = false;
    }

    // Toggle on and off when clicked, pause then resume when next clicked 
    public void toggleBeatmapPreview()
    {
        if (playBeatmapPreview == true)
        {
            StopBeatmapPreview();
        }
        else
        {
            EnableBeatmapPreview();
        }
    }

    // Stop the beatmap preview
    public void StopBeatmapPreview()
    {
        // Allow the metronome to click again
        metronomePro.UnmuteMetronome();
        playBeatmapPreview = false;
    }

    // Play a preview of the beatmap from start to finish
    public void PlayBeatmapPreview()
    {
        // Reset the song time to 0
        // if the play button has been pressed
        // Spawn the 1's hit object
        // add the spawn to a list of preview objects
        // with fade
        // if the pause button is pressed disable the fade script on the object

        // Get the current timer of the song player
        // Calculate how many notes ahead based on the spawn time after being sorted we're at
        // Play the preview from that hit object onwards

        // Mute the metronome so no click sounds play in the preview
        metronomePro.MuteMetronome();

        // Increment the song timer
        songTimer += Time.deltaTime;

            if (previewHitObjectIndex == (editorHitObjectList.Count))
            {
                hasSpawnedAllPreviewHitObjects = true;
            }

            if (hasSpawnedAllPreviewHitObjects == false)
            {
            previewHitObjectSpawnTime = (spawnTimeList[previewHitObjectIndex] - 1);
                if (songTimer >= previewHitObjectSpawnTime)
                {
                Debug.Log("spawning: " + previewHitObjectIndex + "at: " + metronomePro_Player.songAudioSource.time);
                    // Set the preview hit object type by getting the saved type from the placed editor hit objects
                    previewHitObjectType = objectTypeList[previewHitObjectIndex];

                    // Set the preview hit object position by getting the saved position from the placed editor hit object
                    previewHitObjectPosition = editorHitObjectPositionList[previewHitObjectIndex];

                    // Instantiate the preview hit object and add to the previewHitObjectList
                    previewHitObjectList.Add(Instantiate(previewHitObjects[previewHitObjectType], previewHitObjectPosition, Quaternion.Euler(0, 45, 0)));

                    previewHitObjectIndex++;
                }
            }
        
    }

    // Change instantiated hit objects material
    public void ChangeInstantiatedEditorHitObjectMaterial(string materialTypePass)
    {
        // Get the timelinebar selected handle image
        Image instantiatedTimelineImage = instantiatedTimelineObjectList[raycastTimelineObjectListIndex].GetComponentInChildren<Image>();

        // Get the renderer attached to the editor hit object
        Renderer rend = instantiatedEditorHitObject.GetComponentInChildren<Renderer>();
        // If the object has a renderer component change its material
        if (rend != null)
        {
            // Change the material based on the type passed
            switch (materialTypePass)
            {
                case "GREEN":
                    instantiatedTimelineImage.color = greenTimelineBarColor;
                    rend.material = greenEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 3;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 3;
                    break;
                case "YELLOW":
                    instantiatedTimelineImage.color = yellowTimelineBarColor;
                    rend.material = yellowEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 4;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 4;
                    break;
                case "ORANGE":
                    instantiatedTimelineImage.color = orangeTimelineBarColor;
                    rend.material = orangeEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 5;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 5;
                    break;
                case "BLUE":
                    instantiatedTimelineImage.color = blueTimelineBarColor;
                    rend.material = blueEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 0;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 0;
                    break;
                case "PURPLE":
                    instantiatedTimelineImage.color = purpleTimelineBarColor;
                    rend.material = purpleEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 1;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 1;
                    break;
                case "RED":
                    instantiatedTimelineImage.color = redTimelineBarColor;
                    rend.material = redEditorHitObjectMaterial;
                    // Save the new color/type for the hit object in the list // the number is the type
                    editorHitObjectList[raycastTimelineObjectListIndex].hitObjectType = 2;
                    // Update the preview list also
                    objectTypeList[raycastTimelineObjectListIndex] = 2;
                    break;
            }
        }
    }

    // Check for color change input when a hit object has spawned
    private void CheckForColorChangeInput()
    {
        // Check if number keys have been pressed, if so change the color of the hit object and timeline bars
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("GREEN");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("YELLOW");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("ORANGE");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("BLUE");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("PURPLE");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayColorChangedSound();
            ChangeInstantiatedEditorHitObjectMaterial("RED");
        }
    }

    // Play color changed sound effect
    private void PlayColorChangedSound()
    {
        menuSFXAudioSource.PlayOneShot(colorChangedSound);
    }



}
