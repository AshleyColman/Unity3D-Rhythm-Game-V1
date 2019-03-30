using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public int score;
    public TextMeshProUGUI scoreText;
    public int combo;
    public TextMeshProUGUI comboText;
    public string judgement;
    public TextMeshProUGUI judgementText;
    public Animator scoreAnimation; // Animate the score text
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

    // Use this for initialization
    void Start () {

        highestCombo = 0;
        
        //scoreText.text = score.ToString();
        comboText.text = combo.ToString() + "x";
        judgementText.text = judgement.ToString();
    }
	
	// Update is called once per frame
	void Update () {


        // Check the current combo and see if it's the highest so far;
        CheckHighestCombo();

        // Calculate the highest score possible in the beatmap
        CalculateHighestScoreForBeatmap();
	}

    // Reset combo
    public void ResetCombo()
    {
        combo = 0;
        comboText.text = "   " + combo.ToString() + "x";
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

        scoreAnimation.Play("GameplayUITextAnimation");

        score += scorePass;

            // Check the score and add the 0's according to the type
            if (score < 1000)
            {
                scoreText.text = "00000" + score.ToString();
            }
            if (score >= 1000 && score < 10000)
            {
                scoreText.text = "0000" + score.ToString();
            }
            if (score >= 10000 && score < 100000)
            {
                scoreText.text = "000" + score.ToString();
            }
            if (score >= 100000 && score < 1000000)
            {
                scoreText.text = "000" + score.ToString();
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
        }
        if (combo >= 10 && combo < 100)
        {
            comboText.text = "  " + combo.ToString() + "x";
        }
        if (combo >= 100 && combo < 1000)
        {
            comboText.text = " " + combo.ToString() + "x";
        }
        if (combo >= 1000 && combo < 10000)
        {
            comboText.text = " " + combo.ToString() + "x";
        }

        comboAnimation.Play("GameplayUITextAnimation");
    }

    // Update judgement text
    public void AddJudgement(string judgementPass)
    {
        judgementText.text = judgementPass.ToString();

        judgementAnimation.Play("GameplayUITextAnimation");

        if (judgementPass == "EARLY")
        {
            judgementText.color = Color.red;

            // Hit so we add 1 to the total for the results screen
            totalEarly++;
        }
        else if (judgementPass == "GOOD")
        {
            judgementText.color = Color.blue;
            // Hit so we add 1 to the total for the results screen
            totalGood++;
        }
        else if (judgementPass == "PERFECT")
        {
            judgementText.color = Color.yellow;
            // Hit so we add 1 to the total for the results screen
            totalPerfect++;
        }
        else if (judgementPass == "MISS")
        {
            judgementText.color = Color.red;
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
        float totalHitObjects = Database.database.LoadedPositionX.Count;
        float scorePerPerfect = 500;
        totalScorePossible = totalHitObjects * scorePerPerfect;

    }
}
