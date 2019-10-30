using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rankbar : MonoBehaviour
{

    // UI
    public TextMeshProUGUI currentPercentageText; // Current percentage text
    public Slider rankBarSlider;
    public Image rankBarFill;

    // Animator
    public Animator percentageTextAnimator;

    // Integers
    private float currentPercentage; // The current percentage of score out of max possible score
    private float fRankAmount, eRankAmount, dRankAmount, cRankAmount, bRankAmount, aRankAmount, sRankAmount, ssRankAmount; // Values for ranks
    private float totalScorePossible; // The total score possible for the beatmap
    private float currentScore; // The players current score

    // Colors
    public Color fRankColor, eRankColor, dRankColor, cRankColor, bRankColor, aRankColor, sRankColor, pRankColor;

    // Strings
    private char percentageSign; // Percentage sign 
    private string fRank, eRank, dRank, cRank, bRank, aRank, sRank, pRank;

    // Scripts
    private ScoreManager scoreManager;
    private GameplayToResultsManager gameplayToResultsManager;
    private FeverTimeManager feverTimeManager;


    // Use this for initialization
    void Start()
    {

        // Initialize
        currentPercentage = 0f;
        percentageSign = '%';
        fRank = "F";
        eRank = "E";
        dRank = "D";
        cRank = "C";
        bRank = "B";
        aRank = "A";
        sRank = "S";
        pRank = "P";

        // Reference
        scoreManager = FindObjectOfType<ScoreManager>();
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();
        feverTimeManager = FindObjectOfType<FeverTimeManager>();

        // Script initialize
        currentScore = scoreManager.CurrentScore; // Get the current score
        totalScorePossible = scoreManager.TotalScorePossible; // Get the total score possible for the beatmap
    }

    // Update percentage text
    void UpdatePercentageText()
    {
        currentPercentageText.text = currentPercentage.ToString("F2") + percentageSign;
    }

    // Update the rank bar with the current rank color and percentage
    public void UpdateRankBar()
    {
        currentScore = scoreManager.CurrentScore; // Get the current score
        totalScorePossible = scoreManager.TotalScorePossible; // Get the total score possible for the beatmap

        currentPercentage = (currentScore / totalScorePossible) * 100;


        if (currentPercentage < 50)
        {
            // F rank 
            rankBarFill.color = fRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = fRank;
        }
        else if (currentPercentage >= 50 && currentPercentage < 60)
        {
            // E rank 
            rankBarFill.color = eRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = eRank;
        }
        else if (currentPercentage >= 60 && currentPercentage < 70)
        {
            // D rank 
            rankBarFill.color = dRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = dRank;
        }
        else if (currentPercentage >= 70 && currentPercentage < 80)
        {
            // C rank 
            rankBarFill.color = cRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = cRank;
        }
        else if (currentPercentage >= 80 && currentPercentage < 90)
        {
            // B rank 
            rankBarFill.color = bRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = bRank;
        }
        else if (currentPercentage >= 90 && currentPercentage < 98)
        {
            // A rank 
            rankBarFill.color = aRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = aRank;
        }
        else if (currentPercentage >= 98 && currentPercentage < 100)
        {
            // S rank 
            rankBarFill.color = sRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = sRank;
        }
        else if (currentPercentage >= 100)
        {
            // P rank 
            rankBarFill.color = pRankColor;
            // Update the rank achieved
            gameplayToResultsManager.GradeAchieved = pRank;
        }

        // Update the current percentage for the gameplayToResults manager
        gameplayToResultsManager.Percentage = currentPercentage.ToString("F2");
        // Set the color and value
        rankBarSlider.value = currentPercentage;
        // Update current percentage text
        UpdatePercentageText();


        if (feverTimeManager.FeverTimeActivated == true)
        {
            // Play score text animation
            percentageTextAnimator.Play("FeverScoreTextAnimation", 0, 0f);
        }
        else
        {
            // Play score text animation
            percentageTextAnimator.Play("ScoreTextAnimation", 0, 0f);
        }

    }
}
