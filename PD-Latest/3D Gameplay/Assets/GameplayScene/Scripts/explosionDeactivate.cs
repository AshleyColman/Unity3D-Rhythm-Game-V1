using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionDeactivate : MonoBehaviour {

    // Integers
    private float timer, deactivateTime; // Explosion deactivation time and timer

    // Reset the timer when reactivated
    private void OnEnable()
    {
        timer = 0f;
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        deactivateTime = 1.2f;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= deactivateTime)
        {
            Deactivate();
        }
    }

    // Deactivate
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
