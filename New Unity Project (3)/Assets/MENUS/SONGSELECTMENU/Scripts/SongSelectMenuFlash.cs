using UnityEngine;
using UnityEngine.UI;

public class SongSelectMenuFlash : MonoBehaviour
{
    // Strings
    private string easyBeatmapDifficulty, advancedBeatmapDifficulty, extraBeatmapDifficulty;

    // Scripts
    private ScriptManager scriptManager;


    void Start()
    {

        // Initialize 
        easyBeatmapDifficulty = "easy";
        advancedBeatmapDifficulty = "advanced";
        extraBeatmapDifficulty = "extra";

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Load beatmap
    public void LoadBeatmap()
    {
        // Stop all coroutines
        scriptManager.beatmapRanking.StopAllCoroutines();
        scriptManager.playerProfile.StopAllCoroutines();

        // Clear all beatmap information
        ClearBeatmapLoaded();

        // Reset leaderboard rankings
        scriptManager.beatmapRanking.ResetLeaderboard();

        // Set to false so new song preview plays
        scriptManager.songSelectManager.HasPlayedSongPreviewOnce = true;

        // Load the beatmap file that exists
        scriptManager.songSelectManager.LoadBeatmapFileThatExists(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex);

        // Reset the leaderboard checking variables
        scriptManager.beatmapRanking.ResetNotChecked();

        // Get leaderboard table name 
        scriptManager.beatmapRanking.GetLeaderboardTableName();
    }

    // Loads the difficulty leaderbaord only and resets the old one. Only does the leaderbaord not load the entire database file
    public void LoadDifficultyLeaderboardOnly()
    {
        // Load the beatmap rankings
        scriptManager.beatmapRanking.StopAllCoroutines();

        // Reset leaderboard rankings
        scriptManager.beatmapRanking.ResetLeaderboard();

        // Reset the leaderboard checking variables
        scriptManager.beatmapRanking.ResetNotChecked();

        // Stop profile loading
        scriptManager.playerProfile.StopAllCoroutines();

        // Get leaderboard table name 
        scriptManager.beatmapRanking.GetLeaderboardTableName();
    }

    // Load the beatmap which the song select menu beatmap button has assigned to it (BUTTON FUNCTION)
    public void LoadBeatmapButtonSong(int _beatmapToLoadIndex)
    {
        // Stop all coroutines
        scriptManager.beatmapRanking.StopAllCoroutines();
        scriptManager.playerProfile.StopAllCoroutines();

        // Clear all loaded beatmaps
        ClearBeatmapLoaded();

        // Reset leaderboard rankings
        scriptManager.beatmapRanking.ResetLeaderboard();

        // Assign the beatmap to load index to the beatmap to load index pass from the button clicked in the song select scene
        scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex = _beatmapToLoadIndex;

        // Set to false so new song preview plays
        scriptManager.songSelectManager.HasPlayedSongPreviewOnce = false;

        // Load the beatmap if the file exists
        scriptManager.songSelectManager.LoadBeatmapFileThatExists(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex);

        // Reset the leaderboard checking variables
        scriptManager.beatmapRanking.ResetNotChecked();
    }

    // Select the Extra difficulty, update and flash
    public void LoadBeatmapDifficulty(string _difficulty)
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Stop all coroutines
            scriptManager.beatmapRanking.StopAllCoroutines();
            scriptManager.playerProfile.StopAllCoroutines();

            // Clear all loaded beatmaps
            ClearBeatmapLoaded();

            // Reset leaderboard rankings
            scriptManager.beatmapRanking.ResetLeaderboard();

            // Reset the leaderboard checking variables
            scriptManager.beatmapRanking.ResetNotChecked();

            // Load the beatmap difficulty based on the _difficulty passed
            switch (_difficulty)
            {
                case "easy":
                    // Load extra difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, easyBeatmapDifficulty);
                    break;
                case "advanced":
                    // Load extra difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, advancedBeatmapDifficulty);
                    break;
                case "extra":
                    // Load extra difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, extraBeatmapDifficulty);
                    break;
            }
        }
    }

    // Select the Extra difficulty, update and flash
    public void LoadBeatmapExtraDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load advanced difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, extraBeatmapDifficulty);
        }
    }

    // Select the Advanced difficulty, update and flash
    public void LoadBeatmapAdvancedDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load advanced difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, advancedBeatmapDifficulty);
        }
    }

    // Select the Easy difficulty, update and flash
    public void LoadBeatmapEasyDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load easy difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, easyBeatmapDifficulty);
        }
    }

    // Clear all beatmap information from the difficulty file
    public void ClearBeatmapLoaded()
    {
        Database.database.Clear();

        scriptManager.songSelectManager.ResetCurrentSelectedBeatmapButton();
    }
}

