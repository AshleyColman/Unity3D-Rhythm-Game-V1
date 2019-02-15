using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingAndScore : MonoBehaviour {

    public float timer; // Timer for timing
    private float squareStartTime; // The time when the note spawns
    private float squarePerfectTime; // The end time for perfect hit
    private float squareEarlyTime; // The early time
    private float squareLateTime; // The late time, last possible hit time before input is cancelled
    private int squareEarlyScore; // The value of the early hits for score
    private int squarePerfectScore; // The value of the perfect hits for score
    private int squareLateScore; // The value of the late hits for score
    public int playerTotalScore; // Total score for the player
    private int combo; // Total combo
    public Text comboText; // The combo text on UI
    public Text judgementText; // The judgement text such as PERFECT, MISS
    public Text playerTotalScoreText; // The score text
    bool squareHit; // Has the square been hit

	// Use this for initialization
	void Start () {

        // Initalize square values
        squareStartTime = 0f; 
        squareEarlyTime = 0.6f;
        squarePerfectTime = 1.1f;
        squareLateTime = 1.2f;
        combo = 0;
        squareHit = false;

        squareEarlyScore = 100; // 100 points for early hits
        squarePerfectScore = 1000; // 1000 points for perfect hits
        squareLateScore = 500; // 500 points for late hits

        playerTotalScore = 0; // Set to 0 at the start
	}
	
	// Update is called once per frame
	void Update () {

        // The timer increments per frame
        timer += Time.deltaTime;

        // RESET TIMER FOR TESTING IF IT GOES ABOVE LARGE TIME
        if (timer > 1)
        {
            timer = 0;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Timing check to calculate the type of hit on timing (perfect, miss)

            // CHECK IF PLAYER HIT EARLY
            if (timer >= squareStartTime && timer <= squareEarlyTime)
            {
                judgementText.text = "EARLY"; // Sets judgement to early
                judgementText.color = Color.red; // Changes text color to red
                squareHit = true; // The square has been hit
                combo++; // Increase combo
                comboText.text = combo.ToString(); // Send current combo to update the UI text
                playerTotalScore += squareEarlyScore; // Add early score value to the players current score
                playerTotalScoreText.text = "Score: " + playerTotalScore; // Update the players score for the UI
            }

            // CHECK IF PLAYER HIT PERFECT
            if (timer > squareEarlyTime && timer <= squarePerfectTime)
            {
                judgementText.text = "PERFECT"; // Sets judgement to perfect
                judgementText.color = Color.yellow; // Changes text color to yellow
                squareHit = true; // The square has been hit
                combo++; // Increase combo
                comboText.text = combo.ToString(); // Send current combo to update the UI text
                playerTotalScore += squarePerfectScore; // Add perfect score value to the players current score
                playerTotalScoreText.text = "Score: " + playerTotalScore; // Update the players score for the UI
            }

            /*

            Add this when more notes are added, does not work for testing currently
             
            // CHECK IF PLAYER HIT LATE
            if (timer > squarePerfectTime && timer <= squareLateTime)
            {
                judgementText.text = "LATE"; // Sets judgement to late
                judgementText.color = Color.magenta; // Changes text color
                squareHit = true; // The square has been hit
                combo++; // Increase combo
                comboText.text = combo.ToString(); // Send current combo to update the UI text
                playerTotalScore += squareLateScore; // Add perfect score value to the players current score
                playerTotalScoreText.text = "Score: " + playerTotalScore; // Update the players score for the UI
            }
            */
        }

	}
}
