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
    public GameObject leaderboard;
    public TextMeshProUGUI overallRankingText;

    private bool hasChecked;
    private bool hasPersonalBest;
    private bool hasLoadedLeaderboard;
    private bool hasLoadedImages;
    private bool playLeaderboardFlashAnimation;
    private bool playFullLeaderboardFlashAnimation;
    private bool fullLeaderboardFlashAnimationFinished;
    private List<string>[] placeLeaderboardData = new List<string>[50];
    private List<string> personalBestLeaderboardData = new List<string>();
    public int leaderboardPlaceToGet;
    public int totalPlacementsChecked;

    public Transform leaderboardContentTransform;
    public OverallLeaderboardButton[] overallLeaderboardButtonArray = new OverallLeaderboardButton[50];
    public OverallLeaderboardButton personalBestLeaderboardButtonScript;
    public Button personalBestButton;

    public TMP_Dropdown overallRankingDropDown;

    private bool[] placeExists = new bool[50];

    private bool hasCheckedPersonalBest = false;

    private string player_id;

    private int totalRankingPlacements;
    private int totalExistingPlacements;
    private int totalImagesUpdated;

    public GameObject loadingIcon;

    public Scrollbar leaderboardScrollbar;

    private Vector3 defaultLeaderboardScrollbarPosition, offscreenLeaderboardScrollbarPosition;

    public Image bottomColorPanel;

    // Sortings
    private const string totalCareerScoreRanking = "total_career_score", levelRanking = "level", totalNotesHitRanking = "total_notes_hit",
        highestBeatmapScoreRanking = "highest_beatmap_score", highestComboRanking = "highest_combo", totalScoreRanking = "total_score",
        playCountRanking = "playcount", careerAccuracyRanking = "career_accuracy", totalCareerXRankRanking = "total_career_x_rank",
        totalCareerSRankRanking = "total_career_s_rank", totalCareerARankRanking = "total_career_a_rank", totalCareerBRankRanking = "total_career_b_rank",
        totalCareerCRankRanking = "total_career_c_rank", totalXRankRanking = "total_x_rank", totalSRankRanking = "total_s_rank",
        totalARankRanking = "total_a_rank", totalBRankRanking = "total_b_rank", totalCRankRanking = "total_c_rank";


    private ScriptManager scriptManager;

    private void OnEnable()
    {
        if (scriptManager = null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        if (scriptManager != null)
        {
            bottomColorPanel.color = scriptManager.songSelectManager.topColorPanelImage.color;
        }
        else
        {
            scriptManager = FindObjectOfType<ScriptManager>();
            bottomColorPanel.color = scriptManager.songSelectManager.topColorPanelImage.color;
        }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        totalPlacementsChecked = 0;
        totalExistingPlacements = 0;
        totalRankingPlacements = 50;
        leaderboardPlaceToGet = 1;
        totalImagesUpdated = 0;
        hasPersonalBest = false;
        hasLoadedLeaderboard = false;
        hasLoadedImages = false;
        hasChecked = false;
        playLeaderboardFlashAnimation = true;
        playFullLeaderboardFlashAnimation = true;
        fullLeaderboardFlashAnimationFinished = false;

        // Get leaderboard scroll bar positions
        defaultLeaderboardScrollbarPosition = leaderboardScrollbar.transform.position;
        offscreenLeaderboardScrollbarPosition = new Vector3(2000, 2000, 0);

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

        if (hasCheckedPersonalBest == true && totalPlacementsChecked == totalRankingPlacements && hasLoadedLeaderboard == false)
        {
            if (hasLoadedImages == false)
            {
                // Calculate how many leaderboard placements exist
                CalculateTotalExistingPlacements();

                // Load the leaderboard player images
                LoadLeaderboardPlayerImages();
            }

            if (totalImagesUpdated == totalExistingPlacements)
            {
                // Update the leaderboard button text information
                UpdateLeaderboardButtonTextInformation();

                // Set to true
                hasLoadedLeaderboard = true;

                // Disable loading icon
                loadingIcon.gameObject.SetActive(false);

                // Activate leaderboard
                leaderboard.gameObject.SetActive(true);

                // Position scrollbar back on screen
                leaderboardScrollbar.transform.position = defaultLeaderboardScrollbarPosition;

                // Play full leaderboard flash animation
                if (playFullLeaderboardFlashAnimation == true)
                {
                    playFullLeaderboardFlashAnimation = false;
                    StartCoroutine(PlayFullLeaderboardFlashAnimation());
                }
            }
        }

        // Leaderboard animation
        if (hasLoadedLeaderboard == true && playLeaderboardFlashAnimation == true && fullLeaderboardFlashAnimationFinished == true)
        {
            playLeaderboardFlashAnimation = false;
            StartCoroutine(PlayLeaderboardFlashAnimation());
        }
    }

    public IEnumerator RetrieveOverallRankingLeaderboard(int _leaderboardPlaceToGet)
    {
        WWWForm form = new WWWForm();

        switch (overallRankingDropDown.value)
        {
            case 0:
                form.AddField("sorting", totalScoreRanking);
                break;
            case 1:
                form.AddField("sorting", totalCareerScoreRanking);
                break;
            case 2:
                form.AddField("sorting", levelRanking);
                break;
            case 3:
                form.AddField("sorting", careerAccuracyRanking);
                break;
            case 4:
                form.AddField("sorting", highestComboRanking);
                break;
            case 5:
                form.AddField("sorting", totalNotesHitRanking);
                break;
            case 6:
                form.AddField("sorting", playCountRanking);
                break;
            case 7:
                form.AddField("sorting", totalCareerXRankRanking);
                break;
            case 8:
                form.AddField("sorting", totalCareerSRankRanking);
                break;
            case 9:
                form.AddField("sorting", totalCareerARankRanking);
                break;
            case 10:
                form.AddField("sorting", totalCareerBRankRanking);
                break;
            case 11:
                form.AddField("sorting", totalCareerCRankRanking);
                break;
            case 12:
                form.AddField("sorting", totalXRankRanking);
                break;
            case 13:
                form.AddField("sorting", totalSRankRanking);
                break;
            case 14:
                form.AddField("sorting", totalARankRanking);
                break;
            case 15:
                form.AddField("sorting", totalBRankRanking);
                break;
            case 16:
                form.AddField("sorting", totalCRankRanking);
                break;
            default:
                form.AddField("sorting", totalScoreRanking);
                break;
        }

        form.AddField("leaderboardPlaceToGet", _leaderboardPlaceToGet);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveoverallranking.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        ArrayList placeList = new ArrayList();

        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        for (int dataType = 0; dataType < 6; dataType++)
        {
            /*
              DataType:
              [0] = username
              [1] = stat
              [2] = playcount
              [3] = date joined
              [4] = playtime
              [5] = player image
            */

            if (www.downloadHandler.text != "1")
            {
                placeLeaderboardData[_leaderboardPlaceToGet - 1].Add(placeList[dataType].ToString());
                placeExists[_leaderboardPlaceToGet - 1] = true;
            }
            else
            {
                placeExists[_leaderboardPlaceToGet - 1] = false;
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

            switch (overallRankingDropDown.value)
            {
                case 0:
                    form.AddField("sorting", totalScoreRanking);
                    break;
                case 1:
                    form.AddField("sorting", totalCareerScoreRanking);
                    break;
                case 2:
                    form.AddField("sorting", levelRanking);
                    break;
                case 3:
                    form.AddField("sorting", careerAccuracyRanking);
                    break;
                case 4:
                    form.AddField("sorting", highestComboRanking);
                    break;
                case 5:
                    form.AddField("sorting", totalNotesHitRanking);
                    break;
                case 6:
                    form.AddField("sorting", playCountRanking);
                    break;
                case 7:
                    form.AddField("sorting", totalCareerXRankRanking);
                    break;
                case 8:
                    form.AddField("sorting", totalCareerSRankRanking);
                    break;
                case 9:
                    form.AddField("sorting", totalCareerARankRanking);
                    break;
                case 10:
                    form.AddField("sorting", totalCareerBRankRanking);
                    break;
                case 11:
                    form.AddField("sorting", totalCareerCRankRanking);
                    break;
                case 12:
                    form.AddField("sorting", totalXRankRanking);
                    break;
                case 13:
                    form.AddField("sorting", totalSRankRanking);
                    break;
                case 14:
                    form.AddField("sorting", totalARankRanking);
                    break;
                case 15:
                    form.AddField("sorting", totalBRankRanking);
                    break;
                case 16:
                    form.AddField("sorting", totalCRankRanking);
                    break;
                default:
                    form.AddField("sorting", totalScoreRanking);
                    break;
            }

            form.AddField("player_id", MySQLDBManager.username);

            UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievepersonalbestoverallranking.php", form);
            www.chunkedTransfer = false;
            yield return www.SendWebRequest();

            ArrayList placeList = new ArrayList();

            placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));


            for (int dataType = 0; dataType < 5; dataType++)
            {
                /*
                  DataType:
                  [1] = stat
                  [2] = playcount
                  [3] = date joined
                  [4] = playtime
                  [5] = player image
                */

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

    // Deactivate all leaderboard buttons
    private void DeactivateAllLeaderboardButtons()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].gameObject.SetActive(false);
        }

        personalBestButton.gameObject.SetActive(false);

        // Set scrollbar position off screen
        leaderboardScrollbar.transform.position = offscreenLeaderboardScrollbarPosition;
    }

    // Activate all leaderboard buttons
    private void ActivateAllLeaderboardButtons()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].gameObject.SetActive(true);
        }

        personalBestButton.gameObject.SetActive(true);

        // Position scrollbar back on screen
        leaderboardScrollbar.transform.position = defaultLeaderboardScrollbarPosition;
    }

    // Update the leaderboard button text information
    private void UpdateLeaderboardButtonTextInformation()
    {
        for (int placementToCheck = 0; placementToCheck < totalRankingPlacements; placementToCheck++)
        {
            if (placeExists[placementToCheck] == true)
            {
                // Update the text for the leaderboard button
                overallLeaderboardButtonArray[placementToCheck].playernameText.text = placeLeaderboardData[placementToCheck][0].ToUpper();
                
                overallLeaderboardButtonArray[placementToCheck].playCountText.text = "PLAYS " + placeLeaderboardData[placementToCheck][2];
                overallLeaderboardButtonArray[placementToCheck].DateJoinedText.text = "JOINED " + placeLeaderboardData[placementToCheck][3];
                overallLeaderboardButtonArray[placementToCheck].playTimeText.text = CalculateDaysHoursMinutes(float.Parse(placeLeaderboardData[placementToCheck][4]));


                // Display the stat text with correct string addition
                switch (overallRankingDropDown.value)
                {
                    case 2:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = "LVL " + placeLeaderboardData[placementToCheck][1];
                        break;
                    case 3:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = placeLeaderboardData[placementToCheck][1] + "%";
                        break;
                    case 4:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = placeLeaderboardData[placementToCheck][1] + "x";
                        break;
                    case 5:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = placeLeaderboardData[placementToCheck][1] + " NOTES";
                        break;
                    case 6:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = placeLeaderboardData[placementToCheck][1] + " PLAYS";
                        break;
                    default:
                        overallLeaderboardButtonArray[placementToCheck].statText.text = placeLeaderboardData[placementToCheck][1];
                        break;
                }

                // Enable content panel
                overallLeaderboardButtonArray[placementToCheck].contentPanel.gameObject.SetActive(true);
            }
        }

        // Update personal best information
        if (hasPersonalBest == true)
        {
            personalBestLeaderboardButtonScript.playernameText.text = MySQLDBManager.username.ToUpper();
            personalBestLeaderboardButtonScript.playCountText.text = "PLAYS " + personalBestLeaderboardData[1];
            personalBestLeaderboardButtonScript.DateJoinedText.text = "JOINED " + personalBestLeaderboardData[2];
            personalBestLeaderboardButtonScript.playTimeText.text = CalculateDaysHoursMinutes(float.Parse(personalBestLeaderboardData[3]));

            // Update the text with the correct string addition
            switch (overallRankingDropDown.value)
            {
                case 2:
                    personalBestLeaderboardButtonScript.statText.text = "LVL " + personalBestLeaderboardData[0];
                    break;
                case 3:
                    personalBestLeaderboardButtonScript.statText.text = personalBestLeaderboardData[0] + "%";
                    break;
                case 4:
                    personalBestLeaderboardButtonScript.statText.text = personalBestLeaderboardData[0] + "x";
                    break;
                case 5:
                    personalBestLeaderboardButtonScript.statText.text = personalBestLeaderboardData[0] + " NOTES";
                    break;
                case 6:
                    personalBestLeaderboardButtonScript.statText.text = personalBestLeaderboardData[0] + " PLAYS";
                    break;
                default:
                    personalBestLeaderboardButtonScript.statText.text = personalBestLeaderboardData[0];
                    break;
            }



            personalBestLeaderboardButtonScript.contentPanel.gameObject.SetActive(true);
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
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.isNetworkError || uwr.isHttpError)
                    {
                        totalImagesUpdated++;
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);

                        // Update the image for the placement
                        overallLeaderboardButtonArray[_placement].profileImage.material.mainTexture = texture;

                        // Set image to false then to true to activate new image
                        overallLeaderboardButtonArray[_placement].profileImage.gameObject.SetActive(true);

                        // Increment
                        totalImagesUpdated++;
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

    // Play the full leaderboard flash animation
    private IEnumerator PlayFullLeaderboardFlashAnimation()
    {
        // Only flash first 10 buttons
        for (int i = 0; i < 10; i++)
        {
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(false);
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        playFullLeaderboardFlashAnimation = false;
        fullLeaderboardFlashAnimationFinished = true;
    }

    private IEnumerator PlayLeaderboardFlashAnimation()
    {
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(false);
            overallLeaderboardButtonArray[i].flashAnimator.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);

        playLeaderboardFlashAnimation = true;
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
    private string CalculateDaysHoursMinutes(float _secs)
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
            overallLeaderboardButtonArray[i].placementText.text = (i + 1).ToString() + ".";

            // Create and assign new profile image material
            overallLeaderboardButtonArray[i].profileImage.material = new Material(Shader.Find("UI/Unlit/Transparent"));
        }
    }

    // Deactivate all button content panels
    private void DeactivateAllButtonContentPanels()
    {
        // Hide all buttons
        for (int i = 0; i < totalRankingPlacements; i++)
        {
            overallLeaderboardButtonArray[i].contentPanel.gameObject.SetActive(false);
            personalBestLeaderboardButtonScript.contentPanel.gameObject.SetActive(false);
        }
    }
   
    // Reset leaderboard
    public void ResetLeaderboard()
    {
        // Stop all coroutines 
        StopAllCoroutines();

        // Deactivate entire leaderboard
        leaderboard.gameObject.SetActive(false);
        // Set scrollbar position off screen
        leaderboardScrollbar.transform.position = offscreenLeaderboardScrollbarPosition;
        // Deactivate all button content panels
        DeactivateAllButtonContentPanels();

        hasCheckedPersonalBest = false;
        hasPersonalBest = false;
        hasLoadedLeaderboard = false;
        hasLoadedImages = false;
        hasChecked = false;
        personalBestLeaderboardData.Clear();
        player_id = "";
        totalPlacementsChecked = 0;
        leaderboardPlaceToGet = 1;
        totalImagesUpdated = 0;
        totalExistingPlacements = 0;

        for (int i = 0; i < totalRankingPlacements; i++)
        {
            placeLeaderboardData[i].Clear();
            placeExists[i] = false;
        }

        loadingIcon.gameObject.SetActive(true);

        // Update the ranking title text based on the current rank sorting
        switch (overallRankingDropDown.value)
        {
            case 0:
                overallRankingText.text = "TOTAL SCORE RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL SCORE RANKING (ALL PLAYS)", "PURPLE");
                break;
            case 1:
                overallRankingText.text = "TOTAL CAREER SCORE RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER SCORE RANKING", "PURPLE");
                break;
            case 2:
                overallRankingText.text = "LEVEL RANKING";
                scriptManager.messagePanel.DisplayMessage("LEVEL RANKING", "PURPLE");
                break;
            case 3:
                overallRankingText.text = "CAREER ACCURACY RANKING";
                scriptManager.messagePanel.DisplayMessage("CAREER ACCURACY RANKING", "PURPLE");
                break;
            case 4:
                overallRankingText.text = "COMBO RANKING";
                scriptManager.messagePanel.DisplayMessage("HIGHEST COMBO RANKING", "PURPLE");
                break;
            case 5:
                overallRankingText.text = "TOTAL NOTES RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL NOTES HIT RANKING", "PURPLE");
                break;
            case 6:
                overallRankingText.text = "PLAY COUNT RANKING";
                scriptManager.messagePanel.DisplayMessage("PLAY COUNT RANKING", "PURPLE");
                break;
            case 7:
                overallRankingText.text = "TOTAL CAREER X RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER X RANK RANKING (TOTAL PLAYS)", "PURPLE");
                break;
            case 8:
                overallRankingText.text = "TOTAL CAREER S RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER S RANK RANKING (TOTAL PLAYS)", "PURPLE");
                break;
            case 9:
                overallRankingText.text = "TOTAL CAREER A RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER A RANK RANKING (TOTAL PLAYS)", "PURPLE");
                break;
            case 10:
                overallRankingText.text = "TOTAL CAREER B RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER B RANK RANKING (TOTAL PLAYS)", "PURPLE");
                break;
            case 11:
                overallRankingText.text = "TOTAL CAREER C RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL CAREER C RANK RANKING (TOTAL PLAYS)", "PURPLE");
                break;
            case 12:
                overallRankingText.text = "TOTAL X RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL X RANK RANKING", "PURPLE");
                break;
            case 13:
                overallRankingText.text = "TOTAL S RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL S RANK RANKING", "PURPLE");
                break;
            case 14:
                overallRankingText.text = "TOTAL A RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL A RANK RANKING", "PURPLE");
                break;
            case 15:
                overallRankingText.text = "TOTAL B RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL B RANK RANKING", "PURPLE");
                break;
            case 16:
                overallRankingText.text = "TOTAL C RANK RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL C RANK RANKING", "PURPLE");
                break;
            default:
                overallRankingText.text = "TOTAL SCORE RANKING";
                scriptManager.messagePanel.DisplayMessage("TOTAL SCORE RANKING (ALL PLAYS)", "PURPLE");
                break;
        }
    }
}
