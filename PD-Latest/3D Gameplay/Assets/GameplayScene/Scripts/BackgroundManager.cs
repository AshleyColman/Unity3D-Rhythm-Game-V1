using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Profiling;
using UnityEngine.Networking;

public class BackgroundManager : MonoBehaviour {

    // Image path variables
    public string filePath = "";
    string imageName = "img";
    string imageType = ".png";

    public string completePath = "";

    // The image to change
    public Image img;

    bool hasLoadedImage; 

    // Get the reference, load the image on start only if the gameplay scene is active
    private LevelChanger levelChanger;

    // Default image if no image has been found
    public Image defaultImage;

    public Texture2D imageTexture;



    void Awake()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
        hasLoadedImage = false;
    }

    void Update()
    {
        // If on gameplay scene load the image in awake
        if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex && hasLoadedImage == false
            || levelChanger.CurrentLevelIndex == levelChanger.ResultsSceneIndex && hasLoadedImage == false)
        {
            LoadBeatmapImage();
            hasLoadedImage = true;
        }
    }


    /*
    // Load the image
    IEnumerator LoadImg()
    {
        yield return 0;
        WWW imgLink = new WWW("file://" + completePath);
        yield return imgLink;

        img.material.mainTexture = imgLink.textureNonReadable;

        //imgLink.LoadImageIntoTexture(img.mainTexture as Texture2D);
    }
    */
    IEnumerator LoadImg()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + completePath))
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

                img.material.mainTexture = texture;

                // Set image to false then to true to activate new image
                img.gameObject.SetActive(false);
                img.gameObject.SetActive(true);
            }
        }
    }


    // Get the image url from the database
    public void LoadBeatmapImage()
    {
        filePath = Database.database.LoadedBeatmapFolderDirectory; // Get the image filepath from the beatmap file
        completePath = filePath + imageName + imageType;

        // Check if the beatmap image exists in the directory / Activate or disable 
        CheckAndLoadBeatmapImage();
    }

    // Get the image url from the url passed from the editor
    public void LoadEditorBeatmapImage(string _filePath)
    {
        filePath = _filePath + @"\";
        completePath = filePath + imageName + imageType;

        // Check if the beatmap image exists in the directory / Activate or disable 
        CheckAndLoadBeatmapImage();
    }

    // Check if the beatmap image exists in the directory / Activate or disable 
    private void CheckAndLoadBeatmapImage()
    {
        // Check if the image file exists in the beatmap directory 
        if (File.Exists(completePath) == false)
        {
            // Deactivate the selectedSongImage which changes to the beatmap image if found
            img.gameObject.SetActive(false);
            // Activate the default image as no file has been found
            defaultImage.gameObject.SetActive(true);
        }
        else
        {
            // Activate the selecteSongImage as an image file has been found
            img.gameObject.SetActive(true);
            // Deactivate the default image
            defaultImage.gameObject.SetActive(false);

            // Load the image from the URL
            StartCoroutine(LoadImg());
        }
    }
}
