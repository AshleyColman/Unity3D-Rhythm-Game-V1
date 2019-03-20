using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class BeatmapRankingManager : MonoBehaviour {


    public string leaderboardTableName;
    public List<string> firstPlaceLeaderboardData = new List<string>();
    public List<string> secondPlaceLeaderboardData = new List<string>();
    public List<string> thirdPlaceLeaderboardData = new List<string>();
    public List<string> fourthPlaceLeaderboardData = new List<string>();
    public List<string> fifthPlaceLeaderboardData = new List<string>();
    public string teststring;
    public int leaderboardPlaceToGet;
    public bool setFirst = false;

    // Leaderboard text
    public Text RankedButtonFirstText;
    public Text RankedButtonSecondText;
    public Text RankedButtonThirdText;
    public Text RankedButtonFourthText;
    public Text RankedButtonFifthText;

    // Button text variables
    string firstButtonUsername;
    string firstButtonScore;
    string secondButtonUsername;
    string secondButtonScore;
    string thirdButtonUsername;
    string thirdButtonScore;
    string fourthButtonUsername;
    string fourthButtonScore;
    string fifthButtonUsername;
    string fifthButtonScore;

    void Start()
    {
        leaderboardPlaceToGet = 1;
    }

    void Update()
    {

        if (notChecked == true)
        {
           for (int i = 0; i < 5; i++)
            {
                StartCoroutine(RetrieveBeatmapLeaderboard(leaderboardPlaceToGet));
                leaderboardPlaceToGet++;
            }

           notChecked = false;
        }

        if (notChecked == false)
        {
            firstButtonUsername = firstPlaceLeaderboardData[6];
            firstButtonScore = firstPlaceLeaderboardData[0];
            RankedButtonFirstText.text = firstButtonUsername + ": " + firstButtonScore;

            secondButtonUsername = secondPlaceLeaderboardData[6];
            secondButtonScore = secondPlaceLeaderboardData[0];
            RankedButtonSecondText.text = secondButtonUsername + ": " + secondButtonScore;

            thirdButtonUsername = thirdPlaceLeaderboardData[6];
            thirdButtonScore = thirdPlaceLeaderboardData[0];
            RankedButtonThirdText.text = thirdButtonUsername + ": " + thirdButtonScore;

            fourthButtonUsername = fourthPlaceLeaderboardData[6];
            fourthButtonScore = fourthPlaceLeaderboardData[0];
            RankedButtonFourthText.text = fourthButtonUsername + ": " + fourthButtonScore;

            fifthButtonUsername = fifthPlaceLeaderboardData[6];
            fifthButtonScore = fifthPlaceLeaderboardData[0];
            RankedButtonFifthText.text = fifthButtonUsername + ": " + fifthButtonScore;
        }


       
    }

    IEnumerator RetrieveBeatmapLeaderboard(int leaderboardPlaceToGetPass)
    {
        GetLeaderboardTableName();

        WWWForm form = new WWWForm();
        form.AddField("leaderboardTableName", leaderboardTableName);
        form.AddField("leaderboardPlaceToGet", leaderboardPlaceToGetPass);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveuserbeatmapscore.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        teststring = www.downloadHandler.text;

        ArrayList placeList = new ArrayList();

        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        for (int dataType = 0; dataType < 7; dataType++)
        {
            /*
              DataType:
              [0] = score
              [1] = perfect
              [2] = good
              [3] = early
              [4] = miss
              [5] = combo
              [6] = player_id
            */

            if (leaderboardPlaceToGetPass == 1)
            {
                firstPlaceLeaderboardData.Add(placeList[dataType].ToString());
            }
            else if (leaderboardPlaceToGetPass == 2)
            {
                secondPlaceLeaderboardData.Add(placeList[dataType].ToString());
            }
            else if (leaderboardPlaceToGetPass == 3)
            {
                thirdPlaceLeaderboardData.Add(placeList[dataType].ToString());
            }
            else if (leaderboardPlaceToGetPass == 4)
            {
                fourthPlaceLeaderboardData.Add(placeList[dataType].ToString());
            }
            else if (leaderboardPlaceToGetPass == 5)
            {
                fifthPlaceLeaderboardData.Add(placeList[dataType].ToString());
            }

        }

    }

    public void GetLeaderboardTableName()
    {
        leaderboardTableName = Database.database.loadedLeaderboardTableName;
    }

    public void ResetLeaderboard()
    {
        RankedButtonFirstText.text = "1#";
        RankedButtonSecondText.text = "2#";
        RankedButtonThirdText.text = "3#";
        RankedButtonFourthText.text = "4#";
        RankedButtonFifthText.text = "5#";
    }
    

}
