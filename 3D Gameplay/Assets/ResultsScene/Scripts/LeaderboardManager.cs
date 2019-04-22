using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour {

    public string leaderboardTableName; // The beatmap table to upload scores to
    public bool notChecked;
    public GameplayToResultsManager gameplayToResultsManager; // Reference required to get the users play data from to upload to the leaderboard
    public string username; // The username of the current user signed in

    private int currentUserScore;
    private bool hasRetrievedCurrentUserScore;
    private int newUserScoreToUpload;
    private bool hasUpdatedUserOverallTotalScore;
    private bool hasIncrementedScore;
    

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

        if (hasRetrievedCurrentUserScore == false)
        {
            // Retrieve the user current total score from the database
            StartCoroutine(RetrieveUserCurrentTotalScore());
        }
        else
        {
            // If the user score has been retrieved
            if (hasUpdatedUserOverallTotalScore == false)
            {
                if (hasIncrementedScore == false)
                {
                    // Add the just played score to the score retrieved
                    newUserScoreToUpload += gameplayToResultsManager.score;
                    hasIncrementedScore = true;
                }

                StartCoroutine(UpdateOverallLeaderboardTotalScore());
            }

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




    IEnumerator RetrieveUserCurrentTotalScore()
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


        WWWForm form = new WWWForm();

        // Send the username
        form.AddField("player_id", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievepersonalbestoverallranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Check if the score retrieve was a success or failure
        if (www.downloadHandler.text != "1")
        {
            Debug.Log("Retrieved overall ranking current total score");
            // Assign the retrieved score
            currentUserScore = int.Parse(www.downloadHandler.text);
            // Set to true as we have retrieved the score
            hasRetrievedCurrentUserScore = true;
        }
        // Error
        else
        {
            Debug.Log("Error with retrieving user overall ranking current total score");
            hasRetrievedCurrentUserScore = true;
        }

    }

    // Update the overall leaderboard total score for the player
    IEnumerator UpdateOverallLeaderboardTotalScore()
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


        WWWForm form = new WWWForm();

        // Send the username and new user score to upload
        form.AddField("player_id", username);
        form.AddField("new_user_score_to_upload", newUserScoreToUpload);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/updateuseroverallleaderboardtotalscore.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Success
        if (www.downloadHandler.text == "1")
        {
            Debug.Log("User score uploaded");
            hasUpdatedUserOverallTotalScore = true;
        }
        // Error
        if (www.downloadHandler.text == "0")
        {
            Debug.Log("Error");
            hasUpdatedUserOverallTotalScore = true;
        }

    }
}
