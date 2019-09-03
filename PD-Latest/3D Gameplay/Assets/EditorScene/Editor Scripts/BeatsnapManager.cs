using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatsnapManager : MonoBehaviour {

    // UI
    public Slider beatsnapPoint; // Get beatsnap point
    public Slider timelineSlider; // Get main timeline slider
    private Slider instantiatedBeatsnap; // Instantiated beatsnap slider
    public TextMeshProUGUI beatsnapTimingOnText, beatsnapTimingOffText; // Text for toggling beatsnap timing on or off
    private Slider objSlider;

    // Gameobjects
    public GameObject timelineCurrentTimeHandle; // Get the main timeline sliders current song position handle, used for getting the position of and spawning beatsnap objects at its current position

    // Audio
    public AudioSource songAudioSource; // The song audio source

    // Bools
    private bool hasCheckedTickDifference;
    private bool beatsnapTimingEnabled;


    // Integers
    private float instantiationTime;// The time between intantiations
    private float timelineValueDifferencePerTick;
    private float instantiatedSliderValue;
    private float tickDifference;
    private float tickTimePercentage;
    private float currentSongTimePercentage; // Current percentage value of the song progresss
    private float sliderValue; // Slider value for the instantiated beatsnap slider based off the current song percentage
    private float beatsnapTime; // Beatsnap time value
    public List<float> beatsnapSliderValueList = new List<float>(); // Slider values of all instantiated beatsnap point sliders
    public List<float> beatsnapTickTimesList = new List<float>();
    public List<Slider> beatsnapGameObjectList = new List<Slider>();
    public float nextPoolTickTime; // The next time to change the current pool hit object to once its gone past its tick time
    public float currentTickTime; // Get the closest tick time based on the current song time
    private int totalBeatsnapPrefabsCount; // Total number of beatsnap prefabs instantiated
    public int previousFrameTick;
    public int nextPoolTickIndex;
    int tick;

    // Vectors
    private Vector3 timelineCurrentTimeHandlePosition; // The position of the currentTimeHandle

    // Transforms
    public Transform timeline; // Timeline transform

    // Scripts
    private MetronomePro metronomePro; // Reference to metronomePro for tick times
    private PlacedObject placedObject;

    // Properties
    public bool BeatsnapTimingEnabled
    {
        get { return beatsnapTimingEnabled; }
    }


    [System.Serializable]
    public class Pool
    {
        public int tag;
        public Slider prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<int, Queue<Slider>> poolDictionary;

    private void Start()
    {
        // Initialize
        instantiationTime = 0.1f;
        beatsnapTimingEnabled = true;

        beatsnapTimingOffText.gameObject.SetActive(false);
        beatsnapTimingOnText.gameObject.SetActive(true);

        // Reference
        metronomePro = FindObjectOfType<MetronomePro>();
        songAudioSource = metronomePro.songAudioSource;
        placedObject = FindObjectOfType<PlacedObject>();


        poolDictionary = new Dictionary<int, Queue<Slider>>();

        foreach (Pool pool in pools)
        {
            Queue<Slider> objectPool = new Queue<Slider>();

            for (int i = 0; i < pool.size; i++)
            {
                Slider obj = Instantiate(pool.prefab);
                obj.transform.SetParent(timeline);

                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
 
                obj.gameObject.SetActive(false);

                // Add to the list
                beatsnapGameObjectList.Add(obj);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Convert all song tick times to slider values
    public void CalculateBeatsnapSliderListValues()
    {
        for (int i = 0; i < metronomePro.songTickTimes.Count; i++)
        {
            sliderValue = placedObject.CalculateTimelineHitObjectSliderValue((float)metronomePro.songTickTimes[i]);
            beatsnapSliderValueList.Add(sliderValue);
        }
    }

    // Toggle beatsnap timing on or off
    public void ToggleBeatsnapTiming()
    {
        if (beatsnapTimingEnabled == true)
        {
            beatsnapTimingOffText.gameObject.SetActive(true);
            beatsnapTimingOnText.gameObject.SetActive(false);
            beatsnapTimingEnabled = false;
        }
        else if (beatsnapTimingEnabled == false)
        {
            beatsnapTimingOffText.gameObject.SetActive(false);
            beatsnapTimingOnText.gameObject.SetActive(true);
            beatsnapTimingEnabled = true;
        }

    }

    // Generate beatsnap
    public void SetupBeatsnaps()
    {
        // Run if a song has been selected
        if (metronomePro.songAudioSource.clip != null)
        {
            // Calculate the intervals
            metronomePro.CalculateIntervals();

            // Get total number of beatsnap prefabs in the lists
            totalBeatsnapPrefabsCount = poolDictionary[0].Count;

            for (int i = 0; i < poolDictionary[0].Count; i++)
            {
                // Get the closest tick time based on the current song time
                currentTickTime = (float)metronomePro.songTickTimes[i];

                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(currentTickTime);
            }
        }
    }


    // Sort the beatsnaps based on the current song position
    public void SortBeatsnaps()
    {
        // Run if a song has been selected
        if (metronomePro.songAudioSource.clip != null)
        {
            // Get total number of beatsnap prefabs in the lists
            totalBeatsnapPrefabsCount = poolDictionary[0].Count;

            // Get the closest tick time based on the current song time
            currentTickTime = placedObject.GetCurrentBeatsnapTime();



            for (int i = 0; i < poolDictionary[0].Count; i++)
            {
                // Get the next tick to place the beatsnap bar at
                tick = (metronomePro.CurrentTick + i);

                // Check if it's over the amount of ticks for the song
                if (tick < metronomePro.songTickTimes.Count)
                {
                    currentTickTime = (float)metronomePro.songTickTimes[tick];
                }
                else
                {
                    // Set it to the first tick in the beatmap
                    currentTickTime = (float)metronomePro.songTickTimes[0];
                }

                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(currentTickTime);
            }
        }
    }

    // Sort the beatsnaps based on the current song position
    public void SortBeatsnapsWithDivision()
    {
        // Run if a song has been selected
        if (metronomePro.songAudioSource.clip != null)
        {
            // Calculate the intervals
            metronomePro.CalculateIntervals();
            // Calculate step
            metronomePro.CalculateActualStep();

            // Get total number of beatsnap prefabs in the lists
            totalBeatsnapPrefabsCount = poolDictionary[0].Count;

            // Get the closest tick time based on the current song time
            currentTickTime = placedObject.GetCurrentBeatsnapTime();



            for (int i = 0; i < poolDictionary[0].Count; i++)
            {
                // Get the next tick to place the beatsnap bar at
                tick = (metronomePro.CurrentTick + i);

                // Check if it's over the amount of ticks for the song
                if (tick < metronomePro.songTickTimes.Count)
                {
                    currentTickTime = (float)metronomePro.songTickTimes[tick];
                }
                else
                {
                    // Set it to the first tick in the beatmap
                    currentTickTime = (float)metronomePro.songTickTimes[0];
                }

                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(currentTickTime);
            }
        }
    }


    /*
    // Generate beatsnap
    public void UpdateBeatsnaps()
    {


        totalBeatsnapPrefabsCount = poolDictionary[0].Count;

        // Get the closest tick time based on the current song time
        //currentTickTime = placedObject.GetCurrentBeatsnapTime();

        int currentTick = metronomePro.CurrentTick;
        currentTickTime = (float)metronomePro.songTickTimes[currentTick];
        Debug.Log("currenttick " + currentTick);
        Debug.Log("currenticktime " + currentTickTime);

        // Get the next pool tick index based off the current tick + the total number of prefabs
        nextPoolTickIndex = metronomePro.CurrentTick + totalBeatsnapPrefabsCount;
        // Get what tick time is from x objects away in the tick time array
        nextPoolTickTime = (float)metronomePro.songTickTimes[nextPoolTickIndex];


        if (previousFrameTick != metronomePro.CurrentTick || previousFrameTick == 0)
        {
            // Check if the current song time has gone past the current beatsnaps tick time
            if (metronomePro.songAudioSource.time >= currentTickTime)
            {
                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(nextPoolTickTime);
            }
        }

        previousFrameTick = metronomePro.CurrentTick;
    }
    */

    private void SpawnFromPool(float _tickTime)
    {
        Slider objectToSpawn = poolDictionary[0].Dequeue();

        // Get how much % the spawn time is out of the entire clip length
        currentSongTimePercentage = (_tickTime / metronomePro.songAudioSource.clip.length);

        // Calculate percentage of 1 based on percentage of currentSongTimePercentage
        sliderValue = (currentSongTimePercentage / 1);

        // Set the timeline slider value to the tick time converted value 
        objectToSpawn.value = sliderValue;


        // Activate the object
        if (objectToSpawn.gameObject.activeSelf == false)
        {
            objectToSpawn.gameObject.SetActive(true);
        }

        poolDictionary[0].Enqueue(objectToSpawn);
    }


    // Reset beatsnapManager
    private void ResetBeatsnapManager()
    {
        timelineSlider.value = 0f;
        instantiationTime = 0;
        timelineValueDifferencePerTick = 0;
        instantiatedSliderValue = 0;
        tickDifference = 0;
        tickTimePercentage = 0;
        currentSongTimePercentage = 0;
        sliderValue = 0;
        beatsnapTime = 0;
        hasCheckedTickDifference = false;

        beatsnapSliderValueList.Clear();
    }
}

