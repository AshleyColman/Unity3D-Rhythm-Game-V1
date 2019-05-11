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

    bool hasLoadedImage; 

    // Get the reference, load the image on start only if the gameplay scene is active
    private LevelChanger levelChanger;

    // Song select preview images
    public Image selectedSongImage;
    public Image previousSongImage;
    public Image nextSongImage;
    public Image nextNextSongImage;

    private string selectedSongImageCompletePath;
    private string previousSongImageCompletePath;
    private string nextSongImageCompletePath;
    private string nextNextSongImageCompletePath;

    void Awake()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
        hasLoadedImage = false;

    }

    void Update()
    {
        // If on gameplay scene load the image in awake
        if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex && hasLoadedImage == false || levelChanger.currentLevelIndex == levelChanger.resultsSceneIndex && hasLoadedImage == false)
        {
            LoadBeatmapImage();
            hasLoadedImage = true;
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

    // Load current song preview list image
    public IEnumerator LoadSongSelectCurrentImg(string imgFilePathPass)
    {
        selectedSongImageCompletePath = imgFilePathPass + @"\" + imageName + imageType;
        Debug.Log(completePath);
        // Recieves the image from the song list passed from the load beatmap script
        yield return 0;
        WWW imgLink = new WWW("file://" + selectedSongImageCompletePath);
        yield return imgLink;
        imgLink.LoadImageIntoTexture(selectedSongImage.mainTexture as Texture2D);
    }

    // Load previous song preview list image
    public IEnumerator LoadSongSelectPreviousImg(string imgFilePathPass)
    {
        previousSongImageCompletePath = imgFilePathPass + @"\" + imageName + imageType;
        Debug.Log(completePath);
        // Recieves the image from the song list passed from the load beatmap script
        yield return 0;
        WWW imgLink = new WWW("file://" + previousSongImageCompletePath);
        yield return imgLink;
        imgLink.LoadImageIntoTexture(previousSongImage.mainTexture as Texture2D);
    }

    // Load next song preview list image
    public IEnumerator LoadSongSelectNextImg(string imgFilePathPass)
    {
        nextSongImageCompletePath = imgFilePathPass + @"\" + imageName + imageType;
        Debug.Log(completePath);
        // Recieves the image from the song list passed from the load beatmap script
        yield return 0;
        WWW imgLink = new WWW("file://" + nextSongImageCompletePath);
        yield return imgLink;
        imgLink.LoadImageIntoTexture(nextSongImage.mainTexture as Texture2D);
    }

    // Load next next  song preview list image
    public IEnumerator LoadSelectNextNextImg(string imgFilePathPass)
    {
        nextNextSongImageCompletePath = imgFilePathPass + @"\" + imageName + imageType;
        Debug.Log(completePath);
        // Recieves the image from the song list passed from the load beatmap script
        yield return 0;
        WWW imgLink = new WWW("file://" + nextNextSongImageCompletePath);
        yield return imgLink;
        imgLink.LoadImageIntoTexture(nextNextSongImage.mainTexture as Texture2D);
    }

}
