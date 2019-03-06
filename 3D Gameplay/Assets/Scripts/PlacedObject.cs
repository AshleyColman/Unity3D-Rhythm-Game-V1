using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour {

    public GameObject editorPlacedHitObject;
    public MouseFollow mouseFollow; // Get the position of the mouse when pressed for placement
    int totalEditorHitObjects = 0;
    List<Vector3> editorHitObjectPositions = new List<Vector3>();
    Vector3 instantiatePosition;
    bool hasClickedUIButton = false;

    // Use this for initialization
    void Start () {
        mouseFollow = FindObjectOfType<MouseFollow>();
    }
	
	// Update is called once per frame
	void Update () {

        // Place a hit object only if the mouse has been clicked and the UI button has been clicked
        if (Input.GetMouseButtonDown(0) && hasClickedUIButton == true)
        {
            instantiatePosition = mouseFollow.pos;
            InstantiateEditorPlacedHitObject(instantiatePosition);
            // Store the time spawned and position of the object
            editorHitObjectPositions.Add(mouseFollow.pos);
            // Add to total
            totalEditorHitObjects += 1;
        }



    }

    // Instantiate placed hit object at the position on the mouse
    public void InstantiateEditorPlacedHitObject(Vector3 instantiatePositionPass)
    {
        Instantiate(editorPlacedHitObject, instantiatePositionPass, Quaternion.Euler(0, 45, 0));
    }

    public void HasClickedUIButton()
    {
        // The button has been clicked enable positioning of objects
        hasClickedUIButton = true;
    }
}
