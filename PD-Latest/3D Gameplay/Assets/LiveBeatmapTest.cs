using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiveBeatmapTest : MonoBehaviour {

    // Animator
    public Animator hitObjectAnimator;

    // Gameobjects
    public List<GameObject> spawnedList = new List<GameObject>(); // List of all spawned hit objects in the scene

    // Transform
    public Canvas canvas;

    // Strings
    private string hitObjectTag; // The tag on the hit object - Blue, Green, Purple, Red, Orange, Yellow

    // Integers
    private int nextIndex;
    private int totalHitObjects;
    private int totalHitObjectListSize; // The total amount of hit objects to be spawned
    private int startingYPosition; // The y position that is decremented each time a hit object is spawned to make the earliest appear ontop of future spawns
    private int startYPositionDecrementValue; // Value to decrement the y positions by each time they spawn
    private int hitObjectID; // The hit object ID
    private int objectThatCanBeHitIndex; // The current hit object that can be hit - activated
    private float songTimer; // The current time in the song
    private float trackStartTime; // The time that the song started from
    private float[] hitObjectSpawnTimes;  // Hit object spawn times

    // Bools
    private bool startCheck; // Controls checking for the first hit object when the first hit object has spawned
    private bool hasSpawnedAllHitObjects; // Has the game spawned all hit objects?
    private bool gameplayHasStarted; // Tracks starting and stopping gameplay
    private bool allHitObjectsHaveBeenHit; // Have all the hit objects been hit? Used for going to the results screen if they have

    // Keycodes
    private KeyCode startGameKey; // Game to start the gameplay

    // Scripts
    private PlacedObject placedObject;
    private MetronomePro metronomePro;
    private MetronomePro_Player metronomePro_Player;


    [System.Serializable]
    public class Pool
    {
        public int tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<int, Queue<GameObject>> poolDictionary;


    // Use this for initialization
    void Start()
    {
        // Initialize
        songTimer = 0;
        objectThatCanBeHitIndex = 0;
        nextIndex = 0;
        hitObjectID = 0;
        startYPositionDecrementValue = 10;
        startingYPosition = 99500; // Set the startYPosition to the highest range of the camera
        startCheck = false;
        gameplayHasStarted = false;
        hasSpawnedAllHitObjects = false; // Set to false as all object haven't been spawned yet
        startGameKey = KeyCode.Space; // Assign starting the game key to the spacebar


        // Reference
        placedObject = FindObjectOfType<PlacedObject>();
        metronomePro = FindObjectOfType<MetronomePro>();
        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();

        // Functions
        totalHitObjects = placedObject.editorHitObjectList.Count; // Assign the total number of hit objects based on how many x positions there are
        totalHitObjectListSize = totalHitObjects; // Get total number of objects to spawn


        poolDictionary = new Dictionary<int, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(canvas.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Update the song timer with the current song time if gameplay has started
        UpdateSongTimer();

        // Check if all the hit objects have spawned
        CheckIfAllHitObjectsHaveSpawned();

        // Check if it's time to spawn the next hit object
        CheckIfTimeToSpawnHitObject();

        // Control which hit object can be hit - earliest spawned
        EnableHitObjectsToBeHit();

        // Check if all hit objects have been hit
        CheckIfAllHitObjectsHaveBeenHit();
    }

    // Start the live test
    public void StartLiveTest()
    {
        // Reset
        ResetLiveTestFromStart();

        // Disable object placing and timeline
        placedObject.CanPlaceHitObjects = false;

        // Start spawning objects and allowing key input
        gameplayHasStarted = true;

        // Mute the metrnome tick sound during gameplay
        metronomePro.MetronomeIsMuted = true;

        // Play the song
        metronomePro_Player.PlayOrPauseSong();
    }

    // Restart live test
    public void ResetLiveTestFromStart()
    {
        // Stop the song
        metronomePro_Player.StopSong();
        // Unmuted
        metronomePro.MetronomeIsMuted = false;
        gameplayHasStarted = false;
    }

    // Disable live testing
    public void DisableLiveTest()
    {
        gameplayHasStarted = false;
        // Unmuted
        metronomePro.MetronomeIsMuted = false;
        placedObject.CanPlaceHitObjects = false;
    }

    private void SpawnFromPool(int _tag, Vector3 _position)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            GameObject objectToSpawn = poolDictionary[_tag].Dequeue();
            hitObjectAnimator = objectToSpawn.GetComponent<Animator>();
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.position = _position;
            //objectToSpawn.transform.rotation = Quaternion.Euler(0, 45, 0);
            //objectToSpawn.transform.rotation = Quaternion.Euler(-90, 0, 45);
            objectToSpawn.transform.rotation = Quaternion.Euler(90, 0, 45);

            hitObjectAnimator.Play("HitObject", 0, 0f);

            poolDictionary[_tag].Enqueue(objectToSpawn);

            spawnedList.Add(objectToSpawn);

            // If the first hit object has spawned allow the next index to increment
            if (startCheck != false)
            {
                // Increment next index for the hit object
                nextIndex++;
            }

            // Set to true as the first hit object has been instantiated
            startCheck = true;
        }
    }

    // Update the song timer with the current time of the song if gameplay has started
    private void UpdateSongTimer()
    {
        // Check if gameplay has started
        if (gameplayHasStarted == true)
        {
            // Update the song timer with the current song time
            songTimer = metronomePro.songAudioSource.time;
        }
    }

    // Check if all the hit objects have spawned
    private void CheckIfAllHitObjectsHaveSpawned()
    {
        // If the current hit object ID is the same as the hit object list size
        if (hitObjectID == totalHitObjectListSize)
        {
            // Set to true as all hit objects have now spawned
            hasSpawnedAllHitObjects = true;
        }
    }

    // Check if it's time to spawn the next hit object
    private void CheckIfTimeToSpawnHitObject()
    {
        // If all hit objects haven't been spawned yet
        if (hasSpawnedAllHitObjects == false)
        {
            // Check if it's time to spawn the next hit boject
            if (songTimer >= placedObject.editorHitObjectList[hitObjectID].hitObjectSpawnTime)
            {

                SpawnFromPool(placedObject.editorHitObjectList[hitObjectID].hitObjectType, placedObject.editorHitObjectList[hitObjectID].hitObjectPosition);

                // Spawn the next hit object
                //SpawnHitObject(hitObjectPositions[hitObjectID], Database.database.LoadedObjectType[hitObjectID], hitObjectID);
                // Increment the hit object ID to spawn the next hit object
                hitObjectID++;
            }
        }
    }

    // Control which hit object can be hit - earliest spawned
    private void EnableHitObjectsToBeHit()
    {
        // If spawning has started
        if (startCheck == true)
        {
            // If there has been a hit object spawned into the gameplay scene
            if (spawnedList.Count != 0) // 3
            {
                // If there is a hit object in the spawnedList to track 
                //if (spawnedList[objectThatCanBeHitIndex] == null)
                if (spawnedList[objectThatCanBeHitIndex].gameObject.activeSelf == false)
                {
                    // Check if the hit object is within the range of the hit object list and that there is another hit object to track after
                    if (objectThatCanBeHitIndex < totalHitObjectListSize && nextIndex > objectThatCanBeHitIndex)
                    {
                        // Check if it's the last hit object
                        if (objectThatCanBeHitIndex == (totalHitObjectListSize - 1))
                        {
                            // If it's the last hit object - Do not increment
                        }
                        else
                        {
                            // If it's not the last hit object - Increment to check the next hit object
                            objectThatCanBeHitIndex++;
                        }
                    }
                    else
                    {
                        // Do not increment
                    }
                }
                else
                {
                    // Allow the hit objects to be hit
                    spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().CanBeHit = true;
                }
            }
        }
    }

    // Check if all hit objects have been hit (for scene transition)
    private void CheckIfAllHitObjectsHaveBeenHit()
    {
        // If the first hit object has spawned and all hit objects have spawned
        if (startCheck == true && hasSpawnedAllHitObjects == true)
        {
            // If the last hit object has been hit == null
            if (spawnedList[totalHitObjectListSize - 1].gameObject.activeSelf == false)
            {
                // Set to true
                allHitObjectsHaveBeenHit = true;
            }
            else
            {
                // Set to false
                allHitObjectsHaveBeenHit = false;
            }
        }
        else
        {
            // Set to false
            allHitObjectsHaveBeenHit = false;
        }
    }
}
