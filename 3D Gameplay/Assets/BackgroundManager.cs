using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BackgroundManager : MonoBehaviour {

    // Image path variables
    public string filePath = "";
    string imageName = "img";
    string imageType = ".png";

    public string completePath = "";

    // The image to change
    public Image img;

    // Get the reference, load the image on start only if the gameplay scene is active
    private LevelChanger levelChanger; 

    void Awake()
    {
        levelChanger = FindObjectOfType<LevelChanger>();


    }

    void Update()
    {
        // If on gameplay scene load the image in awake
        if (levelChanger.currentLevelIndex == 4)
        {
            LoadBeatmapImage();
        }
    }

    // Load the image
    IEnumerator LoadImg()
    {
        yield return 0;
        WWW imgLink = new WWW("file://" + completePath);
        yield return imgLink;
        imgLink.LoadImageIntoTexture(img.mainTexture as Texture2D);
    }

    // Get the image url from the database
    public void LoadBeatmapImage()
    {
        filePath = Database.database.beatmapFolderDirectory; // Get the image filepath from the beatmap file
        completePath = filePath + imageName + imageType;

        // Load the image from the URL
        StartCoroutine(LoadImg());
    }

    // Get the image url from the url passed from the editor
    public void LoadEditorBeatmapImage(string filePathPassed)
    {
        filePath = filePathPassed + @"\";
        completePath = filePath + imageName + imageType;

        // Load the image from the URL
        StartCoroutine(LoadImg());
    }



}
