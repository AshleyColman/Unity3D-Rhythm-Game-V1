using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GameplayToResultsManager : MonoBehaviour {

    private ScoreManager scoreManager;
    public LevelChanger levelChanger;
    public SongProgressBar songProgressBar;

    // UI variables
    public int highestCombo;
    public int totalPerfect;
    public int totalGood;
    public int totalEarly;
    public int totalMiss;
    public string gradeAchieved;
    public float totalScorePossible;
    public float score;
    public float Percentage;

    // Use this for initialization
    void Start () {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
	
	// Update is called once per frame
	void Update () {

        levelChanger = FindObjectOfType<LevelChanger>();

        if (levelChanger.currentLevelIndex == 4)
        {
            float AudioSourceTime = songProgressBar.songAudioSource.time;
            float AudioSourceLength = songProgressBar.songAudioSource.clip.length;

            if (AudioSourceTime >= AudioSourceLength)
            {
                // Get the results from the score manager before transitioning
                GetResults();
                TransitionToResultsPage();
            }
        }

        if (levelChanger.currentLevelIndex == 4 || levelChanger.currentLevelIndex == 5)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }



        if (levelChanger.currentLevelIndex == 5)
        {
            CalculateGradeAchieved();
        }
    }

    private void TransitionToResultsPage()
    {
        levelChanger.FadeToLevel(5);
    }

    // Get the results from the scoreManager just before the game ends
    public void GetResults()
    {
        highestCombo = scoreManager.highestCombo;
        totalPerfect = scoreManager.totalPerfect;
        totalGood = scoreManager.totalGood;
        totalEarly = scoreManager.totalEarly;
        totalMiss = scoreManager.totalMiss;
        score = scoreManager.score;
        totalScorePossible = scoreManager.totalScorePossible;
    }

    public void CalculateGradeAchieved()
    {
        //Percentage = ((totalScorePossible - score) * 100) / score;
        Percentage = (score / totalScorePossible) * 100;

        if (Percentage < 50)
        {
            gradeAchieved = "F";
        }
        else if (Percentage >= 50 && Percentage < 60)
        {
            gradeAchieved = "E";
        }
        else if (Percentage >= 60 && Percentage < 70)
        {
            gradeAchieved = "D";
        }
        else if (Percentage >= 70 && Percentage < 80)
        {
            gradeAchieved = "C";
        }
        else if (Percentage >= 80 && Percentage < 90)
        {
            gradeAchieved = "B";
        }
        else if (Percentage >= 90 && Percentage < 95)
        {
            gradeAchieved = "A";
        }
        else if (Percentage >= 95 && Percentage < 100)
        {
            gradeAchieved = "S";
        }
        else if (Percentage == 100)
        {
            gradeAchieved = "SS";
        }

    }
}
