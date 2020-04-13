using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoadAndRunBeatmap : MonoBehaviour
{
    #region Variables
    // UI
    public TextMeshProUGUI SongNameAndArtistNameText, designedByCreatorText, easyDifficultyText, normalDifficultyText,
        hardDifficultyText, pressSpacebarToPlayText;

    // Gameobjects
    public List<GameObject> spawnedList = new List<GameObject>();
    public GameObject noteLight; 

    // Transform
    public Transform canvas;

    // Strings
    private string hitObjectTag; 

    // Integers
    private int nextIndex, totalHitObjects, totalHitObjectListSize, hitObjectID, objectThatCanBeHitIndex,
        lastHitMouseHitObject;
    private float songTimer, trackStartTime;
    private float[] hitObjectSpawnTimes;

    public int TESTNUMBER;


    // Vectors
    private Vector3[] hitObjectPositions; 
    private Vector3 hitObjectPosition;

    // Bools
    private bool startCheck, hasSpawnedAllHitObjects, gameplayHasStarted, allHitObjectsHaveBeenHit, mouseActive;

    // Keycodes
    private KeyCode startGameKey; 

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool AllHitObjectsHaveBeenHit
    {
        get { return allHitObjectsHaveBeenHit; }
    }

    public int LastHitMouseHitObject
    {
        set { lastHitMouseHitObject = value; }
    }

    [System.Serializable]
    public class Pool
    {
        public int tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<int, Queue<GameObject>> poolDictionary;
    #endregion

    #region Functions
    void Start()
    {
        // Initialize
        songTimer = 0;
        objectThatCanBeHitIndex = 0;
        nextIndex = 0;
        hitObjectID = 0;
        lastHitMouseHitObject = 0;
        startCheck = false;
        gameplayHasStarted = false;
        hasSpawnedAllHitObjects = false;
        mouseActive = true;
        startGameKey = KeyCode.Space;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        totalHitObjects = Database.database.LoadedPositionX.Count; // Assign the total number of hit objects based on how many x positions there are
        totalHitObjectListSize = totalHitObjects; // Get total number of objects to spawn
        LoadHitObjectPositions(); // Load the hit object xyz positions from the beatmap file
        LoadHitObjectSpawnTimes(); // Load and update the hit object spawn times with the fade speed selected value
        StartCoroutine(GetAudioFile()); // Get the audio file and load it into an audio clip
        EnableMouse(); // Enable mouse to hit mouse hit objects

        // Initialize pool
        poolDictionary = new Dictionary<int, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(canvas.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void Update()
    {
        if (gameplayHasStarted == false)
        {
            if (Input.GetKeyDown(startGameKey))
            {
                gameplayHasStarted = true;
                StartMusic(); 
                pressSpacebarToPlayText.gameObject.SetActive(false);
            }
        }

        if (allHitObjectsHaveBeenHit == false)
        {
            // Update the song timer with the current song time if gameplay has started
            UpdateSongTimer();

            // Check if all the hit objects have spawned
            CheckIfAllHitObjectsHaveSpawned();

            // Check if it's time to spawn the next hit object
            CheckIfTimeToSpawnHitObject();

            // Check mouse reset
            CheckMouseReset();

            // Control which hit object can be hit - earliest spawned
            EnableHitObjectsToBeHit();

            // Check if all hit objects have been hit
            CheckIfAllHitObjectsHaveBeenHit();
        }
    }

    // Spawn hit object from pool
    private void SpawnFromPool(int _tag, Vector3 _position)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            GameObject objectToSpawn = poolDictionary[_tag].Dequeue();
            objectToSpawn.gameObject.SetActive(true);

            objectToSpawn.transform.position = _position;
            objectToSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);

            objectToSpawn.transform.SetAsFirstSibling();

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

    // Setup timing information
    private void SetupTiming()
    {
        scriptManager.metronomePro.SetupGameplayTiming(Database.database.LoadedBPM, Database.database.LoadedOffsetMS);
    }

    // Load the hit object xyz positions from the beatmap file and insert into the vector3 position array
    private void LoadHitObjectPositions()
    {
        // Initialize the hit object positions array with the size of the totalHitObjects
        hitObjectPositions = new Vector3[totalHitObjects];

        // Merge all 3 position lists from the beatmap file to create the new beatmap position
        for (int i = 0; i < totalHitObjects; i++)
        {
            // Create the new position for inserting to the array
            hitObjectPosition = new Vector3(Database.database.LoadedPositionX[i], Database.database.LoadedPositionY[i], Database.database.LoadedPositionZ[i]);
            // Add the position of all the values into the array of positions used for spawning
            hitObjectPositions[i] = hitObjectPosition;
        }
    }

    // Load and update the hit object spawn times with the fade speed selected value
    private void LoadHitObjectSpawnTimes()
    {
        int fadeSpeedValue = 1;
        // Load each spawn time from the beatmap file
        // Update the spawn time position by taking away the time to fade in based on the fade speed selected
        for (int i = 0; i < totalHitObjects; i++)
        {
            // Set the hit object spawn time to equal the hit object spawn time - the fade speed selected (2, 1, 0.5)
            Database.database.LoadedHitObjectSpawnTime[i] = (Database.database.LoadedHitObjectSpawnTime[i] - fadeSpeedValue);
        }
    }

    // Check keyboard input to start gameplay
    private void CheckToStartGameplay()
    {
        // If the startGameKey has been pressed we start the song and song timer
        if (Input.GetKeyDown(startGameKey) && gameplayHasStarted == false)
        {
            // Set to true as gameplay has now started
            gameplayHasStarted = true;

            // Start the music
            StartMusic();
        }
    }

    // Update the song timer with the current time of the song if gameplay has started
    private void UpdateSongTimer()
    {
        // Check if gameplay has started
        if (gameplayHasStarted == true)
        {
            // Update the song timer with the current song time
            songTimer = scriptManager.rhythmVisualizatorPro.audioSource.time;
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
            if (songTimer >= Database.database.LoadedHitObjectSpawnTime[hitObjectID])
            {
                TESTNUMBER = Random.Range(4, 8);
                //SpawnFromPool(Database.database.LoadedObjectType[hitObjectID], hitObjectPositions[hitObjectID]);
                SpawnFromPool(TESTNUMBER, hitObjectPositions[hitObjectID]);

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
            if (spawnedList.Count != 0)
            {
                // If there is a hit object in the spawnedList to track 
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
                    // Mouse object type
                    //switch (Database.database.LoadedObjectType[objectThatCanBeHitIndex])
                    switch (TESTNUMBER)
                    {
                        case Constants.MOUSE_HIT_OBJECT_TYPE_DOWN:
                            SetMouseHitObjectToBeHit();
                            break;
                        case Constants.MOUSE_HIT_OBJECT_TYPE_UP:
                            SetMouseHitObjectToBeHit();
                            break;
                        case Constants.MOUSE_HIT_OBJECT_TYPE_LEFT:
                            SetMouseHitObjectToBeHit();
                            break;
                        case Constants.MOUSE_HIT_OBJECT_TYPE_RIGHT:
                            SetMouseHitObjectToBeHit();
                            break;
                        default:
                            spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit = true;
                            break;
                    }

                    // Set note light to next note position (lerp?)
                    //noteLight.transform.position = spawnedList[objectThatCanBeHitIndex].transform.position;
                }
            }
        }
    }

    // Start the music
    void StartMusic()
    {
        trackStartTime = (float)AudioSettings.dspTime;
        scriptManager.rhythmVisualizatorPro.audioSource.PlayScheduled(trackStartTime);
        scriptManager.rhythmVisualizatorPro.audioSource.volume = 1f;
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

    // Get audio file from path
    private IEnumerator GetAudioFile()
    {
        string audioFilePath = Database.database.LoadedBeatmapFolderDirectory + "audio.ogg";

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + audioFilePath, AudioType.OGGVORBIS))
        {
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {

            }
            else
            {
                scriptManager.rhythmVisualizatorPro.audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

                SetupTiming(); // Setting metronome timing information
            }
        }
    }

    // Set mouse hit object to be hit
    private void SetMouseHitObjectToBeHit()
    {
        if (mouseActive == true)
        {
            spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit = true;
        }
    }

    // Reset mouse
    public void ResetMouse()
    {
        mouseActive = false;
        scriptManager.mouseFollow.cursorImage.color = Color.red;
    }

    // Enable mouse
    private void EnableMouse()
    {
        mouseActive = true;
        scriptManager.mouseFollow.cursorImage.color = Color.green;
    }

    // Check mouse reset
    private void CheckMouseReset()
    {
        if (mouseActive == false)
        {
            switch (lastHitMouseHitObject)
            {
                case Constants.MOUSE_HIT_OBJECT_TYPE_LEFT:
                    if (Input.mousePosition.x >= Constants.RESET_MOUSE_LEFT_POS_X)
                    {
                        EnableMouse();
                    }
                    break;
                case Constants.MOUSE_HIT_OBJECT_TYPE_RIGHT:
                    if (Input.mousePosition.x <= Constants.RESET_MOUSE_RIGHT_POS_X)
                    {
                        EnableMouse();
                    }
                    break;
                case Constants.MOUSE_HIT_OBJECT_TYPE_UP:
                    if (Input.mousePosition.y <= Constants.RESET_MOUSE_UP_POS_Y)
                    {
                        EnableMouse();
                    }
                    break;
                case Constants.MOUSE_HIT_OBJECT_TYPE_DOWN:
                    if (Input.mousePosition.y >= Constants.RESET_MOUSE_DOWN_POS_Y)
                    {
                        EnableMouse();
                    }
                    break;
            }
        }
    }
    #endregion
}