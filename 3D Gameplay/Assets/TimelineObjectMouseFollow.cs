using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineObjectMouseFollow : MonoBehaviour {

    private float distance = 500f;
    public Vector3 pos;
    private float posX;
    private float posY;
    private float posZ;


    public void Update()
    {

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        pos = r.GetPoint(distance);

        posX = pos.x;
        posY = 0;
        posZ = transform.position.y;

        if (posX <= -345)
        {
            posX = -345;
        }
        if (posX >= 342)
        {
            posX = 342;
        }

        Vector3 newTimelineObjectPosition = new Vector3(posX, 0, posZ);
        transform.position = newTimelineObjectPosition;


    }
}
