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
    public TextMeshProUGUI GradeTextSS;
    public TextMeshProUGUI GradeTextS;
    public TextMeshProUGUI GradeTextA;
    public TextMeshProUGUI GradeTextB;
    public TextMeshProUGUI GradeTextC;
    public TextMeshProUGUI GradeTextD;
    public TextMeshProUGUI GradeTextE;
    public TextMeshProUGUI GradeTextF;
    public TextMeshProUGUI ScoreText;
    public float gradePercentage;
    public GameplayToResultsManager gameplayToResultsManager;
   
    void Start()
    {
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();
    }

    void Update()
    {
        LoadResults();
    }

    public void LoadResults()
    {
        PerfectText.text = gameplayToResultsManager.totalPerfect.ToString();
        GoodText.text = gameplayToResultsManager.totalGood.ToString();
        EarlyText.text = gameplayToResultsManager.totalEarly.ToString();
        MissText.text = gameplayToResultsManager.totalMiss.ToString();
        ComboText.text = gameplayToResultsManager.highestCombo.ToString() + "/" + gameplayToResultsManager.totalHitObjects;
        PercentageText.text = gameplayToResultsManager.Percentage.ToString();
        gradePercentage = gameplayToResultsManager.Percentage;
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
        if (gameplayToResultsManager.gradeAchieved == "SS")
        {
            GradeTextSS.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "S")
        {
            GradeTextS.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "A")
        {
            GradeTextA.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "B")
        {
            GradeTextB.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "C")
        {
            GradeTextC.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "D")
        {
            GradeTextD.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "E")
        {
            GradeTextE.gameObject.SetActive(true);
        }
        else if (gameplayToResultsManager.gradeAchieved == "F")
        {
            GradeTextF.gameObject.SetActive(true);
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
