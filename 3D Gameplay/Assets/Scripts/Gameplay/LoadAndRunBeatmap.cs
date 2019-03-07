using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAndRunBeatmap : MonoBehaviour {

    
    public SongProgressBar songProgressBar; // Required for song time for spawning

    float timer = 0f;

    public float spawnTime;
    private int hitObjectType;
    private string hitObjectTag;

    public GameObject[] hitObject = new GameObject[7];
    public List<float> hitObjectPositionsX = new List<float>();
    public List<float> hitObjectPositionsY = new List<float>();
    public List<float> hitObjectPositionsZ = new List<float>();
    public List<Vector3> hitObjectPositions = new List<Vector3>();
    public List<float> hitObjectSpawnTimes = new List<float>();
    public List<GameObject> spawnedList = new List<GameObject>();
    public Vector3 hitObjectPosition;
    private int hitObjectID;
    private int earliestIndex; // The current earliest note
    public bool hasHit;
    private bool startCheck;
    private int sizeOfList;
    private int nextIndex;
    private bool justHit = false;
    public float songTimer;
    void Awake()
    {
        // Load beatmap
        Database.database.Load();
    }

    // Use this for initialization
    void Start () {

        songProgressBar = FindObjectOfType<SongProgressBar>();

        songTimer = 0;
        hitObjectType = 0;
        earliestIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;
        hitObjectID = 0;

        // Load the hit object positions first into their own list
        hitObjectPositionsX = Database.database.LoadedPositionX;
        hitObjectPositionsY = Database.database.LoadedPositionY;
        hitObjectPositionsZ = Database.database.LoadedPositionZ;
        // Now merge together for vector3 positions
        for (int i = 0; i < hitObjectPositionsX.Count; i++)
        {
            // Create the new position for inserting to the list
            hitObjectPosition = new Vector3(hitObjectPositionsX[i], hitObjectPositionsY[i], hitObjectPositionsZ[i]);
            // Add the position of all the values into the list of positions used for spawning
            hitObjectPositions.Add(hitObjectPosition);
        }
        // Get the spawn times and insert into the list
        hitObjectSpawnTimes = Database.database.LoadedHitObjectSpawnTime;

        
        // Update the spawn times to match when they should be clicked (1 second earlier)
        for (int i = 0; i < hitObjectSpawnTimes.Count; i++)
        {
            // Remove 1 second from each of the loaded spawn times
            hitObjectSpawnTimes[i] = hitObjectSpawnTimes[i] - 1;
        }
        

    }
	
	// Update is called once per frame
	void Update () {

        // Update the song timer with the current song time
        songTimer = songProgressBar.songAudioSource.time;


        if (songTimer >= hitObjectSpawnTimes[hitObjectID])
        {
            SpawnHitObject(hitObjectPositions[hitObjectID], hitObjectType);
            hitObjectID++;
        }

        
        
        // Size of list
        sizeOfList = spawnedList.Count;

        if (startCheck == true)
        {
            
            // If the index object exists
            if (spawnedList[earliestIndex] != null)
            {
                // Set the earliest hit object that has spawned to be the earliest for hit detection
                spawnedList[earliestIndex].GetComponent<TimingAndScore>().isEarliest = true;
            }
            // If the earliest object has been destroyed
            if (spawnedList[earliestIndex] == null)
            {
                // Check if another object has spawned
                if (nextIndex > earliestIndex)
                {
                    earliestIndex++;
                }
           }
            



            /*
            // If the index object doesn't exist and the size of the list is greater than the nextIndex required 
            if (spawnedList[earliestIndex] == null && sizeOfList > nextIndex)
            {
                earliestIndex++;
            }
            */
        }
        
    }

    // Spawn the hit object
    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
        // Increment the highest index currently
        nextIndex++;
    }
}
