using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DownloadButton : MonoBehaviour
{
    private int beatmapButtonIndex;

    private bool hasEasyDifficulty, hasAdvancedDifficulty, hasExtraDifficulty;

    private string imageUrl, downloadUrl, totalDownloads, totalFavorites, totalPlays, rankedDate, creatorMessage, creatorName, easyLevel, advancedLevel,
        extraLevel;

    public Image overlayColorImage;
    public GameObject easyDifficultyImage, advancedDifficultyImage, extraDifficultyImage;
    public TextMeshProUGUI easyDifficultyLevelText, advancedDifficultyLevelText, extraDifficultyLevelText;
    public TextMeshProUGUI songNameText, artistText, beatmapCreatorText;

    // Scripts
    private ScriptManager scriptManager;

    // Properties

    public int BeatmapButtonIndex
    {
        get { return beatmapButtonIndex; }
        set { beatmapButtonIndex = value; }
    }

    public string EasyLevel
    {
        set { easyLevel = value; }
    }

    public string AdvancedLevel
    {
        set { advancedLevel = value; }
    }

    public string ExtraLevel
    {
        set { extraLevel = value; }
    }

    public string CreatorName
    {
        set { creatorName = value; }
    }

    public string CreatorMessage
    {
        set { creatorMessage = value; }
    }

    public string RankedDate
    {
        set { rankedDate = value; }
    }

    public string TotalDownloads
    {
        set { totalDownloads = value; }
    }

    public string TotalFavorites
    {
        set { totalFavorites = value; }
    }

    public string TotalPlays
    {
        set { totalPlays = value; }
    }

    public string ImageUrl
    {
        set { imageUrl = value; }
    }

    public string DownloadUrl
    {
        set { downloadUrl = value; }
    }

    // Use this for initialization
    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Load the beatmap assigned to the button when clicked
    public void LoadBeatmap()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        scriptManager.uploadPlayerImage.StopAllCoroutines();
        scriptManager.backgroundManager.StopAllCoroutines();

        scriptManager.downloadPanel.songSelectInformationAnimator.gameObject.SetActive(false);

        // Set the new url for the download button
        scriptManager.downloadPanel.DownloadUrl = downloadUrl;

        scriptManager.backgroundManager.LoadBackgroundImageURL(imageUrl);

        // Load beatmap creator profile image
        scriptManager.uploadPlayerImage.CallBeatmapCreatorUploadImage(creatorName, scriptManager.uploadPlayerImage.downloadBeatmapCreatorProfileImage);

        // Play song information panel
        scriptManager.downloadPanel.rankedDateText.text = "RANKED " + rankedDate;
        scriptManager.downloadPanel.creatorText.text = "DESIGNED BY " + creatorName.ToUpper();
        scriptManager.downloadPanel.creatorMessageText.text = creatorMessage;
        scriptManager.downloadPanel.favoriteCountText.text = totalFavorites + " FAVORITES";
        scriptManager.downloadPanel.playCountText.text = totalPlays + " PLAYS";
        scriptManager.downloadPanel.downloadCountText.text = totalDownloads + " DOWNLOADS";

        if (easyLevel != "0")
        {
            scriptManager.downloadPanel.easyLevelText.text = "EASY " + easyLevel;
        }
        else
        {
            scriptManager.downloadPanel.easyLevelText.text = "-";
        }

        if (advancedLevel != "0")
        {
            scriptManager.downloadPanel.advancedLevelText.text = "ADVANCED " + advancedLevel;
        }
        else
        {
            scriptManager.downloadPanel.advancedLevelText.text = "-";
        }

        if (extraLevel != "0")
        {
            scriptManager.downloadPanel.extraLevelText.text = "EXTRA " + extraLevel;
        }
        else
        {
            scriptManager.downloadPanel.extraLevelText.text = "-";
        }
    }

    // Play the song preview when clicked
    private void PlaySongPreview()
    {

    }

    // Set the beatmap butotn index during instantiation
    public void SetBeatmapButtonIndex(int _beatmapButtonIndex)
    {
        beatmapButtonIndex = _beatmapButtonIndex;
    }
}
