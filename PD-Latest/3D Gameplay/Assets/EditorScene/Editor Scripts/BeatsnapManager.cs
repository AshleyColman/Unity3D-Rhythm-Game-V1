using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BeatsnapManager : MonoBehaviour {

    // Get beatsnap point
    public Slider beatsnapPoint;

    // Get main timeline slider
    public Slider timelineSlider;

    // Get the main timeline sliders current song position handle, used for getting the position of and spawning beatsnap objects at its current position
    public GameObject timelineCurrentTimeHandle;

    private float timelineValueDifferencePerTick;

    private float instantiatedSliderValue;

    // The position of the currentTimeHandle
    private Vector3 timelineCurrentTimeHandlePosition;

    // Reference to metronomePro for tick times
    private MetronomePro metronomePro;

    // The song audio source
    public AudioSource songAudioSource;

    // List of instantiated beatsnap objects
    public List<Slider> instantiatedBeatsnapTimelineObjectList = new List<Slider>();

    // Slider values of all instantiated beatsnap point sliders
    public List<float> beatsnapSliderValueList = new List<float>();

    // The time between intantiations
    private float instantiationTime = 0.1f;

    float tickDifference;

    float tickTimePercentage;

    bool hasCheckedTickDifference;

    float sliderValue;

    private void Start()
    {
        // Get reference to the metronomePro
        metronomePro = FindObjectOfType<MetronomePro>();

        // Get reference to the song audio source
        songAudioSource = metronomePro.songAudioSource;

        // Generate the beatsnaps
        //GenerateBeatsnaps();
    }

    // Generate beatsnap
    public void GenerateBeatsnaps()
    {
        // Destroy all preview timeline objects
        DestroyBeatsnapTimelineObjects();

        // Calculate the intervals
        metronomePro.CalculateIntervals();

        /*
            // Based off the difference for song tick time slider value, use the difference to instantiate the remaining song tick time objects
            if (hasCheckedTickDifference == false)
            {
                // tick time = 6
                // song duration = 60 seconds
                // get percentage

                // Convert 10% into the percentage from 0-1
                // 10% of 1 = 0.1
                // slider value = 0.1

                // Get tick time
                // Convert to slider value
                // Set timeline bar to that value when spawned


                tickDifference = ((float)metronomePro.songTickTimes[1] - (float)metronomePro.songTickTimes[0]);
                tickTimePercentage = (tickDifference / songAudioSource.clip.length);

                // Calculate percentage of 1 based on percentage of songPercentage
                sliderValue = (tickTimePercentage / 1);

                // Get the difference for 1 tick for the slider value
                timelineValueDifferencePerTick = sliderValue;

        }

        */

         // Instantiate a beatsnap object at the current timeline's handle position
         StartCoroutine(GraduallyInstantiate());
    }

    // Instantiate beatsnap timeline object
    private void InstantiateBeatsnapTimelineObject(int iCountPass)
    {
        // Instantiate by ignoring the y? 

        Slider instantiatedBeatsnap = Instantiate(beatsnapPoint, transform.position, Quaternion.Euler(90, 0, 0),
        GameObject.FindGameObjectWithTag("Timeline").transform);

        // Add the beatsnap game object to the list of instantiated beatsnap timeline objects, and instantiate in the game
        instantiatedBeatsnapTimelineObjectList.Add(instantiatedBeatsnap);


        /*
        // Add the offset slider value to the tick slider value also for the first tick, else only add the tick per difference value to each slider
        if (iCountPass == 0)
        {
            //HOW MUCH % SLIDER VALUE THE OFFSET IS, THEN ADD IT TO THE FIRST ONE
            // Get the offset in millisconds
            float offset = ((float)metronomePro.OffsetMS / 1000);

            // Get how much % the offset is out of the entire song
            float offsetPercentage = (offset / songAudioSource.clip.length);

            // Calculate percentage of 1 based on percentage of offsetPercentage
            float offsetSliderValue = (offsetPercentage / 1);

            // For the first tick time add the offset slider value + the timeline value difference per tick
            instantiatedSliderValue += (timelineValueDifferencePerTick + offsetSliderValue);
        }
        else
        {
            // Increment the instantiated slider value
            instantiatedSliderValue += timelineValueDifferencePerTick;
        }
        

        // Increment the instantiated slider value
        instantiatedSliderValue += timelineValueDifferencePerTick;

        // Set the timeline slider value to the current song time handles value
        beatsnapPoint.value = instantiatedSliderValue;
        */


        // Get how much % the spawn time is out of the entire clip length
        float currentSongTimePercentage = ((float)metronomePro.songTickTimes[iCountPass] / metronomePro.songAudioSource.clip.length);

        // Calculate percentage of 1 based on percentage of currentSongTimePercentage
        float sliderValue = (currentSongTimePercentage / 1);

        // Set the timeline slider value to the tick time converted value 
        beatsnapPoint.value = sliderValue;

        // Add the slider value for the beatsnap to the list
        beatsnapSliderValueList.Add(beatsnapPoint.value);
    }

    //This coroutine gradually creates gameobjects of the given prefab.
    IEnumerator GraduallyInstantiate()
    {
        // Loop through all song tick times and set the song time to the tick time, moving the slider, instantiate a beatsnap timeline object at the timeline position
        for (int iCount = 0; iCount < metronomePro.songTickTimes.Count; iCount++)
        {
            // Instantiate the beatsnap timeline object
            InstantiateBeatsnapTimelineObject(iCount);
            // Set the beatsnap time for the timeline object
            SetBeatsnapTime(iCount);
            yield return new WaitForSeconds(instantiationTime);
        }
    }

    // Set the beatsnap time for the beatsnapTimelineObject
    private void SetBeatsnapTime(int iCountPass)
    {
        // Get the BeatsnapTimelineObject script component from the instantiated beatsnap object
        BeatsnapTimelineObject beatsnapTimelineObject = instantiatedBeatsnapTimelineObjectList[iCountPass].GetComponent<BeatsnapTimelineObject>();

        // Get the time for the tick for the beatsnap timeline object
        float beatsnapTime = (float)metronomePro.songTickTimes[iCountPass];
        // Set the beatsnapTime for the beatsnapTimelineObject
        beatsnapTimelineObject.SetBeatsnapTime(beatsnapTime);
    }

    // Reset beatsnapManager
    private void ResetBeatsnapManager()
    {
        timelineSlider.value = 0f;

        instantiatedSliderValue = 0f;

        tickDifference = 0f;

        tickTimePercentage = 0f;

        hasCheckedTickDifference = false;

        beatsnapSliderValueList.Clear();
    }

    // Destroy all instantiatedBeatsnapTimelineObjects
    public void DestroyBeatsnapTimelineObjects()
    {
        StopAllCoroutines();
        ResetBeatsnapManager();
        if (instantiatedBeatsnapTimelineObjectList.Count != 0)
        {
            for (int i = 0; i < instantiatedBeatsnapTimelineObjectList.Count; i++)
            {
                Destroy(instantiatedBeatsnapTimelineObjectList[i].gameObject);
            }

            instantiatedBeatsnapTimelineObjectList.Clear();
        }
    }
}

