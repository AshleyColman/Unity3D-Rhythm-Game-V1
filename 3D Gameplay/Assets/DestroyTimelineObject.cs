using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimelineObject : MonoBehaviour {

    private PlacedObject placedObject;

    private void Start()
    {
        placedObject = FindObjectOfType<PlacedObject>();
    }

    public void DestroyEditorTimelineObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Destroy the game object
            Destroy(this.gameObject);
        }
    }
    
    public void FindIndexOfTimelineObject()
    {
        // Pass this game object
        placedObject.GetIndexOfRaycastTimelineObject(this.gameObject);
    }

}
