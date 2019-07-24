using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverTimeManager : MonoBehaviour {

    public Slider feverTimeBar; // Fever time bar
    private int feverTimeBarMaxValue; // Max value for the fever time bar
    private int feverTimeBarMinValue; // Min value for the fever time bar
    public AudioSource menuSFXAudioSource; // Menu SFX audio source
    public AudioClip feverTimeReadySound; // Soundclip when fever bar is full
    public AudioClip feverTimeActivatedSound; // Soundclip when fever time has been activated
    public AudioClip feverTimeDeactivatedSound; // Soundlcip when fever time has been deactivated
    public Animator feverTimeBarAnimator; // Animator for controlling fever time bar animations
    public GameObject feverTimeBackground; // FeverTime background 
    public Animator feverTimeBackgroundAnimator; // Fevertime background animator

    private float timeStartedLerping; // Time that the lerp started
    private float lerpTimeUp; // Time to lerp from min to max value
    private float lerpTimeDown; // Time to lerp from max to min value
    private bool shouldLerpUp; // Controls lerping up
    private bool shouldLerpDown; // Controls lerping down

    public GameObject buttonGlow; // Fever time button glow
    private bool hasPlayedFeverTimeReadySound; // Prevents sound from playing multiple times
    private bool hasPlayedFeverTimeActivatedSound; // Prevents sound from playing multiple times
    private bool hasPlayedFeverTimeDeactivatedSound; // PRevents sound from playing multiple times

    public ParticleSystem feverTimeBarParticles; // Fever time bar particlesystem

    private bool feverTimeReady; // Controls whether fever time can be activated
    private bool feverTimeActivated, feverTimeActivatedPrevious; // Has fever time been activated, previous for checking if fever time has been deactivated or activated

    public AudioReverbFilter songAudioReverbFilter; // Adds reverb to the song audio source
    public AudioReverbFilter menuSFXAudioReverbFiler; // Adds reverb to the hit sound audio source

    private ScoreManager scoreManager; // Controls scoring/combo

    private int feverTimeActivateCombo; // The combo required to activate fever time
    private KeyCode feverTimeActivateKey; // Key to activate fever time

    public Image feverTimeBorderImage; // Border image that appears during fever time

    private bool gameplayStarted; // Has gameplay started 

    public GameObject feverPanel; // Fever panel

    public MetronomeForEffects metronomeForEffects; // Metronome for effects for beat sync functions

    private float feverTimeBackgroundFadeTimer;
    private bool hasPlayedFeverTimeFadeAnimation;

    // Properties

    // Check if fever time is activated
    public bool FeverTimeActivated
    {
        get { return feverTimeActivated; }
    }

    // Check if the fever time was activated on the previous frame
    public bool FeverTimeActivatedPrevious
    {
        get { return feverTimeActivatedPrevious; }
    }


    // >> fever time activated true
    // >> previous = fevertimeactivated true

    // >> fever time deactivated false
    // Check if fevertime activated == fevertimeprevious
    // If not the same value = fever time has been deactivated
    // Change the material
    // >> fever time 

    // 


    // Use this for initialization
    void Start () {

        // Get the reference to the score manager
        scoreManager = FindObjectOfType<ScoreManager>();
        // Get the reference to the metronomeForEffects 
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();

        // Initialize 
        timeStartedLerping = 0f;
        feverTimeBarMaxValue = 1;
        feverTimeBarMinValue = 0;
        lerpTimeUp = 0.5f;
        lerpTimeDown = 5f;
        feverTimeBar.value = 0;
        shouldLerpUp = false;
        shouldLerpDown = false;
        feverTimeActivated = false;
        hasPlayedFeverTimeActivatedSound = false;
        hasPlayedFeverTimeReadySound = false;
        hasPlayedFeverTimeDeactivatedSound = true; // Set to true at the start to prevent the deactivated sound playing instantly
        feverTimeBackground.gameObject.SetActive(true); // Set to true at the start 
        feverTimeActivateCombo = 25; // Next fever time combo
        feverTimeActivateKey = KeyCode.Space; // Set the key to activate fever time to the spacebar
        gameplayStarted = false; // Set gameplay to started at the start
        feverPanel.gameObject.SetActive(false); // Deactivate at the start

        // Insert function for calculating the lerpTimeDown = how long a measure/bar would last in-game
	}

    // Update is called once per frame
    void Update() {

        // If gameplay has not started yet
        if (gameplayStarted == false)
        {
            // Check if the game has started yet from the spacebar being pressed
            CheckGameplayStarted();
        }




        // Check combo
        CheckCombo();

        // Check if user has pressed the fever time key
        CheckFeverTimeKeyInput();

        // Activate/Deactivate fever time border image
        ToggleBorderImage();

        // Lerp the fever time bar up if fever time is not ready
        if (shouldLerpUp == true && feverTimeReady == false)
        {
            // Increment the time since spawned
            timeStartedLerping += Time.deltaTime;

            // Lerp fever time bar value
            feverTimeBar.value = LerpFeverBarUp();
        }

        // Lerp the fever time bar down if it has been activated
        if (shouldLerpDown == true && feverTimeActivated == true)
        {
            // Increment the time since spawned
            timeStartedLerping += Time.deltaTime;

            // Play the fever time background animation
            PlayFeverBackgroundAnimation();

            // Lerp the fever time slider bar value down
            feverTimeBar.value = LerpFeverBarDown();
        }

        // Only run if the gameplay has started
        if (gameplayStarted == true)
        {
            // Check the current value of the fever bar
            CheckFeverBarValue();
        }


        // Update the feverTime activated and previous values
        UpdateFeverTimeValues();
    }

    // Update the feverTime activated and previous values
    private void UpdateFeverTimeValues()
    {
        feverTimeActivatedPrevious = feverTimeActivated;
    }

    // Calculate fever time duration
    private void CalculateFeverTimeDuration()
    {
        // LerpTimeDown = FeverTimeDuration
        // Get the fever time duration
        // Fever time duration = duration of 1 measure for the songs
        lerpTimeDown = metronomeForEffects.GetMeasureDuration();
    }


    // Check if the game has started yet from the spacebar being pressed
    private void CheckGameplayStarted()
    {
        // Check if the spacebar has been pressed (starts gameplay)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Disable
            //feverTimeBackground.gameObject.SetActive(false);

            StartCoroutine(PlayFeverTimeBackgroundFade());

        }
    }

    // Toggle fever time border image
    private void ToggleBorderImage()
    {
        // If fever time is active
        if (feverTimeActivated == true)
        {
            // Activate border image
            feverTimeBorderImage.gameObject.SetActive(true);
        }
        else if (feverTimeActivated == false)
        {
            // Deactivate border image
            feverTimeBorderImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayFeverTimeBackgroundFade()
    {
        feverTimeBackgroundAnimator.Play("FevertimeBackgroundFade");

        yield return new WaitForSeconds(1f);


        // Set to true as gameplay has now started
        gameplayStarted = true;
    }

    // Play the fever time background animation
    private void PlayFeverBackgroundAnimation()
    {
        // Ensure gameobject is active
        feverTimeBackground.gameObject.SetActive(true);
        // Activate fever time background animation
        feverTimeBackgroundAnimator.Play("FevertimeBackground");
    }

    // Check if user has pressed the fever time key
    private void CheckFeverTimeKeyInput()
    {
        // Activate fever time with key press if available
        if (Input.GetKeyDown(feverTimeActivateKey))
        {
            // Check if fever time is ready and the bar is full
            if (feverTimeReady == true)
            {
                // Activate fever time
                ActivateFeverTime();
            }
        }
    }

    // Check if the current combo is at the combo required to activate fever time
    private void CheckCombo()
    {
        // Check if the current combo is at the combo required to activate fever time
        if (scoreManager.Combo == feverTimeActivateCombo)
        {
            // Play the lerp bar animation to ready up fever time
            // Set to true to lerp the bar value up
            shouldLerpUp = true;

            // Increment the feverTimeActivateCombo so it can be activated in another 25 combo
            feverTimeActivateCombo += 25;
        }

        // If combo is at 0/reset
        if (scoreManager.Combo == 0)
        {
            // Reset feverTimeActivateCombo back to 25
            feverTimeActivateCombo = 25;
        }
    }

    // Lerp the fever bar value to gradually fill it up
    public float LerpFeverBarUp()
    {
        float timeSinceStarted = timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTimeUp;

        var result = Mathf.Lerp(feverTimeBarMinValue, feverTimeBarMaxValue, percentageComplete);

        return result;
    }


    // Lerp the fever bar value to gradually go down
    public float LerpFeverBarDown()
    {
        float timeSinceStarted = timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTimeDown;

        var result = Mathf.Lerp(feverTimeBarMaxValue, feverTimeBarMinValue, percentageComplete);

        return result;
    }

    // Check the fever bar current value
    private void CheckFeverBarValue()
    {
        // Check if the feverTimeBar value is at max value 1
        if (feverTimeBar.value == feverTimeBarMaxValue)
        {
            // Fever time is ready and can be activated
            feverTimeReady = true;

            // Allow the deactivate sound to play
            hasPlayedFeverTimeDeactivatedSound = false;

            // Reset lerp variables
            ResetLerpVariables();

            // Reset fever time activated bool to allow the sound to play
            hasPlayedFeverTimeActivatedSound = false;

            // Activate fever time particles
            feverTimeBarParticles.gameObject.SetActive(true);
            // Activate button glow
            buttonGlow.gameObject.SetActive(true);

            // Check if the fever time ready sound has been played previosuly
            if (hasPlayedFeverTimeReadySound == false)
            {
                // Play fever time ready sound effect
                menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

                // Play fever time hit animation
                feverTimeBarAnimator.Play("FeverTimeButton_HIT");

                // Set to true
                hasPlayedFeverTimeReadySound = true;
            }
        }

        // Check if the feverTimeBar value is at 0
        if (feverTimeBar.value == feverTimeBarMinValue)
        {
            // Reset lerp variables
            ResetLerpVariables();

            // Deactivate fever time background
            feverTimeBackground.gameObject.SetActive(false);

            // Disable fever time
            feverTimeActivated = false;

            // Reset fever time ready bool to allow sound to play again when at max value
            hasPlayedFeverTimeReadySound = false;

            // Play the default fever time button animation
            feverTimeBarAnimator.Play("FeverTimeButton_DEFAULT");

            // Deactivate feverPanel
            feverPanel.gameObject.SetActive(false);

            // Check if the deactivate sound has already played
            if (hasPlayedFeverTimeDeactivatedSound == false)
            {
                // Play deactivate sound 
                menuSFXAudioSource.PlayOneShot(feverTimeDeactivatedSound);
                // Set to false to prevent sound from playing multiple times
                hasPlayedFeverTimeDeactivatedSound = true;

                // Turn off audio reverb
                songAudioReverbFilter.enabled = false;
                // Turn off menuSFX reverb
                menuSFXAudioReverbFiler.enabled = false;
            }
        }

        // Check if the fever time bar value is less than the max value
        if (feverTimeBar.value < feverTimeBarMaxValue)
        {
            // Reset fever time is ready
            feverTimeReady = false;
        }
    }

    // Reset lerp
    private void ResetLerpVariables()
    {
        // Reset lerp variables
        timeStartedLerping = 0;
        shouldLerpUp = false;
        shouldLerpDown = false;
    }

    // Activate fever time
    private void ActivateFeverTime()
    {
        // Play the fever time button activated animation
        feverTimeBarAnimator.Play("FeverTimeButton_ACTIVATED");

        // Check if sound has already been played previously
        if (hasPlayedFeverTimeActivatedSound == false)
        {
            // Play activate sound effect
            menuSFXAudioSource.PlayOneShot(feverTimeActivatedSound);
            // Set to true
            hasPlayedFeverTimeActivatedSound = true;

            // Enable sound reverb to the song
            songAudioReverbFilter.enabled = true;
            // Enable menuSFX reverb
            menuSFXAudioReverbFiler.enabled = true;
        }

        // Activate fever time background
        feverTimeBackground.gameObject.SetActive(true);

        // Activate feverPanel gameobject
        feverPanel.gameObject.SetActive(true);

        // Set shouldLerpDown to true to allow the fever bar value to go down
        shouldLerpDown = true;

        // Set to true as fever time has been activated
        feverTimeActivated = true;
    }
}
