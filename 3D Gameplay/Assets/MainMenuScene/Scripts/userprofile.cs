using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userprofile : MonoBehaviour {

    public Text usernameText;

    void Update()
    {
        if (MySQLDBManager.loggedIn)
        {
            usernameText.text = "Logged in as: " + MySQLDBManager.username;
        }
    }
}
