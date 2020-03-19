using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;

public class DownloadPanel : MonoBehaviour
{
    #region Variables
    // Dropdown
    public TMP_Dropdown sortingDropdown;

    // Input field
    public TMP_InputField beatmapSearchInputField;

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
    private string shaderLocation;
    public List<string>[] downloadData;

    // Integers
    public int downloadButtonIndexToGet, currentLoadedButtonIndex, totalDownloadableBeatmaps, totalDownloadsChecked,
        totalDownloadImagesChecked, rowToGet, currentSelectedDownloadButtonIndex, totalCreatorImagesChecked;
    private float buttonListContentPositionX, buttonListContentPositionY, buttonListContentPositionZ, newButtonListContentPositionY;

    // Bools
    private bool hasLoadedAllBeatmapButtons, hasCheckedTotalDownloadCount, hasCheckedAllDownloadInformation, hasActivatedPanel;

    // Material
    private Material beatmapImageMaterial, defautBeatmapImageMaterial;

    // Vectors
    private Vector3 downloadButtonPosition, newButtonListContentPosition;

    // Scripts
    private ScriptManager scriptManager;
    private DownloadButton instantiatedDownloadButtonScript, selectedDownloadButtonScript;
    public List<DownloadButton> downloadButtonList = new List<DownloadButton>();
    public List<DownloadButton> listToSort = new List<DownloadButton>();
    #endregion

