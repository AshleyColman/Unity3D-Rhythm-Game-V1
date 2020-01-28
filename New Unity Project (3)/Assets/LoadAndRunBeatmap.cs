using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoadAndRunBeatmap : MonoBehaviour
{

    // Animation
    public Animator pressPlayAnimator;

    // UI
    public TextMeshProUGUI gameplayTitleText, beatmapCreatorText, difficultyText;

    // Gameobjects
    public List<GameObject> spawnedList = new List<GameObject>(); // List of all spawned hit objects in the scene

    // Transform
    public Transform canvas;

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
    private const int NORMAL_FADE_SPEED_TIME = 1; // Time to remove from hit object spawn times if normal fade speed selected
    // Vectors
    private Vector3[] hitObjectPositions; // Hit object positions containing all 3 xyz values from the other lists
    private Vector3 hitObjectPosition; // The hit object position to spawn at

    // Bools
    private bool startCheck; // Controls checking for the first hit object when the first hit object has spawned
    private bool hasSpawnedAllHitObjects; // Has the game spawned all hit objects?
    private bool gameplayHasStarted; // Tracks starting and stopping gameplay
    private bool allHitObjectsHaveBeenHit; // Have all the hit objects been hit? Used for going to the results screen if they have
    private bool hasLoadedTiming;

    // Keycodes
    private KeyCode startGameKey; // Game to start the gameplay

    // Scripts
    private ScriptManager scriptManager;

    // Properties

    // Get all hit object have been hit
    public bool AllHitObjectsHaveBeenHit
    {
        get { return allHitObjectsHaveBeenHit; }
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
        hasLoadedTiming = false;
        startGameKey = KeyCode.Space; // Assign starting the game key to the spacebar

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        totalHitObjects = Database.database.LoadedPositionX.Count; // Assign the total number of hit objects based on how many x positions there are
        totalHitObjectListSize = totalHitObjects; // Get total number of objects to spawn
        LoadHitObjectPositions(); // Load the hit object xyz positions from the beatmap file
        LoadHitObjectSpawnTimes(); // Load and update the hit object spawn times with the fade speed selected value
        UpdateGameplayUI(); // When gameplay scene has loaded update the UI text
        StartCoroutine(GetAudioFile()); // Get the audio file and load it into an audio clip

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

    // Update is called once per frame
    void Update()
    {
        if (hasLoadedTiming == false)
        {
            if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
            {
                LoadTiming(); // Load metronome timing information
                hasLoadedTiming = true;
            }
        }

        if (gameplayHasStarted == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressPlayAnimator.Play("PressPlay_Animation", 0, 0f);
                scriptManager.menuSFXManager.PlaySoundEffect(0);
                StartMusic();
                scriptManager.rotatorManager.CalculateRotations();
                scriptManager.rotatorManager.UpdateTimeToReachTarget();
                scriptManager.rotatorManager.ToggleLerpOn();
                gameplayHasStarted = true;
            }
        }
        else
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
    }

    private void LoadTiming()
    {
        scriptManager.metronomePro.Bpm = Database.database.LoadedBPM;
        scriptManager.metronomePro.OffsetMS = Database.database.LoadedOffsetMS;
        scriptManager.metronomePro.CalculateIntervals();
        scriptManager.metronomePro.CalculateActualStep();
    }

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
        // Load each spawn time from the beatmap file
        // Update the spawn time position by taking away the time to fade in based on the fade speed selected
        for (int i = 0; i < totalHitObjects; i++)
        {
            Database.database.LoadedHitObjectSpawnTime[i] = (Database.database.LoadedHitObjectSpawnTime[i] - NORMAL_FADE_SPEED_TIME);


            // Set the hit object spawn time to equal the hit object spawn time - the fade speed selected (2, 1, 0.5)
            //Database.database.loadedHitObjectSpawnTime[i] = (Database.database.loadedHitObjectSpawnTime[i] - playerSkillsManager.GetFadeSpeedSelected());
        }
    }

    // Calculate all the y positions for the hit objects, preventing overlap during gameplay
    private void CalculateNewHitObjectYPositions()
    {
        // For all y positions calculate the new positions to prevent overlap in the gameplay
        for (int i = 0; i < totalHitObjects; i++)
        {
            // Decrement startYPosition so each note has a differnt Y position sorting the layer
            startingYPosition -= startYPositionDecrementValue;

            // Assign the changed y position to the hit object y position list used for spawning the hit objects
            Database.database.LoadedPositionY[i] = startingYPosition;
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
        // Update the song timer with the current song time
        songTimer = scriptManager.rhythmVisualizatorPro.audioSource.time;
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

                SpawnFromPool(Database.database.LoadedObjectType[hitObjectID], hitObjectPositions[hitObjectID]);

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
                    // If the user has failed set can be hit to false
                    if (scriptManager.failAndRetryManager.HasFailed == true)
                    {
                        // Make hit objects unhittable
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().CanBeHit = false;
                    }
                    else
                    {
                        // Allow the hit objects to be hit
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().CanBeHit = true;
                    }
                }
            }
        }
    }

    // Start the music
    void StartMusic()
    {
        trackStartTime = (float)AudioSettings.dspTime;
        scriptManager.rhythmVisualizatorPro.audioSource.PlayScheduled(trackStartTime);
        scriptManager.rhythmVisualizatorPro.audioSource.volume = 0.5f;
    }

    // Update the song name, artist and difficulty with the values entered
    public void UpdateGameplayUI()
    {
        // Create the gameplay title from song name + artist
        string gameplayTitle = Database.database.LoadedSongName + " [ " + Database.database.LoadedSongArtist + " ]";

        // Set gameplay text to gameplay title, make all upper case
        gameplayTitleText.text = gameplayTitle.ToUpper();

        beatmapCreatorText.text = "DESIGNED BY " + Database.database.LoadedBeatmapCreator.ToUpper();

        difficultyText.text = Database.database.LoadedBeatmapDifficulty.ToUpper() + " " + Database.database.LoadedBeatmapDifficultyLevel;

        // Set difficulty text color 
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

                // If the user hasn't failed
                if (scriptManager.failAndRetryManager.HasFailed == false)
                {
                    // Transition to the next scene
                    //gameplayToResultsManager.TransitionScene();
                }
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
            }
        }
    }
}