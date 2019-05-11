using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardCreate : MonoBehaviour {

    public string leaderboardTableName;
    public BeatmapSetup beatmapSetup;
    string difficultySelected;

    void Start()
    {
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
    }

    // Create a leaderboard for the beatmap
    public void CreateLeaderboard()
    {
        StartCoroutine(CreateNewBeatmapLeaderboard());
    }

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
            Debug.Log("Leaderboard Created");
        }
        // Error
        if (www.downloadHandler.text == "1")
        {
            Debug.Log("Error");
        }

    }

    // Get the beatmap difficulty selected from the buttons easy/advanced/extra, which is used for the leaderbaord table name
    public void GetBeatmapDifficultySelected(string difficultySelectedPass)
    {
        difficultySelected = difficultySelectedPass.ToUpper();
    }

    // Create a leaderboard table for this beatmap
    public void SetLeaderboardTableName()
    {
        // Get the name of the user currently logged in
        string beatmapCreator = MySQLDBManager.username;

        // Get the name of the beatmap song being charted
        string beatmapSong = beatmapSetup.songName;

        //string beatmapSong = "BLUEDRAGON";
        //string beatmapCreator = "Ashley";
        // string beatmapDifficulty = advanced
        // Combine all together to create a unique leaderboard table name
        leaderboardTableName = beatmapCreator + beatmapSong + difficultySelected;

        // Save in the database
        Database.database.leaderboardTableName = leaderboardTableName;
    }
}
