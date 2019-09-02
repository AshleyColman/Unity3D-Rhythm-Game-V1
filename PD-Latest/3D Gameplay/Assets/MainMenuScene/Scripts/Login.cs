using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour {

    // UI
    public TMP_InputField usernameInputField, passwordInputField; // Login input fields for getting the username and password values
    public Image incorrectDetailsImage; // Error image that appears if incorrect details / login failure
    public GameObject loggedInCanvas, loginCanvas;
    public Button submitButton;

    // Gameobjects
    public GameObject accountProgressIcon; // The loading icon for logging in and signing up

    // Strings
    private string username, password, guestUsername;

    // Scripts
    private EnterGameScene enterGameScene;
    private StartSceneEnterGame startSceneEnterGame;

    private void Start()
    {
        // Initialize
        guestUsername = "GUEST";

        // References
        enterGameScene = FindObjectOfType<EnterGameScene>();
        startSceneEnterGame = FindObjectOfType<StartSceneEnterGame>();
    }

    // Login to the game offline/online as a guest account
    public void LoginAsGuest()
    {
        // Set the username 
        MySQLDBManager.username = guestUsername;
        // Check if the user has logged in
        enterGameScene.CheckIfLoggedIn();
        // Disable the login canvas
        DisableLoginCanvas();
        // Enable the logged in canvas
        EnableLoggedInCanvas();

        // Select the play button
        startSceneEnterGame.SelectPlayModeButton();
    }

    // Call the login function
    public void CallLogin()
    {
        // Attempt to log the user in
        StartCoroutine(LoginUser());

        // Enable the loading icon
        EnableAccountProgressLoadingIcon();
    }
    
    // Attempt to log the user in online
    private IEnumerator LoginUser()
    {
        username = usernameInputField.text;
        password = passwordInputField.text;

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/login.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();


        // Check if the login was success or fail
        if (www.downloadHandler.text == "0")
        {
            // SUCCESS 

            // Update the username with the value from the username field
            MySQLDBManager.username = usernameInputField.text;
            // Disable the login canvas
            DisableLoginCanvas();
            // Enable the logged in canvas
            EnableLoggedInCanvas();
            // Check if the user has logged in
            enterGameScene.CheckIfLoggedIn();
        }
        // Check if the login was success or fail
        if (www.downloadHandler.text == "1")
        {
            // ERROR - LOGIN FAILED

            // Display the incorrect details icon
            incorrectDetailsImage.gameObject.SetActive(true);

            // Disable the loading icon
            DisableAccountProgressLoadingIcon();
        }
    }

    // Verify the inputs to ensure the username and password are in length
    public void VerifyInputs()
    {
        submitButton.interactable = (usernameInputField.text.Length >= 4 && usernameInputField.text.Length <= 10 && passwordInputField.text.Length >= 5);
    }

    // Disable the login canvas
    public void DisableLoginCanvas()
    {
        // Disable the register canvas
        loginCanvas.gameObject.SetActive(false);
        // Disable the loading icon
        DisableAccountProgressLoadingIcon();

        // Select register button
        startSceneEnterGame.SelectRegisterButton();
    }

    // Enable the enter game canvas
    public void EnableLoggedInCanvas()
    {
        // Enable the enter game canvas
        loggedInCanvas.gameObject.SetActive(true);
    }

    // Disable the loading icon
    private void DisableAccountProgressLoadingIcon()
    {
        accountProgressIcon.gameObject.SetActive(false);
    }

    // Enable the loading icon
    private void EnableAccountProgressLoadingIcon()
    {
        accountProgressIcon.gameObject.SetActive(true);
    }
}
