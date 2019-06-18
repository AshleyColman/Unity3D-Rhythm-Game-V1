using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailAndRetryManager : MonoBehaviour {

    LoadAndRunBeatmap loadAndRunBeatmap; // Controls running the beatmap
    SpecialTimeManager specialTimeManager;

    PlayerSkillsManager playerSkillsManager;

    private bool hasFailed; // Has the user failed
    private bool canFail; // Can the user fail
    private bool failScreenActivated; // Has the fail screen been activated
    public AudioSource audioSource; // The song audio source

    LevelChanger levelChanger; // Level changer

    public AudioSource menuSFXAudioSource; // Menu SFX Audio Source
    public AudioClip failSound; // Fail sound

    public Animator failedCanvasAnimator; // The failed canvas animator
    public GameObject failedCanvas; // The failed canvas gameobject

    private bool hasPressedRetryKey; // The user has pressed the retry key

    // Use this for initialization
    void Start () {
        // Get the reference to the level changer
        levelChanger = FindObjectOfType<LevelChanger>();
        // Get the reference to the player skills manager used for tracking the mods equiped that enable/disable failing
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        // Set has pressed the retry key to false at the start
        hasPressedRetryKey = false;
        // Set has failed to false by default
        hasFailed = false;
        // Set fail screen activated to false
        failScreenActivated = false;
        // Set can fail based on the mods equiped
        CheckModsEquiped();
	}
	
	// Update is called once per frame
	void Update () {

        // If the fail screen has been activated check for retry input
        if (failScreenActivated == true)
        {
            CheckUserInput();
        }
	}

    // Play the fail audio effect
    private void ChangeAudio()
    {
        audioSource.spatialBlend = 0.5f;
    }

    // Check no fail mod
    private void CheckModsEquiped()
    {
        // Check the player manager to see if the no fail mod has been selected
        if (playerSkillsManager.noFailSelected == true)
        {
            // Set canFail to false so the player cannot fail
            canFail = false;
        }
        else
        {
            // Set canFail to true so the player can fail
            canFail = true;
        }
    }

    // Return has pressed retry key
    public bool ReturnHasPressedRetryKey()
    {
        return hasPressedRetryKey;
    }

    // The user has failed
    public void HasFailed()
    {
        // If the user can fail
        if (canFail == true)
        {
            // Set hasFailed to true
            hasFailed = true;
            // Change the song audio effect
            ChangeAudio();
            // Change audio pitch
            ChangeSongSpeed();
            // Activate the failed screen 
            ActivateFailedScreen();
            // Play the fail sound
            PlayFailSound();
        }

    }

    // Check for user input during fail screen
    private void CheckUserInput()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Has pressed retry key to true
            hasPressedRetryKey = true;
            // Restart gameplay scene
            RestartGameplayScene();
        }
        */
    }

    // Activate the failed screen
    public void ActivateFailedScreen()
    {
        // Fail screen activated
        failScreenActivated = true;

        // Activate the fail canvas
        failedCanvas.gameObject.SetActive(true);

        // Play the animation
        failedCanvasAnimator.Play("FailedCanvasAnimation");
    }

    // Change song audio pitch
    private void ChangeSongSpeed()
    {
        audioSource.pitch = (0.75f);
    }

    // Play the fail sound
    private void PlayFailSound()
    {
        menuSFXAudioSource.PlayOneShot(failSound);
    }

    // Set has failed to true so the user has now failed
    public void SetHasFailedToTrue()
    {
        hasFailed = true;
    }

    // Return whether the user has failed
    public bool ReturnHasFailed()
    {
        return hasFailed;
    }

    // Relaod gameplay scene
    private void RestartGameplayScene()
    {
        SceneManager.LoadScene(levelChanger.gameplaySceneIndex);
    }
}
