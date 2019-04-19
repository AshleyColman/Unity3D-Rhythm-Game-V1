using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitObject : MonoBehaviour {
    
    // Timer 
    private float timer;
    // Preview hit object destroy time
    private float destroyTime = 1.2f; 
    // Editor sound controller
    private EditorSoundController editorSoundController;

    // Reference to the PlacedObject script
    private PlacedObject placedObject;

    // Use this for initialization
    void Start()
    {
        // Get the reference to the PlacedObject script
        placedObject = FindObjectOfType<PlacedObject>();
        // Get the reference to the editor sound controller
        editorSoundController = FindObjectOfType<EditorSoundController>();
        // Set the timer to 0 
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {




        // If the beatmap preview has stopped stop incrementing the timer to allow the object to stay on screen and not be destroyed
        if (placedObject.playBeatmapPreview == false)
        {
            // Do not increment the timer so the object doesn't get destroyed
        }
        else
        {
            // If the beatmap preview has resumed allow the timer to increment again allowing the object to eventually be destroyed
            timer += Time.deltaTime;
        }

        // Destroy the game object
        if (timer >= destroyTime)
        {
            // Play the hit sound effect
            PlayHitSound();
            // Destroy the object
            DestroyHitObject();
        }
    }

    // Play the hit sound
    private void PlayHitSound()
    {
        editorSoundController.PlayPlacedSound();
    }

    // Destroy hit object
    private void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
