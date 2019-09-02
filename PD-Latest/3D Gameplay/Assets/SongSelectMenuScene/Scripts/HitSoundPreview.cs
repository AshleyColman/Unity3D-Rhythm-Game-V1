using UnityEngine;

public class HitSoundPreview : MonoBehaviour {

    // Audiosource
    [SerializeField]
    private AudioSource hitSoundAudioSource; // The audio source that plays the hit sounds

    // Integers
    public int hitSoundChosenIndex; // The hit sound selected
    public float hitSoundVolume, missSoundVolume; // Hit and miss sound volume

    // Scripts
    private LevelChanger levelChanger; // Level changer for changing scenes
    private HitSoundDatabase hitSoundDatabase; // Required for loading all the hit sounds in the game

    void Start()
    {
        // Initialize
        hitSoundChosenIndex = 0;
        hitSoundVolume = 0.5f; 
        missSoundVolume = 0.5f; 

        // Reference
        hitSoundDatabase = FindObjectOfType<HitSoundDatabase>();

        // Functions
        LoadPlayerPrefsHitSoundSelectedIndex(); // Load the saved hit sound selected index if it exists
        LoadPlayerPrefsHitSoundVolume(); // Load the hit sound volume
    }

    // Set player prefs hit sound selected index 
    private void SetPlayerPrefsHitSoundSelectedIndex()
    {
        PlayerPrefs.SetInt("hitSoundChosenIndex", hitSoundChosenIndex);
        PlayerPrefs.Save();
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


    // Load the hit sound selected index from the player prefs loading
    private void LoadPlayerPrefsHitSoundSelectedIndex()
    {
        if (PlayerPrefs.HasKey("hitSoundChosenIndex"))
        {
            hitSoundChosenIndex = PlayerPrefs.GetInt("hitSoundChosenIndex");

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

    // Increment the hit sound selected
    public void IncrementHitSoundSelected()
    {
        if (hitSoundChosenIndex != hitSoundDatabase.hitSoundClip.Length - 1)
        {
            // Increase the hitsound chosen index
            hitSoundChosenIndex++;

            // Set the new hit sound selected in the player prefs for saving
            SetPlayerPrefsHitSoundSelectedIndex();

            // Play the new hit sound
            PlayHitSound();
        }
    }

    // Decrement the hit sound selected
    public void DecrementHitSoundSelected()
    {
        if (hitSoundChosenIndex != 0)
        {
            // Decrement the chosen hit sound index
            hitSoundChosenIndex--;

            // Set the new hit sound selected in the player prefs for saving
            SetPlayerPrefsHitSoundSelectedIndex();

            // Play the hit sound
            PlayHitSound();
        }
    }

    // Play the hit sound chosen
    public void PlayHitSound()
    {
        hitSoundAudioSource.PlayOneShot(hitSoundDatabase.hitSoundClip[hitSoundChosenIndex], hitSoundVolume);
    }

    // Play miss sound
    public void PlayMissSound()
    {
        hitSoundAudioSource.PlayOneShot(hitSoundDatabase.missSoundClip, missSoundVolume);
    }
}
