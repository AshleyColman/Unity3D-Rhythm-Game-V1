using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadAndRunBeatmap : MonoBehaviour
{

    // UI
    public TextMeshProUGUI pressPlayText; // Pressplay prompt text
    public TextMeshProUGUI gameplayTitleText; // Song title and artist
    public Image songProgressBarImage; // Song progress bar

    // Audio
    public AudioSource menuSFXAudioSource; // The audio source for playing the start sound effect
    public AudioClip pressPlaySoundClip; // The sound that plays when you press play at the start of the game

    // Animation
    public Animator pressPlayAnimator; // Animates the Press Play Text at the start of the song

    public Animator hitObjectAnimator;


    // Colors
    public Color easyDifficultyColor, advancedDifficultyColor, extraDifficultyColor; // Song progress bar colors based on beatmap difficulty

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
    private float levelChangerAnimationTimer;

    // Vectors
    private Vector3[] hitObjectPositions; // Hit object positions containing all 3 xyz values from the other lists
    private Vector3 hitObjectPosition; // The hit object position to spawn at

    // Bools
    private bool startCheck; // Controls checking for the first hit object when the first hit object has spawned
    private bool hasSpawnedAllHitObjects; // Has the game spawned all hit objects?
    private bool gameplayHasStarted; // Tracks starting and stopping gameplay
    private bool allHitObjectsHaveBeenHit; // Have all the hit objects been hit? Used for going to the results screen if they have

    // Keycodes
    private KeyCode startGameKey; // Game to start the gameplay

    // Scripts
    private SongProgressBar songProgressBar; // Required for song time for spawning
    private PlayerSkillsManager playerSkillsManager; // Reference required for getting the fade speed and adjusting the spawn times based on the speed chosen
    private FailAndRetryManager failAndRetryManager; // Used for tracking whether the user has failed and restarting the game scene
    GameplayToResultsManager gameplayToResultsManager;

    // Properties

    public float LevelChangerAnimationTimer
    {
        get { return levelChangerAnimationTimer; }
    }


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
        startGameKey = KeyCode.Space; // Assign starting the game key to the spacebar


        // Reference
        songProgressBar = FindObjectOfType<SongProgressBar>();
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();

        // Functions
        totalHitObjects = Database.database.loadedPositionX.Count; // Assign the total number of hit objects based on how many x positions there are
        totalHitObjectListSize = totalHitObjects; // Get total number of objects to spawn
        //CalculateNewHitObjectYPositions(); // Calculate all the y positions for the hit objects, preventing overlap during gameplay
        LoadHitObjectPositions(); // Load the hit object xyz positions from the beatmap file
        LoadHitObjectSpawnTimes(); // Load and update the hit object spawn times with the fade speed selected value
        UpdateGameplayUI(); // When gameplay scene has loaded update the UI text
        SetDifficultyUI(); // Check the difficulty selected, change the song progress bar color to the difficulty color


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
        levelChangerAnimationTimer += Time.deltaTime;

        if (levelChangerAnimationTimer >= 2f)
        {
            // Check keyboard input to start gameplay
            CheckToStartGameplay();
        }

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

    private void SpawnFromPool(int _tag, Vector3 _position)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            GameObject objectToSpawn = poolDictionary[_tag].Dequeue();
            hitObjectAnimator = objectToSpawn.GetComponent<Animator>();
            objectToSpawn.gameObject.SetActive(true);


            //float positionX = _position.x;
            //float positionY = _position.y;
            //float positionZ = 100;
            //_position = new Vector3(positionX, positionY, positionZ);

            objectToSpawn.transform.position = _position;
            //objectToSpawn.transform.rotation = Quaternion.Euler(0, 45, 0);
            //objectToSpawn.transform.rotation = Quaternion.Euler(-90, 0, 45);
            objectToSpawn.transform.rotation = Quaternion.Euler(90, 0, 45);

            // Play the animation based on the animation speed
            switch (playerSkillsManager.FadeSpeedSelected)
            {
                case "SLOW":
                    hitObjectAnimator.Play("SlowHitObject", 0, 0f);
                    break;
                case "NORMAL":
                    hitObjectAnimator.Play("HitObject", 0, 0f);
                    break;
                case "FAST":
                    hitObjectAnimator.Play("FastHitObject", 0, 0f);
                    break;
            }

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
            hitObjectPosition = new Vector3(Database.database.loadedPositionX[i], Database.database.loadedPositionY[i], Database.database.loadedPositionZ[i]);
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
            // Set the hit object spawn time to equal the hit object spawn time - the fade speed selected (2, 1, 0.5)
            Database.database.loadedHitObjectSpawnTime[i] = (Database.database.loadedHitObjectSpawnTime[i] - playerSkillsManager.GetFadeSpeedSelected());
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
            Database.database.loadedPositionY[i] = startingYPosition;
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
            // Animate the press play text at the start of the song
            StartCoroutine(AnimatePressPlayText());
            // Play the press play sound effect
            PlayPressPlaySound();
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
            songTimer = songProgressBar.SongTimePosition;
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
            if (songTimer >= Database.database.loadedHitObjectSpawnTime[hitObjectID])
            {

                SpawnFromPool(Database.database.loadedObjectType[hitObjectID], hitObjectPositions[hitObjectID]);

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
                    if (failAndRetryManager.HasFailed == true)
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
        songProgressBar.songAudioSource.PlayScheduled(trackStartTime);
    }

    // Check the difficulty selected, change the song progress bar color to the difficulty color and enable the difficulty text panel
    private void SetDifficultyUI()
    {
        switch (Database.database.LoadedBeatmapDifficulty)
        {
            case "easy":
                songProgressBarImage.color = easyDifficultyColor;
                break;
            case "advanced":
                songProgressBarImage.color = advancedDifficultyColor;
                break;
            case "extra":
                songProgressBarImage.color = extraDifficultyColor;
                break;
        }
    }

    // Play the animation for the PressPlayText
    private IEnumerator AnimatePressPlayText()
    {
        pressPlayAnimator.Play("PressPlayTextAnimation");
        yield return new WaitForSeconds(0.10f);
        pressPlayText.enabled = false;
    }

    // Play the PressPlay sound effect
    private void PlayPressPlaySound()
    {
        menuSFXAudioSource.PlayOneShot(pressPlaySoundClip, 1f);
    }

    // Update the song name, artist and difficulty with the values entered
    public void UpdateGameplayUI()
    {
        // Create the playedByUsername value off the current player logged in
        string playedByUsername = "playing as " + MySQLDBManager.username;
        // Create the gameplay title from song name + artist
        string gameplayTitle = Database.database.LoadedSongName + " [ " + Database.database.LoadedSongArtist + " ]";
        // + " ] " + "      " + playedByUsername;
        // Set gameplay text to gameplay title, make all upper case
        gameplayTitleText.text = gameplayTitle.ToUpper();
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
                if (failAndRetryManager.HasFailed == false)
                {
                    // Transition to the next scene
                    gameplayToResultsManager.TransitionScene();
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
}