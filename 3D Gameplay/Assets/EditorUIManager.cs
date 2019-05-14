using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUIManager : MonoBehaviour {

    public GameObject editorInstructionsPanel;
    private bool editorInstructionsPanelIsActive;

	// Use this for initialization
	void Start () {
        // Set to false by default
        editorInstructionsPanelIsActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Activate/Deactivate editerInstructionsPanel
    public void ActivateOrDeactivateEditorInstructionsPanel()
    {
        // If the editor instructions panel is disabled
        if (editorInstructionsPanelIsActive == false)
        {
            // Enable it
            editorInstructionsPanel.gameObject.SetActive(true);
            // Set is active to true
            editorInstructionsPanelIsActive = true;
        }
        // If the editor instructions panel is active
        else if (editorInstructionsPanelIsActive == true)
        {
            // Disable it
            editorInstructionsPanel.gameObject.SetActive(false);
            // Set is active to false
            editorInstructionsPanelIsActive = false;
        }
    }


}
