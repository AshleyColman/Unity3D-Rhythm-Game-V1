using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class BeatmapRankingManager : MonoBehaviour
{


    public string leaderboardTableName;
    public bool notChecked = true;
    public bool hasPersonalBest = false;
    public List<string> firstPlaceLeaderboardData = new List<string>();
    public List<string> secondPlaceLeaderboardData = new List<string>();
    public List<string> thirdPlaceLeaderboardData = new List<string>();
    public List<string> fourthPlaceLeaderboardData = new List<string>();
    public List<string> fifthPlaceLeaderboardData = new List<string>();
    public List<string> sixthPlaceLeaderboardData = new List<string>();
    public List<string> personalBestLeaderboardData = new List<string>();
    public string teststring;
    public int leaderboardPlaceToGet;
    public bool setFirst = false;

    // Leaderboard text
    public TextMeshProUGUI RankedButtonFirstGradeText;
    public TextMeshProUGUI RankedButtonFirstRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonFirstPlayStatisticsText;
    public TextMeshProUGUI RankedButtonFirstPlayCount;

    public TextMeshProUGUI RankedButtonSecondGradeText;
    public TextMeshProUGUI RankedButtonSecondRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonSecondPlayStatisticsText;
    public TextMeshProUGUI RankedButtonSecondPlayCount;

    public TextMeshProUGUI RankedButtonThirdGradeText;
    public TextMeshProUGUI RankedButtonThirdRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonThirdPlayStatisticsText;
    public TextMeshProUGUI RankedButtonThirdPlayCount;

    public TextMeshProUGUI RankedButtonFourthGradeText;
    public TextMeshProUGUI RankedButtonFourthRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonFourthPlayStatisticsText;
    public TextMeshProUGUI RankedButtonFourthPlayCount;

    public TextMeshProUGUI RankedButtonFifthGradeText;
    public TextMeshProUGUI RankedButtonFifthRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonFifthPlayStatisticsText;
    public TextMeshProUGUI RankedButtonFifthPlayCount;

    public TextMeshProUGUI RankedButtonSixthGradeText;
    public TextMeshProUGUI RankedButtonSixthRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonSixthPlayStatisticsText;
    public TextMeshProUGUI RankedButtonSixthPlayCount;

    public TextMeshProUGUI RankedButtonPersonalBestGradeText;
    public TextMeshProUGUI RankedButtonPersonalBestRankAndUsernameAndScoreText;
    public TextMeshProUGUI RankedButtonPersonalBestPlayStatisticsText;

    // Default character used at the moment
    private string CharacterUsed = "NO FAIL";

    // Button text variables
    string firstButtonScore;
    string firstButtonPerfect;
    string firstButtonGood;
    string firstButtonEarly;
    string firstButtonMiss;
    string firstButtonCombo;
    string firstButtonUsername;
    string firstButtonGrade;
    string firstButtonPercentage;
    string firstButtonPlayCount;

    string secondButtonScore;
    string secondButtonPerfect;
    string secondButtonGood;
    string secondButtonEarly;
    string secondButtonMiss;
    string secondButtonCombo;
    string secondButtonUsername;
    string secondButtonGrade;
    string secondButtonPercentage;
    string secondButtonPlayCount;

    string thirdButtonScore;
    string thirdButtonPerfect;
    string thirdButtonGood;
    string thirdButtonEarly;
    string thirdButtonMiss;
    string thirdButtonCombo;
    string thirdButtonUsername;
    string thirdButtonGrade;
    string thirdButtonPercentage;
    string thirdButtonPlayCount;

    string fourthButtonScore;
    string fourthButtonPerfect;
    string fourthButtonGood;
    string fourthButtonEarly;
    string fourthButtonMiss;
    string fourthButtonCombo;
    string fourthButtonUsername;
    string fourthButtonGrade;
    string fourthButtonPercentage;
    string fourthButtonPlayCount;

    string fifthButtonScore;
    string fifthButtonPerfect;
    string fifthButtonGood;
    string fifthButtonEarly;
    string fifthButtonMiss;
    string fifthButtonCombo;
    string fifthButtonUsername;
    string fifthButtonGrade;
    string fifthButtonPercentage;
    string fifthButtonPlayCount;

    string sixthButtonScore;
    string sixthButtonPerfect;
    string sixthButtonGood;
    string sixthButtonEarly;
    string sixthButtonMiss;
    string sixthButtonCombo;
    string sixthButtonUsername;
    string sixthButtonGrade;
    string sixthButtonPercentage;
    string sixthButtonPlayCount;


    string personalBestButtonScore;
    string personalBestButtonPerfect;
    string personalBestButtonGood;
    string personalBestButtonEarly;
    string personalBestButtonMiss;
    string personalBestButtonCombo;
    string personalBestButtonUsername;
    string personalBestButtonGrade;
    string personalBestButtonPercentage;

    bool firstExists = false, secondExists = false, thirdExists = false, fourthExists = false, fifthExists = false, sixthExists = false;
    bool hasCheckedPersonalBest = false;
    public Color pColor, sColor, aColor, bColor, cColor, dColor, eColor, fColor, defaultColor;

    string player_id;

    public Image personalBestCharacterImage, firstPlaceCharacterImage, secondPlaceCharacterImage, thirdPlaceCharacterImage, fourthPlaceCharacterImage,
        fifthPlaceCharacterImage, sixthPlaceCharacterImage;

    public ParticleSystem firstButtonGradeParticles;
    public ParticleSystem secondButtonGradeParticles;
    public ParticleSystem thirdButtonGradeParticles;
    public ParticleSystem fourthButtonGradeParticles;
    public ParticleSystem fifthButtonGradeParticles;
    public ParticleSystem sixthButtonGradeParticles;
    public ParticleSystem personalBestButtonGradeParticles;

    void Start()
    {
        leaderboardPlaceToGet = 1;
    }

    void Update()
    {

        if (notChecked == true && hasCheckedPersonalBest == false)
        {
            for (int i = 0; i < 6; i++)
            { 
                // Retrieve the top 5 scores
                StartCoroutine(RetrieveBeatmapLeaderboard(leaderboardPlaceToGet));
                leaderboardPlaceToGet++;
            }

            StartCoroutine(RetrievePersonalBest());

            notChecked = false;
        }

      



        if (notChecked == false && hasCheckedPersonalBest == true)
        {

            // Assign the database information to the variables
            if (MySQLDBManager.loggedIn && hasPersonalBest == true)
            {
                personalBestButtonScore = personalBestLeaderboardData[0];
                personalBestButtonPerfect = personalBestLeaderboardData[1]; ;
                personalBestButtonGood = personalBestLeaderboardData[2];
                personalBestButtonEarly = personalBestLeaderboardData[3];
                personalBestButtonMiss = personalBestLeaderboardData[4];
                personalBestButtonCombo = personalBestLeaderboardData[5];
                personalBestButtonUsername = personalBestLeaderboardData[6];
                personalBestButtonGrade = personalBestLeaderboardData[7];
                personalBestButtonPercentage = personalBestLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (personalBestButtonGrade == "S" || personalBestButtonGrade == "P")
                {
                    personalBestButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonPersonalBestGradeText.text = personalBestButtonGrade;
                RankedButtonPersonalBestGradeText.color = SetGradeColor(personalBestButtonGrade);
                RankedButtonPersonalBestRankAndUsernameAndScoreText.text = personalBestButtonUsername + ": " + personalBestButtonScore;
                RankedButtonPersonalBestPlayStatisticsText.text = "[" + personalBestButtonPercentage + "%] " + "[x" + personalBestButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                personalBestCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                personalBestCharacterImage.gameObject.SetActive(false);
            }

            if (firstExists == true)
            {
                // Assign the database information to the variables
                firstButtonScore = firstPlaceLeaderboardData[0];
                firstButtonPerfect = firstPlaceLeaderboardData[1]; ;
                firstButtonGood = firstPlaceLeaderboardData[2];
                firstButtonEarly = firstPlaceLeaderboardData[3];
                firstButtonMiss = firstPlaceLeaderboardData[4];
                firstButtonCombo = firstPlaceLeaderboardData[5];
                firstButtonUsername = firstPlaceLeaderboardData[6];
                firstButtonGrade = firstPlaceLeaderboardData[7];
                firstButtonPercentage = firstPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (firstButtonGrade == "S" || firstButtonGrade == "P")
                {
                    firstButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonFirstGradeText.text = firstButtonGrade;
                RankedButtonFirstGradeText.color = SetGradeColor(firstButtonGrade);
                RankedButtonFirstRankAndUsernameAndScoreText.text = "1# " + firstButtonUsername + ": " + firstButtonScore;
                RankedButtonFirstPlayStatisticsText.text = "[" + firstButtonPercentage + "%] " + "[x" + firstButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                firstPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                firstPlaceCharacterImage.gameObject.SetActive(false);
            }

            if (secondExists == true)
            {
                // Assign the database information to the variables
                secondButtonScore = secondPlaceLeaderboardData[0];
                secondButtonPerfect = secondPlaceLeaderboardData[1]; ;
                secondButtonGood = secondPlaceLeaderboardData[2];
                secondButtonEarly = secondPlaceLeaderboardData[3];
                secondButtonMiss = secondPlaceLeaderboardData[4];
                secondButtonCombo = secondPlaceLeaderboardData[5];
                secondButtonUsername = secondPlaceLeaderboardData[6];
                secondButtonGrade = secondPlaceLeaderboardData[7];
                secondButtonPercentage = secondPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (secondButtonGrade == "S" || secondButtonGrade == "P")
                {
                    secondButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonSecondGradeText.text = secondButtonGrade;
                RankedButtonSecondGradeText.color = SetGradeColor(secondButtonGrade);
                RankedButtonSecondRankAndUsernameAndScoreText.text = "2# " + secondButtonUsername + ": " + secondButtonScore;
                RankedButtonSecondPlayStatisticsText.text = "[" + secondButtonPercentage + "%] " + "[x" + secondButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                secondPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                secondPlaceCharacterImage.gameObject.SetActive(false);
            }

            if (thirdExists == true)
            {
                // Assign the database information to the variables
                thirdButtonScore = thirdPlaceLeaderboardData[0];
                thirdButtonPerfect = thirdPlaceLeaderboardData[1]; ;
                thirdButtonGood = thirdPlaceLeaderboardData[2];
                thirdButtonEarly = thirdPlaceLeaderboardData[3];
                thirdButtonMiss = thirdPlaceLeaderboardData[4];
                thirdButtonCombo = thirdPlaceLeaderboardData[5];
                thirdButtonUsername = thirdPlaceLeaderboardData[6];
                thirdButtonGrade = thirdPlaceLeaderboardData[7];
                thirdButtonPercentage = thirdPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (thirdButtonGrade == "S" || thirdButtonGrade == "P")
                {
                    thirdButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonThirdGradeText.text = thirdButtonGrade;
                RankedButtonThirdGradeText.color = SetGradeColor(thirdButtonGrade);
                RankedButtonThirdRankAndUsernameAndScoreText.text = "3# " + thirdButtonUsername + ": " + thirdButtonScore;
                RankedButtonThirdPlayStatisticsText.text = "[" + thirdButtonPercentage + "%] " + "[x" + thirdButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                thirdPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                thirdPlaceCharacterImage.gameObject.SetActive(false);
            }

            if (fourthExists == true)
            {
                // Assign the database information to the variables
                fourthButtonScore = fourthPlaceLeaderboardData[0];
                fourthButtonPerfect = fourthPlaceLeaderboardData[1]; ;
                fourthButtonGood = fourthPlaceLeaderboardData[2];
                fourthButtonEarly = fourthPlaceLeaderboardData[3];
                fourthButtonMiss = fourthPlaceLeaderboardData[4];
                fourthButtonCombo = fourthPlaceLeaderboardData[5];
                fourthButtonUsername = fourthPlaceLeaderboardData[6];
                fourthButtonGrade = fourthPlaceLeaderboardData[7];
                fourthButtonPercentage = fourthPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (fourthButtonGrade == "S" || fourthButtonGrade == "P")
                {
                    fourthButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonFourthGradeText.text = fourthButtonGrade;
                RankedButtonFourthGradeText.color = SetGradeColor(fourthButtonGrade);
                RankedButtonFourthRankAndUsernameAndScoreText.text = "4# " + fourthButtonUsername + ": " + fourthButtonScore;
                RankedButtonFourthPlayStatisticsText.text = "[" + fourthButtonPercentage + "%] " + "[x" + fourthButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Disable character used image
                fourthPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                fourthPlaceCharacterImage.gameObject.SetActive(false);
            }

            if (fifthExists == true)
            {
                // Assign the database information to the variables
                fifthButtonScore = fifthPlaceLeaderboardData[0];
                fifthButtonPerfect = fifthPlaceLeaderboardData[1]; ;
                fifthButtonGood = fifthPlaceLeaderboardData[2];
                fifthButtonEarly = fifthPlaceLeaderboardData[3];
                fifthButtonMiss = fifthPlaceLeaderboardData[4];
                fifthButtonCombo = fifthPlaceLeaderboardData[5];
                fifthButtonUsername = fifthPlaceLeaderboardData[6];
                fifthButtonGrade = fifthPlaceLeaderboardData[7];
                fifthButtonPercentage = fifthPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (fifthButtonGrade == "S" || fifthButtonGrade == "P")
                {
                    fifthButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonFifthGradeText.text = fifthButtonGrade;
                RankedButtonFifthGradeText.color = SetGradeColor(fifthButtonGrade);
                RankedButtonFifthRankAndUsernameAndScoreText.text = "5# " + fifthButtonUsername + ": " + fifthButtonScore;
                RankedButtonFifthPlayStatisticsText.text = "[" + fifthButtonPercentage + "%] " + "[x" + fifthButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                fifthPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                fifthPlaceCharacterImage.gameObject.SetActive(false);
            }

            if (sixthExists == true)
            {
                // Assign the database information to the variables
                sixthButtonScore = sixthPlaceLeaderboardData[0];
                sixthButtonPerfect = sixthPlaceLeaderboardData[1]; ;
                sixthButtonGood = sixthPlaceLeaderboardData[2];
                sixthButtonEarly = sixthPlaceLeaderboardData[3];
                sixthButtonMiss = sixthPlaceLeaderboardData[4];
                sixthButtonCombo = sixthPlaceLeaderboardData[5];
                sixthButtonUsername = sixthPlaceLeaderboardData[6];
                sixthButtonGrade = sixthPlaceLeaderboardData[7];
                sixthButtonPercentage = sixthPlaceLeaderboardData[8];

                // Check if P or S rank has been achieved, if it has enable the grade particle system
                if (sixthButtonGrade == "S" || sixthButtonGrade == "P")
                {
                    sixthButtonGradeParticles.gameObject.SetActive(true);
                }

                // Assign the text to leaderboard place button
                RankedButtonSixthGradeText.text = sixthButtonGrade;
                RankedButtonSixthGradeText.color = SetGradeColor(sixthButtonGrade);
                RankedButtonSixthRankAndUsernameAndScoreText.text = "6# " + sixthButtonUsername + ": " + sixthButtonScore;
                RankedButtonSixthPlayStatisticsText.text = "[" + sixthButtonPercentage + "%] " + "[x" + sixthButtonCombo + "] " + "[" + CharacterUsed + "]";

                // Enable character used image
                sixthPlaceCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                // Disable character used image
                sixthPlaceCharacterImage.gameObject.SetActive(false);
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

        for (int dataType = 0; dataType < 9; dataType++)
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
              [7] = grade
              [8] = percentage
            */

            if (leaderboardPlaceToGetPass == 1)
            {
                if (www.downloadHandler.text != "1")
                {
                    firstPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    firstExists = true;
                }
                else
                {
                    firstExists = false;
                }
            }
            else if (leaderboardPlaceToGetPass == 2)
            {
                if (www.downloadHandler.text != "1")
                {
                    secondPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    secondExists = true;
                }
                else
                {
                    secondExists = false;
                }
            }
            else if (leaderboardPlaceToGetPass == 3)
            {
                if (www.downloadHandler.text != "1")
                {
                    thirdPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    thirdExists = true;
                }
                else
                {
                    thirdExists = false;
                }
            }
            else if (leaderboardPlaceToGetPass == 4)
            {
                if (www.downloadHandler.text != "1")
                {
                    fourthPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    fourthExists = true;
                }
                else
                {
                    fourthExists = false;
                }
            }
            else if (leaderboardPlaceToGetPass == 5)
            {
                if (www.downloadHandler.text != "1")
                {
                    fifthPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    fifthExists = true;
                }
                else
                {
                    fifthExists = false;
                }
            }
            else if (leaderboardPlaceToGetPass == 6)
            {
                if (www.downloadHandler.text != "1")
                {
                    sixthPlaceLeaderboardData.Add(placeList[dataType].ToString());
                    sixthExists = true;
                }
                else
                {
                    sixthExists = false;
                }
            }

        }

    }

    public void GetLeaderboardTableName()
    {
        leaderboardTableName = Database.database.loadedLeaderboardTableName;

        // Make sure to do a check on whether the leaderboard is ranked, if it is then get the leaderboard name and enable all the leaderbaord checks, if not disable
    }

    public void ResetLeaderboard()
    {
        RankedButtonFirstGradeText.text = "";
        RankedButtonFirstRankAndUsernameAndScoreText.text = "";
        RankedButtonFirstPlayStatisticsText.text = "";

        RankedButtonSecondGradeText.text = "";
        RankedButtonSecondRankAndUsernameAndScoreText.text = "";
        RankedButtonSecondPlayStatisticsText.text = "";

        RankedButtonThirdGradeText.text = "";
        RankedButtonThirdRankAndUsernameAndScoreText.text = "";
        RankedButtonThirdPlayStatisticsText.text = "";

        RankedButtonFourthGradeText.text = "";
        RankedButtonFourthRankAndUsernameAndScoreText.text = "";
        RankedButtonFourthPlayStatisticsText.text = "";

        RankedButtonFifthGradeText.text = "";
        RankedButtonFifthRankAndUsernameAndScoreText.text = "";
        RankedButtonFifthPlayStatisticsText.text = "";

        RankedButtonSixthGradeText.text = "";
        RankedButtonSixthRankAndUsernameAndScoreText.text = "";
        RankedButtonSixthPlayStatisticsText.text = "";

        RankedButtonPersonalBestGradeText.text = "";
        RankedButtonPersonalBestRankAndUsernameAndScoreText.text = "";
        RankedButtonPersonalBestPlayStatisticsText.text = "";

        firstPlaceLeaderboardData.Clear();
        secondPlaceLeaderboardData.Clear();
        thirdPlaceLeaderboardData.Clear();
        fourthPlaceLeaderboardData.Clear();
        fifthPlaceLeaderboardData.Clear();
        sixthPlaceLeaderboardData.Clear();
        personalBestLeaderboardData.Clear();

        // Reset Exists bools
        firstExists = false;
        secondExists = false;
        thirdExists = false;
        fourthExists = false;
        fifthExists = false;
        sixthExists = false;

        // Reset Not Checked for leaderboard
        ResetNotChecked();

        // Reset character images
        firstPlaceCharacterImage.gameObject.SetActive(false);
        secondPlaceCharacterImage.gameObject.SetActive(false);
        thirdPlaceCharacterImage.gameObject.SetActive(false);
        fourthPlaceCharacterImage.gameObject.SetActive(false);
        fifthPlaceCharacterImage.gameObject.SetActive(false);
        sixthPlaceCharacterImage.gameObject.SetActive(false);
        personalBestCharacterImage.gameObject.SetActive(false);

        // Reset grade particles
        firstButtonGradeParticles.gameObject.SetActive(false);
        secondButtonGradeParticles.gameObject.SetActive(false);
        thirdButtonGradeParticles.gameObject.SetActive(false);
        fourthButtonGradeParticles.gameObject.SetActive(false);
        fifthButtonGradeParticles.gameObject.SetActive(false);
        sixthButtonGradeParticles.gameObject.SetActive(false);
        personalBestButtonGradeParticles.gameObject.SetActive(false);
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

            for (int dataType = 0; dataType < 9; dataType++)
            {
                // If it succeeded
                if (www.downloadHandler.text != "1")
                {
                    personalBestLeaderboardData.Add(placeList[dataType].ToString());
                    hasPersonalBest = true;
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

}

