using UnityEngine;
using TMPro;

public class PlayInformation : MonoBehaviour
{
    #region Variables
    // Animator
    public Animator comboTextAnimator, multiplierAnimator, cornerMultiplierAnimator;
    private Animator[] plusScoreTextAnimatorArr, plusAccuracyTextAnimatorArr;

    // UI
    public TextMeshProUGUI scoreText, zeroScoreText, comboText,
        percentageText, multiplierText, cornerMultiplierText, cornerMultiplierShadowText,
        streakText, followJudgementText, followComboText,
        activeFeverBonusPointsText, maxPlusText, maxText, greatText, lateText, earlyText;
    public TextMeshProUGUI[] plusScoreTextArr, plusAccuracyTextArr;

    // Gameobject
    public GameObject followInfo, lastFollowInfoTextObject;

    // Integers
    private int highestCombo, totalLate, totalMaxPlus, totalMax, totalGreat, totalEarly, totalMiss, currentCombo, currentScore, totalHit,
        currentMultiplier, scoreStringLength, plusScoreIndex, nextStreak, plusAccuracyIndex, totalFeverPhrasesHit;
    private float currentPercentage, scoreToLerpTo, scoreLerp, percentageCalculationScore, totalScorePossible,
        prevFramePercentage, totalFeverBonusScore, activeFeverBonusScore, activeFeverBonusScoreLerp,
        activeFeverBonusScoreToLerpTo;

    // Strings
    private string gradeAchieved, scoreString, percentageType;

    // Bool
    private bool lerpScore, lerpActiveFeverBonusScore;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool LerpScore
    {
        set { lerpScore = value; }
    }
    
    public float CurrentPercentage
    {
        get { return currentPercentage; }
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
        totalFeverBonusScore = 0;
        activeFeverBonusScore = 0;
        percentageCalculationScore = 0;
        scoreLerp = 0;
        currentMultiplier = 0;
        currentPercentage = 0;
        prevFramePercentage = 0;
        scoreStringLength = 0;
        activeFeverBonusScoreToLerpTo = 0;
        activeFeverBonusScoreLerp = 0;
        nextStreak = Constants.STREAK_INTERVAL;
        scoreString = "";
        gradeAchieved = Constants.GRADE_TBD;
        lerpScore = false;
        lerpActiveFeverBonusScore = false;
        percentageType = Constants.PERCENTAGE_TYPE_INCREASE;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        ReferencePlusScoreTextAnimatorArr();
        ReferencePlusAccuracyTextAnimatorArr();
        CalculateHighestScoreForBeatmap();
        UpdateComboText();
        CalculatePercentage();
    }

