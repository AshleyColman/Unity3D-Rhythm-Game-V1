using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundManager : MonoBehaviour
{
    // Audiosource
    public AudioSource[] hitSoundAudioSource, missSoundAudioSource;

    // Clips
    public AudioClip[] whistleHitSoundArray, clapHitSoundArray, finishHitSoundArray, missSoundArray;

    public AudioClip defaultHitSound, defaultMissSound;

    // Integers
    private float hitSoundVolume, missSoundVolume; // Hit and miss sound volume
    private int currentHitSoundAudioSourceIndex, currentMissSoundAudioSourceIndex;


    // Scripts
    private ScriptManager scriptManager;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // Initialize
        hitSoundVolume = 1f;
        missSoundVolume = 1f;

        currentHitSoundAudioSourceIndex = 0;
        currentMissSoundAudioSourceIndex = 0;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        LoadPlayerPrefsHitSoundVolume(); // Load the hit sound volume

        for (int i = 0; i < hitSoundAudioSource.Length; i++)
        {
            hitSoundAudioSource[i].clip = defaultHitSound;
        }

        for (int i = 0; i < missSoundAudioSource.Length; i++)
        {
            missSoundAudioSource[i].clip = defaultMissSound;
        }
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
        if (currentHitSoundAudioSourceIndex >= hitSoundAudioSource.Length)
        {
            currentHitSoundAudioSourceIndex = 0;
        }

        hitSoundAudioSource[currentHitSoundAudioSourceIndex].Play();

        currentHitSoundAudioSourceIndex++;
    }

    // Play miss sound
    public void PlayMissSound()
    {
        if (currentMissSoundAudioSourceIndex >= missSoundAudioSource.Length)
        {
            currentMissSoundAudioSourceIndex = 0;
        }

        missSoundAudioSource[currentMissSoundAudioSourceIndex].Play();

        currentMissSoundAudioSourceIndex++;
    }
}
