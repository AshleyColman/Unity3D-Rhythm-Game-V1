using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class PlacedObject : MonoBehaviour
{

    // UI
    private Slider timelineSlider, songPreviewPointSlider;

    // Gameobjects
    private GameObject timelineObject, instantiatedHitObject;
    private List<GameObject> previewHitObjectList = new List<GameObject>(); // Preview hit objects that have been spawned when the preview button has been pressed and the song timer has reached the spawn time for the hit object
    public List<GameObject> instantiatedTimelineObjectList = new List<GameObject>(); // List of instantiated timeline objects that are added to the list when instantiated
    public GameObject[] hitObjectPrefab = new GameObject[2];
    public GameObject[] timelineObjectPrefab = new GameObject[2];

    // Script objects
    public List<EditorHitObject> hitObjectList = new List<EditorHitObject>(); // List of editorHitObjects (includes spawn time, object type and positions)

    // Integers   
    private const int HIT_OBJECT_TYPE_KEY_1 = 0, HIT_OBJECT_TYPE_KEY_2 = 1, HIT_OBJECT_TYPE_KEY_3 = 2, HIT_OBJECT_TYPE_KEY_4 = 3,
        HIT_OBJECT_TYPE_KEY_5 = 4;
    private const int ANIMATION_TYPE_NONE = 0, ANIMATION_TYPE_CAMERASHAKE = 1, ANIMATION_TYPE_BACKGROUNDPULSE = 2;
    private const int SOUND_TYPE_CLAP = 0, SOUND_TYPE_FINISH = 1, SOUND_TYPE_WHISTLE = 2;
    private int instantiatedTimelineObjectType;
    private int keyMode; 
    private const int timelineObjectDeactivateValue = 15; // Value that determines when to deactive timeline objects
    private int raycastTimelineObjectListIndex; // The index of the timeline bar clicked in the editor, used to delete and update existing notes spawn times, position etc by getting the index on click
    private int timelineObjectIndex; // The index for all editor objects, increases by 1 everytime one is instantiated
    private int nullTimelineObjectIndex; // Index for checking null gameobjects
    private int hitObjectSavedType; // Saved hit object type
    private float currentSongTimePercentage; // The current time in the song turned to percentage value
    private float currentTickTime; // Current tick time
    private float nextTickTime; // Next tick time
    private float userPressedTime; // Time the user pressed the key down
    private float closestTickTime; // Closest tick time based on the user pressing the key down
    private float hitObjectSpawnTime; // Hit object spawn time
    private float timelineObjectSpawnTime; // Object spawn time for activating/deactivating objects
    private float currentSongTime; // Current song time
    private float deactivateAfterObjectTime; // Time to deactivate the timeline object after it reaches this number
    private float deactivateBeforeObjectTime; // Time to deactivate the timeline object before it reaches this number
    private float deactivateObjectTimer; // Timer for controlling checks on deactivating timeline hit objects
    private List<float> tickTimesList = new List<float>(); // Tick times for comparing and calculating the closest tick time based on user key press time
    private List<int> nullObjectsList = new List<int>(); // List of all null gameobjects

    // Vectors
    private Vector3 timelineObjectPosition; // Object to instantiate timeline object to

    // Bools
    private bool instantiatedhitObjectExists; // Used to check if a timeline bar has been clicked, instantiating a hitobject to appear on screen, if another timeline bar is pressed
    private bool objectSpawnTimeIsTaken; // Check if spawn time already exists or taken by another hit object
    private bool canPlaceHitObjects; // Controls whether hit objects can be placed with key presses 

    // Colors
    public Color customObjectColor1, customObjectColor2; // Timeline and hit object colors

    // Transform
    public Transform canvas, timeline;

    // Keycode
    private const KeyCode HIT_OBJECT_TYPE_KEY_CODE_1 = KeyCode.D, HIT_OBJECT_TYPE_KEY_CODE_2 = KeyCode.F, HIT_OBJECT_TYPE_KEY_CODE_3 = KeyCode.G,
        HIT_OBJECT_TYPE_KEY_CODE_4 = KeyCode.J, HIT_OBJECT_TYPE_KEY_CODE_5 = KeyCode.K;

    // Scripts
    private DestroyTimelineObject destroyTimelineObject; // Destroy timeline object script attached to instantiated timeline objects
    public List<DestroyTimelineObject> destroyTimelineObjectList = new List<DestroyTimelineObject>();
    private ScriptManager scriptManager;

    // Properties
    public bool CanPlaceHitObjects
    {
        set { canPlaceHitObjects = value; }
    }

    public int KeyMode
    {
        get { return keyMode; }
    }


    // Pools for placed hit objects on the grid
    [System.Serializable]
    public class Pool
    {
        public int type;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<int, Queue<GameObject>> poolDictionary;

    // Use this for initialization
    void Start()
    {
        // Intialize
        nullTimelineObjectIndex = 0;
        hitObjectSavedType = 0;
        hitObjectSpawnTime = 0;
        objectSpawnTimeIsTaken = false;
        canPlaceHitObjects = true;
        timelineObjectPosition = new Vector3(0, 0, 0);

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
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.type, objectPool);
        }


        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If key input for placing hit objects is allowed
        if (canPlaceHitObjects == true && scriptManager.rhythmVisualizatorPro.audioSource.time > 2f)
        {
            // Spawn key hit object type
            if (Input.GetKeyDown(HIT_OBJECT_TYPE_KEY_CODE_1))
            {
                PlaceHitObject(HIT_OBJECT_TYPE_KEY_1);
            }
            /*
            else if (Input.GetKeyDown(HIT_OBJECT_TYPE_KEY_CODE_2))
            {
                PlaceHitObject(HIT_OBJECT_TYPE_KEY_2);
            }
            else if (Input.GetKeyDown(HIT_OBJECT_TYPE_KEY_CODE_3))
            {
                PlaceHitObject(HIT_OBJECT_TYPE_KEY_3);
            }
            else if (Input.GetKeyDown(HIT_OBJECT_TYPE_KEY_CODE_4))
            {
                PlaceHitObject(HIT_OBJECT_TYPE_KEY_4);
            }
            else if (Input.GetKeyDown(HIT_OBJECT_TYPE_KEY_CODE_5))
            {
                PlaceHitObject(HIT_OBJECT_TYPE_KEY_5);
            }
            */

            // Song preview start time key pressed
            else if (Input.GetKeyDown(KeyCode.T))
            {
                // Update the song preview point
                UpdateSongPreviewPoint();
            }
        }
    }
    
    // Spawn from pool
    private void SpawnFromPool(int _type)
    {
        if (poolDictionary.ContainsKey(_type) == true)
        {
            GameObject objectToSpawn = poolDictionary[_type].Dequeue();
            objectToSpawn.gameObject.SetActive(true);

            // Could be improved
            objectToSpawn.GetComponent<Animator>().Play("EditorHitObject_FadeOut_Animation", 0, 0f);
            objectToSpawn.transform.localPosition = scriptManager.pathPlacer.points[scriptManager.metronomePro.CurrentTick - 1];
            objectToSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);
            objectToSpawn.transform.SetAsLastSibling();

            poolDictionary[_type].Enqueue(objectToSpawn);
        }
    }

    // Place new hit object based on the type
    public void PlaceHitObject(int _type)
    {
        AddEditorHitObjectToList(_type);
    }

    // Destroy all previewHitObjects that appear on screen
    private void DestroyAllPreviewHitObjects()
    {
        for (int i = 0; i < previewHitObjectList.Count; i++)
        {
            Destroy(previewHitObjectList[i]);
        }
    }

    // Delete all objects in the instantiated timeline list then clear it
    private void DestroyAllInstantiatedTimelineObjects()
    {
        for (int i = 0; i < instantiatedTimelineObjectList.Count; i++)
        {
            Destroy(instantiatedTimelineObjectList[i]);
        }
    }

    // Reset the editor
    public void ResetEditor()
    {
        // Clear all hit objects
        hitObjectList.Clear();

        // Clears
        destroyTimelineObjectList.Clear();

        // Destroy all preview hit objects
        DestroyAllPreviewHitObjects();

        // Clear the previewHitObject list
        previewHitObjectList.Clear();

        // Clear all null object list
        nullObjectsList.Clear();

        // Delete all objects in the instantiated timeline list then clear it
        DestroyAllInstantiatedTimelineObjects();

        // Clear the instantaitedTimelineObjectList also
        instantiatedTimelineObjectList.Clear();

        // Reset the raycast timeline object index
        raycastTimelineObjectListIndex = 0;

        // Reset object saved type
        hitObjectSavedType = 0;

        // Reset null index
        nullTimelineObjectIndex = 0;

        // Reset the song to 0 and the metronome
        scriptManager.metronomePro_Player.StopSong();

        // Reset tick times list
        tickTimesList.Clear();

        // Set hit object to false
        instantiatedhitObjectExists = false;

        // Reset editor song
        scriptManager.metronomePro_Player.StopSong();
    }

    // Save the changed instantiated editor objects position
    public void SaveNewInstantiatedHitObjectsPosition()
    {
        // Set the saved editor position to the new position of the current object/replace the old value
        hitObjectList[raycastTimelineObjectListIndex].HitObjectPosition = instantiatedHitObject.transform.position;
    }

    // Removes the timeline object from the list
    public void RemoveTimelineObject(int _timelineIndex)
    {
        // Make the timeline object passed null in the list
        instantiatedTimelineObjectList[_timelineIndex] = null;
        // Make the destroy timeline script also null to be removed
        destroyTimelineObjectList[_timelineIndex] = null;

        // The index of null objects found
        nullTimelineObjectIndex = 0;

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

            // Remove the hit object tied to the timeline from the list
            hitObjectList.RemoveAt(nullTimelineObjectIndex);
        }

        // Clear the list for next time
        nullObjectsList.Clear();

        // Reset for next time
        nullTimelineObjectIndex = 0;



        // Check if any objects are null 
        for (int i = 0; i < destroyTimelineObjectList.Count; i++)
        {
            // Check if any objects are null in the list
            if (destroyTimelineObjectList[i] == null)
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
            destroyTimelineObjectList.RemoveAt(nullTimelineObjectIndex);
        }

        // Clear the list for next time
        nullObjectsList.Clear();

        // Reset for next time
        nullTimelineObjectIndex = 0;
    }

    // Update editorHitObject in the list's spawn time to the new value
    public void UpdateEditorHitObjectSpawnTime(float _spawnTime, int _hitObjectIndex)
    {
        // Slider change updates the spawn time for this hit object
        hitObjectList[_hitObjectIndex].HitObjectSpawnTime = _spawnTime;
    }

    // Get the information for the editor hit object which is instantiated when the timeline bar has been clicked
    public Vector3 GetHitObjectPositionInformation()
    {
        // Get the position for the hit object saved
        Vector3 hitObjectSavedPosition = hitObjectList[raycastTimelineObjectListIndex].HitObjectPosition;
        // Return the position back to the instantiation function for the hit object
        return hitObjectSavedPosition;
    }

    // Get the hit object type information
    public int GetHitObjectTypeInformation()
    {
        // Get the object type for the hit object saved
        // Return the type back to the instantiation function for the hit object
        return hitObjectList[raycastTimelineObjectListIndex].HitObjectType;
    }

    // Instantiate the editor hit object in the editor scene for the timeline object selected with its correct positioning, disable the fade script
    public void InstantiateEditorHitObject()
    {
        // Run functions to get the position, type and spawn time of this editor object based off the timelineobjectindex
        // Get the hit object position saved
        Vector3 hitObjectSavedPosition = GetHitObjectPositionInformation();

        // Get the hit object type saved
        hitObjectSavedType = GetHitObjectTypeInformation();

        // Instantiate the editor hit object with its loaded information previously saved
        instantiatedHitObject = Instantiate(hitObjectPrefab[hitObjectSavedType], hitObjectSavedPosition, Quaternion.Euler(-90, 45, 0));

        // Set to true as object has been instantiated
        instantiatedhitObjectExists = true;
    }

    // Update timeline objects sort order, indexs and text
    public void UpdateTimelineObjects()
    {
        if (instantiatedTimelineObjectList.Count != 0)
        {
            for (int i = 0; i < instantiatedTimelineObjectList.Count; i++)
            {
                // Update position
                destroyTimelineObjectList[i].UpdateHierarchyPosition();
                // Update index
                destroyTimelineObjectList[i].TimelineObjectListIndex = i;
                // Update text
                destroyTimelineObjectList[i].UpdateNumberText(i);
            }
        }
    }

    // Instantiate a timeline object at the current song time
    public void UpdateSongPreviewPoint()
    {
        // Calculate the slider value based off the timeline hit object spawn time
        // Update the slider value 
        songPreviewPointSlider.value = CalculateTimelineHitObjectSliderValue(scriptManager.rhythmVisualizatorPro.audioSource.time);

        // Update the preview start time with the current song time
        scriptManager.setupBeatmap.GetSongPreviewStartTime(scriptManager.rhythmVisualizatorPro.audioSource.time);
    }

    // Instantiate a timeline object at the current song time
    public void InstantiateTimelineObject(int _instantiatedTimelineObjectType, float _hitObjectSpawnTime, int _objectType)
    {
        // Instantiate the type of object

        timelineObject = Instantiate(timelineObjectPrefab[_instantiatedTimelineObjectType], Vector3.zero,
        Quaternion.identity);

        timelineObject.transform.SetParent(timeline, false);

        // Update the scale of the timeline to the current timeline size settings
        var rectTransform = timelineObject.transform as RectTransform;
        rectTransform.sizeDelta = new Vector2(scriptManager.timelineScript.CurrentTimelineWidth, rectTransform.sizeDelta.y);

        // Stretch to fit the parent timeline object
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Reset position left, right, up, down
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        // Get the timeline slider from the timeline object instantiated
        timelineSlider = timelineObject.GetComponent<Slider>();

        // Add the instantiated timeline object to the list of instantiated timeline objects
        instantiatedTimelineObjectList.Add(timelineObject);

        // Get the reference to the destroy timeline object script attached to the timeline object
        destroyTimelineObject = timelineObject.GetComponent<DestroyTimelineObject>();
        // Set the spawn time inside the object to the spawn time calculated from ticks previously
        destroyTimelineObject.TimelineHitObjectSpawnTime = _hitObjectSpawnTime;
        // Set default
        destroyTimelineObject.TimelineHitObjectAnimationType = ANIMATION_TYPE_NONE;
        destroyTimelineObject.TimelineHitObjectSoundType = SOUND_TYPE_CLAP;
        destroyTimelineObject.TimelineHitObjectType = _objectType;
        // Set position to current mouse position for cursor object
        destroyTimelineObject.TimelineHitObjectPosition = scriptManager.cursorHitObject.positionObject.transform.position;

        // Add to list
        destroyTimelineObjectList.Add(destroyTimelineObject);

        // Calculate the slider value based off the timeline hit object spawn time
        // Update the instantiated timeline hit object's slider to the correct value calculated from ticks
        timelineSlider.value = CalculateTimelineHitObjectSliderValue(_hitObjectSpawnTime); ;

        // Update the last saved slider value
        destroyTimelineObject.UpdateLastSavedSliderValue();

        // Increase the timeline object index
        timelineObjectIndex++;
    }

    // Calculate the timeline editor hit object sliders value based off the tick time converted to percentage of 0-1 slider value
    public float CalculateTimelineHitObjectSliderValue(float _spawnTime)
    {
        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            // Get how much % the spawn time is out of the entire clip length
            currentSongTimePercentage = (_spawnTime / scriptManager.rhythmVisualizatorPro.audioSource.clip.length);
        }

        // Calculate and return the percentage of 1 based on percentage of currentSongTimePercentage
        return (currentSongTimePercentage / 1);
    }

    // Get current beatsnap tick time
    public float GetCurrentBeatsnapTime(float _time)
    {
        if (scriptManager.metronomePro.CurrentTick != 0 && scriptManager.metronomePro.CurrentTick < scriptManager.metronomePro.songTickTimes.Count)
        {
            currentTickTime = (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick];
            tickTimesList.Add(currentTickTime);

            // Previous tick
            nextTickTime = (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick + 1];
            tickTimesList.Add(nextTickTime);

            // Previous tick
            float previousTickTime = (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick - 1];
            tickTimesList.Add(previousTickTime);


            // Check which time the users press was closest to
            closestTickTime = tickTimesList.Select(p => new { Value = p, Difference = Math.Abs(p - _time) })
            .OrderBy(p => p.Difference)
            .First().Value;
        }
        else
        {
            closestTickTime = 0;
        }

        // Reset list
        tickTimesList.Clear();
        return closestTickTime;
    }

    // Instantiate placed hit object at the position on the mouse
    public void InstantiateEditorPlacedHitObject(int _editorHitObjectType)
    {
        // Instantiate a new placed object onto the grid where the current cursor position is
        GameObject instantiatedhitObject = Instantiate(hitObjectPrefab[_editorHitObjectType], Vector3.zero, Quaternion.Euler(0, 0, 0));

        // Set the object to be in the canvas
        instantiatedhitObject.transform.SetParent(canvas);

        // Update the position
        instantiatedhitObject.transform.position = scriptManager.cursorHitObject.transform.position;
    }

    // Check if the spawn time for the hit object is taken or available
    private void CheckIfSpawnTimeIsTaken()
    {
        // Check through all the editor hit objects in the list, check if the spawn time exists already
        for (int i = 0; i < hitObjectList.Count; i++)
        {
            if (hitObjectSpawnTime == hitObjectList[i].HitObjectSpawnTime)
            {
                objectSpawnTimeIsTaken = true;
                break;
            }
        }
    }

    // Add a new editor hit object to the editorHitObjectList saving the spawn times, positions and object type. Instantiate a timeline object for this object also
    public void AddEditorHitObjectToList(int _objectType)
    {
        // Reset
        objectSpawnTimeIsTaken = false;

        if (scriptManager.beatsnapManager.BeatsnapTimingEnabled == true)
        {
            // If the audio source is playing
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying == true)
            {
                // Get the closest beatsnap time based on the current song time
                hitObjectSpawnTime = GetCurrentBeatsnapTime(userPressedTime);
            }
            else
            {
                // Audio source is not playing
                // Set the spawn time to the current tick
                hitObjectSpawnTime = (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick];
            }

            // Check if the spawn time for the hit object is taken or available
            CheckIfSpawnTimeIsTaken();
        }
        else
        {
            hitObjectSpawnTime = scriptManager.rhythmVisualizatorPro.audioSource.time;
        }

        // If the objects spawn time does not exist/is not taken, allow instantiation of another hit object
        if (objectSpawnTimeIsTaken == false)
        {
            // Create a new editor hit object (class object) and assign all the variables such as position, spawn time and type
            EditorHitObject newEditorHitObject = new EditorHitObject();

            // Set position rotate line to closest tick rotation
            // Save position of current grid point for hit object position

            // Set position to the type position on the grid point line
            newEditorHitObject.HitObjectPosition = scriptManager.pathPlacer.points[scriptManager.metronomePro.CurrentTick - 1];

            // Update properties of the hit object
            newEditorHitObject.HitObjectType = _objectType;
            newEditorHitObject.HitObjectSpawnTime = hitObjectSpawnTime;
            newEditorHitObject.HitObjectAnimationType = ANIMATION_TYPE_NONE;
            newEditorHitObject.HitObjectSoundType = SOUND_TYPE_CLAP;

            // Add the hit object to the editorHitObjectList
            hitObjectList.Add(newEditorHitObject);

            // Set the instantiate position to the editor hit object position but with a Y of 0
            // Spawn hit object from the pool at the cursors position
            SpawnFromPool(_objectType);

            // Call the instantiateTimelineObject function and pass the object type to instantiate a timeline object of the correct note color type
            //InstantiateTimelineObject(_objectType, hitObjectSpawnTime, _objectType);
            InstantiateTimelineObject(0, hitObjectSpawnTime, 0);

            // Reorder the editorHitObject list
            SortListOrders();

            // Update the timeline objects
            UpdateTimelineObjects();

            // If audio is not playing
            if (scriptManager.rhythmVisualizatorPro.audioSource.isPlaying == false)
            {
                // Navigate ahead 1 tick on the timeline
                scriptManager.timelineScript.TimelineNavigationForwardOneTick();

                // Update timeline object activation
                DisableTimelineObjects();
            }

            // Update cursor position
            scriptManager.cursorHitObject.UpdateDistanceSnapPosition();
        }
    }

    /*
    // Load the editor hit object and timeline object based from the file if editing an existing file
    public void LoadEditorHitObjectFromFile(int _objectType, float _spawnTime, Vector3 _position)
    {
        // Set the instantiate position to the editor hit object position but with a Y of 0
        InstantiateEditorPlacedHitObject(_objectType);

        // Call the instantiateTimelineObject function and pass the object type to instantiate a timeline object of the correct note color type
        InstantiateTimelineObject(_objectType, _spawnTime,);

        // Create a new editor hit object (class object) and assign all the variables such as position, spawn time and type
        EditorHitObject newEditorHitObject = new EditorHitObject();

        newEditorHitObject.HitObjectPosition = _position;
        newEditorHitObject.HitObjectType = _objectType;
        newEditorHitObject.HitObjectSpawnTime = _spawnTime;

        // Add the hit object to the editorHitObjectList
        hitObjectList.Add(newEditorHitObject);


        // Reorder the editorHitObject list
        SortListOrders();

        // Update the timeline objects
        UpdateTimelineObjects();
    }
    */

    // Sort all lists based on spawn time so they're in order
    public void SortListOrders()
    {
        hitObjectList = hitObjectList.OrderBy(w => w.HitObjectSpawnTime).ToList();

        // Sort the timeline script list by spawn time
        destroyTimelineObjectList = destroyTimelineObjectList.OrderBy(w => w.TimelineHitObjectSpawnTime).ToList();
    }

    // Disable timeline objects based on song time
    public void DisableTimelineObjects()
    {
        if (hitObjectList.Count != 0)
        {
            currentSongTime = scriptManager.rhythmVisualizatorPro.audioSource.time;

            for (int i = 0; i < hitObjectList.Count; i++)
            {
                // Get spawn time for timeline object
                // Check the current time
                // If current time is greater by 10 seconds of the hit object spawn time
                // Deactivate the timeline object

                timelineObjectSpawnTime = hitObjectList[i].HitObjectSpawnTime;
                deactivateAfterObjectTime = (timelineObjectSpawnTime + timelineObjectDeactivateValue);
                deactivateBeforeObjectTime = (timelineObjectSpawnTime - timelineObjectDeactivateValue);


                // If the current song time is greater than the time to deactivate the hit object based off its spawn time
                if (currentSongTime > deactivateAfterObjectTime || currentSongTime < deactivateBeforeObjectTime)
                {
                    // Deactivate the timeline hit object
                    if (instantiatedTimelineObjectList[i].gameObject.activeSelf == true)
                    {
                        instantiatedTimelineObjectList[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    // Keep the game object active
                    if (instantiatedTimelineObjectList[i].gameObject.activeSelf == false)
                    {
                        instantiatedTimelineObjectList[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    // Save all list information to the database script
    public void SaveListsToDatabase()
    {
        // Sort the editorHitObjects based on the spawn time
        SortListOrders();

        // Save to the database everything - spawn times, object type, positions
        for (int i = 0; i < hitObjectList.Count; i++)
        {
            // Add the positions to the database
            Database.database.PositionX.Add(hitObjectList[i].HitObjectPosition.x);
            Database.database.PositionY.Add(hitObjectList[i].HitObjectPosition.y);
            Database.database.PositionZ.Add(hitObjectList[i].HitObjectPosition.z);
            // Add the spawn times to the database
            Database.database.HitObjectSpawnTime.Add(hitObjectList[i].HitObjectSpawnTime);
            // Add the object type to the database
            Database.database.ObjectType.Add(hitObjectList[i].HitObjectType);
            Database.database.AnimationType.Add(hitObjectList[i].HitObjectAnimationType);
            Database.database.SoundType.Add(hitObjectList[i].HitObjectSoundType);
        }

        // Save
        Database.database.Save();
    }

}
