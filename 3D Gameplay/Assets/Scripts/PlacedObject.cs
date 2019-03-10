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

    // Use this for initialization
    void Start () {
        specialTimeKeyPresses = 0;
        backgroundImage.enabled = false;
        mouseFollow = FindObjectOfType<MouseFollow>();
        songProgressBar = FindObjectOfType<SongProgressBar>();
    }
	
	// Update is called once per frame
	void Update () {

        // Place a hit object only if the mouse has been clicked and the UI button has been clicked
        if (hasClickedUIButton == true)
        {
            // Blue Key Pressed
            if (Input.GetKeyDown(KeyCode.J))
            {
                // Set the type to BLUE as the J key has been pressed
                editorPlacedHitObjectType = 0;

                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
            }
            // Purple Key Pressed
            else if (Input.GetKeyDown(KeyCode.K))
            {
                // Set the type to PURPLE as the K key has been pressed
                editorPlacedHitObjectType = 1;

                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
            }
            // Red Key Pressed
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Set the type to RED as the L key has been pressed
                editorPlacedHitObjectType = 2;

                SpawnAndSavePlacedObject(editorPlacedHitObjectType);
            }

            // Special Time Key Press Set Times
            if (Input.GetKeyDown(KeyCode.H))
            {
                specialTimeKeyPresses++;

                // If the first press set the start time for special time
                if (specialTimeKeyPresses == 1)
                {
                    ActivateBorder();
                    SetSpecialTimeStart();
                }
                // The the second time set the special time end
                if (specialTimeKeyPresses == 2)
                {
                    DeActivateBorder();
                    SetSpecialTimeEnd();
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
        Database.database.HitObjectSpawnTime.Add(songProgressBar.songAudioSource.time);

        // Save object type
        Database.database.ObjectType.Add(editorPlacedHitObjectType);
    }

    // Set the special time start when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeStart()
    {
        Database.database.SpecialTimeStart = songProgressBar.songAudioSource.time;
    }

    // Set the special time end when the key has been pressed at the current time of the song when pressed
    public void SetSpecialTimeEnd()
    {
        Database.database.SpecialTimeEnd = songProgressBar.songAudioSource.time;
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
}
