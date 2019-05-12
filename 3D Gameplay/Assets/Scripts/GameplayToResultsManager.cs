using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GameplayToResultsManager : MonoBehaviour {

    private ScoreManager scoreManager;
    public LevelChanger levelChanger;
    private LoadAndRunBeatmap loadAndRunBeatmap;
    private PlayerSkillsManager playerSkillsManager;

    // UI variables
    public int highestCombo;
    public int totalPerfect;
    public int totalGood;
    public int totalEarly;
    public int totalMiss;
    public string gradeAchieved;
    public float totalScorePossible;
    public int score;
    public float Percentage;
    public float totalHit;
    public float totalSpecial;
    public float totalHitObjects;
    public string modUsed;

    float AudioSourceLength;
    float AudioSourceTime;

    // Song information for results page 
    public string songTitle;
    public string beatmapCreator;

    private bool allHitObjectsHaveBeenHit; // Have all the hit objects been hit? Used for going to the results screen if they have

    // Use this for initialization
    void Start () {
        scoreManager = FindObjectOfType<ScoreManager>();
        loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
    }
	
	// Update is called once per frame
	void Update () {

        levelChanger = FindObjectOfType<LevelChanger>();

        GetBeatmapInformation();

        if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex)
        {
            // Check if the last hit object has been hit
            allHitObjectsHaveBeenHit = loadAndRunBeatmap.CheckIfAllHitObjectsHaveBeenHit();

            if (allHitObjectsHaveBeenHit == true)
            {
                // Get the results from the score manager before transitioning
                GetResults();
                TransitionToResultsPage();
            }


        }

        if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex || levelChanger.currentLevelIndex == levelChanger.resultsSceneIndex)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }



        if (levelChanger.currentLevelIndex == levelChanger.resultsSceneIndex)
        {
            CalculateGradeAchieved();
        }
    }

    // Get the title, artist and creator for results page text
    private void GetBeatmapInformation()
    {
        // Get the beatmap information for passing to the results screen
        songTitle = Database.database.loadedSongName + " [ " + Database.database.loadedSongArtist + " ]";
        beatmapCreator = "Beatmap created by: " + Database.database.loadedBeatmapCreator;
    }

    private void TransitionToResultsPage()
    {
        levelChanger.FadeToLevel(levelChanger.resultsSceneIndex);
    }


    // Get mod used
    private string GetModUsed()
    {
        if (playerSkillsManager.tripleTimeSelected == true)
        {
            modUsed = "TRIPLE TIME";
        }
        else if (playerSkillsManager.doubleTimeSelected == true)
        {
            modUsed = "DOUBLE TIME";
        }
        else if (playerSkillsManager.halfTimeSelected == true)
        {
            modUsed = "HALF TIME";
        }
        else if (playerSkillsManager.judgementPlusSelected == true)
        {
            modUsed = "JUDGEMENT+";
        }
        else if (playerSkillsManager.noFailSelected == true)
        {
            modUsed = "NO FAIL";
        }
        else if (playerSkillsManager.instantDeathSelected == true)
        {
            modUsed = "INSTANT DEATH";
        }
        else
        {
            modUsed = "";
        }

        return modUsed;
    }

    // Get the results from the scoreManager just before the game ends
    public void GetResults()
    {
        highestCombo = scoreManager.highestCombo;
        totalPerfect = scoreManager.totalPerfect;
        totalGood = scoreManager.totalGood;
        totalEarly = scoreManager.totalEarly;
        totalMiss = scoreManager.totalMiss;
        score = scoreManager.currentScore;
        totalScorePossible = scoreManager.totalScorePossible;
        totalHit = scoreManager.totalHit;
        totalSpecial = scoreManager.totalSpecial;
        totalHitObjects = scoreManager.totalHitObjects;
        modUsed = GetModUsed();
    }

    public void CalculateGradeAchieved()
    {
        //Percentage = ((totalScorePossible - score) * 100) / score;
        Percentage = (score / totalScorePossible) * 100;


        if (Percentage < 60)
        {
            gradeAchieved = "F";
        }
        else if (Percentage >= 60 && Percentage < 70)
        {
            gradeAchieved = "E";
        }
        else if (Percentage >= 70 && Percentage < 80)
        {
            gradeAchieved = "D";
        }
        else if (Percentage >= 80 && Percentage < 90)
        {
            gradeAchieved = "C";
        }
        else if (Percentage >= 90 && Percentage < 95)
        {
            gradeAchieved = "B";
        }
        else if (Percentage >= 95 && Percentage < 98)
        {
            gradeAchieved = "A";
        }
        else if (Percentage >= 98 && Percentage < 100)
        {
            gradeAchieved = "S";
        }
        else if (Percentage == 100)
        {
            gradeAchieved = "P";
        }

    }
}
