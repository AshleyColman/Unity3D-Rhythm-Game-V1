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
    private float nextPoolTickTime; // The next time to change the current pool hit object to once its gone past its tick time
    private float currentTickTime; // Get the closest tick time based on the current song time
    private int totalBeatsnapPrefabsCount; // Total number of beatsnap prefabs instantiated
    private int previousFrameTick;
    private int nextPoolTickIndex;
    private int tick;

    private const float LINE_HEIGHT_35 = 35, LINE_HEIGHT_25 = 25, LINE_HEIGHT_15 = 15, LINE_HEIGHT_10 = 10, LINE_HEIGHT_5 = 5;

    // Scripts
    private ScriptManager scriptManager;
    private BeatsnapObject beatsnapObjectScript; // Beatline script for changing beatsnap bar properties

    // Properties
    public bool BeatsnapTimingEnabled
    {
        get { return beatsnapTimingEnabled; }
    }

    public List<Slider> beatsnapSliderList = new List<Slider>();
    public Slider beatsnapPrefab;
    private const int BEATSNAP_LIST_SIZE = 130;
    private int currentSliderListIndex;

    private void Start()
    {
        // Initialize
        beatsnapTimingEnabled = true;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        currentSliderListIndex = 0;

        // Instantiate list of beatsnaps
        for (int i = 0; i < BEATSNAP_LIST_SIZE; i++)
        {
            Slider obj = Instantiate(beatsnapPrefab);

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
    
            // Deactivate
            obj.gameObject.SetActive(false);

            // Add to the list
            beatsnapSliderList.Add(obj);
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
        /*
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
        */
    }

    public void SortLatestBeatsnap()
    {
        // Check if the current tick + total pool numbers is greater than the song list size
        int currentStep = scriptManager.metronomePro.currentStep;

        // Get tick time for the beatsnap object
        int tickTimeIndex = scriptManager.metronomePro.CurrentTick + beatsnapSliderList.Count;

        // Beatsnap time to pass 
        float beatsnapTime;

        if (tickTimeIndex < scriptManager.metronomePro.songTickTimes.Count)
        {
            // Set beatsnap time to the current tick + total beatsnap bars (pushed to the end) - 1 (avoids beat 0)
            beatsnapTime = (float)scriptManager.metronomePro.songTickTimes[tickTimeIndex] - 1;

            // Spawn next beatsnap object
            SpawnBeatsnapFromPool(beatsnapTime, currentStep);
        }
        else
        {
            // Deactivate the next beatsnap
            DeactivateBeatsnapFromPool();
        }
    }

    public void SortBeatsnapsWithNewDivision()
    {
        scriptManager.metronomePro.CalculateIntervals();
        scriptManager.metronomePro.CalculateActualStep();
        SortBeatsnaps();
    }

    // Sort the beatsnaps based on the current song position
    public void SortBeatsnaps()
    {
        // Based on the currentStep
        // Update all visuals for the beatsnaps

        // Get the closest tick time based on the current song time
        currentTickTime = scriptManager.placedObject.GetCurrentBeatsnapTime();

        // Get the current step
        int currentStep = scriptManager.metronomePro.currentStep;

        int tickTimeIndex;

        // Loop through all beatsnap objects
        for (int i = 0; i < beatsnapSliderList.Count; i++)
        {
            // Get the tick index
            tickTimeIndex = scriptManager.metronomePro.CurrentTick + i;

            // If the index is within the song tick time list length
            if (tickTimeIndex < scriptManager.metronomePro.songTickTimes.Count)
            {
                // Set the beatsnap tick time to the tick index time
                currentTickTime = (float)scriptManager.metronomePro.songTickTimes[tickTimeIndex];
            }
            else
            {
                // Deactivate the next beatsnap
                //DeactivateBeatsnapFromPool();
            }

            if (currentStep > 4)
            {
                currentStep = 1;
            }

            SpawnBeatsnapFromPool(currentTickTime, currentStep);

            currentStep++;
        }

        /*
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
        */
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

    /*
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
    */

    // Deactivate beatsnap from pool
    private void DeactivateBeatsnapFromPool()
    {
        // Error check list size
        if (currentSliderListIndex == beatsnapSliderList.Count)
        {
            currentSliderListIndex = 0;
        }

        Slider objectToSpawn = beatsnapSliderList[currentSliderListIndex];

        if (objectToSpawn.gameObject.activeSelf == true)
        {
            objectToSpawn.gameObject.SetActive(false);
        }

        // Increment current index
        currentSliderListIndex++;
    }

    private void SpawnBeatsnapFromPool(float _tickTime, int _step)
    {
        // Check the current slider list index to make sure its within the list
        if (currentSliderListIndex == beatsnapSliderList.Count)
        {
            // Set to 0 start of the list
            currentSliderListIndex = 0;
        }

        Slider objectToSpawn = beatsnapSliderList[currentSliderListIndex];

        // Get how much % the spawn time is out of the entire clip length
        currentSongTimePercentage = (_tickTime / scriptManager.rhythmVisualizatorPro.audioSource.clip.length);

        // Calculate percentage of 1 based on percentage of currentSongTimePercentage
        sliderValue = (currentSongTimePercentage / 1);

        // Set the timeline slider value to the tick time converted value 
        objectToSpawn.value = sliderValue;

        // Get rect transform for the image
        var rectTransform = objectToSpawn.GetComponent<BeatsnapObject>().beatsnapImage.transform as RectTransform;
        // Reference script for the object
        beatsnapObjectScript = objectToSpawn.GetComponent<BeatsnapObject>();


        switch (_step)
        {
            case 4:
                // Update image color
                beatsnapObjectScript.beatsnapImage.color = scriptManager.colorManager.selectedColor;
                // Update size
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, LINE_HEIGHT_15);
                break;
            default:
                // Update image color
                beatsnapObjectScript.beatsnapImage.color = scriptManager.colorManager.whiteColor;
                // Update size
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, LINE_HEIGHT_10);
                break;
        }

        // Activate the object
        if (objectToSpawn.gameObject.activeSelf == false)
        {
            objectToSpawn.gameObject.SetActive(true);
        }

        // Increment current slider list index
        currentSliderListIndex++;
    }

    // Reset beatsnapManager
    private void ResetBeatsnapManager()
    {
        timelineSlider.value = 0f;
        tickTimePercentage = 0;
        currentSongTimePercentage = 0;
        sliderValue = 0;
        beatsnapTime = 0;
        currentSliderListIndex = 0;
        hasCheckedTickDifference = false;

        beatsnapSliderValueList.Clear();
    }
}

