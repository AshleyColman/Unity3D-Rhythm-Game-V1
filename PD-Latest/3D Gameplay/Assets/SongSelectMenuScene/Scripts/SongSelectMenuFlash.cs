using UnityEngine;

public class SongSelectMenuFlash : MonoBehaviour {

    // Bools
    public bool hasPressedArrowKey; // Passed in the loading function to change the preview song each load but not for when hovering over the difficulties

    // Strings
    private string easyBeatmapDifficulty, defaultBeatmapDifficulty, extraBeatmapDifficulty;
    private string keyPressed; // The key pressed - right or left

    // Scripts
    private SongSelectManager songSelectManager; // Song select manager for loading beatmaps                                     
    private BeatmapRanking beatmapRanking; // Loads beatmap leaderboard information
    private PlayerProfile playerProfile; // Loads player profile information

    void Start () {

        // Initialize 
        hasPressedArrowKey = false;
        easyBeatmapDifficulty = "easy";
        defaultBeatmapDifficulty = "advanced";
        extraBeatmapDifficulty = "extra";

        // Reference
        songSelectManager = FindObjectOfType<SongSelectManager>();
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
        playerProfile = FindObjectOfType<PlayerProfile>();
	}
	
	// Update is called once per frame
	void Update () {

        // Check for keyboard arrow input
        CheckArrowKeyboardInput();
    }

    // Check for keyboard arrow input
    private void CheckArrowKeyboardInput()
    {
        // Right arrow key
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Set key pressed to right
            keyPressed = "RIGHT";
            // Stop beatmap leaderboard ranking loading functions
            beatmapRanking.StopAllCoroutines();
            playerProfile.StopAllCoroutines();
            // Load next beatmap in the directory
            LoadBeatmap(keyPressed);
        }
        // Left arrow key
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Set key pressed to left
            keyPressed = "LEFT";
            // Stop beatmap leaderboard ranking loading functions
            beatmapRanking.StopAllCoroutines();
            playerProfile.StopAllCoroutines();
            // Load the previous beatmap in the directory
            LoadBeatmap(keyPressed);
        }
    }

    // Load beatmap
    public void LoadBeatmap(string _keyPressed)
    {
        // Stop all coroutines
        beatmapRanking.StopAllCoroutines();
        playerProfile.StopAllCoroutines();

        // Set to true as an arrow key has been pressed
        hasPressedArrowKey = true;

        // Clear all beatmap information
        ClearBeatmapLoaded();

        // Disable the keys requried UI for the beatmap
        songSelectManager.DisableKeysRequiredForBeatmap();

        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();

        // Load next or previous beatmap based on the key pressed
        if (_keyPressed == "RIGHT")
        {
            // LOAD NEXT BEATMAP
            // Increment the beatmap index to load the next beatmap
            songSelectManager.IncrementSelectedBeatmapDirectoryIndex();
        }
        else if (_keyPressed == "LEFT")
        {
            // LOAD PREVIOUS BEATMAP
            // Decrement the beatmap selected index to load the previous song
            songSelectManager.DecrementSelectedBeatmapDirectoryIndex();
        }
        else if (_keyPressed == "RANDOM")
        {
            // LOAD RANDOM BEATMAP
            songSelectManager.RandomSelectedBeatmapDirectoryIndex();
        }
        // Load the beatmap file that exists
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.SelectedBeatmapDirectoryIndex, hasPressedArrowKey);

        // Reset the arrow key being pressed
        hasPressedArrowKey = false;

        // Reset the leaderboard checking variables
        beatmapRanking.ResetNotChecked();

        // Get leaderboard table name 
       // beatmapRanking.GetLeaderboardTableName();
    }

    // Loads the difficulty leaderbaord only and resets the old one. Only does the leaderbaord not load the entire database file
    public void LoadDifficultyLeaderboardOnly()
    {
        // Load the beatmap rankings
        beatmapRanking.StopAllCoroutines();
        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();
        // Reset the leaderboard checking variables
        beatmapRanking.ResetNotChecked();

        // Stop profile loading
        playerProfile.StopAllCoroutines();

        // Get leaderboard table name 
        //beatmapRanking.GetLeaderboardTableName();
    }

    // Load the beatmap which the song select menu beatmap button has assigned to it (BUTTON FUNCTION)
    public void LoadBeatmapButtonSong(int _beatmapToLoadIndex)
    {
        // Stop all coroutines
        beatmapRanking.StopAllCoroutines();
        playerProfile.StopAllCoroutines();

        // Clear all loaded beatmaps
        ClearBeatmapLoaded();

        // Disable the keys required for the beatmap
        songSelectManager.DisableKeysRequiredForBeatmap();

        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();

        // Has pressed arrow key
        hasPressedArrowKey = true;

        // Assign the beatmap to load index to the beatmap to load index pass from the button clicked in the song select scene
        songSelectManager.SelectedBeatmapDirectoryIndex = _beatmapToLoadIndex;

        // Load the beatmap if the file exists
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.SelectedBeatmapDirectoryIndex, hasPressedArrowKey);

        // Set back to false
        hasPressedArrowKey = false;

        // Reset the leaderboard checking variables
        beatmapRanking.ResetNotChecked();
    }

    // Select the Extra difficulty, update and flash
    public void LoadBeatmapExtraDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, extraBeatmapDifficulty, hasPressedArrowKey);
    }

    // Select the Advanced difficulty, update and flash
    public void LoadBeatmapAdvancedDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
    }

    // Select the Easy difficulty, update and flash
    public void LoadBeatmapEasyDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, easyBeatmapDifficulty, hasPressedArrowKey);
    }

    // Clear all beatmap information from the difficulty file
    public void ClearBeatmapLoaded()
    {
        Database.database.Clear();
    }
}
