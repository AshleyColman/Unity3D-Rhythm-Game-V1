using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    public Slider healthBarSlider;
    public Image healthBarFill;
    public float currentHealth;
    float redBarAmount; // The amount needed to change the bar to red
    float yellowBarAmount; // The amount needed to change the bar to yellow
    float greenBarAmount; // The amount needed to change the bar to green
    public Color redBarColor;
    public Color yellowBarColor;
    public Color greenBarColor;
    public int healthBarValue;
    float lerpSpeed;
    public bool assignHealthBarLerp;
    float healthValueToLerpTo;

    // Use this for initialization
    void Start () {

        // We set the health to 75 by default to prevent lerping at start of gameplay
        healthValueToLerpTo = 75;
        // Used to assign the new value to lerp to when an object has been hit
        assignHealthBarLerp = false;
        // The value passed from hit objects when hit or missed to increase or decrease the slider bar
        healthBarValue = 0;
        // The slider lerp speed when updating the length
        lerpSpeed = 20f;
        // Set it to 75 at the start of the game
        currentHealth = 75;
        // Update the slider to the current health
        healthBarSlider.value = currentHealth;

        // Set bar amount percentages
        redBarAmount = 33.33f;
        yellowBarAmount = 66.66f;
        greenBarAmount = 100f;

    }
	
	// Update is called once per frame
	void Update () {

        // Check and update the color based on its current value
        UpdateHealthbarColor();

        UpdateHealthbar(healthBarValue, assignHealthBarLerp);
    }

    /*
    // Update the healthbar when an object has been hit
    public void UpdateHealthbar(int healthBarValuePass)
    {
        // healthBarValuePass is sent from when an object has been hit or missed
        //healthBarSlider.value = currentHealth + healthBarValuePass;
        Debug.Log("current health" + currentHealth);
        Debug.Log("curhealth + barpass" + (currentHealth + healthBarValuePass));
        healthBarSlider.value = Mathf.Lerp(currentHealth, (currentHealth + healthBarValuePass), Time.deltaTime);
        Debug.Log("current health afer" + currentHealth);
        Debug.Log("curhealth + barpass aftre" + (currentHealth + healthBarValuePass));
        // Set the current health to the healthbar slider value
        currentHealth = healthBarSlider.value;

        // Prevent bar from going below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
            healthBarSlider.value = 0;
        }
        // Prevent bar from going above 100
        if (currentHealth > 100)
        {
            currentHealth = 100;
            healthBarSlider.value = 100;
        }
    }
    */

    public void UpdateHealthbar(int healthBarValuePass, bool assignHealthbarLerpPass)
    {
        // We assign the value to lerp to when an object has been missed or hit only, prevents constant lerp values increasing
        if (assignHealthBarLerp == true)
        {
            healthValueToLerpTo = (currentHealth + healthBarValuePass);
            assignHealthBarLerp = false;
        }

        // Error check for higher or lower values
        if (healthValueToLerpTo >= 100)
        {
            healthValueToLerpTo = 100;
        }
        if (healthValueToLerpTo <= 0)
        {
            healthValueToLerpTo = 0;
        }

        // Lerp the slider value to the current health, to the new health over time
        healthBarSlider.value = Mathf.Lerp(currentHealth, healthValueToLerpTo, Time.deltaTime * lerpSpeed);

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

    // Check the current health bar value and change the color if  below certain amounts
    private void UpdateHealthbarColor()
    {
        if (currentHealth <= redBarAmount)
        {
            healthBarFill.color = redBarColor;
        }
        else if (currentHealth > redBarAmount && currentHealth <= yellowBarAmount)
        {
            healthBarFill.color = yellowBarColor;
        }
        else if (currentHealth > yellowBarAmount && currentHealth <= greenBarAmount)
        {
            healthBarFill.color = greenBarColor;
        }
    }
}
