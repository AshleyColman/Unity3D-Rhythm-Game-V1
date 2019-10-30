using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // UI
    public Slider healthBarSlider;
    public Image healthBarFill, healthBarHandle;

    // Integers
    private float redBarAmount; // The amount needed to change the bar to red
    private float yellowBarAmount; // The amount needed to change the bar to yellow
    private float greenBarAmount; // The amount needed to change the bar to green
    public float currentHealth; // Current health
    private int failHealthValue; // Value to cause failing
    private int maxHealthValue; // Max health value

    // Strings
    private string currentHealthColor; // The current health color of the healthbar
    private string redHealthTextValue, yellowHealthTextValue, greenHealthTextValue;

    // Bools
    private bool canFail; // Controls whether the user can fail or not

    // Colors
    public Color redBarColor, yellowBarColor, greenBarColor;

    // Scripts
    private FailAndRetryManager failAndRetryManager; // Reference to the failAndRetryManager

    // Properties

    // Get the current health color
    public string CurrentHealthColor
    {
        get { return currentHealthColor; }
    }



    // Use this for initialization
    void Start()
    {
        // Initialize
        canFail = true;
        currentHealth = 75;
        redBarAmount = 33.33f;
        yellowBarAmount = 66.66f;
        greenBarAmount = 100f;
        failHealthValue = 0;
        maxHealthValue = 100;
        redHealthTextValue = "RED";
        yellowHealthTextValue = "YELLOW";
        greenHealthTextValue = "GREEN";

        // Reference
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();

        // Functions
        UpdateHealthBarValue(0); // Update the healthbar value
    }

    // Update is called once per frame
    void Update()
    {
        if (failAndRetryManager.HasFailed == false)
        {
            // Check if the player has failed if they can fail
            CheckIfFailed();
        }
    }

    // Update the healthbar value
    public void UpdateHealthBarValue(float valuePass)
    {
        // Prevent value from going above 100 health
        if (currentHealth + valuePass > 100)
        {
            currentHealth = 100;
        }
        // Prevnet value from going below 0
        else if (currentHealth + valuePass < 0)
        {
            currentHealth = 0;
        }
        else
        {
            // Add/minus health 
            currentHealth = currentHealth + valuePass;
        }

        // Update healthbar value
        healthBarSlider.value = currentHealth / 100;
        // Check and update the color based on its current value
        UpdateHealthbarColor();
    }

    // Check the current health bar value and change the color if  below certain amounts
    private void UpdateHealthbarColor()
    {
        if (currentHealth <= redBarAmount)
        {
            healthBarFill.color = redBarColor;
            healthBarHandle.color = redBarColor;
            currentHealthColor = redHealthTextValue;
        }
        else if (currentHealth > redBarAmount && currentHealth <= yellowBarAmount)
        {
            healthBarFill.color = yellowBarColor;
            healthBarHandle.color = yellowBarColor;
            currentHealthColor = yellowHealthTextValue;
        }
        else if (currentHealth > yellowBarAmount && currentHealth <= greenBarAmount)
        {
            healthBarFill.color = greenBarColor;
            healthBarHandle.color = greenBarColor;
            currentHealthColor = greenHealthTextValue;
        }
    }


    // Check if the player has failed and restart if they have
    private void CheckIfFailed()
    {
        // If the health is less than or equal to 0 
        if (currentHealth <= failHealthValue)
        {
            // Activate failed screen and disable gameplay
            failAndRetryManager.PlayerHasFailed();
        }
    }
}