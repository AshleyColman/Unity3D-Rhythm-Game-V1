using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterGameScene : MonoBehaviour {

    public TextMeshProUGUI usernameDisplayText;
    public Button loginCanvas; // The login detail canvas
    public Button registerCanvas; // The register canvas
    public Button enterGameCanvas; // The enter game canvas
    public Button loggedInCanvas; // The logged in canvas

    public TextMeshProUGUI pressAnywhereText;
    public GameObject modeButtonPanel;
    public Animator modeButtonPanelAnimator;

    public GameObject[] pressAnywhereDiamonds = new GameObject[2];

    private bool animationHasPlayed;

    void Start()
    {
    }

    void Update()
    {
        if (MySQLDBManager.loggedIn)
        {
            usernameDisplayText.text = "Playing as " + MySQLDBManager.username;
            loggedInCanvas.gameObject.SetActive(true);
            enterGameCanvas.gameObject.SetActive(false);

            if (animationHasPlayed == false)
            {
                // Update press anywhere text
                pressAnywhereText.text = "Successfully signed in" + '\n' + "Select a mode";

                // Enable mode buttons once signed in
                EnableModePanel();

                // Disable the press anywhere diamonds
                for (int i = 0; i < pressAnywhereDiamonds.Length; i++)
                {
                    pressAnywhereDiamonds[i].gameObject.SetActive(false);
                }

                // Set to true
                animationHasPlayed = true;
            }
        }
    }

    // Enable and play modeButtonPanel animation
    private void EnableModePanel()
    {
        // Activate panel
        modeButtonPanel.gameObject.SetActive(true);

        // Play animation
        PlayModePanelAnimation();

        modeButtonPanel.transform.position = new Vector3(0, -6, 0);
    }

    private void PlayModePanelAnimation()
    {
        // Play animation
        modeButtonPanelAnimator.Play("ModeButtonPanelAnimation");
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
