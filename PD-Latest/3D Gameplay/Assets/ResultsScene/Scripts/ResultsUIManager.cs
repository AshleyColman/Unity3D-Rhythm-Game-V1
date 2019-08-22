using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIManager : MonoBehaviour {

    // UI
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI beatmapCreatorText;
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI earlyText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI scoreText;
    public Button easyButton, advancedButton, extraButton;

    // Color
    public Color pColor, sColor, aColor, bColor, cColor, dColor, eColor, fColor;
    

    // Gameobjects
    public GameObject tripleTimeMod, doubleTimeMod, halfTimeMod, instantDeathMod, noFailMod, modGlow;

    // Particles
    public ParticleSystem pRankExplosion, sRankExplosion, aRankExplosion, bRankExplosion, cRankExplosion, dRankExplosion, eRankExplosion, fRankExplosion;

    // Integers
    private float gradePercentage;
    private float numberAnimationDuration; // Duration for lerping the score
    private float gradeDisplayTimer;
    private float gradeDisplayTime;

    // Bools
    private bool hasLoadedResults;

    // Scripts
    private GameplayToResultsManager gameplayToResultsManager;
    private PlayerSkillsManager playerSkillsManager;

    // Properties
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
        // Reference
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        gameplayToResultsManager = FindObjectOfType<GameplayToResultsManager>();

        // Disable all difficulty buttons
        easyButton.gameObject.SetActive(false);
        advancedButton.gameObject.SetActive(false);
        extraButton.gameObject.SetActive(false);

        // Disable all rank buttons
        pRankExplosion.gameObject.SetActive(false);
        sRankExplosion.gameObject.SetActive(false);
        aRankExplosion.gameObject.SetActive(false);
        bRankExplosion.gameObject.SetActive(false);
        cRankExplosion.gameObject.SetActive(false);
        dRankExplosion.gameObject.SetActive(false);
        eRankExplosion.gameObject.SetActive(false);
        fRankExplosion.gameObject.SetActive(false);

        // Get the beatmap difficulty button and activate
        DisplayBeatmapDifficultyButton();

        // Initialize
        gradeDisplayTime = 1f;
        gradeDisplayTimer = 0f;
        numberAnimationDuration = 1f;

        // Functions
        StartCoroutine(UpdateText());
        DisplayBeatmapInformation(); // Display beatmap information                            
        CheckMods(); // Check the mods used
        DisplayName();// Display the player name on screen
    }

    void Update()
    {
        // Increment timer
        gradeDisplayTimer += Time.deltaTime;

        // Display the grade icon after a time limit
        if (gradeDisplayTimer >= gradeDisplayTime)
        {
            DisplayGradeIcon();
        }
    }

    // Display beatmap information
    private void DisplayBeatmapInformation()
    {
        // Update the song title text
        songTitleText.text = gameplayToResultsManager.SongTitle;
        // Update the beatmap creator text
        beatmapCreatorText.text = gameplayToResultsManager.BeatmapCreator;
    }

    private void DisplayBeatmapDifficultyButton()
    {
        switch (gameplayToResultsManager.BeatmapDifficulty)
        {
            case "easy":
                easyButton.gameObject.SetActive(true);
                break;
            case "advanced":
                advancedButton.gameObject.SetActive(true);
                break;
            case "extra":
                extraButton.gameObject.SetActive(true);
                break;
        }
    }

    private void DisplayGradeIcon()
    {
        switch (gameplayToResultsManager.GradeAchieved)
        {
            case "P":
                pRankExplosion.gameObject.SetActive(true);
                break;
            case "S":
                sRankExplosion.gameObject.SetActive(true);
                break;
            case "A":
                aRankExplosion.gameObject.SetActive(true);
                break;
            case "B":
                bRankExplosion.gameObject.SetActive(true);
                break;
            case "C":
                cRankExplosion.gameObject.SetActive(true);
                break;
            case "D":
                dRankExplosion.gameObject.SetActive(true);
                break;
            case "E":
                eRankExplosion.gameObject.SetActive(true);
                break;
            case "F":
                fRankExplosion.gameObject.SetActive(true);
                break;
            default:
                fRankExplosion.gameObject.SetActive(true);
                break;
        }
    }

    private void DisplayName()
    {
        // If logged in get the username
        if (MySQLDBManager.loggedIn)
        {
            usernameText.text = MySQLDBManager.username.ToString();
        }
        else
        {
            usernameText.text = "GUEST";
        }
    }

    // Update text
    IEnumerator UpdateText()
    {
        // Lerp time
        float i = 0f;

        // Lerp
        while (i <= 1f)
        {
            // Increment
            i += Time.deltaTime / numberAnimationDuration;

            // Lerp values
            int newCombo = (int)Mathf.Lerp(0f, gameplayToResultsManager.HighestCombo, i);
            int newPerfect = (int)Mathf.Lerp(0f, gameplayToResultsManager.TotalPerfect, i);
            int newGood = (int)Mathf.Lerp(0f, gameplayToResultsManager.TotalGood, i);
            int newEarly = (int)Mathf.Lerp(0f, gameplayToResultsManager.TotalEarly, i);
            int newMiss = (int)Mathf.Lerp(0f, gameplayToResultsManager.TotalMiss, i);
            int newScore = (int)Mathf.Lerp(0f, gameplayToResultsManager.Score, i);

            // Update text
            comboText.text = newCombo + "/" + gameplayToResultsManager.TotalHitObjects;
            perfectText.text = newPerfect.ToString();
            goodText.text = newGood.ToString();
            earlyText.text = newEarly.ToString();
            missText.text = newMiss.ToString();

            if (gameplayToResultsManager.Percentage == "" || gameplayToResultsManager.Percentage == null)
            {
                gameplayToResultsManager.Percentage = "0";
            }

            percentageText.text = gameplayToResultsManager.Percentage + "%";
            CheckScore(newScore);

            yield return null;
        }
    }

    // Check mod used and display mod used
    private void CheckMods()
    {
        // Enable the mod used gameobject based on the mod selected
        switch (playerSkillsManager.ModSelected)
        {
            case "TRIPLE TIME":
                tripleTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "DOUBLE TIME":
                doubleTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "HALF TIME":
                halfTimeMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "NO FAIL":
                noFailMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
            case "INSTANT DEATH":
                instantDeathMod.gameObject.SetActive(true);
                modGlow.gameObject.SetActive(true);
                break;
        }

    }
    
    // Check score and add 0's
    private void CheckScore(int scorePass)
    {
        // Get the score
        //Sint score = gameplayToResultsManager.score;

        // Check the score and add the 0's according to the type
        if (scorePass < 1000)
        {
            scoreText.text = "00000" + scorePass.ToString();
        }
        if (scorePass >= 1000 && scorePass < 10000)
        {
            scoreText.text = "0000" + scorePass.ToString();
        }
        if (scorePass >= 10000 && scorePass < 100000)
        {
            scoreText.text = "000" + scorePass.ToString();
        }
        if (scorePass >= 100000 && scorePass < 1000000)
        {
            scoreText.text = "00" + scorePass.ToString();
        }
        if (scorePass >= 1000000 && scorePass < 10000000)
        {
            scoreText.text = "0" + scorePass.ToString();
        }
        if (scorePass >= 10000000 && scorePass < 100000000)
        {
            scoreText.text = scorePass.ToString();
        }
    }
}
