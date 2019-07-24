using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Registration : MonoBehaviour {

    // UI
    public TMP_InputField usernameInputField, passwordInputField;
    public TextMeshProUGUI pressAnywhereText;
    public Image incorrectDetailsImage;
    public TextMeshProUGUI usernameFieldDescription, passwordFieldDescription;
    public Button submitButton, registerCanvas, enterGameCanvas;

    // Gameobjects
    public GameObject accountProgressIcon; // The loading icon for logging in and signing up

    // Strings
    private string username, password, registerSuccessValue;

    private void Start()
    {
        // Initialize
        registerSuccessValue = "Account created successfully";
    }

    // Activate the username field description game object
    public void ActivateUsernameFieldDescription()
    {
        // Activate the username field description
        usernameFieldDescription.gameObject.SetActive(true);
        // Deactivate the password field description
        passwordFieldDescription.gameObject.SetActive(false);
    }

    // Activate the password field description game object
    public void ActivatePasswordFieldDescription()
    {
        // Deactivate the username field description
        usernameFieldDescription.gameObject.SetActive(false);
        // Activate the password field description 
        passwordFieldDescription.gameObject.SetActive(true);
    }

    // Call the register function
    public void CallRegister()
    {
        // Register an account 
        StartCoroutine(Register());

        // Enable the loading icon
        EnableAccountProgressLoadingIcon();
    }

    // Register account online 
    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        username = usernameInputField.text;
        password = passwordInputField.text;
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/register.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        // If no errors were found during registering 
        if (www.error == null)
        {
            // REGISTER SUCCESS

            // Disable the register
            DisableRegisterCanvas();
            // Enable the enter game canvas
            EnableEnterGameCanvas();

            // Disable the loading icon
            DisableAccountProgressLoadingIcon();

            // Update press anywhere text
            pressAnywhereText.text = registerSuccessValue;
        }
        else
        {
            // REGISTER FAILED

            // Activate the register failed icon
            incorrectDetailsImage.gameObject.SetActive(true);

            // Disable the loading icon
            DisableAccountProgressLoadingIcon();
        }
    }

    // Verify the inputs in the username and password fields, ensure they're the correct length
    public void VerifyInputs()
    {
        // Activate the submit button if within the length
        submitButton.interactable = (usernameInputField.text.Length >= 4 && usernameInputField.text.Length <= 10 && passwordInputField.text.Length >= 5);
    }

    // Disable the register canvas
    public void DisableRegisterCanvas()
    {
        // Disable the register canvas
        registerCanvas.gameObject.SetActive(false);

        // Disable the loading icon
        DisableAccountProgressLoadingIcon();
    }

    // Enable the enter game canvas
    public void EnableEnterGameCanvas()
    {
        // Enable the enter game canvas
        enterGameCanvas.gameObject.SetActive(true);
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
