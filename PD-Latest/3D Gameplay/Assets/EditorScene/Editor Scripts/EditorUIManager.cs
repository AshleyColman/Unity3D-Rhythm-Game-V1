using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EditorUIManager : MonoBehaviour {

    public GameObject editorInstructionsPanel;
    public GameObject setupBeatmapPanel, beatmapToolsPanel, beatmapButtonsPanel, previewPanel;

    public Button beatmapToolsButton, beatmapButtonsButton, previewButton, playTestButton;

    public TextMeshProUGUI descriptionText;

    private MetronomePro metronomePro;

    private void Start()
    {
        metronomePro = FindObjectOfType<MetronomePro>();

        if (metronomePro.songAudioSource.clip == null)
        {
            beatmapToolsButton.interactable = false;
            beatmapButtonsButton.interactable = false;
            previewButton.interactable = false;
            playTestButton.interactable = false;
        }
    }

    public void EnableToolButtons()
    {
        beatmapToolsButton.interactable = true;
        beatmapButtonsButton.interactable = true;
        previewButton.interactable = true;
        playTestButton.interactable = true;
    }

    // Update description text
    public void UpdateDescriptionText(string _descriptionTextValue)
    {
        descriptionText.text = _descriptionTextValue;
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


    // Activate preview panel
    public void ActivatePreviewPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        previewPanel.gameObject.SetActive(true);
    }

    // Deactivate all active panels
    public void DeactivateAllPanels()
    {
        // Deactivate the other panels
        beatmapToolsPanel.gameObject.SetActive(false);
        setupBeatmapPanel.gameObject.SetActive(false);
        beatmapButtonsPanel.gameObject.SetActive(false);
        previewPanel.gameObject.SetActive(false);
    }
}
