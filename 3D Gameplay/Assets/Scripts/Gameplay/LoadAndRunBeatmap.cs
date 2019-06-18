using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadAndRunBeatmap : MonoBehaviour {

    public SongProgressBar songProgressBar; // Required for song time for spawning
    public Image songProgressBarImage; // Song progress bar image
    public Color easyDifficultyColor; // Easy difficulty song progress bar color
    public Color advancedDifficultyColor; // Advanced difficulty song progress bar color
    public Color extraDifficultyColor; // Extra difficulty song progress bar color

    float timer = 0f;

    public float spawnTime;
    private string hitObjectTag;

    public GameObject[] hitObject = new GameObject[7];
    public GameObject[] specialHitObject = new GameObject[7];
    public List<float> hitObjectPositionsX = new List<float>();
    public List<float> hitObjectPositionsY = new List<float>();
    public List<float> hitObjectPositionsZ = new List<float>();
    public List<Vector3> hitObjectPositions = new List<Vector3>();
    public List<float> hitObjectSpawnTimes = new List<float>();
    public List<int> hitObjectType = new List<int>();
    public List<GameObject> spawnedList = new List<GameObject>();
    public Vector3 hitObjectPosition;
    private int hitObjectID;
    public int objectThatCanBeHitIndex; // The current earliest note
    public bool hasHit;
    public bool startCheck;
    public int sizeOfList;
    public int nextIndex;
    private bool justHit = false;
    public double songTimer;
    public float specialTimeStart;
    public float specialTimeEnd;
    public SpecialTimeManager specialTimeManager;
    public bool isSpecialTime;
    public bool startSongTimer;
    public Animator pressPlayAnimator; // Animates the Press Play Text at the start of the song
    public TextMeshProUGUI pressPlayText; // The Press Play text at the start of the song
    public AudioSource menuSFXAudioSource; // The audio source for playing the start sound effect
    public AudioClip PressPlaySound; // The sound that plays when you press play at the start of the game
    public TextMeshProUGUI gameplayTitleText;
    public string songName;
    public string songArtist;
    public string beatmapDifficulty;
    private bool hasPressedSpacebar; // Used for tracking if the song has been started, if it has then we disable the song from restarting when the spacebar is pressed again
    private int totalHitObjects;
    bool hasSpawnedAllHitObjects; // Has the game spawned all hit objects?
    int totalHitObjectListSize; // The total hit amount of hit objects to be spawned
    bool checkObjectsThatCanBeHit = false;
    public bool[] hitObjectSpawned;
    private int startingYPosition; // The y position that is decremented each time a diamond is spawned to make the earliest appear ontop of later spawning diamonds
    PlayerSkillsManager playerSkillsManager; // Reference required for getting the fade speed and adjusting the spawn times based on the speed chosen
    private float fadeSpeedSelected; // The fade speed selected

    private bool allHitObjectsHaveBeenHit; // Have all the hit objects been hit? Used for going to the results screen if they have

    FailAndRetryManager failAndRetryManager; // Used for tracking whether the user has failed and restarting the game scene

    bool hasFailed; // Has the user faileds

    // Song start time
    double trackStartTime;

    BeatSoundManager beatSoundManager;


    // Use this for initialization
    void Start()
    {
        hasFailed = false;
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();
        songProgressBar = FindObjectOfType<SongProgressBar>();
        specialTimeManager = FindObjectOfType<SpecialTimeManager>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        beatSoundManager = FindObjectOfType<BeatSoundManager>();
        isSpecialTime = false;
        songTimer = 0;
        startSongTimer = false;
        objectThatCanBeHitIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;
        hitObjectID = 0;

        // Set the startYPosition to the highest range of the camera
        startingYPosition = 99500; 

        // Get the fade speed selected
        fadeSpeedSelected = playerSkillsManager.GetFadeSpeedSelected();

        // Load the hit object positions first into their own list
        hitObjectPositionsX = Database.database.LoadedPositionX;
        hitObjectPositionsY = Database.database.LoadedPositionY;
        hitObjectPositionsZ = Database.database.LoadedPositionZ;
        totalHitObjects = hitObjectPositionsX.Count;

        // For all y positions calculate the new positions to prevent overlap in the gameplay
        for (int i = 0; i < hitObjectPositionsY.Count; i++)
        {
            // Decrement startYPosition so each note has a differnt Y position sorting the layer
            startingYPosition -= 10;

            // Assign
            hitObjectPositionsY[i] = startingYPosition;
        }

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
            // Remove the fade speed seconds from each of the loaded spawn times
            hitObjectSpawnTimes[i] = hitObjectSpawnTimes[i] - fadeSpeedSelected;
        }

        // Load the hit object types
        hitObjectType = Database.database.LoadedObjectType;

        // Load the UI text information from the loaded file
        songName = Database.database.loadedSongName;
        songArtist = Database.database.loadedSongArtist;
        beatmapDifficulty = Database.database.loadedBeatmapDifficulty;
        // When gameplay scene has loaded update the UI text
        UpdateGameplayUI();

        // Set to false at the start, set to true when spacebar has been pressed
        hasPressedSpacebar = false;

        // Set to false as all object haven't been spawned yet
        hasSpawnedAllHitObjects = false;
        // Get total number of objects to spawn
        totalHitObjectListSize = Database.database.LoadedPositionX.Count;

        hitObjectSpawned = new bool[totalHitObjectListSize];

        // Check the difficulty selected, change the song progress bar color to the difficulty color
        SetSongProgressBarColor();
        
    }

    // Update is called once per frame
    void Update()
    {

        // Load special time start
        specialTimeStart = specialTimeManager.specialTimeStart;
        // Load special time end
        specialTimeEnd = specialTimeManager.specialTimeEnd;

        // Check if it's special time 
        CheckSpecialTime();

        // If the space key has been pressed we start the song and song timer
        if (Input.GetKeyDown(KeyCode.Space) && hasPressedSpacebar == false)
        {
            // Spacebar has been pressed
            hasPressedSpacebar = true;
            // Start the song timer as the game has started
            startSongTimer = true;
            // Animate the press play text at the start of the song
            StartCoroutine(AnimatePressPlayText());
            // Play the press play sound effect
            PlayPressPlaySound();

            StartMusic();

            // Start the beatSoundManager
            beatSoundManager.Play();
        }

        if (startSongTimer == true)
        {
            // Update the song timer with the current song time
            //songTimer += Time.deltaTime;

            songTimer = songProgressBar.songTimePosition;
        }

        if (hitObjectID == (totalHitObjectListSize))
        {
            hasSpawnedAllHitObjects = true;
        }

        if (isSpecialTime == false && hasSpawnedAllHitObjects == false)
        {
            if (songTimer >= hitObjectSpawnTimes[hitObjectID])
            {
                SpawnHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID], hitObjectID);
                hitObjectID++;
            }
        }
        // Spawn special notes if special time
        else if (isSpecialTime == true && hasSpawnedAllHitObjects == false)
        {
            if (songTimer >= hitObjectSpawnTimes[hitObjectID])
            {
                SpawnSpecialHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID]);
                hitObjectID++;
            }
        }


        if (startCheck == true)
        {
            if (spawnedList.Count != 0) // 3
            {
                if (spawnedList[objectThatCanBeHitIndex] == null) 
                {
                    // Object has been destroyed
                    // && objectThatCanBeHitIndex < nextIndex
                    if (objectThatCanBeHitIndex < totalHitObjectListSize && nextIndex > objectThatCanBeHitIndex) 
                    {
                        if (objectThatCanBeHitIndex == (totalHitObjectListSize - 1)) 
                        {
                            // Do not increment
                        }
                        else
                        {
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
                    // Check if the user has failed
                    CheckIfFailed();

                    // If the user has failed set can be hit to false
                    if (hasFailed == true)
                    {
                        // Make hit objects unhittable
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().CannotBeHit();
                    }
                    else
                    {
                        // Allow the hit objects to be hit
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().CanBeHit();
                    }
                    
                }
            }
        }

    }

    void StartMusic()
    {
        trackStartTime = AudioSettings.dspTime;
        songProgressBar.songAudioSource.PlayScheduled(trackStartTime);
    }

    // Check the difficulty selected, change the song progress bar color to the difficulty color
    private void SetSongProgressBarColor()
    {
        switch (beatmapDifficulty)
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

    // Spawn the hit object
    private void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass, int hitObjectID)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));

        if (startCheck == false)
        {
            // DO not increment first hit object next index
        }
        else
        {
            nextIndex++;
        }

        startCheck = true;


        // Add to the list of spawned
        hitObjectSpawned[hitObjectID] = true;
    }

    // Spawn special hit object during special time
    private void SpawnSpecialHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(specialHitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));

        if (startCheck == false)
        {
            // DO not increment first hit object next index
        }
        else
        {
            nextIndex++;
        }

        startCheck = true;
        // Increment the highest index currently


        // Add to the list of spawned
        hitObjectSpawned[hitObjectID] = true;
    }

    // Check if it's special time, if it is we spawn special time notes
    private void CheckSpecialTime()
    {
        if (specialTimeManager.isSpecialTime == true)
        {
            isSpecialTime = true;
        }
        if (specialTimeManager.isSpecialTime == false)
        {
            isSpecialTime = false;
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
        menuSFXAudioSource.PlayOneShot(PressPlaySound, 1f);
    }

    // Update the song name, artist and difficulty with the values entered
    public void UpdateGameplayUI()
    {
        gameplayTitleText.text = songName + " [ " + songArtist + " ] " + " [ " + beatmapDifficulty.ToUpper() + " ] ";
    }

    // Check if all hit objects have been hit
    public bool CheckIfAllHitObjectsHaveBeenHit()
    {
        if (startCheck == true && hasSpawnedAllHitObjects == true)
        {
            // If the last hit object has been hit == null
            if (spawnedList[totalHitObjectListSize - 1] == null)
            {
                // Set to true
                allHitObjectsHaveBeenHit = true;
                return allHitObjectsHaveBeenHit;
            }
            else
            {
                // Set to false
                allHitObjectsHaveBeenHit = false;
                return allHitObjectsHaveBeenHit;
            }
        }
        else
        {
            // Set to false
            allHitObjectsHaveBeenHit = false;
            return allHitObjectsHaveBeenHit;
        }
    }

    // Check if the user has failed or not
    private void CheckIfFailed()
    {
        // Check if the user has failed or not
        hasFailed = failAndRetryManager.ReturnHasFailed();
    }
}