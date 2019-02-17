using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    private float timer;
    private float destroyTime = 1.2f; // Hit object destroy time

    public TimingAndScore timingAndScore;

	// Use this for initialization
	void Start () {

        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {

        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= destroyTime)
        {
            DestroyHitObject();
        }

        // If the buttons key has been pressed destroy object
        if (timingAndScore.hitObjectHit == true)
        {
            DestroyHitObject();
        }
    }

    public void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
