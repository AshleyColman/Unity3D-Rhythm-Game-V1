using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {

    public float distance = 10f;
    public Vector3 pos;

    public void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        pos = r.GetPoint(distance);
        transform.position = pos;
    }
}
