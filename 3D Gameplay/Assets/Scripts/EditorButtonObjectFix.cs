using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorButtonObjectFix : MonoBehaviour {

    public GameObject editorHitObject; // The editor hit object
    public PlaceObject placeObject; // The placeObject script for getting the spawned editor hit object reference
    public PlacedObject placedObject; // the placedObject script for checking if the UI button has been clicked
    bool hasClickedUIButton = false; // Has the UI button been clicked
    Material editorHitObjectMaterial; // The material of the editor hit object used for disabling/enabling when hovering over UI elements
    Color editorHitOBjectColor;  // The color of the editor hit object used for disabling/enabling when hovering over UI elements

    void Start()
    {
        // Reference to the Place Object script
        placeObject = FindObjectOfType<PlaceObject>();
        // Reference to the Placed Object script
        placedObject = FindObjectOfType<PlacedObject>();
    }

    void Update()
    {
        // Check if the editorHitObject has been instantiated
        if (placeObject.hasInstantiated == true)
        {
            // If it has assign the reference to the object
            editorHitObject = placeObject.instantiatedEditorHitObjectGhost;

            // Get the reference to the editorHitObject material (from the child)
            editorHitObjectMaterial = editorHitObject.GetComponentInChildren<Renderer>().material;
        }
    }

    // Deactive the object when the mouse is over the button with the object spawned
    public void DeactivateEditorHitObject()
    {
        // Check if the UI button has been clicked
        CheckUIButtonClicked();

        // If the UI button has been clicked before
        if (hasClickedUIButton == true)
        {
            // Disable the editor hit object
            editorHitOBjectColor = editorHitObjectMaterial.color;
            editorHitOBjectColor.a = 0;
            editorHitObjectMaterial.color = editorHitOBjectColor;
        }
    }

    // Activate the object when the mouse is not on the button
    public void ActivateEditorHitObject()
    {
        if (hasClickedUIButton == true)
        {
            editorHitOBjectColor = editorHitObjectMaterial.color;
            editorHitOBjectColor.a = 1;
            editorHitObjectMaterial.color = editorHitOBjectColor;
        }
    }

    // Check if the UI button has been clicked
    public bool CheckUIButtonClicked()
    {
        // If the UI button has been clicked
        if (placedObject.hasClickedUIButton == true)
        {
            // Set it to have been clicked
            hasClickedUIButton = true;
        }
        else
        {
            // It hasn't been clicked so set to false
            hasClickedUIButton = false;
        }

        return hasClickedUIButton;
    }
}
