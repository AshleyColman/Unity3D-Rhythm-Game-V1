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
    public TextMeshProUGUI TotalHitText;
    public TextMeshProUGUI SpecialText;
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
        ScoreText.text = gameplayToResultsManager.score.ToString();
        ComboText.text = gameplayToResultsManager.highestCombo.ToString();
        UsernameText.text = MySQLDBManager.username.ToString();
        PercentageText.text = gameplayToResultsManager.Percentage.ToString() + "%";
        gradePercentage = gameplayToResultsManager.Percentage;
        TotalHitText.text = gameplayToResultsManager.totalHit.ToString();
        SpecialText.text = gameplayToResultsManager.totalSpecial.ToString();
        SongTitleText.text = gameplayToResultsManager.songTitle.ToString();
        BeatmapCreatorText.text = gameplayToResultsManager.beatmapCreator.ToString();

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
}
