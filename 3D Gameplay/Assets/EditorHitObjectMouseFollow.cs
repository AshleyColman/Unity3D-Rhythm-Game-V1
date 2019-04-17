using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorHitObjectMouseFollow : MonoBehaviour {

    private float distance = 500f;
    private Vector3 pos;

	// Update is called once per frame
	void Update () {

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        pos = r.GetPoint(distance);
        transform.position = pos;

	}
}
