using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectPanel : MonoBehaviour
{

    public GameObject beatmapButton; // The button to instantiate
    private List<GameObject> instantiatedBeatmapButtonList = new List<GameObject>();
    public List<Image> instantiatedBeatmapButtonImageList = new List<Image>();
    private SongSelectManager songSelectManager; // Reference to the song select manager which manages loading songs, used to get the beatmap img addresses for loading images

    int beatmapDirectoryCount; // Number of beatmap folders in the beatmap directory

    public string[] beatmapDirectoryPaths; // The beatmap directory paths


    // Image path variables
    string imageName = "img";
    string imageType = ".png";
    public string completePath = "";

    Texture2D beatmapButtonNewImageTexture;

    private bool hasLoadedAllBeatmapDirectories;

    int beatmapButtonIndexToGet;

    List<WWW> beatmapButtonTexturesList = new List<WWW>();

    // Use this for initialization
    void Start()
    {
        beatmapButtonIndexToGet = 0;

        // Set to false at the start, set to true when all have loaded
        hasLoadedAllBeatmapDirectories = false;

        // Assign the reference 
        songSelectManager = FindObjectOfType<SongSelectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLoadedAllBeatmapDirectories == false)
        {
            // Load the beatmap buttons in the scroll list
            for (int beatmapButtonIndex = 0; beatmapButtonIndex < beatmapDirectoryCount; beatmapButtonIndex++)
            {
                InstantiateBeatmapButton(beatmapButtonIndexToGet);

                // Increment the beatmapButtonIndexToGet (Increments after each coroutine only so it loads the image 1 after another)
                beatmapButtonIndexToGet++;
            }

            beatmapButtonIndexToGet = 0;

            for (int i = 0; i < beatmapDirectoryCount; i++)
            {
                // Change the beatmap image
                StartCoroutine(LoadNewBeatmapButtonImage(beatmapButtonIndexToGet));

                beatmapButtonIndexToGet++;
            }

            hasLoadedAllBeatmapDirectories = true;
        }
    }

    // Get the number of beatmap folders in the beatmap folder
    public void GetBeatmapFolderCount()
    {
        beatmapDirectoryCount = songSelectManager.beatmapDirectories.Length;
    }

    // Get the beatmap directory paths
    public void GetBeatmapDirectoryPaths()
    {
        // Initialise the array with the amount of beatmap directories found
        beatmapDirectoryPaths = new string[beatmapDirectoryCount];

        // Loop, get and store the beatmap directories for all beatmaps in the beatmap folder
        for (int i = 0; i < beatmapDirectoryPaths.Length; i++)
        {
            // Assign the paths
            beatmapDirectoryPaths[i] = songSelectManager.beatmapDirectories[i];
        }
    }

    private void InstantiateBeatmapButton(int beatmapButtonIndexPass)
    {
        // Set to 500 on z to fix the "moving image" problem, instantiates the images to z of 0 so the images don't move when the mouse cursor has moved
        Vector3 beatmapButtonPosition = new Vector3(0, 0, 500);

        // Assign the index and image to this button
        GameObject beatmapButtonInstantiate = Instantiate(beatmapButton, beatmapButtonPosition, Quaternion.Euler(0, 0, -45),
        GameObject.FindGameObjectWithTag("ButtonListContent").transform) as GameObject;
        // Add the instantiated button to the list
        instantiatedBeatmapButtonList.Add(beatmapButtonInstantiate);

        // Get the child image transform from the instantiated button so we can change the image
        Transform beatmapButtonInstantiateChildImage = beatmapButtonInstantiate.gameObject.transform.GetChild(0);

        // Get the image component of the instantiated button
        Image instantiatedBeatmapButtonImage = beatmapButtonInstantiateChildImage.GetComponent<Image>();

        // Store in the list so we can change it later
        instantiatedBeatmapButtonImageList.Add(instantiatedBeatmapButtonImage);

        // Create a new texture for the beatmap image
        beatmapButtonNewImageTexture = CreateNewTextureForBeatmapButton();

        Sprite newSprite;

        newSprite = Sprite.Create(beatmapButtonNewImageTexture, new Rect(0.0f, 0.0f, beatmapButtonNewImageTexture.width, beatmapButtonNewImageTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

        instantiatedBeatmapButtonImage.sprite = newSprite;

        // Connect texture to material of GameObject this script is attached to
        //instantiatedBeatmapButtonImage.material.mainTexture = beatmapButtonNewImageTexture;


        // Get the beatmap button script component attached to the newly instantiated button
        // Assign the beatmap index to load inside the script
        BeatmapButton instantiatedBeatmapButtonScript = beatmapButtonInstantiate.GetComponent<BeatmapButton>();
        instantiatedBeatmapButtonScript.SetBeatmapButtonIndex(beatmapButtonIndexPass);
    }




    // Load a new beatmap image for the beatmap button instantiated
    private IEnumerator LoadNewBeatmapButtonImage(int beatmapButtonIndexPass)
    {
       
        // Has img URL
        // Sends img URL
        // Path is Updated
        // Finally gets back and path has changed
        // Need a way of getting the path


        // Recieves the image from the song list passed from the load beatmap script
        yield return 0;

        
        WWW imgLink = new WWW("file://" + beatmapDirectoryPaths[beatmapButtonIndexPass] + @"\" + imageName + imageType);

        yield return imgLink;

        beatmapButtonTexturesList.Add(imgLink);


        // Load the image for the beatmapButton instantiated
        imgLink.LoadImageIntoTexture(instantiatedBeatmapButtonImageList[beatmapButtonIndexPass].mainTexture as Texture2D);
    }






    private Texture2D CreateNewTextureForBeatmapButton()
    {
        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        var texture = new Texture2D(1080, 720, TextureFormat.ARGB32, false);

        // set the pixel values
        texture.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        texture.SetPixel(1, 0, Color.clear);
        texture.SetPixel(0, 1, Color.white);
        texture.SetPixel(1, 1, Color.black);

        // Apply all SetPixel calls
        texture.Apply();

        return texture;
    }
}
