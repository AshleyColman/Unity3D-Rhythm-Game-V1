using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboFlash : MonoBehaviour {

    public Animator comboFlashAnimator; // The combo flash animator
    public ScoreManager scoreManager; // Required for getting the current combo
    private float currentCombo; // Current combo
    private float nextComboFlashCombo; // Combo required for combo flash
    private bool hasFlashed; // Used for making sure the combo only flashes once
    private bool hasFlashedPreviously;
    private bool canFlash;

    bool previousComboFlashRight; // Was the previous flash from the right side?

    void Start()
    {
        previousComboFlashRight = false; // Set to false at the start so the first flash is on the right side
        currentCombo = 0f;
        nextComboFlashCombo = 10f;
        hasFlashed = false;
        canFlash = true;

        // Set the reference
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current combo from the score manager
        currentCombo = scoreManager.combo;

        // Check if the image has just flashed
        if (hasFlashed == true)
        {
            // Check if the combo required has been reached for a flash
            if (currentCombo == nextComboFlashCombo)
            {
                // Set/Reset variables for the flash / Prevents the flash happening every frame whilst at the combo required
                canFlash = true;
                hasFlashed = false;
            }
        }

        // Check if the current combo is at the combo required for the flash, that we havent previously flashed and that we can flash
        if (currentCombo == nextComboFlashCombo && hasFlashed == false && canFlash == true)
        {
            // Play flash animation
            FlashComboFlashImage();
            // Set the next combo required for next flash
            nextComboFlashCombo = currentCombo += 10;
            // Set hasFlashed to true
            hasFlashed = true;
        }


    }

    // Animate the flash on screen
    public void FlashComboFlashImage()
    {
        // Do a left or right side flash by checking whether the last combo flash was the right side
        if (previousComboFlashRight == false)
        {
            // Play the right side combo flash animation
            comboFlashAnimator.Play("ComboFlashAnimation");
            previousComboFlashRight = true;
        }
        else if (previousComboFlashRight == true)
        {
            // Play the left side combo flash animation
            comboFlashAnimator.Play("ComboFlashAnimationLeft");
            previousComboFlashRight = false;
        }
    }
}
