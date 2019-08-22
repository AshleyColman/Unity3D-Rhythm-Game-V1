using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeverTimeManager : MonoBehaviour {

    // UI
    public Slider feverTimeBar; // Fever time bar that fills up
    public Slider whiteFeverTimebar; // White background bar
    public Image feverTimeBarFillImage;
    public Image feverTimeBorderImage; // Border image that appears during fever time
    public TextMeshProUGUI multiplierText, shadowMultiplierText; // Multiplier text 


    // Gameobjects
    public GameObject feverTimeBackground; // FeverTime background 

    // Color
    public Color defaultBarColor, activatedBarColor;

    // Animation
    public Animator feverTimeBackgroundAnimator;
    public Animator bar25PercentAnimator, bar50PercentAnimator, bar75PercentAnimator, bar100PercentAnimator;
    public Animator multiplierTextAnimator;
    public Animator feverTimeSideParticlesAnimator;

    // Integers
    private int feverTimeBarMaxValue; // Max value for the fever time bar
    private int feverTimeBarMinValue; // Min value for the fever time bar
    private int feverTimeActivateCombo; // The combo required to activate fever time
    public int fullFeverTimeBarCount; // How many fever time bars are full
    public int feverTimeCombo; // Fever time combo for controlling fever time bars
    private float timeStartedLerping; // Time that the lerp started
    public float lerpTimeDown; // Time to lerp from max to min value
    private float feverTimeBackgroundFadeTimer;
    public float measureDuration; // Total time for 1 measure
    private float lerpValue, lerpValue25Percent, lerpValue50Percent, lerpValue75Percent, lerpValue100Percent; // Values to lerp from
    

    // Bools
    public bool shouldLerpDown; // Controls lerping down
    public bool hasPlayedFeverTimeReadySound; // Prevents sound from playing multiple times
    public bool hasPlayedFeverTimeReadySound2; // Second fever time bar hitting max value
    public bool hasPlayedFeverTimeReadySound3; // Third fever time bar hitting max value
    public bool hasPlayedFeverTimeReadySound4; // Fourth fever time bar hitting max value
    private bool hasPlayedFeverTimeActivatedSound; // Prevents sound from playing multiple times
    private bool hasPlayedFeverTimeDeactivatedSound; // PRevents sound from playing multiple times
    public bool feverTimeReady; // Controls whether fever time can be activated
    public bool feverTimeActivated, feverTimeActivatedPrevious; // Has fever time been activated, previous for checking if fever time has been deactivated or activated
    private bool gameplayStarted; // Has gameplay started 
    private bool hasPlayedFeverTimeFadeAnimation;
    private bool hasSet0Percent, hasSet25Percent, hasSet50Percent, hasSet75Percent, hasSet100Percent; // Controls setting the bar to x percent once per activation

    private string defaultMultiplier, feverTimeMultiplier;

    // Audio
    public AudioSource menuSFXAudioSource; // Menu SFX audio source
    public AudioClip feverTimeReadySound; // Soundclip when fever bar is full
    public AudioClip feverTimeActivatedSound; // Soundclip when fever time has been activated
    public AudioClip feverTimeDeactivatedSound; // Soundlcip when fever time has been deactivated
    public AudioReverbFilter songAudioReverbFilter; // Adds reverb to the song audio source
    public AudioReverbFilter menuSFXAudioReverbFiler; // Adds reverb to the hit sound audio source

    // Keycodes
    private KeyCode feverTimeActivateKey; // Key to activate fever time

    // Scripts
    private ScoreManager scoreManager; // Controls scoring/combo
    private MetronomeForEffects metronomeForEffects; // Metronome for effects for beat sync functions
    private LoadAndRunBeatmap loadAndRunBeatmap; 

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

    public bool HasSet25Percent
    {
        get { return hasSet25Percent; }
    }

    public bool HasSet50Percent
    {
        get { return hasSet50Percent; }
    }

    public bool HasSet75Percent
    {
        get { return hasSet75Percent; }
    }

    public bool HasSet100Percent
    {
        get { return hasSet100Percent; }
    }


    // Use this for initialization
    void Start () {

        // Get the reference to the score manager
        scoreManager = FindObjectOfType<ScoreManager>();
        // Get the reference to the metronomeForEffects 
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();
        loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>();

        // Initialize 
        timeStartedLerping = 0f;
        feverTimeBarMaxValue = 1;
        feverTimeBarMinValue = 0;
        fullFeverTimeBarCount = 0;
        lerpTimeDown = 5f;
        lerpValue25Percent = 0.25f;
        lerpValue50Percent = 0.50f;
        lerpValue75Percent = 0.75f;
        lerpValue100Percent = 1.0f;
        lerpValue = 0f;
        defaultMultiplier = "1x";
        feverTimeMultiplier = "2x";

        multiplierText.text = defaultMultiplier;

        feverTimeSideParticlesAnimator.gameObject.SetActive(false);

        feverTimeCombo = 0;
        shouldLerpDown = false;
        feverTimeActivated = false;
        hasPlayedFeverTimeActivatedSound = false;
        hasPlayedFeverTimeReadySound = false;
        hasPlayedFeverTimeReadySound2 = false;
        hasPlayedFeverTimeReadySound3 = false;
        hasPlayedFeverTimeReadySound4 = false;
        hasPlayedFeverTimeDeactivatedSound = true; // Set to true at the start to prevent the deactivated sound playing instantly
        feverTimeBackground.gameObject.SetActive(true); // Set to true at the start 
        feverTimeActivateCombo = 5; // Next fever time combo
        feverTimeActivateKey = KeyCode.Space; // Set the key to activate fever time to the spacebar
        gameplayStarted = false; // Set gameplay to started at the start
        hasSet0Percent = false;
        hasSet25Percent = false;
        hasSet50Percent = false;
        hasSet75Percent = false;
        hasSet100Percent = false;
        feverTimeBar.value = 0;
        whiteFeverTimebar.value = 0;
    }

    // Update is called once per frame
    void Update() {

        // If gameplay has not started yet
        if (gameplayStarted == false)
        {
            if (loadAndRunBeatmap.LevelChangerAnimationTimer >= 2f)
            {
                // Check if the game has started yet from the spacebar being pressed
                CheckGameplayStarted();
            }
        }

        // Only run if the gameplay has started
        if (gameplayStarted == true)
        {
            // Check combo
            CheckCombo();

            // Check if user has pressed the fever time key
            CheckFeverTimeKeyInput();

            // Activate/Deactivate fever time border image
            ToggleBorderImage();

            // Check which bar to lerp down based on the amount of bars earned
            CheckBarToLerpDown();

            CheckFirstBarMinValue();

            // Update the feverTime activated and previous values
            UpdateFeverTimeValues();
        }
    }

    // Lerp down the fever bar
    private void CheckBarToLerpDown()
    {
        // Lerp the fever time bar down if it has been activated
        if (shouldLerpDown == true && feverTimeActivated == true)
        {
            // Increment the time since spawned
            timeStartedLerping += Time.deltaTime;

            // Play the fever time background animation
            PlayFeverBackgroundAnimation();

            // Lerp the fever time bar value down
            feverTimeBar.value = LerpFeverBarDown();
            whiteFeverTimebar.value = LerpFeverBarDown() + 0.1f;
        }
    }

    // Update the feverTime activated and previous values
    private void UpdateFeverTimeValues()
    {
        feverTimeActivatedPrevious = feverTimeActivated;
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
        // Combo is going to have to be reset after being activated
        // Combo will need to be incremented manually from this script only
        
        // Only check for combo if fever time is not activated
        if (feverTimeActivated == false)
        {
            if (feverTimeCombo == 25 && hasSet25Percent == false)
            {
                SetBarTo25Percent();
            }
            else if (feverTimeCombo == 50 && hasSet50Percent == false)
            {
                SetBarTo50Percent();
            }
            else if (feverTimeCombo == 75 && hasSet75Percent == false)
            {
                SetBarTo75Percent();
            }
            else if (feverTimeCombo == 100 && hasSet100Percent == false)
            {
                SetBarTo100Percent();
            }
        }
    }

    // Bar has reached 25 percent
    private void SetBarTo25Percent()
    {
        feverTimeBar.value = 0.25f;
        whiteFeverTimebar.value = 0.26f;
        lerpValue = lerpValue25Percent;

        fullFeverTimeBarCount = 1;

        CalculateFeverTimeDuration();

        bar25PercentAnimator.Play("FeverTimeBarPercent", 0, 0f);

        menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

        feverTimeReady = true;

        hasSet25Percent = true;

        // Reset
        hasSet0Percent = false;
    }

    // Bar has reached 50 percent
    private void SetBarTo50Percent()
    {
        feverTimeBar.value = 0.50f;
        whiteFeverTimebar.value = 0.51f;

        lerpValue = lerpValue50Percent;

        fullFeverTimeBarCount = 2;

        CalculateFeverTimeDuration();

        bar50PercentAnimator.Play("FeverTimeBarPercent", 0, 0f);

        menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

        feverTimeReady = true;

        hasSet50Percent = true;
    }

    // Bar has reached 75 percent
    private void SetBarTo75Percent()
    {
        feverTimeBar.value = 0.75f;
        whiteFeverTimebar.value = 0.76f;

        lerpValue = lerpValue75Percent;

        fullFeverTimeBarCount = 3;

        CalculateFeverTimeDuration();

        bar75PercentAnimator.Play("FeverTimeBarPercent", 0, 0f);

        menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

        feverTimeReady = true;

        hasSet75Percent = true;
    }

    // Bar has reached 100 percent
    private void SetBarTo100Percent()
    {
        feverTimeBar.value = 1f;
        whiteFeverTimebar.value = 1f;
        lerpValue = lerpValue100Percent;

        fullFeverTimeBarCount = 4;

        CalculateFeverTimeDuration();

        bar100PercentAnimator.Play("FeverTimeBarPercent", 0, 0f);

        menuSFXAudioSource.PlayOneShot(feverTimeReadySound);

        feverTimeReady = true;

        hasSet100Percent = true;
    }

    // Lerp the fever bar value to gradually go down
    public float LerpFeverBarDown()
    {
        float timeSinceStarted = timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTimeDown;

        var result = Mathf.Lerp(lerpValue, feverTimeBarMinValue, percentageComplete);

        return result;
    }


    // Check the first bar against the min value
    private void CheckFirstBarMinValue()
    {
        // Check if the feverTimeBar value is at 0
        if (feverTimeBar.value == feverTimeBarMinValue && hasSet0Percent == false)
        {
            feverTimeSideParticlesAnimator.gameObject.SetActive(false);

            // 0 bars are full
            fullFeverTimeBarCount = 0;

            // Update multiplier text 
            multiplierText.text = defaultMultiplier;
            shadowMultiplierText.text = defaultMultiplier;

            // Play animation
            multiplierTextAnimator.Play("DefaultMultiplier");

            // Reset combo
            feverTimeCombo = 0;

            // Reset lerp variables
            ResetLerpVariables();

            // Reset bar variables
            ResetBarVariables();

            // Deactivate fever time background
            feverTimeBackground.gameObject.SetActive(false);

            // Disable fever time
            feverTimeActivated = false;

            // Reset fever time ready bool to allow sound to play again when at max value
            hasPlayedFeverTimeReadySound = false;

            lerpValue = 0f;

    
            //menuSFXAudioSource.PlayOneShot(feverTimeDeactivatedSound);

            feverTimeReady = false;

            feverTimeBarFillImage.color = defaultBarColor;

            // Turn off audio reverb
            songAudioReverbFilter.enabled = false;
            // Turn off menuSFX reverb
            menuSFXAudioReverbFiler.enabled = false;

            // Set to true
            hasSet0Percent = true;
        }
    }

    // Reset lerp variables
    private void ResetLerpVariables()
    {
        timeStartedLerping = 0;
        shouldLerpDown = false;
    }

    // Reset bar variables - sound and current bar count
    private void ResetBarVariables()
    {
        hasPlayedFeverTimeReadySound = false;
        hasPlayedFeverTimeReadySound2 = false;
        hasPlayedFeverTimeReadySound3 = false;
        hasPlayedFeverTimeReadySound4 = false;
        fullFeverTimeBarCount = 0;

        hasSet0Percent = false;
        hasSet25Percent = false;
        hasSet50Percent = false;
        hasSet75Percent = false;
        hasSet100Percent = false;

        hasPlayedFeverTimeActivatedSound = false;
    }

    // Calculate the duration of fever time based on the amount of bars earned
    private void CalculateFeverTimeDuration()
    {
        // Get the time for 1 measure
        measureDuration = metronomeForEffects.GetMeasureDuration();
        
        switch (fullFeverTimeBarCount)
        {
            case 1:
                // 2 Measures
                lerpTimeDown = (measureDuration * 2);
                break;
            case 2:
                // 4 Measures
                lerpTimeDown = (measureDuration * 4);
                break;
            case 3:
                // 6 Measures
                lerpTimeDown = (measureDuration * 6);
                break;
            case 4:
                // 8 Measures
                lerpTimeDown = (measureDuration * 8);
                break;
        }
    }

    // Activate fever time
    private void ActivateFeverTime()
    {
        feverTimeSideParticlesAnimator.gameObject.SetActive(true);
        feverTimeSideParticlesAnimator.Play("FeverTimeSideParticles");

        // Update multiplier text 
        multiplierText.text = feverTimeMultiplier;
        shadowMultiplierText.text = feverTimeMultiplier;

        // Play animation
        multiplierTextAnimator.Play("FeverTimeMultiplier");

        // Calculate the duration of fever time based on the amount of bars earned
        CalculateFeverTimeDuration();



        // Change the bar color
        feverTimeBarFillImage.color = activatedBarColor;

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

        // Set shouldLerpDown to true to allow the fever bar value to go down
        shouldLerpDown = true;

        // Set to true as fever time has been activated
        feverTimeActivated = true;
    }

    // Increment the fever time combo
    public void IncrementFeverTimeCombo()
    {
        feverTimeCombo++;
    }
}
