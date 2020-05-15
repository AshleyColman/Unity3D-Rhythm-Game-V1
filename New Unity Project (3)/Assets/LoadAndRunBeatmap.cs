using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LoadAndRunBeatmap : MonoBehaviour
{
    #region Variables
    // UI
    public TextMeshProUGUI songNameText, artistNameText, creatorNameText, difficultyNameText, pressSpacebarToPlayText,
        beingPlayedByText;

    // Gameobjects
    public GameObject noteLight;

    // Image
    public Image leftSideGradientImage, difficultyButtonImage;

    // Transform
    public Transform canvas;

    // Strings
    private string hitObjectTag;

    // Integers
    public int nextIndex, totalHitObjects, totalHitObjectListSize, hitObjectID, objectThatCanBeHitIndex,
        lastHitMouseHitObject, nextFeverPhraseObjectID, feverPhraseArrayLength;
    private float songTimer, trackStartTime, noteLightPositionLerp, notesLockedTimer;
    private float[] hitObjectSpawnTimes;
    private int TESTNUMBER;
    private int feverPhraseToCheck;

    // Vectors
    private Vector3[] hitObjectPositions;
    private Vector3 hitObjectPosition, noteLightPositionToLerpTo;

    // Bools
    public bool startCheck, hasSpawnedAllHitObjects, gameplayHasStarted, allHitObjectsHaveBeenHit, mouseActive, lerpNoteLight,
        notesLocked;

    // Keycodes
    private KeyCode startGameKey;

    // Scripts
    private ScriptManager scriptManager;
    private FeverPhrase[] feverPhraseArr;
    private List<FeverHitObject> spawnedFeverPhraseHitObjects = new List<FeverHitObject>();
    public List<HitObject> spawnedList = new List<HitObject>();
    public List<HitObject> activeList = new List<HitObject>();
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

    public int FeverPhraseToCheck
    {
        get { return feverPhraseToCheck; }
    }

    public bool NotesLocked
    {
        get { return notesLocked; }
        set { notesLocked = value; }
    }

    public FeverPhrase[] FeverPhraseArr
    {
        get { return feverPhraseArr; }
    }

    [System.Serializable]
    public class Pool
    {
        public int tag;
        public HitObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<int, Queue<HitObject>> poolDictionary;
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
        noteLightPositionLerp = 0;
        feverPhraseToCheck = 0;
        feverPhraseArrayLength = 0;
        notesLockedTimer = 0f;
        lerpNoteLight = false;
        startCheck = false;
        gameplayHasStarted = false;
        hasSpawnedAllHitObjects = false;
        notesLocked = false;
        mouseActive = true;
        noteLightPositionToLerpTo = noteLight.transform.position;
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
        UpdateUI(); // Update ui

        // Initialize pool
        poolDictionary = new Dictionary<int, Queue<HitObject>>();
        foreach (Pool pool in pools)
        {
            Queue<HitObject> objectPool = new Queue<HitObject>();

            for (int i = 0; i < pool.size; i++)
            {
                HitObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(canvas.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        // INITIALIZE FEVER PHRASE FOR TESTING
        int totalPhrases = 3;
        int totalNoteInPhrase = 5;

        FeverPhrase feverPhrase1 = new FeverPhrase();
        FeverPhrase feverPhrase2 = new FeverPhrase();
        FeverPhrase feverPhrase3 = new FeverPhrase();

        feverPhraseArr = new FeverPhrase[totalPhrases];

        feverPhrase1.Contructor(totalNoteInPhrase, 0);
        feverPhraseArr[0] = feverPhrase1;

        feverPhrase2.Contructor(totalNoteInPhrase, 1);
        feverPhraseArr[1] = feverPhrase2;

        feverPhrase3.Contructor(totalNoteInPhrase, 2);
        feverPhraseArr[2] = feverPhrase3;
    }

    void Update()
    {
        if (gameplayHasStarted == false)
        {
            if (Input.GetKeyDown(startGameKey))
            {
                gameplayHasStarted = true;
                StartMusic();
                StartCoroutine(scriptManager.healthbar.PlayNoFailCountdown());
                pressSpacebarToPlayText.gameObject.SetActive(false);
            }
        }

        if (allHitObjectsHaveBeenHit == false)
        {
            // Check locked note timer
            CheckLockedNoteTimer();

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

            // Lerp note light
            switch (lerpNoteLight)
            {
                case true:
                    LerpNoteLightToNextPosition();
                    break;
            }
        }
    }
    
    // Check locked note timer
    private void CheckLockedNoteTimer()
    {
        if (notesLocked == true)
        {
            notesLockedTimer += Time.deltaTime;

            if (notesLockedTimer >= Constants.LOCK_NOTE_DURATION)
            {
                notesLocked = false;
            }
        }
    }

    // Lock all notes from being hit due to incorrect note/spam
    public void ResetLockNoteTimer()
    {
        notesLocked = true;
        notesLockedTimer = 0f;
        scriptManager.cameraScript.PlayCameraShakeAnimation();
    }

    // Update gameplay ui
    private void UpdateUI()
    {
        songNameText.text = Database.database.LoadedSongName;
        artistNameText.text = Database.database.LoadedSongArtist;
        creatorNameText.text = Database.database.LoadedBeatmapCreator;
        beingPlayedByText.text = Constants.BEING_PLAYED_BY_STRING + MySQLDBManager.username + " " + Utilities.GetCurrentDate();

        Color color08, color05;

        switch (Database.database.LoadedBeatmapDifficulty)
        {
            case Constants.EASY_DIFFICULTY:
                // Text
                difficultyNameText.text = Constants.EASY_DIFFICULTY.ToUpper() + " " + Constants.LEVEL_PREFIX + 
                    " " + Database.database.LoadedBeatmapDifficultyLevel;

                // Create new color
                color08 = new Color(scriptManager.uiColorManager.easyDifficultyColor.r,
                        scriptManager.uiColorManager.easyDifficultyColor.g, scriptManager.uiColorManager.easyDifficultyColor.b,
                        0.8f);
                break;
            case Constants.NORMAL_DIFFICULTY:
                // Text
                difficultyNameText.text = Constants.NORMAL_DIFFICULTY.ToUpper() + " " + Constants.LEVEL_PREFIX +
                    " " + Database.database.LoadedBeatmapDifficultyLevel;

                // Create new color
                color08 = new Color(scriptManager.uiColorManager.normalDifficultyColor.r,
                        scriptManager.uiColorManager.normalDifficultyColor.g, scriptManager.uiColorManager.normalDifficultyColor.b,
                        0.8f);
                break;
            case Constants.HARD_DIFFICULTY:
                // Text
                difficultyNameText.text = Constants.HARD_DIFFICULTY.ToUpper() + " " + Constants.LEVEL_PREFIX +
                    " " + Database.database.LoadedBeatmapDifficultyLevel;
                // Create new color
                color08 = new Color(scriptManager.uiColorManager.hardDifficultyColor.r,
                        scriptManager.uiColorManager.hardDifficultyColor.g, scriptManager.uiColorManager.hardDifficultyColor.b,
                        0.8f);
                break;
            default:
                color08 = scriptManager.uiColorManager.blackColor08;
                break;
        }

        // Create color
        color05 = new Color(color08.r, color08.g, color08.b, Constants.LEFT_SIDE_GRADIENT_IMAGE_ALPHA);
        // Set color
        leftSideGradientImage.color = color05;
        difficultyButtonImage.color = color08;
    }

    // Spawn hit object from pool
    private void SpawnFromPool(int _tag, Vector3 _position)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            // If fever phrase array has more than 0 phrases
            if (feverPhraseArr.Length > 0)
            {
                // If the fever phrase has not started yet
                if (feverPhraseArr[feverPhraseToCheck].PhraseStarted == false)
                {
                    // Check if the current hit object id == the next hit object id for the phrase
                    //if (hitObjectID == feverPhraseArr[feverPhraseToCheck].NextObjectID)
                    if (hitObjectID == feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[0])
                    {
                        // Set phrase started to true
                        feverPhraseArr[feverPhraseToCheck].PhraseStarted = true;

                        // Update fever phrase array length
                        feverPhraseArrayLength = (feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr.Length - 1);

                        // Control remaining note count text and play animation
                        switch (scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.activeSelf)
                        {
                            case true:
                                scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.SetActive(true);
                                break;
                            case false:
                                scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.SetActive(false);
                                scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.SetActive(true);
                                break;
                        }

                        // Update text count with the total amount of notes in the phrase
                        scriptManager.feverTimeManager.UpdateRemainingPhraseNoteCountText((feverPhraseArrayLength + 1));
                    }
                }

                // If the fever phrase has started
                if (feverPhraseArr[feverPhraseToCheck].PhraseStarted == true)
                {
                    // If the fever phrase hasn't finished
                    if (feverPhraseArr[feverPhraseToCheck].PhraseFinished == false)
                    {
                        // Check current fever phrase coming up
                        nextFeverPhraseObjectID = feverPhraseArr[feverPhraseToCheck].NextObjectID;

                        // Check the current fever phrase, check the next object in the phrase
                        // If the current fever phrase, next object to spawn ID == hitObjectID
                        if (feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[nextFeverPhraseObjectID] == hitObjectID)
                        {
                            //switch (Database.database.LoadedObjectType[hitObjectID])

                            // Change note to fever variant 
                            switch (TESTNUMBER)
                            {
                                case Constants.HIT_OBJECT_TYPE_KEY_D:
                                    _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY_D;
                                    break;
                                case Constants.HIT_OBJECT_TYPE_KEY_F:
                                    _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY_F;
                                    break;
                                case Constants.HIT_OBJECT_TYPE_KEY_J:
                                    _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY_J;
                                    break;
                                case Constants.HIT_OBJECT_TYPE_KEY_K:
                                    _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY_K;
                                    break;
                            }
                            #region Code for last hit object type
                            /*
                            // If the next object ID is the last note in the fever series
                            if (nextFeverPhraseObjectID == (feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr.Length - 1))
                            {
                                _tag = Constants.PHRASE_FEVER_HIT_OBJECT_TYPE;
                            }
                            else
                            {
                                //switch (Database.database.LoadedObjectType[hitObjectID])
                                switch (TESTNUMBER)
                                {
                                    case Constants.KEY_HIT_OBJECT_TYPE_KEY1:
                                        _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY1;
                                        //CheckFeverPhraseNextObjectIDAgainstLength();
                                        break;
                                    case Constants.KEY_HIT_OBJECT_TYPE_KEY2:
                                        _tag = Constants.FEVER_HIT_OBJECT_TYPE_KEY2;
                                        //CheckFeverPhraseNextObjectIDAgainstLength();
                                        break;
                                }
                            }
                            */
                            #endregion
                        }
                    }
                }
            }

            HitObject objectToSpawn = poolDictionary[_tag].Dequeue();
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.position = _position;
            objectToSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);
            objectToSpawn.transform.SetAsFirstSibling();
            poolDictionary[_tag].Enqueue(objectToSpawn);
            spawnedList.Add(objectToSpawn);
            activeList.Add(objectToSpawn);

            // Initialize the fever phrase hit object index if the note type is fever
            switch (_tag)
            {
                case Constants.FEVER_HIT_OBJECT_TYPE_KEY_D:
                    InitializeFeverPhraseObject();
                    CheckFeverPhraseNextObjectIDAgainstLength();
                    break;
                case Constants.FEVER_HIT_OBJECT_TYPE_KEY_F:
                    InitializeFeverPhraseObject();
                    CheckFeverPhraseNextObjectIDAgainstLength();
                    break;
                case Constants.FEVER_HIT_OBJECT_TYPE_KEY_J:
                    InitializeFeverPhraseObject();
                    CheckFeverPhraseNextObjectIDAgainstLength();
                    break;
                case Constants.FEVER_HIT_OBJECT_TYPE_KEY_K:
                    InitializeFeverPhraseObject();
                    CheckFeverPhraseNextObjectIDAgainstLength();
                    break;
            }

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

    // Remove hit object from the active hit object list
    public void RemoveObjectFromActiveList(HitObject _hitObject)
    {
        activeList.RemoveAt(activeList.IndexOf(_hitObject));
    }

    // Initialize the fever phrase hit object index
    private void InitializeFeverPhraseObject()
    {
        // Get reference to script of object
        FeverHitObject feverHitObject = spawnedList[hitObjectID].GetComponent<FeverHitObject>();
        // update fever phrase object index
        feverHitObject.FeverPhraseObjectIndex = feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[nextFeverPhraseObjectID];
        // Add fever phrase to spawned fever phrase object list
        spawnedFeverPhraseHitObjects.Add(feverHitObject);
    }

    // Fever phrase broken
    private void FeverPhraseBroken()
    {
        // Play fever broken animation on first note miss only
        switch (feverPhraseArr[feverPhraseToCheck].PhraseBroken)
        {
            case false:
                switch (scriptManager.feverTimeManager.feverFailedPanel.gameObject.activeSelf)
                {
                    case false:
                        scriptManager.feverTimeManager.feverFailedPanel.gameObject.SetActive(true);
                        break;
                    case true:
                        scriptManager.feverTimeManager.feverFailedPanel.gameObject.SetActive(false);
                        scriptManager.feverTimeManager.feverFailedPanel.gameObject.SetActive(true);
                        break;
                }

                scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.SetActive(false);
                break;
        }

        // Set to true as fever phrase has been broke
        feverPhraseArr[feverPhraseToCheck].PhraseBroken = true;
        
        // Change color of active fever objects to unactive
        for (int i = 0; i < spawnedFeverPhraseHitObjects.Count; i++)
        {
            if (spawnedFeverPhraseHitObjects[i].gameObject.activeSelf == true)
            {
                spawnedFeverPhraseHitObjects[i].AssignUnactive();
            }
        }
    }

    // Update fever phrase information
    public void UpdateFeverPhrase(bool _hit, int _feverPhraseObjectIndex)
    {
        switch (_hit)
        {
            case false:
                FeverPhraseBroken();
                break;
        }

        // Get index of the current note in the fever phrase
        /*
        int index = Array.IndexOf(feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr,
            feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[nextFeverPhraseObjectID]);
        // Calculate the count
        int count = (feverPhraseArrayLength - index);
                    */

        // Calculate the count
        int count = ((feverPhraseArrayLength + 1) - nextFeverPhraseObjectID);

        // Update remaining phrase note count text
        scriptManager.feverTimeManager.UpdateRemainingPhraseNoteCountText(count);

        // Check if last fever phrase hit object has been hit
        if (_feverPhraseObjectIndex == feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[feverPhraseArrayLength])
        {
            feverPhraseArr[feverPhraseToCheck].PhraseFinished = true;
            spawnedFeverPhraseHitObjects.Clear();

            // On last note hit/miss
            switch (_hit)
            {
                case true:
                    if (feverPhraseArr[feverPhraseToCheck].PhraseBroken != true)
                    {
                        scriptManager.feverTimeManager.AddPhrase();
                    }
                    break;
            }

            if (feverPhraseToCheck != (feverPhraseArr.Length - 1))
            {
                feverPhraseToCheck++;
            }
        }
    }

    // Check fever phrase next object id against the last object id found at the end of the fever phrase
    private void CheckFeverPhraseNextObjectIDAgainstLength()
    {
        // If the next object id == the object id in the last note of the fever series
        if (feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[nextFeverPhraseObjectID] ==
            feverPhraseArr[feverPhraseToCheck].PhraseObjectIDArr[feverPhraseArrayLength])
        {
            /*
            // Do not increment next object index
            feverPhraseArr[feverPhraseToCheck].PhraseFinished = true;
            
            // Check to make sure there is another fever phrase to check after incrementing
            if (feverPhraseToCheck != (feverPhraseArr.Length - 1))
            {
                feverPhraseToCheck++;
            }
            */
        }
        else
        {
            // Increment
            feverPhraseArr[feverPhraseToCheck].NextObjectID++;
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
                //TESTNUMBER = 4;
                TESTNUMBER = UnityEngine.Random.Range(4, 6);
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
                    /*
                    switch (TESTNUMBER[hitObjectID])
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
                            if (spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit == false)
                            {
                                spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit = true;
                            }
                            break;
                    }
                    */

                    if (spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit == false)
                    {
                        spawnedList[objectThatCanBeHitIndex].GetComponent<HitObject>().CanBeHit = true;
                    }

                    // Assign next note light lerp position
                    AssignNextNoteLightPosition(spawnedList[objectThatCanBeHitIndex].transform.position);
                }
            }
        }
    }

    // Assign next note light position
    public void AssignNextNoteLightPosition(Vector3 _position)
    {
        noteLightPositionToLerpTo = _position;

        noteLightPositionLerp = 0f;
        lerpNoteLight = true;
    }

    // Lerp note light to the next hit object position
    private void LerpNoteLightToNextPosition()
    {
        noteLightPositionLerp += Time.deltaTime / Constants.SCORE_LERP_DURATION;
        Vector3 position = Vector3.Lerp(noteLight.transform.position, noteLightPositionToLerpTo, noteLightPositionLerp);

        noteLight.transform.position = position;

        if (noteLight.transform.position == noteLightPositionToLerpTo)
        {
            lerpNoteLight = false;
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
                scriptManager.feverTimeManager.CalculateMeasureDuration(); // Calculate measure duration for fever time
                scriptManager.feverTimeManager.CalculateFeverDuration(); // Calculate fever duration for fever time
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