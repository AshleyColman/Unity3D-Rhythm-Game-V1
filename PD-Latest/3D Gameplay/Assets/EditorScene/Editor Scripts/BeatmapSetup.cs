using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class BeatmapSetup : MonoBehaviour {

    // Beatmap file
    public static Beatmap beatmap;

    // UI
    public Button finishedButton;
    public Button saveButton;
    public Button resetButton;
    public Button newSongButton;
    public Button resetConfirmationButtonNo;
    public TMP_InputField songPreviewStartTimeInputField;    // The textbox to get the vlaue of the start time
    public TextMeshProUGUI statusText; // Displays status and user activity
    public TextMeshProUGUI songPreviewStartTimeText;

    // Gameobjects
    public GameObject difficultyButtonsPanel;
    public GameObject songSelectPanel;
    public GameObject difficultyLevelPanel;
    public GameObject settingsPanel; // The settings panel that's displayed when the user has finished creating the beatmap or is editing beatmap information
    public GameObject songPreviewStartTimePanel;    // The song preview start time panel
    public GameObject statusPanel; // Status panel for displaying information on user activity in the editor
    public GameObject saveBeatmapPanel; // Save beatmap panel
    public GameObject resetBeatmapInformationPanel; // Reset beatmap information confirmation panel

    // strings
    private string beatmapEasyDifficultyLevel, beatmapAdvancedDifficultyLevel, beatmapExtraDifficultyLevel;
    private string beatmapFolderName, folderDirectory, beatmapDifficulty;
    private string songName, songArtist, beatmapCreator;
    private const string FILE_EXTENSION = ".dia";
    private string statusBeatmapSaved, statusBeatmapReset, statusDeletedHitObject;
    private string beatmapCreatedDate;
    
    // integers
    private int songClipChosenIndex;
    private float songPreviewStartTime;  // Song preview start time in th song select screen
    private float statusPanelDeactivateTime;
    private float statusPanelTimer;

    // bools
    private bool settingUp; // Is true when in the setup screen, used for allowing keyboard press without starting the editor
    private bool hasUpdatedUIText;

    // Properties

    public string BeatmapCreatedDate
    {
        get { return beatmapCreatedDate; }
        set { beatmapCreatedDate = value; }
    }

    public string BeatmapEasyDifficultyLevel
    {
        get { return beatmapEasyDifficultyLevel; }
        set { beatmapEasyDifficultyLevel = value; }
    }

    public string BeatmapAdvancedDifficultyLevel
    {
        get { return beatmapAdvancedDifficultyLevel; }
        set { beatmapAdvancedDifficultyLevel = value; }
    }

    public string BeatmapExtraDifficultyLevel
    {
        get { return beatmapExtraDifficultyLevel; }
        set { beatmapExtraDifficultyLevel = value; }
    }

    public string SongName
    {
        get { return songName; }
        set { songName = value; }
    }

    public string SongArtist
    {
        get { return songArtist; }
        set { songArtist = value; }
    }

    public string BeatmapCreator
    {
        get { return beatmapCreator; }
        set { beatmapCreator = value; }
    }

    public int SongClipChosenIndex
    {
        get { return songClipChosenIndex; }
        set { songClipChosenIndex = value; }
    }

    public float SongPreviewStartTime
    {
        get { return songPreviewStartTime; }
        set { songPreviewStartTime = value; }
    }

    public string FolderDirectory
    {
        get { return folderDirectory; }
        set { folderDirectory = value; }
    }

    public string BeatmapDifficulty
    {
        get { return beatmapDifficulty; }
        set { beatmapDifficulty = value; }
    }



    private void Awake()
    {
        beatmap = new Beatmap();
    }

    private void Start()
    {
        // Initialize
        statusBeatmapSaved = "Beatmap saved at ";
        statusBeatmapReset = "Beatmap reset";
        statusDeletedHitObject = "Deleted hit object";
      
        // Setting up at the start 
        settingUp = true;

        statusPanelDeactivateTime = 5f;

        // Get the user logged in and set the creator to that user
        if (MySQLDBManager.loggedIn)
        {
            beatmapCreator = MySQLDBManager.username;
        }
        else
        {
            beatmapCreator = "GUEST";
        }

    }

    private void Update()
    {
        // Display reset confirmation panel 
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Display
            resetBeatmapInformationPanel.gameObject.SetActive(true);
            // Select the reset panel NO button
            resetConfirmationButtonNo.Select();
        }

        if (statusPanel.gameObject.activeSelf == true)
        {
            statusPanelTimer += Time.deltaTime;

            if (statusPanelTimer >= statusPanelDeactivateTime)
            {
                statusPanel.gameObject.SetActive(false);
            }
        }
    }

    // Deactivate the reset confirmation panel
    public void DeactivateResetConfirmationPanel()
    {
        resetBeatmapInformationPanel.gameObject.SetActive(false);
    }

    // Save the song preview start time
    public void GetSongPreviewStartTime(float _time)
    {
        songPreviewStartTime = _time;

        // Update the song preview start time text
        songPreviewStartTimeText.text =  "BEATMAP SONG PREVIEW START TIME: " + UtilityMethods.FromSecondsToMinutesAndSeconds(songPreviewStartTime);
    }

    // Activate the save beatmap panel
    private void ActivateSaveBeatmapPanel()
    {
        saveBeatmapPanel.gameObject.SetActive(true);
    }

    // Turn off the settings panels
    public void DeactivateSettingsPanel()
    {
        // Disable all panels 
        DisableAllFinishedBeatmapSetupPanels();

        settingsPanel.gameObject.SetActive(false);
    }

    // Insert all song information when the song selected has been clicked
    public void InsertSongName(string songNamePass)
    {
        songName = songNamePass;

        // No longer setting up
        settingUp = false;

        // Update the editor UI
        UpdateEditorUI();
    }

    // Insert all song information when the song selected has been clicked
    public void InsertSongArtist(string songArtistPass)
    {
        songArtist = songArtistPass;

        // No longer setting up
        settingUp = false;
        
        // Update the editor UI
        UpdateEditorUI();
    }


    // Get the song chosen to load 
    public void GetSongChosen(int songChosenIndexPass)
    {
        // Get the index of the song chosens
        songClipChosenIndex = songChosenIndexPass;
    }

    // Create the folder based on the name inserted in the folderNameInputField
    public void CreateBeatmapFolder()
    {
        // Disable the save button
        saveButton.interactable = false;

        // Folder name is based off the user name and the song name charted
        beatmapFolderName = beatmapCreator + "_" +songName + "_" + songArtist;

        // Create a new folder for the beatmap
        var folder = Directory.CreateDirectory(@"C:\Beatmaps\" + beatmapFolderName); 
        // Save the folder directory to save the files into later
        folderDirectory = @"C:\Beatmaps\" + beatmapFolderName + @"\";

        // Get the current date
        GetCurrentDate();
    }

    // Get the current date
    public void GetCurrentDate()
    {
        beatmapCreatedDate = DateTime.Now.ToString("MM/dd/yyyy h:mm");
    }

    // Insert beatmap difficulty
    public void InsertBeatmapDifficulty(string beatmapDifficultyPass)
    {
        // The beatmap difficulty is the difficulty button chosen during the setup menu
        beatmapDifficulty = beatmapDifficultyPass;


        // Disable the difficulty buttons
        difficultyButtonsPanel.gameObject.SetActive(false);

        // Activate the difficulty level buttons
        difficultyLevelPanel.gameObject.SetActive(true);
    }

    // Insert the beatmap difficulty level
    public void InsertBeatmapDifficultyLevel(string beatmapDifficultyLevelPass)
    {
        // If the beatmap difficulty selected previously is easy
        if (beatmapDifficulty == "easy")
        {
            // Insert the advanced difficulty level
            beatmapEasyDifficultyLevel = beatmapDifficultyLevelPass;
        }
        // If the beatmap difficulty selected previously is advanced
        else if (beatmapDifficulty == "advanced")
        {
            // Insert the advanced difficulty level
            beatmapAdvancedDifficultyLevel = beatmapDifficultyLevelPass;
        }
        // If the beatmap difficulty selected previously is extra
        else if (beatmapDifficulty == "extra")
        {
            // Insert the extra difficulty level
            beatmapExtraDifficultyLevel = beatmapDifficultyLevelPass;
        }

        // Disable the difficulty level panel
        difficultyLevelPanel.gameObject.SetActive(false);

        // Activate save button panel and disable settings panel
        ActivateSaveBeatmapPanel();
    }

    // Update the song name, artist and difficulty with the values entered
    public void UpdateEditorUI()
    {
        // Disable the song select menu
        songSelectPanel.gameObject.SetActive(false);
    }

    // Change the beatmap song selected
    public void OpenSongSelectMenu()
    {
        if (songSelectPanel.gameObject.activeSelf == false)
        {
            // Set the song panel to active
            songSelectPanel.gameObject.SetActive(true);
        }
        else
        {
            // Set the song panel to active
            songSelectPanel.gameObject.SetActive(false);
        }

    }

    // Disable all panels that were left on 
    private void DisableAllFinishedBeatmapSetupPanels()
    {
        // Activate the difficulty level buttons
        difficultyLevelPanel.gameObject.SetActive(false);

        saveBeatmapPanel.gameObject.SetActive(false);
    }

    // Activate the difficulty buttons
    public void ActivateDifficultyTypeButtons()
    {
        // Disable all panels

        // Activate settings panel
        settingsPanel.gameObject.SetActive(true);

        difficultyButtonsPanel.gameObject.SetActive(true);

        /*
        finishedButton.interactable = false;
        resetButton.interactable = false;
        newSongButton.interactable = false;
        */
    }

    // Show the success save status
    public void DisplayStatus(string _status)
    {
        // Reset timer
        statusPanelTimer = 0f;

        // Activate panel
        statusPanel.gameObject.SetActive(true);

        // Display the correct status
        switch (_status)
        {
            case "SAVED":
                statusText.text = statusBeatmapSaved + "C:Beatmaps " + beatmapFolderName;
                break;
            case "DELETED":
                statusText.text = statusDeletedHitObject;
                break;
            case "RESET":
                statusText.text = statusBeatmapReset;
                break;
        }
    }
}