    private void Update()
    {
        switch (lerpScore)
        {
            case true:
                LerpToScore();
                break;
        }

        switch (lerpActiveFeverBonusScore)
        {
            case true:
                LerpToActiveFeverBonusScore();
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
    private void PlayPlusScoreTextAnimation(TMP_ColorGradient _colorGradient, string _score)
    {
        if (plusScoreIndex == plusScoreTextAnimatorArr.Length)
        {
            plusScoreIndex = 0;
        }

        plusScoreTextArr[plusScoreIndex].text = Constants.PLUS_PREFIX + _score;
        plusScoreTextArr[plusScoreIndex].colorGradientPreset = _colorGradient;
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

    // Increase total fever bonus score
    public void IncreaseTotalFeverBonusScore(int _score)
    {
        totalFeverBonusScore += _score;
        activeFeverBonusScoreToLerpTo = (activeFeverBonusScore + _score);
        activeFeverBonusScoreLerp = 0f;
        lerpActiveFeverBonusScore = true;
    }

    // Reset active fever bonus score
    public void ResetActiveFeverBonusScore()
    {
        activeFeverBonusScoreLerp = 0f;
        activeFeverBonusScore = 0;
        lerpActiveFeverBonusScore = false;
        activeFeverBonusPointsText.text = Constants.PLUS_PREFIX + 0.ToString();
    }

    // Increase total fever phrases hit
    public void IncreaseTotalFeverPhrasesHit()
    {
        totalFeverPhrasesHit++;
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
                    cornerMultiplierText.text = Constants.MULTIPLIER_2X_STRING;
                    cornerMultiplierShadowText.text = Constants.MULTIPLIER_2X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    cornerMultiplierAnimator.Play("CornerMultiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.eRankColor;
                    cornerMultiplierText.color = scriptManager.uiColorManager.eRankColor;
                }
                break;
            case Constants.MULTIPLIER_2X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_3X)
                {
                    multiplierText.text = Constants.MULTIPLIER_3X_STRING;
                    cornerMultiplierText.text = Constants.MULTIPLIER_3X_STRING;
                    cornerMultiplierShadowText.text = Constants.MULTIPLIER_3X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    cornerMultiplierAnimator.Play("CornerMultiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.cRankColor;
                    cornerMultiplierText.color = scriptManager.uiColorManager.cRankColor;
                }
                break;
            case Constants.MULTIPLIER_3X_STRING:
                if (currentCombo >= Constants.MULTIPLIER_COMBO_4X)
                {
                    multiplierText.text = Constants.MULTIPLIER_4X_STRING;
                    cornerMultiplierText.text = Constants.MULTIPLIER_4X_STRING;
                    cornerMultiplierShadowText.text = Constants.MULTIPLIER_4X_STRING;
                    multiplierAnimator.Play("Multiplier_Increase_Animation", 0, 0f);
                    cornerMultiplierAnimator.Play("CornerMultiplier_Increase_Animation", 0, 0f);
                    multiplierText.color = scriptManager.uiColorManager.dRankColor;
                    cornerMultiplierText.color = scriptManager.uiColorManager.dRankColor;
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
            cornerMultiplierText.text = Constants.MULTIPLIER_1X_STRING;
            cornerMultiplierShadowText.text = Constants.MULTIPLIER_1X_STRING;
            multiplierText.color = scriptManager.uiColorManager.offlineColorSolid;
            cornerMultiplierText.color = scriptManager.uiColorManager.solidBlackColor;
            multiplierAnimator.Play("Multiplier_Reset_Animation", 0, 0f);
            cornerMultiplierAnimator.Play("CornerMultiplier_Reset_Animation", 0, 0f);
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

    // Lerp to the score
    private void LerpToActiveFeverBonusScore()
    {
        activeFeverBonusScoreLerp += Time.deltaTime / Constants.SCORE_LERP_DURATION;
        int score = (int)Mathf.Lerp(activeFeverBonusScore, activeFeverBonusScoreToLerpTo, activeFeverBonusScoreLerp);

        activeFeverBonusScore = score;

        activeFeverBonusPointsText.text = Constants.PLUS_PREFIX + activeFeverBonusScore.ToString();

        if (activeFeverBonusScore >= activeFeverBonusScoreToLerpTo)
        {
            lerpActiveFeverBonusScore = false;
        }
    }

    // Add judgement
    public void AddEarlyJudgement()
    {
        // Increment total hit
        totalEarly++;

        // Take away the difference to calculate percentage
        percentageCalculationScore -= (Constants.MAXPLUS_SCORE_VALUE - Constants.EARLY_SCORE_VALUE);

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.lateColorGradient, Constants.EARLY_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.EARLY_JUDGEMENT, scriptManager.uiColorManager.cRankColor);
    }

    // Add judgement
    public void AddGreatJudgement()
    {
        // Increment total hit
        totalGreat++;

        // Take away the difference to calculate percentage
        percentageCalculationScore -= (Constants.MAXPLUS_SCORE_VALUE - Constants.GREAT_SCORE_VALUE);

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.greatColorGradient, Constants.GREAT_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.GREAT_JUDGEMENT, scriptManager.uiColorManager.easyDifficultyColor);
    }

    // Add judgement
    public void AddMaxPlusJudgement()
    {
        // Increment total hit
        totalMaxPlus++;

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.maxPlusColorGradient, Constants.MAXPLUS_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.MAXPLUS_JUDGEMENT, scriptManager.uiColorManager.selectedColor);
    }

    // Add judgement
    public void AddMaxJudgement()
    {
        // Increment total hit
        totalMax++;

        // Take away the difference to calculate percentage
        percentageCalculationScore -= (Constants.MAXPLUS_SCORE_VALUE - Constants.MAX_SCORE_VALUE);

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.maxColorGradient, Constants.MAX_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.MAX_JUDGEMENT, scriptManager.uiColorManager.selectedColor);
    }


    // Add judgement
    public void AddLateJudgement()
    {
        // Increment total hit
        totalLate++;

        // Take away the difference to calculate percentage
        percentageCalculationScore -= (Constants.MAXPLUS_SCORE_VALUE - Constants.LATE_SCORE_VALUE);

        // Play the next plus score text animation
        PlayPlusScoreTextAnimation(scriptManager.uiColorManager.lateColorGradient, Constants.LATE_SCORE_STRING);

        // Display follow info
        UpdateJudgementText(Constants.LATE_JUDGEMENT, scriptManager.uiColorManager.selectedColor);
    }

    // Add miss judgement
    public void AddMissJudgement()
    {
        // Increment total missed
        totalMiss++;

        // Take away perfect score difference
        percentageCalculationScore -= Constants.MAXPLUS_SCORE_VALUE;

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
    public void DisplayFollowInfo(Vector3 _position, string _judgement)
    {
        followInfo.transform.position = _position;
        HideFollowInfo();
        followInfo.gameObject.SetActive(true);

        switch (_judgement)
        {
            case Constants.MAXPLUS_JUDGEMENT:
                maxPlusText.gameObject.SetActive(true);
                lastFollowInfoTextObject = maxPlusText.gameObject;
                break;
            case Constants.MAX_JUDGEMENT:
                maxText.gameObject.SetActive(true);
                lastFollowInfoTextObject = maxText.gameObject;
                break;
            case Constants.LATE_JUDGEMENT:
                lateText.gameObject.SetActive(true);
                lastFollowInfoTextObject = lateText.gameObject;
                break;
            case Constants.GREAT_JUDGEMENT:
                greatText.gameObject.SetActive(true);
                lastFollowInfoTextObject = greatText.gameObject;
                break;
            case Constants.EARLY_JUDGEMENT:
                earlyText.gameObject.SetActive(true);
                lastFollowInfoTextObject = earlyText.gameObject;
                break;
        }
    }

    // Hide follow info
    public void HideFollowInfo()
    {
        lastFollowInfoTextObject.gameObject.SetActive(false);
        followInfo.gameObject.SetActive(false);
    }

    // Check the highest score possible by calculating the total notes in the song x perfect judgement
    public void CalculateHighestScoreForBeatmap()
    {
        totalScorePossible = Database.database.LoadedPositionX.Count * Constants.MAXPLUS_SCORE_VALUE;
        percentageCalculationScore = totalScorePossible;
    }

    // Calculate percentage
    private void CalculatePercentage()
    {
        switch (percentageType)
        {
            case Constants.PERCENTAGE_TYPE_DECREASE:
                currentPercentage = (percentageCalculationScore / totalScorePossible) * 100;
                break;
            case Constants.PERCENTAGE_TYPE_INCREASE:
                currentPercentage = (currentScore / totalScorePossible) * 100;
                break;
        }

        // Check if current grade has increased
        string returnedGradeAchieved = scriptManager.gradeManager.CalculateGrade(currentPercentage);

        // Update grade if it has changed
        if (returnedGradeAchieved != gradeAchieved)
        {
            gradeAchieved = returnedGradeAchieved;
            scriptManager.gradebar.UpdateGradeAchieved(gradeAchieved);
        }

        // Update percentage text
        percentageText.text = currentPercentage.ToString("F2") + Constants.PERCENTAGE_PREFIX;

        // Check previous frame percentage with new percentage
        if (prevFramePercentage != currentPercentage)
        {
            string accuracyString = "";

            switch (percentageType)
            {
                case Constants.PERCENTAGE_TYPE_DECREASE:
                    accuracyString = (prevFramePercentage - currentPercentage).ToString("F2");
                    if (prevFramePercentage > currentPercentage)
                    {
                        PlayPlusAccuracyTextAnimation(scriptManager.uiColorManager.offlineColorSolid,
                            (Constants.NEGATIVE_PREFIX + accuracyString));
                        scriptManager.gradebar.UpdateGradeSlider();
                    }
                    break;
                case Constants.PERCENTAGE_TYPE_INCREASE:
                    accuracyString = (currentPercentage - prevFramePercentage).ToString("F2");
                    if (prevFramePercentage < currentPercentage)
                    {
                        PlayPlusAccuracyTextAnimation(scriptManager.uiColorManager.onlineColorSolid,
                            (Constants.PLUS_PREFIX + accuracyString));
                        scriptManager.gradebar.UpdateGradeSlider();
                    }
                    break;
            }
        }

        // Update previous frame percentage
        prevFramePercentage = currentPercentage;

        //currentPercentage = (currentScore / totalScorePossible) * 100;
    }
    #endregion
}
