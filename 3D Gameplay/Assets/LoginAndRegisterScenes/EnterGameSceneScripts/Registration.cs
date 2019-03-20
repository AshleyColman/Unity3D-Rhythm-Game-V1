using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Registration : MonoBehaviour {

    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button submitButton;
    public Button registerCanvas; // The register canvas
    public Button enterGameCanvas; // The enter game canvas
    public string username;
    public string password;
    public string error;


    public void CallRegister()
    {
        StartCoroutine(Register());
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
        }
        else
        {
            error = "error";
            Debug.Log("User creation failed.");
            Debug.Log(www.downloadHandler.text);
        }
    }


    public void VerifyInputs()
    {
        submitButton.interactable = (usernameInputField.text.Length >= 5 && usernameInputField.text.Length <= 10 && passwordInputField.text.Length >= 5);
    }

    // Disable the register canvas
    public void DisableRegisterCanvas()
    {
        // Disable the register canvas
        registerCanvas.gameObject.SetActive(false);
    }

    // Enable the enter game canvas
    public void EnableEnterGameCanvas()
    {
        // Enable the enter game canvas
        enterGameCanvas.gameObject.SetActive(true);
    }
}
