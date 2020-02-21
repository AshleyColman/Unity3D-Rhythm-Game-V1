using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundManager : MonoBehaviour
{
    #region Variables
    // Audio 
    public AudioSource[] hitSoundAudioSource;
    public AudioSource[] missSoundAudioSource;
    public AudioClip defaultHitSound, defaultMissSound;

    // Integers
    private float hitSoundVolume, missSoundVolume; // Hit and miss sound volume
    private int hitSoundAudioSourceIndex, missSoundAudioSourceIndex; // Index for playing

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    void Start()
    {
        // Initialize
        hitSoundVolume = 1f;
        missSoundVolume = 1f;
        hitSoundAudioSourceIndex = 0;
        missSoundAudioSourceIndex = 0;

       // Reference
       scriptManager = FindObjectOfType<ScriptManager>();

        // Assign clips
        for (int i = 0; i < hitSoundAudioSource.Length; i++)
        {
            hitSoundAudioSource[i].clip = defaultHitSound;
            missSoundAudioSource[i].clip = defaultMissSound;
        }

        // Load the hit sound volume
        LoadPlayerPrefsHitSoundVolume(); 
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
        if (hitSoundAudioSourceIndex >= hitSoundAudioSource.Length)
        {
            hitSoundAudioSourceIndex = 0;
        }

        hitSoundAudioSource[hitSoundAudioSourceIndex].Play();

        hitSoundAudioSourceIndex++;
    }

    // Play miss sound
    public void PlayMissSound()
    {
        if (missSoundAudioSourceIndex >= missSoundAudioSource.Length)
        {
            missSoundAudioSourceIndex = 0;
        }

        missSoundAudioSource[missSoundAudioSourceIndex].Play();

        missSoundAudioSourceIndex++;
    }
    #endregion
}
