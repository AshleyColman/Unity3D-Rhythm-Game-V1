using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    // UI
    public TMP_InputField usernameInputField, passwordInputField; // Login input fields for getting the username and password values
    public Button loginButton;
    public TextMeshProUGUI successStatusText, failedStatusText;

    // Strings
    private string username, password, guestUsername;

    // Scripts
    private MenuManager menuManager;
    private StartMenu startMenu;

    private void Start()
    {
        // Initialize
        guestUsername = "GUEST";

        // Reference
        menuManager = FindObjectOfType<MenuManager>();
        startMenu = FindObjectOfType<StartMenu>();
    }

    // Login to the game offline/online as a guest account
    public void LoginAsGuest()
    {
        // Set the username 
        MySQLDBManager.username = guestUsername;
    }

    // Call the login function
    public void CallLogin()
    {
        // Attempt to log the user in
        StartCoroutine(LoginUser());
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

            // Display text
            failedStatusText.gameObject.SetActive(false);
            successStatusText.gameObject.SetActive(true);

            // Transition to main menu
            menuManager.StartMenuToMainMenu();
        }
        // Check if the login was success or fail
        if (www.downloadHandler.text == "1")
        {
            // ERROR - LOGIN FAILED

            // Display text
            failedStatusText.gameObject.SetActive(true);
        }
    }

    // Verify the inputs in the username and password fields, ensure they're the correct length
    public void VerifyInputs()
    {
        // Activate the submit button if within the length
        loginButton.interactable = (usernameInputField.text.Length >= 5 && usernameInputField.text.Length <= 8 && passwordInputField.text.Length >= 5 && passwordInputField.text.Length <= 15);
    }
}
