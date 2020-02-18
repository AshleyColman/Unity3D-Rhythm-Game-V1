using UnityEngine;
using System.Collections;

// HIT OBJECT TIMING SCRIPT
public class TimingAndScore : MonoBehaviour
{

    // Strings
    private string objectTag, objectMissedTag; // The tag of the object
    private string perfectJudgement, goodJudgement, earlyJudgement, missJudgement; // Judgement values 

    // Integers
    private float timeWhenHit; // The time when the object is hit
    private float hitObjectTimer; // Timer for getting the time when the player hit the hit object key - judgement
    private float hitObjectStartTime; // The time when the note spawns
    private float perfectJudgementTime, earlyJudgementTime; // Judgement time values
    private float destroyedTime; // The late time, last possible hit time before input is cancelled
    private int perfectScoreValue, goodScoreValue, earlyScoreValue; // Score values for judgements 
    private int perfectHealthValue, goodHealthValue, earlyHealthValue, missHealthValue; // Healthbar values based on judgements

    // Bools
    private bool hitObjectHit; // Has the hit object been hit
    private bool canBeHit; // Controls whether the hit object can be hit 

    // Keycodes
    private KeyCode objectKey;
    private KeyCode alternateObjectKey;
    private KeyCode feverTimeActivateKey;

    // Scripts
    private ScriptManager scriptManager;

    // Animation
    public Animator hitObjectAnimator; // Animator 

    // Properties

    // Get or set canBeHit bool - controls allowing the hit object to be hit or not
    public bool CanBeHit
    {
        get { return canBeHit; }
        set { canBeHit = value; }
    }

    public float HitObjectTimer
    {
        get { return hitObjectTimer; }
    }

    // Initialize every time the object is reactivated
    void OnEnable()
    {
        canBeHit = false;
        hitObjectHit = false;
        timeWhenHit = 0;
        hitObjectTimer = 0;

        // Play animation
        hitObjectAnimator.Play("HitObject_FadeIn_Animation", 0, 0f);
    }

    // Use this for initialization
    void Start()
    {
        // Initialize scores
        // Scores start at 1/2/5 but are multiplied by the multiplier, default being 100
        // Real scores are: 100, 200, 500
        // Scores start as single values as floats cannot be uploaded to the server and therfore need to be multiplied by 100 to get the original value
        // To work with mods

        // Initialize
        canBeHit = false;
        hitObjectHit = false;
        earlyJudgementTime = 0.4f;
        perfectJudgementTime = 0.8f;
        destroyedTime = 1.1f;
        earlyScoreValue = 100;
        perfectScoreValue = 500;
        goodScoreValue = 250;
        hitObjectStartTime = 0f;
        timeWhenHit = 0;
        earlyHealthValue = -5;
        perfectHealthValue = 5;
        goodHealthValue = -5;
        missHealthValue = -5;
        perfectJudgement = "PERFECT";
        goodJudgement = "GOOD";
        earlyJudgement = "EARLY";
        missJudgement = "MISS";
        objectTag = gameObject.tag;
        objectMissedTag = "MISS";
        objectKey = KeyCode.None;
        alternateObjectKey = KeyCode.None;
        feverTimeActivateKey = KeyCode.Space;

        // References
        scriptManager = FindObjectOfType<ScriptManager>();

        // Functions
        //GetAndSetFadeSpeed(); // Get and set the fade speed for the hit object
        CheckTagType(); // Check the tag for input of the hit object
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the hit object timer used for judgements
        IncrementHitObjectTimer();

        // Check if it's time to destroy the hit object - miss
        CheckIfReadyToDestroy();

        // Check keypresses for judgements
        CheckJudgements();
    }

    private void DeactivateGameObject()
    {
        this.gameObject.SetActive(false);
    }


