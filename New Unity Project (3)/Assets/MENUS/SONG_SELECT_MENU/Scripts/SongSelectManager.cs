using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SongSelectManager : MonoBehaviour
{
    #region Variables
    // UI
    public Image leftSideGradientImage;

    // Text
    public TextMeshProUGUI songTitleText, songArtistText, beatmapInformationText, beatmapCreatorText, beatmapCreatorMessageText, currentDifficultyText;

    // Gameobjects
    public GameObject beatmapCountErrorPanel; // Beatmap error panel when 0 beatmaps are in the directory

    // Animation
    public Animator songInformationPanelAnimator;

    // Strings
    public string[] beatmapDirectories; // All beatmap folder locations in the beatmap directory
    private string beatmapSongName, beatmapSongArtist; // Beatmap information
    private string easyDifficultyLevel, normalDifficultyLevel, hardDifficultyLevel;
    private string currentBeatmapDifficulty; // Last selected beatmap difficulty
    private string easyBeatmapFileName, normalBeatmapFileName, hardBeatmapFileName; // Beatmap file names
    private string easyDifficultyName, normalDifficultyName, hardDifficultyName; // Beatmap difficulty names
    private string beatmapCreator, beatmapCreatedDate, beatmapCreatorMessage; // Creator
    private string disabledLevelTextValue;
    private string previousDifficulty;

    // Integers
    private int selectedBeatmapDirectoryIndex, previousBeatmapDirectoryIndex; // Beatmap directory indexs
    private float songPreviewStartTime;
    private float beatmapSongOffset, beatmapSongBpm;
    private int frameInterval;
    private int beatmapDirectoryCount;

    // Bools
    private bool easyDifficultyExist, normalDifficultyExist, hardDifficultyExist;
    private bool hasPlayedSongPreviewOnce; // Used to play the start preview once upon entering the song select screen for the first time so the song plays at the current set time once.
    private bool hasSelectedCurrentBeatmapButton;
    public AudioSource songAudioSource;

    // Scripts
    public DifficultyButton easyDifficultyButtonScript, normalDifficultyButtonScript, hardDifficultyButtonScript;
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public string EasyDifficultyName
    {
        get { return easyDifficultyName; }
    }

    public string NormalDifficultyName
    {
        get { return normalDifficultyName; }
    }

    public string HardDifficultyName
    {
        get { return hardDifficultyName; }
    }

    public int SelectedBeatmapDirectoryIndex
    {
        get { return selectedBeatmapDirectoryIndex; }
        set { selectedBeatmapDirectoryIndex = value; }
    }

    public string EasyBeatmapFileName
    {
        get { return easyBeatmapFileName; }
    }

    public string NormalBeatmapFileName
    {
        get { return normalBeatmapFileName; }
    }

    public string HardBeatmapFileName
    {
        get { return hardBeatmapFileName; }
    }

    public bool HasPlayedSongPreviewOnce
    {
        set { hasPlayedSongPreviewOnce = value; }
    }

    public string CurrentBeatmapDifficulty
    {
        get { return currentBeatmapDifficulty; }
    }
    #endregion

    #region Functions

    private void Awake()
    {
        ReferenceScriptManager();
        scriptManager.songSelectPanel.GetBeatmapDirectoryPaths(); // Get the directory paths for all the beatmap folders in the beatmap directory
        CheckBeatmapDirectories(); // Get and check the beatmaps in the directory#

        easyBeatmapFileName = "easy.dia";
        normalBeatmapFileName = "normal.dia";
        hardBeatmapFileName = "hard.dia";
        easyDifficultyName = "easy";
        normalDifficultyName = "normal";
        hardDifficultyName = "hard";
        disabledLevelTextValue = "X";
    }

    // Use this for initialization
    private void Start()
    {
        // Initialize
        hasPlayedSongPreviewOnce = false;

        // Property initalize
        selectedBeatmapDirectoryIndex = scriptManager.loadLastBeatmapManager.LastBeatmapDirectoryIndex; // Load the last beatmap directory index
        previousBeatmapDirectoryIndex = selectedBeatmapDirectoryIndex; // Assign the previous beatmap index to the current beatmap index
        currentBeatmapDifficulty = scriptManager.loadLastBeatmapManager.LastBeatmapDifficulty; // Check if the last selected difficulty was set or not (first time entering game or not)

        // Load the beatmap if it exists
        LoadBeatmapFileThatExists(selectedBeatmapDirectoryIndex);

        // Set to false at the start
        hasPlayedSongPreviewOnce = true;
    }

    private void ReferenceScriptManager()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
    }

    // Get and check the beatmaps in the directory
    private void CheckBeatmapDirectories()
    {
        // Get the beatmap directoriess
        beatmapDirectories = scriptManager.songSelectPanel.beatmapDirectoryPaths;

        // Check how many beatmaps are in the directory
        beatmapDirectoryCount = beatmapDirectories.Length;

        // Beatmap count error check
        // If there's more than 0 beatmaps
        switch (beatmapDirectoryCount)
        {
            case 0:
                beatmapCountErrorPanel.gameObject.SetActive(true);
                break;
            default:
                beatmapCountErrorPanel.gameObject.SetActive(false);
                break;
        }
    }

    // Load beatmap song select information
    public void LoadBeatmapSongSelectInformation(int _selectedBeatmapDirectoryIndex, string _beatmapDifficulty)
    {
        easyDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);
        normalDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);
        hardDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);

        if (previousDifficulty != _beatmapDifficulty)
        {
            // Update the personal best leaderboard button color to the difficulty currently selected
            switch (_beatmapDifficulty)
            {
                case "easy":
                    easyDifficultyButtonScript.gameObject.SetActive(true);
                    easyDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                    beatmapCreatorMessageText.color = scriptManager.uiColorManager.easyDifficultyColor;
                    leftSideGradientImage.color = new Color(scriptManager.uiColorManager.easyDifficultyColor.r,
                        scriptManager.uiColorManager.easyDifficultyColor.g, scriptManager.uiColorManager.easyDifficultyColor.b,
                        Constants.LEFT_SIDE_GRADIENT_IMAGE_ALPHA);

                    // Update highlighted colors for UI buttons
                    scriptManager.uiColorManager.difficultyColor = scriptManager.uiColorManager.easyDifficultyColor;
                    break;
                case "normal":
                    normalDifficultyButtonScript.gameObject.SetActive(true);
                    normalDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                    beatmapCreatorMessageText.color = scriptManager.uiColorManager.normalDifficultyColor;
                    leftSideGradientImage.color = new Color(scriptManager.uiColorManager.normalDifficultyColor.r,
                        scriptManager.uiColorManager.normalDifficultyColor.g, scriptManager.uiColorManager.normalDifficultyColor.b,
                        Constants.LEFT_SIDE_GRADIENT_IMAGE_ALPHA);

                    // Update highlighted colors for UI buttons
                    scriptManager.uiColorManager.difficultyColor = scriptManager.uiColorManager.normalDifficultyColor;
                    break;
                case "hard":
                    hardDifficultyButtonScript.gameObject.SetActive(true);
                    hardDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                    beatmapCreatorMessageText.color = scriptManager.uiColorManager.hardDifficultyColor;
                    leftSideGradientImage.color = new Color(scriptManager.uiColorManager.hardDifficultyColor.r,
                        scriptManager.uiColorManager.hardDifficultyColor.g, scriptManager.uiColorManager.hardDifficultyColor.b,
                        Constants.LEFT_SIDE_GRADIENT_IMAGE_ALPHA);

                    // Update highlighted colors for UI buttons
                    scriptManager.uiColorManager.difficultyColor = scriptManager.uiColorManager.hardDifficultyColor;
                    break;
                default:
                    break;
            }
        }

        // Assign new UI colors
        scriptManager.uiColorManager.UpdateDifficultyColorBlocks();
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.topMenu.menuDropdown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.songSelectPanel.difficultySortingDropDown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.songSelectPanel.sortingDropDown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.beatmapRanking.leaderboardSortViewDropdown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.beatmapRanking.leaderboardSortTypeDropdown);
        scriptManager.uiColorManager.UpdateButtonColors(scriptManager.topMenu.profileButton);
        scriptManager.uiColorManager.UpdateScrollbarColors(scriptManager.beatmapRanking.leaderboardScrollbar);
        scriptManager.uiColorManager.UpdateScrollbarColors(scriptManager.songSelectPanel.beatmapButtonListScrollbar);
        

        // If any difficulty files exist
        if (normalDifficultyExist == true || hardDifficultyExist == true || easyDifficultyExist == true)
        {
            // LOAD BEATMAP INFORMATION - IMAGES / NOTES / SCREEN CHANGES

            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(beatmapDirectories[_selectedBeatmapDirectoryIndex], _beatmapDifficulty);
            // Load the song select UI variables from the database
            beatmapSongName = Database.database.LoadedSongName;
            //beatmapCreator = Database.database.LoadedBeatmapCreator;

            // TEST DELETE THIS
            beatmapCreator = "SHPRO";


            beatmapCreatedDate = Database.database.LoadedBeatmapCreatedDate;
            beatmapSongArtist = Database.database.LoadedSongArtist;
            songPreviewStartTime = Database.database.LoadedSongPreviewStartTime;
            beatmapSongBpm = Database.database.LoadedBPM;
            beatmapSongOffset = Database.database.LoadedOffsetMS;
            beatmapCreatorMessage = Database.database.LoadedBeatmapCreatorMessage;

            // Load beatmap creator profile image
            scriptManager.uploadPlayerImage.CallBeatmapCreatorUploadImage(beatmapCreator, scriptManager.uploadPlayerImage.beatmapCreatorProfileImage);

            switch (scriptManager.backgroundManager.VideoTickBoxSelected)
            {
                case true:
                    // Load video or background image based on url
                    scriptManager.backgroundManager.LoadVideoOrImage(beatmapDirectories[_selectedBeatmapDirectoryIndex]);
                    // Play the background video
                    PlayVideo();
                    break;
                case false:
                    // Load only the background image
                    scriptManager.backgroundManager.LoadImageOnly(beatmapDirectories[_selectedBeatmapDirectoryIndex]);
                    break;
            }

            // Check if the first song preview when entering the song select menu has played once, if it hasn't play the song preview at the correct set time
            if (hasPlayedSongPreviewOnce == false)
            {
                // Reset, update and play the metronome effects for the song select scene
                scriptManager.metronomeForEffects.GetSongData(beatmapSongBpm, beatmapSongOffset);

                // Play the song preview for the first song when entering at the correct set time in the beatmap information
                scriptManager.songSelectPreview.GetBeatmapAudio(beatmapDirectories[_selectedBeatmapDirectoryIndex], songPreviewStartTime);

                // Set to true
                hasPlayedSongPreviewOnce = true;
            }

            // Change the current song selected text to the information loaded from the current directory
            songTitleText.text = beatmapSongName;
            songArtistText.text = beatmapSongArtist;

            // Update current difficulty text
            currentDifficultyText.text = _beatmapDifficulty.ToUpper();

            // Update the other beatmap information text
            string totalObjects = Database.database.LoadedPositionX.Count.ToString();

            // Update beatmap information text
            beatmapInformationText.text = "[ " + beatmapSongBpm + " BPM | " + totalObjects + " OBJECTS | " + beatmapCreatedDate + " ]";

            beatmapCreatorText.text = "Beatmap designed by " + beatmapCreator;
            beatmapCreatorMessageText.text = beatmapCreatorMessage;

            // Play current selected beatmap count animation
            scriptManager.songSelectPanel.selectedBeatmapCountTextAnimator.Play("SelectedBeatmapNumberText_Animation", 0, 0f);

            // Save the selected beatmap index
            scriptManager.loadLastBeatmapManager.SetPlayerPrefsLastBeatmapIndex(_selectedBeatmapDirectoryIndex);

            // Update the previous index to be the current index
            previousBeatmapDirectoryIndex = _selectedBeatmapDirectoryIndex;

            // Save the last selected difficulty
            scriptManager.loadLastBeatmapManager.SetPlayerPrefsLastBeatmapDifficulty(_beatmapDifficulty);

            // Set the last selected difficulty
            currentBeatmapDifficulty = _beatmapDifficulty;

            // Update the selected beatmap directory index
            selectedBeatmapDirectoryIndex = _selectedBeatmapDirectoryIndex;

            // Get the leaderboard ranking information
            scriptManager.beatmapRanking.GetLeaderboardTableName();

            // Play animation
            songInformationPanelAnimator.Play("SongInformationPanel_Animation", 0, 0f);
        }
        else
        {
            // As we tried to access a file with no difficulties remain on the current index
            selectedBeatmapDirectoryIndex = previousBeatmapDirectoryIndex;
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
            if (scriptManager.songSelectPanel.HasLoadedAllBeatmapButtons == true)
            {
                EventSystem.current.SetSelectedGameObject(null);

                scriptManager.songSelectPanel.instantiatedBeatmapButtonList[selectedBeatmapDirectoryIndex].GetComponent<Button>().Select();

                hasSelectedCurrentBeatmapButton = true;
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
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[_selectedDirectoryIndex], easyDifficultyName);
            // Assign the easy beatmap difficulty value
            easyDifficultyLevel = Database.database.LoadedBeatmapDifficultyLevel;
            // Update the easy beatmap difficulty level text
            easyDifficultyButtonScript.levelText.text = easyDifficultyLevel;
        }

        // If the normalbeatmap difficulty file exists
        if (normalDifficultyExist == true)
        {
            // Load the normal beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[_selectedDirectoryIndex], normalDifficultyName);
            // Assign the normal beatmap difficulty value
            normalDifficultyLevel = Database.database.LoadedBeatmapDifficultyLevel;
            // Update the normal beatmap difficulty level text
            normalDifficultyButtonScript.levelText.text = normalDifficultyLevel;
        }

        // If the hard beatmap difficulty file exists
        if (hardDifficultyExist == true)
        {
            // Load the hard beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[_selectedDirectoryIndex], hardDifficultyName);
            // Assign the hard beatmap difficulty level
            hardDifficultyLevel = Database.database.LoadedBeatmapDifficultyLevel;
            // Update the hard beatmap difficulty level text
            hardDifficultyButtonScript.levelText.text = hardDifficultyLevel;
        }
    }

    // Play the background video
    private void PlayVideo()
    {
        // Play video based on the current active video player
        switch (scriptManager.backgroundManager.ActiveVideoPlayerIndex)
        {
            case 1:
                // Stop videp player 2
                scriptManager.backgroundManager.videoPlayer2.Stop();

                // Play video player 1
                scriptManager.backgroundManager.videoPlayer.Play();

                // Set video play start time
                scriptManager.backgroundManager.videoPlayer.time = songPreviewStartTime;
                break;
            case 2:
                // Stop video player 1
                scriptManager.backgroundManager.videoPlayer.Stop();

                // Play video player 2
                scriptManager.backgroundManager.videoPlayer2.Play();

                // Set video play start time
                scriptManager.backgroundManager.videoPlayer2.time = songPreviewStartTime;
                break;
        }
    }

    // Check the beatmap directory for the easy difficulty file
    public void CheckIfEasyDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the easy.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + easyBeatmapFileName))
        {
            // ALLOW GAMEPLAY
            // Set bool to true as the file exists
            easyDifficultyExist = true;
            // Enable the button
            easyDifficultyButtonScript.difficultyButton.interactable = true;
        }
        else
        {
            // Disable the button
            easyDifficultyButtonScript.difficultyButton.interactable = false;
            // Print disabled text on the level
            easyDifficultyButtonScript.levelText.text = disabledLevelTextValue;
            // Set bool to not exist as the easy difficulty file does not exist
            easyDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the normal difficulty file
    public void CheckIfNormalDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the normal.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + normalBeatmapFileName))
        {
            // ALLOW GAMEPLAY
            // Set bool to exist as the file exists
            normalDifficultyExist = true;
            // Enable the button
            normalDifficultyButtonScript.difficultyButton.interactable = true;
        }
        else
        {
            // Disable the button
            normalDifficultyButtonScript.difficultyButton.interactable = false;
            // Print disabled text on the level
            normalDifficultyButtonScript.levelText.text = disabledLevelTextValue;
            // Set bool to not exist
            normalDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the hard difficulty file
    public void CheckIfHardDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the hard.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + hardBeatmapFileName))
        {
            // ALLOW GAMEPLAY
            // Set bool to exist
            hardDifficultyExist = true;
            // Enable the button
            hardDifficultyButtonScript.difficultyButton.interactable = true;
        }
        else
        {
            // Disable the button
            hardDifficultyButtonScript.difficultyButton.interactable = false;
            // Print disabled text on the level
            hardDifficultyButtonScript.levelText.text = disabledLevelTextValue;
            // Set bool to exist as the file does not exist
            hardDifficultyExist = false;
        }
    }

    // Check the beatmap files that exist (this is requested by songSelectMenuFlash), check what files exist and load the file that exists
    public void LoadBeatmapFileThatExists(int _selectedBeatmapDirectoryIndex)
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
        CheckIfEasyDifficultyExists(_selectedBeatmapDirectoryIndex);
        CheckIfNormalDifficultyExists(_selectedBeatmapDirectoryIndex);
        CheckIfHardDifficultyExists(_selectedBeatmapDirectoryIndex);

        // Check the difficulty files if they exist and update the levels for each of them
        UpdateDifficultyLevelText(_selectedBeatmapDirectoryIndex);

        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
    }

    // Check the difficulty and try to load that specific beatmap file if it exists
    private void LoadSelectedDifficultyBeatmapFile(int _selectedBeatmapDirectoryIndex)
    {
        // If the last beatmap difficulty has been set
        if (currentBeatmapDifficulty != null)
        {
            // Choose the beatmap file to load based on the last difficulty loaded (if it exists)
            switch (currentBeatmapDifficulty)
            {
                case "easy":
                    // If the easy.dia difficulty file exists
                    if (easyDifficultyExist == true)
                    {
                        // Load the song select information for easy
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, easyDifficultyName);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
                    }
                    break;

                case "normal":
                    // If the normal.dia difficulty file exists
                    if (normalDifficultyExist == true)
                    {
                        // Load the song select information for normal
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, normalDifficultyName);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
                    }
                    break;

                case "hard":
                    // If the hard.dia file difficulty exists 
                    if (hardDifficultyExist == true)
                    {
                        // Load the song select information for hard
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, hardDifficultyName);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
                    }
                    break;
            }
        }
    }

    // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
    private void LoadFirstBeatmapFileThatExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the easy.dia difficulty file exists
        if (easyDifficultyExist == true)
        {
            // Load the song select information for easy
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, easyDifficultyName);
        }
        // If the normal.dia difficulty file exists
        else if (normalDifficultyExist == true)
        {
            // Load the song select information for normal
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, normalDifficultyName);
        }
        // If the hard.dia file difficulty exists 
        else if (hardDifficultyExist == true)
        {
            // Load the song select information for hard
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, hardDifficultyName);
        }
        else
        {
            // No difficulties were found for that beatmap - load the previous beatmap in the list?
            AttemptToLoadPreviousBeatmap(_selectedBeatmapDirectoryIndex);
        }
    }

    private void AttemptToLoadPreviousBeatmap(int _selectedBeatmapDirectoryIndex)
    {
        // Try to load the previous beatmap
        _selectedBeatmapDirectoryIndex = (_selectedBeatmapDirectoryIndex - 1);
        selectedBeatmapDirectoryIndex = (_selectedBeatmapDirectoryIndex - 1);

        // Load previous beatmap
        LoadBeatmapFileThatExists((_selectedBeatmapDirectoryIndex));
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
    #endregion
}

