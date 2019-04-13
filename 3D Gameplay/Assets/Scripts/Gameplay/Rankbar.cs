using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rankbar : MonoBehaviour {

    public Slider rankBarSlider;
    public Image rankBarFill;
    private float currentPercentage; // The current percentage of score out of max possible score
    float fRankAmount, eRankAmount, dRankAmount, cRankAmount, bRankAmount, aRankAmount, sRankAmount, ssRankAmount;
    public Color fRankColor, eRankColor, dRankColor, cRankColor, bRankColor, aRankColor, sRankColor, ssRankColor;

    // Get the reference to the score manager
    private ScoreManager scoreManager;

    private float totalScorePossible; // The total score possible for the beatmap
    private float currentScore; // The players current score

    // Use this for initialization
    void Start () {

        currentPercentage = 0f;
        scoreManager = FindObjectOfType<ScoreManager>();

	}
	
	// Update is called once per frame
	void Update () {

        // Get the current values for the rank bar
        GetValues();

        // Calculate and update the rank bar
        UpdateRankBar();
	}

    // Get the current score and total score possible
    public void GetValues()
    {
        currentScore = scoreManager.currentScore;
        totalScorePossible = scoreManager.totalScorePossible;
    }

    // Update the rank bar with the current rank color and percentage
    public void UpdateRankBar()
    {
        //Percentage = ((totalScorePossible - score) * 100) / score;
        currentPercentage = (currentScore / totalScorePossible) * 100;

        if (currentPercentage < 50)
        {
            // F rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = fRankColor;
        }
        else if (currentPercentage >= 50 && currentPercentage < 60)
        {
            // E rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = eRankColor;
        }
        else if (currentPercentage >= 60 && currentPercentage < 70)
        {
            // D rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = dRankColor;
        }
        else if (currentPercentage >= 70 && currentPercentage < 80)
        {
            // C rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = cRankColor;
        }
        else if (currentPercentage >= 80 && currentPercentage < 90)
        {
            // B rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = bRankColor;
        }
        else if (currentPercentage >= 90 && currentPercentage < 95)
        {
            // A rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = aRankColor;
        }
        else if (currentPercentage >= 95 && currentPercentage < 100)
        {
            // S rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = sRankColor;
        }
        else if (currentPercentage == 100)
        {
            // SS rank 
            // Set the color and value
            rankBarSlider.value = currentPercentage;
            rankBarFill.color = ssRankColor;
        }
    }
}
