using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayInformation : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator comboTextAnimator, gradeTextAnimator, multiplierAnimator;
    private Animator[] plusScoreTextAnimatorArr;

    // Slider
    public Slider multiplierSlider;

    public Image multiplierSliderImage;

    // UI
    public TextMeshProUGUI scoreText, zeroScoreText, comboText, comboShadowText, gradeText, gradeShadowText,
        feverScoreText, percentageText, multiplierText;
    public TextMeshProUGUI[] plusScoreTextArr;

    // Integers
    private int highestCombo, totalPerfect, totalGood, totalEarly, totalMiss, currentCombo, currentScore, totalScorePossible,
        totalHit, currentMultiplier, scoreStringLength, plusScoreIndex;
    private float currentPercentage;

    // Strings
    private string gradeAchieved, scoreString;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    void Start()
    {
        // Initialize
        highestCombo = 0;
        currentCombo = 0;
        currentScore = 0;
        plusScoreIndex = 0;
        currentMultiplier = 0;
        currentPercentage = 100;
        scoreStringLength = 0;
        scoreString = "";
        multiplierSlider.value = 0f;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        CalculateHighestScoreForBeatmap();
        UpdateComboText();
        CalculatePercentage();
        UpdatePercentageText();
        ReferencePlusScoreTextAnimatorArr();

        multiplierSliderImage.color = scriptManager.uiColorManager.offlineColorSolid;
    }

    // Get all references to plus score text animators
    private void ReferencePlusScoreTextAnimatorArr()
    {
        plusScoreTextAnimatorArr = new Animator[plusScoreTextArr.Length];

        for (int i = 0; i < plusScoreTextArr.Length; i++)
        {
            plusScoreTextAnimatorArr[i] = plusScoreTextArr[i].GetComponent<Animator>();
        }
    }

    // Play the next plus score text animation
    private void PlayPlusScoreTextAnimation(Color _color, string _score)
    {
        if (plusScoreIndex == plusScoreTextAnimatorArr.Length)
        {
            plusScoreIndex = 0;
        }

        plusScoreTextArr[plusScoreIndex].text = Constants.PLUS_PREFIX + _score;
        plusScoreTextArr[plusScoreIndex].color = _color;
        plusScoreTextArr[plusScoreIndex].transform.SetAsLastSibling();
        plusScoreTextAnimatorArr[plusScoreIndex].Play("PlusScoreText_Animation", 0, 0f);

        plusScoreIndex++;
    }

    // Increase multiplier
    private void IncreaseMultiplier()
    {
        currentMultiplier++;
        multiplierSlider.value += Constants.MULTIPLIER_PER_NOTE_SLIDER_VALUE;

        switch (multiplierText.text)
        {
            case Constants.MULTIPLIER_1X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_2X)
                {
                    multiplierText.text = Constants.MULTIPLIER_2X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.eRankColor;
                    multiplierSliderImage.color = scriptManager.uiColorManager.eRankColor;
                }
                break;
            case Constants.MULTIPLIER_2X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_3X)
                {
                    multiplierText.text = Constants.MULTIPLIER_3X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.cRankColor;
                    multiplierSliderImage.color = scriptManager.uiColorManager.cRankColor;
                }
                break;
            case Constants.MULTIPLIER_3X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_4X)
                {
                    multiplierText.text = Constants.MULTIPLIER_4X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.dRankColor;
                    multiplierSliderImage.color = scriptManager.uiColorManager.dRankColor;

                    switch (currentCombo)
                    {
                        case Constants.MULTIPLIER_COMBO_4X:
                            break;
                    }
                }
                break;
            case Constants.MULTIPLIER_4X_STRING:
                multiplierSlider.value = 1f;
                break;
        }
    }

    // Reset multiplier
    private void ResetMultiplier()
    {
        if (currentCombo >= Constants.MULTIPLIER_COMBO_2X)
        {
            currentMultiplier = Constants.DEFAULT_MULTIPLIER;
            multiplierText.text = Constants.MULTIPLIER_1X_STRING;
            multiplierText.color = scriptManager.uiColorManager.offlineColorSolid;
            multiplierSliderImage.color = scriptManager.uiColorManager.offlineColorSolid;
            multiplierAnimator.Play("Multiplier_Reset_Animation", 0, 0f);
        }

        multiplierSlider.value = 0f;
    }

    // Add combo
    public void AddCombo()
    {
        // Add to the existing combo
        currentCombo++;

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Increase multiplier
        IncreaseMultiplier();

        // Update combo text
        UpdateComboText();

        // Update percentage
        CalculatePercentage();
        UpdatePercentageText();
    }

    // Reset combo
    public void ResetCombo()
    {
        // Reset multiplier
        ResetMultiplier();

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check if combo is greater than comboBreakValue
        if (currentCombo >= Constants.COMBO_BREAK)
        {
            // Play miss sound
            scriptManager.hitSoundManager.PlayMissSound();
        }

        // Play combo break animation
        comboTextAnimator.Play("ComboBreak_Animation", 0, 0f);

        // Reset combo
        currentCombo = 0;

        // Update current combo text
        UpdateComboText();

        // Update percentage
        CalculatePercentage();
        UpdatePercentageText();
    }

    // Update combo text
    private void UpdateComboText()
    {
        comboText.text = currentCombo.ToString() + Constants.COMBO_PREFIX;
        comboShadowText.text = comboText.text;
    }

    // Add score and update score text
    public void AddScore(int _scoreValue)
    {
        // Increment score
        currentScore += _scoreValue;

        // Add 0's
        if (currentScore == 0)
        {
            scoreText.text = Constants.ZERO_SCORE_PREFIX_1;
            zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_7;
        }

        // Get score as string
        scoreString = currentScore.ToString();

        // Get length of score string
        scoreStringLength = scoreString.Length;

        // Based on the length update the score text
        switch (scoreStringLength)
        {
            case Constants.STRING_LENGTH_1:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_7;
                break;
            case Constants.STRING_LENGTH_2:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_6;
                break;
            case Constants.STRING_LENGTH_3:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_5;
                break;
            case Constants.STRING_LENGTH_4:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_4;
                break;
            case Constants.STRING_LENGTH_5:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_3;
                break;
            case Constants.STRING_LENGTH_6:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_2;
                break;
            case Constants.STRING_LENGTH_7:
                zeroScoreText.text = Constants.ZERO_SCORE_PREFIX_1;
                break;
            case Constants.STRING_LENGTH_8:
                zeroScoreText.text = "";
                break;
        }

        // Set score text
        scoreText.text = scoreString;
    }

    // Add early judgement
    public void AddEarlyJudgement()
    {
        totalEarly++;

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.normalDifficultyColor, Constants.EARLY_SCORE_STRING);
    }

    // Add good judgement
    public void AddGoodJudgement()
    {
        totalGood++;

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.easyDifficultyColor, Constants.GOOD_SCORE_STRING);
    }

    // Add perfect judgement
    public void AddPerfectJudgement()
    {
        totalPerfect++;

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.selectedColor, Constants.PERFECT_SCORE_STRING);
    }

    // Add miss judgement
    public void AddMissJudgement()
    {
        totalMiss++;
    }

    // Check if the current combo is the highest combo so far
    public void CheckHighestCombo()
    {
        if (currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }
    }

    // Check the highest score possible by calculating the total notes in the song x perfect judgement
    public void CalculateHighestScoreForBeatmap()
    {
        totalScorePossible = Database.database.LoadedPositionX.Count * Constants.PERFECT_SCORE_VALUE;
    }

    // Calculate percentage
    private void CalculatePercentage()
    {
        currentPercentage = (currentScore / totalScorePossible) * 100;

        // Check if current grade has increased
        string returnedGradeAchieved = scriptManager.gradeManager.CheckGradeAchieved(gradeAchieved, currentPercentage);

        // Update grade if it has changed
        if (returnedGradeAchieved != gradeAchieved)
        {
            gradeAchieved = returnedGradeAchieved;
            UpdateGradeAchievedText();
            gradeTextAnimator.Play("NewGradeAchieved_Animation", 0, 0f);
        }

        /*
        // NEED TO TRACK DEFAULT SCORE SEPERATELY FROM ACTUAL SCORE WITH SP FOR ACCURATE %
        currentPercentage = (currentScore / totalScorePossibleNow) * 100;
        int totalScorePossibleNow = scriptManager.loadAndRunBeatmap.totalspawnedandhit ?;
        */
    }

    // Update percentage text
    private void UpdatePercentageText()
    {
        percentageText.text = currentPercentage.ToString("F2") + Constants.PERCENTAGE_PREFIX;
    }

    // Update grade achieved text
    private void UpdateGradeAchievedText()
    {
        gradeText.text = gradeAchieved;
        gradeShadowText.text = gradeText.text;
        gradeText.colorGradientPreset = scriptManager.uiColorManager.SetGradeColorGradient(gradeAchieved);
    }
    #endregion
}
