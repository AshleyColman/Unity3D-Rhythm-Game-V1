using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour {

    public GameObject timeline;
    Vector3 timelineCurrentPosition;
    float x;

    // Update is called once per frame
    void Update () {
		

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            x = timeline.transform.position.x;
            float y = timeline.transform.position.y;
            float z = timeline.transform.position.z;

            x -= 100;
            Vector3 newTimelinePosition = new Vector3(x, y, z);
            timeline.transform.position = newTimelinePosition;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            x = timeline.transform.position.x;
            float y = timeline.transform.position.y;
            float z = timeline.transform.position.z;

            x += 100;
            Vector3 newTimelinePosition = new Vector3(x, y, z);
            timeline.transform.position = newTimelinePosition;
        }
    }
}
