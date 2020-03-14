using UnityEngine;

public class SongSelectMenuFlash : MonoBehaviour
{
    #region Variables
    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    private void Start()
    {
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

    // Select the difficulty, update and flash
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
                    // Load difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex, 
                        scriptManager.songSelectManager.EasyDifficultyName);
                    break;
                case "normal":
                    // Load difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex,
                        scriptManager.songSelectManager.NormalDifficultyName);
                    break;
                case "hard":
                    // Load difficulty information and beatmap file from database
                    scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex,
                        scriptManager.songSelectManager.HardDifficultyName);
                    break;
            }
        }
    }

    // Select the Hard difficulty, update and flash
    public void LoadBeatmapHardDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex,
                scriptManager.songSelectManager.HardDifficultyName);
        }
    }

    // Select the Normal difficulty, update and flash
    public void LoadBeatmapNormalDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex,
                scriptManager.songSelectManager.NormalDifficultyName);
        }
    }

    // Select the Easy difficulty, update and flash
    public void LoadBeatmapEasyDifficulty()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Load easy difficulty information and beatmap file from database
            scriptManager.songSelectManager.LoadBeatmapSongSelectInformation(scriptManager.songSelectManager.SelectedBeatmapDirectoryIndex,
                scriptManager.songSelectManager.EasyDifficultyName);
        }
    }

    // Clear all beatmap information from the difficulty file
    public void ClearBeatmapLoaded()
    {
        Database.database.Clear();

        scriptManager.songSelectManager.ResetCurrentSelectedBeatmapButton();
    }
    #endregion
}

