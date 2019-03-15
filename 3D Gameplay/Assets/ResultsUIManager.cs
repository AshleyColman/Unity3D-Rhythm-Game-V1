using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUIManager : MonoBehaviour {

    public Text PerfectText;
    public Text GoodText;
    public Text EarlyText;
    public Text MissText;
    public Text ComboText;
    public Text GradeTextSS;
    public Text GradeTextS;
    public Text GradeTextA;
    public Text GradeTextB;
    public Text GradeTextC;
    public Text GradeTextD;
    public Text GradeTextE;
    public Text GradeTextF;
    public Text ScoreText;
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


        gradePercentage = gameplayToResultsManager.Percentage;
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
