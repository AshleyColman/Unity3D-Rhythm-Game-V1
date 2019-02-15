using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingAndScore : MonoBehaviour {

    public float timer; // Timer for timing
    private float hitObjectStartTime; // The time when the note spawns
    private float perfectJudgementTime; // The end time for perfect hit
    private float earlyJudgementTime; // The early time
    private float destroyedTime; // The late time, last possible hit time before input is cancelled
    private int earlyScore; // The value of the early hits for score
    private int perfectScore; // The value of the perfect hits for score
    private int goodScore; // The value of the good hits for score
    public int playerTotalScore; // Total score for the player
    private int combo; // Total combo
    public Text comboText; // The combo text on UI
    public Text judgementText; // The judgement text such as PERFECT, MISS
    public Text playerTotalScoreText; // The score text
    public Text timeWhenHitText; // Time when the user pressed down the key and hit a note
    bool hitObjectHit; // Has the square been hit
    public AudioSource clickSound; // The sound that plays when the button is pressed
    public ParticleSystem particles; // The particles that appear when the button is pressed

    private float timeWhenHit;

    // Use this for initialization
    void Start () {

        // Initalize hit object
        hitObjectStartTime = 0f; 

        // Initialize judgements
        earlyJudgementTime = 0.4f;
        perfectJudgementTime = 0.8f;
        destroyedTime = 1.2f;
        combo = 0;
        hitObjectHit = false;

        // Initialize scores
        earlyScore = 1000; 
        perfectScore = 5000; 
        goodScore = 2500; 

        playerTotalScore = 0;


        timeWhenHit = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // The timer increments per frame
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Timing check to calculate the type of hit on timing (perfect, miss)

            if (hitObjectHit == false)
            {
                // CHECK IF PLAYER HIT EARLY
                if (timer >= hitObjectStartTime && timer <= earlyJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    particles.Play(); // Play the particle animation
                    clickSound.Play(); // Play the click sound effect

                    judgementText.text = "EARLY"; // Sets judgement to early
                    judgementText.color = Color.red; // Changes text color to red
                  
                    combo++; // Increase combo
                    comboText.text = "x " + combo.ToString(); // Send current combo to update the UI text

                    playerTotalScore += earlyScore; // Add early score value to the players current score
                    playerTotalScoreText.text = playerTotalScore.ToString(); // Update the players score for the UI

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= earlyJudgementTime && timer <= perfectJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    particles.Play(); // Play the particle animation
                    clickSound.Play(); // Play the click sound effect

                    judgementText.text = "GOOD"; // Sets judgement to early
                    judgementText.color = Color.blue; // Changes text color to red

                    combo++; // Increase combo
                    comboText.text = "x " + combo.ToString(); // Send current combo to update the UI text

                    playerTotalScore += goodScore; // Add early score value to the players current score
                    playerTotalScoreText.text = playerTotalScore.ToString(); // Update the players score for the UI

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= perfectJudgementTime && timer <= destroyedTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    particles.Play(); // Play the particle animation
                    clickSound.Play(); // Play the click sound effect

                    judgementText.text = "PERFECT"; // Sets judgement to early
                    judgementText.color = Color.yellow; // Changes text color to red

                    combo++; // Increase combo
                    comboText.text = "x " + combo.ToString(); // Send current combo to update the UI text

                    playerTotalScore += perfectScore; // Add early score value to the players current score
                    playerTotalScoreText.text = playerTotalScore.ToString(); // Update the players score for the UI

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note
                }
            }

        }

	}
}
