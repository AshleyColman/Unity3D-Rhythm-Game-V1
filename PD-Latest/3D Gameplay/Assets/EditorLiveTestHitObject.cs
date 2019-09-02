using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLiveTestHitObject : MonoBehaviour {


    // Strings
    private string objectTag, objectMissedTag; // The tag of the object
    private string perfectJudgement, goodJudgement, earlyJudgement, missJudgement; // Judgement values 

    // Integers
    private float timeWhenHit; // The time when the object is hit
    public float hitObjectTimer; // Timer for getting the time when the player hit the hit object key - judgement
    private float hitObjectStartTime; // The time when the note spawns
    private float perfectJudgementTime, earlyJudgementTime; // Judgement time values
    private float destroyedTime; // The late time, last possible hit time before input is cancelled
    private int perfectScoreValue, goodScoreValue, earlyScoreValue; // Score values for judgements 
    private int perfectHealthValue, goodHealthValue, earlyHealthValue, missHealthValue; // Healthbar values based on judgements

    // Bools
    private bool hitObjectHit; // Has the hit object been hit
    private bool canBeHit; // Controls whether the hit object can be hit 

    // Vectors
    private Vector3 hitObjectPosition; // The position of the object

    // Keycodes
    private KeyCode objectKey;
    private KeyCode alternateObjectKey;
    private KeyCode feverTimeActivateKey;

    // Scripts
    private HitSoundPreview hitSoundPreview; // Plays hit and miss sounds
    private ExplosionController explosionController; // Manage explosions

    // Animation
    private Animator hitObjectAnimator; // Animator 

    // Properties

    // Get or set canBeHit bool - controls allowing the hit object to be hit or not
    public bool CanBeHit
    {
        get { return canBeHit; }
        set { canBeHit = value; }
    }


    // Initialize every time the object is reactivated
    void OnEnable()
    {
        canBeHit = false;
        hitObjectHit = false;
        timeWhenHit = 0;
        hitObjectTimer = 0;
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
        earlyScoreValue = 1;
        perfectScoreValue = 5;
        goodScoreValue = 2;
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
        hitObjectPosition = transform.position; // Set the position of the object
        objectTag = gameObject.tag;
        objectMissedTag = objectTag + "Miss";
        objectKey = KeyCode.None;
        alternateObjectKey = KeyCode.None;
        feverTimeActivateKey = KeyCode.Space;

        // References
        explosionController = FindObjectOfType<ExplosionController>();
        hitSoundPreview = FindObjectOfType<HitSoundPreview>();

        // Get the animator reference
        hitObjectAnimator = this.gameObject.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        // Check the tag for input of the hit object
        CheckTagType();

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

                            timeWhenHit = hitObjectTimer; // Get the time when the user pressed the key to hit the hit object
                            hitSoundPreview.PlayHitSound(); // Play the hit sound effect

                            this.gameObject.SetActive(false);

                            //DestroyHitObject(); // Destroy hit object
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
                }
            }
        }
    }




    // Check if the player hit early judgement
    private void CheckEarlyJudgement()
    {
        // Check if the player hit for early judgement
        if (hitObjectTimer >= hitObjectStartTime && hitObjectTimer <= earlyJudgementTime)
        {
            hitObjectPosition = transform.position;
            explosionController.SpawnExplosion(hitObjectPosition, "EARLY");
        }
    }

    // Check if the player hit good judgement
    private void CheckGoodJudgement()
    {
        // Check if the player hit good judgement
        if (hitObjectTimer >= earlyJudgementTime && hitObjectTimer <= perfectJudgementTime)
        {
            hitObjectPosition = transform.position;
            explosionController.SpawnExplosion(hitObjectPosition, "GOOD");
        }
    }

    // Check if the player hit perfect judgement
    private void CheckPerfectJudgement()
    {
        // Check if the player hit perfect judgement
        if (hitObjectTimer >= perfectJudgementTime && hitObjectTimer <= destroyedTime)
        {
            hitObjectPosition = transform.position;
            explosionController.SpawnExplosion(hitObjectPosition, "PERFECT");
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
        hitObjectPosition = transform.position;
        explosionController.SpawnExplosion(hitObjectPosition, objectMissedTag); // Pass the position and hit object type, spawn miss explosion

        this.gameObject.SetActive(false);
        //DestroyHitObject(); // Destroy the hit object
    }

    // Destroy hit object
    private void DestroyHitObject()
    {
        // Destroy hit object
        Destroy(gameObject);
    }

    // Assign the key to hit the hit object based on the objects tag color
    private void CheckTagType()
    {
        switch (objectTag)
        {
            case "Green":
                objectKey = KeyCode.S;
                alternateObjectKey = KeyCode.Z;
                break;
            case "Orange":
                objectKey = KeyCode.F;
                alternateObjectKey = KeyCode.C;
                break;
            case "Yellow":
                objectKey = KeyCode.D;
                alternateObjectKey = KeyCode.X;
                break;
            case "Blue":
                objectKey = KeyCode.J;
                alternateObjectKey = KeyCode.M;
                break;
            case "Purple":
                objectKey = KeyCode.K;
                alternateObjectKey = KeyCode.Comma;
                break;
            case "Red":
                objectKey = KeyCode.L;
                alternateObjectKey = KeyCode.Period;
                break;
        }
    }
}
