using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterGameScene : MonoBehaviour {

    public Text usernameDisplayText;
    public Button loginCanvas; // The login detail canvas
    public Button registerCanvas; // The register canvas
    public Button enterGameCanvas; // The enter game canvas

    void Start()
    {
        // Disable the login canvas on start
        loginCanvas.gameObject.SetActive(false);
        // Disable the register canvas on start
        registerCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (MySQLDBManager.loggedIn)
        {
            usernameDisplayText.text = "Logged in as: " + MySQLDBManager.username;
        }
    }
    public void GoToRegister()
    {
        // Disable the register canvas
        registerCanvas.gameObject.SetActive(true);
        // Disable enter game canvas
        enterGameCanvas.gameObject.SetActive(false);
    }

    public void GoToLogin()
    {
        // Disable the login canvas
        loginCanvas.gameObject.SetActive(true);
        // Disable enter game canvas
        enterGameCanvas.gameObject.SetActive(false);
    }
}
