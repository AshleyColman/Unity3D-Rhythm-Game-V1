using UnityEngine;

public class ComboFlash : MonoBehaviour {

    // Animation
    public Animator comboFlashAnimator; // The combo flash animator
    public Animator glassFlashAnimator; // Glass combo flash animator

    // Bools
    private bool hasFlashed; // Used for making sure the combo only flashes once
    private bool previousComboFlashRight; // Was the previous flash from the right side?

    // Integers
    private float nextComboFlashCombo; // Combo required for combo flash

    // Scripts
    private ScoreManager scoreManager; // Required for getting the current combo


    void Start()
    {
        // Initialize
        previousComboFlashRight = false; // Set to false at the start so the first flash is on the right side
        hasFlashed = false;
        nextComboFlashCombo = 10f;

        // Set the reference
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Check whether the flash animation can be played based on the current combo
    public void CheckCanFlash()
    {
        // Check if the image has just flashed
        if (hasFlashed == true)
        {
            // Check if the combo required has been reached for a flash
            if (scoreManager.Combo == nextComboFlashCombo)
            {
                hasFlashed = false;
            }
        }

        // Check if the current combo is at the combo required for the flash, that we havent previously flashed and that we can flash
        if (scoreManager.Combo == nextComboFlashCombo && hasFlashed == false)
        {
            // Play flash animation
            FlashComboFlashImage();
            // Set the next combo required for next flash
            nextComboFlashCombo = (scoreManager.Combo + 10);
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
            glassFlashAnimator.Play("GameplayGlassFlash");
            previousComboFlashRight = true;
        }
        else if (previousComboFlashRight == true)
        {
            // Play the left side combo flash animation
            comboFlashAnimator.Play("ComboFlashAnimationLeft");
            glassFlashAnimator.Play("GameplayGlassFlash");
            previousComboFlashRight = false;
        }
    }
}
