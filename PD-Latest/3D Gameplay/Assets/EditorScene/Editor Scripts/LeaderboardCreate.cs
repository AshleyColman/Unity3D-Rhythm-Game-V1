using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardCreate : MonoBehaviour {

    // Strings
    private string leaderboardTableName;
    private string difficultySelected;
    private string beatmapCreator;
    // Scripts
    private BeatmapSetup beatmapSetup;


    void Start()
    {
        // Reference
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
    }

    // Create a leaderboard for the beatmap
    public void CreateLeaderboard()
    {
        StartCoroutine(CreateNewBeatmapLeaderboard());
    }

    // Create new leaderboard in the database 
    IEnumerator CreateNewBeatmapLeaderboard()
    {
        SetLeaderboardTableName();

        WWWForm form = new WWWForm();
        form.AddField("leaderboardTableName", leaderboardTableName);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/createbeatmapleaderboard.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Success
        if (www.downloadHandler.text == "0")
        {
            // LEADERBOARD CREATED
        }
        // Error
        if (www.downloadHandler.text == "1")
        {
            // ERROR

            // Show a message saying leaderboard creation failed?
        }
    }

    // Get the beatmap difficulty selected from the buttons easy/advanced/extra, which is used for the leaderbaord table name
    public void GetBeatmapDifficultySelected(string _difficultySelected)
    {
        difficultySelected = _difficultySelected.ToUpper();
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
        string beatmapSong = beatmapSetup.SongName.Replace(' ', '_');

        // Combine all together to create a unique leaderboard table name
        leaderboardTableName = beatmapCreator + "_" + beatmapSong + "_" + difficultySelected;

        // Save in the database
        Database.database.LeaderboardTableName = leaderboardTableName;
    }
}
