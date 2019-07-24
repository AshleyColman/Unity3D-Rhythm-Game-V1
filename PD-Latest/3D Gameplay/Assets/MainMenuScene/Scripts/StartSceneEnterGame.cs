using UnityEngine;
using TMPro;

public class StartSceneEnterGame : MonoBehaviour {

    // Audio
    public AudioSource menuSFXAudioSource;
    public AudioClip menuSFXMenuSourceClip;

    // UI
    public TextMeshProUGUI pressAnywhereText;

    // Gameobjects
    public GameObject signupAndLoginPanel, pressAnywhereLeftDiamond, pressAnywhereRightDiamond;

    // Scripts
    private MetronomeForEffects metronomeForEffects;

    // Bools
    private bool hasClicked;

    // Strings
    private string loginTextValue;
    private string selectModeValue;

    // Use this for initialization
    void Start () {

        // Get reference
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();

        // Initialize
        menuSFXAudioSource.clip = menuSFXMenuSourceClip;
        hasClicked = false;
        loginTextValue = "Create a new account or sign into an existing one";
        selectModeValue = "Select a mode";
    }

    void Update()
    {
        // If user presses left click on title screen
        if (Input.GetMouseButtonDown(0) && hasClicked == false)
        {
            // Show the login panel
            ShowLoginPanel();

            // Set to true
            hasClicked = true;

            // Play sound effect
            menuSFXAudioSource.PlayOneShot(menuSFXMenuSourceClip);
        }
    }

    private void ShowLoginPanel()
    {
        // Update the text to show login instructions
        pressAnywhereText.text = loginTextValue;

        // Activate the signup and login panel
        signupAndLoginPanel.gameObject.SetActive(true);

        // Disable the press anywhere diamonds
        pressAnywhereLeftDiamond.gameObject.SetActive(false);
        pressAnywhereRightDiamond.gameObject.SetActive(false);
    }

    private void EnterGame()
    {
        // Play sound effect
        menuSFXAudioSource.Play();

        // Update press anywhere text
        pressAnywhereText.text = selectModeValue;
    }
}