    // Check keypresses for judgements
    private void CheckJudgements()
    {
        // Check if any key has been pressed 
        if (Input.anyKeyDown)
        {
            // If the hit object can be hit
            if (canBeHit == true)
            {
                // If the user has pressed the right object key enable hit 
                if (Input.GetKeyDown(objectKey) || Input.GetKeyDown(alternateObjectKey))
                {
                    // If the hit object has not been hit yet
                    if (hitObjectHit == false)
                    {
                        // If the hit object has been hit before the destroyed time has been reached
                        if (hitObjectTimer < destroyedTime)
                        {
                            // Check if the player hit early judgement
                            CheckEarlyJudgement();
                            // Check if the player hit good judgement
                            CheckGoodJudgement();
                            // Check if the player hit perfect judgement
                            CheckPerfectJudgement();
                            // Increment the current combo
                            scriptManager.scoreManager.AddCombo();
                            // Get the time when the user pressed the key to hit the hit object
                            timeWhenHit = hitObjectTimer;
                            // Play hit sound
                            scriptManager.hitSoundManager.PlayHitSound();

                            //scriptManager.feverTimeManager.FillFeverSlider();

                            // Play follower hit animation
                            scriptManager.follower.PlayFollowerHitAnimation();

                            this.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    // Check if fever time key has been pressed
                    if (Input.GetKeyDown(feverTimeActivateKey))
                    {
                        // Do not count as miss
                    }
                    else
                    {
                        // Pressed incorrect key, reduce health
                    }
                }
            }
        }
    }

    private void SpawnExplosion()
    {
        /*
        switch (objectTag)
        {
            case "Left":
                scriptManager.explosionManager.SpawnExplosion(this.transform.position, "0");
                break;
        }
        */

       // scriptManager.explosionManager.SpawnExplosion(this.transform.position, "0");
    }

    // Check if the player hit early judgement
    private void CheckEarlyJudgement()
    {
        // Check if the player hit for early judgement
        if (hitObjectTimer >= hitObjectStartTime && hitObjectTimer <= earlyJudgementTime)
        {
            scriptManager.scoreManager.AddJudgement(earlyJudgement); // Display early judgement
            scriptManager.scoreManager.AddScore(earlyScoreValue); // Update the score
            SpawnExplosion();
        }
    }

    // Check if the player hit good judgement
    private void CheckGoodJudgement()
    {
        // Check if the player hit good judgement
        if (hitObjectTimer >= earlyJudgementTime && hitObjectTimer <= perfectJudgementTime)
        {
            scriptManager.scoreManager.AddJudgement(goodJudgement); // Sets judgement to good
            scriptManager.scoreManager.AddScore(goodScoreValue); // Update the score

            SpawnExplosion();
        }

    }

    // Check if the player hit perfect judgement
    private void CheckPerfectJudgement()
    {
        // Check if the player hit perfect judgement
        if (hitObjectTimer >= perfectJudgementTime && hitObjectTimer <= destroyedTime)
        {
            scriptManager.scoreManager.AddJudgement(perfectJudgement); // Display perfect judgement
            scriptManager.scoreManager.AddScore(perfectScoreValue); // Pass to score manager to update text

            SpawnExplosion();
        }
    }

    // Increment the hit object timer used for judgements
    private void IncrementHitObjectTimer()
    {
        // The timer increments per frame
        hitObjectTimer += Time.deltaTime;
    }

    // Check if it's time to destroy the hit object - miss
    private void CheckIfReadyToDestroy()
    {
        // Missed object, spawn the miss explosion, set the healthbar to miss value and play sound effect
        if (hitObjectTimer >= destroyedTime)
        {
            // Run the miss function
            MissedObject();
        }
    }

    // Do the miss object functions
    private void MissedObject()
    {
        // Pass the position and hit object type, spawn miss explosion
        //scriptManager.explosionManager.SpawnExplosion(this.transform.position, objectMissedTag);
        // Sets judgement to miss
        scriptManager.scoreManager.AddJudgement(missJudgement);
        // Reset combo as missed
        scriptManager.scoreManager.ResetCombo();
        // Play miss sound
        scriptManager.hitSoundManager.PlayMissSound();

        this.gameObject.SetActive(false);
    }

    // Assign the key to hit the hit object based on the objects tag color
    private void CheckTagType()
    {
        switch (objectTag)
        {
            case "Key1":
                objectKey = KeyCode.D;
                break;
            case "Key2":
                objectKey = KeyCode.F;
                break;
            case "Key3":
                objectKey = KeyCode.Space;
                break;
            case "Key4":
                objectKey = KeyCode.J;
                break;
            case "Key5":
                objectKey = KeyCode.K;
                break;
        }

        alternateObjectKey = KeyCode.F1;
    }

    // Check the fade speed selected from the song select menu, set the judgements based on the fade speed
    public void GetAndSetFadeSpeed()
    {
        /*
        // Set the fade speeds based on the fade speed selected
        switch (playerSkillsManager.FadeSpeedSelected)
        {
            case "SLOW":
                earlyJudgementTime = 1f;
                perfectJudgementTime = 1.8f;
                destroyedTime = 2.2f;
                break;
            case "NORMAL":
                earlyJudgementTime = 0.4f;
                perfectJudgementTime = 0.8f;
                destroyedTime = 1.2f;
                break;
            case "FAST":
                earlyJudgementTime = 0.2f;
                perfectJudgementTime = 0.4f;
                destroyedTime = 0.7f;
                break;
        }
        */
    }

}
