using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {

    // UI
    public TextMeshProUGUI uploadStatusText;
    public Button retryScoreUploadButton;

    // Strings
    private string leaderboardTableName; // The beatmap table to upload scores to
    private string username; // The username of the current user signed in
    private string failedToUploadScore; 
    private string uploadingScoreToGameServer;
    private string scoreSuccessfullyUploaded;
    private string guestUsername;

    // Integers
    private int newUserScoreToUpload;
    private int currentUserScore;

    // Bools
    private bool hasCheckedCurrentUserScore;
    private bool hasUpdatedUserOverallTotalScore;
    private bool hasIncrementedScore;
    private bool hasExistingScoreInOverallRankings;
    private bool notChecked;

    // Scripts
    public GameplayToResultsManager gameplayToResultsManager; // Reference required to get the users play data from to upload to the leaderboard


    // Properties

    public bool NotChecked
    {
        get { return notChecked; }
    }


    void Start()
    {
        // Initialize
        notChecked = true; // For checking if scores have uploaded or attempted once
        failedToUploadScore = "Failed to upload score";
        uploadingScoreToGameServer = "Uploading score to the game server...";
        scoreSuccessfullyUploaded = "Score successfully uploaded";
        guestUsername = "GUEST";

        // Reference
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();

        // Functions
        UpdateStatusTextUploading(); // Update the status text
    }

    void Update()
    {
        if (notChecked == true)
        {
            StartCoroutine(UploadUserScore());
            notChecked = false;
        }

        if (hasCheckedCurrentUserScore == false)
        {
            // Retrieve the user current total score from the database
            StartCoroutine(RetrieveUserCurrentTotalScore());
        }
        else
        {
            // If the user score has been retrieved
            if (hasUpdatedUserOverallTotalScore == false && hasCheckedCurrentUserScore == true)
            {
                if (hasExistingScoreInOverallRankings == true)
                {
                    if (hasIncrementedScore == false)
                    {
                        // Add the just played score to the score retrieved
                        newUserScoreToUpload = gameplayToResultsManager.Score + currentUserScore;
                        hasIncrementedScore = true;
                    }

                    StartCoroutine(UpdateOverallLeaderboardTotalScore());
                }
                else
                {
                    // Add the just played score to the score retrieved
                    newUserScoreToUpload = gameplayToResultsManager.Score;

                    StartCoroutine(UpdateOverallLeaderboardTotalScore());
                }
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
            username = "GUEST";
        }

        GetLeaderboardTableName();

        WWWForm form = new WWWForm();
        // Get all the data from the beatmap played and user information, submit to the php file that uploads to the leaderboard
        form.AddField("leaderboardTableName", leaderboardTableName);
        form.AddField("score", gameplayToResultsManager.Score);
        form.AddField("perfect", gameplayToResultsManager.TotalPerfect);
        form.AddField("good", gameplayToResultsManager.TotalGood);
        form.AddField("early", gameplayToResultsManager.TotalEarly);
        form.AddField("miss", gameplayToResultsManager.TotalMiss);
        form.AddField("combo", gameplayToResultsManager.HighestCombo);
        form.AddField("player_id", username);
        form.AddField("grade", gameplayToResultsManager.GradeAchieved);
        form.AddField("percentage", gameplayToResultsManager.Percentage);
        form.AddField("modused", gameplayToResultsManager.ModUsed);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/uploaduserscoretobeatmap.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Success
        if (www.downloadHandler.text == "0")
        {
            // Successful score upload
        }
        // Error
        if (www.downloadHandler.text == "1")
        {
            // Error - failed score upload
        }

    }

    // Get leaderboard table for this beatmap
    public void GetLeaderboardTableName()
    {
        leaderboardTableName = Database.database.LoadedLeaderboardTableName;
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
            username = guestUsername;
        }

        WWWForm form = new WWWForm();

        // Send the username
        form.AddField("player_id", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievepersonalbestoverallranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Check if the score retrieve was a success or failure
        if (www.downloadHandler.text != "0")
        {
            //Debug.Log("Retrieved overall ranking current total score");
            // Assign the retrieved score
            string currentUserScoreString = www.downloadHandler.text;
            currentUserScore = Convert.ToInt32(currentUserScoreString);
            // Set to true as we have retrieved the score
            hasCheckedCurrentUserScore = true;
            hasExistingScoreInOverallRankings = true;
        }
        // Error
        else
        {
            //Debug.Log("Error with retrieving user overall ranking current total score");
            hasCheckedCurrentUserScore = true;
            hasExistingScoreInOverallRankings = false;
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
            username = guestUsername;
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
            //Debug.Log("User score uploaded");
            hasUpdatedUserOverallTotalScore = true;

            // Update status text 
            UpdateStatusTextUploadSuccessful();
        }
        // Error
        if (www.downloadHandler.text == "0")
        {
            //Debug.Log("Error");
            hasUpdatedUserOverallTotalScore = true;

            // Update status text 
            UpdateStatusTextFailed();
        }
    }

    // Change text to failed to upload
    private void UpdateStatusTextFailed()
    {
        uploadStatusText.text = failedToUploadScore;

        // Set the retry button to active?
        retryScoreUploadButton.gameObject.SetActive(true);
    }

    // Change text to uploading
    private void UpdateStatusTextUploading()
    {
        uploadStatusText.text = uploadingScoreToGameServer;
    }

    // Change text to upload successful
    private void UpdateStatusTextUploadSuccessful()
    {
        uploadStatusText.text = scoreSuccessfullyUploaded;
    }

    // Retry uploading the score
    public void RetryUploadingScore()
    {
        // Reset all variables so allow checking to happen again
        leaderboardTableName = "";
        currentUserScore = 0;
        newUserScoreToUpload = 0;
        notChecked = true;
        hasCheckedCurrentUserScore = false;
        hasUpdatedUserOverallTotalScore = false;
        hasIncrementedScore = false;
        hasExistingScoreInOverallRankings = false;

        // Disable the retry button
        retryScoreUploadButton.gameObject.SetActive(false);
        // Update the text to trying to upload
        UpdateStatusTextUploading();
    }
}
