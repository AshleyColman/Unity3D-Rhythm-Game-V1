using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorUIManager : MonoBehaviour {

    public GameObject setupBeatmapPanel, beatmapToolsPanel, beatmapButtonsPanel, previewPanel, gameplayTestPanel, changeObjectPropertiesPanel;
    public TextMeshProUGUI setBeatmapPanelText, beatmapToolsPanelText, beatmapButtonPanelText, previewPanelText, gameplayTestPanelText;

    public List<GameObject> panelList;
    
    private int currentPanelIndex;

    public Button beatmapToolsButton, beatmapButtonsButton, previewButton, playTestButton;

    public TextMeshProUGUI descriptionText;

    private MetronomePro metronomePro;

    public GameObject instantiatedEditableHitObject;

    public AudioClip timelineObjectSelectedClip, timelineObjectClickedClip, timelineObjectDeletedClip;

    public Color whiteColor, selectedColor, blackColor;

    public TextMeshProUGUI selectSongText;

    private void Start()
    {
        metronomePro = FindObjectOfType<MetronomePro>();

        // Initialize the panel array
        panelList.Add(gameplayTestPanel);
        panelList.Add(previewPanel);
        panelList.Add(setupBeatmapPanel);
        panelList.Add(beatmapToolsPanel);
        panelList.Add(beatmapButtonsPanel);

        // Set index to the setup panel
        currentPanelIndex = 2;

        if (metronomePro.songAudioSource.clip == null)
        {
            beatmapToolsButton.interactable = false;
            beatmapButtonsButton.interactable = false;
            previewButton.interactable = false;
            playTestButton.interactable = false;
        }

        // Activate the setup panel
        ActivateBeatmapSetupPanel();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (metronomePro.songAudioSource.clip != null)
            {
                // If the editable hit object properties panel is not active
                if (changeObjectPropertiesPanel.gameObject.activeSelf == false)
                {
                    // Change to the next panel
                    ChangeToNextPanel();
                }
            }
        }
    }

    // Change the color of the select a song text when a song has been chosen
    public void ChangeSelectSongTextColor()
    {
        selectSongText.color = whiteColor;
    }
   
    // Change to the next panel
    public void ChangeToNextPanel()
    {
        // Display the next panel 
        if (currentPanelIndex == panelList.Count - 1)
        {
            currentPanelIndex = 0;
        }
        else
        {
            currentPanelIndex++;
        }

        // Display the current index panel
        DisplayCurrentIndexPanel();
    }

    // Display the current index panel
    public void DisplayCurrentIndexPanel()
    {
        DeactivateAllPanels();

        switch (currentPanelIndex)
        {
            case 0:
                ActivatePlayTestPanel();
                break;
            case 1:
                ActivatePreviewPanel();
                break;
            case 2:
                ActivateBeatmapSetupPanel();
                break;
            case 3:
                ActivateBeatmapToolsPanel();
                break;
            case 4:
                ActivateBeatmapButtonsPanel();
                break;
        }
    }

    // Turn off the editable hit object
    public void DeactivateEditableHitObject()
    {
        if (instantiatedEditableHitObject != null && instantiatedEditableHitObject.gameObject.activeSelf == true)
        {
            instantiatedEditableHitObject.gameObject.SetActive(false);
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

        // Unmute metronome
        metronomePro.UnmuteMetronome();

        // Change color of all text
        beatmapButtonPanelText.color = selectedColor;
    }

    // Activate beatmap tools panel
    public void ActivateBeatmapToolsPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        beatmapToolsPanel.gameObject.SetActive(true);

        // Unmute metronome
        metronomePro.UnmuteMetronome();

        // Change color of all text
        beatmapToolsPanelText.color = selectedColor;
    }

    // Activate beatmap setup panel
    public void ActivateBeatmapSetupPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        setupBeatmapPanel.gameObject.SetActive(true);

        // Unmute metronome
        metronomePro.UnmuteMetronome();

        // Change color of all text
        setBeatmapPanelText.color = selectedColor;
    }

    // Activate the editable hit object properties panel
    public void ActivateObjectPropertiesPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        changeObjectPropertiesPanel.gameObject.SetActive(true);
    }

    // Activate preview panel
    public void ActivatePreviewPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        previewPanel.gameObject.SetActive(true);

        // Mute metronome
        metronomePro.MuteMetronome();

        // Change color of all text
        previewPanelText.color = selectedColor;
    }

    public void ActivatePlayTestPanel()
    {
        // Deactivate all active panels
        DeactivateAllPanels();

        // Activate
        gameplayTestPanel.gameObject.SetActive(true);

        // Mute metronome
        metronomePro.MuteMetronome();

        // Change color of all text
        gameplayTestPanelText.color = selectedColor;
    }

    // Deactivate all active panels
    public void DeactivateAllPanels()
    {
        // Deactivate the other panels
        beatmapToolsPanel.gameObject.SetActive(false);
        setupBeatmapPanel.gameObject.SetActive(false);
        beatmapButtonsPanel.gameObject.SetActive(false);
        changeObjectPropertiesPanel.gameObject.SetActive(false);
        previewPanel.gameObject.SetActive(false);
        gameplayTestPanel.gameObject.SetActive(false);

        // Change color of all text
        setBeatmapPanelText.color = blackColor;
        beatmapToolsPanelText.color = blackColor;
        beatmapButtonPanelText.color = blackColor;
        previewPanelText.color = blackColor;
        gameplayTestPanelText.color = blackColor;
    }

 
}
