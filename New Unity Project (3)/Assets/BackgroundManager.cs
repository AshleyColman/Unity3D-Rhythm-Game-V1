using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.Video;

public class BackgroundManager : MonoBehaviour
{
    // Animator
    public Animator backgroundImageTransitionAnimator, videoPlayerTransitionAnimator;

    public GameObject loadingIcon;

    // UI
    public Button videoTickBoxButton;
    public Image img, img2;
    public Image videoTickBoxSelectedImage;
    public Texture2D imageTexture;

    // Video 
    public VideoPlayer videoPlayer, videoPlayer2;

    private int activeVideoPlayerIndex, activeBackgroundImageIndex;

    // Strings
    public string filePath = "";
    private const string imageName = "img";
    private const string imageType = ".png";
    private const string videoName = "video";
    private const string videoType = ".mp4";
    private string completeImagePath = "", completeVideoPath = "";

    // Bools
    private bool loadSecondBackgroundImage, loadSecondVideoPlayer;
    private bool videoTickBoxSelected;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public int ActiveVideoPlayerIndex
    {
        get { return activeVideoPlayerIndex; }
    }

    public int ActiveBackgroundImageIndex
    {
        get { return activeBackgroundImageIndex; }
    }

    public bool VideoTickBoxSelected
    {
        get { return videoTickBoxSelected; }
    }

