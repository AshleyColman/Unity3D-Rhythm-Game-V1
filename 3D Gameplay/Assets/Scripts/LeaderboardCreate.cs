using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardCreate : MonoBehaviour {

    public string leaderboardTableName;
    public BeatmapSetup beatmapSetup;

    void Start()
    {
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
    }

    void Update()
    {
        // If the q key is pressed create a leaderboard for this beatmap
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("qpressed");
            StartCoroutine(CreateNewBeatmapLeaderboard());
        }
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


    // Create a leaderboard table for this beatmap
    public void SetLeaderboardTableName()
    {
        // Get the name of the user currently logged in
        string beatmapCreator = MySQLDBManager.username;

        // Get the name of the beatmap song being charted
        string beatmapSong = beatmapSetup.songName;

        //string beatmapSong = "BLUEDRAGON";
        //string beatmapCreator = "Ashley";
        // Combine both together to create a unique leaderboard table name
        leaderboardTableName = beatmapCreator + beatmapSong;

        // Save in the database
        Database.database.leaderboardTableName = leaderboardTableName;
    }
}
