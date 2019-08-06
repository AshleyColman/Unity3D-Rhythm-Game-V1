using UnityEngine;
using UnityEngine.UI;

public class EditorUIManager : MonoBehaviour {

    public GameObject editorInstructionsPanel;
    public GameObject setupBeatmapPanel, beatmapToolsPanel, beatmapButtonsPanel;

    private bool editorInstructionsPanelIsActive;

	void Start () {
        // Set to false by default
        editorInstructionsPanelIsActive = false;
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


    // Activate beatmap buttons panel
    public void ActivateBeatmapButtonsPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        beatmapButtonsPanel.gameObject.SetActive(true);
    }

    // Activate beatmap tools panel
    public void ActivateBeatmapToolsPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        beatmapToolsPanel.gameObject.SetActive(true);
    }

    // Activate beatmap setup panel
    public void ActivateBeatmapSetupPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        setupBeatmapPanel.gameObject.SetActive(true);
    }

    // Deactivate all active panels
    public void DeactivateAllPanels()
    {
        // Deactivate the other panels
        beatmapToolsPanel.gameObject.SetActive(false);
        setupBeatmapPanel.gameObject.SetActive(false);
        beatmapButtonsPanel.gameObject.SetActive(false);
    }
}
