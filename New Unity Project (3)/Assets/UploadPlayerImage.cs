using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UploadPlayerImage : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI playerNameText;

    // Inputfield
    public TMP_InputField imageUrlInputField;

    // Image
    public Image beatmapCreatorProfileImage, downloadBeatmapCreatorProfileImage, playerImage;

    // Gamebobjects
    public GameObject beatmapCreatorProfileImageLoadingIcon, downloadCreatorProfileImageLoadingIcon;

    // Strings
    private string image_url, username;
    #endregion

    #region Properties
    public Image PlayerImage
    {
        get { return playerImage; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        // TEST - DELETE THIS
        MySQLDBManager.username = "Ashley";



        switch (MySQLDBManager.loggedIn)
        {
            case true:
                // Set username
                username = MySQLDBManager.username;
                // Set username to guest
                playerNameText.text = "PLAYING AS " + username.ToUpper();
                // Load player image
                StartCoroutine(RetrievePlayerImage(username, playerImage));
                break;
            case false:
                // Set username to guest
                playerNameText.text = "PLAYING AS GUEST";
                break;
        }
    }

    // Get and upload the beatmap creator image
    public void CallBeatmapCreatorUploadImage(string _beatmapCreatorUsername, Image _image)
    {
        // Deactivate image
        _image.gameObject.SetActive(false);

        // Activate loading icon
        if (_image == beatmapCreatorProfileImage)
        {
            beatmapCreatorProfileImageLoadingIcon.gameObject.SetActive(true);
        }
        else if (_image == downloadBeatmapCreatorProfileImage)
        {
            downloadCreatorProfileImageLoadingIcon.gameObject.SetActive(true);
        }

        // Attempt to load the image on entering the game
        StartCoroutine(RetrievePlayerImage(_beatmapCreatorUsername, _image));
    }

    // Call the upload image function
    public void CallUploadImage()
    {
        if (MySQLDBManager.loggedIn == true)
        {
            // Attempt to submit player image
            StartCoroutine(UploadNewProfileImageURL());
        }
    }

    // Retrieve the player image
    private IEnumerator RetrievePlayerImage(string _username, Image _image)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _username);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/rhythmgamex/retrieve_player_image.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        switch (www.downloadHandler.text)
        {
            case "1":
                // ERROR 
                break;
            default:
                // SUCCESS - Load the player image with the value from the database - user image url saved
                StartCoroutine(LoadPlayerImg(www.downloadHandler.text, _image));
                break;
        }
    }

    // Load the player image
    IEnumerator LoadPlayerImg(string _url, Image _image)
    {
        if (_url != "")
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log("Error uploading profile image");
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);

                    _image.material.mainTexture = texture;

                    // Set image to false then to true to activate new image
                    _image.gameObject.SetActive(false);
                    _image.gameObject.SetActive(true);

                    // Display loading icon
                    if (_image == beatmapCreatorProfileImage)
                    {
                        beatmapCreatorProfileImageLoadingIcon.gameObject.SetActive(false);
                    }
                    else if (_image == downloadBeatmapCreatorProfileImage)
                    {
                        downloadCreatorProfileImageLoadingIcon.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    // Upload a new image URL - Save to the database for player and load the image
    private IEnumerator UploadNewProfileImageURL()
    {
        // Get URL from input field
        image_url = imageUrlInputField.text;

        WWWForm form = new WWWForm();
        form.AddField("image_url", image_url);
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/uploadplayerimage.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        switch (www.downloadHandler.text)
        {
            case "0":
                // SUCCESS - Load the player image with the value from the image url input field
                StartCoroutine(LoadPlayerImg(image_url, playerImage));
                break;
            default:
                // ERROR - Upload failed
                break;
        }
    }
    #endregion
}
