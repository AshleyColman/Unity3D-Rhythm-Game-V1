using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSceneEnterGame : MonoBehaviour {

    public AudioSource MenuSFXAudioSource;
    public AudioClip MenuSFXMenuSourceClip;
    private MetronomeForEffects metronomeForEffects;

    public TextMeshProUGUI pressAnywhereText;

    public GameObject SignupAndLoginPanel;

    public GameObject[] pressAnywhereDiamonds = new GameObject[2];

    private bool hasClicked;

    // Use this for initialization
    void Start () {
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();
        hasClicked = false;
        MenuSFXAudioSource.clip = MenuSFXMenuSourceClip;
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
            MenuSFXAudioSource.PlayOneShot(MenuSFXMenuSourceClip);
        }
    }

    private void ShowLoginPanel()
    {
        // Show the login/sign up buttons
        pressAnywhereText.text = "Create a new account or sign into an existing one";

        // Activate the signup and login panel
        SignupAndLoginPanel.gameObject.SetActive(true);

        // Disable the press anywhere diamonds
        for (int i = 0; i < pressAnywhereDiamonds.Length; i++)
        {
            pressAnywhereDiamonds[i].gameObject.SetActive(false);
        }
    }

    private void EnterGame()
    {
        // Play sound effect
        MenuSFXAudioSource.Play();

        // Update press anywhere text
        pressAnywhereText.text = "Select a mode";

    }

    private void EnableModeButtons()
    {

    }
}
