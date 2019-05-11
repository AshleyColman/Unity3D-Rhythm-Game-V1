using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpecialTimelineObject : MonoBehaviour {


    private PlacedObject placedObject;
    public float timelineHitObjectSpawnTime;
    public Slider timelineSlider;
    public MetronomePro_Player metronome_Player;
    private int timelineObjectListIndex;

    private void Start()
    {
        timelineObjectListIndex = 0;

        placedObject = FindObjectOfType<PlacedObject>();

        // Get the reference to the timelines own slider
        timelineSlider = this.gameObject.GetComponent<Slider>();

        // Get the reference to the metronome_Player
        metronome_Player = FindObjectOfType<MetronomePro_Player>();
    }

    // Update the timelines spawn time
    public void UpdateTimelineHitObjectSpawnTime()
    {
        if (metronome_Player != null)
        {
            timelineHitObjectSpawnTime = metronome_Player.UpdateTimelineHitObjectSpawnTimes(timelineSlider);
        }
    }


            // Update the special time start/end times
            public void UpdateSpecialTimeValues()
    {
        if (metronome_Player != null)
        {
            // Update the start time if the timeline objects tag is the start timeline object
            if (gameObject.tag == "SpecialTimeStartTimelineObject")
            {
                placedObject.UpdateSpecialTimeStart(timelineHitObjectSpawnTime);
            }

            // Update the end time if the timeline objects tag is the end timeline object
            if (gameObject.tag == "SpecialTimeEndTimelineObject")
            {
                placedObject.UpdateSpecialTimeEnd(timelineHitObjectSpawnTime);
            }

        }
    }
}
