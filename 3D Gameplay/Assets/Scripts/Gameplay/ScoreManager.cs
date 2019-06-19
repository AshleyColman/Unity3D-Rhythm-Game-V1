using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public int currentScore;
    public int scoreToLerpTo;
    public TextMeshProUGUI scoreText;
    public int combo;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI largeComboText;
    public string judgement;
    public TextMeshProUGUI judgementText;
    public TextMeshProUGUI judgementTextLarge;

    public Animator comboAnimation; // Animate the combo text
    public Animator judgementAnimation; // Animate the judgement text

    public int highestCombo;
    public int totalPerfect;
    public int totalGood;
    public int totalEarly;
    public int totalMiss;
    public int gradeAchieved;
    public float totalScorePossible;
    public float totalHit;
    public float totalSpecial;
    private int pointIncreasePerSecond;
    public float totalHitObjects;
    private HitSoundPreview hitSoundPreview;

    public ParticleSystem comboParticlesWhite;
    public ParticleSystem comboParticlesRainbow;

    PlayerSkillsManager playerSkillsManager;

    FailAndRetryManager failAndRetryManager; // Controls failing/retrying

    private bool hasFailed; // Has the player failed

    // Mod icons
    public GameObject tripleTimeMod, doubleTimeMod, halfTimeMod, instantDeathMod, noFailMod;
    public GameObject modGlow;




    // Use this for initialization
    void Start () {

        // Reference to the failAndRetryManager
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();

        // Reference to the hitSoundPreview for playing the miss combo break sound
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();

        // Reference to the player skills manager for getting the mod used
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();

        highestCombo = 0;

        pointIncreasePerSecond = 100;

        //scoreText.text = score.ToString();
        comboText.text = combo.ToString() + "x";
        largeComboText.text = combo.ToString() + "x";
        judgementText.text = judgement.ToString();
    }
	
	// Update is called once per frame
	void Update () {

        // Lerp the score overtime with the timer 
        LerpScore();

        // Check the mods used
        CheckMods();

        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check the combo for spawning the correct particle effect
        CheckComboParticles();

        // Calculate the highest score possible in the beatmap
        CalculateHighestScoreForBeatmap();
	}

    // Check mod used and display mod used
    private void CheckMods()
    {
        if (playerSkillsManager.tripleTimeSelected == true)
        {
            tripleTimeMod.gameObject.SetActive(true);
            modGlow.gameObject.SetActive(true);
        }
        else if (playerSkillsManager.doubleTimeSelected == true)
        {
            doubleTimeMod.gameObject.SetActive(true);
            modGlow.gameObject.SetActive(true);
        }
        else if (playerSkillsManager.halfTimeSelected == true)
        {
            halfTimeMod.gameObject.SetActive(true);
            modGlow.gameObject.SetActive(true);
        }
        else if (playerSkillsManager.instantDeathSelected == true)
        {
            instantDeathMod.gameObject.SetActive(true);
            modGlow.gameObject.SetActive(true);
        }
        else if (playerSkillsManager.noFailSelected == true)
        {
            noFailMod.gameObject.SetActive(true);
            modGlow.gameObject.SetActive(true);
        }
    }


    // Activate white combo particles
    private void ActivateWhiteComboParticles()
    {
        comboParticlesWhite.gameObject.SetActive(true);
    }

    // Deactivate white combo particles
    private void DeactivateWhiteComboParticles()
    {
        comboParticlesWhite.gameObject.SetActive(false);
    }

    // Activate rainbow combo particles
    private void ActivateRainbowComboParticles()
    {
        comboParticlesRainbow.gameObject.SetActive(true);
    }

    // Deactivate rainbow combo particles
    private void DeactivateRainbowComboParticles()
    {
        comboParticlesRainbow.gameObject.SetActive(false);
    }

    // Check combo for particles
    private void CheckComboParticles()
    {
        if (combo < 100)
        {
            DeactivateWhiteComboParticles();
            DeactivateRainbowComboParticles();
        }
        
        if (combo >= 100 && combo < 150)
        {
            ActivateWhiteComboParticles();
        }

        if (combo >= 150)
        {
            DeactivateWhiteComboParticles();
            ActivateRainbowComboParticles();
        }
    }

    // Reset combo
    public void ResetCombo()
    {
        // Play the miss sound if a combo break
        if (combo >= 5)
        {
            hitSoundPreview.PlayMissSound();

            comboAnimation.Play("ComboBreakTextAnimation", 0, 0f);
        }

        combo = 0;
        comboText.text = "   " + combo.ToString() + "x";
        largeComboText.text = "   " + combo.ToString() + "x";

    }

    // Update the score text
    public void AddScore(int scorePass, string objectScoreType)
    {
        // A hit object has been hit add 1 to total hit objects hit
        totalHit++;
    
        // Scoretypes can be special or normal

        // Check which type of object was hit
        if (objectScoreType == "SPECIAL")
        {
            totalSpecial++;
        }

        // Play the score animation
        //scoreAnimation.Play("GameplayUITextAnimation");



        // Multiply the default score per note passed by the mod mutiplier
        scorePass = (scorePass * playerSkillsManager.scoreMultiplier);

        // Increase the score to lerp to
        scoreToLerpTo += scorePass;
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
    }

    // Update combo text
    public void AddCombo(int comboPass)
    {
        combo += comboPass;

        // Check the combo and add spacing according to the type
        if (combo < 10)
        {
            comboText.text = "   " + combo.ToString() + "x";
            largeComboText.text = "   " + combo.ToString() + "x";
        }
        if (combo >= 10 && combo < 100)
        {
            comboText.text = "  " + combo.ToString() + "x";
            largeComboText.text = "  " + combo.ToString() + "x";
        }
        if (combo >= 100 && combo < 1000)
        {
            comboText.text = " " + combo.ToString() + "x";
            largeComboText.text = " " + combo.ToString() + "x";
        }
        if (combo >= 1000 && combo < 10000)
        {
            comboText.text = " " + combo.ToString() + "x";
            largeComboText.text = " " + combo.ToString() + "x";
        }

        comboAnimation.Play("ComboAnimation", 0, 0f);
    }

    // Update judgement text
    public void AddJudgement(string judgementPass)
    {
        judgementText.text = judgementPass.ToString();
        judgementTextLarge.text = judgementPass.ToString();

        switch (judgementPass)
        {
            case "EARLY":
                judgementAnimation.Play("EARLY", 0, 0f);
                totalEarly++;
                break;
            case "GOOD":
                judgementAnimation.Play("GOOD", 0, 0f);
                totalGood++;
                break;
            case "PERFECT":
                judgementAnimation.Play("PERFECT", 0, 0f);
                totalPerfect++;
                break;
            case "MISS":
                judgementAnimation.Play("MISS", 0, 0f);
                totalMiss++;
                break;
        }

        // Check if instant death is equiped and if the judgement passed is not perfect
        CheckIfInstantDeath(judgementPass);

    }

    // Check if instant death
    private void CheckIfInstantDeath(string judgementPass)
    {
        // Check if the user has failed yet
        hasFailed = failAndRetryManager.ReturnHasFailed();

        // If the user has not failed yet
        if (hasFailed == false)
        {
            // If the judgement passed is not perfect, and instant death has been equiped
            if (judgementPass != "PERFECT" && playerSkillsManager.instantDeathSelected == true)
            {
                // Activate fail screen
                failAndRetryManager.HasFailed();
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
        totalHitObjects = Database.database.LoadedPositionX.Count;
        // Multiply the score per perfect by the multiplier, 5 is the score for a standard perfect note but will be multiplied by the default multiplier (100) to give 500 points
        float scorePerPerfect = (5 * playerSkillsManager.scoreMultiplier);
        totalScorePossible = totalHitObjects * scorePerPerfect;


        /*
        // Get the total number of hit objects possible in the map
        totalHitObjects = Database.database.LoadedPositionX.Count;
        float scorePerPerfect = 500;
        totalScorePossible = totalHitObjects * scorePerPerfect;
        */
    }

}
