using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class OverallRankingManager : MonoBehaviour
{

    private bool notChecked;
    public bool hasPersonalBest = false;
    private List<string>[] placeLeaderboardData = new List<string>[50];
    private List<string> personalBestLeaderboardData = new List<string>();
    public string teststring;
    public int leaderboardPlaceToGet;
    public bool setFirst = false;

    // Leaderboard text
    public TextMeshProUGUI[] rankedButtonUsernameText = new TextMeshProUGUI[50];
    public TextMeshProUGUI[] rankedButtonScoreText = new TextMeshProUGUI[50];
    public TextMeshProUGUI personalBestButtonUsernameText;
    public TextMeshProUGUI personalBestButtonScoreText;

    // Button text variables
    string[] rankedButtonUsername = new string[50];
    string[] rankedButtonScore = new string[50];
    string personalBestScore;

    bool[] placeExists = new bool[50];

    bool hasCheckedPersonalBest = false;

    string player_id;

    public Image[] playerImage = new Image[5];

    int totalRankingPlacements;


    void Start()
    {
        totalRankingPlacements = 50;
        leaderboardPlaceToGet = 1;
        notChecked = true;


        // Instantiate the lists
        for (int i = 0; i < placeLeaderboardData.Length; i++)
        {
            placeLeaderboardData[i] = new List<string>();
        }

    }

    void Update()
    {

        if (notChecked == true && hasCheckedPersonalBest == false)
        {
            for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
            {
                // Retrieve top players
                StartCoroutine(RetrieveOverallRankingLeaderboard(leaderboardPlaceToGet));
                // Increment the placement to get
                leaderboardPlaceToGet++;
            }

            StartCoroutine(RetrievePersonalBest());

            notChecked = false;
        }

        if (notChecked == false && hasCheckedPersonalBest == true)
        {

            for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
            {
                if (placeExists[placementToCheck] == true)
                {
                    // Assign the database information to the variables
                    rankedButtonUsername[placementToCheck] = placeLeaderboardData[placementToCheck][0];
                    rankedButtonScore[placementToCheck] = placeLeaderboardData[placementToCheck][1];

                    // Update the text for the leaderboard button
                    rankedButtonUsernameText[placementToCheck].text = rankedButtonUsername[placementToCheck];
                    rankedButtonScoreText[placementToCheck].text = rankedButtonScore[placementToCheck];
                }
            }

            if (MySQLDBManager.loggedIn == true)
            {
                if (hasPersonalBest == true)
                {
                    personalBestScore = personalBestLeaderboardData[0];

                    personalBestButtonUsernameText.text = MySQLDBManager.username;
                    personalBestButtonScoreText.text = personalBestScore;
                }
            }
        }
    }

    public IEnumerator RetrieveOverallRankingLeaderboard(int leaderboardPlaceToGetPass)
    {

        WWWForm form = new WWWForm();
        form.AddField("leaderboardPlaceToGet", leaderboardPlaceToGetPass);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveoverallranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        teststring = www.downloadHandler.text;

        ArrayList placeList = new ArrayList();

        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        for (int dataType = 0; dataType < 2; dataType++)
        {
            /*
              DataType:
              [0] = username
              [1] = score
            */


            if (www.downloadHandler.text != "1")
            {
                placeLeaderboardData[leaderboardPlaceToGetPass - 1].Add(placeList[dataType].ToString());
                placeExists[leaderboardPlaceToGetPass - 1] = true;

            }
            else
            {
                placeExists[leaderboardPlaceToGetPass - 1] = false;
            }

        }

    }




    // Retrieve the users personal best score
    public IEnumerator RetrievePersonalBest()
    {

        if (MySQLDBManager.loggedIn)
        {
            player_id = MySQLDBManager.username;

            WWWForm form = new WWWForm();
            form.AddField("player_id", player_id);

            UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievepersonalbestoverallranking.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            teststring = www.downloadHandler.text;

            ArrayList placeList = new ArrayList();

            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

                // If it succeeded
                if (www.downloadHandler.text != "1")
                {
                    personalBestLeaderboardData.Add(placeList[0].ToString());
                    hasPersonalBest = true;
                    hasCheckedPersonalBest = true;
                }
                else
                {
                    // Personal best failed
                }
            hasCheckedPersonalBest = true;
        }
        else
        {
            hasPersonalBest = false;
            hasCheckedPersonalBest = true;
            yield return null;
        }

    }
}


