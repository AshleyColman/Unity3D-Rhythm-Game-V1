using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UploadPlayerImage : MonoBehaviour
{

    // UI
    public TextMeshProUGUI playerNameText;
    public TMP_InputField imageUrlInputField;
    public Image playerImage;
    public GameObject uploadPlayerImagePanel;
    public Button uploadImageButton;

    // Strings
    private string image_url, username;

    // Properties
    public Image PlayerImage
    {
        get { return playerImage; }
    }

    private void Start()
    {
        // TESTING REMOVE THIS
        MySQLDBManager.username = "Ashley";
        
        if (MySQLDBManager.loggedIn == false || MySQLDBManager.username == "GUEST")
        {
            //uploadImageButton.interactable = false;

            // Set player name
            playerNameText.text = "GUEST";
        }
        else
        {
            // Attempt to load the image on entering the game
            StartCoroutine(RetrievePlayerImage());

            if (MySQLDBManager.loggedIn == true)
            {
                // Set player name
                playerNameText.text = MySQLDBManager.username.ToUpper();
            }
        }
    }

    // Call the upload image function
    public void CallUploadImage()
    {
        if (MySQLDBManager.loggedIn == true && MySQLDBManager.username != "GUEST")
        {
            // Attempt to submit player image
            StartCoroutine(AttemptToUploadPlayerImage());

            // Clear the text field
            //imageUrlInputField.text = "";

            // Enable the loading icon
            //EnableAccountProgressLoadingIcon();
        }
        else
        {
            // Cannot upload as guest or not logged in error message
        }
    }


    // Retrieve the player image
    private IEnumerator RetrievePlayerImage()
    {
        username = MySQLDBManager.username;
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/retrieveuserimage.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Check if the login was success or fail
        if (www.downloadHandler.text == "1")
        {
            // ERROR - UPLOAD FAILED
            Debug.Log("error with loading image at start");
        }
        else
        {
            // SUCCESS 

            if (www.downloadHandler.text != "")
            {
                // Load the player image with the value from the database - user image url saved
                StartCoroutine(LoadPlayerImg(www.downloadHandler.text));
            }

        }
    }

    // Load the player image
    IEnumerator LoadPlayerImg(string _url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                playerImage.material.mainTexture = texture;

                // Set image to false then to true to activate new image
                playerImage.gameObject.SetActive(false);
                playerImage.gameObject.SetActive(true);
            }
        }
    }


    private IEnumerator AttemptToUploadPlayerImage()
    {
        image_url = imageUrlInputField.text;
        username = MySQLDBManager.username;
        WWWForm form = new WWWForm();
        form.AddField("image_url", image_url);
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/uploadplayerimage.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Check if the login was success or fail
        if (www.downloadHandler.text == "0")
        {
            // SUCCESS 

            // Load the player image with the value from the image url input field
            StartCoroutine(LoadPlayerImg(image_url));
        }
        // Check if the login was success or fail
        if (www.downloadHandler.text != "0")
        {
            // ERROR - UPLOAD FAILED
        }
    }


    // Activate the player image panel
    public void ActivatePlayerImagePanel()
    {
        uploadPlayerImagePanel.gameObject.SetActive(true);
    }

    // Deactivate the player image panel
    public void DeactivatePlayerImagePanel()
    {
        uploadPlayerImagePanel.gameObject.SetActive(false);
    }
}
