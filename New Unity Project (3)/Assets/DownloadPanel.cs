using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;

public class DownloadPanel : MonoBehaviour
{
    #region Variables
    // Text    
    public TextMeshProUGUI songNameText, artistText, creatorText, creatorMessageText, downloadStatText, downloadProgressText;

    // Image
    public Image downloadSliderBackgroundImage;

    // Scrollbar
    public Scrollbar downloadButtonListScrollbar;

    // Slider
    public Slider downloadSlider;

    // Canvas group
    public CanvasGroup downloadButtonListScrollbarCanvasGroup;

    // Animator
    public Animator songSelectInformationAnimator;

    // Button
    public Button downloadOpenFolderButton;

    // Navigation
    private Button button, buttonDown, buttonUp;
    private Navigation navigation;

    // Gameobjects
    public GameObject loadingIcon, downloadButton, downloadPanel;
    public List<GameObject> instantiatedDownloadButtonList = new List<GameObject>();
    private GameObject downloadButtonInstantiate; // The instantiated beatmap button
    private GameObject currentSelectedDownloadButton;

    // Transform
    public Transform buttonListContent; // Where the beatmap buttons instantiate to

    // Strings
    private string shaderLocation, downloadUrl, sorting;
    public List<string>[] downloadData;
    /*
    private const string SONG_NAME = "song_name", ARTIST_NAME = "artist_name", CREATOR_NAME = "creator_name",
        EASY_DIFFICULTY_LEVEL = "easy_difficulty_level", NORMAL_DIFFICULTY_LEVEL = "normal_difficulty_level",
        HARD_DIFFICULTY_LEVEL = "hard_difficulty_level", RANKED_DATE = "ranked_date", DOWNLOADS = "downloads", PLAYS = "plays";
        */

    // Integers
    private int downloadButtonIndexToGet, currentLoadedButtonIndex, totalDownloadableBeatmaps, totalDownloadsChecked,
        totalDownloadImagesChecked, rowToGet, currentSelectedDownloadButtonIndex;
    private float buttonListContentPositionX, buttonListContentPositionY, buttonListContentPositionZ, newButtonListContentPositionY;

    // Bools
    public bool hasLoadedAllBeatmapButtons, hasCheckedTotalDownloadCount, hasCheckedAllDownloadInformation, hasActivatedPanel;

    // Material
    private Material beatmapImageMaterial, defautBeatmapImageMaterial;

    // Vectors
    private Vector3 downloadButtonPosition, newButtonListContentPosition;

    // Scripts
    private ScriptManager scriptManager;
    private DownloadButton instantiatedDownloadButtonScript, selectedDownloadButtonScript;
    public List<DownloadButton> downloadButtonList = new List<DownloadButton>();
    public DifficultyButton easyDifficultyButtonScript, normalDifficultyButtonScript, hardDifficultyButtonScript;
    #endregion

    #region Properties
    public string DownloadUrl
    {
        set { downloadUrl = value; }
    }

