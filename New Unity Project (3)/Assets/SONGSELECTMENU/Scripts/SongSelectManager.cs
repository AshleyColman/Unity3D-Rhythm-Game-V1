using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SongSelectManager : MonoBehaviour
{

    // UI
    public Image topColorPanelImage, bottomColorPanelImage;

    public TextMeshProUGUI songTitleText, songArtistText, beatmapInformationText, beatmapCreatorText, beatmapCreatorMessageText;
    public TextMeshProUGUI difficultyButtonEasyText, difficultyButtonAdvancedText, difficultyButtonExtraText;
    public TextMeshProUGUI defaultEasyNameText, defaultAdvancedNameText, defaultExtraNameText;
    public TextMeshProUGUI selectedEasyNameText, selectedAdvancedNameText, selectedExtraNameText;
    public GameObject easySelectedGameobject, advancedSelectedGameobject, extraSelectedGameobject;
    public Button difficultyOptionEasyButton, difficultyOptionAdvancedButton, difficultyOptionExtraButton;

    // Event triggers
    public EventTrigger easyDifficultyButtonEventTrigger, advancedDifficultyButtonEventTrigger, extraDifficultyButtonEventTrigger; // Button event triggers

    // Gameobjects
    public GameObject beatmapCountErrorPanel; // Beatmap error panel when 0 beatmaps are in the directory

    // Colors
    public Color easyDifficultyButtonColor, advancedDifficultyButtonColor, extraDifficultyButtonColor;

    // Animation
    public Animator songSelectFlashAnimator, songInformationPanelAnimator;

    // Strings
    public string[] beatmapDirectories; // All beatmap folder locations in the beatmap directory
    private string beatmapSongName, beatmapSongArtist; // Beatmap information
    private string easyDifficultyLevel, advancedDifficultyLevel, extraDifficultyLevel;
    private string currentBeatmapDifficulty; // Last selected beatmap difficulty
    private string easyBeatmapFileName, advancedBeatmapFileName, extraBeatmapFileName; // Beatmap file names
    private string easyDifficultyName, advancedDifficultyName, extraDifficultyName; // Beatmap difficulty names
    private string beatmapCreator, beatmapCreatedDate;
    private string disabledLevelTextValue;

    // Integers
    private int selectedBeatmapDirectoryIndex, previousBeatmapDirectoryIndex; // Beatmap directory indexs
    private float songPreviewStartTime;
    private float beatmapSongOffset, beatmapSongBpm;
    private int songClipChosenIndex;
    private int frameInterval;
    private int beatmapDirectoryCount;

    // Bools
    private bool easyDifficultyExist, advancedDifficultyExist, extraDifficultyExist;
    private bool hasPlayedSongPreviewOnce; // Used to play the start preview once upon entering the song select screen for the first time so the song plays at the current set time once.
    private bool hasSelectedCurrentBeatmapButton;
    public AudioSource songAudioSource;

    // Scripts
    private ScriptManager scriptManager;

    // Properties

    // Get the selectedBeatmapDirectoryIndex
    public int SelectedBeatmapDirectoryIndex
    {
        get { return selectedBeatmapDirectoryIndex; }
        set { selectedBeatmapDirectoryIndex = value; }
    }

    public string EasyBeatmapFileName
    {
        get { return easyBeatmapFileName; }
    }

    public string AdvancedBeatmapFileName
    {
        get { return advancedBeatmapFileName; }
    }

    public string ExtraBeatmapFileName
    {
        get { return extraBeatmapFileName; }
    }

    public bool HasPlayedSongPreviewOnce
    {
        set { hasPlayedSongPreviewOnce = value; }
    }

    public string CurrentBeatmapDifficulty
    {
        get { return currentBeatmapDifficulty; }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        hasPlayedSongPreviewOnce = false;
        songClipChosenIndex = 0;
        easyBeatmapFileName = "easy.dia";
        advancedBeatmapFileName = "advanced.dia";
        extraBeatmapFileName = "extra.dia";
        easyDifficultyName = "easy";
        advancedDifficultyName = "advanced";
        extraDifficultyName = "extra";
        disabledLevelTextValue = "X";

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        CheckBeatmapDirectories(); // Get and check the beatmaps in the directory

        // Outside script functions
        scriptManager.songSelectPanel.GetBeatmapDirectoryPaths(); // Get the directory paths for all the beatmap folders in the beatmap directory

        // Property initalize
        selectedBeatmapDirectoryIndex = scriptManager.loadLastBeatmapManager.LastBeatmapDirectoryIndex; // Load the last beatmap directory index
        previousBeatmapDirectoryIndex = selectedBeatmapDirectoryIndex; // Assign the previous beatmap index to the current beatmap index
        currentBeatmapDifficulty = scriptManager.loadLastBeatmapManager.LastBeatmapDifficulty; // Check if the last selected difficulty was set or not (first time entering game or not)

        // Load the beatmap if it exists
        LoadBeatmapFileThatExists(selectedBeatmapDirectoryIndex);

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

        // Set total beatmap count text
        scriptManager.songSelectPanel.totalBeatmapCountText.text = "/ " + beatmapDirectoryCount.ToString();
        
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

    private void DisableSelectedDifficultyButtonVisuals()
    {
        defaultEasyNameText.gameObject.SetActive(false);
        defaultAdvancedNameText.gameObject.SetActive(false);
        defaultExtraNameText.gameObject.SetActive(false);

        selectedEasyNameText.gameObject.SetActive(false);
        selectedAdvancedNameText.gameObject.SetActive(false);
        selectedExtraNameText.gameObject.SetActive(false);

        easySelectedGameobject.gameObject.SetActive(false);
        advancedSelectedGameobject.gameObject.SetActive(false);
        extraSelectedGameobject.gameObject.SetActive(false);
    }

    // Load beatmap song select information
    public void LoadBeatmapSongSelectInformation(int _selectedBeatmapDirectoryIndex, string _beatmapDifficulty)
    {
        DisableSelectedDifficultyButtonVisuals();

        // Update the personal best leaderboard button color to the difficulty currently selected
        switch (_beatmapDifficulty)
        {
            case "easy":
                selectedEasyNameText.gameObject.SetActive(true);
                easySelectedGameobject.gameObject.SetActive(true);
                topColorPanelImage.color = easyDifficultyButtonColor;
                bottomColorPanelImage.color = easyDifficultyButtonColor;
                beatmapCreatorMessageText.color = easyDifficultyButtonColor;

                // Update highlighted colors for UI buttons
                scriptManager.uiColorManager.difficultyColor = easyDifficultyButtonColor;

                // Order the button so that it appears ontop
                difficultyOptionEasyButton.transform.SetAsLastSibling();

                // Activate the other button text
                defaultAdvancedNameText.gameObject.SetActive(true);
                defaultExtraNameText.gameObject.SetActive(true);

                //FlashImage("SongSelectMenuFlashEasy");
                break;
            case "advanced":
                selectedAdvancedNameText.gameObject.SetActive(true);
                advancedSelectedGameobject.gameObject.SetActive(true);
                topColorPanelImage.color = advancedDifficultyButtonColor;
                bottomColorPanelImage.color = advancedDifficultyButtonColor;
                beatmapCreatorMessageText.color = advancedDifficultyButtonColor;

                // Update highlighted colors for UI buttons
                scriptManager.uiColorManager.difficultyColor = advancedDifficultyButtonColor;

                // Order the button so that it appears ontop
                difficultyOptionAdvancedButton.transform.SetAsLastSibling();

                // Activate the other button text
                defaultEasyNameText.gameObject.SetActive(true);
                defaultExtraNameText.gameObject.SetActive(true);

                //FlashImage("SongSelectMenuFlashAdvanced");
                break;
            case "extra":
                //FlashImage("SongSelectMenuFlashExtra");
                selectedExtraNameText.gameObject.SetActive(true);
                extraSelectedGameobject.gameObject.SetActive(true);
                topColorPanelImage.color = extraDifficultyButtonColor;
                bottomColorPanelImage.color = extraDifficultyButtonColor;
                beatmapCreatorMessageText.color = extraDifficultyButtonColor;

                // Update highlighted colors for UI buttons
                scriptManager.uiColorManager.difficultyColor = extraDifficultyButtonColor;

                // Order the button so that it appears ontop
                difficultyOptionExtraButton.transform.SetAsLastSibling();

                // Activate the other button text
                defaultEasyNameText.gameObject.SetActive(true);
                defaultAdvancedNameText.gameObject.SetActive(true);
                break;
            default:
                //FlashImage("SongSelectMenuFlash");
                break;
        }

        // Assign new UI colors
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.songSelectPanel.difficultySortingDropDown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.songSelectPanel.sortingDropDown);
        scriptManager.uiColorManager.UpdateDropDownColors(scriptManager.beatmapRanking.leaderboardSortDropDown);
        scriptManager.uiColorManager.UpdateGradientButtonColors(scriptManager.playerSkillsManager.songSelectCharacterButton);
        scriptManager.uiColorManager.UpdateTickBoxButtonColors(scriptManager.backgroundManager.videoTickBoxButton);
        scriptManager.uiColorManager.UpdateScrollbarColors(scriptManager.beatmapRanking.leaderboardScrollbar);
        scriptManager.uiColorManager.UpdateScrollbarColors(scriptManager.songSelectPanel.beatmapButtonListScrollbar);

        for (int i = 0; i < scriptManager.beatmapRanking.leaderboardButton.Length; i++)
        {
            scriptManager.uiColorManager.UpdateGradientButtonColors(scriptManager.beatmapRanking.leaderboardButton[i]);
        }

        scriptManager.uiColorManager.UpdateGradientButtonColors(scriptManager.beatmapRanking.personalBestButton);


        // If any difficulty files exist
        if (advancedDifficultyExist == true || extraDifficultyExist == true || easyDifficultyExist == true)
        {
            // LOAD BEATMAP INFORMATION - IMAGES / NOTES / SCREEN CHANGES

            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(beatmapDirectories[_selectedBeatmapDirectoryIndex], _beatmapDifficulty);
            // Load the song select UI variables from the database
            beatmapSongName = Database.database.LoadedSongName;
            beatmapCreator = Database.database.LoadedBeatmapCreator;
            beatmapCreatedDate = Database.database.LoadedBeatmapCreatedDate;
            beatmapSongArtist = Database.database.LoadedSongArtist;
            songPreviewStartTime = Database.database.LoadedSongPreviewStartTime;
            beatmapSongBpm = Database.database.LoadedBPM;
            beatmapSongOffset = Database.database.LoadedOffsetMS;
            songClipChosenIndex = Database.database.LoadedSongClipChosenIndex;

            // Load beatmap creator profile image
            scriptManager.uploadPlayerImage.CallBeatmapCreatorUploadImage(beatmapCreator);

            // Play animation
            songInformationPanelAnimator.Play("SongInformationPanel_Animation", 0, 0f);

            // If video tick box is enabled load a video if it exists
            if (scriptManager.backgroundManager.VideoTickBoxSelected == true)
            {
                // Load video or background image based on url
                scriptManager.backgroundManager.LoadVideoOrImage(beatmapDirectories[_selectedBeatmapDirectoryIndex]);

                // Play the background video
                PlayVideo();
            }
            else
            {
                // Load only the background image
                scriptManager.backgroundManager.LoadImageOnly(beatmapDirectories[_selectedBeatmapDirectoryIndex]);
            }


            // Check if the first song preview when entering the song select menu has played once, if it hasn't play the song preview at the correct set time
            if (hasPlayedSongPreviewOnce == false)
            {
                // Play the song preview for the first song when entering at the correct set time in the beatmap information
                scriptManager.songSelectPreview.GetBeatmapAudio(beatmapDirectories[_selectedBeatmapDirectoryIndex], songPreviewStartTime);

                // Reset, update and play the metronome effects for the song select scene
                scriptManager.metronomeForEffects.GetSongData(beatmapSongBpm, beatmapSongOffset);
                scriptManager.metronomeForEffects.CalculateIntervals();
                scriptManager.metronomeForEffects.CalculateActualStep();
                scriptManager.metronomeForEffects.CalculateCurrentTick();

                // Set to true
                hasPlayedSongPreviewOnce = true;
            }

            // Change the current song selected text to the information loaded from the current directory
            songTitleText.text = beatmapSongName; 
            songArtistText.text = beatmapSongArtist;

            // Update the other beatmap information text
            string totalObjects = Database.database.loadedPositionX.Count.ToString();

            beatmapInformationText.text = beatmapCreatedDate + " | " + totalObjects + " OBJECTS | " + 
                UtilityMethods.FromSecondsToMinutesAndSeconds(songAudioSource.clip.length);

            beatmapCreatorText.text = "DESIGNED BY " + beatmapCreator.ToUpper();
            //beatmapCreatorMessageText.text = ;

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
            easyDifficultyLevel = Database.database.LoadedBeatmapEasyDifficultyLevel;
            // Update the easy beatmap difficulty level text
            difficultyButtonEasyText.text = easyDifficultyLevel;
        }

        // If the advanced beatmap difficulty file exists
        if (advancedDifficultyExist == true)
        {
            // Load the advanced beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[_selectedDirectoryIndex], advancedDifficultyName);
            // Assign the advanced beatmap difficulty value
            advancedDifficultyLevel = Database.database.LoadedBeatmapAdvancedDifficultyLevel;
            // Update the advanced beatmap difficulty level text
            difficultyButtonAdvancedText.text = advancedDifficultyLevel;
        }

        // If the extra beatmap difficulty file exists
        if (extraDifficultyExist == true)
        {
            // Load the extra beatmap difficulty level
            Database.database.LoadBeatmapDifficultyLevel(beatmapDirectories[_selectedDirectoryIndex], extraDifficultyName);
            // Assign the extra beatmap difficulty level
            extraDifficultyLevel = Database.database.LoadedBeatmapExtraDifficultyLevel;
            // Update the extra beatmap difficulty level text
            difficultyButtonExtraText.text = extraDifficultyLevel;
        }
    }

    // Animate the flash on screen for the difficulties: easy/advanced/extra
    public void FlashImage(string _animation)
    {
        songSelectFlashAnimator.Play(_animation);
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
            difficultyButtonEasyText.text = disabledLevelTextValue;
            // Set bool to not exist as the easy difficulty file does not exist
            easyDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the advanced difficulty file
    public void CheckIfAdvancedDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the advanced.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + advancedBeatmapFileName))
        {
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
            // Disable the button
            difficultyOptionAdvancedButton.interactable = false;
            // Disable the event trigger to prevent it trying to load the advanced file that doesn't exist
            advancedDifficultyButtonEventTrigger.enabled = false;
            // Print disabled level text as the beatmap difficulty level
            difficultyButtonAdvancedText.text = disabledLevelTextValue;
            // Set bool to not exist
            advancedDifficultyExist = false;
        }
    }

    // Check the beatmap directory for the extra difficulty file
    public void CheckIfExtraDifficultyExists(int _selectedBeatmapDirectoryIndex)
    {
        // If the extra.dia file exists
        if (File.Exists(beatmapDirectories[_selectedBeatmapDirectoryIndex] + @"\" + extraBeatmapFileName))
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
            difficultyButtonExtraText.text = disabledLevelTextValue;
            // Set bool to exist as the file does not exist
            extraDifficultyExist = false;
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
        CheckIfAdvancedDifficultyExists(_selectedBeatmapDirectoryIndex);
        CheckIfExtraDifficultyExists(_selectedBeatmapDirectoryIndex);

        // Check the difficulty files if they exist and update the levels for each of them
        UpdateDifficultyLevelText(_selectedBeatmapDirectoryIndex);

        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
        /*
        // Only load the beatmap file if 1 of the difficulties exist
        if (easyDifficultyExist || advancedDifficultyExist || extraDifficultyExist)
        {
            // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
            LoadSelectedDifficultyBeatmapFile(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
        }
        else
        {
            // No difficulties were found for that beatmap - load the previous beatmap in the list?
            AttemptToLoadPreviousBeatmap(_selectedBeatmapDirectoryIndex, _hasPressedArrowKey);
        }
        */
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

                case "advanced":
                    // If the advanced.dia difficulty file exists
                    if (advancedDifficultyExist == true)
                    {
                        // Load the song select information for advanced
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, advancedDifficultyName);
                    }
                    else
                    {
                        // Check which difficulty files exist and load the first one to be found when the arrow key has been pressed in song select
                        LoadFirstBeatmapFileThatExists(_selectedBeatmapDirectoryIndex);
                    }
                    break;

                case "extra":
                    // If the extra.dia file difficulty exists 
                    if (extraDifficultyExist == true)
                    {
                        // Load the song select information for extra
                        LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, extraDifficultyName);
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
        // If the advanced.dia difficulty file exists
        else if (advancedDifficultyExist == true)
        {
            // Load the song select information for advanced
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, advancedDifficultyName);
        }
        // If the extra.dia file difficulty exists 
        else if (extraDifficultyExist == true)
        {
            // Load the song select information for extra
            LoadBeatmapSongSelectInformation(_selectedBeatmapDirectoryIndex, extraDifficultyName);
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
}

