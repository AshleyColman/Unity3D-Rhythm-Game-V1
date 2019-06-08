using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    public Slider healthBarSlider;
    public Image healthBarFill;
    public Animator healthBarAnimator;

    public float currentHealth;
    // The amount needed to change the bar to red
    float redBarAmount;
    // The amount needed to change the bar to yellow
    float yellowBarAmount;
    // The amount needed to change the bar to green
    float greenBarAmount; 
    public Color redBarColor;
    public Color yellowBarColor;
    public Color greenBarColor;
    public int healthBarValue;
    // Controls whether the user can fail or not
    private bool canFail;
    // Has the user failed
    private bool hasFailed;

    // Reference to the failAndRetryManager
    FailAndRetryManager failAndRetryManager;

    float lerpSpeed;
    public bool assignHealthBarLerp;
    float healthValueToLerpTo;

    // Use this for initialization
    void Start () {
        // Set hasfailed to false at the start
        hasFailed = false;
        // Reference to the failAndRetryManager
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();
        // Set can fail to true at the start 
        canFail = true;
        // The value passed from hit objects when hit or missed to increase or decrease the slider bar
        healthBarValue = 0;
        // Set it to 75 at the start of the game
        currentHealth = 75;
        // Update the slider to the current health
        healthBarSlider.value = currentHealth;

        // Set bar amount percentages
        redBarAmount = 33.33f;
        yellowBarAmount = 66.66f;
        greenBarAmount = 100f;

        // We set the health to 50 by default to prevent lerping at start of gameplay
        healthValueToLerpTo = 50;
        // Used to assign the new value to lerp to when an object has been hit
        assignHealthBarLerp = false;
        // The slider lerp speed when updating the length
        lerpSpeed = 20f;
    }

    // Update is called once per frame
    void Update () {

        // Check and update the color based on its current value
        SetHealthBarColor();

        // Check the failAndRetryManagers hasFailed bool
        hasFailed = failAndRetryManager.ReturnHasFailed();

        if (hasFailed == false)
        {
            // Check if the player has failed if they can fail
            CheckIfFailed();
        }
    }
    

    // Update health bar value
    public void UpdateHealthBarValue(int healthValuePass)
    {
        // Assign the health bar value to add to the current health bars value
        healthBarValue = healthValuePass; 

        // Update the health bar sliders value to be the new value
        healthBarSlider.value = (currentHealth + healthBarValue);

        // Assign the current health to the new slider value
        currentHealth = healthBarSlider.value;

        // Prevent bar from going below zero
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBarSlider.value = 0;
        }
        // Prevent bar from going above 100
        if (currentHealth >= 100)
        {
            currentHealth = 100;
            healthBarSlider.value = 100;
        }

        // Play healthbar animation
        PlayHealthBarAnimation();
    }

    // Play healthbar animation based on the current value
    private void PlayHealthBarAnimation()
    {
        // If the healthbar value is red zone
        if (currentHealth <= redBarAmount)
        {
            // Set the healthbar value to red
            healthBarFill.color = redBarColor;
            // Play red healthbar animation
            healthBarAnimator.Play("HealthBarRed");
        }
        // If the healthbar value is yellow zone
        else if (currentHealth > redBarAmount && currentHealth <= yellowBarAmount)
        {
            // Set the healthbar value to yellow
            healthBarFill.color = yellowBarColor;
            // Play yellow healthbar animation
            healthBarAnimator.Play("HealthBarYellow");
        }
        // If the healthbar value is green zone
        else if (currentHealth > yellowBarAmount && currentHealth <= greenBarAmount)
        {
            // Set the healthbar value to green
            healthBarFill.color = greenBarColor;
            // Play green healthbar animation
            healthBarAnimator.Play("HealthBarGreen");
        }
    }


    // Set healthbar color
    private void SetHealthBarColor()
    {
        // If the healthbar value is red zone
        if (currentHealth <= redBarAmount)
        {
            // Set the healthbar value to red
            healthBarFill.color = redBarColor;
        }
        // If the healthbar value is yellow zone
        else if (currentHealth > redBarAmount && currentHealth <= yellowBarAmount)
        {
            // Set the healthbar value to yellow
            healthBarFill.color = yellowBarColor;
        }
        // If the healthbar value is green zone
        else if (currentHealth > yellowBarAmount && currentHealth <= greenBarAmount)
        {
            // Set the healthbar value to green
            healthBarFill.color = greenBarColor;
        }
    }

    /*
    public void UpdateHealthBarValue(int healthBarValuePass)
    {
        healthValueToLerpTo = (currentHealth + healthBarValuePass);
        assignHealthBarLerp = false;

        // Error check for higher or lower values
        if (healthValueToLerpTo >= 100)
        {
            healthValueToLerpTo = 100;
        }
        if (healthValueToLerpTo <= 1)
        {
            healthValueToLerpTo = 1;
        }

        if (currentHealth > 0 && currentHealth < 100)
        {
            // Lerp the slider value to the current health, to the new health over time
            healthBarSlider.value = Mathf.Lerp(currentHealth, healthValueToLerpTo, Time.deltaTime * lerpSpeed);

            // Check if the player has failed if they can fail
            if (hasFailed == false)
            {
                CheckIfFailed();
            }

            // Assign the current health to the new slider value
            currentHealth = healthBarSlider.value;

            // Prevent bar from going below zero
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                healthBarSlider.value = 0;
            }
            // Prevent bar from going above 100
            if (currentHealth >= 100)
            {
                currentHealth = 100;
                healthBarSlider.value = 100;
            }
        }

        // Play healthbar animation
        PlayHealthBarAnimation();
    }
    */

    // Check if the player has failed and restart if they have
    private void CheckIfFailed()
    {
        // If the health is less than or equal to 1 
        if (currentHealth <= 0)
        {
            // Set has failed to true as the user has now failed
            hasFailed = true;
            
            // Activate failed screen and disable gameplay
            failAndRetryManager.HasFailed();
        }
    }
}
