using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    // UI
    public TextMeshProUGUI scoreText, comboText;

    // Integers
    private int highestCombo;
    private int totalPerfect;
    private int totalGood;
    private int totalEarly;
    private int totalMiss;
    private int gradeAchieved;
    private int combo;
    public int currentScore;
    private float totalScorePossible;
    private int totalHit;
    private int totalHitObjects;
    private int activateWhiteComboParticlesValue, activateRainbowComboParticlesValue;
    private int comboBreakValue;

    // Chars
    private char comboX;

    // Bools
    private bool hasFailed; // Has the player failed

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public int HighestCombo
    {
        get { return highestCombo; }
    }

    public int TotalPerfect
    {
        get { return totalPerfect; }
    }

    public int TotalGood
    {
        get { return totalGood; }
    }

    public int TotalEarly
    {
        get { return totalEarly; }
    }

    public int TotalMiss
    {
        get { return totalMiss; }
    }

    public float CurrentScore
    {
        get { return currentScore; }
    }

    public int TotalHit
    {
        get { return totalHit; }
    }

    public int TotalHitObjects
    {
        get { return totalHitObjects; }
    }

    public float TotalScorePossible
    {
        get { return totalScorePossible; }
    }

    public int Combo
    {
        get { return combo; }
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        highestCombo = 0;
        activateWhiteComboParticlesValue = 100;
        activateRainbowComboParticlesValue = 150;
        comboBreakValue = 5;
        comboX = 'x';
        comboText.text = combo.ToString() + comboX;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        CalculateHighestScoreForBeatmap(); // Calculate the highest score possible in the beatmap
    }

    // Reset combo
    public void ResetCombo()
    {
        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Reset the fever time combo

        // Check if combo is below 5
        if (combo >= comboBreakValue)
        {
            // Play miss sound
            //hitSoundPreview.PlayMissSound();
            // Play combo break animation
            //comboAnimator.Play("BadScoreTextAnimation", 0, 0f);
        }

        // Reset combo
        combo = 0;

        // Update the text
        comboText.text = comboX + combo.ToString();
    }

    // Update the score text
    public void AddScore(int _scoreValue)
    {
        // A hit object has been hit add 1 to total hit objects hit
        totalHit++;

        // Increment score
        currentScore += _scoreValue;

        // Check the score and add the 0's according to the type
        if (currentScore < 1000)
        {
            scoreText.text = "00000" + currentScore.ToString();
        }
        if (currentScore >= 1000 && currentScore < 10000)
        {
            scoreText.text = "0000" + currentScore.ToString();
        }
        if (currentScore >= 10000 && currentScore < 100000)
        {
            scoreText.text = "000" + currentScore.ToString();
        }
        if (currentScore >= 100000 && currentScore < 1000000)
        {
            scoreText.text = "00" + currentScore.ToString();
        }
        if (currentScore >= 1000000 && currentScore < 10000000)
        {
            scoreText.text = "0" + currentScore.ToString();
        }
        if (currentScore >= 10000000 && currentScore < 100000000)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    // Update combo text
    public void AddCombo()
    {
        // Add to the existing combo
        combo++;

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Update combo text
        comboText.text = comboX + combo.ToString();

        // Update the rank bar percentage and color
        //rankbar.UpdateRankBar();
    }

    // Update judgement text
    public void AddJudgement(string _judgement)
    {
        switch (_judgement)
        {
            case "EARLY":
                totalEarly++;
                break;
            case "GOOD":
                totalGood++;
                break;
            case "PERFECT":
                totalPerfect++;
                break;
            case "MISS":
                totalMiss++;
                break;
        }
    }

    // Check if the current combo is the highest combo so far
    public void CheckHighestCombo()
    {
        if (combo > highestCombo)
        {
            highestCombo = combo;
        }
    }

    // Check the highest score possible by calculating the total notes in the song x perfect judgement
    public void CalculateHighestScoreForBeatmap()
    {
        // Check if a mod has been activated
        // If a mod is activated get its multiplier per note
        // Multiply each note by that amount bonus to get the max
        // Get the total score 
        // Use that score as the highest score

        // Get the total number of hit objects possible in the map
        totalHitObjects = Database.database.LoadedPositionX.Count;
        // Multiply the score per perfect by the multiplier, 5 is the score for a standard perfect note but will be multiplied by the default multiplier (100) to give 500 points
        //float scorePerPerfect = (5 * playerSkillsManager.ScoreMultiplier);
        //totalScorePossible = totalHitObjects * scorePerPerfect;
    }

}
