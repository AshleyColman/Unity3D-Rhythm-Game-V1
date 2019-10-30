using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{

    // Ints
    public int totalPlacements, totalProfilesChecked, totalExistingProfiles, totalURLImagesToUpload;

    // Strings
    private string playerName, playerImageUrl;
    private List<string>[] placeData;
    public string[] usernameArray, playerImageUrlArray, playcountArray, highestComboArray, notesHitArray, feverTimeActivationsArray, highestBeatmapScoreArray,
        oneKeyTotalScoreArray, twoKeyTotalScoreArray, fourKeyTotalScoreArray, sixKeyTotalScoreArray, overallTotalScoreArray,
        pRankTotalArray, sRankTotalArray, aRankTotalArray, bRankTotalArray, cRankTotalArray, dRankTotalArray, eRankTotalArray, fRankTotalArray,
        highestPercentageArray, levelArray, messageArray, dateJoinedArray;

    // Bools
    public bool[] placeExists;
    public bool profileHasLoaded;
    public bool informationAssigned;

    // Text
    public TextMeshProUGUI usernameText, playCountText, highestComboText, notesHitText, feverTimeActivationsText, highestBeatmapScoreText,
        oneKeyTotalScoreText, twoKeyTotalScoreText, fourKeyTotalScoreText, sixKeyTotalScoreText, overallTotalScoreText, pRankText,
        sRankText, aRankText, bRankText, cRankText, dRankText, eRankText, fRankText, highestPercentageText, levelText, messageText, dateJoinedText;

    public Slider levelSlider;

    // Images
    public Image profileImage;

    // Materials
    public Material place1Material, place2Material, place3Material, place4Material, place5Material, place6Material, place7Material, place8Material,
        place9Material, place10Material;

    public Material defaultPlayerProfileMaterial;

    // Gameobjects
    public GameObject profileLoadingIcon;
    public GameObject playerProfilePanel;
    public GameObject playerProfile, characterSelect, leaderboard;

    // Scripts
    private BeatmapRanking beatmapRanking;

    // Properties
    public bool InformationAssigned
    {
        get { return informationAssigned; }
    }

    public int TotalExistingProfiles
    {
        get { return totalExistingProfiles; }
    }

    public string[] PlayerImageUrlArray
    {
        get { return playerImageUrlArray; }
    }

    public int TotalURLImagesToUpload
    {
        get { return totalURLImagesToUpload; }
    }


    // Use this for initialization
    void Start()
    {

        // Initalize
        placeData = new List<string>[10];
        usernameArray = new string[10];
        playcountArray = new string[10];
        highestComboArray = new string[10];
        notesHitArray = new string[10];
        feverTimeActivationsArray = new string[10];
        highestBeatmapScoreArray = new string[10];
        oneKeyTotalScoreArray = new string[10];
        twoKeyTotalScoreArray = new string[10];
        fourKeyTotalScoreArray = new string[10];
        sixKeyTotalScoreArray = new string[10];
        overallTotalScoreArray = new string[10];
        pRankTotalArray = new string[10];
        sRankTotalArray = new string[10];
        aRankTotalArray = new string[10];
        bRankTotalArray = new string[10];
        cRankTotalArray = new string[10];
        dRankTotalArray = new string[10];
        eRankTotalArray = new string[10];
        fRankTotalArray = new string[10];
        playerImageUrlArray = new string[10];
        highestPercentageArray = new string[10];
        levelArray = new string[10];
        messageArray = new string[10];
        dateJoinedArray = new string[10];

        placeExists = new bool[10];
        totalPlacements = 10;
        totalProfilesChecked = 0;
        totalExistingProfiles = 99; // Set to 99 at start to prevent auto assign at start of frame
        profileHasLoaded = false;
        informationAssigned = false;

        // Instantiate the lists
        for (int i = 0; i < placeData.Length; i++)
        {
            placeData[i] = new List<string>();
        }

        playerProfilePanel.gameObject.SetActive(false);

        // Reference
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
    }

    private void Update()
    {
        // If all profiles have been checked
        if (totalProfilesChecked == totalExistingProfiles && informationAssigned == false)
        {
            // Loop through all player profile placements and assign the information
            AssignPlayerProfileInformation();

            // Information has been assigned
            informationAssigned = true;
        }
    }


    // Loop through all player profile placements and assign the information
    private void AssignPlayerProfileInformation()
    {
        // Loop through all the placements
        for (sbyte placementToCheck = 0; placementToCheck < totalExistingProfiles; placementToCheck++)
        {
            // If placement information was found for the position on the leaderboard
            if (placeExists[placementToCheck] == true)
            {
                /*
                    [0] = $player_name_return;
                    [1] = $play_count_return;
                    [2] = $highest_combo_return; 
                    [3] = $notes_hit_return; 
                    [4] = $highest_beatmap_score_return;
                    [5] = $fever_time_activations_return; 
                    [6] = $1key_total_score_return; 
                    [7] = $2key_total_score_return;
                    [8] = $4key_total_score_return; 
                    [9] = $6key_total_score_return;
                    [10] = $overall_total_score_return;
                    [11] = $p_rank_total_return;
                    [12] = $s_rank_total_return; 
                    [13] = $a_rank_total_return;
                    [14] = $b_rank_total_return; 
                    [15] = $c_rank_total_return; 
                    [16] = $d_rank_total_return; 
                    [17] = $e_rank_total_return; 
                    [18] = $f_rank_total_return; 
                    [19] = $image_url_return; 
                    [20] = $highestPercentage_return;
                    [21] = $level_return;
                    [22] = $date_joined_return;
                */

                // Assign the database information to the variables
                usernameArray[placementToCheck] = placeData[placementToCheck][0];
                playcountArray[placementToCheck] = placeData[placementToCheck][1];
                highestComboArray[placementToCheck] = placeData[placementToCheck][2];
                notesHitArray[placementToCheck] = placeData[placementToCheck][3];
                feverTimeActivationsArray[placementToCheck] = placeData[placementToCheck][4];
                highestBeatmapScoreArray[placementToCheck] = placeData[placementToCheck][5];
                oneKeyTotalScoreArray[placementToCheck] = placeData[placementToCheck][6];
                twoKeyTotalScoreArray[placementToCheck] = placeData[placementToCheck][7];
                fourKeyTotalScoreArray[placementToCheck] = placeData[placementToCheck][8];
                sixKeyTotalScoreArray[placementToCheck] = placeData[placementToCheck][9];
                overallTotalScoreArray[placementToCheck] = placeData[placementToCheck][10];
                pRankTotalArray[placementToCheck] = placeData[placementToCheck][11];
                sRankTotalArray[placementToCheck] = placeData[placementToCheck][12];
                aRankTotalArray[placementToCheck] = placeData[placementToCheck][13];
                bRankTotalArray[placementToCheck] = placeData[placementToCheck][14];
                cRankTotalArray[placementToCheck] = placeData[placementToCheck][15];
                dRankTotalArray[placementToCheck] = placeData[placementToCheck][16];
                eRankTotalArray[placementToCheck] = placeData[placementToCheck][17];
                fRankTotalArray[placementToCheck] = placeData[placementToCheck][18];
                playerImageUrlArray[placementToCheck] = placeData[placementToCheck][19];
                highestPercentageArray[placementToCheck] = placeData[placementToCheck][20];
                levelArray[placementToCheck] = placeData[placementToCheck][21];
                messageArray[placementToCheck] = placeData[placementToCheck][22];
                dateJoinedArray[placementToCheck] = placeData[placementToCheck][23];



                // Check if the player image url is null
                if (playerImageUrlArray[placementToCheck] != null)
                {
                    // Increment the total amount of totalURLImages to upload
                    totalURLImagesToUpload++;
                }
            }
        }
    }

    // Display the player profile selected
    public void DisplayPlayerProfile(int _leaderboardPlacement)
    {
        // _leadeboardPlacement = the leadeboard button selected

        // Turn on player profile
        // Hide character select 
        // Hide leaderboard
        playerProfile.gameObject.SetActive(true);
        characterSelect.gameObject.SetActive(false);
        leaderboard.gameObject.SetActive(false);

        // Reset the player profile text and image
        ResetPlayerProfile();

        // Update player profile text
        usernameText.text = usernameArray[_leaderboardPlacement].ToUpper();
        playCountText.text = playcountArray[_leaderboardPlacement];
        highestComboText.text = highestComboArray[_leaderboardPlacement];
        notesHitText.text = notesHitArray[_leaderboardPlacement];
        feverTimeActivationsText.text = feverTimeActivationsArray[_leaderboardPlacement];
        highestBeatmapScoreText.text = highestBeatmapScoreArray[_leaderboardPlacement];
        oneKeyTotalScoreText.text = oneKeyTotalScoreArray[_leaderboardPlacement];
        twoKeyTotalScoreText.text = twoKeyTotalScoreArray[_leaderboardPlacement];
        fourKeyTotalScoreText.text = fourKeyTotalScoreArray[_leaderboardPlacement];
        sixKeyTotalScoreText.text = sixKeyTotalScoreArray[_leaderboardPlacement];
        overallTotalScoreText.text = overallTotalScoreArray[_leaderboardPlacement];
        pRankText.text = pRankTotalArray[_leaderboardPlacement];
        sRankText.text = sRankTotalArray[_leaderboardPlacement];
        aRankText.text = aRankTotalArray[_leaderboardPlacement];
        bRankText.text = bRankTotalArray[_leaderboardPlacement];
        cRankText.text = cRankTotalArray[_leaderboardPlacement];
        dRankText.text = dRankTotalArray[_leaderboardPlacement];
        eRankText.text = eRankTotalArray[_leaderboardPlacement];
        fRankText.text = fRankTotalArray[_leaderboardPlacement];
        messageText.text = messageArray[_leaderboardPlacement];
        //levelText.text =
        // levelSlider.value = 
        highestPercentageText.text = highestPercentageArray[_leaderboardPlacement] + "%";
        dateJoinedText.text = dateJoinedArray[_leaderboardPlacement];

        // Load the profile image
        LoadProfileImage((_leaderboardPlacement + 1));

        // Update player profile image
        // StartCoroutine(LoadPlayerImg(playerImageUrlArray[_leaderboardPlacement]));
    }

    // Load the player image
    IEnumerator LoadPlayerImg(string _url)
    {
        // Activate the loading icon
        profileLoadingIcon.gameObject.SetActive(true);

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                // ERROR 
                Debug.Log(uwr.error);

                // Deactivate the loading icon
                profileLoadingIcon.gameObject.SetActive(false);

                // Display error message
            }
            else
            {
                // Deactivate the loading icon
                profileLoadingIcon.gameObject.SetActive(false);
                // Activate the profile panel
                playerProfilePanel.gameObject.SetActive(true);

                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Update the image with the new image
                profileImage.material.mainTexture = texture;

                // Set image to false then to true to activate new image
                profileImage.gameObject.SetActive(false);
                profileImage.gameObject.SetActive(true);
            }
        }
    }

    private void LoadProfileImage(int _leaderboardPlaceIndex)
    {
        // Get downloaded asset bundle

        switch (_leaderboardPlaceIndex)
        {
            case 1:
                profileImage.material = place1Material;
                break;
            case 2:
                profileImage.material = place2Material;
                break;
            case 3:
                profileImage.material = place3Material;
                break;
            case 4:
                profileImage.material = place4Material;
                break;
            case 5:
                profileImage.material = place5Material;
                break;
            case 6:
                profileImage.material = place6Material;
                break;
            case 7:
                profileImage.material = place7Material;
                break;
            case 8:
                profileImage.material = place8Material;
                break;
            case 9:
                profileImage.material = place9Material;
                break;
            case 10:
                profileImage.material = place10Material;
                break;
        }

        // Update the image with the new image


        // Set image to false then to true to activate new image
        profileImage.gameObject.SetActive(false);
        profileImage.gameObject.SetActive(true);

        // Activate the profile panel
        playerProfilePanel.gameObject.SetActive(true);
        // Deactivate the loading icon
        profileLoadingIcon.gameObject.SetActive(false);

    }

    // Close the player profile and go to the leaderboard
    public void ClosePlayerProfile()
    {
        leaderboard.gameObject.SetActive(true);
        playerProfilePanel.gameObject.SetActive(false);
    }

    // Loop through and get all player profile information
    public void GetPlayerProfiles()
    {
        // Calculate total existing placements
        CalculateTotalExistingPlacements();

        // Loop through all the placements
        for (int placementToCheck = 0; placementToCheck < totalExistingProfiles; placementToCheck++)
        {
            // If the leaderboard placement exists
            if (beatmapRanking.PlaceExists[placementToCheck] == true)
            {
                StartCoroutine(RetrievePlayerProfilesFromServer(placementToCheck));
            }
            else
            {
                // Disable interaction for the leaderboard button that would load a profile that does not contain information
                //
                //
                //

            }
        }
    }


    // Retrieve player profile information from the server
    private IEnumerator RetrievePlayerProfilesFromServer(int _placement)
    {
        WWWForm form = new WWWForm();

        // Get the placement username - profile to load
        string username = beatmapRanking.RankedButtonUsername[_placement];

        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveplayerprofileinformation.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Stores all the placement information from the database
        ArrayList placeList = new ArrayList();

        // Split the information retrieved from the database
        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        // Loop through all the player profile data and assign
        for (sbyte dataType = 0; dataType < 24; dataType++)
        {
            /*
              DataType:
              [0] = $player_name_return;
              [1] = $play_count_return;
              [2] = $highest_combo_return; 
              [3] = $notes_hit_return; 
              [4] = $highest_beatmap_score_return;
              [5] = $fever_time_activations_return; 
              [6] = $1key_total_score_return; 
              [7] = $2key_total_score_return;
              [8] = $4key_total_score_return; 
              [9] = $6key_total_score_return;
              [10] = $overall_total_score_return;
              [11] = $p_rank_total_return;
              [12] = $s_rank_total_return; 
              [13] = $a_rank_total_return;
              [14] = $b_rank_total_return; 
              [15] = $c_rank_total_return; 
              [16] = $d_rank_total_return; 
              [17] = $e_rank_total_return; 
              [18] = $f_rank_total_return; 
              [19] = $image_url_return; 
              [20] = $highestPercentage_return;
              [21] = $level_return;
              [22] = $date_joined_return;
            */

            if (www.downloadHandler.text != "1")
            {
                // SUCCESS - DATA FOUND
                // Save to the player profile data for this placement number
                placeData[_placement].Add(placeList[dataType].ToString());
                placeExists[_placement] = true;
            }
            else
            {
                // ERROR - NO DATA FOR THIS PLACEMENT
                placeData[_placement].Add("");
                placeExists[_placement] = false;
            }
        }

        // Increment profiles checked
        totalProfilesChecked++;
    }

    // Calculate the total amount of existing leaderboard placements that can show profile information
    public void CalculateTotalExistingPlacements()
    {
        // Reset to 0
        totalExistingProfiles = 0;

        for (int i = 0; i < beatmapRanking.PlaceExists.Length; i++)
        {
            if (beatmapRanking.PlaceExists[i] == true)
            {
                totalExistingProfiles++;
            }
        }

    }

    // Reset all player profile information
    public void ResetPlayerProfileVariables()
    {
        playerName = "";
        playerImageUrl = "";

        profileHasLoaded = false;
        informationAssigned = false; // Allow information to be assigned again
        totalProfilesChecked = 0; // Reset profiles check
        totalExistingProfiles = 99; // Reset to 99 to prevent instant assign on first frame
        totalURLImagesToUpload = 0; // Reset total url images to upload

        // RESET ARRAYS
        // Loop through all the placements
        for (sbyte placementToCheck = 0; placementToCheck < totalPlacements; placementToCheck++)
        {
            // Assign the database information to the variables
            usernameArray[placementToCheck] = "";
            playcountArray[placementToCheck] = "";
            highestComboArray[placementToCheck] = "";
            notesHitArray[placementToCheck] = "";
            feverTimeActivationsArray[placementToCheck] = "";
            highestBeatmapScoreArray[placementToCheck] = "";
            oneKeyTotalScoreArray[placementToCheck] = "";
            twoKeyTotalScoreArray[placementToCheck] = "";
            fourKeyTotalScoreArray[placementToCheck] = "";
            sixKeyTotalScoreArray[placementToCheck] = "";
            overallTotalScoreArray[placementToCheck] = "";
            pRankTotalArray[placementToCheck] = "";
            sRankTotalArray[placementToCheck] = "";
            aRankTotalArray[placementToCheck] = "";
            bRankTotalArray[placementToCheck] = "";
            cRankTotalArray[placementToCheck] = "";
            dRankTotalArray[placementToCheck] = "";
            eRankTotalArray[placementToCheck] = "";
            fRankTotalArray[placementToCheck] = "";
            playerImageUrlArray[placementToCheck] = "";
            highestPercentageArray[placementToCheck] = "";
            levelArray[placementToCheck] = "";
            messageArray[placementToCheck] = "";
            dateJoinedArray[placementToCheck] = "";
        }
    }

    // Reset the player profile text and image
    private void ResetPlayerProfile()
    {
        // Deactivate the player profile panel
        playerProfilePanel.gameObject.SetActive(false);

        // Reset material
        profileImage.material = defaultPlayerProfileMaterial;

        // Reset text
        usernameText.text = "";
        playCountText.text = "";
        highestComboText.text = "";
        notesHitText.text = "";
        feverTimeActivationsText.text = "";
        highestBeatmapScoreText.text = "";
        oneKeyTotalScoreText.text = "";
        twoKeyTotalScoreText.text = "";
        fourKeyTotalScoreText.text = "";
        sixKeyTotalScoreText.text = "";
        overallTotalScoreText.text = "";
        pRankText.text = "";
        sRankText.text = "";
        aRankText.text = "";
        bRankText.text = "";
        cRankText.text = "";
        dRankText.text = "";
        eRankText.text = "";
        fRankText.text = "";
        //levelText.text = "";
        highestPercentageText.text = "";
        messageText.text = "";

        //levelSlider.value = 0;
    }
}