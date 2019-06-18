using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelectMenuFlash : MonoBehaviour {

    public SongSelectManager songSelectManager; // Reference to the song select manager
    public string easyBeatmapDifficulty; // Advanced is default
    public string defaultBeatmapDifficulty; // Advanced is default
    public string extraBeatmapDifficulty; // Extra difficulty
    public string lastSelectedDifficulty; // The last selected difficulty on the current beatmap, so if extra was last selected allow the flash for advanced
    public bool hasPressedArrowKey; // Passed in the loading function to change the preview song each load but not for when hovering over the difficulties

    // Used for loading the beatmap leaderboard information
    private BeatmapRanking beatmapRanking;

    // Use this for initialization
    void Start () {

        hasPressedArrowKey = false;
        // Set the easy beatmap difficulty to easy
        easyBeatmapDifficulty = "easy";
        // Set the default beatmap difficulty to advanced
        defaultBeatmapDifficulty = "advanced";
        // Set the extra beatmap difficulty to extra
        extraBeatmapDifficulty = "extra";

        // Get the reference
        songSelectManager = FindObjectOfType<SongSelectManager>();

        // Get the reference
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Stop beatmap leaderboard ranking loads
            beatmapRanking.StopAllCoroutines();
            // Load next song
            LoadNextSong();

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Stop beatmap leaderboard ranking loads
            beatmapRanking.StopAllCoroutines();
            // Load the previous song
            LoadPreviousSong();
        }

    }

    // Load the next song
    public void LoadNextSong()
    {
        // Clear all loaded beatmaps
        ClearBeatmapLoaded();
        // Disable the keys required for the beatmap
        songSelectManager.DisableKeysRequiredForBeatmap();
        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();
        // Has pressed arrow key
        hasPressedArrowKey = true;
        // Load the next beatmap in the song select menu
        // Increase the current index by 1 so we go to the next song
        songSelectManager.selectedDirectoryIndex++;
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.selectedDirectoryIndex, hasPressedArrowKey);
        // Set back to false
        hasPressedArrowKey = false;
        // Load the beatmap rankings
        beatmapRanking.leaderboardPlaceToGet = 1;
        beatmapRanking.ResetNotChecked();
    }

    // Loads the difficulty leaderbaord only and resets the old one. Only does the leaderbaord not load the entire database file
    public void LoadDifficultyLeaderboardOnly()
    {
        // Load the beatmap rankings
        beatmapRanking.StopAllCoroutines();
        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();
        beatmapRanking.leaderboardPlaceToGet = 1;
        beatmapRanking.ResetNotChecked();
    }

    // Load the next song
    public void LoadBeatmapButtonSong(int beatmapToLoadIndexPass)
    {
        // Clear all loaded beatmaps
        ClearBeatmapLoaded();
        // Disable the keys required for the beatmap
        songSelectManager.DisableKeysRequiredForBeatmap();
        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();
        // Has pressed arrow key
        hasPressedArrowKey = true;
        // Load the next beatmap in the song select menu
        // Assign the beatmap to load index to the beatmap to load index pass from the button clicked in the song select scene
        songSelectManager.selectedDirectoryIndex = beatmapToLoadIndexPass;
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.selectedDirectoryIndex, hasPressedArrowKey);
        // Set back to false
        hasPressedArrowKey = false;
        // Load the beatmap rankings
        beatmapRanking.leaderboardPlaceToGet = 1;
        beatmapRanking.ResetNotChecked();
    }

    // Load the previous song
    public void LoadPreviousSong()
    {
        // Clear all loaded beatmaps
        ClearBeatmapLoaded();
        // Disable the keys required for the beatmap
        songSelectManager.DisableKeysRequiredForBeatmap();
        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();
        // Has pressed arrow key
        hasPressedArrowKey = true;
        // Load the next beatmap in the song select menu
        // Decrease the current index by 1 so we go to the next song
        songSelectManager.selectedDirectoryIndex--;
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.selectedDirectoryIndex, hasPressedArrowKey);
        // Set back to false
        hasPressedArrowKey = false;
        // Load the beatmap rankings
        beatmapRanking.leaderboardPlaceToGet = 1;
        beatmapRanking.ResetNotChecked();
    }
    
    // Select the Extra difficulty, update and flash
    public void LoadBeatmapExtraDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, extraBeatmapDifficulty, hasPressedArrowKey);
        // Set the last selected difficulty to extra
        lastSelectedDifficulty = extraBeatmapDifficulty;
    }

    // Select the Advanced difficulty, update and flash
    public void LoadBeatmapAdvancedDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
        // Set the last selected difficulty to Advanced
        lastSelectedDifficulty = defaultBeatmapDifficulty;
    }

    // Select the Easy difficulty, update and flash
    public void LoadBeatmapEasyDifficulty()
    {
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, easyBeatmapDifficulty, hasPressedArrowKey);
        // Set the last selected difficulty to Advanced
        lastSelectedDifficulty = easyBeatmapDifficulty;
    }




    // Clear all loaded beatmap currently in song select
    public void ClearBeatmapLoaded()
    {
        Database.database.Clear();
    }
}
