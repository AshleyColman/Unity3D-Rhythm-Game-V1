using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class SongSelectPanel : MonoBehaviour
{
    // UI
    private List<Image> instantiatedBeatmapButtonImageList = new List<Image>();
    private Image instantiatedBeatmapButtonImage;
    public Scrollbar beatmapButtonListScrollbar;


    // Gameobjects
    public GameObject beatmapButton; // The button to instantiate
    private List<GameObject> instantiatedBeatmapButtonList = new List<GameObject>();
    private GameObject beatmapButtonInstantiate; // The instantiated beatmap button
    public Transform buttonListContent; // Where the beatmap buttons instantiate to
    private Transform beatmapButtonInstantiateChildImage; // Child image for the beatmap button

    // Strings
    private string imageName;
    private string imageType;
    private string[] beatmapDirectoryPaths; // The beatmap directory paths
    private string shaderLocation;
    string completePath;
    string fileCheckPath;

    // Integers
    private int beatmapButtonIndexToGet;
    private int activeButtonIndex;

    // Bools
    private bool hasLoadedAllBeatmapDirectories;
    private bool hasResetSliderBarValue;

    // Material
    private Material childImageMaterial; // Child image for beatmap buttons
    public Material defautChildImageMaterial; // Default image that is displayed when no file can be found 

    // Vectors
    private Vector3 beatmapButtonPosition;

    // Scripts
    private SongSelectManager songSelectManager; // Reference to the song select manager which manages loading songs, used to get the beatmap img addresses for loading images



    // Use this for initialization
    void Start()
    {
        // Initialize
        beatmapButtonIndexToGet = 0;
        hasResetSliderBarValue = false;
        hasLoadedAllBeatmapDirectories = false;  // Set to false at the start, set to true when all have loaded
        beatmapButtonPosition = new Vector3(0, 0, 500); // Set to 500 on z to fix the "moving image" problem, instantiates the images to z of 0 so the images don't move when the mouse cursor has moved
        shaderLocation = "UI/Unlit/Transparent";
        imageType = ".png";
        imageName = "img";
        completePath = "";
        fileCheckPath = "";
        beatmapButtonListScrollbar.value = 0;


        // Reference
        songSelectManager = FindObjectOfType<SongSelectManager>();
    }

    private void Update()
    {
        if (hasLoadedAllBeatmapDirectories == false)
        {
            // Create all beatmap buttons to go in the song select panel
            CreateSongSelectPanel();

            hasLoadedAllBeatmapDirectories = true;
        }

        if (beatmapButtonListScrollbar.value != 0f && hasResetSliderBarValue == false)
        {
            beatmapButtonListScrollbar.value = 0f;
            hasResetSliderBarValue = true;
        }
    }

    // Create all beatmap buttons to go in the song select panel
    private void CreateSongSelectPanel()
    {
        // Load the beatmap buttons in the scroll list
        for (int beatmapButtonIndex = 0; beatmapButtonIndex < songSelectManager.beatmapDirectories.Length; beatmapButtonIndex++)
        {
            // Instantiate a new beatmap button to go in the song select panel
            InstantiateBeatmapButton(beatmapButtonIndexToGet);

            // Increment the beatmapButtonIndexToGet (Increments after each coroutine only so it loads the image 1 after another)
            beatmapButtonIndexToGet++;
        }

        /// Reset
        beatmapButtonIndexToGet = 0;

        for (int i = 0; i < songSelectManager.beatmapDirectories.Length; i++)
        {
            // Change the beatmap image
            StartCoroutine(LoadNewBeatmapButtonImage(beatmapButtonIndexToGet));

            beatmapButtonIndexToGet++;
        }

        // Reset the scroll bar value
        beatmapButtonListScrollbar.value = 0f;
    }


    // Get the beatmap directory paths
    public void GetBeatmapDirectoryPaths()
    {
        // Initialise the array with the amount of beatmap directories found
        // Get the beatmap directoriess
        beatmapDirectoryPaths = Directory.GetDirectories(@"c:\Beatmaps");
    }

    // Instantiate a new beatmap button in to the song select panel
    private void InstantiateBeatmapButton(int _beatmapButtonIndex)
    {
        // Assign the index and image to this button
        beatmapButtonInstantiate = Instantiate(beatmapButton, beatmapButtonPosition, Quaternion.Euler(0, 0, -45),
        buttonListContent) as GameObject;

        // Add the instantiated button to the list
        instantiatedBeatmapButtonList.Add(beatmapButtonInstantiate);

        // Get the child image transform from the instantiated button so we can change the image
        beatmapButtonInstantiateChildImage = beatmapButtonInstantiate.gameObject.transform.GetChild(0);


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
    }

    // Load a new beatmap image for the beatmap button instantiated
    private IEnumerator LoadNewBeatmapButtonImage(int _beatmapButtonIndex)
    {
        completePath = "file://" + songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
            @"\" + imageName + imageType;

        fileCheckPath = songSelectManager.beatmapDirectories[_beatmapButtonIndex] +
            @"\" + imageName + imageType;

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
}
