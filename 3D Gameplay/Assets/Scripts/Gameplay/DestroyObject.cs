using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    private float timer;
    private float destroyTime = 1.2f; // Hit object destroy time
    private SoundController soundController; // Manage audio

	// Use this for initialization
	void Start () {

        soundController = FindObjectOfType<SoundController>(); // Find the sound controller to control the miss sound

        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {

        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= destroyTime)
        {
            soundController.PlayMissSound(); // Play the hitsound
            DestroyHitObject();
        }
    }

    // Destory hit object
    public void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
