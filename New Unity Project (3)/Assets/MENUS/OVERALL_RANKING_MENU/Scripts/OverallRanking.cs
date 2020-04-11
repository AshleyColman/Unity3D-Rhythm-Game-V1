using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System;

public class OverallRanking : MonoBehaviour
{
    #region Variables
    // Button
    public Button personalBestButton;

    // Dropdown
    public TMP_Dropdown overallRankingDropDown;

    // Gameobject
    public GameObject loadingIcon;

    // Transform
    public Transform leaderboardContentTransform;

    // Scrollbar
    public Scrollbar leaderboardScrollbar;

    // Canvas group
    public CanvasGroup leaderboardScrollbarCanvasGroup;

    // Text
    public TextMeshProUGUI overallRankingText;

    // Bool
    private bool hasChecked, hasPersonalBest, hasLoadedLeaderboard, hasLoadedImages, hasCheckedPersonalBest, completeLeaderboardReady;
    private bool[] placeExists;

    // String
    private List<string>[] placeLeaderboardData = new List<string>[50];
    private List<string> personalBestLeaderboardData = new List<string>();
    private string player_id;
    private const string RANKING_CAREER_SCORE = "career_score", TOTAL_SCORE = "total_score", CAREER_COMBO = "career_combo", CAREER_EASY_SCORE =
        "career_easy_score", CAREER_NORMAL_SCORE = "career_normal_score", CAREER_HARD_SCORE = "career_hard_score", TOTAL_EASY_SCORE = "total_easy_score",
        TOTAL_NORMAL_SCORE = "total_normal_score", TOTAL_HARD_SCORE = "total_hard_score", TOTAL_COMBO = "total_combo", CAREER_PERFECT = "career_perfect",
        CAREER_GOOD = "career_good", CAREER_EARLY = "career_early", CAREER_MISS = "career_miss", TOTAL_PERFECT = "total_perfect",
        TOTAL_GOOD = "total_good", TOTAL_EARLY = "total_early", TOTAL_MISS = "total_miss", CAREER_NOTES_HIT = "career_notes_hit", TOTAL_NOTES_HIT =
        "total_notes_hit", HIGHEST_COMBO = "highest_combo", TOTAL_PLAY_COUNT = "total_play_count", CAREER_SPLUS_RANK = "career_s+_rank",
        CAREER_S_RANK = "career_s_rank", CAREER_A_RANK = "career_a_rank", CAREER_B_RANK = "career_b_rank", CAREER_C_RANK = "career_c_rank",
        CAREER_D_RANK = "career_d_rank", CAREER_E_RANK = "career_e_rank", CAREER_F_RANK = "career_f_rank", TOTAL_SPLUS_RANK = "total_s+_rank",
        TOTAL_S_RANK = "total_s_rank", TOTAL_A_RANK = "total_a_rank", TOTAL_B_RANK = "total_b_rank", TOTAL_C_RANK = "total_c_rank",
        TOTAL_D_RANK = "total_d_rank", TOTAL_E_RANK = "total_e_rank", TOTAL_F_RANK = "total_f_rank", LEVEL = "level";
    private const string DATE_JOINED_PREFIX = "DATE JOINED: ", PLAY_TIME_PREFIX = "PLAY TIME: ", PLAY_COUNT_PREFIX = "PLAYS: ", TOTAL_SPLUS_RANK_PREFIX = "S+: ",
        TOTAL_S_RANK_PREFIX = "S: ", TOTAL_A_RANK_PREFIX = "A: ", TOTAL_B_RANK_PREFIX = "B: ", TOTAL_C_RANK_PREFIX = "C: ";

    // Integer
    public int leaderboardPlaceToGet, totalPlacementsChecked, totalRankingPlacements, totalExistingPlacements, totalImagesUpdated, totalURLImagesUpdated,
        totalURLImagesToLoad, timeToNextLeaderboardFlash;
    private float leaderboardFlashTimer;

