using UnityEngine;
using TMPro;

public class FailAndRetryManager : MonoBehaviour {

    // UI
    public GameObject failedCanvas; // Failed game canvas that appears when the player has failed
    public TextMeshProUGUI failScoreText, failComboText, failPerfectText, failGoodText, failEarlyText, failMissText, failedAtTimeText; // Fail statistic text

    // Animation
    public Animator failedCanvasAnimator; // Animator of the failed canvas

    // Audio
    public AudioSource menuSFXAudioSource; // Audiosources for sfx
    public AudioClip failSoundClip; // Fail sound that plays when the user has failed - health reached 0

    // Bools
    private bool hasFailed, canFail, hasPressedRetryKey; // Fail control

    // Strings
    private string failScoreValue, failComboValue, failPerfectValue, failGoodValue, failEarlyValue, failMissValue, failedAtTimeValue; // Fail statistic values

    // Scripts
    private SongProgressBar songProgressBar;
    private PlayerSkillsManager playerSkillsManager;
    private ScoreManager scoreManager;
    private LevelChanger levelChanger;

    // Properties

    // Get whether the player has failed
    public bool HasFailed
    {
        get { return hasFailed; }
    }


    // Use this for initialization
    void Start () {

        // Initialize
        hasPressedRetryKey = false; 
        hasFailed = false; 
        failScoreValue = "Score: ";
        failComboValue = "Combo: x";
        failPerfectValue = "Perfect: ";
        failGoodValue = "Good: ";
        failEarlyValue = "Early: ";
        failMissValue = "Miss: ";
        failedAtTimeValue = "Failed at: ";

        // Reference
        levelChanger = FindObjectOfType<LevelChanger>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        songProgressBar = FindObjectOfType<SongProgressBar>();

        // Functionss
        CheckModsEquiped(); // Check the mods equiped, set canFail based on the mods equiped
	}

    // Update the fail canvas statistic text
    private void UpdateFailStatisticText()
    {
        failScoreText.text = failScoreValue + scoreManager.CurrentScore.ToString();
        failComboText.text =  failComboValue + scoreManager.HighestCombo.ToString();
        failPerfectText.text = failPerfectValue + scoreManager.TotalPerfect.ToString();
        failGoodText.text = failGoodValue + scoreManager.TotalGood.ToString();
        failEarlyText.text = failEarlyValue + scoreManager.TotalEarly.ToString();
        failMissText.text = failMissValue + scoreManager.TotalMiss.ToString();
        failedAtTimeText.text = failedAtTimeValue + songProgressBar.SongTimePosition.ToString("F2");
    }

    // Check the character mods selected from the song select scene
    private void CheckModsEquiped()
    {
        // Check the player manager script to see if the no fail mod has been selected
        if (playerSkillsManager.ModSelected == playerSkillsManager.NoFailTextValue)
        {
            // If the no fail mod has been selected prevent the player from failing
            canFail = false;
        }
    }

    // The user has failed
    public void PlayerHasFailed()
    {
        // If the user can fail
        if (canFail == true)
        {
            // Set to true as the user has now failed
            hasFailed = true;
            // Activate the failed screen
            ActivateFailedScreen();
            // Update the fail screen statistic text with the play values
            UpdateFailStatisticText();
            // Play the fail sound
            PlayFailSound();
        }
    }

    // Activate the failed screen
    public void ActivateFailedScreen()
    {
        // Activate the fail canvas
        failedCanvas.gameObject.SetActive(true);

        // Play the animation
        failedCanvasAnimator.Play("FailedCanvasAnimation");
    }

    // Play the fail sound
    private void PlayFailSound()
    {
        menuSFXAudioSource.PlayOneShot(failSoundClip);
    }
}
