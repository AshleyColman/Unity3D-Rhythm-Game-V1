using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    #region Variables
    // Strings
    private string playerName, playerImageUrl, profileImageUrl, bannerImageUrl, facebookProfileUrl, twitterProfileUrl,
        youtubeProfileUrl;
    private List<string> placeData = new List<string>();

    // Bool
    public bool profileHasLoaded;
    public bool informationAssigned, informationRetrieved;

    // Text
    public TextMeshProUGUI titleUsernameText, careerDateJoinedText, totalDateJoinedText, totalPlayCountText, levelText,
        highestBeatmapScoreText, careerScoreText, careerEasyScoreText, careerNormalScoreText, careerHardScoreText,
        careerComboText, careerHighestComboText, careerPerfectText, careerGoodText, careerEarlyText, careerMissText,
        careerNotesHitText, careerSPlusRankText, careerSRankText, careerARankText, careerBRankText, careerCRankText,
        careerDRankText, careerERankText, careerFRankText, totalPlaytime, totalScoreText, totalEasyScoreText, totalNormalScoreText,
        totalHardScoreText, totalComboText, totalPerfectText, totalGoodText, totalEarlyText, totalMissText, totalNotesHitText,
        totalSPlusRankText, totalSRankText, totalARankText, totalBRankText, totalCRankText, totalDRankText, totalERankText,
        totalFRankText, careerUsernameText, totalUsernameText, totalLevelText, editConfirmationText;

    // Inputfield
    public TMP_InputField profileImageUrlInputfield, bannerImageUrlInputfield, twitterProfileUrlInputfield,
        facebookProfileUrlInputfield, youtubeProfileUrlInputfield;

    // Slider
    public Slider levelSlider;

    // Images
    public Image profileImage, bannerImage;

    // Material
    public Material defaultMaterial, userImagePlayerProfileMaterial, userBannerProfileMaterial;

    // Gameobjects
    public GameObject profileLoadingIcon, profilePanel, editProfilePanel, editorProfileConfirmationPanel,
        viewProfileInformationPanel, editProfileInputFieldsPanel;

    // Button
    public Button editProfileButton, facebookButton, twitterButton, youtubeButton, saveProfileButton;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    private void OnEnable()
    {
        ResetPlayerProfile();
    }

    // Use this for initialization
    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        if (profileHasLoaded == false)
        {
            switch (informationRetrieved)
            {
                case false:
                    StartCoroutine(GetPlayerProfileInformation());
                    break;
                case true:
                    switch (informationAssigned)
                    {
                        case false:
                            AssignProfileInformation();
                            profileHasLoaded = true;
                            break;
                    }
                    break;
            }
        }
    }

    // Get player profile information
    private IEnumerator GetPlayerProfileInformation()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", MySQLDBManager.username);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_profile.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Stores all the placement information from the database
        ArrayList placeList = new ArrayList();

        // Split the information retrieved from the database
        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        // Loop through all the leaderboard data and assign
        for (sbyte dataType = 0; dataType < 48; dataType++)
        {
            /*
                $returnarray[0] = $infoarray["username"];
                $returnarray[1] = $infoarray["date_joined"];
                $returnarray[2] = $infoarray["image_url"];
                $returnarray[3] = $infoarray["play_count"];
                $returnarray[4] = $infoarray["play_time"];
                $returnarray[5] = $infoarray["level"];
    
                // Career
                $returnarray[6] = $infoarray["highest_beatmap_score"];
                $returnarray[7] = $infoarray["career_score"];
                $returnarray[8] = $infoarray["career_easy_score"];
                $returnarray[9] = $infoarray["career_normal_score"];
                $returnarray[10] = $infoarray["career_hard_score"];
                $returnarray[11] = $infoarray["career_combo"];
                $returnarray[12] = $infoarray["highest_combo"];
                $returnarray[13] = $infoarray["career_perfect"];
                $returnarray[14] = $infoarray["career_good"];
                $returnarray[15] = $infoarray["career_early"];
                $returnarray[16] = $infoarray["career_miss"];
                $returnarray[17] = $infoarray["career_notes_hit"];
                $returnarray[18] = $infoarray["career_s+_rank"];
                $returnarray[19] = $infoarray["career_s_rank"];
                $returnarray[20] = $infoarray["career_a_rank"];
                $returnarray[21] = $infoarray["career_b_rank"];
                $returnarray[22] = $infoarray["career_c_rank"];
                $returnarray[23] = $infoarray["career_d_rank"];
                $returnarray[24] = $infoarray["career_e_rank"];
                $returnarray[25] = $infoarray["career_f_rank"];

                // Total
                $returnarray[26] = $infoarray["total_score"];
                $returnarray[27] = $infoarray["total_easy_score"];
                $returnarray[28] = $infoarray["total_normal_score"];
                $returnarray[29] = $infoarray["total_hard_score"];
                $returnarray[30] = $infoarray["total_combo"];
                $returnarray[31] = $infoarray["total_perfect"];
                $returnarray[32] = $infoarray["total_good"];
                $returnarray[33] = $infoarray["total_early"];
                $returnarray[34] = $infoarray["total_miss"];
                $returnarray[35] = $infoarray["total_notes_hit"];
                $returnarray[36] = $infoarray["total_s+_rank"];
                $returnarray[37] = $infoarray["total_s_rank"];
                $returnarray[38] = $infoarray["total_a_rank"];
                $returnarray[39] = $infoarray["total_b_rank"];
                $returnarray[40] = $infoarray["total_c_rank"];
                $returnarray[41] = $infoarray["total_d_rank"];
                $returnarray[42] = $infoarray["total_e_rank"];
                $returnarray[43] = $infoarray["total_f_rank"];
                $returnarray[44] = $infoarray["banner_image_url"];
                $returnarray[45] = $infoarray["facebook_url"];
                $returnarray[46] = $infoarray["twitter_url"];
                $returnarray[47] = $infoarray["youtube_url"];
            */

            switch (www.downloadHandler.text)
            {
                case "ERROR":
                    Debug.Log("ERROR");
                    break;
                default:
                    placeData.Add(placeList[dataType].ToString());
                    break;
            }
        }

        informationRetrieved = true;
    }

    // Assign the profile information
    private void AssignProfileInformation()
    {
        titleUsernameText.text = placeData[0] + "'s";

        // CAREER PANEL
        careerUsernameText.text = placeData[0];
        careerDateJoinedText.text = placeData[1];
        highestBeatmapScoreText.text = placeData[6];
        careerScoreText.text = placeData[7];
        careerEasyScoreText.text = placeData[8];
        careerNormalScoreText.text = placeData[9];
        careerHardScoreText.text = placeData[10];
        careerComboText.text = placeData[11];
        careerHighestComboText.text = placeData[12];
        careerPerfectText.text = placeData[13];
        careerGoodText.text = placeData[14];
        careerEarlyText.text = placeData[15];
        careerMissText.text = placeData[16];
        careerNotesHitText.text = placeData[17];
        careerSPlusRankText.text = placeData[18];
        careerSRankText.text = placeData[19];
        careerARankText.text = placeData[20];
        careerBRankText.text = placeData[21];
        careerCRankText.text = placeData[22];
        careerDRankText.text = placeData[23];
        careerERankText.text = placeData[24];
        careerFRankText.text = placeData[25];

        // TOTAL PANEL
        totalUsernameText.text = placeData[0];
        totalDateJoinedText.text = placeData[1];
        totalPlayCountText.text = placeData[3];
        totalPlaytime.text = scriptManager.overallRanking.CalculateDaysHoursMinutes(float.Parse(placeData[4]));
        totalScoreText.text = placeData[26];
        totalEasyScoreText.text = placeData[27];
        totalNormalScoreText.text = placeData[28];
        totalHardScoreText.text = placeData[29];
        totalComboText.text = placeData[30];
        totalPerfectText.text = placeData[31];
        totalGoodText.text = placeData[32];
        totalEarlyText.text = placeData[33];
        totalMissText.text = placeData[34];
        totalNotesHitText.text = placeData[35];
        totalSPlusRankText.text = placeData[36];
        totalSRankText.text = placeData[37];
        totalARankText.text = placeData[38];
        totalBRankText.text = placeData[39];
        totalCRankText.text = placeData[40];
        totalDRankText.text = placeData[41];
        totalERankText.text = placeData[42];
        totalFRankText.text = placeData[43];
        totalLevelText.text = placeData[5];

        if (placeData[45].Contains("facebook.com"))
        {
            facebookButton.interactable = true;
        }

        if (placeData[46].Contains("twitter.com"))
        {
            twitterButton.interactable = true;
        }

        if (placeData[47].Contains("youtube.com"))
        {
            youtubeButton.interactable = true;
        }

        // Load profile image
        StartCoroutine(LoadProfileImage());

        // Load banner image
        StartCoroutine(LoadBannerImage());

        informationAssigned = true;
    }

    // Load profile image
    private IEnumerator LoadProfileImage()
    {
        // placeData[2] = image url
        string completePath = placeData[2];

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                LoadDefaultMaterial(profileImage);      
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                profileImage.material = userImagePlayerProfileMaterial;
                profileImage.material.mainTexture = texture;
            }
        }
        yield return null;
    }

    // Load banner image
    private IEnumerator LoadBannerImage()
    {
        // placeData[44] = banner_image_url
        string completePath = placeData[44];

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                LoadDefaultMaterial(bannerImage);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                bannerImage.material = userBannerProfileMaterial;
                bannerImage.material.mainTexture = texture;
            }
        }

        // Activate panel
        profileLoadingIcon.gameObject.SetActive(false);
        profilePanel.gameObject.SetActive(true);
        viewProfileInformationPanel.gameObject.SetActive(true);

        yield return null;
    }

    // Load default material
    private void LoadDefaultMaterial(Image _image)
    {
        _image.material = defaultMaterial;
    }

    // Reset profile 
    private void ResetPlayerProfile()
    {
        placeData.Clear();

        titleUsernameText.text = "";

        // CAREER PANEL
        careerUsernameText.text = "";
        careerDateJoinedText.text = "";
        highestBeatmapScoreText.text = "";
        careerScoreText.text = "";
        careerEasyScoreText.text = "";
        careerNormalScoreText.text = "";
        careerHardScoreText.text = "";
        careerComboText.text = "";
        careerHighestComboText.text = "";
        careerPerfectText.text = "";
        careerGoodText.text = "";
        careerEarlyText.text = "";
        careerMissText.text = "";
        careerNotesHitText.text = "";
        careerSPlusRankText.text = "";
        careerSRankText.text = "";
        careerARankText.text = "";
        careerBRankText.text = "";
        careerCRankText.text = "";
        careerDRankText.text = "";
        careerERankText.text = "";
        careerFRankText.text = "";

        // TOTAL PANEL
        careerUsernameText.text = "";
        totalDateJoinedText.text = "";
        totalPlayCountText.text = "";
        totalPlaytime.text = "";
        totalScoreText.text = "";
        totalEasyScoreText.text = "";
        totalNormalScoreText.text = "";
        totalHardScoreText.text = "";
        totalComboText.text = "";
        totalPerfectText.text = "";
        totalGoodText.text = "";
        totalEarlyText.text = "";
        totalMissText.text = "";
        totalNotesHitText.text = "";
        totalSPlusRankText.text = "";
        totalSRankText.text = "";
        totalARankText.text = "";
        totalBRankText.text = "";
        totalCRankText.text = "";
        totalDRankText.text = "";
        totalERankText.text = "";
        totalFRankText.text = "";

        LoadDefaultMaterial(profileImage);
        LoadDefaultMaterial(bannerImage);

        facebookButton.interactable = false;
        twitterButton.interactable = false;
        youtubeButton.interactable = false;

        informationRetrieved = false;
        profileHasLoaded = false;
        informationAssigned = false;
        profilePanel.gameObject.SetActive(false);
        profileLoadingIcon.gameObject.SetActive(true);
    }

    public void OpenFacebookURL()
    {
        Application.OpenURL(placeData[45]);
    }

    public void OpenTwitterURL()
    {
        Application.OpenURL(placeData[46]);
    }

    public void OpenYoutubeURL()
    {
        Application.OpenURL(placeData[47]);
    }

    // Enable profile edit panel
    public void EnableProfileEditPanel()
    {
        // If profile is players own profile
        if (placeData[0] == MySQLDBManager.username)
        {
            // Enable editing
            StopAllCoroutines();
            viewProfileInformationPanel.gameObject.SetActive(false);
            editProfilePanel.gameObject.SetActive(true);
            editProfileInputFieldsPanel.gameObject.SetActive(true);
            editorProfileConfirmationPanel.gameObject.SetActive(false);
        }
    }

    // Reset edit profile panel
    public void ResetEditProfilePanel()
    {
        profileImageUrlInputfield.text = "";
        bannerImageUrlInputfield.text = "";
        facebookProfileUrlInputfield.text = "";
        twitterProfileUrlInputfield.text = "";
        youtubeProfileUrlInputfield.text = "";
        saveProfileButton.interactable = false;
    }

    // Enable save button if any input field length > 0
    public void ValidateEditProfilePanelInputFieldLength()
    {
        if (profileImageUrlInputfield.text.Length > 0 || bannerImageUrlInputfield.text.Length > 0 ||
            facebookProfileUrlInputfield.text.Length > 0 || twitterProfileUrlInputfield.text.Length > 0 ||
            youtubeProfileUrlInputfield.text.Length > 0)
        {
            saveProfileButton.interactable = true;
        }
        else
        {
            saveProfileButton.interactable = false;
        }
    }

    // Display edit profile confirmation panel
    public void DisplayEditProfileConfirmationPanel()
    {
        editProfileInputFieldsPanel.gameObject.SetActive(false);
        editorProfileConfirmationPanel.gameObject.SetActive(true);
        CheckProfileEdit();
    }

    // Close edit profile confirmation panel
    public void HideEditProfileConfirmationPanel()
    {
        editorProfileConfirmationPanel.gameObject.SetActive(false);
        editProfileInputFieldsPanel.gameObject.SetActive(true);
    }

    // Check profile url addresses are valid
    public void CheckProfileEdit()
    {
        // Set to empty
        profileImageUrl = "";
        bannerImageUrl = "";
        facebookProfileUrl = "";
        twitterProfileUrl = "";
        youtubeProfileUrl = "";

        editConfirmationText.text = "You are about to update: ";


        if (profileImageUrlInputfield.text.Length > 0)
        {
            profileImageUrl = profileImageUrlInputfield.text;
            editConfirmationText.text += "profile image url, ";
        }

        if (bannerImageUrlInputfield.text.Length > 0)
        {
            bannerImageUrl = bannerImageUrlInputfield.text;
            editConfirmationText.text += "banner image url, ";
        }

        if (facebookProfileUrlInputfield.text.Contains("www.facebook.com"))
        {
            facebookProfileUrl = facebookProfileUrlInputfield.text;
            editConfirmationText.text += "facebook profile url, ";
        }

        if (twitterProfileUrlInputfield.text.Contains("www.twitter.com"))
        {
            twitterProfileUrl = twitterProfileUrlInputfield.text;
            editConfirmationText.text += "twitter profile url, ";
        }

        if (youtubeProfileUrlInputfield.text.Contains("www.youtube.com"))
        {
            youtubeProfileUrl = youtubeProfileUrlInputfield.text;
            editConfirmationText.text += "youtube profile url, ";
        }

        // Remove last 2 characters from string
        editConfirmationText.text = editConfirmationText.text.Remove(editConfirmationText.text.Length - 2);
    }

    // Button click save profile information
    public void OnClickSaveProfileEdit()
    {
        StartCoroutine(SaveProfileEdit());
    }

    private IEnumerator SaveProfileEdit()
    {
        profileLoadingIcon.gameObject.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("username", MySQLDBManager.username);
        form.AddField("profileImageUrl", profileImageUrl);
        form.AddField("bannerImageUrl", bannerImageUrl);
        form.AddField("facebookProfileUrl", facebookProfileUrl);
        form.AddField("twitterProfileUrl", twitterProfileUrl);
        form.AddField("youtubeProfileUrl", youtubeProfileUrl);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/edit_player_profile.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        switch (www.downloadHandler.text)
        {
            case "ERROR":
                scriptManager.messagePanel.DisplayMessage("ERROR UPDATING PROFILE", scriptManager.uiColorManager.offlineColorSolid);
                break;
            case "SUCCESS":
                scriptManager.messagePanel.DisplayMessage("SUCCESSFULLY UPDATED PROFILE", 
                    scriptManager.uiColorManager.onlineColorSolid);
                CloseEditProfilePanel();
                CloseProfile();
                break;
        }

        profileLoadingIcon.gameObject.SetActive(false);
    }

    // Close edit profile panel
    public void CloseEditProfilePanel()
    {
        StopAllCoroutines();
        editProfilePanel.gameObject.SetActive(false);
        viewProfileInformationPanel.gameObject.SetActive(true);
    }

    // Close player profile
    public void CloseProfile()
    {
        editProfilePanel.gameObject.SetActive(false);
        editorProfileConfirmationPanel.gameObject.SetActive(false);
        editProfileInputFieldsPanel.gameObject.SetActive(false);
        ResetPlayerProfile();
        ResetEditProfilePanel();
        StopAllCoroutines();
        this.gameObject.SetActive(false);
        scriptManager.menuManager.DisablePlayerProfile();
    }
    #endregion
}