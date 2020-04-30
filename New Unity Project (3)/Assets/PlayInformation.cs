using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayInformation : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator comboTextAnimator, multiplierAnimator, gradeTextAnimator;
    private Animator[] plusScoreTextAnimatorArr, plusAccuracyTextAnimatorArr;

    // UI
    public TextMeshProUGUI scoreText, zeroScoreText, comboText, comboShadowText, gradeText, gradeShadowText,
        feverScoreText, percentageText, multiplierText, totalPerfectText, totalGoodText, totalEarlyText, totalMissText,
        streakText, followJudgementText, followComboText;
    public TextMeshProUGUI[] plusScoreTextArr, plusAccuracyTextArr;

    // Gameobject
    public GameObject followInfo;

    // Integers
    private int highestCombo, totalPerfect, totalGood, totalEarly, totalMiss, currentCombo, currentScore, totalHit,
        currentMultiplier, scoreStringLength, plusScoreIndex, nextStreak, plusAccuracyIndex;
    private float currentPercentage, scoreToLerpTo, scoreLerp, percentageCalculationScore, totalScorePossible,
        prevFramePercentage;

    // Strings
    private string gradeAchieved, scoreString;

    // Bool
    private bool lerpScore;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool LerpScore
    {
        set { lerpScore = value; }
    }
    #endregion

    #region Functions
    void Start()
    {
        // Initialize
        highestCombo = 0;
        currentCombo = 0;
        currentScore = 0;
        scoreToLerpTo = 0;
        plusScoreIndex = 0;
        plusAccuracyIndex = 0;
        percentageCalculationScore = 0;
        scoreLerp = 0;
        currentMultiplier = 0;
        currentPercentage = 100;
        prevFramePercentage = 100;
        scoreStringLength = 0;
        nextStreak = Constants.STREAK_INTERVAL;
        scoreString = "";
        //gradeAchieved = Constants.Grade
        lerpScore = false;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        CalculateHighestScoreForBeatmap();
        UpdateComboText();
        CalculatePercentage();
        ReferencePlusScoreTextAnimatorArr();
        ReferencePlusAccuracyTextAnimatorArr();
    }

    private void Update()
    {
        switch (lerpScore)
        {
            case true:
                LerpToScore();
                break;
        }
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

    // Get all references to plus accuracy text animators
    private void ReferencePlusAccuracyTextAnimatorArr()
    {
        plusAccuracyTextAnimatorArr = new Animator[plusAccuracyTextArr.Length];

        for (int i = 0; i < plusAccuracyTextArr.Length; i++)
        {
            plusAccuracyTextAnimatorArr[i] = plusAccuracyTextArr[i].GetComponent<Animator>();
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

    // Play the next plus accuracy text animation
    private void PlayPlusAccuracyTextAnimation(Color _color, string _accuracy)
    {
        if (plusAccuracyIndex == plusAccuracyTextAnimatorArr.Length)
        {
            plusAccuracyIndex = 0;
        }

        plusAccuracyTextArr[plusAccuracyIndex].text = _accuracy;
        plusAccuracyTextArr[plusAccuracyIndex].color = _color;
        plusAccuracyTextArr[plusAccuracyIndex].transform.SetAsLastSibling();
        plusAccuracyTextAnimatorArr[plusAccuracyIndex].Play("PlusScoreText_Animation", 0, 0f);

        plusAccuracyIndex++;
    }


    // Increase multiplier
    private void IncreaseMultiplier()
    {
        currentMultiplier++;

        switch (multiplierText.text)
        {
            case Constants.MULTIPLIER_1X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_2X)
                {
                    multiplierText.text = Constants.MULTIPLIER_2X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.eRankColor;
                }
                break;
            case Constants.MULTIPLIER_2X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_3X)
                {
                    multiplierText.text = Constants.MULTIPLIER_3X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.cRankColor;
                }
                break;
            case Constants.MULTIPLIER_3X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_4X)
                {
                    multiplierText.text = Constants.MULTIPLIER_4X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.dRankColor;

                    switch (currentCombo)
                    {
                        case Constants.MULTIPLIER_COMBO_4X:
                            break;
                    }
                }
                break;
            case Constants.MULTIPLIER_4X_STRING:

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
            multiplierAnimator.Play("Multiplier_Reset_Animation", 0, 0f);
        }
    }

    // Add combo
    public void AddCombo()
    {
        // Add to the existing combo
        currentCombo++;

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check streak
        CheckStreak();

        // Increase multiplier
        IncreaseMultiplier();

        // Update combo text
        UpdateComboText();

        // Update percentage
        CalculatePercentage();
    }

    // Reset combo
    public void ResetCombo()
    {
        // Reset multiplier
        ResetMultiplier();

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check streak
        CheckStreak();

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
    }

    // Check the current streak
    private void CheckStreak()
    {
        if (currentCombo >= nextStreak)
        {
            streakText.text = nextStreak + Constants.STREAK_PREFIX;
            nextStreak += Constants.STREAK_INTERVAL;
            streakText.gameObject.SetActive(false);
            streakText.gameObject.SetActive(true);
        }
    }

    // Reset streak
    private void ResetStreak()
    {
        nextStreak = Constants.STREAK_INTERVAL;
    }

    // Update combo text
    private void UpdateComboText()
    {
        comboText.text = currentCombo.ToString() + Constants.COMBO_PREFIX;
        comboShadowText.text = comboText.text;
        followComboText.text = comboText.text;
    }

    // Add score and update score text
    public void AddScore(int _scoreValue)
    {
        scoreToLerpTo = (currentScore + _scoreValue);
        scoreLerp = 0f;
        lerpScore = true;

        CalculatePercentage();
    }

    // Lerp to the score
    private void LerpToScore()
    {
        scoreLerp += Time.deltaTime / Constants.SCORE_LERP_DURATION;
        int score = (int)Mathf.Lerp(currentScore, scoreToLerpTo, scoreLerp);

        currentScore = score;

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

        scoreText.text = currentScore.ToString();

        if (currentScore >= scoreToLerpTo)
        {
            lerpScore = false;
        }
    }

    // Add early judgement
    public void AddEarlyJudgement()
    {
        totalEarly++;

        // Take away the difference between early and perfect to update the percentage
        percentageCalculationScore -= (Constants.PERFECT_SCORE_VALUE - Constants.EARLY_SCORE_VALUE);

        // Increment text
        totalEarlyText.text = Constants.TOTAL_EARLY_PREFIX + totalEarly.ToString();

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.normalDifficultyColor, Constants.EARLY_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.EARLY_JUDGEMENT, scriptManager.uiColorManager.cRankColor);
    }

    // Add good judgement
    public void AddGoodJudgement()
    {
        totalGood++;

        // Take away the difference between good and perfect to update the percentage
        percentageCalculationScore -= (Constants.PERFECT_SCORE_VALUE - Constants.GOOD_SCORE_VALUE);

        // Increment text
        totalGoodText.text = Constants.TOTAL_GOOD_PREFIX + totalGood.ToString();

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.easyDifficultyColor, Constants.GOOD_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.GOOD_JUDGEMENT, scriptManager.uiColorManager.easyDifficultyColor);
    }

    // Add perfect judgement
    public void AddPerfectJudgement()
    {
        totalPerfect++;

        // Increment text
        totalPerfectText.text = Constants.TOTAL_PERFECT_PREFIX + totalPerfect.ToString();

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.selectedColor, Constants.PERFECT_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.PERFECT_JUDGEMENT, scriptManager.uiColorManager.selectedColor);
    }

    // Add miss judgement
    public void AddMissJudgement()
    {
        totalMiss++;

        // Take away perfect score difference
        percentageCalculationScore -= Constants.PERFECT_SCORE_VALUE;

        // Increment text
        totalMissText.text = Constants.TOTAL_MISS_PREFIX + totalMiss.ToString();

        // Hide follow info
        HideFollowInfo();
    }

    // Check if the current combo is the highest combo so far
    public void CheckHighestCombo()
    {
        if (currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }
    }

    // Display judgement text
    private void UpdateJudgementText(string _judgementString, Color _color)
    {
        followJudgementText.text = _judgementString;
        followJudgementText.color = _color;
    }

    // Display follow info
    public void DisplayFollowInfo(Vector3 _position)
    {
        followInfo.transform.position = _position;
        HideFollowInfo();
        followInfo.gameObject.SetActive(true);
    }

    // Hide follow info
    public void HideFollowInfo()
    {
        followInfo.gameObject.SetActive(false);
    }

    // Check the highest score possible by calculating the total notes in the song x perfect judgement
    public void CalculateHighestScoreForBeatmap()
    {
        totalScorePossible = Database.database.LoadedPositionX.Count * Constants.PERFECT_SCORE_VALUE;
        percentageCalculationScore = totalScorePossible;
    }

    // Calculate percentage
    private void CalculatePercentage()
    {
        currentPercentage = (percentageCalculationScore / totalScorePossible) * 100;

        // Check if current grade has increased
        string returnedGradeAchieved = scriptManager.gradeManager.CalculateGrade(currentPercentage);

        // Update grade if it has changed
        if (returnedGradeAchieved != gradeAchieved)
        {
            gradeAchieved = returnedGradeAchieved;
            UpdateGradeAchievedText();
        }

        // Update percentage text
        percentageText.text = currentPercentage.ToString("F2") + Constants.PERCENTAGE_PREFIX;

        // Check previous frame percentage with new percentage
        if (prevFramePercentage != currentPercentage)
        {
            string accuracyString = (prevFramePercentage - currentPercentage).ToString("F2");

            if (prevFramePercentage > currentPercentage)
            {
                PlayPlusAccuracyTextAnimation(scriptManager.uiColorManager.offlineColorSolid,
                    (Constants.NEGATIVE_PREFIX + accuracyString));
            }
        }

        // Update previous frame percentage
        prevFramePercentage = currentPercentage;

        //currentPercentage = (currentScore / totalScorePossible) * 100;
    }

    // Update grade achieved text
    private void UpdateGradeAchievedText()
    {
        gradeText.text = gradeAchieved;
        gradeShadowText.text = gradeAchieved;
        gradeText.colorGradientPreset = scriptManager.uiColorManager.SetGradeColorGradient(gradeAchieved);
        gradeTextAnimator.Play("Grade_Achieved_Animation", 0, 0f);
    }
    #endregion
}
