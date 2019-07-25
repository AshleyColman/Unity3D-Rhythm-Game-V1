using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterGameScene : MonoBehaviour {

    // UI
    public TextMeshProUGUI pressAnywhereText, usernameDisplayText;
    public Button loginCanvas, registerCanvas, enterGameCanvas, loggedInCanvas; // Login and sign up canvas objects

    // Animation
    public Animator modeButtonPanelAnimator; // Animator for the mode buttons

    // Gameobjects
    public GameObject modeButtonPanel; // Mode button panel gameobject
    public GameObject pressAnywhereLeftDiamond, pressAnywhereRightDiamond; // Press anywhere diamond gameobjects

    // Bools
    private bool modeButtonAnimationHasPlayed; // Controls the mode button animation
    private bool hasLoggedIn; // Has the user logged in

    // Strings
    private string successfullySignedInValue = "Successfully signed in" + '\n' + "Select a mode";
   
    private void Start()
    {
        // Initialize
        modeButtonAnimationHasPlayed = false;
        hasLoggedIn = false;

        // Check if the user has logged in
        CheckIfLoggedIn();
    }

    // Check if the user has logged in
    public void CheckIfLoggedIn()
    {
        // If the user has not logged in yet
        if (hasLoggedIn == false)
        {
            // Check if the user has logged in
            if (MySQLDBManager.loggedIn == true)
            {
                // Update the username logged in playing as text
                usernameDisplayText.text = "Playing as " + MySQLDBManager.username;

                // Disable the login and enter game canvas
                loggedInCanvas.gameObject.SetActive(true);
                enterGameCanvas.gameObject.SetActive(false);

                // If the mode button animation has not played yet
                if (modeButtonAnimationHasPlayed == false)
                {
                    // Update press anywhere text to successfully signed in
                    pressAnywhereText.text = successfullySignedInValue;

                    // Enable mode buttons once signed in
                    EnableModePanel();

                    // Disable the press anywhere diamonds
                    pressAnywhereLeftDiamond.gameObject.SetActive(false);
                    pressAnywhereRightDiamond.gameObject.SetActive(false);
                }
                else
                {
                    // Update the mode panel position so that it stays on screen after the animation
                    modeButtonPanel.transform.position = new Vector3(0, -6, 0);
                }

                // Set to true as the user has now logged in
                hasLoggedIn = true;
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
    }

    // Play the mode panel animation
    private void PlayModePanelAnimation()
    {
        // Play animation
        modeButtonPanelAnimator.Play("ModeButtonPanelAnimation");

        // Set to true as the animation has played
        modeButtonAnimationHasPlayed = true;
    }

    // Go to the register canvas
    public void GoToRegister()
    {
        // Disable the register canvas
        registerCanvas.gameObject.SetActive(true);
        // Disable enter game canvas
        enterGameCanvas.gameObject.SetActive(false);
    }

    // Go to the login canvas
    public void GoToLogin()
    {
        // Disable the login canvas
        loginCanvas.gameObject.SetActive(true);
        // Disable enter game canvas
        enterGameCanvas.gameObject.SetActive(false);
    }
}
