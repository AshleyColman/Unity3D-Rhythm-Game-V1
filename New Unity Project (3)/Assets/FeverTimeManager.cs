using UnityEngine;
using UnityEngine.UI;

public class FeverTimeManager : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator feverSliderAnimator;

    // Audio
    public AudioReverbFilter audioReverbFilter;

    // Slider
    public Slider feverTimeSlider;

    // Float
    float tickDuration, measureDuration, feverSliderValueToLerpTo, feverSliderLerp, feverDuration2,
        feverDuration3, feverDuration4;

    // Bool 
    private bool feverActivated, lerpFeverSlider, canActivate, feverPhraseActive;

    // Scripts
    ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool FeverPhraseActive
    {
        get { return feverPhraseActive; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        // Initialize
        feverTimeSlider.value = 0f;
        feverSliderValueToLerpTo = 0f;
        feverActivated = false;
        lerpFeverSlider = false;
        canActivate = false;
        feverPhraseActive = true;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        if (canActivate == true)
        {
            if (Input.GetKeyDown(Constants.FEVER_ACTIVATION_KEY))
            {
                if (feverActivated == false)
                {
                    ActivateFever();
                }
            }
        }

        switch (feverActivated)
        {
            case true:
                switch (feverTimeSlider.value)
                {
                    case Constants.DEACTIVATE_FEVER_VALUE:
                        DeactivateFever();
                        break;
                    default:
                        LerpFeverSlider();
                        break;
                }
                break;
            case false:
                break;
        }
    }

    // Calculate measure duration
    public void CalculateMeasureDuration()
    { 
        // Get duration per tick
        tickDuration = (float)(scriptManager.metronomePro.songTickTimes[1] - scriptManager.metronomePro.songTickTimes[0]);
        // Get 1 measure duration by multiplying tick duration by 4
        measureDuration = (tickDuration * 4);
    }

    // Calculate how long fever will last once activated
    public void CalculateFeverDuration()
    {
        switch (feverTimeSlider.value)
        {
            case Constants.FEVER_FILL_2:
                feverDuration2 = (measureDuration * 2);
                break;
            case Constants.FEVER_FILL_3:
                feverDuration3 = (measureDuration * 3);
                break;
            case Constants.FEVER_FILL_4:
                feverDuration4 = (measureDuration * 4);
                break;
        }
    }

    // Add phrase to fever
    public void AddPhrase()
    {
        switch (feverTimeSlider.value)
        {
            case 0:
                feverTimeSlider.value = Constants.FEVER_FILL_1;
                break;
            case Constants.FEVER_FILL_1:
                feverTimeSlider.value = Constants.FEVER_FILL_2;
                break;
            case Constants.FEVER_FILL_2:
                feverTimeSlider.value = Constants.FEVER_FILL_3;
                break;
            case Constants.FEVER_FILL_3:
                feverTimeSlider.value = Constants.FEVER_FILL_4;
                break;
        }
    }

    // Break fever phrase
    public void BreakFeverPhrase()
    {
        feverActivated = false;
    }

    // Reset fever phrase
    public void ActivateFeverPhrase()
    {
        if (feverActivated == false)
        {
            feverActivated = true;
        }
    }

    // Lerp fever slider
    private void LerpFeverSlider()
    {
        feverSliderLerp += Time.deltaTime / Constants.SCORE_LERP_DURATION;
        float sliderValue = Mathf.Lerp(feverTimeSlider.value, feverSliderValueToLerpTo, feverSliderLerp);
        feverTimeSlider.value = sliderValue;
    }

    // Activate fever
    private void ActivateFever()
    {
        feverActivated = true;
        canActivate = false;
        lerpFeverSlider = true;
        feverSliderLerp = 0f;
        feverSliderAnimator.Play("FeverEffect1_Animation", 0, 0f);
        audioReverbFilter.enabled = true;
    }

    // Deactivate fever
    private void DeactivateFever()
    {
        // Reset animation to default
        feverSliderAnimator.gameObject.SetActive(false);
        feverSliderAnimator.gameObject.SetActive(true);
        audioReverbFilter.enabled = false;
        feverActivated = false;
    }

    // Fill fever slider
    public void FillFeverSlider()
    {
        if ((feverTimeSlider.value += Constants.PER_NOTE_FILL) <= 1f)
        {
            feverTimeSlider.value += Constants.PER_NOTE_FILL;
        }
    }

    // Decrease fever slider
    public void DecreaseFeverSlider()
    {
        feverTimeSlider.value -= Constants.PER_NOTE_FILL;
    }

    // Reset fever slider
    public void ResetFeverSlider()
    {
        feverTimeSlider.value = 0f;
    }
    #endregion
}
