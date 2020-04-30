using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Healthbar : MonoBehaviour
{
    #region Variables
    // Integer
    private const int PERFECT_HEALTH_VALUE = 10, GOOD_HEALTH_VALUE = 5, EARLY_HEALTH_VALUE = -5, MISS_HEALTH_VALUE = -10;

    // Text
    public TextMeshProUGUI healthText, noFailText;

    // Bool
    private bool failed;

    // Slider
    public Slider healthbarSlider, noFailSlider;

    // Image
    public Image healthbarSliderImage;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool Failed
    {
        get { return failed; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Initialize
        healthbarSlider.value = Constants.GREEN_HEALTH_VALUE;
        failed = false;

        // Functions
        UpdateUI();
    }

    // Update UI slider colors
    private void UpdateUI()
    {
        if (healthbarSlider.value <= 0)
        {
            // FAILED
            failed = true;
            // Display fail screen
        }
        else if (healthbarSlider.value > 0 && healthbarSlider.value <= Constants.RED_HEALTH_VALUE)
        {
            // RED
            if (healthbarSliderImage.color != scriptManager.uiColorManager.offlineColor08)
            {
                healthbarSliderImage.color = scriptManager.uiColorManager.offlineColor08;
            }
        }
        else if (healthbarSlider.value > Constants.RED_HEALTH_VALUE && healthbarSlider.value <= Constants.ORANGE_HEALTH_VALUE)
        {
            // ORANGE
            if (healthbarSliderImage.color != scriptManager.uiColorManager.orangeColor08)
            {
                healthbarSliderImage.color = scriptManager.uiColorManager.orangeColor08;
            }
        }
        else if (healthbarSlider.value > Constants.ORANGE_HEALTH_VALUE && healthbarSlider.value <= Constants.GREEN_HEALTH_VALUE)
        {
            // GREEN
            if (healthbarSliderImage.color != scriptManager.uiColorManager.onlineColor08)
            {
                healthbarSliderImage.color = scriptManager.uiColorManager.onlineColor08;
            }
        }

        healthText.text = Constants.HEALTH_STRING + healthbarSlider.value.ToString();
    }

    // Update health
    public void UpdateHealth(float _value)
    {
        if (scriptManager.rhythmVisualizatorPro.audioSource.time < Constants.NO_FAIL_TIMER_DURATION)
        {
            if (healthbarSlider.value <= Constants.RED_HEALTH_VALUE)
            {
                healthbarSlider.value = Constants.RED_HEALTH_VALUE;
                healthbarSlider.value = Constants.RED_HEALTH_VALUE;
            }
            else
            {
                healthbarSlider.value += _value;
                UpdateUI();
            }
        }
        else
        {
            healthbarSlider.value += _value;
            UpdateUI();
        }
    }

    // Play no fail countdown
    public IEnumerator PlayNoFailCountdown()
    {
        for (int i = 10; i > 0; i--)
        {
            noFailText.text = Constants.HEALTH_GUARD_STRING + i;
            yield return new WaitForSeconds(1f);
        }

        noFailSlider.gameObject.SetActive(false);
    }
    #endregion
}
