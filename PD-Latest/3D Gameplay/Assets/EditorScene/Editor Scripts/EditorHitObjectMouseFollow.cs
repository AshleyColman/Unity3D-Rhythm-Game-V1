using UnityEngine;

public class EditorHitObjectMouseFollow : MonoBehaviour {

    // Audio
    public AudioSource menuSFXAudioSource;
    public AudioClip placedSound, selectedSound;

    // Bools
    private bool raycastObjectDragActive; // Is true when the hit object is following the mouse, false when it isn't

    // Scripts
    private PlacedObject placedObject;


    private void Start()
    {
        // Reference
        placedObject = FindObjectOfType<PlacedObject>();
    }


    // Update is called once per frame
    void Update()
    {
        // If M key is pressed start updating the instantiated editor hit object position
        if (Input.GetKeyDown(KeyCode.M) && raycastObjectDragActive == false)
        {
            // Set is dragging to true
            raycastObjectDragActive = true;
            // Play the selected sound
            PlaySelectedSound();
        }
        // If M key is pressed and previously was following the mouse
        else if (Input.GetKeyDown(KeyCode.M) && raycastObjectDragActive == true)
        {
            // Set is dragging to false
            raycastObjectDragActive = false;

            // Save the current position of the instantiated hit object?
            placedObject.SaveNewInstantiatedEditorObjectsPosition();

            // Play the placed sound
            PlayPlacedSound();
        }
    }

    // Play selectedSound
    private void PlaySelectedSound()
    {
        menuSFXAudioSource.PlayOneShot(selectedSound);
    }

    // Play the placed sound
    private void PlayPlacedSound()
    {
        menuSFXAudioSource.PlayOneShot(placedSound);
    }
}

