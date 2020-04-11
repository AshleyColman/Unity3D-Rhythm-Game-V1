using UnityEngine;
using TMPro;

public class PlayInformation : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator comboTextAnimator, gradeTextAnimator;

    // UI
    public TextMeshProUGUI scoreText, zeroScoreText, comboText, gradeText, gradeShadowText, feverScoreText, percentageText;

    // Integers
    private int highestCombo, totalPerfect, totalGood, totalEarly, totalMiss, currentCombo, currentScore, totalScorePossible,
        totalHit, comboBreakValue, currentMultiplier, scoreStringLength;

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
        currentMultiplier = 0;
        currentPercentage = 100;
        comboBreakValue = 5;
        scoreStringLength = 0;
        scoreString = "";    
        
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        CalculateHighestScoreForBeatmap();
        UpdateComboText();
        CalculatePercentage();
        UpdatePercentageText();
    }

    // Reset combo
    public void ResetCombo()
    {
        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check if combo is greater than comboBreakValue
        if (currentCombo >= comboBreakValue)
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

    // Add combo
    public void AddCombo()
    {
        // Add to the existing combo
        currentCombo++;

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Update combo text
        UpdateComboText();

        // Update percentage
        CalculatePercentage();
        UpdatePercentageText();
    }

    // Add early judgement
    public void AddEarlyJudgement()
    {
        totalEarly++;
    }

    // Add good judgement
    public void AddGoodJudgement()
    {
        totalGood++;
    }

    // Add perfect judgement
    public void AddPerfectJudgement()
    {
        totalPerfect++;
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
