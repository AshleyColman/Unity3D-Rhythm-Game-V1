using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLookAtYOnly : MonoBehaviour {

    public float sensitivity = 1;
    public float newRotationX;



    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        newRotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;
        newRotationX = Mathf.Clamp(newRotationX, 0, 0.5f);

        gameObject.transform.localEulerAngles = new Vector3(newRotationX, 0, 0);

    }
}
