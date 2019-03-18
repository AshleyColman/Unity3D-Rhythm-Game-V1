using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterGameScene : MonoBehaviour {

    public Text usernameDisplayText;

    void Start()
    {
        if (MySQLDBManager.loggedIn)
        {
            usernameDisplayText.text = "Logged in as: " + MySQLDBManager.username;
        }
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene(7);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(8);
    }
}
