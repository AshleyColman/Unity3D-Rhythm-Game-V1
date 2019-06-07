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

    // Song select menu UI elements
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI beatmapCreatorText;
    public TextMeshProUGUI beatmapStatisticsText;

    // Loaded song variables
    private string songName;
    private string songArtist;
    private string beatmapCreator;
    private string easyDifficultyLevel;
    private string advancedDifficultyLevel;
    private string extraDifficultyLevel;
    private int totalDiamonds;
    private float songPreviewStartTime;

    // References to the difficulty buttons
    public Button DifficultyOptionEasyButton;
    public Button DifficultyOptionAdvancedButton;
    public Button DifficultyOptionExtraButton;
    public TextMeshProUGUI DifficultyOptionEasyLevelText;
    public TextMeshProUGUI DifficultyOptionAdvancedLevelText;
    public TextMeshProUGUI DifficultyOptionExtraLevelText;
    public string disabledText = "X";
    // Bools for checking if the files exist for loading advanced/extra difficulties
    private bool easyDifficultyExist;
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

    // Difficulty Text in song select screen
    public TextMeshProUGUI difficultyText;

    // Leaderboard side image bar
    public Image leaderboardSideBar;

    // Colors for advanced and extra buttons
    public Color easyDifficultyButtonColor;
    public Color advancedDifficultyButtonColor;
    public Color extraDifficultyButtonColor;

    // The flash animator
    public Animator songSelectFlashAnimator;

    // The song player bar image
    public Image songPlayerBarImage;

    // Use this for initialization
    void Start () {

        difficultyText.text = "";

        // Set to false at the start
        hasPlayedSongPreviewOnce = false;

        songSelectPanel = FindObjectOfType<SongSelectPanel>();
        songClipChosenIndex = 0;
        songSelectPreview = FindObjectOfType<SongSelectPreview>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        loadLastSelectedSong = FindObjectOfType<LoadLastSelectedSong>();
        beatmapDirectories = Directory.GetDirectories(@"c:\Beatmaps");

        // Load the beatmap directories found for the songSelectPanel
        // Get the amount of beatmap folders in the beatmap directory
        songSelectPanel.GetBeatmapFolderCount();
        // Get the directory paths for all the beatmap folders in the beatmap directory
        songSelectPanel.GetBeatmapDirectoryPaths();
    

        // Load the lastSelectedSongIndex
        selectedDirectoryIndex = loadLastSelectedSong.LoadSelectedDirectoryIndex();
        previousDirectoryIndex = selectedDirectoryIndex;

        // Check if the last selected difficulty was set or not (first time entering game or not)
        string lastSelectedDifficulty = loadLastSelectedSong.lastSelectedDifficulty;


        if (string.IsNullOrEmpty(lastSelectedDifficulty))
        {
            // If last selected difficulty cannot be found load a difficulty that does exist
            LoadBeatmapFileThatExists(selectedDirectoryIndex, false);
        }
        else
        {
            // Load the last selected difficulty
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, lastSelectedDifficulty, false);


            // Check the difficulty files if they exist and update the levels for each of them
            UpdateDifficultyLevelText(selectedDirectoryIndex);
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

        // Check if the easy difficulty exists
        CheckIfEasyDifficultyExists();
        // Check if the advanced difficulty exists
        CheckIfAdvancedDifficultyExists();
        // Check if the extra difficulty exists
        CheckIfExtraDifficultyExists();

        // Update the song select scene difficulty text for the difficulty selected
        difficultyText.text = beatmapDifficulty.ToUpper();

        // Update the personal best leaderboard button color to the difficulty currently selected
        switch (beatmapDifficulty)
        {
            case "easy":
                FlashImage("SongSelectMenuFlashEasy");
                songPlayerBarImage.color = easyDifficultyButtonColor;
                leaderboardSideBar.color = easyDifficultyButtonColor;
                break;
            case "advanced":
                FlashImage("SongSelectMenuFlashAdvanced");
                songPlayerBarImage.color = advancedDifficultyButtonColor;
                leaderboardSideBar.color = advancedDifficultyButtonColor;
                break;
            case "extra":
                FlashImage("SongSelectMenuFlashExtra");
                songPlayerBarImage.color = extraDifficultyButtonColor;
                leaderboardSideBar.color = extraDifficultyButtonColor;
                break;
            default:
                FlashImage("SongSelectMenuFlash");
                songPlayerBarImage.color = advancedDifficultyButtonColor;
                leaderboardSideBar.color = advancedDifficultyButtonColor;
                break;
        }
        
        if (advancedDifficultyExist == true || extraDifficultyExist == true || easyDifficultyExist == true)
        {
            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(beatmapDirectories[selectedDirectoryIndexPass], beatmapDifficulty);

            // Load the song select UI variables from the database
            songName = Database.database.loadedSongName;
            songArtist = Database.database.loadedSongArtist;
            beatmapCreator = Database.database.loadedBeatmapCreator;
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
            if (hasPlayedSongPreviewOnce == false || hasPressedArrowKey == true)
            {
                // Play the song preview for the first song when entering at the correct set time in the beatmap information
                PlaySongPreview();
                // Set to true
                hasPlayedSongPreviewOnce = true;
                // Set back to false
                hasPressedArrowKey = false;
            }

            // Load the image by passing the current beatmap directory
            backgroundManager.LoadEditorBeatmapImage(beatmapDirectories[selectedDirectoryIndexPass]);


            // Change the current song selected text to the information loaded from the current directory
            songTitleText.text = songName + " [ " + songArtist + " ] ";
            beatmapCreatorText.text = "Beatmap Created By: " + beatmapCreator;
            beatmapStatisticsText.text = "Total Diamonds: " + totalDiamonds.ToString();

            // Enable the required keys for the beatmap images
            EnableKeysRequiredForBeatmap();

            // Save the selected song index
            loadLastSelectedSong.SaveSelectedDirectoryIndex(selectedDirectoryIndex);

            // Update the previous index to be the current index
            previousDirectoryIndex = selectedDirectoryIndex;

            // Save the last selected difficulty
            loadLastSelectedSong.SaveLastSelectedDifficulty(beatmapDifficulty);
        }
        else
        {
            // As we tried to access a file with no difficulties remain on the current index
            selectedDirectoryIndex = previousDirectoryIndex;
        }

        /*
        if (hasPressedArrowKey == true)
        {
            songSelectPreview.GetSongChosen(songClipChosenIndex);
        }
        */
    }

    public void UpdateDifficultyLevelText(int selectedDirectoryIndexPass)
    {
        if (easyDifficultyExist == true)
        {
            // Load the database and beatmap information for the beatmap directory selected
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[selectedDirectoryIndexPass], "easy");
            easyDifficultyLevel = Database.database.loadedbeatmapEasyDifficultyLevel;
            DifficultyOptionEasyLevelText.text = easyDifficultyLevel;
        }

        if (advancedDifficultyExist == true)
        {
            // Load the database and beatmap information for the beatmap directory selected
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[selectedDirectoryIndexPass], "advanced");
            advancedDifficultyLevel = Database.database.loadedbeatmapAdvancedDifficultyLevel;
            DifficultyOptionAdvancedLevelText.text = advancedDifficultyLevel;
        }

        if (extraDifficultyExist == true)
        {
            // Load the database and beatmap information for the beatmap directory selected
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[selectedDirectoryIndexPass], "extra");
            extraDifficultyLevel = Database.database.loadedbeatmapExtraDifficultyLevel;
            DifficultyOptionExtraLevelText.text = extraDifficultyLevel;
        }

    }

    // Animate the flash on screen for the difficulties: easy/advanced/extra
    public void FlashImage(string animationPass)
    {
        songSelectFlashAnimator.Play(animationPass);  
    }

    // Play the song preview at the saved preview time
    public void PlaySongPreview()
    {
        // Start the song preview as it has now been loaded
        songSelectPreview.PlaySongSelectScenePreview(songPreviewStartTime, songClipChosenIndex);
    }

    // Check if easy difficulty exists
    public void CheckIfEasyDifficultyExists()
    {
        if (File.Exists(beatmapDirectories[selectedDirectoryIndex] + @"\" + "easy.dia"))
        {
            // Allow gameplay
            // Set bool to exist
            easyDifficultyExist = true;
            // Enable the button
            DifficultyOptionEasyButton.GetComponent<Button>().interactable = true;
            // Enable the event trigger 
            DifficultyOptionEasyButton.GetComponent<EventTrigger>().enabled = true;
        }
        else
        {
            // Disable the button
            DifficultyOptionEasyButton.GetComponent<Button>().interactable = false;
            // Disable the event trigger to prevent it trying to load the advanced file that doesn't exist
            DifficultyOptionEasyButton.GetComponent<EventTrigger>().enabled = false;
            // Print disabled text on the level
            DifficultyOptionEasyLevelText.text = disabledText;
            // Set bool to not exist
            easyDifficultyExist = false;
        }
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

    // Check the beatmap files that exist (this is requested by songSelectMenuFlash), check what files exist and load the file that exists
    public void LoadBeatmapFileThatExists(int selectedDirectoryIndexPass, bool hasPressedArrowKeyPass)
    {
        // Do a check on the selectedDirectoryIndexPass, if it's out of the directory range reset back to 0 to loop through the song list
        if (selectedDirectoryIndex == beatmapDirectories.Length)
        {
            selectedDirectoryIndex = 0; // Reset the variable passed
        }
        // Do a check on the selectedDirectoryIndexPass, if it's out of the directory range go to max range to loop through the song list
        if (selectedDirectoryIndex < 0)
        {
            selectedDirectoryIndex = beatmapDirectories.Length - 1; // Set the index to the last song in the list
        }

        // Check if the difficulties exist
        CheckIfEasyDifficultyExists();
        CheckIfAdvancedDifficultyExists();
        CheckIfExtraDifficultyExists();

        // Check the difficulty files if they exist and update the levels for each of them
        UpdateDifficultyLevelText(selectedDirectoryIndexPass);

        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
        if (easyDifficultyExist == true)
        {
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, "easy", hasPressedArrowKeyPass);
        }
        else if (advancedDifficultyExist == true)
        {
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, "advanced", hasPressedArrowKeyPass);
        }
        else if (extraDifficultyExist == true)
        {
            LoadBeatmapSongSelectInformation(selectedDirectoryIndex, "extra", hasPressedArrowKeyPass);
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
