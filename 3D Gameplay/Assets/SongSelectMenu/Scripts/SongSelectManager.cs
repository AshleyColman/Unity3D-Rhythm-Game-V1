using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SongSelectManager : MonoBehaviour {

    // Get the directories and folder names of all songs in the beatmap folder
    public string[] beatmapDirectories;
    public int selectedDirectoryIndex;
    public int previousDirectoryIndex;
    private string defaultBeatmapDifficulty = "advanced";
    private string extraBeatmapDifficuly = "extra";

    // Song select menu UI elements
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI beatmapCreatorText;
    public TextMeshProUGUI beatmapStatisticsText;

    // Loaded song variables
    private string songName;
    private string songArtist;
    private string beatmapCreator;
    private string advancedDifficultyLevel;
    private string extraDifficultyLevel;
    private int totalDiamonds;
    private float songPreviewStartTime;

    // References to the difficulty buttons
    public Button DifficultyOptionAdvancedButton;
    public Button DifficultyOptionExtraButton;
    public TextMeshProUGUI DifficultyOptionAdvancedLevelText;
    public TextMeshProUGUI DifficultyOptionExtraLevelText;
    public string disabledText = "X";
    // Bools for checking if the files exist for loading advanced/extra difficulties
    private bool advancedDifficultyExist;
    private bool extraDifficultyExist;

    // Key pressed variables and image references, the keys used in the beatmap
    private bool pressedKeyS;
    private bool pressedKeyD;
    private bool pressedKeyF;
    private bool pressedKeyJ;
    private bool pressedKeyK;
    private bool pressedKeyL;

    public Image pressedKeySImage;
    public Image pressedKeyDImage;
    public Image pressedKeyFImage;
    public Image pressedKeyJImage;
    public Image pressedKeyKImage;
    public Image pressedKeyLImage;

    public Image disabledPressedKeySImage;
    public Image disabledPressedKeyDImage;
    public Image disabledPressedKeyFImage;
    public Image disabledPressedKeyJImage;
    public Image disabledPressedKeyKImage;
    public Image disabledPressedKeyLImage;

    // Get reference to song select preview to control playing the song previews
    private SongSelectPreview songSelectPreview;
    private int songClipChosenIndex;


    // Reference to the background image manager for loading the beatmap image
    private BackgroundManager backgroundManager;


    // Reference to the loadLastSong manager for loading the last song in the song select screen
    private LoadLastSelectedSong loadLastSelectedSong;


    // Song List Images and directories for loading the images
    private string previousSongButtonDirectoryPath;
    private string nextSongButtonDirectoryPath;
    private string nextNextSongButtonDirectoryPath;

    // Reference to the SongSelectPanel for loading the song beatmap buttons with the directories found
    private SongSelectPanel songSelectPanel;

    // Used to play the start preview once upon entering the song select screen for the first time so the song plays at the current set time once.
    public bool hasPlayedSongPreviewOnce;

    // Use this for initialization
    void Start () {

        // Set to false at the start
        hasPlayedSongPreviewOnce = false;

        songSelectPanel = FindObjectOfType<SongSelectPanel>();
        songClipChosenIndex = 0;
        songSelectPreview = FindObjectOfType<SongSelectPreview>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        loadLastSelectedSong = FindObjectOfType<LoadLastSelectedSong>();
        beatmapDirectories = Directory.GetDirectories(@"c:\Beatmaps");
        CheckIfAdvancedDifficultyExists();
        CheckIfExtraDifficultyExists();

        // Load the beatmap directories found for the songSelectPanel
        // Get the amount of beatmap folders in the beatmap directory
        songSelectPanel.GetBeatmapFolderCount();
        // Get the directory paths for all the beatmap folders in the beatmap directory
        songSelectPanel.GetBeatmapDirectoryPaths();
    

       


        // Load the lastSelectedSongIndex
        selectedDirectoryIndex = loadLastSelectedSong.LoadSelectedDirectoryIndex();
        previousDirectoryIndex = selectedDirectoryIndex;

        if (advancedDifficultyExist)
        {
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, defaultBeatmapDifficulty, true);
        }
        else if (extraDifficultyExist)
        {
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, defaultBeatmapDifficulty, true);
        }
        else
        {
            // Do not load any new song
        }

        // Set to false at the start
        hasPlayedSongPreviewOnce = true;

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    // Load beatmap song select information
    public void LoadBeatmapSongSelectInformation(int selectedDirectoryIndexPass, string beatmapDifficulty, bool hasPressedArrowKey)
    {
        // Reset difficulty check bools
        advancedDifficultyExist = false;
        extraDifficultyExist = false;

        // Do a check on the selectedDirectoryIndexPass, if it's out of the directory range reset back to 0 to loop through the song list
        if (selectedDirectoryIndexPass == beatmapDirectories.Length)
        {
            selectedDirectoryIndexPass = 0; // Reset the variable passed
            selectedDirectoryIndex = 0; // Also reset the variable that is being sent from SongSelectMenuFlash when arrow key is pressed
        }
        // Do a check on the selectedDirectoryIndexPass, if it's out of the directory range go to max range to loop through the song list
        if (selectedDirectoryIndexPass < 0)
        {
            selectedDirectoryIndexPass = beatmapDirectories.Length - 1; // Set the index to the last song in the list
            selectedDirectoryIndex = beatmapDirectories.Length - 1; // Also set the variable that is being sent from SongSelectMenuFlash when arrow key is pressed
        }

        // Check if the advanced difficulty exists
        CheckIfAdvancedDifficultyExists();
        // Check if the extra difficulty exists
        CheckIfExtraDifficultyExists();

        // If the advanced file only exists and the extra difficulty does not exist
        if (advancedDifficultyExist == true && extraDifficultyExist == false)
        {
            // Set the beatmap to be loaded first to be the advanced difficulty
            beatmapDifficulty = defaultBeatmapDifficulty;
        }
        // If the advanced file does not exist but the extra difficulty does exist
        else if (advancedDifficultyExist == false && extraDifficultyExist == true)
        {
            // Set the beatmap to be loaded first to be the extra difficulty
            beatmapDifficulty = extraBeatmapDifficuly;
        }

        if (advancedDifficultyExist == true || extraDifficultyExist == true)
        {
            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(beatmapDirectories[selectedDirectoryIndexPass], beatmapDifficulty);

            // Load the song select UI variables from the database
            songName = Database.database.loadedSongName;
            songArtist = Database.database.loadedSongArtist;
            beatmapCreator = Database.database.loadedBeatmapCreator;
            advancedDifficultyLevel = Database.database.loadedbeatmapAdvancedDifficultyLevel;
            extraDifficultyLevel = Database.database.loadedbeatmapExtraDifficultyLevel;
            songClipChosenIndex = Database.database.loadedSongClipChosenIndex;
            totalDiamonds = Database.database.LoadedPositionX.Count;
            pressedKeyS = Database.database.loadedPressedKeyS;
            pressedKeyD = Database.database.loadedPressedKeyD;
            pressedKeyF = Database.database.loadedPressedKeyF;
            pressedKeyJ = Database.database.loadedPressedKeyJ;
            pressedKeyK = Database.database.loadedPressedKeyK;
            pressedKeyL = Database.database.loadedPressedKeyL;
            songPreviewStartTime = Database.database.loadedSongPreviewStartTime;

        
            // Check if the first song preview when entering the song select menu has played once, if it hasn't play the song preview at the correct set time
            if (hasPlayedSongPreviewOnce == false)
            {
                // Play the song preview for the first song when entering at the correct set time in the beatmap information
                PlaySongPreview();
                // Set to true
                hasPlayedSongPreviewOnce = true;
            }

            // Load the image by passing the current beatmap directory
            backgroundManager.LoadEditorBeatmapImage(beatmapDirectories[selectedDirectoryIndexPass]);


            // Change the current song selected text to the information loaded from the current directory
            songTitleText.text = songName + " [ " + songArtist + " ] ";
            beatmapCreatorText.text = "Beatmap Created By: " + beatmapCreator;
            beatmapStatisticsText.text = "Total Diamonds: " + totalDiamonds.ToString();

            // Enable the required keys for the beatmap images
            EnableKeysRequiredForBeatmap();

            // Do a check to ensure the level is outputted if exists, and if it doesn't output the missing difficulty text
            if (extraDifficultyExist == true)
            {
                DifficultyOptionExtraLevelText.text = extraDifficultyLevel;
            }
            else if (advancedDifficultyExist == true)
            {
                DifficultyOptionAdvancedLevelText.text = advancedDifficultyLevel;
            }

            // Save the selected song index
            loadLastSelectedSong.SaveSelectedDirectoryIndex(selectedDirectoryIndex);



            // Update the previous index to be the current index
            previousDirectoryIndex = selectedDirectoryIndex;
        }
        else
        {
            // As we tried to access a file with no difficulties remain on the current index
            selectedDirectoryIndex = previousDirectoryIndex;
        }

        if (hasPressedArrowKey == true)
        {
            songSelectPreview.GetSongChosen(songClipChosenIndex);
        }
    }

    // Play the song preview at the saved preview time
    public void PlaySongPreview()
    {
        // Start the song preview as it has now been loaded
        songSelectPreview.PlaySongSelectScenePreview(songPreviewStartTime);
    }

    // Check if advanced difficulty exists
    public void CheckIfAdvancedDifficultyExists()
    {
        if (File.Exists(beatmapDirectories[selectedDirectoryIndex] + @"\" + "advanced.dia"))
        {
            // Allow gameplay
            // Set bool to exist
            advancedDifficultyExist = true;
            // Enable the button
            DifficultyOptionAdvancedButton.GetComponent<Button>().interactable = true;
            // Enable the event trigger 
            DifficultyOptionAdvancedButton.GetComponent<EventTrigger>().enabled = true;
        }
        else
        {
            // Disable the button
            DifficultyOptionAdvancedButton.GetComponent<Button>().interactable = false;
            // Disable the event trigger to prevent it trying to load the advanced file that doesn't exist
            DifficultyOptionAdvancedButton.GetComponent<EventTrigger>().enabled = false;
            // Print disabled text on the level
            DifficultyOptionAdvancedLevelText.text = disabledText;
            // Set bool to not exist
            advancedDifficultyExist = false;
        }
    }

    // Check if extra difficulty exists
    public void CheckIfExtraDifficultyExists()
    {
        // Check if the file exists for the current song selected
        if (File.Exists(beatmapDirectories[selectedDirectoryIndex] + @"\" + "extra.dia"))
        {
            // Allow gameplay
            // Set bool to exist
            extraDifficultyExist = true;
            // Enable the button
            DifficultyOptionExtraButton.GetComponent<Button>().interactable = true;
            // Enable the event trigger 
            DifficultyOptionExtraButton.GetComponent<EventTrigger>().enabled = true;
        }
        else
        {
            // Disable the button
            DifficultyOptionExtraButton.GetComponent<Button>().interactable = false;
            // Disable the event trigger to prevent it trying to load the extra file that doesn't exist
            DifficultyOptionExtraButton.GetComponent<EventTrigger>().enabled = false;
            // Print disabled text on the level
            DifficultyOptionExtraLevelText.text = disabledText;

            // Set bool to exist
            extraDifficultyExist = false;
        }
    }

    // Enable the keys required for the beatmap
    public void EnableKeysRequiredForBeatmap()
    {
        if (pressedKeyS == true)
        {
            pressedKeySImage.gameObject.SetActive(true);
            disabledPressedKeySImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeySImage.gameObject.SetActive(true);
        }

        if (pressedKeyD == true)
        {
            pressedKeyDImage.gameObject.SetActive(true);
            disabledPressedKeyDImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyDImage.gameObject.SetActive(true);
        }

        if (pressedKeyF == true)
        {
            pressedKeyFImage.gameObject.SetActive(true);
            disabledPressedKeyFImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyFImage.gameObject.SetActive(true);
        }

        if (pressedKeyJ == true)
        {
            pressedKeyJImage.gameObject.SetActive(true);
            disabledPressedKeyJImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyJImage.gameObject.SetActive(true);
        }

        if (pressedKeyK == true)
        {
            pressedKeyKImage.gameObject.SetActive(true);
            disabledPressedKeyKImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyKImage.gameObject.SetActive(true);
        }

        if (pressedKeyL == true)
        {
            pressedKeyLImage.gameObject.SetActive(true);
            disabledPressedKeyLImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyLImage.gameObject.SetActive(true);
        }
    }

    // Disable all keys required for beatmap
    public void DisableKeysRequiredForBeatmap()
    {
        pressedKeySImage.gameObject.SetActive(false);
        pressedKeyDImage.gameObject.SetActive(false);
        pressedKeyFImage.gameObject.SetActive(false);
        pressedKeyJImage.gameObject.SetActive(false);
        pressedKeyKImage.gameObject.SetActive(false);
        pressedKeyLImage.gameObject.SetActive(false);

        disabledPressedKeySImage.gameObject.SetActive(false);
        disabledPressedKeyDImage.gameObject.SetActive(false);
        disabledPressedKeyFImage.gameObject.SetActive(false);
        disabledPressedKeyJImage.gameObject.SetActive(false);
        disabledPressedKeyKImage.gameObject.SetActive(false);
        disabledPressedKeyLImage.gameObject.SetActive(false);
    }
}
