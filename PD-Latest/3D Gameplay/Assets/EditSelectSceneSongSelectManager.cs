using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class EditSelectSceneSongSelectManager : MonoBehaviour {

    // UI
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI creatorAndDateText, beatmapInformationText;
    public TextMeshProUGUI difficultyOptionEasyLevelText, difficultyOptionAdvancedLevelText, difficultyOptionExtraLevelText;
    public Button difficultyOptionEasyButton, difficultyOptionAdvancedButton, difficultyOptionExtraButton;
    public Image pressedKeySImage, pressedKeyDImage, pressedKeyFImage, pressedKeyJImage, pressedKeyKImage, pressedKeyLImage;
    public Image disabledPressedKeySImage, disabledPressedKeyDImage, disabledPressedKeyFImage, disabledPressedKeyJImage,
        disabledPressedKeyKImage, disabledPressedKeyLImage;
    public Image songPlayerBarImage;

    // Event triggers
    public EventTrigger easyDifficultyButtonEventTrigger, advancedDifficultyButtonEventTrigger, extraDifficultyButtonEventTrigger; // Button event triggers

    // Gameobjects
    public GameObject beatmapCountErrorPanel; // Beatmap error panel when 0 beatmaps are in the directory

    // Colors
    public Color easyDifficultyButtonColor, advancedDifficultyButtonColor, extraDifficultyButtonColor;

    // Animation
    public Animator songSelectFlashAnimator; // The flash animator

    // Strings
    public string[] beatmapDirectories; // All beatmap folder locations in the beatmap directory
    private string beatmapSongName, beatmapSongArtist; // Beatmap information
    private string easyDifficultyLevel, advancedDifficultyLevel, extraDifficultyLevel, disabledLevelText = "X";
    private string lastBeatmapDifficulty; // Last selected beatmap difficulty
    private string easyBeatmapFileName, advancedBeatmapFileName, extraBeatmapFileName; // Beatmap file names
    private string easyDifficultyName, advancedDifficultyName, extraDifficultyName; // Beatmap difficulty names
    private string beatmapCreator, beatmapCreatedDate;
    public List<string> userCreatedBeatmapDirectories = new List<string>(); // List of all beatmaps that are created by the user or has permission to edit

    // Integers
    private int selectedBeatmapDirectoryIndex, previousBeatmapDirectoryIndex; // Beatmap directory indexs
    private float songPreviewStartTime;
    private float beatmapSongOffset, beatmapSongBpm;
    private int songClipChosenIndex;
    private int frameInterval;
    private int beatmapDirectoryCount;

    // Bools
    private bool easyDifficultyExist, advancedDifficultyExist, extraDifficultyExist;
    private bool beatmapKeyExistsS, beatmapKeyExistsD, beatmapKeyExistsF, beatmapKeyExistsJ, beatmapKeyExistsK,
        beatmapKeyExistsL; // Beatmap keys featured in the beatmap
    private bool hasPlayedSongPreviewOnce; // Used to play the start preview once upon entering the song select screen for the first time so the song plays at the current set time once.
    private bool hasSelectedCurrentBeatmapButton;
    public AudioSource songAudioSource;

    // Scripts
    private SongSelectPreview songSelectPreview; // Get reference to song select preview to control playing the song previews
    private BackgroundManager backgroundManager; // Reference to the background image manager for loading the beatmap image
    private SongSelectPanel songSelectPanel; // Reference to the SongSelectPanel for loading the song beatmap buttons with the directories found
    private MetronomeForEffects metronomeForEffects; // MetronomeForEffects - controls beat animations


    // Properties

    // Get the selectedBeatmapDirectoryIndex
    public int SelectedBeatmapDirectoryIndex
    {
        get { return selectedBeatmapDirectoryIndex; }
        set { selectedBeatmapDirectoryIndex = value; }
    }


    // Use this for initialization
    void Start()
    {

        // Initialize
        hasPlayedSongPreviewOnce = false;
        songClipChosenIndex = 0;
        selectedBeatmapDirectoryIndex = 0;
        easyBeatmapFileName = "easy.dia";
        advancedBeatmapFileName = "advanced.dia";
        extraBeatmapFileName = "extra.dia";
        easyDifficultyName = "easy";
        advancedDifficultyName = "advanced";
        extraDifficultyName = "extra";

        // Reference
        songSelectPanel = FindObjectOfType<SongSelectPanel>();
        songSelectPreview = FindObjectOfType<SongSelectPreview>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();

        // Functions
        CheckBeatmapDirectories(); // Get and check the beatmaps in the directory

        CheckIfBeatmapWasCreatedByUser(); // Check all beatmap files to see if the beatmap was created by the user logged in

        // Property initalize
        previousBeatmapDirectoryIndex = selectedBeatmapDirectoryIndex; // Assign the previous beatmap index to the current beatmap index

        // Load the beatmap if it exists
        LoadBeatmapFileThatExists(selectedBeatmapDirectoryIndex, false);

        // Set to false at the start
        hasPlayedSongPreviewOnce = true;
    }

    // Get and check the beatmaps in the directory
    private void CheckBeatmapDirectories()
    {
        // Get the beatmap directoriess
        beatmapDirectories = Directory.GetDirectories(@"c:\Beatmaps");

        // Check how many beatmaps are in the directory
        beatmapDirectoryCount = beatmapDirectories.Length;

        // Beatmap count error check
        // If there's more than 0 beatmaps
        if (beatmapDirectoryCount <= 0)
        {
            // Enable error panel
            beatmapCountErrorPanel.gameObject.SetActive(true);
        }
        else
        {
            // Disable error panel
            beatmapCountErrorPanel.gameObject.SetActive(false);
        }
    }

    // Load beatmap song select information
    public void LoadBeatmapSongSelectInformation(int _selectedBeatmapDirectoryIndex, string _beatmapDifficulty, bool _hasPressedArrowKey)
    {
        // Update the personal best leaderboard button color to the difficulty currently selected
        switch (_beatmapDifficulty)
        {
            case "easy":
                FlashImage("SongSelectMenuFlashEasy");
                songPlayerBarImage.color = easyDifficultyButtonColor;
                break;
            case "advanced":
                FlashImage("SongSelectMenuFlashAdvanced");
                songPlayerBarImage.color = advancedDifficultyButtonColor;
                break;
            case "extra":
                FlashImage("SongSelectMenuFlashExtra");
                songPlayerBarImage.color = extraDifficultyButtonColor;
                break;
            default:
                FlashImage("SongSelectMenuFlash");
                songPlayerBarImage.color = advancedDifficultyButtonColor;
                break;
        }

        // Update the difficulty selected for the metronomeForEffects animations
        metronomeForEffects.UpdateDifficultyAnimations(_beatmapDifficulty);

        // If any difficulty files exist
        if (advancedDifficultyExist == true || extraDifficultyExist == true || easyDifficultyExist == true)
        {
            // Load all beatmap file information from the database
            LoadBeatmapDatabaseFileInformation(_selectedBeatmapDirectoryIndex, _beatmapDifficulty);

            // Check if the first song preview when entering the song select menu has played once, if it hasn't play the song preview at the correct set time
            if (hasPlayedSongPreviewOnce == false || _hasPressedArrowKey == true)
            {
                // Play the song preview for the first song when entering at the correct set time in the beatmap information
                PlaySongPreview();
                // Reset and allow the current tick based of current song time to be calcualted
                metronomeForEffects.ResetCalculateCurrentTick();

                // Reset, update and play the metronome effects for the song select scene
                metronomeForEffects.GetSongData(beatmapSongBpm, beatmapSongOffset);
                metronomeForEffects.CalculateIntervals();

                // Set to true
                hasPlayedSongPreviewOnce = true;
                // Set back to false
                _hasPressedArrowKey = false;
            }

            // Load the image by passing the current beatmap directory
            backgroundManager.LoadEditorBeatmapImage(userCreatedBeatmapDirectories[_selectedBeatmapDirectoryIndex]);

            // Change the current song selected text to the information loaded from the current directory
            songTitleText.text = beatmapSongName + " [ " + beatmapSongArtist + " ] ";

            // Update the other beatmap information text
            string totalObjects = Database.database.loadedPositionX.Count.ToString();

            beatmapInformationText.text = "[ Created by " + beatmapCreator + " ] [ " + beatmapCreatedDate + " ] " +
            "[ Objects: " + totalObjects + " ] " + " [ BPM: " + beatmapSongBpm.ToString() + " ]" +
                " [ Duration: " + UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.clip.length) + " ]";

            // Enable the required keys for the beatmap images
            EnableKeysRequiredForBeatmap();

            // Update the previous index to be the current index
            previousBeatmapDirectoryIndex = _selectedBeatmapDirectoryIndex;

            // Update the selected beatmap directory index
            selectedBeatmapDirectoryIndex = _selectedBeatmapDirectoryIndex;
        }
        else
        {
            // As we tried to access a file with no difficulties remain on the current index
            selectedBeatmapDirectoryIndex = previousBeatmapDirectoryIndex;
        }
    }

    // Load the database and beatmap information for the beatmap directory selected
    private void LoadBeatmapDatabaseFileInformation(int _selectedBeatmapDirectoryIndex, string _beatmapDifficulty)
    {
        // LOAD BEATMAP INFORMATION - IMAGES / NOTES / SCREEN CHANGES

        // Load the database and beatmap information for the beatmap directory selected
        Database.database.Load(userCreatedBeatmapDirectories[_selectedBeatmapDirectoryIndex], _beatmapDifficulty);
        // Load the song select UI variables from the database
        beatmapSongName = Database.database.LoadedSongName;
        beatmapCreator = Database.database.LoadedBeatmapCreator;
        beatmapCreatedDate = Database.database.LoadedBeatmapCreatedDate;
        beatmapSongArtist = Database.database.LoadedSongArtist;
        songPreviewStartTime = Database.database.LoadedSongPreviewStartTime;
        beatmapSongBpm = Database.database.LoadedBPM;
        beatmapSongOffset = Database.database.LoadedOffsetMS;
        songClipChosenIndex = Database.database.LoadedSongClipChosenIndex;
        beatmapKeyExistsS = Database.database.LoadedPressedKeyS;
        beatmapKeyExistsD = Database.database.LoadedPressedKeyD;
        beatmapKeyExistsF = Database.database.LoadedPressedKeyF;
        beatmapKeyExistsJ = Database.database.LoadedPressedKeyJ;
        beatmapKeyExistsK = Database.database.LoadedPressedKeyK;
        beatmapKeyExistsL = Database.database.LoadedPressedKeyL;
    }

    // Get the creator from the file loaded
    private void LoadBeatmapDatabaseCreatorInformation(int _selectedBeatmapDirectoryIndex, string _beatmapDifficulty)
    {
        Database.database.Load(beatmapDirectories[_selectedBeatmapDirectoryIndex], _beatmapDifficulty);
        beatmapCreator = Database.database.LoadedBeatmapCreator;

        // Check if the creator name is the same as the user logged in
        if (beatmapCreator == MySQLDBManager.username)
        {
            // Add to the list
            userCreatedBeatmapDirectories.Add(beatmapDirectories[_selectedBeatmapDirectoryIndex]);
        }
    }

    // Check if the beatmap file loaded was created by the user logged in
    private void CheckIfBeatmapWasCreatedByUser()
    {
        // Loop through all user created beatmap directories
        for (int i = 0; i < beatmapDirectories.Length; i++)
        {
            // Load the creator file information if it exists
            LoadBeatmapFileThatExistsCreatorInformation(i);
        }

        // If all files have been checked and there exists 1 directory found to contain the logged in user as it's creator
        if (userCreatedBeatmapDirectories.Count != 0)
        {
            songSelectPanel.GetLoggedInUserEditableBeatmapDirectories(); // Get the directory paths for all the user editable beatmaps
        }
    }

    private void Update()
    {
        SelectCurrentBeatmapButton();
    }

    // Reset
    public void ResetCurrentSelectedBeatmapButton()
    {
        hasSelectedCurrentBeatmapButton = false;
    }

    // Select the current beatmap button
    private void SelectCurrentBeatmapButton()
    {
        if (hasSelectedCurrentBeatmapButton == false)
        {
            if (songSelectPanel.HasLoadedAllBeatmapButtons == true)
            {
                if (songSelectPanel.instantiatedBeatmapButtonList.Count != 0)
                {
                    EventSystem.current.SetSelectedGameObject(null);

                    songSelectPanel.instantiatedBeatmapButtonList[selectedBeatmapDirectoryIndex].GetComponent<Button>().Select();

                    hasSelectedCurrentBeatmapButton = true;
                }
            }
        }
    }

    // Update the difficulty level text for the beatmap selected
    public void UpdateDifficultyLevelText(int _selectedDirectoryIndex)
    {
        // If the easy beatmap difficulty file exists
        if (easyDifficultyExist == true)
        {
            // Load the easy beatmap difficulty level 
            Database.database.LoadBeatmapDifficultyLevel(userCreatedBeatmapDirectories[_selectedDirectoryIndex], easyDifficultyName);
            // Assign the easy beatmap difficulty value
            easyDifficultyLevel = Database.database.LoadedBeatmapEasyDifficultyLevel;
            // Update the easy beatmap difficulty level text
            difficultyOptionEasyLevelText.text = easyDifficultyLevel;
        }

        // If the advanced beatmap difficulty file exists
        if (advancedDifficultyExist == true)
        {
            // Load the advanced beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(userCreatedBeatmapDirectories[_selectedDirectoryIndex], advancedDifficultyName);
            // Assign the advanced beatmap difficulty value
            advancedDifficultyLevel = Database.database.LoadedBeatmapAdvancedDifficultyLevel;
            // Update the advanced beatmap difficulty level text
            difficultyOptionAdvancedLevelText.text = advancedDifficultyLevel;
        }

        // If the extra beatmap difficulty file exists
        if (extraDifficultyExist == true)
        {
            // Load the extra beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(userCreatedBeatmapDirectories[_selectedDirectoryIndex], extraDifficultyName);
            // Assign the extra beatmap difficulty level
            extraDifficultyLevel = Database.database.LoadedBeatmapExtraDifficultyLevel;
            // Update the extra beatmap difficulty level text
            difficultyOptionExtraLevelText.text = extraDifficultyLevel;
        }
    }

    // Animate the flash on screen for the difficulties: easy/advanced/extra
    public void FlashImage(string _animation)
    {
        songSelectFlashAnimator.Play(_animation);
    }

    // Play the song preview at the saved preview time
    public void PlaySongPreview()
    {
        // Start the song preview as it has now been loaded
        songSelectPreview.PlaySongSelectScenePreview(songPreviewStartTime, songClipChosenIndex);
    }

    // Check the beatmap directory for the easy difficulty file
    public void CheckIfEasyDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the easy.dia file exists
        if (File.Exists(userCreatedBeatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + easyBeatmapFileName))
        {
            Debug.Log("easy exists " + _selectedBeatmapDirectoryIndex);
            // ALLOW GAMEPLAY
            // Set bool to true as the file exists
            easyDifficultyExist = true;
            // Enable the button
            difficultyOptionEasyButton.interactable = true;
            // Enable the event trigger 
            easyDifficultyButtonEventTrigger.enabled = true;
        }
        else
        {
            // Disable the button
            difficultyOptionEasyButton.interactable = false;
            // Disable the event trigger to prevent it trying to load the easy file that doesn't exist
            easyDifficultyButtonEventTrigger.enabled = false;
            // Print disabled text on the level
            difficultyOptionEasyLevelText.text = disabledLevelText;
            // Set bool to not exist as the easy difficulty file does not exist
            easyDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the advanced difficulty file
    public void CheckIfAdvancedDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the advanced.dia file exists
        if (File.Exists(userCreatedBeatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + advancedBeatmapFileName))
        {
            Debug.Log("advanced exists " + _selectedBeatmapDirectoryIndex);
            // ALLOW GAMEPLAY
            // Set bool to exist as the file exists
            advancedDifficultyExist = true;
            // Enable the button
            difficultyOptionAdvancedButton.interactable = true;
            // Enable the event trigger 
            advancedDifficultyButtonEventTrigger.enabled = true;
        }
        else
        {
            Debug.Log("extra exists " + _selectedBeatmapDirectoryIndex);
            // Disable the button
            difficultyOptionAdvancedButton.interactable = false;
            // Disable the event trigger to prevent it trying to load the advanced file that doesn't exist
            advancedDifficultyButtonEventTrigger.enabled = false;
            // Print disabled level text as the beatmap difficulty level
            difficultyOptionAdvancedLevelText.text = disabledLevelText;
            // Set bool to not exist
            advancedDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the extra difficulty file
    public void CheckIfExtraDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the extra.dia file exists
        if (File.Exists(userCreatedBeatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + extraBeatmapFileName))
        {
            // ALLOW GAMEPLAY
            // Set bool to exist
            extraDifficultyExist = true;
            // Enable the button
            difficultyOptionExtraButton.interactable = true;
            // Enable the event trigger 
            extraDifficultyButtonEventTrigger.enabled = true;
        }
        else
        {
            // Disable the button
            difficultyOptionExtraButton.interactable = false;
            // Disable the event trigger to prevent it trying to load the extra file that doesn't exist
            extraDifficultyButtonEventTrigger.enabled = false;
            // Print disabled text on the level
            difficultyOptionExtraLevelText.text = disabledLevelText;
            // Set bool to exist as the file does not exist
            extraDifficultyExist = false;
        }
    }

    // Load the beatmap file that exists and get the creator information from the file only
    public void LoadBeatmapFileThatExistsCreatorInformation(int _selectedBeatmapDirectoryIndex)
    {
        // Do a check on the _selectedDirectoryIndexPass, if it's out of the directory range reset back to 0 to loop through the song list
        if (_selectedBeatmapDirectoryIndex >= beatmapDirectories.Length)
        {
            // Reset the beatmap directory to the beginning of the list
            _selectedBeatmapDirectoryIndex = 0;
        }
        // Do a check on the _selectedDirectoryIndex, if it's out of the directory range go to max range to loop through the song list
        else if (_selectedBeatmapDirectoryIndex < 0)
        {
            // Set the index to the last song in the list
            _selectedBeatmapDirectoryIndex = beatmapDirectories.Length - 1;
        }

        // Check the beatmap directory for the beatmap difficulty files and whether they exist or not
        // If the easy.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + easyBeatmapFileName))
        {
            // Get the creator from the file loaded
            LoadBeatmapDatabaseCreatorInformation(_selectedBeatmapDirectoryIndex, easyDifficultyName);
        }
        else if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + advancedBeatmapFileName))
        {
            // Get the creator from the file loaded
            LoadBeatmapDatabaseCreatorInformation(_selectedBeatmapDirectoryIndex, advancedDifficultyName);
        }
        else if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + extraBeatmapFileName))
        {
            // Get the creator from the file loaded
            LoadBeatmapDatabaseCreatorInformation(_selectedBeatmapDirectoryIndex, extraDifficultyName);
        }
    }

    // Check the beatmap files that exist (this is requested by songSelectMenuFlash), check what files exist and load the file that exists
    public void LoadBeatmapFileThatExists(int _selectedBeatmapDirectoryIndex, bool _hasPressedArrowKey)
    {
        // Do a check on the _selectedDirectoryIndexPass, if it's out of the directory range reset back to 0 to loop through the song list
        if (_selectedBeatmapDirectoryIndex >= userCreatedBeatmapDirectories.Count)
        {
            // Reset the beatmap directory to the beginning of the list
            _selectedBeatmapDirectoryIndex = 0;
        }
        // Do a check on the _selectedDirectoryIndex, if it's out of the directory range go to max range to loop through the song list
        else if (_selectedBeatmapDirectoryIndex < 0)
        {
            // Set the index to the last song in the list
            _selectedBeatmapDirectoryIndex = userCreatedBeatmapDirectories.Count - 1;
        }

        // If there's directories to check
        if (userCreatedBeatmapDirectories.Count != 0)
        {
            // Check the beatmap directory for the beatmap difficulty files and whether they exist or not
            CheckIfEasyDifficultyExists(_selectedBeatmapDirectoryIndex);
            CheckIfAdvancedDifficultyExists(_selectedBeatmapDirectoryIndex);
            CheckIfExtraDifficultyExists(_selectedBeatmapDirectoryIndex);

            // Check the difficulty files if they exist and update the levels for each of them
            UpdateDifficultyLevelText(_selectedBeatmapDirectoryIndex);

            LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
        }
        else
        {
            // Disable all difficulty buttons and interactivity
            DisableAllDifficultyButtons();
        }
    }

    // Disable all difficulty buttons and interactivity
    private void DisableAllDifficultyButtons()
    {
        // EASY
        // Disable the button
        difficultyOptionEasyButton.interactable = false;
        // Disable the event trigger to prevent it trying to load the extra file that doesn't exist
        easyDifficultyButtonEventTrigger.enabled = false;
        // Print disabled text on the level
        difficultyOptionEasyLevelText.text = disabledLevelText;
        // Set bool to exist as the file does not exist
        easyDifficultyExist = false;

        // ADVANCED
        // Disable the button
        difficultyOptionAdvancedButton.interactable = false;
        // Disable the event trigger to prevent it trying to load the extra file that doesn't exist
        advancedDifficultyButtonEventTrigger.enabled = false;
        // Print disabled text on the level
        difficultyOptionAdvancedLevelText.text = disabledLevelText;
        // Set bool to exist as the file does not exist
        advancedDifficultyExist = false;

        // EXTRA
        // Disable the button
        difficultyOptionExtraButton.interactable = false;
        // Disable the event trigger to prevent it trying to load the extra file that doesn't exist
        extraDifficultyButtonEventTrigger.enabled = false;
        // Print disabled text on the level
        difficultyOptionExtraLevelText.text = disabledLevelText;
        // Set bool to exist as the file does not exist
        extraDifficultyExist = false;
    }

    // Check the difficulty and try to load that specific beatmap file if it exists
    private void LoadSelectedDifficultyBeatmapFile(int _selectedBeatmapDirectoryIndex, bool _hasPressedArrowKey)
    {
        // If the last beatmap difficulty has been set
        if (lastBeatmapDifficulty != null)
        {
            // Choose the beatmap file to load based on the last difficulty loaded (if it exists)
            switch (lastBeatmapDifficulty)
            {
                case "easy":
                    // If the easy.dia difficulty file exists
                    if (easyDifficultyExist == true)
                    {
                        // Load the song select information for easy
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, easyDifficultyName, _hasPressedArrowKey);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
                    }
                    break;

                case "advanced":
                    // If the advanced.dia difficulty file exists
                    if (advancedDifficultyExist == true)
                    {
                        // Load the song select information for advanced
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, advancedDifficultyName, _hasPressedArrowKey);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
                    }
                    break;

                case "extra":
                    // If the extra.dia file difficulty exists 
                    if (extraDifficultyExist == true)
                    {
                        // Load the song select information for extra
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, extraDifficultyName, _hasPressedArrowKey);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
                    }
                    break;
            }
        }
    }

    // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
    private void LoadFirstBeatmapFileThatExists(int _selectedBeatmapDirectoryIndex, bool _hasPressedArrowKey)
    {
        // If the easy.dia difficulty file exists
        if (easyDifficultyExist == true)
        {
            // Load the song select information for easy
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, easyDifficultyName, _hasPressedArrowKey);
        }
        // If the advanced.dia difficulty file exists
        else if (advancedDifficultyExist == true)
        {
            // Load the song select information for advanced
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, advancedDifficultyName, _hasPressedArrowKey);
        }
        // If the extra.dia file difficulty exists 
        else if (extraDifficultyExist == true)
        {
            // Load the song select information for extra
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, extraDifficultyName, _hasPressedArrowKey);
        }
        else
        {

        }
    }


    private void AttemptToLoadPreviousBeatmap(int _selectedBeatmapDirectoryIndex, bool _hasPressedArrowKey)
    {
        // Try to load the previous beatmap
        _selectedBeatmapDirectoryIndex = (_selectedBeatmapDirectoryIndex - 1);
        selectedBeatmapDirectoryIndex = (_selectedBeatmapDirectoryIndex - 1);

        // Load previous beatmap
        LoadBeatmapFileThatExists((_selectedBeatmapDirectoryIndex), _hasPressedArrowKey);
    }

    // Enable the keys required for the beatmap
    public void EnableKeysRequiredForBeatmap()
    {
        if (beatmapKeyExistsS == true)
        {
            pressedKeySImage.gameObject.SetActive(true);
            disabledPressedKeySImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeySImage.gameObject.SetActive(true);
        }

        if (beatmapKeyExistsD == true)
        {
            pressedKeyDImage.gameObject.SetActive(true);
            disabledPressedKeyDImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyDImage.gameObject.SetActive(true);
        }

        if (beatmapKeyExistsF == true)
        {
            pressedKeyFImage.gameObject.SetActive(true);
            disabledPressedKeyFImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyFImage.gameObject.SetActive(true);
        }

        if (beatmapKeyExistsJ == true)
        {
            pressedKeyJImage.gameObject.SetActive(true);
            disabledPressedKeyJImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyJImage.gameObject.SetActive(true);
        }

        if (beatmapKeyExistsK == true)
        {
            pressedKeyKImage.gameObject.SetActive(true);
            disabledPressedKeyKImage.gameObject.SetActive(false);
        }
        else
        {
            disabledPressedKeyKImage.gameObject.SetActive(true);
        }

        if (beatmapKeyExistsL == true)
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

    // Increment selected directory index
    public void IncrementSelectedBeatmapDirectoryIndex()
    {
        selectedBeatmapDirectoryIndex++;
    }

    // Get random beatmap index
    public void RandomSelectedBeatmapDirectoryIndex()
    {
        selectedBeatmapDirectoryIndex = Random.Range(0, beatmapDirectories.Length);
    }


    // Decrement selected directory index
    public void DecrementSelectedBeatmapDirectoryIndex()
    {
        // Error 
        selectedBeatmapDirectoryIndex--;
    }

    // Get the beatmap directory from the index passed
    public string GetBeatmapDirectory(int _directoryIndex)
    {
        return beatmapDirectories[_directoryIndex];
    }
}

