using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class DownloadButton : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI easyDifficultyLevelText, normalDifficultyLevelText, hardDifficultyLevelText,
        songNameText, artistText, beatmapCreatorText, downloadProgressText, rankedDateText, totalDownloadsText, totalPlaysText;

    // Gameobject
    public GameObject flashAnimationImage;

    // Slider
    public Slider downloadProgressSlider;

    // Image
    public Image beatmapImage, downloadProgressSliderImage, creatorProfileImage;

    // Button
    public Button button, viewInDirectoryButton;

    // Integer
    private int beatmapButtonIndex;

    // Bool
    private bool hasEasyDifficulty, hasAdvancedDifficulty, hasExtraDifficulty, hasDownloaded;

    // String
    private string imageUrl, downloadUrl, totalDownloads, totalPlays, rankedDate, creatorName, easyLevel, normalLevel,
        hardLevel, bpm, folderDirectory, songName, artistName, tableID;
    private const string FILE_EXTENSION = ".zip", FOLDER = "Beatmaps";

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public int BeatmapButtonIndex
    {
        get { return beatmapButtonIndex; }
        set { beatmapButtonIndex = value; }
    }

    public string TableID
    {
        get { return tableID; }
        set { tableID = value; }
    }

    public string SongName
    {
        get { return songName; }
        set { songName = value; }
    }

    public string ArtistName
    {
        get { return artistName; }
        set { artistName = value; }
    }

    public string EasyLevel
    {
        set { easyLevel = value; }
        get { return easyLevel; }
    }

    public string NormalLevel
    {
        set { normalLevel = value; }
        get { return normalLevel; }
    }

    public string HardLevel
    {
        set { hardLevel = value; }
        get { return hardLevel; }
    }

    public string CreatorName
    {
        set { creatorName = value; }
        get { return creatorName; }
    }

    public string RankedDate
    {
        set { rankedDate = value; }
        get { return rankedDate; }
    }

    public string TotalDownloads
    {
        set { totalDownloads = value; }
        get { return totalDownloads; }
    }

    public string TotalPlays
    {
        set { totalPlays = value; }
        get { return totalPlays; }
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
        get { return bpm; }
    }

    public bool HasDownloaded
    {
        get { return hasDownloaded; }
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

        // Load background image
        scriptManager.backgroundManager.StopAllCoroutines();
        scriptManager.backgroundManager.LoadBackgroundImageURL(imageUrl);
    }

    // Set the beatmap butotn index during instantiation
    public void SetBeatmapButtonIndex(int _beatmapButtonIndex)
    {
        beatmapButtonIndex = _beatmapButtonIndex;
    }

    // Click function for buttons
    public void DownloadBeatmap()
    {
        // Download beatmap file
        StartCoroutine(DownloadBeatmapFile());
    }

    // Download beatmap file
    private IEnumerator DownloadBeatmapFile()
    {
        var uwr = new UnityWebRequest(downloadUrl, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, FOLDER + "/" + creatorName + "_" + songName + FILE_EXTENSION);
        uwr.downloadHandler = new DownloadHandlerFile(path);

        // Assign
        folderDirectory = path;

        yield return null;

        downloadProgressSlider.value = uwr.downloadProgress;

        downloadProgressSlider.gameObject.SetActive(true);
        viewInDirectoryButton.gameObject.SetActive(false);
        button.interactable = false;
        hasDownloaded = false;

        uwr.SendWebRequest();

        // While downloading
        while (uwr.isDone == false)
        {
            downloadProgressText.text = "DOWNLOADING " + (uwr.downloadProgress * 100).ToString("F0") + "%";

            downloadProgressSlider.value = uwr.downloadProgress;

            if (uwr.downloadProgress >= 0 && uwr.downloadProgress < 0.33f)
            {
                downloadProgressSliderImage.color = scriptManager.uiColorManager.offlineColor08;
            }
            else if (uwr.downloadProgress >= 0.33f && uwr.downloadProgress < 0.66f)
            {
                downloadProgressSliderImage.color = scriptManager.uiColorManager.orangeColor08;
            }
            else if (uwr.downloadProgress >= 0.66f && uwr.downloadProgress < 1f)
            {
                downloadProgressSliderImage.color = scriptManager.uiColorManager.onlineColor08;
            }
            yield return null;
        }

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            hasDownloaded = false;
            button.interactable = true;
            viewInDirectoryButton.gameObject.SetActive(false);
            downloadProgressSliderImage.color = scriptManager.uiColorManager.offlineColor08;
            downloadProgressText.text = "DOWNLOAD FAILED";
        }
        else
        {
            hasDownloaded = true;
            downloadProgressSliderImage.color = scriptManager.uiColorManager.onlineColor08;
            downloadProgressText.text = "DOWNLOAD COMPLETE";
            viewInDirectoryButton.gameObject.SetActive(true);
            downloadProgressSlider.value = 1f;
            StartCoroutine(IncrementTotalDownloadCount());
        }
    }

    // Open beatmap folder directory
    public void OpenBeatmapFolder()
    {
        System.Diagnostics.Process.Start(folderDirectory);
    }

    // Increment total downloads for this beatmap in the table
    private IEnumerator IncrementTotalDownloadCount()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", tableID);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/increment_beatmap_download.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        /*
        switch (www.downloadHandler.text)
        {
            case "ERROR":
                //Debug.Log("ERROR");
                break;
            default:
                //Debug.Log("SUCCESS");
                break;
        }
        */
    }
    #endregion
}
