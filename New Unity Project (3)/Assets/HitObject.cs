using UnityEngine;
using UnityEngine.UI;

public class HitObject : MonoBehaviour
{
    #region Variables
    // UI
    public Image colorImage;

    // Integers
    private float timeWhenHit, hitObjectTimer;

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

        // Play approach animation
        PlayApproachAnimation();
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
    }

    // Update is called once per frame
    private void Update()
    {
        // Increment the hit object timer used for judgements
        IncrementHitObjectTimer();

        // Check if it's time to destroy the hit object - miss
        CheckIfReadyToDestroy();

        // If hit object has failed or not
        switch (scriptManager.healthbar.Failed)
        {
            case false:
                // Check input for hitting object
                CheckInput();
                break;
        }
    }

    // Play approach animation
    protected virtual void PlayApproachAnimation()
    {

    }

    public virtual void AssignFeverColors()
    {

    }

    public virtual void AssignNormalColors()
    {

    }

    // Assign color based on tag
    protected virtual void AssignColor()
    {

    }

    // Checks input for hitting the hit object
    protected virtual void CheckInput()
    {
        // Input for checking judgements
    }

    // Check keypresses for judgements
    protected virtual void CheckJudgements()
    {
        if (scriptManager.healthbar.Failed == false)
        {
            // If the hit object has been hit before the destroyed time has been reached
            if (hitObjectTimer < Constants.MISS_TIME)
            {
                // Hit object has been hit
                HasHit();
            }
        }
    }

    // Check incorrect inputs
    protected virtual void CheckIncorrectInput()
    {
        // Input for checking incorrect input
    }

    // Hit object has been hit
    protected virtual void HasHit()
    {
        // Check judgements
        CheckEarlyJudgement();
        CheckGreatJudgement();
        CheckMaxJudgement();
        CheckMaxPlusJudgement();
        CheckLateJudgement();

        if (hitObjectHit == true)
        {
            // Increment the current combo
            scriptManager.playInformation.AddCombo();
            // Get the time when the user pressed the key to hit the hit object
            timeWhenHit = hitObjectTimer;
            // Play hit sound
            scriptManager.hitSoundManager.PlayHitSound();
            // Remove this object from the active list
            scriptManager.loadAndRunBeatmap.RemoveObjectFromActiveList(this);
            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }

    // Check early judgement timing
    private void CheckEarlyJudgement()
    {
        if (hitObjectTimer >= Constants.JUDGEMENT_START_TIME_EARLY && hitObjectTimer < Constants.JUDGEMENT_START_TIME_GREAT)
        {
            scriptManager.playInformation.AddEarlyJudgement();
            scriptManager.playInformation.AddScore(Constants.EARLY_SCORE_VALUE);
            scriptManager.explosionManager.SpawnHitExplosion(this.transform.position,
                Constants.EARLY_SCORE_VALUE, colorImage.color);
            IncreaseFeverScore(Constants.EARLY_SCORE_VALUE);
            scriptManager.playInformation.DisplayFollowInfo(this.transform.position, Constants.EARLY_JUDGEMENT);
            hitObjectHit = true;
        }
    }

    // Check great judgement timing
    private void CheckGreatJudgement()
    {
        if (hitObjectTimer >= Constants.JUDGEMENT_START_TIME_GREAT && hitObjectTimer < Constants.JUDGEMENT_START_TIME_MAX)
        {
            scriptManager.playInformation.AddGreatJudgement();
            scriptManager.playInformation.AddScore(Constants.GREAT_SCORE_VALUE);
            scriptManager.explosionManager.SpawnHitExplosion(this.transform.position,
                Constants.GREAT_SCORE_VALUE, colorImage.color);
            IncreaseFeverScore(Constants.GREAT_SCORE_VALUE);
            scriptManager.playInformation.DisplayFollowInfo(this.transform.position, Constants.GREAT_JUDGEMENT);
            hitObjectHit = true;
            scriptManager.healthbar.UpdateHealth(Constants.GREAT_HEALTH_VALUE);
        }
    }

    // Check max judgement timing
    private void CheckMaxJudgement()
    {
        if (hitObjectTimer >= Constants.JUDGEMENT_START_TIME_MAX && hitObjectTimer < Constants.JUDGEMENT_START_TIME_MAXPLUS)
        {
            MaxJudgementFunctions();
        }
        else if (hitObjectTimer >= Constants.JUDGEMENT_END_TIME_MAXPLUS && hitObjectTimer < Constants.JUDGEMENT_START_TIME_LATE)
        {
            MaxJudgementFunctions();
        }
    }

    // Check max plus judgement timing
    private void CheckMaxPlusJudgement()
    {
        if (hitObjectTimer >= Constants.JUDGEMENT_START_TIME_MAXPLUS && hitObjectTimer < Constants.JUDGEMENT_END_TIME_MAXPLUS)
        {
            scriptManager.playInformation.AddMaxPlusJudgement();
            scriptManager.playInformation.AddScore(Constants.MAXPLUS_SCORE_VALUE);
            scriptManager.explosionManager.SpawnHitExplosion(this.transform.position,
                Constants.MAXPLUS_SCORE_VALUE, colorImage.color);
            IncreaseFeverScore(Constants.MAXPLUS_SCORE_VALUE);
            scriptManager.playInformation.DisplayFollowInfo(this.transform.position, Constants.MAXPLUS_JUDGEMENT);
            hitObjectHit = true;
            scriptManager.healthbar.UpdateHealth(Constants.MAXPLUS_HEALTH_VALUE);
        }
    }

    // Check late judgement timing
    private void CheckLateJudgement()
    {
        if (hitObjectTimer >= Constants.JUDGEMENT_START_TIME_LATE && hitObjectTimer < Constants.JUDGEMENT_END_TIME_LATE)
        {
            scriptManager.playInformation.AddLateJudgement();
            scriptManager.playInformation.AddScore(Constants.LATE_SCORE_VALUE);
            scriptManager.explosionManager.SpawnHitExplosion(this.transform.position,
                Constants.LATE_SCORE_VALUE, colorImage.color);
            IncreaseFeverScore(Constants.LATE_SCORE_VALUE);
            scriptManager.playInformation.DisplayFollowInfo(this.transform.position, Constants.LATE_JUDGEMENT);
            hitObjectHit = true;
            scriptManager.healthbar.UpdateHealth(Constants.LATE_HEALTH_VALUE);
        }
    }

    // Max judgement functions
    private void MaxJudgementFunctions()
    {
        scriptManager.playInformation.AddMaxJudgement();
        scriptManager.playInformation.AddScore(Constants.MAX_SCORE_VALUE);
        scriptManager.explosionManager.SpawnHitExplosion(this.transform.position,
            Constants.MAX_SCORE_VALUE, colorImage.color);
        IncreaseFeverScore(Constants.MAX_SCORE_VALUE);
        scriptManager.playInformation.DisplayFollowInfo(this.transform.position, Constants.MAX_JUDGEMENT);
        hitObjectHit = true;
        scriptManager.healthbar.UpdateHealth(Constants.MAX_HEALTH_VALUE);
    }

    // Increase fever score
    private void IncreaseFeverScore(int _score)
    {
        switch (scriptManager.feverTimeManager.FeverActivated)
        {
            case true:
                scriptManager.playInformation.IncreaseTotalFeverBonusScore(_score);
                break;
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
        if (hitObjectTimer >= Constants.MISS_TIME)
        {
            MissedObject();
        }
    }

    // Do the miss object functions
    protected virtual void MissedObject()
    {
        // Spawn explosion
        scriptManager.explosionManager.SpawnMissExplosion(tag, this.transform.position, 
            Constants.MISS_SCORE_VALUE, colorImage.color);
        // Sets judgement to miss
        scriptManager.playInformation.AddMissJudgement();
        // Reset combo as missed
        scriptManager.playInformation.ResetCombo();
        // Play miss sound
        scriptManager.hitSoundManager.PlayMissSound();
        // Update health
        scriptManager.healthbar.UpdateHealth(Constants.MISS_HEALTH_VALUE);
        // Remove this object from the active list
        scriptManager.loadAndRunBeatmap.RemoveObjectFromActiveList(this);
        // Deactivate gameobject
        this.gameObject.SetActive(false);
    }
    #endregion
}
