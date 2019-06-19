using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIManager : MonoBehaviour {

    public TextMeshProUGUI SongTitleText;
    public TextMeshProUGUI BeatmapCreatorText;

    public TextMeshProUGUI PerfectText;
    public TextMeshProUGUI GoodText;
    public TextMeshProUGUI EarlyText;
    public TextMeshProUGUI MissText;
    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI PercentageText;
    public TextMeshProUGUI UsernameText;
    public Animator gradeIconAnimator;
    public TextMeshProUGUI ScoreText;
    private float gradePercentage;
    public GameplayToResultsManager gameplayToResultsManager;

    public PlayerSkillsManager playerSkillsManager;

    // Mod icons
    public GameObject tripleTimeMod, doubleTimeMod, halfTimeMod, instantDeathMod, noFailMod;
    public GameObject modGlow;

    bool hasLoadedResults;

    public float GradePercentage
    {
        get
        {
            return gradePercentage;
        }

        set
        {
            gradePercentage = value;
        }
    }

    void Start()
    {
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();
    }

    void Update()
    {
        CheckMods();

        LoadResults();
    }

    /*
    IEnumerator UpdateTextTest()
    {
        //increase by animation
        float i = 0f;
        while (i <= 1f)
        {
            print(2);
            i += Time.deltaTime / NumberAnimationDuration;

            int newCombo = (int)Mathf.Lerp(0f, (float)maxCombo, i);
            Color newColor = Color.Lerp(startColor, fullColor, (float)newCombo / (float)fullNoteCounts);

            //set text
            winComboScoreText.text = newCombo.ToString();

            //set color
            winComboCircle.color = newColor;
            winComboScoreText.color = newColor;
            winComboText.color = newColor;

            yield return null;
        }

    }
    */

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


    public void LoadResults()
    {
        PerfectText.text = gameplayToResultsManager.totalPerfect.ToString();
        GoodText.text = gameplayToResultsManager.totalGood.ToString();
        EarlyText.text = gameplayToResultsManager.totalEarly.ToString();
        MissText.text = gameplayToResultsManager.totalMiss.ToString();
        ComboText.text = gameplayToResultsManager.highestCombo.ToString() + "/" + gameplayToResultsManager.totalHitObjects;
        PercentageText.text = gameplayToResultsManager.Percentage.ToString();
        GradePercentage = gameplayToResultsManager.Percentage;
        SongTitleText.text = gameplayToResultsManager.songTitle.ToString();
        BeatmapCreatorText.text = gameplayToResultsManager.beatmapCreator.ToString();

        // If logged in get the username
        if (MySQLDBManager.loggedIn)
        {
            UsernameText.text = MySQLDBManager.username.ToString();
        }
        else
        {
            UsernameText.text = "GUEST";
        }

        // Check score for 0's adding on
        CheckScore();






        // Enable the grade achieved
        if (gameplayToResultsManager.gradeAchieved == "P")
        {
            gradeIconAnimator.Play("BunnyPRank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "S")
        {
            gradeIconAnimator.Play("BunnySRank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "A")
        {
            gradeIconAnimator.Play("BunnyARank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "B")
        {
            gradeIconAnimator.Play("BunnyBRank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "C")
        {
            gradeIconAnimator.Play("BunnyCRank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "D")
        {
            gradeIconAnimator.Play("BunnyDRank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "E")
        {
            gradeIconAnimator.Play("BunnyERank");
        }
        else if (gameplayToResultsManager.gradeAchieved == "F")
        {
            gradeIconAnimator.Play("BunnyFRank");
        }
    }
    
    // Check score and add 0's
    private void CheckScore()
    {
        // Get the score
        int score = gameplayToResultsManager.score;

        // Check the score and add the 0's according to the type
        if (score < 1000)
        {
            ScoreText.text = "00000" + score.ToString();
        }
        if (score >= 1000 && score < 10000)
        {
            ScoreText.text = "0000" + score.ToString();
        }
        if (score >= 10000 && score < 100000)
        {
            ScoreText.text = "000" + score.ToString();
        }
        if (score >= 100000 && score < 1000000)
        {
            ScoreText.text = "00" + score.ToString();
        }
        if (score >= 1000000 && score < 10000000)
        {
            ScoreText.text = "0" + score.ToString();
        }
        if (score >= 10000000 && score < 100000000)
        {
            ScoreText.text = score.ToString();
        }
    }
}
