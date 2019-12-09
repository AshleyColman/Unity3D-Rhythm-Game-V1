using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Register : MonoBehaviour
{
    // UI
    public TMP_InputField usernameInputField, passwordInputField;
    public TextMeshProUGUI successStatusText, failedStatusText;
    public Button registerButton;

    // Strings
    private string username, password;

    // Scripts
    private StartMenu startMenu;

    private void Start()
    {
        startMenu = FindObjectOfType<StartMenu>();
    }

    // Call the register function
    public void CallRegister()
    {
        // Register an account 
        StartCoroutine(AttemptRegister());
    }

    // Register account online 
    IEnumerator AttemptRegister()
    {
        WWWForm form = new WWWForm();
        username = usernameInputField.text;
        password = passwordInputField.text;
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/register.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // Check if success
        if (www.downloadHandler.text == "0")
        {
            // REGISTER SUCCESS

            // Update text
            failedStatusText.gameObject.SetActive(false);
            successStatusText.gameObject.SetActive(true);

            // Show login panel
            startMenu.ShowLoginProfileMenu();
        }
        else
        {
            // REGISTER FAILED

            // Enable text
            failedStatusText.gameObject.SetActive(true);
        }
    }

    // Verify the inputs in the username and password fields, ensure they're the correct length
    public void VerifyInputs()
    {
        // Activate the submit button if within the length
        registerButton.interactable = (usernameInputField.text.Length >= 5 && usernameInputField.text.Length <= 8 && passwordInputField.text.Length >= 5 && passwordInputField.text.Length <= 15);
    }


}