    #region Properties
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
        totalDownloadImagesChecked = 0;
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
            switch (hasActivatedPanel)
            {
                case false:
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

                            if (totalDownloadImagesChecked == totalDownloadableBeatmaps && totalCreatorImagesChecked == totalDownloadableBeatmaps)
                            {
                                // Update the information for each button
                                if (hasActivatedPanel == false)
                                {
                                    // Enable the panel
                                    ActivateDownloadPanel();

                                    // Sort buttons to new sorting
                                    SortDownloadButtonsToNewSorting();

                                    hasActivatedPanel = true;
                                }
                            }
                        }
                    }
                    break;
                case true:
                    // Check for any input
                    if (Input.anyKeyDown)
                    {
                        // Check for mouse or navigation input
                        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
                        {
                            // Scroll the beatmap button content panel up
                            //ScrollListUp();

                            // Scroll the beatmap button content panel up
                            //ScrollListDown();
                        }
                        else
                        {
                            if (beatmapSearchInputField.isFocused == false)
                            {
                                // Select search bar if any keyboard key has been pressed
                                beatmapSearchInputField.ActivateInputField();
                            }
                        }
                    }
                    break;
            }
        }
    }

    // Search beatmaps with the search bar
    public void SearchBeatmaps()
    {
        listToSort = downloadButtonList;

        // Turn input to upper case
        string beatmapSearchValue = beatmapSearchInputField.text.ToUpper();

        // Button search variables
        string songName, artistName, creatorName, bpm, easyLevel, normalLevel, hardLevel, totalDownloads, totalPlays;
    
        if (beatmapSearchValue.Length == 0)
        {
            SortDownloadButtonsToNewSorting();
        }
        else
        {
            // Loop through all button scripts
            for (int i = 0; i < listToSort.Count; i++)
            {
                songName = listToSort[i].SongName.ToUpper();
                artistName = listToSort[i].ArtistName.ToUpper();
                creatorName = listToSort[i].CreatorName.ToUpper();
                bpm = listToSort[i].Bpm;
                easyLevel = listToSort[i].EasyLevel;
                normalLevel = listToSort[i].NormalLevel;
                hardLevel = listToSort[i].HardLevel;
                totalDownloads = listToSort[i].TotalDownloads;
                totalPlays = listToSort[i].TotalPlays;

                // Search all buttons to see if the current input field text is the same as the song name, artist, beatmap creator or difficulty levels
                if (songName.Contains(beatmapSearchValue) || artistName.Contains(beatmapSearchValue) ||
                    creatorName.Contains(beatmapSearchValue) || bpm.Contains(beatmapSearchValue) || easyLevel.Contains(beatmapSearchValue)
                    || normalLevel.Contains(beatmapSearchValue) || hardLevel.Contains(beatmapSearchValue) ||
                    totalDownloads.Contains(beatmapSearchValue) || totalPlays.Contains(beatmapSearchValue))
                {
                    listToSort[i].gameObject.SetActive(true);
                }
                else
                {
                    listToSort[i].gameObject.SetActive(false);
                }
            }
        }

        // Update navigation of all buttons
        UpdateButtonNavigation();
    }

    private void DeactivateDownloadPanel()
    {
        // Deactivate at the start
        downloadPanel.gameObject.SetActive(false);
    }

    private void ActivateDownloadPanel()
    {
        downloadPanel.gameObject.SetActive(true);
        loadingIcon.gameObject.SetActive(false);
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
                $returnarray[9] = $infoarray["plays"];
                $returnarray[10] = $infoarray["downloads"];
                $returnarray[11] = $infoarray["download_url"];
                $returnarray[12] = $infoarray["image_url"];
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
            $returnarray[9] = $infoarray["plays"];
            $returnarray[10] = $infoarray["downloads"];
            $returnarray[11] = $infoarray["download_url"];
            $returnarray[12] = $infoarray["image_url"];
        */



        downloadButtonList[_index].SongName = downloadData[_index][0];
        downloadButtonList[_index].ArtistName = downloadData[_index][1];
        downloadButtonList[_index].CreatorName = downloadData[_index][2];
        downloadButtonList[_index].RankedDate = downloadData[_index][3];
        downloadButtonList[_index].EasyLevel = downloadData[_index][4];
        downloadButtonList[_index].NormalLevel = downloadData[_index][5];
        downloadButtonList[_index].HardLevel = downloadData[_index][6];
        downloadButtonList[_index].ImageUrl = downloadData[_index][7];
        downloadButtonList[_index].Bpm = downloadData[_index][8];
        downloadButtonList[_index].TotalPlays = downloadData[_index][9];
        downloadButtonList[_index].TotalDownloads = downloadData[_index][10];
        downloadButtonList[_index].DownloadUrl = downloadData[_index][11];

        // Update text
        downloadButtonList[_index].songNameText.text = downloadData[_index][0];
        downloadButtonList[_index].artistText.text = downloadData[_index][1];
        downloadButtonList[_index].beatmapCreatorText.text = downloadData[_index][2];
        downloadButtonList[_index].rankedDateText.text = downloadData[_index][3];
        downloadButtonList[_index].totalDownloadsText.text = "DOWNLOADS: " + downloadData[_index][10];
        downloadButtonList[_index].totalPlaysText.text = "PLAYS: " + downloadData[_index][9];

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

        downloadButtonInstantiate.transform.localPosition = new Vector3(downloadButtonInstantiate.transform.localPosition.x,
            downloadButtonInstantiate.transform.localPosition.y, 0f);

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

        // Create new material for creator button image
        instantiatedDownloadButtonScript.creatorProfileImage.material = new Material(Shader.Find(shaderLocation));

        // Load beatmap creator image
        StartCoroutine(LoadCreatorButtonImage(_buttonIndex));
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

    // Load beatmap creator profile image
    private IEnumerator LoadCreatorButtonImage(int _buttonIndex)
    {
        string completePath = downloadData[_buttonIndex][12];

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {

            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Update the creator profile image
                downloadButtonList[_buttonIndex].creatorProfileImage.material.mainTexture = texture;
            }
        }

        // Increment
        totalCreatorImagesChecked++;

        yield return null;
    }

    // Open directory of beatmaps
    public void OpenBeatmapDirectory()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    // Sort beatmap buttons by song name alphabetical order 
    public void SortDownloadButtonsToNewSorting()
    {
        // Sort the difficulty buttons based on the current difficulty sorting in place
        switch (sortingDropdown.value)
        {
            case 0:
                listToSort = downloadButtonList.OrderBy(x => x.TotalDownloads).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("TOTAL DOWNLOADS [ HIGH TO LOW ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 1:
                listToSort = downloadButtonList.OrderBy(x => x.TotalDownloads).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("TOTAL DOWNLOADS [ LOW TO HIGH ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 2:
                listToSort = downloadButtonList.OrderBy(x => x.TotalPlays).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("TOTAL PLAYS [ HIGH TO LOW ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 3:
                listToSort = downloadButtonList.OrderBy(x => x.TotalPlays).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("TOTAL PLAYS [ LOW TO HIGH ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 4:
                listToSort = downloadButtonList.OrderBy(x => x.SongName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("SONG NAME [ A - Z ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 5:
                listToSort = downloadButtonList.OrderBy(x => x.SongName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("SONG NAME [ Z - A ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 6:
                listToSort = downloadButtonList.OrderBy(x => x.ArtistName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("ARTIST NAME [ A - Z ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 7:
                listToSort = downloadButtonList.OrderBy(x => x.ArtistName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("ARTIST NAME [ Z - A ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 8:
                listToSort = downloadButtonList.OrderBy(x => x.CreatorName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("CREATOR NAME [ A - Z ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 9:
                listToSort = downloadButtonList.OrderBy(x => x.CreatorName).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("CREATOR NAME [ Z - A ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 10:
                listToSort = downloadButtonList.OrderBy(x => x.EasyLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].EasyLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("EASY LEVEL [ 1 - 10 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 11:
                listToSort = downloadButtonList.OrderBy(x => x.EasyLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].EasyLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("EASY LEVEL [ 10 - 1 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 12:
                listToSort = downloadButtonList.OrderBy(x => x.NormalLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].NormalLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("NORMAL LEVEL [ 1 - 10 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 13:
                listToSort = downloadButtonList.OrderBy(x => x.NormalLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].NormalLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("NORMAL LEVEL [ 10 - 1 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 14:
                listToSort = downloadButtonList.OrderBy(x => x.HardLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].HardLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("HARD LEVEL [ 1 - 10 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 15:
                listToSort = downloadButtonList.OrderBy(x => x.HardLevel).ToList();

                for (int i = 0; i < listToSort.Count; i++)
                {
                    switch (listToSort[i].HardLevel)
                    {
                        case "0":
                            listToSort[i].gameObject.SetActive(false);
                            break;
                        default:
                            listToSort[i].gameObject.SetActive(true);
                            break;
                    }
                }

                scriptManager.messagePanel.DisplayMessage("HARD LEVEL [ 10 - 1 ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
            case 16:
                listToSort = downloadButtonList.OrderBy(x => x.Bpm).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("BPM [ LOW TO HIGH ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsLastSibling();
                break;
            case 17:
                listToSort = downloadButtonList.OrderBy(x => x.Bpm).ToList();
                ActivateAllListButtons();
                scriptManager.messagePanel.DisplayMessage("BPM [ HIGH TO LOW ]", scriptManager.uiColorManager.purpleColor);
                SetListButtonsAsFirstSibling();
                break;
        }

        // Update navigation of all buttons
        UpdateButtonNavigation();

        // Play flash animation for all buttons
        StartCoroutine(PlayFullButtonListFlashAnimation());
    }

    // Update download button navigation
    private void UpdateButtonNavigation()
    {
        for (int i = 0; i < listToSort.Count; i++)
        {
            // Get current button setting navigation for
            button = listToSort[i].button;

            // Get the Navigation data
            navigation = button.navigation;

            // Switch mode to Explicit to allow for custom assigned behavior
            navigation.mode = Navigation.Mode.Explicit;

            // Get button down
            if (i == listToSort.Count - 1)
            {
                // If at end of list set down navigation to own button
                buttonDown = button;
            }
            else
            {
                // Set button down to the next button in the list
                buttonDown = listToSort[i + 1].GetComponent<Button>();
            }

            switch (i)
            {
                case 0:
                    // If at the start of the list set up button to it's own button
                    buttonUp = button;
                    break;
                default:
                    // Set button up to the previous button in the list
                    buttonUp = listToSort[i - 1].GetComponent<Button>();
                    break;
            }

            // Set navigation
            navigation.selectOnDown = buttonDown;
            navigation.selectOnUp = buttonUp;

            // Reassign the struct data to the button
            button.navigation = navigation;
        }
    }

    // Activate all list buttons
    private void ActivateAllListButtons()
    {
        for (int i = 0; i < listToSort.Count; i++)
        {
            if (listToSort[i].gameObject.activeSelf == false)
            {
                listToSort[i].gameObject.SetActive(true);
            }
        }
    }

    // Set list buttons as last siblings in the UI canvas
    private void SetListButtonsAsLastSibling()
    {
        // Loop through all button scripts
        for (int i = 0; i < listToSort.Count; i++)
        {
            // Update order of UI buttons
            listToSort[i].transform.SetAsLastSibling();
        }
    }

    // Set list buttons as first siblings in the UI canvas
    private void SetListButtonsAsFirstSibling()
    {
        // Loop through all button scripts
        for (int i = 0; i < listToSort.Count; i++)
        {
            // Update order of UI buttons
            listToSort[i].transform.SetAsFirstSibling();
        }
    }

    // Deselect search bar
    public void DeselectBeatmapSearchbar()
    {
        if (beatmapSearchInputField.text.Length == 0)
        {
            // Sort buttons
            SortDownloadButtonsToNewSorting();
        }
    }

    // Play full button list flash animation
    private IEnumerator PlayFullButtonListFlashAnimation()
    {
        // Activate
        for (int i = 0; i < listToSort.Count; i++)
        {
            listToSort[i].flashAnimationImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        // Deactivate
        for (int i = 0; i < listToSort.Count; i++)
        {
            listToSort[i].flashAnimationImage.gameObject.SetActive(false);
        }
    }

    
}
