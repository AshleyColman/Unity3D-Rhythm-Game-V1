using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatsnapManager : MonoBehaviour
{

    // UI
    public Slider timelineSlider; // Get main timeline slider
    private Slider instantiatedBeatsnap; // Instantiated beatsnap slider

    // Bools
    private bool hasCheckedTickDifference;
    private bool beatsnapTimingEnabled;


    // Integers
    private float tickTimePercentage;
    private float currentSongTimePercentage; // Current percentage value of the song progresss
    private float sliderValue; // Slider value for the instantiated beatsnap slider based off the current song percentage
    private float beatsnapTime; // Beatsnap time value
    public List<float> beatsnapSliderValueList = new List<float>(); // Slider values of all instantiated beatsnap point sliders
    public List<float> beatsnapTickTimesList = new List<float>();
    public List<Slider> beatsnapGameObjectList = new List<Slider>();
    private float nextPoolTickTime; // The next time to change the current pool hit object to once its gone past its tick time
    private float currentTickTime; // Get the closest tick time based on the current song time
    private int totalBeatsnapPrefabsCount; // Total number of beatsnap prefabs instantiated
    private int previousFrameTick;
    private int nextPoolTickIndex;
    private int tick;

    // Scripts
    private ScriptManager scriptManager;

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
        beatsnapTimingEnabled = true;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        poolDictionary = new Dictionary<int, Queue<Slider>>();

        foreach (Pool pool in pools)
        {
            Queue<Slider> objectPool = new Queue<Slider>();

            for (int i = 0; i < pool.size; i++)
            {
                Slider obj = Instantiate(pool.prefab);

                /*

                obj.transform.SetParent(scriptManager.timelineScript.timelineSlider.transform);

                obj.transform.localPosition = new Vector3(7500, 0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);

                */


                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.transform.SetParent(scriptManager.timelineScript.timelineSlider.transform, false);

                // Get rect transform
                var rectTransform = obj.transform as RectTransform;

                // Stretch to fit the parent timeline object
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                // Reset position left, right, up, down
                rectTransform.offsetMin = new Vector2(0, 0);
                rectTransform.offsetMax = new Vector2(0, 0);


                obj.gameObject.SetActive(false);

                // Add to the list
                beatsnapGameObjectList.Add(obj);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.B) && scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            // Select the next beatsnap division
            SelectNextBeatsnapDivision();
        }
        */
    }

    // Select the next beatsnap division
    private void SelectNextBeatsnapDivision()
    {
        /*
        if (scriptManager.metronomePro.Division == 0)
        {
            // Load 8 beatsnap division
            scriptManager.metronomePro.UpdateBeatsnapDivision(8);
        }
        else if (scriptManager.metronomePro.Division == 8)
        {
            // Load 16 beatsnap division
            scriptManager.metronomePro.UpdateBeatsnapDivision(16);
        }
        else if (scriptManager.metronomePro.Division == 16)
        {
            // Load 0 beatsnap division
            scriptManager.metronomePro.UpdateBeatsnapDivision(0);
        }



        // Sort the beatsnaps with the new division
        SortBeatsnapsWithDivision();
        */
    }

    // Convert all song tick times to slider values
    public void CalculateBeatsnapSliderListValues()
    {
        for (int i = 0; i < scriptManager.metronomePro.songTickTimes.Count; i++)
        {
            sliderValue = scriptManager.placedObject.CalculateTimelineHitObjectSliderValue((float)scriptManager.metronomePro.songTickTimes[i]);
            beatsnapSliderValueList.Add(sliderValue);
        }
    }

    /*
    // Toggle beatsnap timing on or off
    public void ToggleBeatsnapTiming()
    {
        if (beatsnapTimingEnabled == true)
        {
            beatsnapTimingEnabled = false;
        }
        else if (beatsnapTimingEnabled == false)
        {
            beatsnapTimingEnabled = true;
        }
    }
    */

    // Generate beatsnap
    public void SetupBeatsnaps()
    {
        // Run if a song has been selected
        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            // Calculate the intervals
            scriptManager.metronomePro.CalculateIntervals();

            // Get total number of beatsnap prefabs in the lists
            totalBeatsnapPrefabsCount = poolDictionary[0].Count;

            for (int i = 0; i < poolDictionary[0].Count; i++)
            {
                // Get the closest tick time based on the current song time
                currentTickTime = (float)scriptManager.metronomePro.songTickTimes[i];

                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(currentTickTime);
            }
        }
    }


    // Sort the beatsnaps based on the current song position
    public void SortBeatsnaps()
    {
        // Get the closest tick time based on the current song time
        currentTickTime = scriptManager.placedObject.GetCurrentBeatsnapTime();

        for (int i = 0; i < poolDictionary[0].Count; i++)
        {
            // Get the next tick to place the beatsnap bar at
            tick = (scriptManager.metronomePro.CurrentTick + i);

            // Check if it's over the amount of ticks for the song
            if (tick < scriptManager.metronomePro.songTickTimes.Count)
            {
                currentTickTime = (float)scriptManager.metronomePro.songTickTimes[tick];
            }
            else
            {
                // Set it to the first tick in the beatmap
                currentTickTime = (float)scriptManager.metronomePro.songTickTimes[0];
            }

            // Spawn - activate the beatsnap object to appear at the end
            SpawnFromPool(currentTickTime);
        }
    }

    // Sort the beatsnaps based on the current song position
    public void SortBeatsnapsWithDivision()
    {
        // Run if a song has been selected
        if (scriptManager.rhythmVisualizatorPro.audioSource.clip != null)
        {
            // Calculate the intervals
            scriptManager.metronomePro.CalculateIntervals();
            // Calculate step
            scriptManager.metronomePro.CalculateActualStep();

            // Get total number of beatsnap prefabs in the lists
            totalBeatsnapPrefabsCount = poolDictionary[0].Count;

            // Get the closest tick time based on the current song time
            currentTickTime = scriptManager.placedObject.GetCurrentBeatsnapTime();



            for (int i = 0; i < poolDictionary[0].Count; i++)
            {
                // Get the next tick to place the beatsnap bar at
                tick = (scriptManager.metronomePro.CurrentTick + i);

                // Check if it's over the amount of ticks for the song
                if (tick < scriptManager.metronomePro.songTickTimes.Count)
                {
                    currentTickTime = (float)scriptManager.metronomePro.songTickTimes[tick];
                }
                else
                {
                    // Set it to the first tick in the beatmap
                    currentTickTime = (float)scriptManager.metronomePro.songTickTimes[0];
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
        // currentTickTime = scriptManager.placedObject.GetCurrentBeatsnapTime();

        currentTickTime = (float)scriptManager.metronomePro.songTickTimes[scriptManager.metronomePro.CurrentTick];

        // Get the next pool tick index based off the current tick + the total number of prefabs
        nextPoolTickIndex = scriptManager.metronomePro.CurrentTick + totalBeatsnapPrefabsCount;
        // Get what tick time is from x objects away in the tick time array
        nextPoolTickTime = (float)scriptManager.metronomePro.songTickTimes[nextPoolTickIndex];


        if (previousFrameTick != scriptManager.metronomePro.CurrentTick || previousFrameTick == 0)
        {
            // Check if the current song time has gone past the current beatsnaps tick time
            if (scriptManager.rhythmVisualizatorPro.audioSource.time >= currentTickTime)
            {
                // Spawn - activate the beatsnap object to appear at the end
                SpawnFromPool(nextPoolTickTime);
            }
        }

        previousFrameTick = scriptManager.metronomePro.CurrentTick;
    }
    */

    private void SpawnFromPool(float _tickTime)
    {
        Slider objectToSpawn = poolDictionary[0].Dequeue();

        // Get how much % the spawn time is out of the entire clip length
        currentSongTimePercentage = (_tickTime / scriptManager.rhythmVisualizatorPro.audioSource.clip.length);

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
        tickTimePercentage = 0;
        currentSongTimePercentage = 0;
        sliderValue = 0;
        beatsnapTime = 0;
        hasCheckedTickDifference = false;

        beatsnapSliderValueList.Clear();
    }
}

