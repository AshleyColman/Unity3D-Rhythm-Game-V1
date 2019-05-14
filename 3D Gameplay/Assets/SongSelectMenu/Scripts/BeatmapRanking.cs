using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class BeatmapRanking : MonoBehaviour {

    public Button leaderboardLoadingIconButton;


    public string leaderboardTableName;
    private bool notChecked;
    public bool hasPersonalBest = false;
    private List<string>[] placeLeaderboardData = new List<string>[20];
    private List<string> personalBestLeaderboardData = new List<string>();
    public string teststring;
    public int leaderboardPlaceToGet;
    public bool setFirst = false;

    // Leaderboard text
    public TextMeshProUGUI[] rankedButtonGradeText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonUsernameAndScoreText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonPlayStatisticsText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonPerfectText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonGoodText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonEarlyText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonMissText = new TextMeshProUGUI[20];



    string[] rankedButtonUsername = new string[20];
    string[] rankedButtonPerfect = new string[20];
    string[] rankedButtonGood = new string[20];
    string[] rankedButtonEarly = new string[20];
    string[] rankedButtonMiss = new string[20];
    string[] rankedButtonCombo = new string[20];
    string[] rankedButtonPercentage = new string[20];
    string[] rankedButtonGrade = new string[20];
    string[] rankedButtonScore = new string[20];
    string[] rankedButtonMod = new string[20];

    public ParticleSystem[] rankedButtonGradeParticles = new ParticleSystem[20];
    public Image[] playerImage = new Image[20];

    public TextMeshProUGUI personalBestButtonGradeText;
    public TextMeshProUGUI personalBestButtonUsernameAndScoreText;
    public TextMeshProUGUI personalBestButtonPlayStatisticsText;
    public TextMeshProUGUI personalBestPerfectText;
    public TextMeshProUGUI personalBestGoodText;
    public TextMeshProUGUI personalBestEarlyText;
    public TextMeshProUGUI personalBestMissText;

    string personalBestScore;
    string personalBestPerfect;
    string personalBestGood;
    string personalBestEarly;
    string personalBestMiss;
    string personalBestCombo;
    string personalBestPercentage;
    string personalBestGrade;
    string personalBestUsername;
    string personalBestMod;

    public ParticleSystem personalBestGradeParticles;
    public Image personalBestImage;

    bool[] placeExists = new bool[20];

    bool hasCheckedPersonalBest = false;

    string player_id;


    int totalRankingPlacements;

    public Color pColor, sColor, aColor, bColor, cColor, dColor, eColor, fColor, defaultColor;

    bool hasLoadedLeaderbaord;


    void Start()
    {
        hasLoadedLeaderbaord = false;
        totalRankingPlacements = 20;
        leaderboardPlaceToGet = 1;
        notChecked = true;


        // Instantiate the lists
        for (int i = 0; i < placeLeaderboardData.Length; i++)
        {
            placeLeaderboardData[i] = new List<string>();
        }

        // Reset the leaderboard at the start
        ResetLeaderboard();
    }

    void Update()
    {
        // Display leaderboard loading animation until the leaderbaord has loaded
        if (hasLoadedLeaderbaord == true)
        {
            leaderboardLoadingIconButton.gameObject.SetActive(false);
        }
        else
        {
            leaderboardLoadingIconButton.gameObject.SetActive(true);
        }

        if (notChecked == true && hasCheckedPersonalBest == false)
        {
            for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
            {
                // Retrieve top players
                StartCoroutine(RetrieveBeatmapLeaderboard(leaderboardPlaceToGet));
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
                    rankedButtonScore[placementToCheck] = placeLeaderboardData[placementToCheck][0];
                    rankedButtonPerfect[placementToCheck] = placeLeaderboardData[placementToCheck][1];
                    rankedButtonGood[placementToCheck] = placeLeaderboardData[placementToCheck][2];
                    rankedButtonEarly[placementToCheck] = placeLeaderboardData[placementToCheck][3];
                    rankedButtonMiss[placementToCheck] = placeLeaderboardData[placementToCheck][4];
                    rankedButtonCombo[placementToCheck] = placeLeaderboardData[placementToCheck][5];
                    rankedButtonUsername[placementToCheck] = placeLeaderboardData[placementToCheck][6];
                    rankedButtonGrade[placementToCheck] = placeLeaderboardData[placementToCheck][7];
                    rankedButtonPercentage[placementToCheck] = placeLeaderboardData[placementToCheck][8];
                    rankedButtonMod[placementToCheck] = placeLeaderboardData[placementToCheck][9];

                    if (rankedButtonGrade[placementToCheck] == "S" || rankedButtonGrade[placementToCheck] == "P")
                    {
                        rankedButtonGradeParticles[placementToCheck].gameObject.SetActive(true);
                    }

                    // Update the text for the leaderboard button
                    rankedButtonGradeText[placementToCheck].text = rankedButtonGrade[placementToCheck];

                    rankedButtonGradeText[placementToCheck].color = SetGradeColor(rankedButtonGrade[placementToCheck]);

                    rankedButtonUsernameAndScoreText[placementToCheck].text = (placementToCheck + 1) + "# " + rankedButtonUsername[placementToCheck] + ": " +
                    rankedButtonScore[placementToCheck];

                    if (string.IsNullOrEmpty(rankedButtonMod[placementToCheck]))
                    {
                        rankedButtonPlayStatisticsText[placementToCheck].text = "[" + rankedButtonPercentage[placementToCheck] + "%] " +
                        "[x" + rankedButtonCombo[placementToCheck] + "] ";
                    }
                    else
                    {
                        rankedButtonPlayStatisticsText[placementToCheck].text = "[" + rankedButtonPercentage[placementToCheck] + "%] " +
                        "[x" + rankedButtonCombo[placementToCheck] + "] " + "[" + rankedButtonMod[placementToCheck] + "]";
                    }

                    

                    rankedButtonPerfectText[placementToCheck].text = "P:" + rankedButtonPerfect[placementToCheck];
                    rankedButtonGoodText[placementToCheck].text = "G:" + rankedButtonGood[placementToCheck];
                    rankedButtonEarlyText[placementToCheck].text = "E:" + rankedButtonEarly[placementToCheck];
                    rankedButtonMissText[placementToCheck].text = "M:" + rankedButtonMiss[placementToCheck];

                    // Update the percentage/combo here for text
                }
                hasLoadedLeaderbaord = true;
            }

            if (MySQLDBManager.loggedIn == true)
            {
                if (hasPersonalBest == true)
                {
                    personalBestScore = personalBestLeaderboardData[0];
                    personalBestPerfect = personalBestLeaderboardData[1];
                    personalBestGood = personalBestLeaderboardData[2];
                    personalBestEarly = personalBestLeaderboardData[3];
                    personalBestMiss = personalBestLeaderboardData[4];
                    personalBestCombo = personalBestLeaderboardData[5];
                    personalBestUsername = personalBestLeaderboardData[6];
                    personalBestGrade = personalBestLeaderboardData[7];
                    personalBestPercentage = personalBestLeaderboardData[8];
                    personalBestMod = personalBestLeaderboardData[9];

                    personalBestButtonGradeText.text = personalBestGrade;
                    personalBestButtonGradeText.color = SetGradeColor(personalBestGrade);
                    personalBestButtonUsernameAndScoreText.text = MySQLDBManager.username + ": " + personalBestScore;

                    if (personalBestGrade == "S" || personalBestGrade == "P")
                    {
                        personalBestGradeParticles.gameObject.SetActive(true);
                    }


                    if (string.IsNullOrEmpty(personalBestMod))
                    {
                        personalBestButtonPlayStatisticsText.text = "[" + personalBestPercentage + "%] " +
                        "[x" + personalBestCombo + "] ";
                    }
                    else
                    {
                        personalBestButtonPlayStatisticsText.text = "[" + personalBestPercentage + "%] " +
                        "[x" + personalBestCombo + "] " + "[" + personalBestMod + "]";
                    }
                    

                    personalBestPerfectText.text = "P:" + personalBestPerfect;
                    personalBestGoodText.text =  "G:" + personalBestGood;
                    personalBestEarlyText.text = "E:" + personalBestEarly;
                    personalBestMissText.text = "M:" + personalBestMiss;
                }
            }
        }
    }

    public IEnumerator RetrieveBeatmapLeaderboard(int leaderboardPlaceToGetPass)
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

        for (int dataType = 0; dataType < 10; dataType++)
        {
            /*
              DataType:
              DataType:
              [0] = score
              [1] = perfect
              [2] = good
              [3] = early
              [4] = miss
              [5] = combo
              [6] = player_id
              [7] = grade
              [8] = percentage
              [9] = mod
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

    public void GetLeaderboardTableName()
    {
        leaderboardTableName = Database.database.loadedLeaderboardTableName;

        // Make sure to do a check on whether the leaderboard is ranked, if it is then get the leaderboard name and enable all the leaderbaord checks, if not disable
    }


    // Retrieve the users personal best score
    public IEnumerator RetrievePersonalBest()
    {
        GetLeaderboardTableName();

        if (MySQLDBManager.loggedIn)
        {
            player_id = MySQLDBManager.username;

            WWWForm form = new WWWForm();
            form.AddField("leaderboardTableName", leaderboardTableName);
            form.AddField("player_id", player_id);


            UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveuserpersonalbest.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            teststring = www.downloadHandler.text;

            ArrayList placeList = new ArrayList();

            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));


            for (int dataType = 0; dataType < 10; dataType++)
            {
                // If it succeeded
                if (www.downloadHandler.text != "1")
                {
                    personalBestLeaderboardData.Add(placeList[dataType].ToString());
                    hasPersonalBest = true;
                    hasCheckedPersonalBest = true;
                }
                else
                {
                    // Personal best failed
                }
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

    public void ResetLeaderboard()
    {
        for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
        {
            // Reset variables used for the leaderboard
            rankedButtonScore[placementToCheck] = "";
            rankedButtonPerfect[placementToCheck] = "";
            rankedButtonGood[placementToCheck] = "";
            rankedButtonEarly[placementToCheck] = "";
            rankedButtonMiss[placementToCheck] = "";
            rankedButtonCombo[placementToCheck] = "";
            rankedButtonUsername[placementToCheck] = "";
            rankedButtonGrade[placementToCheck] = "";
            rankedButtonPercentage[placementToCheck] = "";
            rankedButtonMod[placementToCheck] = "";
            // Reset the text on the leaderboard 
            rankedButtonGradeText[placementToCheck].text = "";
            rankedButtonUsernameAndScoreText[placementToCheck].text = "";
            rankedButtonPlayStatisticsText[placementToCheck].text = "";

            rankedButtonPerfectText[placementToCheck].text = "";
            rankedButtonGoodText[placementToCheck].text = "";
            rankedButtonEarlyText[placementToCheck].text = "";
            rankedButtonMissText[placementToCheck].text = "";

            // Disable the particles on the leaderboard
            rankedButtonGradeParticles[placementToCheck].gameObject.SetActive(false);
            // Clear the leaderboard data 
            placeLeaderboardData[placementToCheck].Clear();
            // Reset all places that exist
            placeExists[placementToCheck] = false;

            // Reset personal best information
            personalBestScore = "";
            personalBestPerfect = "";
            personalBestGood = "";
            personalBestEarly = "";
            personalBestMiss = "";
            personalBestCombo = "";
            personalBestPercentage = "";
            personalBestGrade = "";
            personalBestMod = "";

            // Reset personal best text
            personalBestButtonGradeText.text = "";
            personalBestButtonUsernameAndScoreText.text = "";
            personalBestButtonPlayStatisticsText.text = "";

            personalBestPerfectText.text = "";
            personalBestGoodText.text = "";
            personalBestEarlyText.text = "";
            personalBestMissText.text = "";

            // Reset personal best leaderboard data
            personalBestLeaderboardData.Clear();
            // Reset personal best grade particle
            personalBestGradeParticles.gameObject.SetActive(false);

            // Reset checking variables
            ResetNotChecked();

            // Reset the leaderbaord loading animation
            hasLoadedLeaderbaord = false;
        }
    }

    public void ResetNotChecked()
    {
        notChecked = true;
        hasPersonalBest = false;
        hasCheckedPersonalBest = false;
    }

    public Color SetGradeColor(string gradePass)
    {
        if (gradePass == "P")
        {
            return pColor;
        }
        else if (gradePass == "S")
        {
            return sColor;
        }
        else if (gradePass == "A")
        {
            return aColor;
        }
        else if (gradePass == "B")
        {
            return bColor;
        }
        else if (gradePass == "C")
        {
            return cColor;
        }
        else if (gradePass == "D")
        {
            return dColor;
        }
        else if (gradePass == "E")
        {
            return eColor;
        }
        else if (gradePass == "F")
        {
            return fColor;
        }
        else
        {
            return defaultColor;
        }
    }


}
