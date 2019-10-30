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
        allDifficultySortingValue = "all";

    // Integers
    private int beatmapButtonIndexToGet;
    private int activeButtonIndex;

    // Bools
    private bool hasLoadedAllBeatmapDirectories;
    private bool hasResetSliderBarValue;
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

    // Properties
    public bool HasLoadedAllBeatmapButtons
    {
        get { return hasLoadedAllBeatmapButtons; }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        beatmapButtonIndexToGet = 0;
        easyDifficultyTickBoxSelected = true;
        advancedDifficultyTickBoxSelected = true;
        extraDifficultyTickBoxSelected = true;
        hasResetSliderBarValue = false;
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

        if (hasResetSliderBarValue == false)
        {
            StartCoroutine(ResetScrollValue());
        }
    }

    private IEnumerator ResetScrollValue()
    {
        yield return new WaitForSeconds(0.2f);

        beatmapButtonListScrollbar.value = 1f;

        hasResetSliderBarValue = true;
    }

    // Check input for song select panel features
    private void CheckSongSelectPanelInput()
    {
        // Check for any input
        if (Input.anyKeyDown)
        {
            // Check for mouse or navigation input
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Get the selected 
                currentSelectedBeatmapButton = EventSystem.current.currentSelectedGameObject;
                selectedBeatmapButtonScript = currentSelectedBeatmapButton.GetComponent<BeatmapButton>();
                currentSelectedBeatmapButtonIndex = beatmapButtonList.IndexOf(selectedBeatmapButtonScript);

                // Get the current position of the button list content panel
                buttonListContentPositionX = buttonListContent.transform.localPosition.x;
                buttonListContentPositionY = buttonListContent.transform.localPosition.y;
                buttonListContentPositionZ = buttonListContent.transform.localPosition.z;

                // Scroll the beatmap button content panel up
                ScrollListUp();

                // Scroll the beatmap button content panel up
                ScrollListDown();
            }
            else
            {
                // Select search bar if any keyboard key has been pressed
                beatmapSearchInputField.ActivateInputField();
            }
        }
    }

    // Scroll the beatmap button content panel up
    private void ScrollListUp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSelectedBeatmapButtonIndex < beatmapButtonList.Count - 4)
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
            if (currentSelectedBeatmapButtonIndex > 3)
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
            SetDefaultBeatmapButtonNavigation();

            // Set to true
            hasLoadedAllBeatmapButtons = true;

            // Sort by default
            SortBeatmapButtonsDefault();

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

    // Set default navigation for all instantiated buttons
    private void SetDefaultBeatmapButtonNavigation()
    {
        for (int i = 0; i < instantiatedBeatmapButtonList.Count; i++)
        {
            // Get current button setting navigation for
            Button button = instantiatedBeatmapButtonList[i].GetComponent<Button>();
            Button buttonDown;
            Button buttonUp;

            // Get the Navigation data
            Navigation navigation = button.navigation;

            // Switch mode to Explicit to allow for custom assigned behavior
            navigation.mode = Navigation.Mode.Explicit;

            // Get button down
            if (i == instantiatedBeatmapButtonList.Count - 1)
            {
                // If at end of list set down navigation to own button
                buttonDown = button;
            }
            else
            {
                // Set button down to the next button in the list
                buttonDown = instantiatedBeatmapButtonList[i + 1].GetComponent<Button>();
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
                buttonUp = instantiatedBeatmapButtonList[i - 1].GetComponent<Button>();
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
        /*
        else if (levelChanger.CurrentLevelIndex == levelChanger.EditSelectSceneIndex)
        {
            // Assign the index and image to this button
            beatmapButtonInstantiate = Instantiate(editSelectSceneBeatmapButton, beatmapButtonPosition, Quaternion.Euler(0, 0, 0),
            buttonListContent) as GameObject;
        }
        */

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
        /*
        else if (levelChanger.CurrentLevelIndex == levelChanger.EditSelectSceneIndex)
        {
            completePath = "file://" + beatmapDirectoryPaths[_beatmapButtonIndex] +
                @"\" + imageName + imageType;

            fileCheckPath = editSelectSceneSongSelectManager.beatmapDirectories[_beatmapButtonIndex] +
                @"\" + imageName + imageType;
        }
        */

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
            }
        }

        // Set current sorting difficulty
        currentDifficultySorting = advancedDifficultySortingValue;
    }

    // Sort beatmap buttons by extra difficulty
    public void SortBeatmapButtonsExtraDifficulty()
    {
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
            }
        }

        // Set current sorting difficulty
        currentDifficultySorting = extraDifficultySortingValue;
    }

    // Sort beatmap buttons by easy difficulty
    public void SortBeatmapButtonsEasyDifficulty()
    {
        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Check if beatmap button has easy difficulty
            if (beatmapButtonList[i].HasEasyDifficulty == false)
            {
                // Disable all buttons that do not have easy difficulty
                beatmapButtonList[i].gameObject.SetActive(false);
            }
            else if (beatmapButtonList[i].HasEasyDifficulty == true)
            {
                // Enable all buttons that have easy difficulty
                beatmapButtonList[i].gameObject.SetActive(true);

                // Enable color
                beatmapButtonList[i].overlayColorImage.color = easyDifficultyColor;
            }
        }

        // Set current sorting difficulty
        currentDifficultySorting = easyDifficultySortingValue;
    }

    // Sort beatmap buttons by all difficulties
    public void SortBeatmapButtonsAllDifficulties()
    {
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
            }
            else
            {
                // Disable button
                beatmapButtonList[i].gameObject.SetActive(false);
            }
        }

        // Set current sorting difficulty
        currentDifficultySorting = allDifficultySortingValue;
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

        // Set current sorting difficulty
        currentDifficultySorting = extraDifficultySortingValue;
    }

    // Toggle easy difficulty levels
    public void ToggleEasyDifficultyLevels()
    {
        // Change bools when button is clicked
        if (easyDifficultyTickBoxSelected == true)
        {
            // Set to false
            easyDifficultyTickBoxSelected = false;
        }
        else if (easyDifficultyTickBoxSelected == false)
        {
            // Set to true
            easyDifficultyTickBoxSelected = true;
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
        }
        else if (advancedDifficultyTickBoxSelected == false)
        {
            // Set to true
            advancedDifficultyTickBoxSelected = true;
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
        }
        else if (extraDifficultyTickBoxSelected == false)
        {
            // Set to true
            extraDifficultyTickBoxSelected = true;
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
        // Sort the buttons based on the song name
        beatmapButtonList = beatmapButtonList.OrderBy(x => x.songNameText.text).ToList();

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Update order of UI buttons
            beatmapButtonList[i].transform.SetAsLastSibling();
        }
    }

    // Sort beatmap buttons by artist alphabetical order 
    public void SortBeatmapButtonsArtistAlphabetical()
    {
        // Sort the buttons based on the artist 
        beatmapButtonList = beatmapButtonList.OrderBy(x => x.artistText.text).ToList();

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Update order of UI buttons
            beatmapButtonList[i].transform.SetAsLastSibling();
        }
    }

    // Search beatmaps with the search bar
    public void SearchBeatmaps()
    {
        // Sort beatmaps
        SortBeatmapButtonsDefault();

        // Turn input to upper case
        string beatmapSearchValue = beatmapSearchInputField.text.ToUpper();

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Search all buttons to see if the current input field text is the same as the song name, artist, beatmap creator or difficulty levels
            if (beatmapButtonList[i].songNameText.text.Contains(beatmapSearchValue) ||
                beatmapButtonList[i].artistText.text.Contains(beatmapSearchValue) ||
                beatmapButtonList[i].beatmapCreatorText.text.Contains(beatmapSearchValue) ||
                beatmapButtonList[i].easyDifficultyLevelText.text.Contains(beatmapSearchValue) ||
                beatmapButtonList[i].advancedDifficultyLevelText.text.Contains(beatmapSearchValue) ||
                beatmapButtonList[i].extraDifficultyLevelText.text.Contains(beatmapSearchValue))
            {
                // Activate button
                beatmapButtonList[i].gameObject.SetActive(true);
            }
            else
            {
                // Disable button
                beatmapButtonList[i].gameObject.SetActive(false);
            }
        }
    }

    // Sort beatmap buttons by creator alphabetical order 
    public void SortBeatmapButtonsCreatorAlphabetical()
    {
        // Sort the buttons based on the beatmap creator
        beatmapButtonList = beatmapButtonList.OrderBy(x => x.beatmapCreatorText.text).ToList();

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Update order of UI buttons
            beatmapButtonList[i].transform.SetAsLastSibling();
        }
    }

    // Sort beatmap buttons by difficulty level 
    public void SortBeatmapButtonsDifficultyLevel()
    {
        // Sort beatmap levels based on the current difficulty sorting enabled
        switch (currentDifficultySorting)
        {
            case easyDifficultySortingValue:
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.easyDifficultyLevelText.text).ToList();
                break;
            case advancedDifficultySortingValue:
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.advancedDifficultyLevelText.text).ToList();
                break;
            case extraDifficultySortingValue:
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.extraDifficultyLevelText.text).ToList();
                break;
            case allDifficultySortingValue:
                beatmapButtonList = beatmapButtonList.OrderBy(x => x.extraDifficultyLevelText.text).ToList();
                break;
        }

        // Loop through all button scripts
        for (int i = 0; i < beatmapButtonList.Count; i++)
        {
            // Update order of UI buttons
            beatmapButtonList[i].transform.SetAsLastSibling();
        }
    }
}