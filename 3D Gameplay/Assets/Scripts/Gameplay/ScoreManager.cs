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
    public Animator scoreAnimation; // Animate the score text
    public Animator comboAnimation; // Animate the combo text
    public Animator largeComboAnimation; // Animate the large combo text
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

    // Use this for initialization
    void Start () {

        // Reference to the hitSoundPreview for playing the miss combo break sound
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();

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


        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Check the combo for spawning the correct particle effect
        CheckComboParticles();

        // Calculate the highest score possible in the beatmap
        CalculateHighestScoreForBeatmap();
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
        
        if (combo >= 100 && combo < 500)
        {
            ActivateWhiteComboParticles();
        }

        if (combo >= 500)
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
            comboAnimation.Play("ComboBreakTextAnimation");
            largeComboAnimation.Play("LargeComboBreakTextAnimation");
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
        scoreAnimation.Play("GameplayUITextAnimation");

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

        comboAnimation.Play("GameplayUITextAnimation");
        largeComboAnimation.Play("LargeComboTextAnimation");
    }

    // Update judgement text
    public void AddJudgement(string judgementPass)
    {
        judgementText.text = judgementPass.ToString();

        if (judgementPass == "EARLY")
        {
            judgementAnimation.Play("EARLYAnimation");

            // Hit so we add 1 to the total for the results screen
            totalEarly++;
        }
        else if (judgementPass == "GOOD")
        {
            judgementAnimation.Play("GOODAnimation");
            // Hit so we add 1 to the total for the results screen
            totalGood++;
        }
        else if (judgementPass == "PERFECT")
        {
            judgementAnimation.Play("PERFECTAnimation");
            // Hit so we add 1 to the total for the results screen
            totalPerfect++;
        }
        else if (judgementPass == "MISS")
        {
            judgementAnimation.Play("MISSAnimation");
            // Hit so we add 1 to the total for the results screen
            totalMiss++;
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
        // Get the total number of hit objects possible in the map
        totalHitObjects = Database.database.LoadedPositionX.Count;
        float scorePerPerfect = 500;
        totalScorePossible = totalHitObjects * scorePerPerfect;

    }
}