    // Scripts
    public OverallLeaderboardButton[] overallLeaderboardButtonArray = new OverallLeaderboardButton[50];
    public OverallLeaderboardButton personalBestLeaderboardButtonScript;
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    #endregion

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        totalPlacementsChecked = 0;
        totalExistingPlacements = 0;
        totalRankingPlacements = 10;
        leaderboardPlaceToGet = 1;
        totalImagesUpdated = 0;
        totalURLImagesUpdated = 0;
        totalURLImagesToLoad = 0;
        timeToNextLeaderboardFlash = 2;
        leaderboardFlashTimer = 0f;
        hasPersonalBest = false;
        hasLoadedLeaderboard = false;
        hasLoadedImages = false;
        hasChecked = false;
        completeLeaderboardReady = false;
        placeExists = new bool[totalRankingPlacements];

        // Instantiate the lists
        for (int i = 0; i < placeLeaderboardData.Length; i++)
        {
            placeLeaderboardData[i] = new List<string>();
        }

        // Get reference
        personalBestLeaderboardButtonScript = personalBestButton.GetComponent<OverallLeaderboardButton>();

        // Get all leaderboard button references
        GetLeaderboardButtonReferences();

        // Reset leaderboard
        ResetLeaderboard();
    }

    void Update()
    {
        switch (scriptManager.menuManager.overallRankingMenu.gameObject.activeSelf)
        {
            case true:
                switch (completeLeaderboardReady)
                {
                    case true:
                        // Increment timer
                        leaderboardFlashTimer += Time.deltaTime;

                        // If time to flash leaderboard
                        if (leaderboardFlashTimer >= timeToNextLeaderboardFlash)
                        {
                            // Play animation
                            StartCoroutine(PlayLinearLeaderboardFlashAnimation());
                        }
                        break;
                    case false:
                        if (hasChecked == false && hasCheckedPersonalBest == false)
                        {
                            for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
                            {
                                // Retrieve top players
                                StartCoroutine(RetrieveOverallRankingLeaderboard(leaderboardPlaceToGet));
                                // Increment the placement to get
                                leaderboardPlaceToGet++;
                            }

                            StartCoroutine(RetrievePersonalBest());

                            // Set to true to prevent looping
                            hasChecked = true;
                        }

                        if (hasChecked == true && hasCheckedPersonalBest == true && totalPlacementsChecked == 
                            totalRankingPlacements)
                        {
                            switch (hasLoadedLeaderboard)
                            {
                                case true:
                                    // If all images have been uploaded
                                    if (totalImagesUpdated >= totalExistingPlacements && totalURLImagesUpdated >= totalURLImagesToLoad)
                                    {
                                        // Disable loading icon
                                        loadingIcon.gameObject.SetActive(false);

                                        // Activate leaderboard
                                        ActivateAllLeaderboardButtons();

                                        // Play animation
                                        PlayFullLeaderboardFlashAnimation();

                                        // Set true
                                        completeLeaderboardReady = true;
                                    }
                                    break;
                                case false:
                                    // Calculate how many leaderboard placements exist
                                    CalculateTotalExistingPlacements();

                                    // Update the leaderboard button information
                                    UpdateLeaderboardButtonInformation();

                                    // Set to true as leaderboard information has loaded
                                    hasLoadedLeaderboard = true;
                                    break;
                            }
                        }
                        break;
                }
                break;
        }
    }

    public IEnumerator RetrieveOverallRankingLeaderboard(int _leaderboardPlaceToGet)
    {
        WWWForm form = new WWWForm();

        switch (overallRankingDropDown.value)
        {
            case 0:
                form.AddField("sorting", RANKING_CAREER_SCORE);
                break;
            case 1:
                form.AddField("sorting", TOTAL_SCORE);
                break;
            case 2:
                form.AddField("sorting", CAREER_COMBO);
                break;
            case 3:
                form.AddField("sorting", CAREER_EASY_SCORE);
                break;
            case 4:
                form.AddField("sorting", CAREER_NORMAL_SCORE);
                break;
            case 5:
                form.AddField("sorting", CAREER_HARD_SCORE);
                break;
            case 6:
                form.AddField("sorting", TOTAL_EASY_SCORE);
                break;
            case 7:
                form.AddField("sorting", TOTAL_NORMAL_SCORE);
                break;
            case 8:
                form.AddField("sorting", TOTAL_HARD_SCORE);
                break;
            case 9:
                form.AddField("sorting", TOTAL_COMBO);
                break;
            case 10:
                form.AddField("sorting", CAREER_PERFECT);
                break;
            case 11:
                form.AddField("sorting", CAREER_GOOD);
                break;
            case 12:
                form.AddField("sorting", CAREER_EARLY);
                break;
            case 13:
                form.AddField("sorting", CAREER_MISS);
                break;
            case 14:
                form.AddField("sorting", TOTAL_PERFECT);
                break;
            case 15:
                form.AddField("sorting", TOTAL_GOOD);
                break;
            case 16:
                form.AddField("sorting", TOTAL_EARLY);
                break;
            case 17:
                form.AddField("sorting", TOTAL_MISS);
                break;
            case 18:
                form.AddField("sorting", CAREER_NOTES_HIT);
                break;
            case 19:
                form.AddField("sorting", TOTAL_NOTES_HIT);
                break;
            case 20:
                form.AddField("sorting", HIGHEST_COMBO);
                break;
            case 21:
                form.AddField("sorting", TOTAL_PLAY_COUNT);
                break;
            case 22:
                form.AddField("sorting", CAREER_SPLUS_RANK);
                break;
            case 23:
                form.AddField("sorting", CAREER_S_RANK);
                break;
            case 24:
                form.AddField("sorting", CAREER_A_RANK);
                break;
            case 25:
                form.AddField("sorting", CAREER_B_RANK);
                break;
            case 26:
                form.AddField("sorting", CAREER_C_RANK);
                break;
            case 27:
                form.AddField("sorting", CAREER_D_RANK);
                break;
            case 28:
                form.AddField("sorting", CAREER_E_RANK);
                break;
            case 29:
                form.AddField("sorting", CAREER_F_RANK);
                break;
            case 30:
                form.AddField("sorting", TOTAL_SPLUS_RANK);
                break;
            case 31:
                form.AddField("sorting", TOTAL_S_RANK);
                break;
            case 32:
                form.AddField("sorting", TOTAL_A_RANK);
                break;
            case 33:
                form.AddField("sorting", TOTAL_B_RANK);
                break;
            case 34:
                form.AddField("sorting", TOTAL_C_RANK);
                break;
            case 35:
                form.AddField("sorting", TOTAL_D_RANK);
                break;
            case 36:
                form.AddField("sorting", TOTAL_E_RANK);
                break;
            case 37:
                form.AddField("sorting", TOTAL_F_RANK);
                break;
            case 38:
                form.AddField("sorting", LEVEL);
                break;
        }

        form.AddField("leaderboardPlaceToGet", _leaderboardPlaceToGet);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_overall_ranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        ArrayList placeList = new ArrayList();

        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        for (int dataType = 0; dataType < 11; dataType++)
        {
            /*
              DataType:
              $returnarray[0] = $infoarray["username"];
              $returnarray[1] = $infoarray["image_url"];
              $returnarray[2] = $infoarray["play_count"];
              $returnarray[3] = $infoarray["play_time"];
              $returnarray[4] = $infoarray["career_s+_rank"];
              $returnarray[5] = $infoarray["career_s_rank"];
              $returnarray[6] = $infoarray["career_a_rank"];
              $returnarray[7] = $infoarray["career_b_rank"];
              $returnarray[8] = $infoarray["career_c_rank"];
              $returnarray[9] = $infoarray[$sorting];
              $returnarray[10] = $infoarray["date_joined"];
            */

            switch (www.downloadHandler.text)
            {
                case "ERROR":
                    placeExists[_leaderboardPlaceToGet - 1] = false;
                    break;
                default:
                    placeLeaderboardData[_leaderboardPlaceToGet - 1].Add(placeList[dataType].ToString());
                    placeExists[_leaderboardPlaceToGet - 1] = true;
                    break;
            }
        }

        // Increment total placements checked
        totalPlacementsChecked++;
    }

    // Retrieve the users personal best score
    public IEnumerator RetrievePersonalBest()
    {
        if (MySQLDBManager.loggedIn == true)
        {
            WWWForm form = new WWWForm();

            // Send sorting based on drop down value
            switch (overallRankingDropDown.value)
            {
                case 0:
                    form.AddField("sorting", RANKING_CAREER_SCORE);
                    break;
                case 1:
                    form.AddField("sorting", TOTAL_SCORE);
                    break;
                case 2:
                    form.AddField("sorting", CAREER_COMBO);
                    break;
                case 3:
                    form.AddField("sorting", CAREER_EASY_SCORE);
                    break;
                case 4:
                    form.AddField("sorting", CAREER_NORMAL_SCORE);
                    break;
                case 5:
                    form.AddField("sorting", CAREER_HARD_SCORE);
                    break;
                case 6:
                    form.AddField("sorting", TOTAL_EASY_SCORE);
                    break;
                case 7:
                    form.AddField("sorting", TOTAL_NORMAL_SCORE);
                    break;
                case 8:
                    form.AddField("sorting", TOTAL_HARD_SCORE);
                    break;
                case 9:
                    form.AddField("sorting", TOTAL_COMBO);
                    break;
                case 10:
                    form.AddField("sorting", CAREER_PERFECT);
                    break;
                case 11:
                    form.AddField("sorting", CAREER_GOOD);
                    break;
                case 12:
                    form.AddField("sorting", CAREER_EARLY);
                    break;
                case 13:
                    form.AddField("sorting", CAREER_MISS);
                    break;
                case 14:
                    form.AddField("sorting", TOTAL_PERFECT);
                    break;
                case 15:
                    form.AddField("sorting", TOTAL_GOOD);
                    break;
                case 16:
                    form.AddField("sorting", TOTAL_EARLY);
                    break;
                case 17:
                    form.AddField("sorting", TOTAL_MISS);
                    break;
                case 18:
                    form.AddField("sorting", CAREER_NOTES_HIT);
                    break;
                case 19:
                    form.AddField("sorting", TOTAL_NOTES_HIT);
                    break;
                case 20:
                    form.AddField("sorting", HIGHEST_COMBO);
                    break;
                case 21:
                    form.AddField("sorting", TOTAL_PLAY_COUNT);
                    break;
                case 22:
                    form.AddField("sorting", CAREER_SPLUS_RANK);
                    break;
                case 23:
                    form.AddField("sorting", CAREER_S_RANK);
                    break;
                case 24:
                    form.AddField("sorting", CAREER_A_RANK);
                    break;
                case 25:
                    form.AddField("sorting", CAREER_B_RANK);
                    break;
                case 26:
                    form.AddField("sorting", CAREER_C_RANK);
                    break;
                case 27:
                    form.AddField("sorting", CAREER_D_RANK);
                    break;
                case 28:
                    form.AddField("sorting", CAREER_E_RANK);
                    break;
                case 29:
                    form.AddField("sorting", CAREER_F_RANK);
                    break;
                case 30:
                    form.AddField("sorting", TOTAL_SPLUS_RANK);
                    break;
                case 31:
                    form.AddField("sorting", TOTAL_S_RANK);
                    break;
                case 32:
                    form.AddField("sorting", TOTAL_A_RANK);
                    break;
                case 33:
                    form.AddField("sorting", TOTAL_B_RANK);
                    break;
                case 34:
                    form.AddField("sorting", TOTAL_C_RANK);
                    break;
                case 35:
                    form.AddField("sorting", TOTAL_D_RANK);
                    break;
                case 36:
                    form.AddField("sorting", TOTAL_E_RANK);
                    break;
                case 37:
                    form.AddField("sorting", TOTAL_F_RANK);
                    break;
                case 38:
                    form.AddField("sorting", LEVEL);
                    break;
            }
            form.AddField("username", MySQLDBManager.username);

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_personal_best_overall_ranking.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            ArrayList placeList = new ArrayList();

            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));


            for (int dataType = 0; dataType < 9; dataType++)
            {
                /*
                  DataType:
                  $returnarray[0] = $infoarray["play_count"];
                  $returnarray[1] = $infoarray["play_time"];
                  $returnarray[2] = $infoarray["career_s+_rank"];
                  $returnarray[3] = $infoarray["career_s_rank"];
                  $returnarray[4] = $infoarray["career_a_rank"];
                  $returnarray[5] = $infoarray["career_b_rank"];
                  $returnarray[6] = $infoarray["career_c_rank"];
                  $returnarray[7] = $infoarray[$sorting];
                  $returnarray[8] = $infoarray["date_joined"];
                */

                switch (www.downloadHandler.text)
                {
                    case "ERROR":
                        break;
                    default:
                        personalBestLeaderboardData.Add(placeList[dataType].ToString());
                        hasPersonalBest = true;
                        hasCheckedPersonalBest = true;
                        break;
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

    // Deactivate all leaderboard buttons
    private void DeactivateAllLeaderboardButtons()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].gameObject.SetActive(false);
        }

        personalBestButton.gameObject.SetActive(false);

        // Hide scrollbar 
        leaderboardScrollbarCanvasGroup.alpha = 0;
        leaderboardScrollbar.interactable = false;
    }

    // Activate all leaderboard buttons animation
    private IEnumerator ActivateAllLeaderboardButtonsAnimation()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(0.02f);
        }

        personalBestButton.gameObject.SetActive(true);
    }


    // Activate all leaderboard buttons
    private void ActivateAllLeaderboardButtons()
    {
        // Leaderboard buttons
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            switch (placeExists[i])
            {
                case true:
                    overallLeaderboardButtonArray[i].contentPanel.gameObject.SetActive(true);
                    overallLeaderboardButtonArray[i].noRecordSetText.gameObject.SetActive(false);
                    overallLeaderboardButtonArray[i].leaderboardButton.interactable = true;
                    break;
                case false:
                    overallLeaderboardButtonArray[i].contentPanel.gameObject.SetActive(false);
                    overallLeaderboardButtonArray[i].noRecordSetText.gameObject.SetActive(true);
                    overallLeaderboardButtonArray[i].leaderboardButton.interactable = false;
                    break;
            }
        }

        // Personal best button
        switch (hasPersonalBest)
        {
            case true:
                personalBestLeaderboardButtonScript.contentPanel.gameObject.SetActive(true);
                personalBestLeaderboardButtonScript.noRecordSetText.gameObject.SetActive(false);
                personalBestLeaderboardButtonScript.leaderboardButton.interactable = true;
                break;
            case false:
                personalBestLeaderboardButtonScript.contentPanel.gameObject.SetActive(false);
                personalBestLeaderboardButtonScript.noRecordSetText.gameObject.SetActive(true);
                personalBestLeaderboardButtonScript.leaderboardButton.interactable = false;
                break;
        }

        // Hide scrollbar 
        leaderboardScrollbarCanvasGroup.alpha = 1;
        leaderboardScrollbar.interactable = true;

        // Play activation animation
        StartCoroutine(ActivateAllLeaderboardButtonsAnimation());
    }

    // Update the leaderboard button text information
    private void UpdateLeaderboardButtonInformation()
    {
        /*
            DataType:
            $returnarray[0] = $infoarray["username"];
            $returnarray[1] = $infoarray["image_url"];
            $returnarray[2] = $infoarray["play_count"];
            $returnarray[3] = $infoarray["play_time"];
            $returnarray[4] = $infoarray["career_s+_rank"];
            $returnarray[5] = $infoarray["career_s_rank"];
            $returnarray[6] = $infoarray["career_a_rank"];
            $returnarray[7] = $infoarray["career_b_rank"];
            $returnarray[8] = $infoarray["career_c_rank"];
            $returnarray[9] = $infoarray[$sorting];
            $returnarray[10] = $infoarray["date_joined"];
        */

        for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
        {
            if (placeExists[placementToCheck] == true)
            {
                // Update text for button
                overallLeaderboardButtonArray[placementToCheck].playernameText.text = placeLeaderboardData[placementToCheck][0];
                overallLeaderboardButtonArray[placementToCheck].playCountText.text = PLAY_COUNT_PREFIX + placeLeaderboardData[placementToCheck][2];
                overallLeaderboardButtonArray[placementToCheck].playTimeText.text = PLAY_TIME_PREFIX + 
                    CalculateDaysHoursMinutes(float.Parse(placeLeaderboardData[placementToCheck][3]));
                overallLeaderboardButtonArray[placementToCheck].sPlusRankText.text = TOTAL_SPLUS_RANK_PREFIX + placeLeaderboardData[placementToCheck][4];
                overallLeaderboardButtonArray[placementToCheck].sRankText.text = TOTAL_S_RANK_PREFIX + placeLeaderboardData[placementToCheck][5];
                overallLeaderboardButtonArray[placementToCheck].aRankText.text = TOTAL_A_RANK_PREFIX + placeLeaderboardData[placementToCheck][6];
                overallLeaderboardButtonArray[placementToCheck].bRankText.text = TOTAL_B_RANK_PREFIX + placeLeaderboardData[placementToCheck][7];
                overallLeaderboardButtonArray[placementToCheck].cRankText.text = TOTAL_C_RANK_PREFIX + placeLeaderboardData[placementToCheck][8];
                overallLeaderboardButtonArray[placementToCheck].sortingText.text = placeLeaderboardData[placementToCheck][9];
                overallLeaderboardButtonArray[placementToCheck].dateJoinedText.text = DATE_JOINED_PREFIX + placeLeaderboardData[placementToCheck][10];

                // LOAD PLAYER IMAGE
                StartCoroutine(LoadPlayerImg(placeLeaderboardData[placementToCheck][1], placementToCheck));
            }
        }

        /*
            DataType:
            $returnarray[0] = $infoarray["play_count"];
            $returnarray[1] = $infoarray["play_time"];
            $returnarray[2] = $infoarray["career_s+_rank"];
            $returnarray[3] = $infoarray["career_s_rank"];
            $returnarray[4] = $infoarray["career_a_rank"];
            $returnarray[5] = $infoarray["career_b_rank"];
            $returnarray[6] = $infoarray["career_c_rank"];
            $returnarray[7] = $infoarray[$sorting];
            $returnarray[8] = $infoarray["date_joined"];
        */

        // Update personal best information
        if (hasPersonalBest == true)
        {
            // Update text
            personalBestLeaderboardButtonScript.playernameText.text = MySQLDBManager.username;
            personalBestLeaderboardButtonScript.playCountText.text = PLAY_COUNT_PREFIX + personalBestLeaderboardData[0];
            personalBestLeaderboardButtonScript.playTimeText.text = PLAY_TIME_PREFIX +
                CalculateDaysHoursMinutes(float.Parse(personalBestLeaderboardData[1]));
            personalBestLeaderboardButtonScript.sPlusRankText.text = TOTAL_SPLUS_RANK_PREFIX + personalBestLeaderboardData[2];
            personalBestLeaderboardButtonScript.sRankText.text = TOTAL_S_RANK_PREFIX + personalBestLeaderboardData[3];
            personalBestLeaderboardButtonScript.aRankText.text = TOTAL_A_RANK_PREFIX + personalBestLeaderboardData[4];
            personalBestLeaderboardButtonScript.bRankText.text = TOTAL_B_RANK_PREFIX + personalBestLeaderboardData[5];
            personalBestLeaderboardButtonScript.cRankText.text = TOTAL_C_RANK_PREFIX + personalBestLeaderboardData[6];
            personalBestLeaderboardButtonScript.sortingText.text = personalBestLeaderboardData[7];
            personalBestLeaderboardButtonScript.dateJoinedText.text = DATE_JOINED_PREFIX + personalBestLeaderboardData[8];
        }
    }

    // Load all leaderboard player images
    private void LoadLeaderboardPlayerImages()
    {
        for (int placementToCheck = 0; placementToCheck < totalExistingPlacements; placementToCheck++)
        {
            // Load the player image (passing the URL and index)
            StartCoroutine(LoadPlayerImg(placeLeaderboardData[placementToCheck][5], placementToCheck));
        }

        // Set to true as all images have been loaded
        hasLoadedImages = true;
    }

    // Load the default image
    private void LoadDefaultMaterial(int _placement)
    {
        // Set the material to default
        overallLeaderboardButtonArray[_placement].profileImage.material = scriptManager.beatmapRanking.defaultMaterial;

        // Set image to false then to true to activate new material
        overallLeaderboardButtonArray[_placement].profileImage.gameObject.SetActive(false);
        overallLeaderboardButtonArray[_placement].profileImage.gameObject.SetActive(true);

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
                // Increment total url images to upload
                totalURLImagesToLoad++;

                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        // Increment total URL images to load
                        totalURLImagesToLoad++;

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
                        overallLeaderboardButtonArray[_placement].profileImage.material.mainTexture = texture;

                        // Increment
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

    // Play the linear flash animation
    private IEnumerator PlayLinearLeaderboardFlashAnimation()
    {
        leaderboardFlashTimer = 0f;

        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(false);
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Play full animation
    private void PlayFullLeaderboardFlashAnimation()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(false);
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(true);
        }

        personalBestLeaderboardButtonScript.flashAnimator.gameObject.SetActive(false);
        personalBestLeaderboardButtonScript.flashAnimator.gameObject.SetActive(true);
    }

    // Calculate how many leaderboard placements exist
    private void CalculateTotalExistingPlacements()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            if (placeExists[i] == true)
            {
                totalExistingPlacements++;
            }
        }
    }

    // Convert the seconds to days hours minutes
    public string CalculateDaysHoursMinutes(float _secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(_secs);

        string time = string.Format("{0:D2}d {1:D2}h {2:D2}m {3:D3}s",
                        t.Days,
                        t.Hours,
                        t.Minutes,
                        t.Seconds);

        return time;
    }

    // Get all leaderboard button references
    private void GetLeaderboardButtonReferences()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            // Get button script reference
            overallLeaderboardButtonArray[i] = leaderboardContentTransform.GetChild(i).GetComponent<OverallLeaderboardButton>();

            // Update placement text
            overallLeaderboardButtonArray[i].placementText.text = (i + 1).ToString() + "#";

            // Create and assign new profile image material
            overallLeaderboardButtonArray[i].profileImage.material = new Material(Shader.Find("UI/Unlit/Transparent"));
        }
    }

    // Reset all button information
    private void ResetAllButtonInformation()
    {
        // Reset leaderboard button text
        for (int placementToCheck = 0; placementToCheck < totalExistingPlacements; placementToCheck++)
        {
            overallLeaderboardButtonArray[placementToCheck].playernameText.text = "";
            overallLeaderboardButtonArray[placementToCheck].playCountText.text = "";
            overallLeaderboardButtonArray[placementToCheck].playTimeText.text = "";
            overallLeaderboardButtonArray[placementToCheck].sPlusRankText.text = "";
            overallLeaderboardButtonArray[placementToCheck].sRankText.text = "";
            overallLeaderboardButtonArray[placementToCheck].aRankText.text = "";
            overallLeaderboardButtonArray[placementToCheck].bRankText.text = "";
            overallLeaderboardButtonArray[placementToCheck].cRankText.text = "";
            overallLeaderboardButtonArray[placementToCheck].sortingText.text = "";
            overallLeaderboardButtonArray[placementToCheck].dateJoinedText.text = "";
        }

        // Reset personal best button text
        personalBestLeaderboardButtonScript.playernameText.text = "";
        personalBestLeaderboardButtonScript.playCountText.text = "";
        personalBestLeaderboardButtonScript.playTimeText.text = "";
        personalBestLeaderboardButtonScript.sPlusRankText.text = "";
        personalBestLeaderboardButtonScript.sRankText.text = "";
        personalBestLeaderboardButtonScript.aRankText.text = "";
        personalBestLeaderboardButtonScript.bRankText.text = "";
        personalBestLeaderboardButtonScript.cRankText.text = "";
        personalBestLeaderboardButtonScript.sortingText.text = "";
        personalBestLeaderboardButtonScript.dateJoinedText.text = "";
    }

    // Reset leaderboard
    public void ResetLeaderboard()
    {
        // Stop all coroutines 
        StopAllCoroutines();

        // Turn off scrollbar
        leaderboardScrollbar.interactable = false;
        leaderboardScrollbarCanvasGroup.alpha = 0;

        // Deactivate all buttons
        DeactivateAllLeaderboardButtons();

        // Reset all button information
        ResetAllButtonInformation();

        hasCheckedPersonalBest = false;
        hasPersonalBest = false;
        hasLoadedLeaderboard = false;
        completeLeaderboardReady = false;
        hasLoadedImages = false;
        hasChecked = false;
        personalBestLeaderboardData.Clear();
        player_id = "";
        totalPlacementsChecked = 0;
        leaderboardPlaceToGet = 1;
        totalImagesUpdated = 0;
        totalExistingPlacements = 0;
        totalURLImagesUpdated = 0;
        totalURLImagesToLoad = 0;

        for (int i = 0; i < totalRankingPlacements; i++)
        {
            placeLeaderboardData[i].Clear();
            placeExists[i] = false;
        }

        loadingIcon.gameObject.SetActive(true);
    }
}