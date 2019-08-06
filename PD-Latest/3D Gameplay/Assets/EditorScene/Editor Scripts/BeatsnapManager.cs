using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatsnapManager : MonoBehaviour {

    // UI
    public Slider beatsnapPoint; // Get beatsnap point
    public Slider timelineSlider; // Get main timeline slider
    private Slider instantiatedBeatsnap; // Instantiated beatsnap slider
    public List<Slider> instantiatedBeatsnapTimelineObjectList = new List<Slider>(); // List of instantiated beatsnap objects

    // Gameobjects
    public GameObject timelineCurrentTimeHandle; // Get the main timeline sliders current song position handle, used for getting the position of and spawning beatsnap objects at its current position

    // Audio
    public AudioSource songAudioSource; // The song audio source

    // Bools
    private bool hasCheckedTickDifference;

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

    // Vectors
    private Vector3 timelineCurrentTimeHandlePosition; // The position of the currentTimeHandle

    // Transforms
    public Transform timeline; // Timeline transform

    // Scripts
    private MetronomePro metronomePro; // Reference to metronomePro for tick times

    

    private void Start()
    {
        // Initialize
        instantiationTime = 0.1f;

        // Reference
        metronomePro = FindObjectOfType<MetronomePro>();
        songAudioSource = metronomePro.songAudioSource;
    }

    // Generate beatsnap
    public void GenerateBeatsnaps()
    {
        // Destroy all preview timeline objects
        DestroyBeatsnapTimelineObjects();

        // Calculate the intervals
        metronomePro.CalculateIntervals();

        // Instantiate a beatsnap object at the current timeline's handle position
        StartCoroutine(GraduallyInstantiate());
    }

    // Instantiate beatsnap timeline object
    private void InstantiateBeatsnapTimelineObject(int _iCount)
    {
        // Instantiate a new beatsnap object in the timeline
        instantiatedBeatsnap = Instantiate(beatsnapPoint, transform.position, Quaternion.Euler(90, 0, 0), timeline);

        // Add the beatsnap game object to the list of instantiated beatsnap timeline objects, and instantiate in the game
        instantiatedBeatsnapTimelineObjectList.Add(instantiatedBeatsnap);

        // Get how much % the spawn time is out of the entire clip length
        currentSongTimePercentage = ((float)metronomePro.songTickTimes[_iCount] / metronomePro.songAudioSource.clip.length);

        // Calculate percentage of 1 based on percentage of currentSongTimePercentage
        sliderValue = (currentSongTimePercentage / 1);

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
    private void SetBeatsnapTime(int _iCount)
    {
        // Get the BeatsnapTimelineObject script component from the instantiated beatsnap object
        BeatsnapTimelineObject beatsnapTimelineObject = instantiatedBeatsnapTimelineObjectList[_iCount].GetComponent<BeatsnapTimelineObject>();

        // Get the time for the tick for the beatsnap timeline object
        beatsnapTime = (float)metronomePro.songTickTimes[_iCount];
        // Set the beatsnapTime for the beatsnapTimelineObject
        beatsnapTimelineObject.SetBeatsnapTime(beatsnapTime);
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

    // Destroy all instantiatedBeatsnapTimelineObjects
    public void DestroyBeatsnapTimelineObjects()
    {
        StopAllCoroutines();

        if (instantiatedBeatsnapTimelineObjectList.Count != 0)
        {
            for (int i = 0; i < instantiatedBeatsnapTimelineObjectList.Count; i++)
            {
                Destroy(instantiatedBeatsnapTimelineObjectList[i]);
            }

            instantiatedBeatsnapTimelineObjectList.Clear();
            ResetBeatsnapManager();
        }
    }
}

