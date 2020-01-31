using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayToResultsManager : MonoBehaviour
{

    // Integers
    private int highestCombo;
    private int totalPerfect;
    private int totalGood;
    private int totalEarly;
    private int totalMiss;
    private int score;
    private float totalScorePossible;
    private string percentage;
    private int totalHit;
    private int totalHitObjects;

    // Strings
    private string gradeAchieved;
    private string modUsed;
    private string songTitle;
    private string beatmapCreator;
    private string beatmapDifficulty;
    private string beatmapCreatedBy;

    // Keycodes
    KeyCode destroyObjectKey;

    // Scripts
    private ScoreManager scoreManager;
    private LoadAndRunBeatmap loadAndRunBeatmap;
    private PlayerSkillsManager playerSkillsManager;
    private FailAndRetryManager failAndRetryManager;

    // Properties

    public string BeatmapDifficulty
    {
        get { return beatmapDifficulty; }
    }

    public string BeatmapCreator
    {
        get { return beatmapCreator; }
    }

    public string SongTitle
    {
        get { return songTitle; }
    }

    // Get and update the grade achieved
    public string GradeAchieved
    {
        get { return gradeAchieved; }
        set { gradeAchieved = value; }
    }

    // Update the percentage
    public string Percentage
    {
        get { return percentage; }
        set { percentage = value; }
    }

    public int HighestCombo
    {
        get { return highestCombo; }
    }

    public int TotalPerfect
    {
        get { return totalPerfect; }
    }

    public int TotalGood
    {
        get { return totalGood; }
    }

    public int TotalEarly
    {
        get { return totalEarly; }
    }

    public int TotalMiss
    {
        get { return totalMiss; }
    }

    public int Score
    {
        get { return score; }
    }

    public int TotalHit
    {
        get { return totalHit; }
    }

    public int TotalHitObjects
    {
        get { return totalHitObjects; }
    }

    public float TotalScorePossible
    {
        get { return totalScorePossible; }
    }

    public string ModUsed
    {
        get { return modUsed; }
    }

    // Use this for initialization
    void Start()
    {

        // Initialize
        beatmapCreatedBy = "Beatmap created by ";
        destroyObjectKey = KeyCode.Escape;

        // Reference
        failAndRetryManager = FindObjectOfType<FailAndRetryManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();

        // Functions
        GetBeatmapInformation();

        // Don't destroy this game object
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        // If escape is pressed destroy this gameobject
        if (Input.GetKeyDown(destroyObjectKey))
        {
            Destroy(this.gameObject);
        }
    }

    // Transition
    public void TransitionScene()
    {
        // Get the results from the score manager before transitioning
        GetResults();
    }


    // Get the title, artist and creator for results page text
    private void GetBeatmapInformation()
    {
        // Get the beatmap information for passing to the results screen
        songTitle = Database.database.LoadedSongName + " [ " + Database.database.LoadedSongArtist + " ]";
        beatmapCreator = "Beatmap created by: " + Database.database.LoadedBeatmapCreator;
        beatmapDifficulty = Database.database.LoadedBeatmapDifficulty;
    }

    // Get the results from the scoreManager just before the game ends
    public void GetResults()
    {
        highestCombo = scoreManager.HighestCombo;
        totalPerfect = scoreManager.TotalPerfect;
        totalGood = scoreManager.TotalGood;
        totalEarly = scoreManager.TotalEarly;
        totalMiss = scoreManager.TotalMiss;
        score = (int)scoreManager.CurrentScore;
        totalScorePossible = scoreManager.TotalScorePossible;
        totalHit = scoreManager.TotalHit;
        totalHitObjects = scoreManager.TotalHitObjects;
        //modUsed = playerSkillsManager.ModSelected;
    }
}
