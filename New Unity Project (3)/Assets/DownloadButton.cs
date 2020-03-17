using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DownloadButton : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI easyDifficultyLevelText, normalDifficultyLevelText, hardDifficultyLevelText,
        songNameText, artistText, beatmapCreatorText;

    // Image
    public Image beatmapImage;

    // Integer
    private int beatmapButtonIndex;

    // Bool
    private bool hasEasyDifficulty, hasAdvancedDifficulty, hasExtraDifficulty;

    // String
    private string imageUrl, downloadUrl, totalDownloads, totalPlays, rankedDate, creatorMessage, creatorName, easyLevel, normalLevel,
        hardLevel, bpm;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public int BeatmapButtonIndex
    {
        get { return beatmapButtonIndex; }
        set { beatmapButtonIndex = value; }
    }

    public string EasyLevel
    {
        set { easyLevel = value; }
    }

    public string NormalLevel
    {
        set { normalLevel = value; }
    }

    public string HardLevel
    {
        set { hardLevel = value; }
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

    public string Bpm
    {
        set { bpm = value; }
    }
    #endregion

    #region Functions
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
        scriptManager.downloadPanel.downloadStatText.text = "[ " + totalDownloads + " DOWNLOADS | " + totalPlays + " PLAYS | " +
            bpm + " BPM | " + rankedDate + " ]"; 
        scriptManager.downloadPanel.creatorText.text = "Designed by " + creatorName.ToUpper();
        scriptManager.downloadPanel.creatorMessageText.text = creatorMessage;

        // Update level text and buttons
        switch (easyLevel)
        {
            case "0":
                scriptManager.downloadPanel.easyDifficultyButtonScript.levelText.text = "X";
                scriptManager.downloadPanel.easyDifficultyButtonScript.difficultyButton.interactable = false;
                scriptManager.downloadPanel.easyDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);
                break;
            default:
                scriptManager.downloadPanel.easyDifficultyButtonScript.levelText.text = easyLevel;
                scriptManager.downloadPanel.easyDifficultyButtonScript.difficultyButton.interactable = true;
                scriptManager.downloadPanel.easyDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                break;
        }

        switch (normalLevel)
        {
            case "0":
                scriptManager.downloadPanel.normalDifficultyButtonScript.levelText.text = "X";
                scriptManager.downloadPanel.normalDifficultyButtonScript.difficultyButton.interactable = false;
                scriptManager.downloadPanel.normalDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);
                break;
            default:
                scriptManager.downloadPanel.normalDifficultyButtonScript.levelText.text = normalLevel;
                scriptManager.downloadPanel.normalDifficultyButtonScript.difficultyButton.interactable = true;
                scriptManager.downloadPanel.normalDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                break;
        }

        switch (hardLevel)
        {
            case "0":
                scriptManager.downloadPanel.hardDifficultyButtonScript.levelText.text = "X";
                scriptManager.downloadPanel.hardDifficultyButtonScript.difficultyButton.interactable = false;
                scriptManager.downloadPanel.hardDifficultyButtonScript.selectedGameObject.gameObject.SetActive(false);
                break;
            default:
                scriptManager.downloadPanel.hardDifficultyButtonScript.levelText.text = hardLevel;
                scriptManager.downloadPanel.hardDifficultyButtonScript.difficultyButton.interactable = true;
                scriptManager.downloadPanel.hardDifficultyButtonScript.selectedGameObject.gameObject.SetActive(true);
                break;
        }
    }

    // Set the beatmap butotn index during instantiation
    public void SetBeatmapButtonIndex(int _beatmapButtonIndex)
    {
        beatmapButtonIndex = _beatmapButtonIndex;
    }
    #endregion
}
