using UnityEngine;
using UnityEngine.UI;

public class FeverTimeManager : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator feverSliderAnimator;

    // Slider
    public Slider feverTimeSlider;

    // Float
    private const float FEVER_FILL_1 = 0.25f, FEVER_FILL_2 = 0.5f, FEVER_FILL_3 = 0.75f, FEVER_FILL_4 = 1.0f,
        PER_NOTE_FILL = 0.01f, DEACTIVATE_FEVER_VALUE = 0.0f;

    // Bool 
    private bool feverActivated;

    // Keycode
    private const KeyCode FEVER_ACTIVATION_KEY = KeyCode.LeftShift;
    #endregion

    #region Functions
    private void Start()
    {
        feverTimeSlider.value = 0f;
        feverActivated = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(FEVER_ACTIVATION_KEY))
        {
            if (feverActivated == false)
            {
                ActivateFever();
            }
        }
    
        switch (feverActivated)
        {
            case true:
                if (feverTimeSlider.value == DEACTIVATE_FEVER_VALUE)
                {
                    DeactivateFever();
                }

                // Drain fever value for testing
                feverTimeSlider.value -= PER_NOTE_FILL;
                break;
            case false:
                break;
        }
    }

    private void ActivateFever()
    {
        feverActivated = true;
        feverSliderAnimator.Play("FeverEffect1_Animation", 0, 0f);
    }

    private void DeactivateFever()
    {
        // Reset animation to default
        feverSliderAnimator.gameObject.SetActive(false);
        feverSliderAnimator.gameObject.SetActive(true);
        feverActivated = false;
    }

    public void FillFeverSlider()
    {
        if ((feverTimeSlider.value += PER_NOTE_FILL) <= 1f)
        {
            feverTimeSlider.value += PER_NOTE_FILL;
        }
    }

    public void DecreaseFeverSlider()
    {
        feverTimeSlider.value -= PER_NOTE_FILL;
    }

    public void ResetFeverSlider()
    {
        feverTimeSlider.value = 0f;
    }
    #endregion
}
