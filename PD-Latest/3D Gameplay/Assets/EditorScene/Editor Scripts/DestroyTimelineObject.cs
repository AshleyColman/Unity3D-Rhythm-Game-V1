using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DestroyTimelineObject : MonoBehaviour {

    // UI
    public Slider timelineSlider;
    // Integers
    private int timelineObjectListIndex;
    private float timelineHitObjectSpawnTime;
    private float hitObjectSliderValue;
    private float nearest;
    // Audio
    private AudioSource menuSFXAudioSource;
    private AudioClip selectedSoundClip;
    private AudioClip clickedSoundClip;
    private AudioClip deletedSoundClip;
    // Scripts
    private PlacedObject placedObject;
    private MetronomePro_Player metronome_Player;
    private HitSoundDatabase hitSoundDatabase;
    private BeatsnapManager beatsnapManager;

    // Properties

    public float TimelineHitObjectSpawnTime
    {
        set { timelineHitObjectSpawnTime = value; }
    }

    private void Start()
    {
        // Initialize
        timelineObjectListIndex = 0;
        // Reference
        beatsnapManager = FindObjectOfType<BeatsnapManager>();
        placedObject = FindObjectOfType<PlacedObject>();
        hitSoundDatabase = FindObjectOfType<HitSoundDatabase>();
        metronome_Player = FindObjectOfType<MetronomePro_Player>();
        menuSFXAudioSource = GameObject.FindGameObjectWithTag("MenuSFXAudioSource").GetComponentInChildren<AudioSource>();
        timelineSlider = this.gameObject.GetComponent<Slider>(); // Get the reference to the timelines own slider


        if (hitSoundDatabase != null)
        {
            // Assign the selected sound clip
            selectedSoundClip = hitSoundDatabase.hitSoundClip[13];

            // Assign the clicked sound clip
            clickedSoundClip = hitSoundDatabase.hitSoundClip[1];

            // Assign the deleteed sound clip
            deletedSoundClip = hitSoundDatabase.missSoundClip;
        }
    }

    // Play deleted sound
    public void PlayDeletedSound()
    {
        menuSFXAudioSource.PlayOneShot(deletedSoundClip);
    }

    // Play the selected sound
    public void PlaySelectedSound()
    {
        menuSFXAudioSource.PlayOneShot(selectedSoundClip);
    }

    // Play clicked sound
    public void PlayClickedSound()
    {
        menuSFXAudioSource.PlayOneShot(clickedSoundClip);
    }

    public void DestroyEditorTimelineObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Destroy the instantiated editorHitObject
            placedObject.DestroyInstantiatedEditorHitObject();
            // Play deleted sound effect
            PlayDeletedSound();
            // Check null timeline objects and update the list order/remove null objects from all lists
            placedObject.RemoveTimelineObject();
            // Destroy the game object
            Destroy(this.gameObject);
        }
    }
    
    public void FindIndexOfTimelineObject()
    {
        // Pass this game object
        timelineObjectListIndex = placedObject.GetIndexOfRaycastTimelineObject(this.gameObject);
    }

    // Update the timelines spawn time
    public void UpdateTimelineHitObjectSpawnTime()
    {
        if (metronome_Player != null)
        {
            timelineHitObjectSpawnTime = metronome_Player.UpdateTimelineHitObjectSpawnTimes(timelineSlider);
            // Send the new spawn time and the editorHitObject index to update
            placedObject.UpdateEditorHitObjectSpawnTime(timelineHitObjectSpawnTime, timelineObjectListIndex);
        }
    }

    // Snap the timeline hit object to the nearsest beat snap
    public void SnapToNearestBeatsnap()
    {
        if (Input.GetMouseButton(0))
        {
            // Get the slider value for this timeline hit object
            hitObjectSliderValue = timelineSlider.value;
            //Debug.Log("slider value: " + hitObjectSliderValue);

            // Detect which beatsnap slider value the hit object slider value is closest to

            nearest = beatsnapManager.beatsnapSliderValueList.Select(p => new { Value = p, Difference = Math.Abs(p - hitObjectSliderValue) })
                      .OrderBy(p => p.Difference)
                      .First().Value;


            //Debug.Log("nearest: " + nearest);
            // Set the hit object slider value to the closest beatsnap slider value
            timelineSlider.value = nearest;

            UpdateTimelineHitObjectSpawnTime();
        }
    }
}
