using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour {

    public GameObject timeline;
    Vector3 timelineCurrentPosition;
    float x;

    public GameObject reversedTimelineHandle;
    Vector3 reversedTimelineHandlePosition;
    Vector3 reversedTimelineHandleStartingPosition;

    void Start()
    {
        // Get the starting position for the reversed timeline handle
        reversedTimelineHandleStartingPosition = reversedTimelineHandle.transform.position;
    }

    // Update is called once per frame
    void Update () {

        // Get the position of the reversed timeline handle
        reversedTimelineHandlePosition = reversedTimelineHandle.transform.position;
        // Set the main timeline to the position of the reversed timeline handle
        timeline.transform.position = reversedTimelineHandlePosition;



        /*
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            x = timeline.transform.position.x;
            float y = timeline.transform.position.y;
            float z = timeline.transform.position.z;

            x += 684;
            Vector3 newTimelinePosition = new Vector3(x, y, z);
            timeline.transform.position = newTimelinePosition;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            x = timeline.transform.position.x;
            float y = timeline.transform.position.y;
            float z = timeline.transform.position.z;

            x -= 684;
            Vector3 newTimelinePosition = new Vector3(x, y, z);
            timeline.transform.position = newTimelinePosition;
        }
        */
    }

    // Reset the reversed Timeline sliders position
    public void ResetReversedTimelinePosition()
    {
        reversedTimelineHandlePosition = reversedTimelineHandleStartingPosition;
    }
}
