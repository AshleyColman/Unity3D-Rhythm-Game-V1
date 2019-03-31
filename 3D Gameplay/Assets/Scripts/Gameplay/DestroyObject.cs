using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    private float timer;
    private float destroyTime = 1.2f; // Hit object destroy time
    private HitSoundPreview hitSoundPreview; // Manage audio

	// Use this for initialization
	void Start () {

        hitSoundPreview = FindObjectOfType<HitSoundPreview>(); // Find the sound controller to control the miss sound
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {

        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= destroyTime)
        {
            hitSoundPreview.PlayMissSound(); // Play the hitsound
            DestroyHitObject();
        }
    }

    // Destory hit object
    public void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
