using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class BeatmapRanking : MonoBehaviour
{
    #region Variables
    // UI
    public CanvasGroup leaderboardScrollbarCanvasGroup;
    public TMP_Dropdown leaderboardSortViewDropdown, leaderboardSortTypeDropdown;
    public Scrollbar leaderboardScrollbar;

    // Button
    public Button personalBestButton;
    public Button[] leaderboardProfileButton;

    // Animation
    private Animator personalBestButtonTextAnimator;

    // Gameobject
    public GameObject loadingIcon;

    // Bools
    public bool notChecked, hasPersonalBest, hasCheckedPersonalBest, hasLoadedLeaderboard, completeLeaderboardReady;
    public bool[] placeExists;
    public bool[] placeChecked;

    // Transform
    public Transform leaderboardButtonContentTransform;

    // Strings
    private string username;
    private string leaderboardTableName;
    private string player_id;
    private const string perfectPrefix = "P: ", goodPrefix = "G: ", earlyPrefix = "E: ", missPrefix = "M: ";
    public List<string>[] placeLeaderboardData;
    public List<string> personalBestLeaderboardData;

    // Integers
    public sbyte leaderboardPlaceToGet;
    public sbyte totalRankingPlacements;
    public int totalPlacesChecked, totalImagesUpdated, totalURLImagesUpdated, totalExistingPlaces, totalURLImagesToLoad,
        timeToNextLeaderboardFlash;
    private float leaderboardFlashTimer;

    // Material
    public Material defaultMaterial;

    // Scripts
    public LeaderboardButton[] leaderboardButtonArray;
    public LeaderboardButton personalBestButtonScript;
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool[] PlaceExists
    {
        get { return placeExists; }
    }

    public bool CompleteLeaderboardReady
    {
        get { return completeLeaderboardReady; }
    }
    #endregion

    #region Functions
    void Start()
    {
        // Initialize
        placeExists = new bool[10];
        placeChecked = new bool[10];

        hasPersonalBest = false;
        hasCheckedPersonalBest = false;
        hasLoadedLeaderboard = false;
        notChecked = true;
        placeLeaderboardData = new List<string>[10];
        personalBestLeaderboardData = new List<string>();

        leaderboardButtonArray = new LeaderboardButton[10];
        personalBestButtonScript = personalBestButton.GetComponent<LeaderboardButton>();
        personalBestButtonTextAnimator = personalBestButtonScript.placementText.GetComponent<Animator>();


        totalImagesUpdated = 0;
        totalPlacesChecked = 0;
        totalRankingPlacements = 10;
        leaderboardPlaceToGet = 1;
        totalURLImagesUpdated = 0;
        totalURLImagesToLoad = 0;
        timeToNextLeaderboardFlash = 2;
        leaderboardFlashTimer = 0f;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Instantiate the lists
        for (int i = 0; i < placeLeaderboardData.Length; i++)
        {
            placeLeaderboardData[i] = new List<string>();
        }

        // GET ALL LEADERBAORD BUTTON REFERENCES 
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            leaderboardButtonArray[i] = leaderboardButtonContentTransform.GetChild(i).GetComponent<LeaderboardButton>();
            leaderboardButtonArray[i].placementText.text = (i + 1) + "#";
            leaderboardButtonArray[i].profileImage.material = new Material(Shader.Find("UI/Unlit/Transparent"));
        }

        // Reset the leaderboard at the start
        ResetLeaderboard();
    }

    void Update()
    {
        if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            switch (completeLeaderboardReady)
            {
                case false:
                    // If the leaderboard placements and personal best has not been checked yet
                    if (notChecked == true && hasCheckedPersonalBest == false)
                    {
                        // Reset 
                        totalPlacesChecked = 0;

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
                    if (notChecked == false && hasCheckedPersonalBest == true && hasLoadedLeaderboard == false && totalPlacesChecked == totalRankingPlacements)
                    {
                        // Calculate total existing places
                        CalculateTotalExistingPlaces();

                        // Loop through all the placements
                        for (sbyte placementToCheck = 0; placementToCheck < totalExistingPlaces; placementToCheck++)
                        {
                            /*
                            // Assign variables for easier reading
                            rankedButtonUsername[placementToCheck] = placeLeaderboardData[placementToCheck][0];
                            rankedButtonScore[placementToCheck] = placeLeaderboardData[placementToCheck][1];
                            rankedButtonFeverScore[placementToCheck] = placeLeaderboardData[placementToCheck][2];
                            rankedButtonCombo[placementToCheck] = placeLeaderboardData[placementToCheck][3];
                            rankedButtonPercentage[placementToCheck] = placeLeaderboardData[placementToCheck][4];
                            rankedButtonPerfect[placementToCheck] = placeLeaderboardData[placementToCheck][5];
                            rankedButtonGood[placementToCheck] = placeLeaderboardData[placementToCheck][6];
                            rankedButtonEarly[placementToCheck] = placeLeaderboardData[placementToCheck][7];
                            rankedButtonMiss[placementToCheck] = placeLeaderboardData[placementToCheck][8];
                            rankedButtonDate[placementToCheck] = placeLeaderboardData[placementToCheck][9];
                            rankedButtonMessage[placementToCheck] = placeLeaderboardData[placementToCheck][10];
                            rankedButtonImageURL[placementToCheck] = placeLeaderboardData[placementToCheck][11];
                            rankedButtonPlacement[placementToCheck] = placeLeaderboardData[placementToCheck][12];
                            */

                            // Assign variables to text on leaderboard buttons
                            leaderboardButtonArray[placementToCheck].playernameText.text = placeLeaderboardData[placementToCheck][0];
                            leaderboardButtonArray[placementToCheck].scoreText.text = placeLeaderboardData[placementToCheck][1];
                            //leaderboardButtonArray[placementToCheck].feverScore.text = placeLeaderboardData[placementToCheck][2];
                            leaderboardButtonArray[placementToCheck].perfectJudgementText.text = perfectPrefix + placeLeaderboardData[placementToCheck][5];
                            leaderboardButtonArray[placementToCheck].goodJudgementText.text = goodPrefix + placeLeaderboardData[placementToCheck][6];
                            leaderboardButtonArray[placementToCheck].earlyJudgementText.text = earlyPrefix + placeLeaderboardData[placementToCheck][7];
                            leaderboardButtonArray[placementToCheck].missJudgementText.text = missPrefix + placeLeaderboardData[placementToCheck][8];
                            //leaderboardButtonArray[placementToCheck].dateText.text = placeLeaderboardData[placementToCheck][9];
                            //leaderboardButtonArray[placementToCheck].messageText.text = placeLeaderboardData[placementToCheck][10];
                            leaderboardButtonArray[placementToCheck].statText.text = "[ " + placeLeaderboardData[placementToCheck][4] + "% ] [ " +
                            placeLeaderboardData[placementToCheck][3] + "x ] [ " + placeLeaderboardData[placementToCheck][9] + " ]";

                            /*
                            // Check if a mod was used, update the text with mod or without mod text
                            if (string.IsNullOrEmpty(rankedButtonMod[placementToCheck]) == false)
                            {
                                leaderboardButtonArray[placementToCheck].statText.text = "[ " + rankedButtonPercentage[placementToCheck] + "% ] [ " +
                                rankedButtonCombo[placementToCheck] + "x ]";
                            }
                            else
                            {
                                leaderboardButtonArray[placementToCheck].statText.text = "[ " + rankedButtonPercentage[placementToCheck] + " ] [ " +
                                rankedButtonCombo[placementToCheck] + "x ] [ " + rankedButtonMod[placementToCheck] + " ]";
                            }
                            */

                            // Send parsed percentage to calculate grade
                            string grade = scriptManager.gradeManager.CalculateGrade(float.Parse(placeLeaderboardData[placementToCheck][4]));

                            // Update text
                            leaderboardButtonArray[placementToCheck].rankText.text = grade;

                            // Update color gradient
                            leaderboardButtonArray[placementToCheck].rankText.colorGradientPreset = scriptManager.uiColorManager.SetGradeColorGradient(grade);

                            // Update leaderboard profile border image color to grade color
                            leaderboardButtonArray[placementToCheck].profileBorderImage.color = scriptManager.uiColorManager.SetGradeColor(grade);

                            // Load player image
                            StartCoroutine(LoadPlayerImg(placeLeaderboardData[placementToCheck][11], placementToCheck));

                            // Activate leaderboard profile button
                            leaderboardButtonArray[placementToCheck].profileImage.gameObject.SetActive(true);
                        }

                        // If logged in
                        if (MySQLDBManager.loggedIn == true)
                        {
                            switch (hasPersonalBest)
                            {
                                case true:
                                    /*
                                    // Assign variables
                                    personalBestUsername = MySQLDBManager.username;
                                    personalBestScore = personalBestLeaderboardData[1];
                                    personalBestFeverScore = personalBestLeaderboardData[2];
                                    personalBestCombo = personalBestLeaderboardData[3];
                                    personalBestPercentage = personalBestLeaderboardData[4];
                                    personalBestPerfect = personalBestLeaderboardData[5];
                                    personalBestGood = personalBestLeaderboardData[6];
                                    personalBestEarly = personalBestLeaderboardData[7];
                                    personalBestMiss = personalBestLeaderboardData[8];
                                    personalBestDate = personalBestLeaderboardData[9];
                                    personalBestMessage = personalBestLeaderboardData[10];
                                    personalBestPlacement = personalBestLeaderboardData[11];
                                    */

                                    // Assign text
                                    personalBestButtonScript.playernameText.text = MySQLDBManager.username; ;
                                    personalBestButtonScript.scoreText.text = personalBestLeaderboardData[1]; ;
                                    personalBestButtonScript.statText.text = "[ " + personalBestLeaderboardData[4] + "% ] [ " +
                                        personalBestLeaderboardData[3] + "x ] [ " + personalBestLeaderboardData[9] + " ]";
                                    personalBestButtonScript.perfectJudgementText.text = perfectPrefix + personalBestLeaderboardData[5];
                                    personalBestButtonScript.goodJudgementText.text = goodPrefix + personalBestLeaderboardData[6];
                                    personalBestButtonScript.earlyJudgementText.text = earlyPrefix + personalBestLeaderboardData[7];
                                    personalBestButtonScript.missJudgementText.text = missPrefix + personalBestLeaderboardData[8];
                                    personalBestButtonScript.placementText.text = "Personal Best " + personalBestLeaderboardData[11]
                                        + "# of " + personalBestLeaderboardData[12] + "#";

                                    // Send parsed percentage to calculate grade
                                    string grade = scriptManager.gradeManager.CalculateGrade(float.Parse(personalBestLeaderboardData[4]));

                                    // Update text
                                    personalBestButtonScript.rankText.text = grade;

                                    // Update color gradient
                                    personalBestButtonScript.rankText.colorGradientPreset = scriptManager.uiColorManager.SetGradeColorGradient(grade);
                                    break;
                                case false:
                                    break;
                            }
                        }

                        // Set has loaded leaderboard to true to prevent the leaderbaord from continuing to upload every frame
                        hasLoadedLeaderboard = true;
                    }

                    // If leaderboard imformation has loaded
                    if (hasLoadedLeaderboard == true)
                    {
                        // If all images have been uploaded
                        if (totalImagesUpdated >= totalExistingPlaces && totalURLImagesUpdated >= totalURLImagesToLoad)
                        {
                            completeLeaderboardReady = true;
                        }
                    }

                    // Enable all leaderboard button containers if all information has been uploaded and all profile images have been uploaded
                    if (completeLeaderboardReady == true)
                    {
                        // If the first button is not active
                        if (leaderboardButtonArray[0].gameObject.activeSelf == false)
                        {
                            // Turn off loading icon
                            loadingIcon.gameObject.SetActive(false);

                            // Activate all leaderboard button containers
                            ActivateAllLeaderboardButtons();

                            // Play flash animation
                            PlayFullLeaderboardFlashAnimation();
                        }
                    }
                    break;
                case true:
                    // Increment timer
                    leaderboardFlashTimer += Time.deltaTime;

                    // If time is to play animation
                    if (leaderboardFlashTimer >= timeToNextLeaderboardFlash)
                    {
                        // Play animation
                        StartCoroutine(PlayLinearLeaderboardFlashAnimation());
                    }
                    break;
            }
        }
    }

    // Calculate total number of existing places on the leaderboard
    private void CalculateTotalExistingPlaces()
    {
        for (int i = 0; i < placeExists.Length; i++)
        {
            if (placeExists[i] == true)
            {
                totalExistingPlaces++;
            }
        }
    }

    // Play the linear flash animation
    private IEnumerator PlayLinearLeaderboardFlashAnimation()
    {
        leaderboardFlashTimer = 0f;

        for (int i = 0; i < totalRankingPlacements; i++)
        {
            leaderboardButtonArray[i].flashAnimationImage.gameObject.SetActive(false);
            leaderboardButtonArray[i].flashAnimationImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Play full animation
    private void PlayFullLeaderboardFlashAnimation()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            leaderboardButtonArray[i].flashAnimationImage.gameObject.SetActive(false);
            leaderboardButtonArray[i].flashAnimationImage.gameObject.SetActive(true);
        }

        personalBestButtonScript.flashAnimationImage.gameObject.SetActive(false);
        personalBestButtonScript.flashAnimationImage.gameObject.SetActive(true);
    }

    // Load the default image
    private void LoadDefaultMaterial(int _placement)
    {
        // Set the material to default
        leaderboardButtonArray[_placement].profileImage.material = defaultMaterial;

        // Increment
        totalImagesUpdated++;
    }

    // Load the player image
    IEnumerator LoadPlayerImg(string _url, int _placement)
    {
        if (_url != "")
        {
            if (_url != null)
            {
                // Increment total URL images to load
                totalURLImagesToLoad++;

                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        // Load the default image
                        LoadDefaultMaterial(_placement);

                        // Increment total url images updated
                        totalURLImagesUpdated++;
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);

                        // Update the image for the placement
                        leaderboardButtonArray[_placement].profileImage.material.mainTexture = texture;

                        // Increment images updated
                        totalImagesUpdated++;

                        // Increment total url images updated
                        totalURLImagesUpdated++;
                    }
                }
            }
            else
            {
                // Load the default image
                LoadDefaultMaterial(_placement);
            }
        }
        else
        {
            // Load the default image
            LoadDefaultMaterial(_placement);
        }
    }

    // Change beatmap ranking type sorting for leaderboard
    public void ChangeBeatmapRankingTypeSorting()
    {
        switch (leaderboardSortTypeDropdown.value)
        {
            case 0:
                scriptManager.messagePanel.DisplayMessage("BEATMAP RANKING [ SCORE ]", scriptManager.uiColorManager.purpleColor);
                break;
            case 1:
                scriptManager.messagePanel.DisplayMessage("BEATMAP RANKING [ FEVER SCORE ]", scriptManager.uiColorManager.purpleColor);
                break;
            case 2:
                scriptManager.messagePanel.DisplayMessage("BEATMAP RANKING [ ACCURACY ]", scriptManager.uiColorManager.purpleColor);
                break;
            case 3:
                scriptManager.messagePanel.DisplayMessage("BEATMAP RANKING [ COMBO ]", scriptManager.uiColorManager.purpleColor);
                break;
        }

        ResetLeaderboard();
        ResetNotChecked();
    }

    // Retrieve beatmap leaderboard placement information for the position passed
    public IEnumerator RetrieveBeatmapLeaderboard(int _leaderboardPlaceToGet)
    {
        WWWForm form = new WWWForm();
        form.AddField("leaderboardTableName", leaderboardTableName);
        form.AddField("leaderboardPlaceToGet", _leaderboardPlaceToGet);
        form.AddField("sorting", leaderboardSortTypeDropdown.value);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_player_beatmap_ranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Stores all the placement information from the database
        ArrayList placeList = new ArrayList();

        // Split the information retrieved from the database
        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        // Loop through all the leaderboard data and assign
        for (sbyte dataType = 0; dataType < 12; dataType++)
        {
            /*
              DataType:
              $returnarray[0] = $infoarray["username"];
              $returnarray[1] = $infoarray["score"];
              $returnarray[2] = $infoarray["fever_score"];
              $returnarray[3] = $infoarray["combo"];
              $returnarray[4] = $infoarray["accuracy"];
              $returnarray[5] = $infoarray["perfect"];
              $returnarray[6] = $infoarray["good"];
              $returnarray[7] = $infoarray["early"];
              $returnarray[8] = $infoarray["miss"];
              $returnarray[9] = $infoarray["date"];
              $returnarray[10] = $infoarray["message"];
              $returnarray[11] = $infoarray["image_url"];
            */

            switch (www.downloadHandler.text)
            {
                case "ERROR":
                    // ERROR - NO LEADERBOARD DATA FOR THIS PLACEMENT
                    placeExists[_leaderboardPlaceToGet - 1] = false;
                    break;
                default:
                    // SUCCESS - LEADERBOARD DATA FOUND FOR THIS PLACEMENT
                    placeLeaderboardData[_leaderboardPlaceToGet - 1].Add(placeList[dataType].ToString());
                    placeExists[_leaderboardPlaceToGet - 1] = true;
                    break;
            }
        }

        totalPlacesChecked++;
        placeChecked[_leaderboardPlaceToGet - 1] = true;
    }

    // Get the leaderboard table name to load the beatmap leaderboard from
    public void GetLeaderboardTableName()
    {
        // Get it from the database gameobject
        leaderboardTableName = Database.database.LoadedLeaderboardTableName;
    }

    // Retrieve the users personal best score
    public IEnumerator RetrievePersonalBest()
    {
        // Check if the user has logged in
        if (MySQLDBManager.loggedIn)
        {
            WWWForm form = new WWWForm();
            form.AddField("leaderboardTableName", leaderboardTableName);
            form.AddField("username", MySQLDBManager.username);

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_personalbest_beatmap_ranking.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            // Stores personal best leaderboard data from the database
            ArrayList placeList = new ArrayList();

            // Splits the data retrieved
            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

            // Loop through all the data retrieved and assign to the personal best leaderboard data list
            for (int dataType = 0; dataType < 13; dataType++)
            {
                switch (www.downloadHandler.text)
                {
                    case "ERROR":
                        // ERROR - NO PERSONAL DATA FOUND
                        break;
                    default:
                        // SUCCESS - PERSONAL BEST DATA FOUND
                        personalBestLeaderboardData.Add(placeList[dataType].ToString());
                        hasPersonalBest = true;
                        hasCheckedPersonalBest = true;
                        break;
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
            // Clear the leaderboard data stattext
            placeLeaderboardData[placementToCheck].Clear();
            // Reset all places that exist
            placeExists[placementToCheck] = false;
            // Reset all places checked
            placeChecked[placementToCheck] = false;

            // Null all text
            leaderboardButtonArray[placementToCheck].playernameText.text = "";
            leaderboardButtonArray[placementToCheck].scoreText.text = "";
            leaderboardButtonArray[placementToCheck].statText.text = "";
            leaderboardButtonArray[placementToCheck].perfectJudgementText.text = "";
            leaderboardButtonArray[placementToCheck].goodJudgementText.text = "";
            leaderboardButtonArray[placementToCheck].earlyJudgementText.text = "";
            leaderboardButtonArray[placementToCheck].missJudgementText.text = "";
            leaderboardButtonArray[placementToCheck].rankText.text = "";
        }

        // Deactivate all leaderboard buttons
        DeactivateAllLeaderboardButtons();

        // Reset personal best text
        personalBestButtonScript.playernameText.text = "";
        personalBestButtonScript.scoreText.text = "";
        personalBestButtonScript.statText.text = "";
        personalBestButtonScript.perfectJudgementText.text = "";
        personalBestButtonScript.goodJudgementText.text = "";
        personalBestButtonScript.earlyJudgementText.text = "";
        personalBestButtonScript.missJudgementText.text = "";
        personalBestButtonScript.rankText.text = "";
        personalBestButtonScript.placementText.text = "";
        // Reset personal best leaderboard data
        personalBestLeaderboardData.Clear();

        // Reset the leaderboard loading animation
        hasLoadedLeaderboard = false;

        // Reset total url images updated
        totalURLImagesUpdated = 0;

        // Reset scroll bar
        leaderboardScrollbar.value = 1;

        // Turn on loading icon
        loadingIcon.gameObject.SetActive(true);

        // Reset timer
        leaderboardFlashTimer = 0;
    }

    // Reset the leaderboard checking variables
    public void ResetNotChecked()
    {
        // Reset leaderboard checking
        notChecked = true;
        hasPersonalBest = false;
        hasCheckedPersonalBest = false;
        leaderboardPlaceToGet = 1;
        totalPlacesChecked = 0;
        totalImagesUpdated = 0;
        totalExistingPlaces = 0;
        totalURLImagesToLoad = 0;
    }

    // Deactivate all leaderboard buttons
    private void DeactivateAllLeaderboardButtons()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            leaderboardButtonArray[i].gameObject.SetActive(false);
        }

        personalBestButton.gameObject.SetActive(false);
        leaderboardScrollbarCanvasGroup.alpha = 0;
        leaderboardScrollbar.interactable = false;
        completeLeaderboardReady = false;
    }

    private IEnumerator ActivateAllLeaderboardButtonsAnimation()
    {
        for (int i = 0; i < leaderboardButtonArray.Length; i++)
        {
            leaderboardButtonArray[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(0.02f);
        }

        personalBestButton.gameObject.SetActive(true);
        personalBestButtonTextAnimator.Play("SelectedBeatmapNumberText_Animation", 0, 0f);
    }

    // Activate all leaderboard buttons
    private void ActivateAllLeaderboardButtons()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            switch (placeExists[i])
            {
                case true:
                    leaderboardButtonArray[i].contentPanel.gameObject.SetActive(true);
                    leaderboardButtonArray[i].noRecordSetText.gameObject.SetActive(false);
                    leaderboardButtonArray[i].leaderboardButton.interactable = true;
                    break;
                case false:
                    leaderboardButtonArray[i].contentPanel.gameObject.SetActive(false);
                    leaderboardButtonArray[i].noRecordSetText.gameObject.SetActive(true);
                    leaderboardButtonArray[i].leaderboardButton.interactable = false;
                    break;
            }
        }

        switch (hasPersonalBest)
        {
            case true:
                personalBestButtonScript.contentPanel.gameObject.SetActive(true);
                personalBestButtonScript.noRecordSetText.gameObject.SetActive(false);
                personalBestButton.interactable = true;
                break;
            case false:
                personalBestButtonScript.contentPanel.gameObject.SetActive(false);
                personalBestButtonScript.noRecordSetText.gameObject.SetActive(true);
                personalBestButton.interactable = false;
                break;
        }

        // Turn on scroll functionality
        leaderboardScrollbar.interactable = true;

        // Update scrollbar canvas group
        leaderboardScrollbarCanvasGroup.alpha = 1;

        // Play activation animation
        StartCoroutine(ActivateAllLeaderboardButtonsAnimation());
    }
    #endregion
}