    private void OnEnable()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
    }

    private void OnDisable()
    {
        loadingIcon.gameObject.SetActive(false);
    }
    #endregion

    #region Functions
    #endregion

    // Use this for initialization
    void Start()
    {
        // Initialize
        rowToGet = 1;
        totalDownloadsChecked = 0;
        downloadButtonIndexToGet = 0;
        downloadButtonPosition = new Vector3(0, 0, 500); // Set to 500 on z to fix the "moving image" problem, instantiates the images to z of 0 so the images don't move when the mouse cursor has moved
        shaderLocation = "UI/Unlit/Transparent";
        hasCheckedTotalDownloadCount = false;
        hasLoadedAllBeatmapButtons = false;
        hasActivatedPanel = false;

        // Get the total number of beatmaps to download
        StartCoroutine(GetTotalDownloadCount());

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Deactivate panel
        DeactivateDownloadPanel();
    }

    private void Update()
    {
        if (scriptManager.menuManager.downloadMenu.gameObject.activeSelf == true)
        {
            if (hasCheckedTotalDownloadCount == true && totalDownloadableBeatmaps != 0)
            {
                if (hasCheckedAllDownloadInformation == false)
                {
                    // Retrieve all download button information for all buttons
                    for (sbyte i = 0; i < totalDownloadableBeatmaps; i++)
                    {
                        // Initialize the downloadData list
                        InitializeDownloadDataList();

                        // Get all download button information
                        StartCoroutine(GetDownloadButtonInformation(i));
                    }

                    // Set to true to prevent looping requests
                    hasCheckedAllDownloadInformation = true;
                }


                if (totalDownloadsChecked == totalDownloadableBeatmaps)
                {
                    // Create all beatmap buttons to go in the song select panel
                    CreateDownloadPanel();

                    if (totalDownloadImagesChecked == totalDownloadableBeatmaps)
                    {
                        // Update the information for each button

                        if (hasActivatedPanel == false)
                        {
                            // Enable the panel
                            ActivateDownloadPanel();

                            hasActivatedPanel = true;
                        }
                    }

                    // Download
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (downloadUrl != null)
                        {
                            DownloadBeatmap();
                        }
                    }
                }
            }
        }
    }

    private void DeactivateDownloadPanel()
    {
        // Deactivate at the start
        downloadPanel.gameObject.SetActive(false);
    }

    private void ActivateDownloadPanel()
    {
        downloadPanel.gameObject.SetActive(true);

        for (int i = 0; i < totalDownloadableBeatmaps; i++)
        {
            instantiatedDownloadButtonList[i].gameObject.SetActive(true);
        }

        loadingIcon.gameObject.SetActive(false);

        // Select first button in the list
        downloadButtonList[0].GetComponent<Button>().Select();

        songSelectInformationAnimator.gameObject.SetActive(true);

        downloadButtonListScrollbarCanvasGroup.alpha = 1;
        downloadButtonListScrollbar.interactable = true;
    }

    // Initialize the downloadData list
    private void InitializeDownloadDataList()
    {
        // Initialize the list to the size of total downloads
        downloadData = new List<string>[totalDownloadableBeatmaps];

        // Instantiate the lists
        for (int i = 0; i < downloadData.Length; i++)
        {
            downloadData[i] = new List<string>();
        }
    }

    // Get the total amount of beatmaps to download/rows that are in the beatmap download table
    private IEnumerator GetTotalDownloadCount()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_total_beatmap_download_count.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        switch (www.downloadHandler.text)
        {
            case "ERROR":
                // FAILED - No maps to download
                totalDownloadableBeatmaps = 0;
                scriptManager.messagePanel.DisplayMessage("NO BEATMAPS TO DOWNLOAD, TRY AGAIN LATER",
                    scriptManager.uiColorManager.offlineColorSolid); break;
            default:
                // SUCCESS - Maps to download
                totalDownloadableBeatmaps = int.Parse(www.downloadHandler.text);
                scriptManager.messagePanel.DisplayMessage(totalDownloadableBeatmaps + " BEATMAPS FOUND, LOADING...", 
                    scriptManager.uiColorManager.purpleColor);
                break;
        }

        // Set to true
        hasCheckedTotalDownloadCount = true;
    }

    // Get the download information for each download button
    private IEnumerator GetDownloadButtonInformation(int _index)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", _index);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_beatmap_download.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Stores all the placement information from the database
        ArrayList placeList = new ArrayList();

        // Split the information retrieved from the database
        placeList.AddRange(Regex.Split(www.downloadHandler.text, "->"));

        // Loop and assign the data to the list
        for (sbyte dataType = 0; dataType < 13; dataType++)
        {
            /*
                $returnarray[0] = $infoarray["song_name"];
                $returnarray[1] = $infoarray["artist_name"];
                $returnarray[2] = $infoarray["creator_name"];
                $returnarray[3] = $infoarray["ranked_date"];
                $returnarray[4] = $infoarray["easy_level"];
                $returnarray[5] = $infoarray["normal_level"];
                $returnarray[6] = $infoarray["hard_level"];
                $returnarray[7] = $infoarray["background_image_url"];
                $returnarray[8] = $infoarray["bpm"];
                $returnarray[9] = $infoarray["creator_message"];
                $returnarray[10] = $infoarray["plays"];
                $returnarray[11] = $infoarray["downloads"];
                $returnarray[12] = $infoarray["download_url"];
            */

            switch (www.downloadHandler.text)
            {
                case "ERROR":
                    break;
                default:
                    // Save the data to the list for this index
                    downloadData[_index].Add(placeList[dataType].ToString());
                    break;
            }
        }

        // Increment total checked
        totalDownloadsChecked++;
    }

    // Create all buttons to go in the download panel
    private void CreateDownloadPanel()
    {
        if (hasLoadedAllBeatmapButtons == false)
        {
            // Load the beatmap buttons in the scroll list
            for (int i = 0; i < totalDownloadableBeatmaps; i++)
            {
                // Instantiate a new beatmap button to go in the song select panel
                InstantiateButton(i);

                // Update the download button information
                UpdateDownloadButtonInformation(i);

                if (i == 0)
                {
                    // Select and load the first download button in the list
                    LoadFirstDownloadButton();
                }
            }

            // Set to true
            hasLoadedAllBeatmapButtons = true;

            // Update beatmap button navigation
            //UpdateBeatmapButtonNavigation(defaultDifficultySortingValue);
        }
    }

    // Update the download button information
    private void UpdateDownloadButtonInformation(int _index)
    {
        /*
            $returnarray[0] = $infoarray["song_name"];
            $returnarray[1] = $infoarray["artist_name"];
            $returnarray[2] = $infoarray["creator_name"];
            $returnarray[3] = $infoarray["ranked_date"];
            $returnarray[4] = $infoarray["easy_level"];
            $returnarray[5] = $infoarray["normal_level"];
            $returnarray[6] = $infoarray["hard_level"];
            $returnarray[7] = $infoarray["background_image_url"];
            $returnarray[8] = $infoarray["bpm"];
            $returnarray[9] = $infoarray["creator_message"];
            $returnarray[10] = $infoarray["plays"];
            $returnarray[11] = $infoarray["downloads"];
            $returnarray[12] = $infoarray["download_url"];
        */

        downloadButtonList[_index].songNameText.text = downloadData[_index][0];
        downloadButtonList[_index].artistText.text = downloadData[_index][1];
        downloadButtonList[_index].CreatorName = downloadData[_index][2];
        downloadButtonList[_index].RankedDate = downloadData[_index][3];
        downloadButtonList[_index].EasyLevel = downloadData[_index][4];
        downloadButtonList[_index].NormalLevel = downloadData[_index][5];
        downloadButtonList[_index].HardLevel = downloadData[_index][6];
        downloadButtonList[_index].ImageUrl = downloadData[_index][7];
        downloadButtonList[_index].Bpm = downloadData[_index][8];
        downloadButtonList[_index].CreatorMessage = downloadData[_index][9];
        downloadButtonList[_index].TotalPlays = downloadData[_index][10];
        downloadButtonList[_index].TotalDownloads = downloadData[_index][11];
        downloadButtonList[_index].DownloadUrl = downloadData[_index][12];

        switch (downloadData[_index][4])
        {
            case "0":
                downloadButtonList[_index].easyDifficultyLevelText.gameObject.SetActive(false);
                break;
            default:
                downloadButtonList[_index].easyDifficultyLevelText.text = downloadData[_index][4];
                break;
        }

        switch (downloadData[_index][5])
        {
            case "0":
                downloadButtonList[_index].normalDifficultyLevelText.gameObject.SetActive(false);
                break;
            default:
                downloadButtonList[_index].normalDifficultyLevelText.text = downloadData[_index][5];
                break;
        }

        switch (downloadData[_index][6])
        {
            case "0":
                downloadButtonList[_index].hardDifficultyLevelText.gameObject.SetActive(false);
                break;
            default:
                downloadButtonList[_index].hardDifficultyLevelText.text = downloadData[_index][6];
                break;
        }
    }

    // Load the first download button in the list
    private void LoadFirstDownloadButton()
    {
        // Load first button
        downloadButtonList[0].LoadBeatmap();
    }

    // Update beatmap button navigation
    private void UpdateDownloadButtonNavigation(string _sortingMethod)
    {
        for (int i = 0; i < downloadButtonList.Count; i++)
        {
            // Get current button setting navigation for
            button = downloadButtonList[i].GetComponent<Button>();

            // Get the Navigation data
            navigation = button.navigation;

            // Switch mode to Explicit to allow for custom assigned behavior
            navigation.mode = Navigation.Mode.Explicit;

            // Get button down
            if (i == downloadButtonList.Count - 1)
            {
                // If at end of list set down navigation to own button
                buttonDown = button;
            }
            else
            {
                // Set button down to the next button in the list
                buttonDown = downloadButtonList[i + 1].GetComponent<Button>();
            }

            // Get button up
            if (i == 0)
            {
                // If at the start of the list set up button to it's own button
                buttonUp = button;
            }
            else
            {
                // Set button up to the previous button in the list
                buttonUp = downloadButtonList[i - 1].GetComponent<Button>();
            }

            // Set navigation
            navigation.selectOnDown = buttonDown;
            navigation.selectOnUp = buttonUp;

            // Reassign the struct data to the button
            button.navigation = navigation;
        }
    }

    // Instantiate a new beatmap button in to the song select panel
    private void InstantiateButton(int _buttonIndex)
    {
        // Assign the index and image to this button
        downloadButtonInstantiate = Instantiate(downloadButton, downloadButtonPosition, Quaternion.Euler(0, 0, 0),
        buttonListContent) as GameObject;

        // Add the instantiated button to the list
        instantiatedDownloadButtonList.Add(downloadButtonInstantiate);

        // Get button script reference
        instantiatedDownloadButtonScript = downloadButtonInstantiate.GetComponent<DownloadButton>();
        
        // Set button index
        instantiatedDownloadButtonScript.SetBeatmapButtonIndex(_buttonIndex);

        // Create a new material for the button to assign the beatmap file image to 
        instantiatedDownloadButtonScript.beatmapImage.material = new Material(Shader.Find(shaderLocation));

        // Add the beatmap button to the list
        downloadButtonList.Add(instantiatedDownloadButtonScript);

        // Change the beatmap image
        StartCoroutine(LoadNewBeatmapButtonImage(_buttonIndex));
    }

    // Load a new beatmap image for the beatmap button instantiated
    private IEnumerator LoadNewBeatmapButtonImage(int _buttonIndex)
    {
       string completePath = downloadData[_buttonIndex][7];

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                // Update the beatmap button image material to the default material
                downloadButtonList[_buttonIndex].beatmapImage.material = defautBeatmapImageMaterial;
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Update the beatmap button image material to the file found
                downloadButtonList[_buttonIndex].beatmapImage.material.mainTexture = texture;
            }
        }

        // Increment total downloaded images
        totalDownloadImagesChecked++;

        yield return null;
    }

    // Download the beatmap from the URL
    public void DownloadBeatmap()
    {
        /*
        Application.OpenURL(downloadUrl);

        scriptManager.messagePanel.DisplayMessage("DOWNLOADING BEATMAP", scriptManager.uiColorManager.easyDifficultyColor);
        */


        StartCoroutine(DownloadFile());

    }


    IEnumerator DownloadFile()
    {
        var uwr = new UnityWebRequest("https://cdn.discordapp.com/attachments/626149350828933161/689214904896585760/GUEST_Tune_Up_Bounce.zip", UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, "Beatmaps.zip");
        uwr.downloadHandler = new DownloadHandlerFile(path);

        downloadSlider.value = uwr.downloadProgress;

        // Activate slider
        downloadSlider.gameObject.SetActive(true);
        downloadOpenFolderButton.gameObject.SetActive(false);

        uwr.SendWebRequest();

        // While downloading
        while (uwr.isDone == false)
        {
            downloadProgressText.text = "Downloading " + (uwr.downloadProgress * 100).ToString("F0") + "%"; 

            downloadSlider.value = uwr.downloadProgress;

            if (uwr.downloadProgress >= 0 && uwr.downloadProgress < 0.33f)
            {
                downloadSliderBackgroundImage.color = scriptManager.uiColorManager.offlineColor08;
            }
            else if (uwr.downloadProgress >= 0.33f && uwr.downloadProgress < 0.66f)
            {
                downloadSliderBackgroundImage.color = scriptManager.uiColorManager.orangeColor08;
            }
            else if (uwr.downloadProgress >= 0.66f && uwr.downloadProgress < 1f)
            {
                downloadSliderBackgroundImage.color = scriptManager.uiColorManager.onlineColor08;
            }
            yield return null;
        }

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError(uwr.error);
            downloadSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("File successfully downloaded and saved to " + path);
            downloadSliderBackgroundImage.color = scriptManager.uiColorManager.onlineColorSolid;
            downloadProgressText.text = "Download complete";
            downloadOpenFolderButton.gameObject.SetActive(true);
        }
    }
}
