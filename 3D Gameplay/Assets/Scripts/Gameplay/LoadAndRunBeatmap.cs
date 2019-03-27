using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadAndRunBeatmap : MonoBehaviour {

    
    public SongProgressBar songProgressBar; // Required for song time for spawning

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
    public float songTimer;
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
    public float fadeSpeedModifier; // The time to take off the spawn times according to the fade speed modifier
    bool hasSpawnedAllHitObjects; // Has the game spawned all hit objects?
    int totalHitObjectListSize; // The total hit amount of hit objects to be spawned
    bool checkObjectsThatCanBeHit = false;

    public bool[] hitObjectSpawned;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {

        songProgressBar = FindObjectOfType<SongProgressBar>();
        specialTimeManager = FindObjectOfType<SpecialTimeManager>();
        isSpecialTime = false;
        songTimer = 0;
        startSongTimer = false;
        objectThatCanBeHitIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;
        hitObjectID = 0;

        // Load the hit object positions first into their own list
        hitObjectPositionsX = Database.database.LoadedPositionX;
        hitObjectPositionsY = Database.database.LoadedPositionY;
        hitObjectPositionsZ = Database.database.LoadedPositionZ;
        totalHitObjects = hitObjectPositionsX.Count;
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
}

    // Update is called once per frame
    void Update () {

        // Load special time start
        specialTimeStart = specialTimeManager.specialTimeStart;
        // Load special time end
        specialTimeEnd = specialTimeManager.specialTimeEnd;

        
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
        }

        if (startSongTimer == true)
        {
            // Update the song timer with the current song time
            songTimer += Time.deltaTime;
        }


        // Check if it's special time 
        CheckSpecialTime();

        if (hasSpawnedAllHitObjects == false && hitObjectID < totalHitObjectListSize)
        {
            // Spawn normal notes if not special time
            if (isSpecialTime == false)
            {
                if (songTimer >= hitObjectSpawnTimes[hitObjectID])
                {
                    SpawnHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID], hitObjectID);
                    hitObjectID++;
                }
            }
            // Spawn special notes if special time
            else if (isSpecialTime == true)
            {
                if (songTimer >= hitObjectSpawnTimes[hitObjectID])
                {
                    SpawnSpecialHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID]);
                    hitObjectID++;
                }
            }

            /*
            // Size of list
            sizeOfList = spawnedList.Count;
            Debug.Log("objecttahtcanbehitindex: " + objectThatCanBeHitIndex);
            //if (startCheck == false)
            {
                
                if (hitObjectSpawned[objectThatCanBeHitIndex] == true)
                {
                    if (spawnedList[objectThatCanBeHitIndex] != null)
                    {
                        Debug.Log("object: " + objectThatCanBeHitIndex + " exists and is set to CANBEHIT");
                        // Set the earliest hit object that has spawned to be the earliest for hit detection
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().canBeHit = true;
                    }

                    // If the earliest object has been destroyed
                    if (spawnedList[objectThatCanBeHitIndex] == null)
                    {
                        if (nextIndex > objectThatCanBeHitIndex)
                        {
                            // Check if another object has spawned
                            objectThatCanBeHitIndex++;
                            Debug.Log("current object doesn't not exist and nextIndex is greater than current index, incrementing");
                        }
                        else
                        {
                            Debug.Log("CURRENT OBJECT IS NULL BUT NEXTINDEX NOT LARGE ENOUGH");
                        }
                    }
                }


            }

            */

            if (startCheck == true)
            {
                if (spawnedList[0] != null)
                {
                    if (objectThatCanBeHitIndex == 0)
                    {
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().canBeHit = true;
                    }
                }



                if (spawnedList[objectThatCanBeHitIndex] == null)
                {
                    // Object has been destroyed
                    // assign canBeHit to the next one in the list IF the next one has spawned

                    if (hitObjectSpawned[objectThatCanBeHitIndex + 1] == true)
                    {
                        objectThatCanBeHitIndex++;
                        // A new object has spawned assign it to be hit and increment objectThatCanBeHitIndex
                        spawnedList[objectThatCanBeHitIndex].GetComponent<TimingAndScore>().canBeHit = true;
                    }
                }

            }


        }
    }

    // index = 0
    // first object spawns 
    // next = 1
    // index is hit and next = 1
    // no object spawns
    // index (1) does not exist
    // check if index (1) has spawned, if it has enable checking 


    // Spawn the hit object
    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass, int hitObjectID)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
        nextIndex++;
        checkObjectsThatCanBeHit = true;

        // Add to the list of spawned
        hitObjectSpawned[hitObjectID] = true;
    }

    // Spawn special hit object during special time
    public void SpawnSpecialHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(specialHitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
        // Increment the highest index currently
        nextIndex++;

        // Add to the list of spawned
        hitObjectSpawned[hitObjectID] = true;
    }

    // Check if it's special time, if it is we spawn special time notes
    public void CheckSpecialTime()
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
}
