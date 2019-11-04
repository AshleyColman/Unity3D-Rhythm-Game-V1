using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class SongSelectPanel : MonoBehaviour
{
    // Animation
    public Animator selectedBeatmapCountTextAnimator;

    // UI
    public TextMeshProUGUI totalBeatmapCountText, selectedBeatmapCountText;
    public TMP_InputField beatmapSearchInputField;
    public Image easyDifficultyTickBoxSelectedImage, advancedDifficultyTickBoxSelectedImage, extraDifficultyTickBoxSelectedImage;
    private List<Image> instantiatedBeatmapButtonImageList = new List<Image>();
    private Image instantiatedBeatmapButtonImage;
    public Scrollbar beatmapButtonListScrollbar;

    // Gameobjects
    public GameObject beatmapButton, editSelectSceneBeatmapButton; // The button to instantiate
    public List<GameObject> instantiatedBeatmapButtonList = new List<GameObject>();
    private GameObject beatmapButtonInstantiate; // The instantiated beatmap button
    public Transform buttonListContent; // Where the beatmap buttons instantiate to
    private Transform beatmapButtonInstantiateChildImage; // Child image for the beatmap button

    // Color
    public Color easyDifficultyColor, advancedDifficultyColor, extraDifficultyColor, allDifficultyColor, defaultDifficultyColor;

    // Strings
    private string imageName;
    private string imageType;
    public string[] beatmapDirectoryPaths; // The beatmap directory paths
    private string shaderLocation;
    string completePath;
    string fileCheckPath;
    string easyFileCheckPath, advancedFileCheckPath, extraFileCheckPath;
    string currentDifficultySorting;
    const string easyDifficultySortingValue = "easy", advancedDifficultySortingValue = "advanced", extraDifficultySortingValue = "extra",
        allDifficultySortingValue = "all", defaultDifficultySortingValue = "default";

    // Integers
    private int beatmapButtonIndexToGet;
    private int currentLoadedButtonIndex;
    private int currentSortedListSize;

    // Bools
    private bool hasLoadedAllBeatmapDirectories;
    private bool hasLoadedAllBeatmapButtons;
    private bool easyDifficultyTickBoxSelected, advancedDifficultyTickBoxSelected, extraDifficultyTickBoxSelected;

    // Material
    private Material childImageMaterial; // Child image for beatmap buttons
    public Material defautChildImageMaterial; // Default image that is displayed when no file can be found 
    
    // Vectors
    private Vector3 beatmapButtonPosition;

    // Scripts
    private SongSelectManager songSelectManager; // Reference to the song select manager which manages loading songs, used to get the beatmap img addresses for loading images
    private EditSelectSceneSongSelectManager editSelectSceneSongSelectManager; // Song select manager for the edit select scene
    private MenuManager menuManager;
    private SongSelectMenuFlash songSelectMenuFlash;
    private MessagePanel messagePanel;

    // Move button list content scroll rect up and down variables
    private GameObject currentSelectedBeatmapButton;
    private BeatmapButton selectedBeatmapButtonScript;
    private int currentSelectedBeatmapButtonIndex;
    private float buttonListContentPositionX;
    private float buttonListContentPositionY;
    private float buttonListContentPositionZ;
    private float newButtonListContentPositionY;
    private Vector3 newButtonListContentPosition;

    // Sorting lists
    public List<BeatmapButton> beatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> artistSortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> creatorSortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> easyDifficultySortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> advancedDifficultySortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> extraDifficultySortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> allDifficultySortedBeatmapButtonList = new List<BeatmapButton>();
    public List<BeatmapButton> searchedBeatmapsList = new List<BeatmapButton>();

    // Properties
    public bool HasLoadedAllBeatmapButtons
    {
        get { return hasLoadedAllBeatmapButtons; }
    }

    public string CurrentDifficultySorting
    {
        get { return currentDifficultySorting; }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        beatmapButtonIndexToGet = 0;
        easyDifficultyTickBoxSelected = true;
        advancedDifficultyTickBoxSelected = true;
        extraDifficultyTickBoxSelected = true;
        hasLoadedAllBeatmapDirectories = false;  // Set to false at the start, set to true when all have loaded
        beatmapButtonPosition = new Vector3(0, 0, 500); // Set to 500 on z to fix the "moving image" problem, instantiates the images to z of 0 so the images don't move when the mouse cursor has moved
        shaderLocation = "UI/Unlit/Transparent";
        imageType = ".png";
        imageName = "img";
        completePath = "";
        fileCheckPath = "";
        currentDifficultySorting = allDifficultySortingValue;

        // Reference
        menuManager = FindObjectOfType<MenuManager>();
        songSelectManager = FindObjectOfType<SongSelectManager>();
        editSelectSceneSongSelectManager = FindObjectOfType<EditSelectSceneSongSelectManager>();
        songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        messagePanel = FindObjectOfType<MessagePanel>();
    }

    private void Update()
    {
        if (hasLoadedAllBeatmapDirectories == false)
        {
            // Create all beatmap buttons to go in the song select panel
            CreateSongSelectPanel();

            hasLoadedAllBeatmapDirectories = true;
        }

        // Check input for song select panel objects
        CheckSongSelectPanelInput();
    }

    // Check input for song select panel features
    private void CheckSongSelectPanelInput()
    {
        // Check for any input
        if (Input.anyKeyDown)
        {
            // Check for mouse or navigation input
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
            {
                // Get the selected 
                currentSelectedBeatmapButton = EventSystem.current.currentSelectedGameObject;
                if (currentSelectedBeatmapButton != null)
                {
                    selectedBeatmapButtonScript = currentSelectedBeatmapButton.GetComponent<BeatmapButton>();

                    switch (currentDifficultySorting)
                    {
                        case defaultDifficultySortingValue:
                            currentSelectedBeatmapButtonIndex = beatmapButtonList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = beatmapButtonList.Count;
                            break;
                        case easyDifficultySortingValue:
                            currentSelectedBeatmapButtonIndex = easyDifficultySortedBeatmapButtonList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = easyDifficultySortedBeatmapButtonList.Count;
                            break;
                        case advancedDifficultySortingValue:
                            currentSelectedBeatmapButtonIndex = advancedDifficultySortedBeatmapButtonList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = advancedDifficultySortedBeatmapButtonList.Count;
                            break;
                        case extraDifficultySortingValue:
                            currentSelectedBeatmapButtonIndex = extraDifficultySortedBeatmapButtonList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = extraDifficultySortedBeatmapButtonList.Count;
                            break;
                        case allDifficultySortingValue:
                            currentSelectedBeatmapButtonIndex = allDifficultySortedBeatmapButtonList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = allDifficultySortedBeatmapButtonList.Count;
                            break;
                        case "searched":
                            currentSelectedBeatmapButtonIndex = searchedBeatmapsList.IndexOf(selectedBeatmapButtonScript);
                            currentSortedListSize = searchedBeatmapsList.Count;
                            break;
                    }

                    // Get the current position of the button list content panel
                    buttonListContentPositionX = buttonListContent.transform.localPosition.x;
                    buttonListContentPositionY = buttonListContent.transform.localPosition.y;
                    buttonListContentPositionZ = buttonListContent.transform.localPosition.z;

                    // Scroll the beatmap button content panel up
                    ScrollListUp();

                    // Scroll the beatmap button content panel up
                    ScrollListDown();
                }

                // Select the next difficulty
                SelectNextDifficulty();
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
    }

    // Select the next difficulty
    private void SelectNextDifficulty()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                // Find index where the beatmap button index matches up with the current selected beatmap directory index
                if (beatmapButtonList[i].BeatmapButtonIndex == songSelectManager.SelectedBeatmapDirectoryIndex)
                {
                    // If right arrow key was pressed
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        // Select based on current selected difficulty
                        switch (songSelectManager.CurrentBeatmapDifficulty)
                        {
                            // If the current selected difficulty is easy
                            case "easy":
                                // Check if advanced difficulty exists
                                if (beatmapButtonList[i].HasAdvancedDifficulty == true)
                                {
                                    // Load the advanced difficulty 
                                    songSelectMenuFlash.LoadBeatmapDifficulty("advanced");
                                }
                                break;
                            case "advanced":
                                if (beatmapButtonList[i].HasExtraDifficulty == true)
                                {
                                    songSelectMenuFlash.LoadBeatmapDifficulty("extra");
                                }
                                break;
                        }
                    }

                    // If left arrow key was pressed
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        // Select based on current selected difficulty
                        switch (songSelectManager.CurrentBeatmapDifficulty)
                        {
                            // If the current selected difficulty is easy
                            case "advanced":
                                // Check if advanced difficulty exists
                                if (beatmapButtonList[i].HasEasyDifficulty == true)
                                {
                                    // Load the easy difficulty 
                                    songSelectMenuFlash.LoadBeatmapDifficulty("easy");
                                }
                                break;
                            case "extra":
                                if (beatmapButtonList[i].HasAdvancedDifficulty == true)
                                {
                                    songSelectMenuFlash.LoadBeatmapDifficulty("advanced");
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    // Set the button scroll list to the top
    public void SetButtonScrollListToTop()
    {
        // Create the new Y position
        newButtonListContentPositionY = buttonListContentPositionY - 10000;
        newButtonListContentPosition = new Vector3(buttonListContentPositionX, newButtonListContentPositionY, buttonListContentPositionZ);

        // Assign new position to move the button list content panel up
        buttonListContent.transform.localPosition = newButtonListContentPosition;
    }

    // Set the button scroll list to the bottom
    public void SetButtonScrollListToBottom()
    {
        // Create the new Y position
        newButtonListContentPositionY = buttonListContentPositionY + 10000;
        newButtonListContentPosition = new Vector3(buttonListContentPositionX, newButtonListContentPositionY, buttonListContentPositionZ);

        // Assign new position to move the button list content panel up
        buttonListContent.transform.localPosition = newButtonListContentPosition;
    }

    // Scroll the beatmap button content panel up
    private void ScrollListUp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSelectedBeatmapButtonIndex < currentSortedListSize - 4 && currentSelectedBeatmapButtonIndex > 2)
            {
                // Create the new Y position
                newButtonListContentPositionY = buttonListContentPositionY - 110;
                newButtonListContentPosition = new Vector3(buttonListContentPositionX, newButtonListContentPositionY, buttonListContentPositionZ);

                // Assign new position to move the button list content panel up
                buttonListContent.transform.localPosition = newButtonListContentPosition;
            }
        }
    }

    // Scroll the beatmap button content panel up
    private void ScrollListDown()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSelectedBeatmapButtonIndex > 3 && currentSelectedBeatmapButtonIndex < currentSortedListSize - 3)
            {
                // Create the new Y position
                newButtonListContentPositionY = buttonListContentPositionY + 110;
                newButtonListContentPosition = new Vector3(buttonListContentPositionX, newButtonListContentPositionY, buttonListContentPositionZ);

                // Assign new position to move the button list content panel up
                buttonListContent.transform.localPosition = newButtonListContentPosition;
            }
        }
    }

    // Create all beatmap buttons to go in the song select panel
    private void CreateSongSelectPanel()
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true && hasLoadedAllBeatmapButtons == false)
        {
            // Load the beatmap buttons in the scroll list
            for (int beatmapButtonIndex = 0; beatmapButtonIndex < songSelectManager.beatmapDirectories.Length; beatmapButtonIndex++)
            {
                // Instantiate a new beatmap button to go in the song select panel
                InstantiateBeatmapButton(beatmapButtonIndexToGet);

                // Change the beatmap image
                StartCoroutine(LoadNewBeatmapButtonImage(beatmapButtonIndexToGet));

                // Check beatmap difficulties
                CheckBeatmapDifficulties(beatmapButtonIndexToGet);

                // Increment the beatmapButtonIndexToGet (Increments after each coroutine only so it loads the image 1 after another)
                beatmapButtonIndexToGet++;
            }

            // Set default navigation for all instantiated beatmap buttons
            //SetDefaultBeatmapButtonNavigation();

            // Set to true
            hasLoadedAllBeatmapButtons = true;

            // Sort by default
            SortBeatmapButtonsDefault();

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("default");

            // Select first button in the list 
            beatmapButtonList[0].GetComponent<Button>().Select();
        }
    }

    // Get the beatmap directory paths
    public void GetBeatmapDirectoryPaths()
    {
        // Initialise the array with the amount of beatmap directories found
        // Get the beatmap directoriess
        beatmapDirectoryPaths = Directory.GetDirectories(@"c:\Beatmaps");
    }

    // Get all the directories that the logged in user is allowed to edit in the edit select song scene
    public void GetLoggedInUserEditableBeatmapDirectories()
    {
        if (editSelectSceneSongSelectManager == null)
        {
            editSelectSceneSongSelectManager = FindObjectOfType<EditSelectSceneSongSelectManager>();
        }

        // Update the array size
        beatmapDirectoryPaths = new string[editSelectSceneSongSelectManager.userCreatedBeatmapDirectories.Count];

        // Loop through all user created beatmap directories
        for (int i = 0; i < editSelectSceneSongSelectManager.userCreatedBeatmapDirectories.Count; i++)
        {

            // Get the directory
            beatmapDirectoryPaths[i] = editSelectSceneSongSelectManager.userCreatedBeatmapDirectories[i];
        }
    }

    // Update beatmap button navigation
    private void UpdateBeatmapButtonNavigation(string _sortingMethod)
    {
        // Create a new list
        List<BeatmapButton> listToSortBy = new List<BeatmapButton>();

        // Get the list to sort based on the sorting method
        switch (_sortingMethod)
        {
            case "default":
                listToSortBy = beatmapButtonList;
                break;
            case "artist":
                listToSortBy = artistSortedBeatmapButtonList;
                break;
            case "creator":
                listToSortBy = creatorSortedBeatmapButtonList;
                break;
            case "easyDifficulty":
                listToSortBy = easyDifficultySortedBeatmapButtonList;
                break;
            case "advancedDifficulty":
                listToSortBy = advancedDifficultySortedBeatmapButtonList;
                break;
            case "extraDifficulty":
                listToSortBy = extraDifficultySortedBeatmapButtonList;
                break;
            case "allDifficulty":
                listToSortBy = allDifficultySortedBeatmapButtonList;
                break;
            case "searched":
                listToSortBy = searchedBeatmapsList;
                break;
        }

        for (int i = 0; i < listToSortBy.Count; i++)
        {
            // Get current button setting navigation for
            Button button = listToSortBy[i].GetComponent<Button>();
            Button buttonDown;
            Button buttonUp;

            // Get the Navigation data
            Navigation navigation = button.navigation;

            // Switch mode to Explicit to allow for custom assigned behavior
            navigation.mode = Navigation.Mode.Explicit;

            // Get button down
            if (i == listToSortBy.Count - 1)
            {
                // If at end of list set down navigation to own button
                buttonDown = button;
            }
            else
            {
                // Set button down to the next button in the list
                buttonDown = listToSortBy[i + 1].GetComponent<Button>();
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
                buttonUp = listToSortBy[i - 1].GetComponent<Button>();
            }

            // Set navigation
            navigation.selectOnDown = buttonDown;
            navigation.selectOnUp = buttonUp;

            // Reassign the struct data to the button
            button.navigation = navigation;
        }
    }

    // Instantiate a new beatmap button in to the song select panel
    private void InstantiateBeatmapButton(int _beatmapButtonIndex)
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Assign the index and image to this button
            beatmapButtonInstantiate = Instantiate(beatmapButton, beatmapButtonPosition, Quaternion.Euler(0, 0, 0),
            buttonListContent) as GameObject;
        }

        // Add the instantiated button to the list
        instantiatedBeatmapButtonList.Add(beatmapButtonInstantiate);

        // Get the child image transform from the instantiated button so we can change the image
        Transform beatmapButtonPanelChild = beatmapButtonInstantiate.gameObject.transform.GetChild(1);
        beatmapButtonInstantiateChildImage = beatmapButtonPanelChild.gameObject.transform.GetChild(0);

        // Get the image component of the instantiated button
        instantiatedBeatmapButtonImage = beatmapButtonInstantiateChildImage.GetComponent<Image>();

        // Store in the list so we can change it later
        instantiatedBeatmapButtonImageList.Add(instantiatedBeatmapButtonImage);

        // Get the beatmap button script component attached to the newly instantiated button
        // Assign the beatmap index to load inside the script

        // Create a new material for the button to assign the beatmap file image to 
        childImageMaterial = new Material(Shader.Find(shaderLocation));
        instantiatedBeatmapButtonImage.material = childImageMaterial;

        BeatmapButton instantiatedBeatmapButtonScript = beatmapButtonInstantiate.GetComponent<BeatmapButton>();
        instantiatedBeatmapButtonScript.SetBeatmapButtonIndex(_beatmapButtonIndex);

        // Add the beatmap button to the list
        beatmapButtonList.Add(instantiatedBeatmapButtonScript);
    }

    // Load a new beatmap image for the beatmap button instantiated
    private IEnumerator LoadNewBeatmapButtonImage(int _beatmapButtonIndex)
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            completePath = "file://" + songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + imageName + imageType;

            fileCheckPath = songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + imageName + imageType;
        }

        // Check if the image file exists
        // If the file doesn't exist
        if (File.Exists(fileCheckPath) == false)
        {
            // Update the beatmap button image material to the default material
            instantiatedBeatmapButtonImageList[_beatmapButtonIndex].material = defautChildImageMaterial;
        }
        else
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(completePath))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    //Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);

                    // Update the beatmap button image material to the file found
                    instantiatedBeatmapButtonImageList[_beatmapButtonIndex].material.mainTexture = texture;

                    // Set image to false then to true to activate new image
                    instantiatedBeatmapButtonList[_beatmapButtonIndex].gameObject.SetActive(false);
                    instantiatedBeatmapButtonList[_beatmapButtonIndex].gameObject.SetActive(true);
                }
            }
        }
    }

    // Check the beatmap difficulties and enable/disable the difficulty level image
    private void CheckBeatmapDifficulties(int _beatmapButtonIndex)
    {
        if (menuManager.songSelectMenu.gameObject.activeSelf == true)
        {
            // Easy file path
            easyFileCheckPath = songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + songSelectManager.EasyBeatmapFileName;

            // Advanced file path
            advancedFileCheckPath = songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + songSelectManager.AdvancedBeatmapFileName;

            // Extra file path
            extraFileCheckPath = songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + songSelectManager.ExtraBeatmapFileName;
        }

        // If the easy file exists
        if (File.Exists(easyFileCheckPath) == true)
        {
            // Enable difficulty image
            beatmapButtonList[_beatmapButtonIndex].easyDifficultyImage.gameObject.SetActive(true);
            // Enable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].easyDifficultyLevelText.gameObject.SetActive(true);

            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(songSelectManager.beatmapDirectories[_beatmapButtonIndex], "easy");
            // Load difficulty level text
            beatmapButtonList[_beatmapButtonIndex].easyDifficultyLevelText.text = Database.database.LoadedBeatmapEasyDifficultyLevel;
            // Load beatmap button text information
            beatmapButtonList[_beatmapButtonIndex].songNameText.text = Database.database.LoadedSongName;
            beatmapButtonList[_beatmapButtonIndex].artistText.text = Database.database.LoadedSongArtist;
            beatmapButtonList[_beatmapButtonIndex].beatmapCreatorText.text = Database.database.LoadedBeatmapCreator;
            // Clear the database
            Database.database.Clear();

            // Has difficulty
            beatmapButtonList[_beatmapButtonIndex].HasEasyDifficulty = true;
        }
        else
        {
            // Disable difficulty image
            beatmapButtonList[_beatmapButtonIndex].easyDifficultyImage.gameObject.SetActive(false);
            // Disable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].easyDifficultyLevelText.gameObject.SetActive(false);
        }

        // If the advanced file exists
        if (File.Exists(advancedFileCheckPath) == true)
        {
            // Enable difficulty image
            beatmapButtonList[_beatmapButtonIndex].advancedDifficultyImage.gameObject.SetActive(true);
            // Enable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].advancedDifficultyLevelText.gameObject.SetActive(true);


            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(songSelectManager.beatmapDirectories[_beatmapButtonIndex], "advanced");
            // Load difficulty level text
            beatmapButtonList[_beatmapButtonIndex].advancedDifficultyLevelText.text = Database.database.LoadedBeatmapAdvancedDifficultyLevel;
            // Load beatmap button text information
            beatmapButtonList[_beatmapButtonIndex].songNameText.text = Database.database.LoadedSongName;
            beatmapButtonList[_beatmapButtonIndex].artistText.text = Database.database.LoadedSongArtist;
            beatmapButtonList[_beatmapButtonIndex].beatmapCreatorText.text = Database.database.LoadedBeatmapCreator;
            // Clear the database
            Database.database.Clear();

            // Has difficulty
            beatmapButtonList[_beatmapButtonIndex].HasAdvancedDifficulty = true;
        }
        else
        {
            // Disable difficulty image
            beatmapButtonList[_beatmapButtonIndex].advancedDifficultyImage.gameObject.SetActive(false);
            // Enable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].advancedDifficultyLevelText.gameObject.SetActive(false);
        }

        // If the extra file exists
        if (File.Exists(extraFileCheckPath) == true)
        {
            // Enable difficulty image
            beatmapButtonList[_beatmapButtonIndex].extraDifficultyImage.gameObject.SetActive(true);
            // Enable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].extraDifficultyLevelText.gameObject.SetActive(true);

            // Load the database and beatmap information for the beatmap directory selected
            Database.database.Load(songSelectManager.beatmapDirectories[_beatmapButtonIndex], "extra");
            // Load difficulty level text
            beatmapButtonList[_beatmapButtonIndex].extraDifficultyLevelText.text = Database.database.LoadedBeatmapExtraDifficultyLevel;
            // Load beatmap button text information
            beatmapButtonList[_beatmapButtonIndex].songNameText.text = Database.database.LoadedSongName;
            beatmapButtonList[_beatmapButtonIndex].artistText.text = Database.database.LoadedSongArtist;
            beatmapButtonList[_beatmapButtonIndex].beatmapCreatorText.text = Database.database.LoadedBeatmapCreator;
            // Clear the database
            Database.database.Clear();

            // Has difficulty
            beatmapButtonList[_beatmapButtonIndex].HasExtraDifficulty = true;
        }
        else
        {
            // Disable difficulty image
            beatmapButtonList[_beatmapButtonIndex].extraDifficultyImage.gameObject.SetActive(false);
            // Enable difficulty level text
            beatmapButtonList[_beatmapButtonIndex].extraDifficultyLevelText.gameObject.SetActive(false);
        }
    }

    // Sort beatmap buttons by advanced difficulty
    public void SortBeatmapButtonsAdvancedDifficulty()
    {
        // If the beatmaps are currently sorted by "searched beatmaps"
        if (currentDifficultySorting == "searched")
        {
            // Only sort the searched beatmaps
            // Loop through all button scripts
            for (int i = 0; i < searchedBeatmapsList.Count; i++)
            {
                // Check if beatmap button has easy difficulty
                if (searchedBeatmapsList[i].HasAdvancedDifficulty == false)
                {
                    // Disable all buttons that do not have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(false);
                }
                else if (beatmapButtonList[i].HasAdvancedDifficulty == true)
                {
                    // Enable all buttons that have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(true);

                    // Enable color
                    searchedBeatmapsList[i].overlayColorImage.color = advancedDifficultyColor;
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("searched");

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (searchedBeatmapsList.Count != 0)
            {
                searchedBeatmapsList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + searchedBeatmapsList.Count.ToString();
        }
        else
        {
            // Clear the list
            advancedDifficultySortedBeatmapButtonList.Clear();

            // Loop through all button scripts
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                // Check if beatmap button has advanced difficulty
                if (beatmapButtonList[i].HasAdvancedDifficulty == false)
                {
                    // Disable all buttons that do not have advanced difficulty
                    beatmapButtonList[i].gameObject.SetActive(false);
                }
                else if (beatmapButtonList[i].HasAdvancedDifficulty == true)
                {
                    // Enable all buttons that have advanced difficulty
                    beatmapButtonList[i].gameObject.SetActive(true);

                    // Enable color
                    beatmapButtonList[i].overlayColorImage.color = advancedDifficultyColor;

                    // Add to the sorted list
                    advancedDifficultySortedBeatmapButtonList.Add(beatmapButtonList[i]);
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("advancedDifficulty");

            // Set current sorting difficulty
            currentDifficultySorting = advancedDifficultySortingValue;

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (advancedDifficultySortedBeatmapButtonList.Count != 0)
            {
                advancedDifficultySortedBeatmapButtonList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + advancedDifficultySortedBeatmapButtonList.Count.ToString();
        }
    }

    // Sort beatmap buttons by extra difficulty
    public void SortBeatmapButtonsExtraDifficulty()
    {
        // If the beatmaps are currently sorted by "searched beatmaps"
        if (currentDifficultySorting == "searched")
        {
            // Only sort the searched beatmaps
            // Loop through all button scripts
            for (int i = 0; i < searchedBeatmapsList.Count; i++)
            {
                // Check if beatmap button has extra difficulty
                if (searchedBeatmapsList[i].HasExtraDifficulty == false)
                {
                    // Disable all buttons that do not have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(false);
                }
                else
                {
                    // Enable all buttons that have extra difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(true);

                    // Enable color
                    searchedBeatmapsList[i].overlayColorImage.color = extraDifficultyColor;
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("searched");

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (searchedBeatmapsList.Count != 0)
            {
                searchedBeatmapsList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + searchedBeatmapsList.Count.ToString();
        }
        else
        {
            // Clear the list
            extraDifficultySortedBeatmapButtonList.Clear();

            // Loop through all button scripts
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                // Check if beatmap button has extra difficulty
                if (beatmapButtonList[i].HasExtraDifficulty == false)
                {
                    // Disable all buttons that do not have extra difficulty
                    beatmapButtonList[i].gameObject.SetActive(false);
                }
                else if (beatmapButtonList[i].HasExtraDifficulty == true)
                {
                    // Enable all buttons that have extra difficulty
                    beatmapButtonList[i].gameObject.SetActive(true);

                    // Enable color
                    beatmapButtonList[i].overlayColorImage.color = extraDifficultyColor;

                    // Add to the sorted list
                    extraDifficultySortedBeatmapButtonList.Add(beatmapButtonList[i]);
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("extraDifficulty");

            // Set current sorting difficulty
            currentDifficultySorting = extraDifficultySortingValue;

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (extraDifficultySortedBeatmapButtonList.Count != 0)
            {
                extraDifficultySortedBeatmapButtonList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + extraDifficultySortedBeatmapButtonList.Count.ToString();
        }
    }

    // Sort beatmap buttons by easy difficulty
    public void SortBeatmapButtonsEasyDifficulty()
    {
        // If the beatmaps are currently sorted by "searched beatmaps"
        if (currentDifficultySorting == "searched")
        {
            // Only sort the searched beatmaps
            // Loop through all button scripts
            for (int i = 0; i < searchedBeatmapsList.Count; i++)
            {
                // Check if beatmap button has easy difficulty
                if (searchedBeatmapsList[i].HasEasyDifficulty == false)
                {
                    // Disable all buttons that do not have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(false);
                }
                else
                {
                    // Enable all buttons that have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(true);

                    // Enable color
                    searchedBeatmapsList[i].overlayColorImage.color = easyDifficultyColor;
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("searched");

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (searchedBeatmapsList.Count != 0)
            {
                searchedBeatmapsList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + searchedBeatmapsList.Count.ToString();
        }
        else
        {
            // Clear the list
            easyDifficultySortedBeatmapButtonList.Clear();

            // Loop through all button scripts
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                // Check if beatmap button has easy difficulty
                if (beatmapButtonList[i].HasEasyDifficulty == false)
                {
                    // Disable all buttons that do not have easy difficulty
                    beatmapButtonList[i].gameObject.SetActive(false);
                }
                else
                {
                    // Enable all buttons that have easy difficulty
                    beatmapButtonList[i].gameObject.SetActive(true);

                    // Enable color
                    beatmapButtonList[i].overlayColorImage.color = easyDifficultyColor;

                    // Add to the sorted list
                    easyDifficultySortedBeatmapButtonList.Add(beatmapButtonList[i]);
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("easyDifficulty");

            // Set current sorting difficulty
            currentDifficultySorting = easyDifficultySortingValue;

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (easyDifficultySortedBeatmapButtonList.Count != 0)
            {
                easyDifficultySortedBeatmapButtonList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + easyDifficultySortedBeatmapButtonList.Count.ToString();
        }
    }

    // Sort beatmap buttons by all difficulties
    public void SortBeatmapButtonsAllDifficulties()
    {
        // If the beatmaps are currently sorted by "searched beatmaps"
        if (currentDifficultySorting == "searched")
        {
            // Only sort the searched beatmaps
            // Loop through all button scripts
            for (int i = 0; i < searchedBeatmapsList.Count; i++)
            {
                // Check if the beatmap button contains all 3 difficulties
                if (beatmapButtonList[i].HasEasyDifficulty == true && beatmapButtonList[i].HasAdvancedDifficulty == true &&
                    beatmapButtonList[i].HasExtraDifficulty == true)
                {
                    // Disable all buttons that do not have easy difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(false);
                }
                else
                {
                    // Enable all buttons that have extra difficulty
                    searchedBeatmapsList[i].gameObject.SetActive(true);

                    // Enable color
                    searchedBeatmapsList[i].overlayColorImage.color = allDifficultyColor;
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("searched");

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (searchedBeatmapsList.Count != 0)
            {
                searchedBeatmapsList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + searchedBeatmapsList.Count.ToString();
        }
        else
        {
            // Clear the list
            allDifficultySortedBeatmapButtonList.Clear();

            // Loop through all button scripts
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                // Check if the beatmap button contains all 3 difficulties
                if (beatmapButtonList[i].HasEasyDifficulty == true && beatmapButtonList[i].HasAdvancedDifficulty == true &&
                    beatmapButtonList[i].HasExtraDifficulty == true)
                {
                    // Enable all buttons that have them all
                    beatmapButtonList[i].gameObject.SetActive(true);

                    // Enable color
                    beatmapButtonList[i].overlayColorImage.color = allDifficultyColor;

                    // Add to the sorted list
                    allDifficultySortedBeatmapButtonList.Add(beatmapButtonList[i]);
                }
                else
                {
                    // Disable button
                    beatmapButtonList[i].gameObject.SetActive(false);
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("allDifficulty");

            // Set current sorting difficulty
            currentDifficultySorting = allDifficultySortingValue;

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Select first button in the list
            if (allDifficultySortedBeatmapButtonList.Count != 0)
            {
                allDifficultySortedBeatmapButtonList[0].GetComponent<Button>().Select();
            }

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + allDifficultySortedBeatmapButtonList.Count.ToString();
        }
    }

    // Update the sorted button index
    private void UpdateSortedButtonIndex()
    {
        // Create new list
        List<BeatmapButton> listToSort = new List<BeatmapButton>();

        // Get the list to sort the index by based on the current sorting
        switch (currentDifficultySorting)
        {
            case defaultDifficultySortingValue:
                listToSort = beatmapButtonList;
                break;
            case easyDifficultySortingValue:
                listToSort = easyDifficultySortedBeatmapButtonList;
                break;
            case advancedDifficultySortingValue:
                listToSort = advancedDifficultySortedBeatmapButtonList;
                break;
            case extraDifficultySortingValue:
                listToSort = extraDifficultySortedBeatmapButtonList;
                break;
            case allDifficultySortingValue:
                listToSort = allDifficultySortedBeatmapButtonList;
                break;
            case "searched":
                listToSort = searchedBeatmapsList;
                break;
        }

        // Assign the new index based on current sorting
        for (int i = 0; i < listToSort.Count; i++)
        {
            switch (currentDifficultySorting)
            {
                case defaultDifficultySortingValue:
                    listToSort[i].DefaultButtonIndex = i;
                    break;
                case easyDifficultySortingValue:
                    listToSort[i].EasyDifficultyButtonIndex = i;
                    break;
                case advancedDifficultySortingValue:
                    listToSort[i].AdvancedDifficultyButtonIndex = i;
                    break;
                case extraDifficultySortingValue:
                    listToSort[i].ExtraDifficultyButtonIndex = i;
                    break;
                case allDifficultySortingValue:
                    listToSort[i].AllDifficultyButtonIndex = i;
                    break;
            }
        }
    }

    // Sort buttons by default - everything enabled
    public void SortBeatmapButtonsDefault()
    {
        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Enable all buttons that have them all
            beatmapButtonList[i].gameObject.SetActive(true);

            // Enable color
            beatmapButtonList[i].overlayColorImage.color = defaultDifficultyColor;
        }

        // Set current sorting difficulty to default
        currentDifficultySorting = defaultDifficultySortingValue;

        // Set beatmaps to alphabetical 
        SortBeatmapButtonsSongNameAlphabetical();

        // Update beatmap button navigation
        UpdateBeatmapButtonNavigation("default");

        // Set the scroll view back to the top of the screen
        SetButtonScrollListToTop();

        // Select first button in the list
        if (beatmapButtonList.Count != 0)
        {
            beatmapButtonList[0].GetComponent<Button>().Select();
        }

        // Update the beatmap button index based on the current sorting
        UpdateSortedButtonIndex();

        // Update the total beatmap count based on the current sorting
        totalBeatmapCountText.text = "/ " + beatmapButtonList.Count.ToString();
    }

    // Toggle easy difficulty levels
    public void ToggleEasyDifficultyLevels()
    {
        // Change bools when button is clicked
        if (easyDifficultyTickBoxSelected == true)
        {
            // Set to false
            easyDifficultyTickBoxSelected = false;

            // Display message panel
            messagePanel.DisplayEasyDifficultyToggleOffMessageValue();
        }
        else if (easyDifficultyTickBoxSelected == false)
        {
            // Set to true
            easyDifficultyTickBoxSelected = true;

            // Display message panel
            messagePanel.DisplayEasyDifficultyToggleOnMessageValue();
        }

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Check if the easy difficulty tick box is selected
            if (easyDifficultyTickBoxSelected == true)
            {
                // Check if the beatmap button has easy difficulty
                if (beatmapButtonList[i].HasEasyDifficulty == true)
                {
                    // Enable difficulty level UI
                    beatmapButtonList[i].easyDifficultyImage.gameObject.SetActive(true);
                    beatmapButtonList[i].easyDifficultyLevelText.gameObject.SetActive(true);
                }

                // Enable tick image
                easyDifficultyTickBoxSelectedImage.gameObject.SetActive(true);
            }
            else if (easyDifficultyTickBoxSelected == false)
            {
                // Disable difficulty level UI
                beatmapButtonList[i].easyDifficultyImage.gameObject.SetActive(false);
                beatmapButtonList[i].easyDifficultyLevelText.gameObject.SetActive(false);

                // Disable tick image
                easyDifficultyTickBoxSelectedImage.gameObject.SetActive(false);
            }
        }
    }

    // Toggle advanced difficulty levels
    public void ToggleAdvancedDifficultyLevels()
    {
        // Change bools when button is clicked
        if (advancedDifficultyTickBoxSelected == true)
        {
            // Set to false
            advancedDifficultyTickBoxSelected = false;

            // Display message panel
            messagePanel.DisplayAdvancedDifficultyToggleOffMessageValue();
        }
        else if (advancedDifficultyTickBoxSelected == false)
        {
            // Set to true
            advancedDifficultyTickBoxSelected = true;

            // Display message panel
            messagePanel.DisplayAdvancedDifficultyToggleOnMessageValue();
        }

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Check if the advanced difficulty tick box is selected
            if (advancedDifficultyTickBoxSelected == true)
            {
                // Check if the beatmap button has advanced difficulty
                if (beatmapButtonList[i].HasAdvancedDifficulty == true)
                {
                    // Enable difficulty level UI
                    beatmapButtonList[i].advancedDifficultyImage.gameObject.SetActive(true);
                    beatmapButtonList[i].advancedDifficultyLevelText.gameObject.SetActive(true);
                }

                // Enable tick image
                advancedDifficultyTickBoxSelectedImage.gameObject.SetActive(true);
            }
            else if (advancedDifficultyTickBoxSelected == false)
            {
                // Disable difficulty level UI
                beatmapButtonList[i].advancedDifficultyImage.gameObject.SetActive(false);
                beatmapButtonList[i].advancedDifficultyLevelText.gameObject.SetActive(false);

                // Disable tick image
                advancedDifficultyTickBoxSelectedImage.gameObject.SetActive(false);
            }
        }
    }

    // Toggle extra difficulty levels
    public void ToggleExtraDifficultyLevels()
    {
        // Change bools when button is clicked
        if (extraDifficultyTickBoxSelected == true)
        {
            // Set to false
            extraDifficultyTickBoxSelected = false;

            // Display message panel
            messagePanel.DisplayExtraDifficultyToggleOffMessageValue();
        }
        else if (extraDifficultyTickBoxSelected == false)
        {
            // Set to true
            extraDifficultyTickBoxSelected = true;

            // Display message panel
            messagePanel.DisplayExtraDifficultyToggleOnMessageValue();
        }

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Check if the extra difficulty tick box is selected
            if (extraDifficultyTickBoxSelected == true)
            {
                // Check if the beatmap button has extra difficulty
                if (beatmapButtonList[i].HasExtraDifficulty == true)
                {
                    // Enable difficulty level UI
                    beatmapButtonList[i].extraDifficultyImage.gameObject.SetActive(true);
                    beatmapButtonList[i].extraDifficultyLevelText.gameObject.SetActive(true);
                }

                // Enable tick image
                extraDifficultyTickBoxSelectedImage.gameObject.SetActive(true);
            }
            else if (extraDifficultyTickBoxSelected == false)
            {
                // Disable difficulty level UI
                beatmapButtonList[i].extraDifficultyImage.gameObject.SetActive(false);
                beatmapButtonList[i].extraDifficultyLevelText.gameObject.SetActive(false);

                // Disable tick image
                extraDifficultyTickBoxSelectedImage.gameObject.SetActive(false);
            }
        }
    }

    // Sort beatmap buttons by song name alphabetical order 
    public void SortBeatmapButtonsSongNameAlphabetical()
    {
        // Sort the difficulty buttons based on the current difficulty sorting in place
        switch (currentDifficultySorting)
        {
            case "default":
                // Sort the buttons based on the song name
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("default");
                break;
            case "easy":
                easyDifficultySortedBeatmapButtonList = easyDifficultySortedBeatmapButtonList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("easyDifficulty");
                break;
            case "advanced":
                advancedDifficultySortedBeatmapButtonList = advancedDifficultySortedBeatmapButtonList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("advancedDifficulty");
                break;
            case "extra":
                extraDifficultySortedBeatmapButtonList = extraDifficultySortedBeatmapButtonList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("extraDifficulty");
                break;
            case "all":
                allDifficultySortedBeatmapButtonList = allDifficultySortedBeatmapButtonList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("allDifficulty");
                break;
            case "searched":
                searchedBeatmapsList = searchedBeatmapsList.OrderBy(x => x.songNameText.text).ToList();
                SetListButtonsAsLastSibling("searched");
                break;
        }

        // Update the sorted button index
        UpdateSortedButtonIndex();
    }

    // Sort beatmap buttons by artist alphabetical order 
    public void SortBeatmapButtonsArtistAlphabetical()
    {
        // Sort the difficulty buttons based on the current difficulty sorting in place
        switch (currentDifficultySorting)
        {
            case "default":
                // Sort the buttons based on the artist
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("default");
                break;
            case "easy":
                easyDifficultySortedBeatmapButtonList = easyDifficultySortedBeatmapButtonList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("easyDifficulty");
                break;
            case "advanced":
                advancedDifficultySortedBeatmapButtonList = advancedDifficultySortedBeatmapButtonList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("advancedDifficulty");
                break;
            case "extra":
                extraDifficultySortedBeatmapButtonList = extraDifficultySortedBeatmapButtonList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("extraDifficulty");
                break;
            case "all":
                allDifficultySortedBeatmapButtonList = allDifficultySortedBeatmapButtonList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("allDifficulty");
                break;
            case "searched":
                searchedBeatmapsList = searchedBeatmapsList.OrderBy(x => x.artistText.text).ToList();
                SetListButtonsAsLastSibling("searched");
                break;
        }

        // Update the sorted button index
        UpdateSortedButtonIndex();
    }

    // Search beatmaps with the search bar
    public void SearchBeatmaps()
    {
        // Clear the list
        searchedBeatmapsList.Clear();

        // Turn input to upper case
        string beatmapSearchValue = beatmapSearchInputField.text.ToUpper();

        // Button search variables
        string songNameText;
        string artistText;
        string beatmapCreatorText;

        // If all characters have been erased
        if (beatmapSearchValue.Length == 0)
        {
            // Sort beatmap buttons to default
            SortBeatmapButtonsDefault();
        }
        else
        {
            // Loop through all button scripts
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                songNameText = beatmapButtonList[i].songNameText.text.ToUpper();
                artistText = beatmapButtonList[i].artistText.text.ToUpper();
                beatmapCreatorText = beatmapButtonList[i].beatmapCreatorText.text.ToUpper();

                // Search all buttons to see if the current input field text is the same as the song name, artist, beatmap creator or difficulty levels
                if (songNameText.Contains(beatmapSearchValue) || artistText.Contains(beatmapSearchValue) || beatmapCreatorText.Contains(beatmapSearchValue))
                {
                    // Activate button
                    beatmapButtonList[i].gameObject.SetActive(true);
                    // Add to the current searched list
                    searchedBeatmapsList.Add(beatmapButtonList[i]);

                    // Set color to default purple
                    beatmapButtonList[i].overlayColorImage.color = defaultDifficultyColor;
                }
                else
                {
                    // Disable button
                    beatmapButtonList[i].gameObject.SetActive(false);
                }
            }

            // Update beatmap button navigation
            UpdateBeatmapButtonNavigation("searched");

            // Set current sorting difficulty
            currentDifficultySorting = "searched";

            // Set the scroll view back to the top of the screen
            SetButtonScrollListToTop();

            // Update the beatmap button index based on the current sorting
            UpdateSortedButtonIndex();

            // Update the total beatmap count based on the current sorting
            totalBeatmapCountText.text = "/ " + searchedBeatmapsList.Count.ToString();
        }
    }

    // Deselect beatmap search bar functions
    public void DeselectBeatmapSearchbar()
    {
        if (beatmapSearchInputField.text.Length == 0 || searchedBeatmapsList.Count == 0)
        {
            // Sort buttons back to default
            SortBeatmapButtonsDefault();
        }
    }

    // After typing in the search beatmap input field select the first button
    public void SelectSearchedBeatmapFirstButton()
    {
        // Select first button in the list
        if (searchedBeatmapsList.Count != 0)
        {
            searchedBeatmapsList[0].GetComponent<Button>().Select();
        }
    }

    // Sort beatmap buttons by creator alphabetical order 
    public void SortBeatmapButtonsCreatorAlphabetical()
    {
        // Sort the difficulty buttons based on the current difficulty sorting in place
        switch (currentDifficultySorting)
        {
            case "default":
                // Sort the buttons based on the beatmap creator
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("default");
                break;
            case "easy":
                easyDifficultySortedBeatmapButtonList = easyDifficultySortedBeatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("easyDifficulty");
                break;
            case "advanced":
                advancedDifficultySortedBeatmapButtonList = advancedDifficultySortedBeatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("advancedDifficulty");
                break;
            case "extra":
                extraDifficultySortedBeatmapButtonList = extraDifficultySortedBeatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("extraDifficulty");
                break;
            case "all":
                allDifficultySortedBeatmapButtonList = allDifficultySortedBeatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("allDifficulty");
                break;
            case "searched":
                searchedBeatmapsList = searchedBeatmapsList.OrderBy(x => x.beatmapCreatorText.text).ToList();
                SetListButtonsAsLastSibling("searched");
                break;
        }

        // Update the sorted button index
        UpdateSortedButtonIndex();
    }

    // Sort beatmap buttons by difficulty level 
    public void SortBeatmapButtonsDifficultyLevel()
    {
        // Sort beatmap levels based on the current difficulty sorting enabled
        switch (currentDifficultySorting)
        {
            case easyDifficultySortingValue:
                // Sort the list
                easyDifficultySortedBeatmapButtonList = easyDifficultySortedBeatmapButtonList.OrderBy(x => x.easyDifficultyLevelText.text).ToList();
                // Order the buttons in the list
                SetListButtonsAsLastSibling("easyDifficulty");
                // Update navigation for the buttons
                UpdateBeatmapButtonNavigation("easyDifficulty");
                break;
            case advancedDifficultySortingValue:
                advancedDifficultySortedBeatmapButtonList = advancedDifficultySortedBeatmapButtonList.OrderBy(x => x.advancedDifficultyLevelText.text).ToList();
                SetListButtonsAsLastSibling("advancedDifficulty");
                UpdateBeatmapButtonNavigation("advancedDifficulty");
                break;
            case extraDifficultySortingValue:
                extraDifficultySortedBeatmapButtonList = extraDifficultySortedBeatmapButtonList.OrderBy(x => x.extraDifficultyLevelText.text).ToList();
                SetListButtonsAsLastSibling("extraDifficulty");
                UpdateBeatmapButtonNavigation("extraDifficulty");
                break;
            case allDifficultySortingValue:
                allDifficultySortedBeatmapButtonList = allDifficultySortedBeatmapButtonList.OrderBy(x => x.extraDifficultyLevelText.text).ToList();
                SetListButtonsAsLastSibling("allDifficulty");
                UpdateBeatmapButtonNavigation("allDifficulty");
                break;
            case "searched":
                //searchedBeatmapsList = searchedBeatmapsList.OrderBy(x => x.extraDifficultyLevelText.text).ToList();
                //SetListButtonsAsLastSibling("searched");
                //UpdateBeatmapButtonNavigation("searched");
                break;
        }

        // Update beatmap button sorting
        UpdateSortedButtonIndex();
    }

    // Set list buttons as last siblings in the UI canvas
    private void SetListButtonsAsLastSibling(string _sorting)
    {
        List<BeatmapButton> listToSort = new List<BeatmapButton>();

        switch (_sorting)
        {
            case "default":
                listToSort = beatmapButtonList;
                break;
            case "easyDifficulty":
                listToSort = easyDifficultySortedBeatmapButtonList;
                break;
            case "advancedDifficulty":
                listToSort = advancedDifficultySortedBeatmapButtonList;
                break;
            case "extraDifficulty":
                listToSort = extraDifficultySortedBeatmapButtonList;
                break;
            case "allDifficulty":
                listToSort = allDifficultySortedBeatmapButtonList;
                break;
            case "searched":
                listToSort = searchedBeatmapsList;
                break;
        }

        // Loop through all button scripts
        for (int i = 0; i < listToSort.Count; i++)
        {
            // Update order of UI buttons
            listToSort[i].transform.SetAsLastSibling();
        }

        // Select first button on list
        listToSort[0].GetComponent<Button>().Select();

        // Set the scroll view back to the top of the screen
        SetButtonScrollListToTop();
    }
}