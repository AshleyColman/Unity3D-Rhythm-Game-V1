using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTimelineObject : MonoBehaviour
{


    public GameObject raycastObject;

    private float distance = 500f;
    public Vector3 pos;

    private TimelineObjectMouseFollow timelineObjectMouseFollow;
    private bool active;

    private void Update()
    {
        // Send a ray to find the object at the mouse postion
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;


        // Send a raycast and get the object in view
        if (Physics.Raycast(ray, out hit))
        {
            // Gets the object the ray is casting to
            raycastObject = hit.collider.transform.parent.gameObject;
            
            // Check if the object is a timeline object
            if (raycastObject.tag == "TimelineObject")
            {
                // Follow the mouse if the object has been left clicked by turning on the mouse follow script attached
                if (Input.GetMouseButtonDown(0) && active == false)
                {
                    timelineObjectMouseFollow = raycastObject.GetComponent<TimelineObjectMouseFollow>();
                    timelineObjectMouseFollow.enabled = true;
                    active = true;
                }
                // Disable if the timeline object was previously following the mouse and left click has been pressed again
                else if (Input.GetMouseButtonDown(0) && active == true)
                {
                    timelineObjectMouseFollow.enabled = false;
                    timelineObjectMouseFollow.enabled = false;
                    active = false;
                }
            }

        }

    }

}
