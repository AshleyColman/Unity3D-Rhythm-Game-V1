using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeverTimeManager : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI remainingPhraseNoteCountText;

    // Gameobject
    public GameObject feverSuccessPanel, feverFailedPanel, bonusFeverScorePanel;

    // Animator
    public Animator feverSliderAnimator, flashGlassAnimator;

    // Audio
    public AudioReverbFilter audioReverbFilter;

    // Slider
    public Slider feverTimeSlider;

    // Int 
    public int currentPhraseCount;

    // Float
    public float tickDuration, measureDuration, feverSliderValueToLerpTo, feverSliderLerp, feverDuration1, feverDuration2,
        feverDuration3, feverDuration4;

    // Bool 
    private bool feverActivated, canActivate;

    // Scripts
    ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool FeverActivated
    {
        get { return feverActivated; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        // Initialize
        feverTimeSlider.value = 0f;
        feverSliderValueToLerpTo = 0f;
        currentPhraseCount = 0;
        feverActivated = false;
        canActivate = false;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        if (canActivate == true)
        {
            if (Input.GetKeyDown(Constants.FEVER_ACTIVATION_KEYCODE))
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

    // Update all active hit objects to fever appearance
    private void UpdateHitObjectsToFever()
    {
        for (int i = 0; i < scriptManager.loadAndRunBeatmap.activeList.Count; i++)
        {
            scriptManager.loadAndRunBeatmap.activeList[i].AssignFeverColors();
        }
    }

    // Update all active hit objects to normal appearance
    private void UpdateHitObjectsToNormal()
    {
        for (int i = 0; i < scriptManager.loadAndRunBeatmap.activeList.Count; i++)
        {
            scriptManager.loadAndRunBeatmap.activeList[i].AssignNormalColors();
        }
    }

    // Update the remaining phrase note count text
    public void UpdateRemainingPhraseNoteCountText(int _count)
    {
        remainingPhraseNoteCountText.text = Constants.FEVER_NOTES_REMAINING_STRING + _count.ToString();
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
        feverDuration1 = (measureDuration * 2);
        feverDuration2 = (measureDuration * 4);
        feverDuration3 = (measureDuration * 6);
        feverDuration4 = (measureDuration * 8);
    }

    // Add phrase to fever
    public void AddPhrase()
    {
        // Increase total fever phrases hit count
        scriptManager.playInformation.IncreaseTotalFeverPhrasesHit();

        // Play fever success
        switch (feverSuccessPanel.gameObject.activeSelf)
        {
            case false:
                feverSuccessPanel.gameObject.SetActive(true);
                break;
            case true:
                feverSuccessPanel.gameObject.SetActive(false);
                feverSuccessPanel.gameObject.SetActive(true);
                break;
        }

        scriptManager.feverTimeManager.remainingPhraseNoteCountText.gameObject.SetActive(false);

        if (feverActivated == false)
        {
            if (feverTimeSlider.value != Constants.FEVER_FILL_4)
            {
                flashGlassAnimator.Play("FlashGlass_Animation", 0, 0f);

                switch (currentPhraseCount)
                {
                    case 0:
                        feverTimeSlider.value = Constants.FEVER_FILL_1;
                        currentPhraseCount = 1;
                        break;
                    case 1:
                        feverTimeSlider.value = Constants.FEVER_FILL_2;
                        currentPhraseCount = 2;
                        break;
                    case 2:
                        feverTimeSlider.value = Constants.FEVER_FILL_3;
                        currentPhraseCount = 3;
                        break;
                    case 3:
                        feverTimeSlider.value = Constants.FEVER_FILL_4;
                        currentPhraseCount = 4;
                        break;
                }
            }

            canActivate = true;
        }
    }

    // Lerp fever slider
    private void LerpFeverSlider()
    {
        //float feverSliderLerpPercentage = feverSliderLerp / 10f;
        switch (currentPhraseCount)
        {
            case 1:
                feverSliderLerp += Time.deltaTime / feverDuration1;
                feverTimeSlider.value = Mathf.Lerp(Constants.FEVER_FILL_1, feverSliderValueToLerpTo, feverSliderLerp);
                break;
            case 2:
                feverSliderLerp += Time.deltaTime / feverDuration2;
                feverTimeSlider.value = Mathf.Lerp(Constants.FEVER_FILL_2, feverSliderValueToLerpTo, feverSliderLerp);
                break;
            case 3:
                feverSliderLerp += Time.deltaTime / feverDuration3;
                feverTimeSlider.value = Mathf.Lerp(Constants.FEVER_FILL_3, feverSliderValueToLerpTo, feverSliderLerp);
                break;
            case 4:
                feverSliderLerp += Time.deltaTime / feverDuration4;
                feverTimeSlider.value = Mathf.Lerp(Constants.FEVER_FILL_4, feverSliderValueToLerpTo, feverSliderLerp);
                break;
        }
    }

    // Activate fever
    private void ActivateFever()
    {
        UpdateHitObjectsToFever();
        feverActivated = true;
        audioReverbFilter.enabled = true;
        bonusFeverScorePanel.gameObject.SetActive(true);
        canActivate = false;
        feverSliderLerp = 0f;
        feverSliderAnimator.Play("FeverEffect1_Animation", 0, 0f);
        flashGlassAnimator.Play("FlashGlass_Animation", 0, 0f);
    }

    // Deactivate fever
    private void DeactivateFever()
    {
        UpdateHitObjectsToNormal();
        // Reset animation to default
        feverSliderAnimator.gameObject.SetActive(false);
        feverSliderAnimator.gameObject.SetActive(true);
        bonusFeverScorePanel.gameObject.SetActive(false);
        scriptManager.playInformation.ResetActiveFeverBonusScore();
        audioReverbFilter.enabled = false;
        feverActivated = false;
        currentPhraseCount = 0;
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
