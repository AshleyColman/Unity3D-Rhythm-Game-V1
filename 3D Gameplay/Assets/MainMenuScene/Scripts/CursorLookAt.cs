using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLookAt : MonoBehaviour {

    public float sensitivity = 1;
    public float newRotationY;
    public float newRotationX;

    //public GameObject cameraObject;
    //public float cameraPositionX;
    //public float cameraPositionY;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        // Get the camera rotations for preventing off screen rotation
        //cameraPositionX = cameraObject.transform.rotation.eulerAngles.x;
        //cameraPositionY = cameraObject.transform.rotation.eulerAngles.y;


        // Get the angles and restrict movement after certain amount
        newRotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        newRotationY = Mathf.Clamp(newRotationY, 0, 3);
        newRotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;
        newRotationX = Mathf.Clamp(newRotationX, 0, 3);

        gameObject.transform.localEulerAngles = new Vector3(newRotationX, newRotationY, 0);

    }
}
