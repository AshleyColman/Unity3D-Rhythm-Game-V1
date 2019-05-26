using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Registration : MonoBehaviour {

    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Image incorrectDetailsImage;
    public TextMeshProUGUI usernameFieldDescription;
    public TextMeshProUGUI passwordFieldDescription;
    public Button submitButton;
    public Button registerCanvas; // The register canvas
    public Button enterGameCanvas; // The enter game canvas
    public string username;
    public string password;
    public string error;

    // The loading icon for logging in and signing up
    public GameObject accountProgressIcon;

    // Press anywhere text
    public TextMeshProUGUI pressAnywhereText;

    void Update()
    {
        if (usernameInputField.isFocused)
        {
            usernameFieldDescription.gameObject.SetActive(true);
            passwordFieldDescription.gameObject.SetActive(false);
        }
        else if (passwordInputField.isFocused)
        {
            usernameFieldDescription.gameObject.SetActive(false);
            passwordFieldDescription.gameObject.SetActive(true);
        }
    }

    public void CallRegister()
    {
        StartCoroutine(Register());

        // Enable the loading icon
        EnableAccountProgressLoadingIcon();
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        username = usernameInputField.text;
        password = passwordInputField.text;
        form.AddField("username", username);
        form.AddField("password", password);

        //WWW www = new WWW("http://rhythmgamex.knightstone.io/register.php", form);

        //yield return www;

        /*
        if (www.text == "0")
        {

        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
        }
        */



        UnityWebRequest www = UnityWebRequest.Post("http://rhythmgamex.knightstone.io/register.php", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            error = "success";
            Debug.Log("user created successfully");
            DisableRegisterCanvas();
            EnableEnterGameCanvas();

            // Disable the loading icon
            DisableAccountProgressLoadingIcon();

            // Update press anywhere text
            pressAnywhereText.text = "Account created successfully";
        }
        else
        {
            error = "error";
            Debug.Log("User creation failed.");
            Debug.Log(www.downloadHandler.text);

            incorrectDetailsImage.gameObject.SetActive(true);

            // Disable the loading icon
            DisableAccountProgressLoadingIcon();
        }
    }


    public void VerifyInputs()
    {
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
