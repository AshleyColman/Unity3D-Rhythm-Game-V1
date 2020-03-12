using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardCreate : MonoBehaviour
{
    // Strings
    private string leaderboardTableName;
    private string beatmapCreator;

    // Scripts
    private ScriptManager scriptManager;

    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Create a leaderboard for the beatmap
    public void CreateLeaderboard()
    {
        // Create a new leaderbaord in the database
        StartCoroutine(CreateNewBeatmapLeaderboard());
    }

    // Create new leaderboard in the database 
    IEnumerator CreateNewBeatmapLeaderboard()
    {
        SetLeaderboardTableName();

        WWWForm form = new WWWForm();
        form.AddField("leaderboardTableName", leaderboardTableName);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/create_beatmap_leaderboard.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        if (www.downloadHandler.text == "0")
        {
            // LEADERBOARD CREATED
            Debug.Log("Leaderboard created");
        }
        if (www.downloadHandler.text == "1")
        {
            // ERROR
            Debug.Log("Leaderboard failed");
        }
    }

    // Create a leaderboard table for this beatmap
    public void SetLeaderboardTableName()
    {
        if (MySQLDBManager.loggedIn)
        {
            // Get the name of the user currently logged in
            beatmapCreator = MySQLDBManager.username.Replace(' ', '_');
        }
        else
        {
            beatmapCreator = "GUEST";
        }

        // Get the name of the beatmap song being charted
        string beatmapSong = scriptManager.setupBeatmap.SongName.Replace(' ', '_');
        string beatmapArtist = scriptManager.setupBeatmap.ArtistName.Replace(' ', '_');

        // Combine all together to create a unique leaderboard table name
        leaderboardTableName = beatmapCreator + "_" + beatmapSong + "_" + beatmapArtist + "_ " + scriptManager.setupBeatmap.BeatmapDifficulty;

        // Save in the database
        Database.database.LeaderboardTableName = leaderboardTableName;
    }
}
