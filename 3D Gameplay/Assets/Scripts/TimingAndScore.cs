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
    public bool hitObjectHit; // Has the square been hit
    private float timeWhenHit; // The time when the object is hit
    public Vector3 hitObjectPosition; // The position of the object

    private ScoreManager scoreManager; // Manage score UI text
    private SoundController soundController; // Manage audio
    private ExplosionController explosionController; // Manage explosions
    private DestroyObject destroyObject; // Manages destroys

    private string objectTag; // The tag of the object

    // Use this for initialization
    void Start () {

        // References
        scoreManager = FindObjectOfType<ScoreManager>();
        soundController = FindObjectOfType<SoundController>();
        explosionController = FindObjectOfType<ExplosionController>();
        destroyObject = FindObjectOfType<DestroyObject>();

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

        // Get object tag
        objectTag = gameObject.tag;
	}
	
	// Update is called once per frame
	void Update () {

        // The timer increments per frame
        timer += Time.deltaTime;

        // Spawn miss explosion
        if (timer >= 1.19f)
        {
            hitObjectPosition = transform.position; // Get the position of the object
            explosionController.SpawnExplosion(hitObjectPosition, "Miss"); // Pass the position and spawn a miss particle system
            scoreManager.AddJudgement("MISS"); // Sets judgement to early
            scoreManager.ResetCombo(); // Reset combo as missed
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Timing check to calculate the type of hit on timing (perfect, miss)

            if (hitObjectHit == false)
            {
                // CHECK IF PLAYER HIT EARLY
                if (timer >= hitObjectStartTime && timer <= earlyJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    hitObjectPosition = transform.position; // Get the position of the object

                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    soundController.PlayHitSound(); // Play the hitsound

                    scoreManager.AddJudgement("EARLY"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += earlyScore; // Increase score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    DestroyHitObject(); // Destroy hit object
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= earlyJudgementTime && timer <= perfectJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    hitObjectPosition = transform.position; // Get the position of the object
                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    soundController.PlayHitSound(); // Play the hitsound

                    scoreManager.AddJudgement("GOOD"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += goodScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    DestroyHitObject(); // Destroy hit object
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= perfectJudgementTime && timer <= destroyedTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    hitObjectPosition = transform.position; // Get the position of the object
                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    soundController.PlayHitSound(); // Play the hitsound

                    scoreManager.AddJudgement("PERFECT");

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += perfectScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    DestroyHitObject(); // Destroy hit object
                }
            }
        }
    }

    // Destory hit object
    private void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
