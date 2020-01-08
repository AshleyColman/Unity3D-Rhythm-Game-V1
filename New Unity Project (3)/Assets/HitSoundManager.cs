using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundManager : MonoBehaviour
{
    // Audiosource
    public AudioSource[] hitSoundAudioSourceArray; // The audio source that plays the hit sounds

    // Clips
    public AudioClip[] whistleHitSoundArray, clapHitSoundArray, finishHitSoundArray, missSoundArray;

    public AudioClip defaultHitSound, defaultMissSound;

    // Integers
    private float hitSoundVolume, missSoundVolume; // Hit and miss sound volume
    private int currentAudioSourceIndex;


    // Scripts
    private ScriptManager scriptManager;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // Initialize
        hitSoundVolume = 0.5f;
        missSoundVolume = 0.5f;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        LoadPlayerPrefsHitSoundVolume(); // Load the hit sound volume
    }

    // Set player prefs hit sound volume
    private void SetPlayerPrefsHitSoundVolume()
    {
        PlayerPrefs.SetFloat("hitSoundVolume", hitSoundVolume);
        PlayerPrefs.Save();
    }

    // Load the hit sound volume
    private void LoadPlayerPrefsHitSoundVolume()
    {
        if (PlayerPrefs.HasKey("hitSoundVolume"))
        {
            hitSoundVolume = PlayerPrefs.GetFloat("hitSoundVolume");
        }
    }

    // Lower hit sound volume
    public void LowerHitSoundVolume()
    {
        if (hitSoundVolume <= 0)
        {
            hitSoundVolume = 0;
        }
        else
        {
            hitSoundVolume = hitSoundVolume - 0.1f;
        }

        PlayHitSound();

        SetPlayerPrefsHitSoundVolume();
    }

    // Raise hit sound volume
    public void RaiseHitSoundVolume()
    {
        if (hitSoundVolume < 1)
        {
            hitSoundVolume = hitSoundVolume + 0.1f;
        }

        PlayHitSound();

        SetPlayerPrefsHitSoundVolume();
    }

    // Play the hit sound chosen
    public void PlayHitSound()
    {
        if (currentAudioSourceIndex >= hitSoundAudioSourceArray.Length)
        {
            currentAudioSourceIndex = 0;
        }

        hitSoundAudioSourceArray[currentAudioSourceIndex].PlayOneShot(defaultHitSound, hitSoundVolume);

        // Increment index
        currentAudioSourceIndex++;
    }

    // Play miss sound
    public void PlayMissSound()
    {
        if (currentAudioSourceIndex >= hitSoundAudioSourceArray.Length)
        {
            currentAudioSourceIndex = 0;
        }

        hitSoundAudioSourceArray[currentAudioSourceIndex].PlayOneShot(defaultMissSound, missSoundVolume);

        // Increment index
        currentAudioSourceIndex++;
    }
}
