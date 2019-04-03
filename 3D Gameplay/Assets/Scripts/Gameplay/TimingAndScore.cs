using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingAndScore : MonoBehaviour {

    public float timer; // Timer for timing
    private float hitObjectStartTime; // The time when the note spawns
    public float perfectJudgementTime; // The end time for perfect hit
    public float earlyJudgementTime; // The early time
    public float destroyedTime; // The late time, last possible hit time before input is cancelled
    private int earlyScore; // The value of the early hits for score
    private int perfectScore; // The value of the perfect hits for score
    private int goodScore; // The value of the good hits for score
    public int playerTotalScore; // Total score for the player
    private int combo; // Total combo
    public bool hitObjectHit; // Has the square been hit
    private float timeWhenHit; // The time when the object is hit
    public Vector3 hitObjectPosition; // The position of the object

    private ScoreManager scoreManager; // Manage score UI text
    private HitSoundPreview hitSoundPreview; // Plays hit and miss sounds
    private ExplosionController explosionController; // Manage explosions
    private SongData songData; // Manages the songData
    private SpecialTimeManager specialTimeManager; // Manager special time objects and effects
    private Healthbar healthbar; // Manages the healthbar when hitting objects increasing or descreasing health
    private PlayerSkillsManager playerSkillsManager; // Manages all character skills equiped for gameplay

    private string objectTag; // The tag of the object
    private KeyCode objectKey = KeyCode.None;

    public bool canBeHit = false;
    public bool isSpecial = false; // Is it a special note during special time?

    private int earlyHealthValue; // The amount of health given when hitting early
    private int perfectHealthValue; // The amount of health given when hitting perfect
    private int goodHealthValue; // The amount of health given when hitting good
    private int missHealthValue; // The amount of health taken away if missed

    public int objectEarliestIndex;

    public string objectScoreType;

    // Use this for initialization
    void Start () {

        // References
        scoreManager = FindObjectOfType<ScoreManager>();
        explosionController = FindObjectOfType<ExplosionController>();
        songData = FindObjectOfType<SongData>();
        specialTimeManager = FindObjectOfType<SpecialTimeManager>();
        healthbar = FindObjectOfType<Healthbar>();
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();
        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();

        // Initalize hit object
        hitObjectStartTime = 0f; 

        // Initialize judgements
        earlyJudgementTime = 0.4f;
        perfectJudgementTime = 0.8f;
        destroyedTime = 1.2f;
        combo = 0;
        hitObjectHit = false;

        // Initialize scores
        earlyScore = 100; 
        perfectScore = 500; 
        goodScore = 250; 

        playerTotalScore = 0;
        timeWhenHit = 0;

        // Get object tag
        objectTag = gameObject.tag;

        // Initialize health bar values when hit
        earlyHealthValue = 5;
        perfectHealthValue = 15;
        goodHealthValue = 10;
        missHealthValue = -10;

        // Get and set the fade speed for the hit object
        GetAndSetFadeSpeed();
	}
	
	// Update is called once per frame
	void Update () {

        // Check if special time is now
        CheckIsSpecialTime();

        // Check the tag for input of the hit object
        CheckTagType();

        // Assign the type, normal or special 
        CheckObjectScoreType();

        // The timer increments per frame
        timer += Time.deltaTime;

        // Spawn miss explosion
        if (timer >= destroyedTime)
        {
            healthbar.healthBarValue = missHealthValue; // Update the healthbar with the miss value
            healthbar.assignHealthBarLerp = true; // Assign a new lerp position for the health bar
            hitObjectPosition = transform.position; // Get the position of the object
            explosionController.SpawnExplosion(hitObjectPosition, "Miss"); // Pass the position and spawn a miss particle system
            scoreManager.AddJudgement("MISS"); // Sets judgement to early
            scoreManager.ResetCombo(); // Reset combo as missed
            DestroyHitObject(); // Destroy the hit object
            hitSoundPreview.PlayMissSound(); // Play the miss sound effect
        }

        // If the user has pressed the right object key enable hit 
        if (Input.GetKeyDown(objectKey) && canBeHit == true)
        {
            // Timing check to calculate the type of hit on timing (perfect, miss)

            if (hitObjectHit == false)
            {
                // CHECK IF PLAYER HIT EARLY
                if (timer >= hitObjectStartTime && timer <= earlyJudgementTime)
                {
                    CheckIsSpecial(); // Check if the note is special

                    hitObjectPosition = transform.position; // Get the position of the object

                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    hitSoundPreview.PlayHitSound(); // Play the hitsound

                    scoreManager.AddJudgement("EARLY"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += earlyScore; // Increase score
                    scoreManager.AddScore(playerTotalScore, objectScoreType); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    healthbar.healthBarValue = earlyHealthValue; // Update the healthbar with the miss value
                    healthbar.assignHealthBarLerp = true; // Assign a new lerp position for the health bar

                    DestroyHitObject(); // Destroy hit object
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= earlyJudgementTime && timer <= perfectJudgementTime)
                {
                    CheckIsSpecial(); // Check if the note is special

                    hitObjectPosition = transform.position; // Get the position of the object
                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    hitSoundPreview.PlayHitSound(); // Play the hit sound

                    scoreManager.AddJudgement("GOOD"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += goodScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore, objectScoreType); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    healthbar.healthBarValue = goodHealthValue; // Update the healthbar with the miss value
                    healthbar.assignHealthBarLerp = true; // Assign a new lerp position for the health bar

                    DestroyHitObject(); // Destroy hit object
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= perfectJudgementTime && timer <= destroyedTime)
                {
                    CheckIsSpecial(); // Check if the note is special

                    hitObjectPosition = transform.position; // Get the position of the object
                    explosionController.SpawnExplosion(hitObjectPosition, objectTag); // Pass the position and spawn a particle system

                    hitSoundPreview.PlayHitSound(); // Play the hitsound

                    scoreManager.AddJudgement("PERFECT");

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += perfectScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore, objectScoreType); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit

                    healthbar.healthBarValue = perfectHealthValue; // Update the healthbar with the miss value
                    healthbar.assignHealthBarLerp = true; // Assign a new lerp position for the health bar

                    DestroyHitObject(); // Destroy hit object
                }
            }
        }
    }

    // Check object score type
    private void CheckObjectScoreType()
    {
        if (isSpecial == true)
        {
            objectScoreType = "SPECIAL";
        }
        else
        {
            objectScoreType = "NORMAL";
        }
    }

    // Destory hit object
    private void DestroyHitObject()
    {
        // Destroy hit object
        Destroy(gameObject);
    }

    private void CheckTagType()
    {
        
        if (objectTag == "Green")
        {
            objectKey = KeyCode.S;
        }
        else if (objectTag == "Yellow")
        {
            objectKey = KeyCode.D;
        }
        else if (objectTag == "Orange")
        {
            objectKey = KeyCode.F;
        }
        else if (objectTag == "Blue")
        {
            objectKey = KeyCode.J;
        }
        else if (objectTag == "Purple")
        {
            objectKey = KeyCode.K;
        }
        else if (objectTag == "Red")
        {
            objectKey = KeyCode.L;
        }
        else if (objectTag == "Yellow")
        {
            objectKey = KeyCode.E;
        }
    }
    
    // Set as earliest note
    public void CanBeHit()
    {
        canBeHit = true;
    }

    // Is the note special during special time?
    public void CheckIsSpecial()
    {
        if (isSpecial == true)
        {
            specialTimeManager.BackgroundAnimation();
        }
    }

    // Check if it's special time
    public void CheckIsSpecialTime()
    {
        if(specialTimeManager.isSpecialTime == true)
        {
            isSpecial = true;
        }
         
    }

    // Check the fade speed selected from the song select menu, set the judgements based on the fade speed
    public void GetAndSetFadeSpeed()
    {
        string fadeSpeedSelected = playerSkillsManager.fadeSpeedSelected;

        // Set the fade speeds based on the fade speed selected
        switch (fadeSpeedSelected)
        {
            case "SLOW":
                Debug.Log("SLOW SPEED SET");
                earlyJudgementTime = 1f;
                perfectJudgementTime = 1.8f;
                destroyedTime = 2.2f;
                break;
            case "NORMAL":
                Debug.Log("NORMAL");
                earlyJudgementTime = 0.4f;
                perfectJudgementTime = 0.8f;
                destroyedTime = 1.2f;
                break;
            case "FAST":
                Debug.Log("FAST");
                earlyJudgementTime = 0.2f;
                perfectJudgementTime = 0.4f;
                destroyedTime = 0.7f;
                break;
        }
    }
}
