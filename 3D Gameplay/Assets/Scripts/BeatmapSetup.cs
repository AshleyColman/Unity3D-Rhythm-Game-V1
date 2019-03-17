using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BeatmapSetup : MonoBehaviour {

    // Create folder variables
    public InputField folderNameInputField;
    public Button createFolderButton;
    public string beatmapFolderName;
    public string folderDirectory;

    /*
    // Create difficulty file variables
    public InputField difficultyNameInputField;
    public Button createDifficultyFileButton;
    public string beatmapDifficultyName;
    public string fileDirectory;
    */

    // Beatmap difficulty level
    public InputField beatmapDifficultyLevelInputField;
    public Button beatmapDifficultyLevelSaveButton;
    public string beatmapAdvancedDifficultyLevel;
    public string beatmapExtraDifficultyLevel;

    // Beatmap difficulty
    public InputField beatmapDifficultyInputField;
    public Button beatmapDifficultyAdvancedButton;
    public Button beatmapDifficultyExtraButton;
    public string beatmapDifficulty;

    // Song name
    public string songName;
    public InputField songNameInputField;
    public Button songNameSaveButton;

    // Song artist
    public string songArtist;
    public InputField songArtistInputField;
    public Button songArtistSaveButton;

    // Beatmap creator
    public string beatmapCreator;
    public InputField beatmapCreatorInputField;
    public Button beatmapCreatorSaveButton;

    // Beatmap setup panel shown during the setup
    public Image beatmapSetupPanel;

    public static Beatmap beatmap;
    string FILE_EXTENSION = ".dia";

    // The setup block images in the setup editor panel
    public Image SetupProgressBlock1;
    public Image SetupProgressBlock2;
    public Image SetupProgressBlock3;
    public Image SetupProgressBlock4;
    public Image SetupProgressBlock5;
    public Image SetupProgressBlock6;
    public Color SetupProgressBlockCompletedColor;

    // The editor UI components
    public Text editorTitle;

    // Is true when in the setup screen, used for allowing keyboard press without starting the editor
    public bool settingUp;

    private void Awake()
    {
        beatmap = new Beatmap();
    }

    private void Start()
    {
        settingUp = true;
    }
    

    // Create the folder based on the name inserted in the folderNameInputField
    public void CreateBeatmapFolder()
    {
        // Get the folder name that the user has inputted
        beatmapFolderName = folderNameInputField.text;
        // Create a new folder for the beatmap
        var folder = Directory.CreateDirectory(@"C:\Beatmaps\" + beatmapFolderName); 
        // Save the folder directory to save the files into later
        folderDirectory = @"C:\Beatmaps\" + beatmapFolderName + @"\";

        // Disable the button and textfield and enable the next buttons
        createFolderButton.gameObject.SetActive(false);
        folderNameInputField.gameObject.SetActive(false);
        // Enable the next input field and button
        songNameSaveButton.gameObject.SetActive(true);
        songNameInputField.gameObject.SetActive(true);
        // Change the color of the first progress block 
        SetupProgressBlock1.GetComponent<Image>().color = SetupProgressBlockCompletedColor;
    }

    // Insert the song name
    public void InsertSongName()
    {
        // Save
        songName = songNameInputField.text;
        // Disable the button and textfield and enable the next buttons
        songNameSaveButton.gameObject.SetActive(false);
        songNameInputField.gameObject.SetActive(false);
        // Enable the next input field and button
        songArtistSaveButton.gameObject.SetActive(true);
        songArtistInputField.gameObject.SetActive(true);
        // Change the color of the second progress block 
        SetupProgressBlock2.GetComponent<Image>().color = SetupProgressBlockCompletedColor;
    }

    // Insert the song artist
    public void InsertSongArtist()
    {
        // Save
        songArtist = songArtistInputField.text;
        // Disable the button and textfield and enable the next buttons
        songArtistSaveButton.gameObject.SetActive(false);
        songArtistInputField.gameObject.SetActive(false);
        // Enable the next input field and button
        beatmapCreatorSaveButton.gameObject.SetActive(true);
        beatmapCreatorInputField.gameObject.SetActive(true);
        // Change the color of the third progress block 
        SetupProgressBlock3.GetComponent<Image>().color = SetupProgressBlockCompletedColor;
    }

    // Insert the chart creator
    public void InsertBeatmapCreator()
    {
        // Save
        beatmapCreator = beatmapCreatorInputField.text;
        // Disable the button and textfield and enable the next buttons
        beatmapCreatorSaveButton.gameObject.SetActive(false);
        beatmapCreatorInputField.gameObject.SetActive(false);
        // Disable the button and textfield and enable the next buttons
        beatmapDifficultyAdvancedButton.gameObject.SetActive(true);
        beatmapDifficultyExtraButton.gameObject.SetActive(true);
        beatmapDifficultyInputField.gameObject.SetActive(true);

        // Change the color of the fourth progress block 
        SetupProgressBlock4.GetComponent<Image>().color = SetupProgressBlockCompletedColor;
    }

    // Insert beatmap difficulty
    public void InsertBeatmapDifficulty(string beatmapDifficultyPass)
    {
        // The beatmap difficulty is the difficulty button chosen during the setup menu
        beatmapDifficulty = beatmapDifficultyPass;
        // Disable all
        beatmapDifficultyAdvancedButton.gameObject.SetActive(false);
        beatmapDifficultyExtraButton.gameObject.SetActive(false);
        beatmapDifficultyInputField.gameObject.SetActive(false);

        // Enable next buttons and input field
        beatmapDifficultyLevelInputField.gameObject.SetActive(true);
        beatmapDifficultyLevelSaveButton.gameObject.SetActive(true);

        // Change the color of the fifth progress block 
        SetupProgressBlock5.GetComponent<Image>().color = SetupProgressBlockCompletedColor;
    }

    // Insert the beatmap difficulty level
    public void InsertBeatmapDifficultyLevel()
    {
        // If the beatmap difficulty selected previously is advanced
        if (beatmapDifficulty == "advanced")
        {
            // Insert the advanced difficulty level
            beatmapAdvancedDifficultyLevel = beatmapDifficultyLevelInputField.text;
        }
        // If the beatmap difficulty selected previously is extra
        else if (beatmapDifficulty == "extra")
        {
            // Insert the extra difficulty level
            beatmapExtraDifficultyLevel = beatmapDifficultyLevelInputField.text;
        }

        // Change the color of the sixth progress block 
        SetupProgressBlock6.GetComponent<Image>().color = SetupProgressBlockCompletedColor;

        // Disable the button and textfield 
        beatmapDifficultyLevelInputField.gameObject.SetActive(false);
        beatmapDifficultyLevelSaveButton.gameObject.SetActive(false);
        SetupProgressBlock6.gameObject.SetActive(false);
        beatmapSetupPanel.gameObject.SetActive(false);
        SetupProgressBlock1.gameObject.SetActive(false);
        SetupProgressBlock2.gameObject.SetActive(false);
        SetupProgressBlock3.gameObject.SetActive(false);
        SetupProgressBlock4.gameObject.SetActive(false);
        SetupProgressBlock5.gameObject.SetActive(false);
        // We have finished setting up so set setting up to false allowing editor functionality
        settingUp = false;
        // Update the editor UI with all the new information inserted
        UpdateEditorUI();
    }


    // Update the song name, artist and difficulty with the values entered
    public void UpdateEditorUI()
    {
        editorTitle.text = songName + " [ " + songArtist + " ] " + " [ " + beatmapDifficulty.ToUpper() + " ] ";
    }

    /*
    // Create the difficulty file
    public void CreateDifficultyFile()
    {
        // Get the difficulty name that the user has inputted
        beatmapDifficultyName = difficultyNameInputField.text;

        Debug.Log("saving to: " + folderDirectory + FILE_EXTENSION);
        Stream stream = File.Open(folderDirectory + beatmapDifficultyName + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap.PositionX.Add(1.5f);
        bf.Serialize(stream, beatmap);
        stream.Close();

    }
    */
}
