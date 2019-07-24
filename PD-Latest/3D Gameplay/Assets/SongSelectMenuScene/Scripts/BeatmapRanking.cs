using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class BeatmapRanking : MonoBehaviour {

    // UI
    public Button leaderboardLoadingIconButton;
    public TextMeshProUGUI[] rankedButtonUsernameAndScoreText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonPlayStatisticsText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonPerfectText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonGoodText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonEarlyText = new TextMeshProUGUI[20];
    public TextMeshProUGUI[] rankedButtonMissText = new TextMeshProUGUI[20];
    public TextMeshProUGUI personalBestButtonUsernameAndScoreText;
    public TextMeshProUGUI personalBestButtonPlayStatisticsText;
    public TextMeshProUGUI personalBestPerfectText;
    public TextMeshProUGUI personalBestGoodText;
    public TextMeshProUGUI personalBestEarlyText;
    public TextMeshProUGUI personalBestMissText;
    // Animation
    public Animator[] rankedButtonBunnyGradeIcon = new Animator[20]; // Leaderboard text
    public Animator personalBestButtonBunnyGradeIcon;

    // Bools
    private bool notChecked;
    private bool hasPersonalBest;
    private bool hasCheckedPersonalBest;
    private bool hasLoadedLeaderbaord;
    private bool[] placeExists;

    // Strings
    private string leaderboardTableName;
    private string player_id;
    private string personalBestScore, personalBestPerfect, personalBestGood, personalBestEarly, personalBestMiss, personalBestCombo, personalBestPercentage, 
        personalBestGrade, personalBestUsername, personalBestMod;
    private string perfectTextValue, goodTextValue, earlyTextValue, missTextValue; // Text values on leaderbaord placements
    private List<string>[] placeLeaderboardData;
    private List<string> personalBestLeaderboardData;
    private string[] rankedButtonUsername, rankedButtonPerfect, rankedButtonGood, rankedButtonEarly, rankedButtonMiss, rankedButtonCombo,
        rankedButtonPercentage, rankedButtonGrade, rankedButtonScore, rankedButtonMod;

    // Integers
    private sbyte leaderboardPlaceToGet;
    private sbyte totalRankingPlacements;

    // Colors
    public Color pColor, sColor, aColor, bColor, cColor, dColor, eColor, fColor, defaultColor;

    void Start()
    {
        // Initialize
        placeExists = new bool[20];
        hasPersonalBest = false;
        hasCheckedPersonalBest = false;
        hasLoadedLeaderbaord = false;
        notChecked = true;

        perfectTextValue = "P: ";
        goodTextValue = "G: ";
        earlyTextValue = "E: ";
        missTextValue = "M: ";

        placeLeaderboardData = new List<string>[20];
        personalBestLeaderboardData = new List<string>();
        rankedButtonUsername = new string[20];
        rankedButtonPerfect = new string[20];
        rankedButtonGood = new string[20];
        rankedButtonEarly = new string[20];
        rankedButtonMiss = new string[20];
        rankedButtonCombo = new string[20];
        rankedButtonPercentage = new string[20];
        rankedButtonGrade = new string[20];
        rankedButtonScore = new string[20];
        rankedButtonMod = new string[20];

        totalRankingPlacements = 20;
        leaderboardPlaceToGet = 1;


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
        // If the leaderboard placements and personal best has not been checked yet
        if (notChecked == true && hasCheckedPersonalBest == false)
        {
            // Retrieve all placement information for the leaderboard
            for (sbyte placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
            {
                // Retrieve top players
                StartCoroutine(RetrieveBeatmapLeaderboard(leaderboardPlaceToGet));
                // Increment the placement to get
                leaderboardPlaceToGet++;
            }

            // Retrieve personal best information
            StartCoroutine(RetrievePersonalBest());

            // Set to false as the leaderbaord placements have now been checked
            notChecked = false;
        }

        // If the leaderboard placements and personal best have been checked and retrieved, and the leaderboard has not updated yet
        if (notChecked == false && hasCheckedPersonalBest == true && hasLoadedLeaderbaord == false)
        {
            // Loop through all the placements
            for (sbyte placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
            {
                // If placement information was found for the position on the leaderboard
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

                    // Enable the ranked bunny grade icon if eri was used
                    rankedButtonBunnyGradeIcon[placementToCheck].gameObject.SetActive(true);

                    // Enable correct grade icon based on the grade achieved
                    switch (rankedButtonGrade[placementToCheck])
                    {
                        case "P":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyPRank");
                            break;
                        case "S":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnySRank");
                            break;
                        case "A":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyARank");
                            break;
                        case "B":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyBRank");
                            break;
                        case "C":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyCRank");
                            break;
                        case "D":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyDRank");
                            break;
                        case "E":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyERank");
                            break;
                        case "F":
                            rankedButtonBunnyGradeIcon[placementToCheck].Play("BunnyFRank");
                            break;
                    }

                    // Update the username and score text for the placement on the leaderbaord
                    rankedButtonUsernameAndScoreText[placementToCheck].text = (placementToCheck + 1) + "# " + rankedButtonUsername[placementToCheck] + ": " +
                    rankedButtonScore[placementToCheck];

                    // Check if a mod was used, update the text with mod or without mod text
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

 
                    // Update the perfect, good, early and miss text value information for the placement
                    rankedButtonPerfectText[placementToCheck].text = perfectTextValue + rankedButtonPerfect[placementToCheck];
                    rankedButtonGoodText[placementToCheck].text = goodTextValue + rankedButtonGood[placementToCheck];
                    rankedButtonEarlyText[placementToCheck].text = earlyTextValue + rankedButtonEarly[placementToCheck];
                    rankedButtonMissText[placementToCheck].text = missTextValue + rankedButtonMiss[placementToCheck];
                }

                // Set has loaded leaderbaord to true to prevent the leaderbaord from continuing to upload every frame
                hasLoadedLeaderbaord = true;

                // Turn off the leaderbaord loading icon
                DeactivateLeaderboardLoadingIcon();
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

                    //personalBestButtonGradeText.text = personalBestGrade;
                    //personalBestButtonGradeText.color = SetGradeColor(personalBestGrade);

                    personalBestButtonBunnyGradeIcon.gameObject.SetActive(true);

                    // Enable correct grade icon
                    switch (personalBestGrade)
                    {
                        case "P":
                            personalBestButtonBunnyGradeIcon.Play("BunnyPRank");
                            break;
                        case "S":
                            personalBestButtonBunnyGradeIcon.Play("BunnySRank");
                            break;
                        case "A":
                            personalBestButtonBunnyGradeIcon.Play("BunnyARank");
                            break;
                        case "B":
                            personalBestButtonBunnyGradeIcon.Play("BunnyBRank");
                            break;
                        case "C":
                            personalBestButtonBunnyGradeIcon.Play("BunnyCRank");
                            break;
                        case "D":
                            personalBestButtonBunnyGradeIcon.Play("BunnyDRank");
                            break;
                        case "E":
                            personalBestButtonBunnyGradeIcon.Play("BunnyERank");
                            break;
                        case "F":
                            personalBestButtonBunnyGradeIcon.Play("BunnyFRank");
                            break;
                    }


                    personalBestButtonUsernameAndScoreText.text = MySQLDBManager.username + ": " + personalBestScore;

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

    // Activate the leaderbaord loading icon
    private void ActivateLeaderboardLoadingIcon()
    {
        // Display leaderboard loading animation until the leaderbaord has loaded
        leaderboardLoadingIconButton.gameObject.SetActive(true);
    }

    // Dectivate the leaderbaord loading icon
    private void DeactivateLeaderboardLoadingIcon()
    {
        // Hide the  leaderboard loading animation until the leaderbaord has been refreshed
        leaderboardLoadingIconButton.gameObject.SetActive(false);
    }

    // Retrieve beatmap leaderboard placement information for the position passed
    public IEnumerator RetrieveBeatmapLeaderboard(int leaderboardPlaceToGetPass)
    {
        WWWForm form = new WWWForm();
        form.AddField("leaderboardTableName", leaderboardTableName);
        form.AddField("leaderboardPlaceToGet", leaderboardPlaceToGetPass);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveuserbeatmapscore.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Stores all the placement information from the database
        ArrayList placeList = new ArrayList();

        // Split the information retrieved from the database
        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        // Loop through all the leaderboard data and assign
        for (sbyte dataType = 0; dataType < 10; dataType++)
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

            // SUCCESS - LEADERBOARD DATA FOUND FOR THIS PLACEMENT
            if (www.downloadHandler.text != "1")
            {
                // Save to the placement data list for this placement number
                placeLeaderboardData[leaderboardPlaceToGetPass - 1].Add(placeList[dataType].ToString());
                placeExists[leaderboardPlaceToGetPass - 1] = true;
            }
            else
            {
                // ERROR - NO LEADERBOARD DATA FOR THIS PLACEMENT
                placeExists[leaderboardPlaceToGetPass - 1] = false;
            }
        }
    }
    
    // Get the leaderboard table name to load the beatmap leaderboard from
    public void GetLeaderboardTableName()
    {
        // Get it from the database gameobject
        leaderboardTableName = Database.database.LoadedLeaderboardTableName;

        // Activate the leaderboard loading icon
        ActivateLeaderboardLoadingIcon();

        // Make sure to do a check on whether the leaderboard is ranked, if it is then get the leaderboard name and enable all the leaderbaord checks, if not disable
    }

    // Retrieve the users personal best score
    public IEnumerator RetrievePersonalBest()
    {
        // Check if the user has logged in
        if (MySQLDBManager.loggedIn)
        {
            WWWForm form = new WWWForm();
            form.AddField("leaderboardTableName", leaderboardTableName);
            form.AddField("player_id", MySQLDBManager.username);


            UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveuserpersonalbest.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            // Stores personal best leaderboard data from the database
            ArrayList placeList = new ArrayList();

            // Splits the data retrieved
            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

            // Loop through all the data retrieved and assign to the personal best leaderboard data list
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
                    // FAILED - NO PERSONAL BEST DATA FOUND FOR THIS BEATMAP LEADERBOARD
                }
            }

            // Set to true as the personal best leaderboard information has now been checked
            hasCheckedPersonalBest = true;
        }
        else
        {
            // Set to false as no data was found for personal best
            hasPersonalBest = false;
            // Set to true as the personal best leaderboard information has now been checked
            hasCheckedPersonalBest = true;
            yield return null;
        }
    }

    // Reset the leaderboard
    public void ResetLeaderboard()
    {
        // Loop through all the placements and reset the leaderboard data for each placement
        for (sbyte placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
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
            rankedButtonBunnyGradeIcon[placementToCheck].gameObject.SetActive(false);
            rankedButtonUsernameAndScoreText[placementToCheck].text = "";
            rankedButtonPlayStatisticsText[placementToCheck].text = "";
            rankedButtonPerfectText[placementToCheck].text = "";
            rankedButtonGoodText[placementToCheck].text = "";
            rankedButtonEarlyText[placementToCheck].text = "";
            rankedButtonMissText[placementToCheck].text = "";

            // Clear the leaderboard data 
            placeLeaderboardData[placementToCheck].Clear();
            // Reset all places that exist
            placeExists[placementToCheck] = false;
        }

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
        personalBestButtonBunnyGradeIcon.gameObject.SetActive(false);
        personalBestButtonUsernameAndScoreText.text = "";
        personalBestButtonPlayStatisticsText.text = "";
        personalBestPerfectText.text = "";
        personalBestGoodText.text = "";
        personalBestEarlyText.text = "";
        personalBestMissText.text = "";

        // Reset personal best leaderboard data
        personalBestLeaderboardData.Clear();

        // Reset checking variables
        //ResetNotChecked();
        
        // Reset the leaderbaord loading animation
        hasLoadedLeaderbaord = false;
    }

    // Reset the leaderboard checking variables
    public void ResetNotChecked()
    {
        // Reset leaderboard checking
        notChecked = true;
        hasPersonalBest = false;
        hasCheckedPersonalBest = false;
        leaderboardPlaceToGet = 1;
    }

    // Set the grade icon color based on the grade passed
    public Color SetGradeColor(string _grade)
    {
        switch (_grade)
        {
            case "P":
                return pColor;
            case "S":
                return sColor;
            case "A":
                return aColor;
            case "B":
                return bColor;
            case "C":
                return cColor;
            case "D":
                return dColor;
            case "E":
                return eColor;
            case "F":
                return fColor;
            default:
                return defaultColor;
        }
    }
}
