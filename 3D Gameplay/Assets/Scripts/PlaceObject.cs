using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceObject : MonoBehaviour {

    public GameObject editorHitObject; // The editorHitObject
    public List<GameObject> spawnedEditorHitObject = new List<GameObject>(); // The spawned editorHitObject 
    public bool hasInstantiated; // has the object been instantiated before? If it has don't spawn another when clicked

    void Start()
    {
        hasInstantiated = false;
    }

    // Instantiate the editor hit object
    public void InstantiateEditorHitObject()
    {
        spawnedEditorHitObject.Add(Instantiate(editorHitObject, transform.position, Quaternion.Euler(0, 45, 0)));
    }

    // Check if an editorHitObject has been placed already
    public void CheckIfInstantiated()
    {
        // If it hasn't been instantiated spawn one
        if (hasInstantiated == false)
        {
            InstantiateEditorHitObject();
            // Set it to be instantiated to prevent more to be spawned when clicked
            hasInstantiated = true;
        }
        else
        {
            // Do not spawn another
        }
    }
}
