using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    // UI
    public TextMeshProUGUI scoreText, comboText, judgementText;
    public ParticleSystem comboParticlesWhite, comboParticlesRainbow;

    // Animation
    public Animator scoreTextAnimator; // Animates score text
    public Animator judgementAnimation; // Animates judgement text
    public Animator comboAnimator; // Controls combo animations

    // Gameobjects
    public GameObject tripleTimeMod, doubleTimeMod, halfTimeMod, instantDeathMod, noFailMod, modGlow; // Mod icons

    // Integers
    private int highestCombo;
    private int totalPerfect;
    private int totalGood;
    private int totalEarly;
    private int totalMiss;
    private int gradeAchieved;
    private int pointIncreasePerSecond;
    private int combo;
    private int currentScore;
    private int scoreToLerpTo;
    private float totalScorePossible;
    private int totalHit;
    private int totalHitObjects;
    private int activateWhiteComboParticlesValue, activateRainbowComboParticlesValue;
    private int comboBreakValue;

    // Strings
    private string judgement;

    // Chars
    private char comboX;

    // Bools
    private bool hasFailed; // Has the player failed

    // Scripts
    private HitSoundPreview hitSoundPreview; // Controls hit sounds
    private PlayerSkillsManager playerSkillsManager; // Control player skills
    private FailAndRetryManager failAndRetryManager; // Controls failing/retrying
    private Rankbar rankbar; // Rank bar for displaying rank progress 
    private FeverTimeManager feverTimeManager; // Fever time manager

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
        pointIncreasePerSecond = 100;
        activateWhiteComboParticlesValue = 100;
        activateRainbowComboParticlesValue = 150;
        comboBreakValue = 5;
        comboX = 'x';
        comboText.text = "COMBO: " + combo.ToString() + comboX;
        judgement = "";
        judgementText.text = judgement.ToString();

        // Reference
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();
        rankbar = FindObjectOfType<Rankbar>();
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        feverTimeManager = FindObjectOfType<FeverTimeManager>();

        // Functions
        CheckMods(); // Check the mods used and enable the icon 
        CalculateHighestScoreForBeatmap(); // Calculate the highest score possible in the beatmap
        CheckComboParticles(); // Check combo particles 
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp the score overtime with the timer 
        LerpScore();
    }

    // Check mod used and display mod used
    private void CheckMods()
    {
        // Check the mods based on the mod selected and display the mod icon during gampelay
        switch (playerSkillsManager.ModSelected)
        {
            case "TRIPLE TIME":
                tripleTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "DOUBLE TIME":
                doubleTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "HALF TIME":
                halfTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "NO FAIL":
                noFailMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "INSTANT DEATH":
                instantDeathMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
        }
    }

    // Activate white combo particles
    private void ActivateWhiteComboParticles()
    {
        // Check if combo particles are already activated
        if (comboParticlesWhite.gameObject.activeSelf != true)
        {
            // Activate if they're not 
            comboParticlesWhite.gameObject.SetActive(true);
        }
    }

    // Deactivate white combo particles
    private void DeactivateWhiteComboParticles()
    {
        // Check if the combo particles white are not already deactivated
        if (comboParticlesWhite.gameObject.activeSelf != false)
        {
            // If they aren't deactivate
            comboParticlesWhite.gameObject.SetActive(false);
        }
    }

    // Activate rainbow combo particles
    private void ActivateRainbowComboParticles()
    {
        if (comboParticlesRainbow.gameObject.activeSelf != true)
        {
            comboParticlesRainbow.gameObject.SetActive(true);
        }
    }

    // Deactivate rainbow combo particles
    private void DeactivateRainbowComboParticles()
    {
        if (comboParticlesRainbow.gameObject.activeSelf != false)
        {
            comboParticlesRainbow.gameObject.SetActive(false);
        }
    }

    // Check combo for particles
    private void CheckComboParticles()
    {
        // Deactivate both particles
        if (combo < activateWhiteComboParticlesValue)
        {
            DeactivateWhiteComboParticles();
            DeactivateRainbowComboParticles();
        }

        // Activate white particles
        if (combo >= activateWhiteComboParticlesValue && combo < activateRainbowComboParticlesValue)
        {
            ActivateWhiteComboParticles();
        }

        // Activate rainbow particles
        if (combo >= 150)
        {
            DeactivateWhiteComboParticles();
            ActivateRainbowComboParticles();
        }
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
            hitSoundPreview.PlayMissSound();
            // Play combo break animation
            comboAnimator.Play("BadScoreTextAnimation", 0, 0f);
        }

        // Reset combo
        combo = 0;

        // Update the text
        comboText.text = "COMBO: " + comboX + combo.ToString();

        // Check the combo for spawning the correct particle effect
        CheckComboParticles();
    }

    // Update the score text
    public void AddScore(int _scoreValue)
    {
        // A hit object has been hit add 1 to total hit objects hit
        totalHit++;

        // Multiply the default score per note passed by the mod mutiplier
        _scoreValue = (_scoreValue * playerSkillsManager.ScoreMultiplier);

        if (feverTimeManager.FeverTimeActivated == true)
        {

            // Multiplier the notes value by 2 if fever time has been activated
            _scoreValue = (_scoreValue * 2);
            // Play score text animation
            scoreTextAnimator.Play("FeverScoreTextAnimation", 0, 0f);
        }
        else
        {
            // Play score text animation
            scoreTextAnimator.Play("ScoreTextAnimation", 0, 0f);
        }

        // Increase the score to lerp to
        scoreToLerpTo += _scoreValue;
    }

    // Lerp the score to increase over time
    private void LerpScore()
    {
        if (currentScore < scoreToLerpTo)
        {
            // Increase the current score by the timer each time so that it does increase to the scoreToLerpTo
            currentScore += pointIncreasePerSecond;

            // Check the score and add the 0's according to the type
            if (currentScore < 1000)
            {
                scoreText.text = "SCORE: 00000" + currentScore.ToString();
            }
            if (currentScore >= 1000 && currentScore < 10000)
            {
                scoreText.text = "SCORE: 0000" + currentScore.ToString();
            }
            if (currentScore >= 10000 && currentScore < 100000)
            {
                scoreText.text = "SCORE: 000" + currentScore.ToString();
            }
            if (currentScore >= 100000 && currentScore < 1000000)
            {
                scoreText.text = "SCORE: 00" + currentScore.ToString();
            }
            if (currentScore >= 1000000 && currentScore < 10000000)
            {
                scoreText.text = "SCORE: 0" + currentScore.ToString();
            }
            if (currentScore >= 10000000 && currentScore < 100000000)
            {
                scoreText.text = currentScore.ToString();
            }
        }
    }

    // Update combo text
    public void AddCombo()
    {
        // Add to the existing combo
        combo++;

        // Add to the fever time combo
        feverTimeManager.IncrementFeverTimeCombo();

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Update combo text
        comboText.text = "COMBO: " + comboX + combo.ToString();

        // Play combo animation
        if (feverTimeManager.FeverTimeActivated == true)
        {
            // Play score text animation
            comboAnimator.Play("FeverScoreTextAnimation", 0, 0f);
        }
        else
        {
            // Play score text animation
            comboAnimator.Play("ScoreTextAnimation", 0, 0f);
        }

        // Update the rank bar percentage and color
        rankbar.UpdateRankBar();

        // Check the combo for spawning the correct particle effect
        CheckComboParticles();
    }

    // Update judgement text
    public void AddJudgement(string judgementPass)
    {
        judgementText.text = judgementPass.ToString();

        switch (judgementPass)
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

        // Check if instant death is equiped and if the judgement passed is not perfect
        CheckIfInstantDeath(judgementPass);
    }

    // Check if instant death
    private void CheckIfInstantDeath(string _judgement)
    {
        // Check if the user has failed yet
        hasFailed = failAndRetryManager.HasFailed;

        // If the user has not failed yet
        if (hasFailed == false)
        {
            // If the judgement passed is not perfect, and instant death has been equiped
            if (_judgement != "PERFECT" && playerSkillsManager.ModSelected == "INSTANT DEATH")
            {
                // Activate fail screen
                failAndRetryManager.PlayerHasFailed();
            }
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
        totalHitObjects = Database.database.loadedPositionX.Count;
        // Multiply the score per perfect by the multiplier, 5 is the score for a standard perfect note but will be multiplied by the default multiplier (100) to give 500 points
        float scorePerPerfect = (5 * playerSkillsManager.ScoreMultiplier);
        totalScorePossible = totalHitObjects * scorePerPerfect;
    }

}
