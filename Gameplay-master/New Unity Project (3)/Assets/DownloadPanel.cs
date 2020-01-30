using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;

public class DownloadPanel : MonoBehaviour
{
    // UI
    public TextMeshProUGUI songNameText, artistText, creatorText, creatorMessageText, favoriteCountText, playCountText, downloadCountText, rankedDateText,
        easyLevelText, advancedLevelText, extraLevelText;
    private List<Image> instantiatedDownloadButtonImageList = new List<Image>();
    private Image instantiatedDownloadButtonImage;
    public Scrollbar downloadButtonListScrollbar;
    public Image bottomColorPanel;


    // Animator
    public Animator songSelectInformationAnimator;

    // Navigation
    private Button button;
    private Button buttonDown;
    private Button buttonUp;
    private Navigation navigation;

    // Gameobjects
    public GameObject loadingIcon;
    public GameObject downloadPanel;
    public GameObject downloadButton;
    public List<GameObject> instantiatedDownloadButtonList = new List<GameObject>();
    private GameObject downloadButtonInstantiate; // The instantiated beatmap button
    public Transform buttonListContent; // Where the beatmap buttons instantiate to
    private Transform downloadButtonInstantiateChildImage; // Child image for the beatmap button
    private Transform downloadButtonPanelChild; // Beatmap button panel child when instantiating

    // Strings
    private string shaderLocation, downloadUrl;

    // Integers
    public int downloadButtonIndexToGet;
    public int currentLoadedButtonIndex;
    public int totalDownloadableBeatmaps;
    public int totalDownloadsChecked;
    public int totalDownloadImagesChecked;
    public int rowToGet;

    // Bools
    public bool hasLoadedAllBeatmapButtons;
    public bool hasCheckedTotalDownloadCount;
    public bool hasCheckedAllDownloadInformation;
    public bool hasActivatedPanel;

    // Material
    private Material childImageMaterial; // Child image for beatmap buttons
    public Material defautChildImageMaterial; // Default image that is displayed when no file can be found 

    // Vectors
    private Vector3 downloadButtonPosition;

    // Move button list content scroll rect up and down variables
    private GameObject currentSelectedDownloadButton;
    private DownloadButton selectedDownloadButtonScript;
    private int currentSelectedDownloadButtonIndex;
    private float buttonListContentPositionX;
    private float buttonListContentPositionY;
    private float buttonListContentPositionZ;
    private float newButtonListContentPositionY;
    private Vector3 newButtonListContentPosition;

    private DownloadButton instantiatedDownloadButtonScript;

    // Sorting lists
    public List<DownloadButton> downloadButtonList = new List<DownloadButton>();
    public List<string>[] downloadData;

    // Vector3
    private Vector3 defaultScrollbarPosition, offscreenScrollbarPosition;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
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

