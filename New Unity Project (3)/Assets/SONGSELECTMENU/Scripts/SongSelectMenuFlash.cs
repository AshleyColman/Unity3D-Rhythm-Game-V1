using UnityEngine;
using UnityEngine.UI;

public class SongSelectMenuFlash : MonoBehaviour
{
    // Strings
    private string easyBeatmapDifficulty, advancedBeatmapDifficulty, extraBeatmapDifficulty;

    // Scripts
    private SongSelectManager songSelectManager; // Song select manager for loading beatmaps                                     
    private BeatmapRanking beatmapRanking; // Loads beatmap leaderboard information
    private PlayerProfile playerProfile; // Loads player profile information
    private EditSelectSceneSongSelectManager editSelectSceneSongSelectManager; // Edit select scene song select manager
    private MenuManager menuManager;


    void Start()
    {

        // Initialize 
        easyBeatmapDifficulty = "easy";
        advancedBeatmapDifficulty = "advanced";
        extraBeatmapDifficulty = "extra";

        // Reference
        songSelectManager = FindObjectOfType<SongSelectManager>();
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
        playerProfile = FindObjectOfType<PlayerProfile>();
        editSelectSceneSongSelectManager = FindObjectOfType<EditSelectSceneSongSelectManager>();
        menuManager = FindObjectOfType<MenuManager>();
    }

    // Load beatmap
    public void LoadBeatmap()
    {
        // Stop all coroutines
        beatmapRanking.StopAllCoroutines();
        playerProfile.StopAllCoroutines();

        // Clear all beatmap information
        ClearBeatmapLoaded();

        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();

        // Set to false so new song preview plays
        songSelectManager.HasPlayedSongPreviewOnce = true;

        // Load the beatmap file that exists
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.SelectedBeatmapDirectoryIndex);

        // Reset the leaderboard checking variables
        beatmapRanking.ResetNotChecked();

        // Get leaderboard table name 
        beatmapRanking.GetLeaderboardTableName();
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
        beatmapRanking.GetLeaderboardTableName();
    }

    // Load the beatmap which the song select menu beatmap button has assigned to it (BUTTON FUNCTION)
    public void LoadBeatmapButtonSong(int _beatmapToLoadIndex)
    {
        // Stop all coroutines
        beatmapRanking.StopAllCoroutines();
        playerProfile.StopAllCoroutines();

        // Clear all loaded beatmaps
        ClearBeatmapLoaded();

        // Reset leaderboard rankings
        beatmapRanking.ResetLeaderboard();

        // Assign the beatmap to load index to the beatmap to load index pass from the button clicked in the song select scene
        songSelectManager.SelectedBeatmapDirectoryIndex = _beatmapToLoadIndex;

        // Set to false so new song preview plays
        songSelectManager.HasPlayedSongPreviewOnce = false;

        // Load the beatmap if the file exists
        songSelectManager.LoadBeatmapFileThatExists(songSelectManager.SelectedBeatmapDirectoryIndex);

        // Reset the leaderboard checking variables
        beatmapRanking.ResetNotChecked();
    }

    // Load the beatmap which the song select menu beatmap button has assigned to it (BUTTON FUNCTION)
    public void LoadEditSelectSceneBeatmapButtonSong(int _beatmapToLoadIndex)
    {
        // Clear all loaded beatmaps
        ClearEditSelectSceneBeatmapLoaded();

        // Disable the keys required for the beatmap
        editSelectSceneSongSelectManager.DisableKeysRequiredForBeatmap();

        // Assign the beatmap to load index to the beatmap to load index pass from the button clicked in the song select scene
        editSelectSceneSongSelectManager.SelectedBeatmapDirectoryIndex = _beatmapToLoadIndex;

        // Load the beatmap if the file exists
        editSelectSceneSongSelectManager.LoadBeatmapFileThatExists(editSelectSceneSongSelectManager.SelectedBeatmapDirectoryIndex);
    }

    // Select the Extra difficulty, update and flash
    public void LoadBeatmapDifficulty(string _difficulty)
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Stop all coroutines
            beatmapRanking.StopAllCoroutines();
            playerProfile.StopAllCoroutines();

            // Clear all loaded beatmaps
            ClearBeatmapLoaded();

            // Reset leaderboard rankings
            beatmapRanking.ResetLeaderboard();

            // Reset the leaderboard checking variables
            beatmapRanking.ResetNotChecked();

            // Load the beatmap difficulty based on the _difficulty passed
            switch (_difficulty)
            {
                case "easy":
                    // Load extra difficulty information and beatmap file from database
                    songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, easyBeatmapDifficulty);
                    break;
                case "advanced":
                    // Load extra difficulty information and beatmap file from database
                    songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, advancedBeatmapDifficulty);
                    break;
                case "extra":
                    // Load extra difficulty information and beatmap file from database
                    songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, extraBeatmapDifficulty);
                    break;
            }
        }
    }

    // Select the Advanced difficulty, update and flash
    public void LoadBeatmapAdvancedDifficulty()
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load advanced difficulty information and beatmap file from database
            songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, advancedBeatmapDifficulty);
        }
        /*
        else if (levelChanger.CurrentLevelIndex == levelChanger.EditSelectSceneIndex)
        {
            // Load advanced difficulty information and beatmap file from database
            editSelectSceneSongSelectManager.LoadBeatmapSongSelectInformation(editSelectSceneSongSelectManager.SelectedBeatmapDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
        }
        */
    }

    // Select the Easy difficulty, update and flash
    public void LoadBeatmapEasyDifficulty()
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load easy difficulty information and beatmap file from database
            songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.SelectedBeatmapDirectoryIndex, easyBeatmapDifficulty);
        }

        /*
        else if (levelChanger.CurrentLevelIndex == levelChanger.EditSelectSceneIndex)
        {
            // Load easy difficulty information and beatmap file from database
            editSelectSceneSongSelectManager.LoadBeatmapSongSelectInformation(editSelectSceneSongSelectManager.SelectedBeatmapDirectoryIndex,
                easyBeatmapDifficulty, hasPressedArrowKey);
        }
        */
    }

    // Clear all beatmap information from the difficulty file
    public void ClearBeatmapLoaded()
    {
        Database.database.Clear();

        songSelectManager.ResetCurrentSelectedBeatmapButton();
    }

    // Clear all beatmap information from the difficulty file
    public void ClearEditSelectSceneBeatmapLoaded()
    {
        Database.database.Clear();

        editSelectSceneSongSelectManager.ResetCurrentSelectedBeatmapButton();
    }
}

