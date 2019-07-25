using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FeverTimeManager : MonoBehaviour {

    // UI
    public Slider feverTimeBar; // First time fever time bar
    public Slider feverTimeBarExtend2; // Second extention bar for fever time
    public Slider feverTimeBarExtend3; // Third extention bar for fever time
    public Slider feverTimeBarExtend4; // Fourth extention bar for fever time

    public Image feverTimeBorderImage; // Border image that appears during fever time

    // Animation
    public Animator feverTimeUIAnimator; // Animator for all fever time bar animations
    public Animator feverTimeBackgroundAnimator; // Fevertime background animator

    // Gameobjects
    public GameObject feverTimeBackground; // FeverTime background 
    public GameObject buttonGlow; // Fever time button glow
    public GameObject feverPanel; // Fever panel

    // Integers
    private int feverTimeBarMaxValue; // Max value for the fever time bar
    private int feverTimeBarMinValue; // Min value for the fever time bar
    private int feverTimeActivateCombo; // The combo required to activate fever time
    private int currentFeverTimeBarCount; // How many fever time bars have been currently earned 1-4
    private float timeStartedLerping; // Time that the lerp started
    private float lerpTimeUp; // Time to lerp from min to max value
    private float lerpTimeDown; // Time to lerp from max to min value
    private float feverTimeBackgroundFadeTimer;

    // Bools
    private bool shouldLerpUp; // Controls lerping up
    private bool shouldLerpDown; // Controls lerping down
    private bool hasPlayedFeverTimeReadySound; // Prevents sound from playing multiple times
    private bool hasPlayedFeverTimeReadySoundExtend2; // Second fever time bar hitting max value
    private bool hasPlayedFeverTimeReadySoundExtend3; // Third fever time bar hitting max value
    private bool hasPlayedFeverTimeReadySoundExtend4; // Fourth fever time bar hitting max value



    private bool hasPlayedFeverTimeActivatedSound; // Prevents sound from playing multiple times
    private bool hasPlayedFeverTimeDeactivatedSound; // PRevents sound from playing multiple times
    private bool feverTimeReady; // Controls whether fever time can be activated
    private bool feverTimeActivated, feverTimeActivatedPrevious; // Has fever time been activated, previous for checking if fever time has been deactivated or activated
    private bool gameplayStarted; // Has gameplay started 
    private bool hasPlayedFeverTimeFadeAnimation;

    // Audio
    public AudioSource menuSFXAudioSource; // Menu SFX audio source
    public AudioClip feverTimeReadySound; // Soundclip when fever bar is full
    public AudioClip feverTimeActivatedSound; // Soundclip when fever time has been activated
    public AudioClip feverTimeDeactivatedSound; // Soundlcip when fever time has been deactivated
    public AudioReverbFilter songAudioReverbFilter; // Adds reverb to the song audio source
    public AudioReverbFilter menuSFXAudioReverbFiler; // Adds reverb to the hit sound audio source

    // Particles
    public ParticleSystem feverTimeBarParticles; // Fever time bar particlesystem

    // Keycodes
    private KeyCode feverTimeActivateKey; // Key to activate fever time

    // Scripts
    private ScoreManager scoreManager; // Controls scoring/combo
    private MetronomeForEffects metronomeForEffects; // Metronome for effects for beat sync functions


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
        currentFeverTimeBarCount = 0;
        lerpTimeUp = 0.5f;
        lerpTimeDown = 5f;
        feverTimeBar.value = 0;
        shouldLerpUp = false;
        shouldLerpDown = false;
        feverTimeActivated = false;
        hasPlayedFeverTimeActivatedSound = false;
        hasPlayedFeverTimeReadySoundExtend2 = false;
        hasPlayedFeverTimeReadySoundExtend3 = false;
        hasPlayedFeverTimeReadySoundExtend4 = false;
        hasPlayedFeverTimeReadySound = false;
        hasPlayedFeverTimeDeactivatedSound = true; // Set to true at the start to prevent the deactivated sound playing instantly
        feverTimeBackground.gameObject.SetActive(true); // Set to true at the start 
        feverTimeActivateCombo = 25; // Next fever time combo
        feverTimeActivateKey = KeyCode.Space; // Set the key to activate fever time to the spacebar
        gameplayStarted = false; // Set gameplay to started at the start
        feverPanel.gameObject.SetActive(false); // Deactivate at the start

        feverTimeBar.value = 0;
        feverTimeBarExtend2.value = 0;
        feverTimeBarExtend3.value = 0;
        feverTimeBarExtend4.value = 0;

        // Calculate the duration of fever time for 1 measure
        CalculateFeverTimeDuration();
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

            // Lerp up the correct fever time bar based on how many bars have been earned during gameplay
            switch (currentFeverTimeBarCount)
            {
                case 1:
                    // Lerp the first fever time bar value up
                    feverTimeBar.value = LerpFeverBarUp();
                    break;
                case 2:
                    // Lerp the second fever time bar value up
                    feverTimeBarExtend2.value = LerpFeverBarUp();
                    break;
                case 3:
                    // Lerp the third fever time bar value up
                    feverTimeBarExtend3.value = LerpFeverBarUp();
                    break;
                case 4:
                    // Lerp the fourth fever time bar value up
                    feverTimeBarExtend4.value = LerpFeverBarUp();
                    break;
            }
        }

        // Lerp the fever time bar down if it has been activated
        if (shouldLerpDown == true && feverTimeActivated == true)
        {
            // Increment the time since spawned
            timeStartedLerping += Time.deltaTime;

            // Check the current fever bar values and whether they have reached 0 / do not need updating
            CheckCurrentActivatedBarValues();

            // Play the fever time background animation
            PlayFeverBackgroundAnimation();

            // Lerp up the fever time bar based on how many bars have been earned during gameplay
            switch (currentFeverTimeBarCount)
            {
                case 1:
                    // Lerp the first fever time bar value down
                    feverTimeBar.value = LerpFeverBarDown();
                    break;
                case 2:
                    // Lerp the second fever time bar value down
                    feverTimeBarExtend2.value = LerpFeverBarDown();
                    break;
                case 3:
                    // Lerp the third fever time bar value down
                    feverTimeBarExtend3.value = LerpFeverBarDown();
                    break;
                case 4:
                    // Lerp the fourth fever time bar value down
                    feverTimeBarExtend4.value = LerpFeverBarDown();
                    break;
            }
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

        yield return new WaitForSeconds(0.95f);


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
            // Increment the current bar count to get which fever bar we're on
            currentFeverTimeBarCount++;

            // Play the lerp bar animation to ready up fever time
            // Set to true to lerp the bar value up
            shouldLerpUp = true;

            // Increment the feverTimeActivateCombo so it can be activated in another 25 combo
            feverTimeActivateCombo += 25;
        }

        // If combo is at 0/reset
        if (scoreManager.Combo == 0)
        {
            // Reset the fever time combo to the previous fever time value if a combo has been broken
            switch (currentFeverTimeBarCount)
            {
                case 1:
                    // Reset feverTimeActivateCombo back to 25 if only the first fever bar has been earned
                    feverTimeActivateCombo = 25;
                    break;
                case 2:
                    // Reset feverTimeActivateCombo back to 25 if the second fever bar has been earned
                    feverTimeActivateCombo = 50;
                    break;
                case 3:
                    // Reset feverTimeActivateCombo back to 25 if the third fever bar has been earned
                    feverTimeActivateCombo = 75;
                    break;
                case 4:
                    // Reset feverTimeActivateCombo back to 25 if fourth fever bar has been earned
                    feverTimeActivateCombo = 100;
                    break;
            }
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
            // Reset fever time activated bool to allow the sound to play
            hasPlayedFeverTimeActivatedSound = false;

            // Check if the fever time ready sound has been played previosuly
            if (hasPlayedFeverTimeReadySound == false)
            {
                // Play fever time ready sound effect
                menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

                // Play the first fever time bar hit animation
                feverTimeUIAnimator.Play("FeverTimeUI_FirstHIT");

                // Set to true
                hasPlayedFeverTimeReadySound = true;
            }
            // Reset lerp variables
            //ResetLerpVariables();
        }






        // TESTING - PUT THIS IN IT'S OWN LOOPING FUNCTION
        if (feverTimeBarExtend2.value == feverTimeBarMaxValue)
        {
            // Check if the fever time ready sound has been played previosuly
            if (hasPlayedFeverTimeReadySoundExtend2 == false)
            {
                // Play fever time ready sound effect
                menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

                // Play the first fever time bar hit animation
                feverTimeUIAnimator.Play("FeverTimeUI_SecondHIT");

                // Set to true
                hasPlayedFeverTimeReadySoundExtend2 = true;
            }
            // Reset lerp variables
            //ResetLerpVariables();
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

            // Play the default fever time animation
            feverTimeUIAnimator.Play("FeverTimeUI_FirstDEFAULT");

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

        // Check all the activate fever time bar values against the max bar value they can hold, if any are below it means they've been activated
        // Activating fever time while the values are below the max amount will be disabled
        switch (currentFeverTimeBarCount)
        {
            case 1:
                if (feverTimeBar.value < feverTimeBarMaxValue)
                {
                    feverTimeReady = false;
                }
                break;
            case 2:
                if (feverTimeBar.value < feverTimeBarMaxValue || feverTimeBarExtend2.value < feverTimeBarMaxValue)
                {
                    feverTimeReady = false;
                }
                break;
            case 3:
                if (feverTimeBar.value < feverTimeBarMaxValue || feverTimeBarExtend2.value < feverTimeBarMaxValue
                    || feverTimeBarExtend3.value < feverTimeBarMaxValue)
                {
                    feverTimeReady = false;
                }
                break;
            case 4:
                if (feverTimeBar.value < feverTimeBarMaxValue || feverTimeBarExtend2.value < feverTimeBarMaxValue 
                    || feverTimeBarExtend3.value < feverTimeBarMaxValue || feverTimeBarExtend4.value < feverTimeBarMaxValue)
                {
                    feverTimeReady = false;
                }
                break;
        }
    }

    // Check all the activated fever time bar values, if a bar has reached the min value (0) update the current activate bar count number
    private void CheckCurrentActivatedBarValues()
    {
        switch (currentFeverTimeBarCount)
        {
            case 2:
                // If the 2nd fever bar value has reached 0
                if (feverTimeBarExtend2.value <= feverTimeBarMinValue)
                {
                    // There is only 1 bar left that needs updating/draining
                    currentFeverTimeBarCount = 1;
                }
                break;
            case 3:
                // If the 3rd fever bar value has reached 0
                if (feverTimeBarExtend3.value <= feverTimeBarMinValue)
                {
                    // There are 2 bars left that needs updating/draining
                    currentFeverTimeBarCount = 2;
                }
                break;
            case 4:
                // If the 4th fever bar value has reached 0
                if (feverTimeBarExtend4.value <= feverTimeBarMinValue)
                {
                    // There are 3 bars left that needs updating/draining
                    currentFeverTimeBarCount = 3;
                }
                break;
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
        // Play the fever time activated animation based on the current number of active bars
        switch (currentFeverTimeBarCount)
        {
            case 1:
                feverTimeUIAnimator.Play("FeverTimeUI_FirstACTIVATED");
                break;
            case 2:
                feverTimeUIAnimator.Play("FeverTimeUI_SecondACTIVATED");
                break;
            case 3:
                feverTimeUIAnimator.Play("FeverTimeUI_ThirdACTIVATED");
                break;
            case 4:
                feverTimeUIAnimator.Play("FeverTimeUI_FourthACTIVATED");
                break;
        }

        // Calculate the duration of fever time based on the amount of bars earned
        switch (currentFeverTimeBarCount)
        {
            case 1:
                // 1 Measure
                lerpTimeDown = metronomeForEffects.GetMeasureDuration();
                break;
            case 2:
                // 2 Measures
                lerpTimeDown = (metronomeForEffects.GetMeasureDuration() * 2);
                break;
            case 3:
                // 3 Measures
                lerpTimeDown = (metronomeForEffects.GetMeasureDuration() * 3);
                break;
            case 4:
                // 4 Measures
                lerpTimeDown = (metronomeForEffects.GetMeasureDuration() * 4);
                break;
        }


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
