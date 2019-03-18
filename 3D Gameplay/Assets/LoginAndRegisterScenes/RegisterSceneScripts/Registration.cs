using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour {

    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button submitButton;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInputField.text);
        form.AddField("password", passwordInputField.text);

        WWW www = new WWW("http://localhost/RhythmGameX/sqlconnect/register.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("user created successfully");
            UnityEngine.SceneManagement.SceneManager.LoadScene(6);
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (usernameInputField.text.Length >= 5 && passwordInputField.text.Length >= 5);
    }
}
