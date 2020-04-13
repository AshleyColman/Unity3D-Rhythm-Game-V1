using UnityEngine;
using UnityEngine.UI;

public class HitObject : MonoBehaviour
{
    #region Variables
    // UI
    public Image colorImage;

    // Integers
    private float timeWhenHit;
    private float hitObjectTimer;
    private const float HIT_OBJECT_START_TIME = 0, PERFECT_JUDGEMENT_TIME = 0.8f, EARLY_JUDGEMENT_TIME = 0.4f,
        MISS_TIME = 1.1f;

    // Bools
    protected bool hitObjectHit;
    public bool canBeHit;

    // Scripts
    protected ScriptManager scriptManager;

    // Animation
    public Animator hitObjectAnimator;
    #endregion

    #region Properties
    public bool CanBeHit
    {
        get { return canBeHit; }
        set { canBeHit = value; }
    }

    public float HitObjectTimer
    {
        get { return hitObjectTimer; }
    }
    #endregion

    #region Functions
    // Initialize every time the object is reactivated
    protected virtual void OnEnable()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        // Assign color
        AssignColor();

        // Play animation
        hitObjectAnimator.Play("HitObject_FadeIn_Animation", 0, 0f);
    }

    private void OnDisable()
    {
        // Initialize
        canBeHit = false;
        hitObjectHit = false;
        timeWhenHit = 0;
        hitObjectTimer = 0;
    }

    protected virtual void Start()
    {
        // Initialize
        canBeHit = false;
        hitObjectHit = false;
        timeWhenHit = 0;

        // Reference
        //scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Increment the hit object timer used for judgements
        IncrementHitObjectTimer();

        // Check if it's time to destroy the hit object - miss
        CheckIfReadyToDestroy();

        // Check input for hitting object
        CheckInput();
    }

    // Assign color based on tag
    private void AssignColor()
    {
        switch (tag)
        {
            case Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY1;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY2;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY3_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY1;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY4_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY2;
                break;
            case Constants.MOUSE_HIT_OBJECT_TYPE_LEFT_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_MOUSE_COLOR_LEFT;
                break;
            case Constants.MOUSE_HIT_OBJECT_TYPE_RIGHT_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_MOUSE_COLOR_RIGHT;
                break;
            case Constants.MOUSE_HIT_OBJECT_TYPE_UP_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_MOUSE_COLOR_UP;
                break;
            case Constants.MOUSE_HIT_OBJECT_TYPE_DOWN_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_MOUSE_COLOR_DOWN;
                break;
        }
    }

    // Checks input for hitting the hit object
    protected virtual void CheckInput()
    {
        // Input for checking judgements
    }

    // Check keypresses for judgements
    protected virtual void CheckJudgements()
    {
        // If the hit object has been hit before the destroyed time has been reached
        if (hitObjectTimer < MISS_TIME)
        {
            // Check if the player hit early judgement
            CheckEarlyJudgement();
            // Check if the player hit good judgement
            CheckGoodJudgement();
            // Check if the player hit perfect judgement
            CheckPerfectJudgement();
            // Increment the current combo
            scriptManager.playInformation.AddCombo();
            // Get the time when the user pressed the key to hit the hit object
            timeWhenHit = hitObjectTimer;
            // Play hit sound
            scriptManager.hitSoundManager.PlayHitSound();
            // Fill fever slider
            scriptManager.feverTimeManager.FillFeverSlider();
            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }

    // Check if the player hit early judgement
    protected virtual void CheckEarlyJudgement()
    {
        if (hitObjectTimer >= HIT_OBJECT_START_TIME && hitObjectTimer <= EARLY_JUDGEMENT_TIME)
        {
            scriptManager.playInformation.AddEarlyJudgement();
            scriptManager.playInformation.AddScore(Constants.EARLY_SCORE_VALUE);
            scriptManager.explosionManager.SpawnExplosion(tag, Constants.HIT_TAG, this.transform.position,
                Constants.EARLY_SCORE_VALUE, colorImage.color);
        }
    }

    // Check if the player hit good judgement
    protected virtual void CheckGoodJudgement()
    {
        if (hitObjectTimer >= EARLY_JUDGEMENT_TIME && hitObjectTimer <= PERFECT_JUDGEMENT_TIME)
        {
            scriptManager.playInformation.AddGoodJudgement();
            scriptManager.playInformation.AddScore(Constants.GOOD_SCORE_VALUE);
            scriptManager.explosionManager.SpawnExplosion(tag, Constants.HIT_TAG, this.transform.position,
                Constants.GOOD_SCORE_VALUE, colorImage.color);
        }
    }

    // Check if the player hit perfect judgement
    protected virtual void CheckPerfectJudgement()
    {
        if (hitObjectTimer >= PERFECT_JUDGEMENT_TIME && hitObjectTimer <= MISS_TIME)
        {
            scriptManager.playInformation.AddPerfectJudgement();
            scriptManager.playInformation.AddScore(Constants.PERFECT_SCORE_VALUE);
            scriptManager.explosionManager.SpawnExplosion(tag, Constants.HIT_TAG, this.transform.position,
               Constants.PERFECT_SCORE_VALUE, colorImage.color);
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
        if (hitObjectTimer >= MISS_TIME)
        {
            MissedObject();
        }
    }

    // Do the miss object functions
    private void MissedObject()
    {
        // Spawn explosion
        scriptManager.explosionManager.SpawnExplosion(tag, Constants.MISS_TAG, this.transform.position, Constants.MISS_SCORE_VALUE,
            colorImage.color);
        // Sets judgement to miss
        scriptManager.playInformation.AddMissJudgement();
        // Reset combo as missed
        scriptManager.playInformation.ResetCombo();
        // Play miss sound
        scriptManager.hitSoundManager.PlayMissSound();
        // Deactivate gameobject
        this.gameObject.SetActive(false);
        // Decrease fever slider
        scriptManager.feverTimeManager.DecreaseFeverSlider();
    }
    #endregion
}