        bottomColorPanel.color = scriptManager.songSelectManager.topColorPanelImage.color;
    }

    private void OnDisable()
    {
        loadingIcon.gameObject.SetActive(false);
    }

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
        defaultScrollbarPosition = downloadButtonListScrollbar.transform.position;
        offscreenScrollbarPosition = new Vector3(2000, 2000, 0);
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

        for (int i = 0; i < totalDownloadableBeatmaps; i++)
        {
            instantiatedDownloadButtonList[i].gameObject.SetActive(false);
        }

        loadingIcon.gameObject.SetActive(true);

        songSelectInformationAnimator.gameObject.SetActive(false);

        downloadButtonListScrollbar.transform.position = offscreenScrollbarPosition;
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

        downloadButtonListScrollbar.transform.position = defaultScrollbarPosition;
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

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievetotalbeatmapdownloadcount.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // If there 
        if (www.downloadHandler.text != "0")
        {
            // SUCCESS - Maps to download
            totalDownloadableBeatmaps = int.Parse(www.downloadHandler.text);
            scriptManager.messagePanel.DisplayMessage(totalDownloadableBeatmaps + " BEATMAPS FOUND, LOADING...", "PURPLE");
        }
        else
        {
            // FAILED - No maps to download
            totalDownloadableBeatmaps = 0;
            scriptManager.messagePanel.DisplayMessage("NO BEATMAPS TO DOWNLOAD, TRY AGAIN LATER", "RED");
        }

        // Set to true
        hasCheckedTotalDownloadCount = true;
    }

    // Get the download information for each download button
    private IEnumerator GetDownloadButtonInformation(int _index)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", _index + 1);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrievedownloadinformation.php", form);
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
                $dataArray[0] = $data['song_name'];
                $dataArray[1] = $data['artist_name'];
                $dataArray[2] = $data['creator_name'];
                $dataArray[3] = $data['easy_level'];
                $dataArray[4] = $data['advanced_level'];
                $dataArray[5] = $data['extra_level'];
                $dataArray[6] = $data['total_downloads'];
                $dataArray[7] = $data['total_favorites'];
                $dataArray[8] = $data['total_plays'];
                $dataArray[9] = $data['image_url'];
                $dataArray[10] = $data['download_url'];
                $dataArray[11] = $data['ranked_date'];
                $dataArray[12] = $data['creator_message'];
            */

            // SUCCESS
            if (www.downloadHandler.text != "1")
            {
                // Save the data to the list for this index
                downloadData[_index].Add(placeList[dataType].ToString());
            }
            else
            {
                // ERROR
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

                // Change the beatmap image
                StartCoroutine(LoadNewBeatmapButtonImage(i));

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
            $dataArray[0] = $data['song_name'];
            $dataArray[1] = $data['artist_name'];
            $dataArray[2] = $data['creator_name'];
            $dataArray[3] = $data['easy_level'];
            $dataArray[4] = $data['advanced_level'];
            $dataArray[5] = $data['extra_level'];
            $dataArray[6] = $data['total_downloads'];
            $dataArray[7] = $data['total_favorites'];
            $dataArray[8] = $data['total_plays'];
            $dataArray[9] = $data['image_url'];
            $dataArray[10] = $data['download_url'];
            $dataArray[11] = $data['ranked_date'];
            $dataArray[12] = $data['creator_message'];
        */

        downloadButtonList[_index].songNameText.text = downloadData[_index][0];
        downloadButtonList[_index].artistText.text = downloadData[_index][1];
        downloadButtonList[_index].CreatorName = downloadData[_index][2];
        downloadButtonList[_index].TotalDownloads = downloadData[_index][6];
        downloadButtonList[_index].TotalFavorites = downloadData[_index][7];
        downloadButtonList[_index].TotalPlays = downloadData[_index][8];
        downloadButtonList[_index].ImageUrl = downloadData[_index][9];
        downloadButtonList[_index].DownloadUrl = downloadData[_index][10];
        downloadButtonList[_index].RankedDate = downloadData[_index][11];
        downloadButtonList[_index].CreatorMessage = downloadData[_index][12];

        // If no level found
        if (downloadData[_index][3] == "0")
        {
            // Disable the level gameobject
            downloadButtonList[_index].easyDifficultyImage.gameObject.SetActive(false);
        }
        else
        {
            // Update the level text
            downloadButtonList[_index].easyDifficultyLevelText.text = downloadData[_index][3];
        }

        if (downloadData[_index][4] == "0")
        {
            downloadButtonList[_index].advancedDifficultyImage.gameObject.SetActive(false);
        }
        else
        {
            downloadButtonList[_index].advancedDifficultyLevelText.text = downloadData[_index][4];
        }

        if (downloadData[_index][5] == "0")
        {
            downloadButtonList[_index].extraDifficultyImage.gameObject.SetActive(false);
        }
        else
        {
            downloadButtonList[_index].extraDifficultyLevelText.text = downloadData[_index][5];
        }

        downloadButtonList[_index].EasyLevel = downloadData[_index][3];
        downloadButtonList[_index].AdvancedLevel = downloadData[_index][4];
        downloadButtonList[_index].ExtraLevel = downloadData[_index][5];
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

        // Get the child image transform from the instantiated button so we can change the image
        downloadButtonPanelChild = downloadButtonInstantiate.gameObject.transform.GetChild(1);
        downloadButtonInstantiateChildImage = downloadButtonPanelChild.gameObject.transform.GetChild(0);

        // Get the image component of the instantiated button
        instantiatedDownloadButtonImage = downloadButtonInstantiateChildImage.GetComponent<Image>();

        // Store in the list so we can change it later
        instantiatedDownloadButtonImageList.Add(instantiatedDownloadButtonImage);

        // Get the beatmap button script component attached to the newly instantiated button
        // Assign the beatmap index to load inside the script

        // Create a new material for the button to assign the beatmap file image to 
        childImageMaterial = new Material(Shader.Find(shaderLocation));
        instantiatedDownloadButtonImage.material = childImageMaterial;

        instantiatedDownloadButtonScript = downloadButtonInstantiate.GetComponent<DownloadButton>();
        instantiatedDownloadButtonScript.SetBeatmapButtonIndex(_buttonIndex);

        // Add the beatmap button to the list
        downloadButtonList.Add(instantiatedDownloadButtonScript);
    }

    // Load a new beatmap image for the beatmap button instantiated
    private IEnumerator LoadNewBeatmapButtonImage(int _buttonIndex)
    {
       string completePath = downloadData[_buttonIndex][9];

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                // Update the beatmap button image material to the default material
                instantiatedDownloadButtonImageList[_buttonIndex].material = defautChildImageMaterial;
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Update the beatmap button image material to the file found
                instantiatedDownloadButtonImageList[_buttonIndex].material.mainTexture = texture;
            }
        }

        // Increment total downloaded images
        totalDownloadImagesChecked++;

        yield return null;
    }

    // Download the beatmap from the URL
    public void DownloadBeatmap()
    {
        Application.OpenURL(downloadUrl);

        scriptManager.messagePanel.DisplayMessage("DOWNLOADING BEATMAP", "BLUE");
    }
}
