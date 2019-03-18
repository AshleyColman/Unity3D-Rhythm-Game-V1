using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginUser());
    }
    
    IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInputField.text);
        form.AddField("password", passwordInputField.text);

        WWW www = new WWW("http://localhost/RhythmGameX/sqlconnect/login.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            MySQLDBManager.username = usernameInputField.text;
            SceneManager.LoadScene(6);
        }
        else
        {
            Debug.Log("User login failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (usernameInputField.text.Length >= 5 && passwordInputField.text.Length >= 5);
    }
}
