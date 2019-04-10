using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitSoundPreview : MonoBehaviour {

    public HitSoundDatabase hitSoundDatabase; // Required for loading all the hit sounds in the game
    public AudioSource hitSoundAudioSource; // The audio source that plays the hit sounds
    public int hitSoundChosenIndex; // The hit sound selected
    private float hitSoundVolume = 0.4f; // Volume of the hit sound

    // Song Select Menu UI 
    public TextMeshProUGUI hitSoundSelectedNumberText; // The number of the hit sound selected

    // Level changer
    public LevelChanger levelChanger; // The level changer

    void Start()
    {
        hitSoundChosenIndex = 0;
        hitSoundDatabase = FindObjectOfType<HitSoundDatabase>();
    }

    void Update()
    {
        // Find the current level
        levelChanger = FindObjectOfType<LevelChanger>();

        // Get the hit sound controller, an array is used as another will spawn when going back to the main menu, to prevent multiple from existing we store in an array
        GameObject[] hitSoundController = GameObject.FindGameObjectsWithTag("HitSoundManager");
        // Get the hit sound audio source for playing the hit sounds
        hitSoundAudioSource = GameObject.FindGameObjectWithTag("HitSoundManager").GetComponent<AudioSource>();


        // If the song select, gameplay or results scene do not destroy but destroy for all other scenes
        if (levelChanger.currentLevelIndex == 3 || levelChanger.currentLevelIndex == 4 || levelChanger.currentLevelIndex == 5)
        {
            // Dont destroy the hit sound manager
            DontDestroyOnLoad(this.gameObject);
        }

        // However if escape is pressed delete the hit sound manager
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("destroyed");
            Destroy(this.gameObject);
        }


        // Check if there are more than 1 hit sound controllers
        if (hitSoundController.Length > 1)
        {
            // Destroy is there is more than 1
            Destroy(hitSoundController[1].gameObject);
        }
        else
        {
            // Do not destroy any
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Increment the hit sound selected
    public void IncrementHitSoundSelected()
    {
        if (hitSoundChosenIndex == hitSoundDatabase.hitSoundClip.Length - 1)
        {
            // Do not increment
            Debug.Log("Do not increment");
        }
        else
        { 
            Debug.Log("increment");
            // Increase the hitsound chosen index
            hitSoundChosenIndex++;
            // Update the selected number text
            //hitSoundSelectedNumberText.text = (hitSoundChosenIndex + 1).ToString();
            // Play the new hit sound
            PlayHitSound();
        }
    }

    // Decrement the hit sound selected
    public void DecrementHitSoundSelected()
    {
        if (hitSoundChosenIndex == 0)
        {
            // Do not decrement
            Debug.Log("Do not decrement");
        }
        else
        {
            Debug.Log("decrement");
            // Decrement the chosen hit sound index
            hitSoundChosenIndex--;
            // Update the selected number text
            //hitSoundSelectedNumberText.text = (hitSoundChosenIndex + 1).ToString();
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
        hitSoundAudioSource.PlayOneShot(hitSoundDatabase.missSoundClip, hitSoundVolume);
    }
}
