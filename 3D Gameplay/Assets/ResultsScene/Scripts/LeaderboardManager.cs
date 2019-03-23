using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour {

    public string leaderboardTableName; // The beatmap table to upload scores to
    public bool notChecked;
    public GameplayToResultsManager gameplayToResultsManager; // Reference required to get the users play data from to upload to the leaderboard
    public string username; // The username of the current user signed in

    void Start()
    {
        // For checking if scores have uploaded or attempted once
        notChecked = true;
        // Get the reference
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();
    }

    void Update()
    {
        if (notChecked == true)
        {
            StartCoroutine(UploadUserScore());
            notChecked = false;
        }
    }

    IEnumerator UploadUserScore()
    {
        // Set the username to upload as the current user logged in
        if (MySQLDBManager.loggedIn)
        {
            username = MySQLDBManager.username;
        }
        else
        {
            username = "Guest";
        }


        GetLeaderboardTableName();

        WWWForm form = new WWWForm();
        // Get all the data from the beatmap played and user information, submit to the php file that uploads to the leaderboard
        form.AddField("leaderboardTableName", leaderboardTableName);
        form.AddField("score", gameplayToResultsManager.score);
        form.AddField("perfect", gameplayToResultsManager.totalPerfect);
        form.AddField("good", gameplayToResultsManager.totalGood);
        form.AddField("early", gameplayToResultsManager.totalEarly);
        form.AddField("miss", gameplayToResultsManager.totalMiss);
        form.AddField("combo", gameplayToResultsManager.highestCombo);
        form.AddField("player_id", username);
        form.AddField("grade", gameplayToResultsManager.gradeAchieved);
        form.AddField("percentage", gameplayToResultsManager.Percentage.ToString());

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/uploaduserscoretobeatmap.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Success
        if (www.downloadHandler.text == "0")
        {
            Debug.Log("User score uploaded");
        }
        // Error
        if (www.downloadHandler.text == "1")
        {
            Debug.Log("Error");
        }

    }


    // Get leaderboard table for this beatmap
    public void GetLeaderboardTableName()
    {
        leaderboardTableName = Database.database.loadedLeaderboardTableName;
    }
}
