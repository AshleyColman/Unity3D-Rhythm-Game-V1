using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterGameScene : MonoBehaviour {

    public TextMeshProUGUI usernameDisplayText;
    public TextMeshProUGUI levelDisplayText;
    public Button loginCanvas; // The login detail canvas
    public Button registerCanvas; // The register canvas
    public Button enterGameCanvas; // The enter game canvas
    public Button loggedInCanvas; // The logged in canvas

    void Start()
    {
    }

    void Update()
    {
        if (MySQLDBManager.loggedIn)
        {
            usernameDisplayText.text = "Signed in as: " + MySQLDBManager.username;
            loggedInCanvas.gameObject.SetActive(true);
            enterGameCanvas.gameObject.SetActive(false);
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