    void Awake()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        videoTickBoxSelected = false;
        loadSecondBackgroundImage = false;
        loadSecondVideoPlayer = false;

        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.MenuSceneIndex)
        {
            videoTickBoxSelectedImage.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Toggle video tick box on or off 
    public void ToggleVideoTickBox()
    {
        // Change bools when button is clicked
        if (videoTickBoxSelected == true)
        {
            // Disable tick image
            videoTickBoxSelectedImage.gameObject.SetActive(false);

            // Display message panel
            scriptManager.messagePanel.DisplayVideoToggleOffMessage();

            // Set to false
            videoTickBoxSelected = false;
        }
        else if (videoTickBoxSelected == false)
        {
            // Enable tick image
            videoTickBoxSelectedImage.gameObject.SetActive(true);

            // Display message panel
            scriptManager.messagePanel.DisplayVideoToggleOnMessage();

            // Set to true
            videoTickBoxSelected = true;
        }
    }

    IEnumerator LoadImg()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + completeImagePath))
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

    IEnumerator LoadNextBackgroundImg()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + completeImagePath))
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

                // Check texture size
                float textureWidth = texture.width;
                float textureHeight = texture.height;
                float aspectRatio = (textureWidth / textureHeight);

                if (loadSecondBackgroundImage == false)
                {
                    // Set to true to load second background image next time
                    loadSecondBackgroundImage = true;

                    // Set active index
                    activeBackgroundImageIndex = 1;

                    // Activate the selecteSongImage as an image file has been found
                    img.gameObject.SetActive(true);

                    // Load image for background image 1
                    img.material.mainTexture = texture;

                    // Set aspect ratio
                    SetAspectRatio(aspectRatio, img);

                    // Set image to false then to true to activate new image
                    img.gameObject.SetActive(false);
                    img.gameObject.SetActive(true);

                    // Play transition animation
                    backgroundImageTransitionAnimator.Play("BackgroundImage2ToBackgroundImage1_Animation", 0, 0f);
                }
                else if (loadSecondBackgroundImage == true)
                {
                    // Set to false to load first background image next time
                    loadSecondBackgroundImage = false;

                    // Set active index
                    activeBackgroundImageIndex = 2;

                    // Activate the selectedSongImage as an image file has been found
                    img2.gameObject.SetActive(true);

                    // Load image for background image 1
                    img2.material.mainTexture = texture;

                    // Set aspect ratio
                    SetAspectRatio(aspectRatio, img2);

                    // Set image to false then to true to activate new image
                    img2.gameObject.SetActive(false);
                    img2.gameObject.SetActive(true);

                    // Play transition animation
                    backgroundImageTransitionAnimator.Play("BackgroundImage1ToBackgroundImage2_Animation", 0, 0f);
                }
            }
        }
    }

    // Load an online image
    private IEnumerator LoadNextBackgroundImgURL(string _url)
    {
        // Enable loading icon
        loadingIcon.gameObject.SetActive(true);

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                // Error
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Check texture size
                float textureWidth = texture.width;
                float textureHeight = texture.height;
                float aspectRatio = (textureWidth / textureHeight);

                if (loadSecondBackgroundImage == false)
                {
                    // Set to true to load second background image next time
                    loadSecondBackgroundImage = true;

                    // Set active index
                    activeBackgroundImageIndex = 1;

                    // Activate the selecteSongImage as an image file has been found
                    img.gameObject.SetActive(true);

                    // Load image for background image 1
                    img.material.mainTexture = texture;

                    // Set aspect ratio
                    SetAspectRatio(aspectRatio, img);

                    // Set image to false then to true to activate new image
                    img.gameObject.SetActive(false);
                    img.gameObject.SetActive(true);

                    // Play transition animation
                    backgroundImageTransitionAnimator.Play("BackgroundImage2ToBackgroundImage1_Animation", 0, 0f);
                }
                else if (loadSecondBackgroundImage == true)
                {
                    // Set to false to load first background image next time
                    loadSecondBackgroundImage = false;

                    // Set active index
                    activeBackgroundImageIndex = 2;

                    // Activate the selectedSongImage as an image file has been found
                    img2.gameObject.SetActive(true);

                    // Load image for background image 1
                    img2.material.mainTexture = texture;

                    // Set aspect ratio
                    SetAspectRatio(aspectRatio, img2);

                    // Set image to false then to true to activate new image
                    img2.gameObject.SetActive(false);
                    img2.gameObject.SetActive(true);

                    // Play transition animation
                    backgroundImageTransitionAnimator.Play("BackgroundImage1ToBackgroundImage2_Animation", 0, 0f);
                }

                // Activate the animator gameobject
                scriptManager.downloadPanel.songSelectInformationAnimator.gameObject.SetActive(true);

                // Enable download panel song information panel animation
                scriptManager.downloadPanel.songSelectInformationAnimator.Play("DownloadSongInformationPanel_Animation", 0, 0f);

                // Disable loading icon
                loadingIcon.gameObject.SetActive(false);
            }
        }
    }

    private void SetAspectRatio(float _aspectRatio, Image _img)
    {
        if (_aspectRatio == 1)
        {
            _img.rectTransform.sizeDelta = new Vector2(950, 950);
        }
        else
        {
            _img.rectTransform.sizeDelta = new Vector2(1650, 950);
        }
    }

    // Load video file from beatmap folder
    private void LoadVideo()
    {
        // Set the video player url
        videoPlayer.url = completeVideoPath;
    }

    // Load the background image using an online url address
    public void LoadBackgroundImageURL(string _url)
    {
        StartCoroutine(LoadNextBackgroundImgURL(_url));
    }

    // Load the next video
    private void LoadNextVideo()
    {
        if (loadSecondVideoPlayer == false)
        {
            // Set to true to load second videoPlayer next time
            loadSecondVideoPlayer = true;

            // Set active index
            activeVideoPlayerIndex = 1;

            // Activate the first videoPlayer
            videoPlayer.gameObject.SetActive(true);

            // Set the video player url
            videoPlayer.url = completeVideoPath;

            // Play transition animation
            videoPlayerTransitionAnimator.Play("VideoPlayer2ToVideoPlayer1_Animation", 0, 0f);
        }
        else if (loadSecondVideoPlayer == true)
        {
            // Set to false to load first videoPlayer next time
            loadSecondVideoPlayer = false;

            // Set active index
            activeVideoPlayerIndex = 2;

            // Activate the 2nd video player
            videoPlayer2.gameObject.SetActive(true);

            // Set the video player 2 url
            videoPlayer2.url = completeVideoPath;

            // Play transition animation
            videoPlayerTransitionAnimator.Play("VideoPlayer1ToVideoPlayer2_Animation", 0, 0f);
        }
    }

    // Load only the background image
    public void LoadImageOnly(string _filePath)
    {
        // Image file path
        filePath = _filePath + @"\";
        completeImagePath = filePath + imageName + imageType;

        if (File.Exists(completeImagePath))
        {
            // Disable video players
            videoPlayer.gameObject.SetActive(false);
            videoPlayer2.gameObject.SetActive(false);

            // Load the background image
            StartCoroutine(LoadNextBackgroundImg());
        }
    }

    // Load video if video file is found, load image if video is not found
    public void LoadVideoOrImage(string _filePath)
    {
        // Video file path
        filePath = _filePath + @"\";
        completeVideoPath = filePath + videoName + videoType;

        // Image file path
        filePath = _filePath + @"\";
        completeImagePath = filePath + imageName + imageType;

        // If the video file has been found
        if (File.Exists(completeVideoPath))
        {
            // Load the video
            LoadNextVideo();

            // Disable background images
            img.gameObject.SetActive(false);
            img2.gameObject.SetActive(false);
        }
        else if (File.Exists(completeImagePath))
        {
            // Disable video players
            videoPlayer.gameObject.SetActive(false);
            videoPlayer2.gameObject.SetActive(false);

            // Load the background image
            StartCoroutine(LoadNextBackgroundImg());
        }
    }
}
