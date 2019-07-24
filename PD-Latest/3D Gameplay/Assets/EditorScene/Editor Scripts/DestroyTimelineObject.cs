using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DestroyTimelineObject : MonoBehaviour {

    private PlacedObject placedObject;
    public float timelineHitObjectSpawnTime;
    public Slider timelineSlider;
    public MetronomePro_Player metronome_Player;
    private int timelineObjectListIndex;

    private AudioSource menuSFXAudioSource;
    private HitSoundDatabase hitSoundDatabase;
    private AudioClip selectedSoundClip;
    private AudioClip clickedSoundClip;
    private AudioClip deletedSoundClip;


    BeatsnapManager beatsnapManager;

    public GameObject visibleTracker;

    private void Start()
    {
        beatsnapManager = FindObjectOfType<BeatsnapManager>();

        timelineObjectListIndex = 0;

        placedObject = FindObjectOfType<PlacedObject>();

        // Get the reference to the timelines own slider
        timelineSlider = this.gameObject.GetComponent<Slider>();

        // Get the reference to the metronome_Player
        metronome_Player = FindObjectOfType<MetronomePro_Player>();

        // Find the menuSFXAudioSource
        menuSFXAudioSource = GameObject.FindGameObjectWithTag("MenuSFXAudioSource").GetComponentInChildren<AudioSource>();

        // Get the hit sound database
        hitSoundDatabase = FindObjectOfType<HitSoundDatabase>();


        if (hitSoundDatabase != null)
        {
            // Assign the selected sound clip
            //selectedSoundClip = hitSoundDatabase.hitSoundClip[36];

            // Assign the clicked sound clip
            //clickedSoundClip = hitSoundDatabase.hitSoundClip[0];

            // Assign the deleteed sound clip
            //deletedSoundClip = hitSoundDatabase.hitSoundClip[42];
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
            Debug.Log("timelineHitObjectSpawnTime: " + timelineHitObjectSpawnTime);
            // Send the new spawn time and the editorHitObject index to update
            placedObject.UpdateEditorHitObjectSpawnTime(timelineHitObjectSpawnTime, timelineObjectListIndex);
        }
    }


    // Snap the timeline hit object to the nearsest beat snap
    public void SnapToNearestBeatsnap()
    {
        if (Input.GetMouseButton(0))
        {

            Debug.Log("run");
            // Get the slider for this timeline hit object


            // Get the slider value for this timeline hit object
            float hitObjectSliderValue = timelineSlider.value;
            //Debug.Log("slider value: " + hitObjectSliderValue);

            // Detect which beatsnap slider value the hit object slider value is closest to

            float nearest = beatsnapManager.beatsnapSliderValueList.Select(p => new { Value = p, Difference = Math.Abs(p - hitObjectSliderValue) })
                      .OrderBy(p => p.Difference)
                      .First().Value;


            //Debug.Log("nearest: " + nearest);
            // Set the hit object slider value to the closest beatsnap slider value
            timelineSlider.value = nearest;

            UpdateTimelineHitObjectSpawnTime();
        }

    }

}
