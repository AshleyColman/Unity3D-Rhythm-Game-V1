using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineHandle : MonoBehaviour {

    public Slider timeline;
    private float x;
    public GameObject timelineHandle;
    private MetronomePro_Player MetronomePro_Player;

    private void Start()
    {
        MetronomePro_Player = FindObjectOfType<MetronomePro_Player>();
    }
    // Disable the behaviour when it becomes invisible...
    void OnBecameInvisible()
    {
        // If the song is playing (do a check)
        if (MetronomePro_Player.playing == true && MetronomePro_Player != null)
        {
            if (timeline != null)
            {
                x = timeline.transform.position.x;
                float y = timeline.transform.position.y;
                float z = timeline.transform.position.z;

                x -= 684;
                Vector3 newTimelinePosition = new Vector3(x, y, z);
                timeline.transform.position = newTimelinePosition;
            }
        }
    }


    private void Update()
    {
        // Constantly set the cube to the timeline handles position
        gameObject.transform.position = timelineHandle.transform.position;
    }
}
