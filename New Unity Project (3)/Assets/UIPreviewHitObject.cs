using UnityEngine;
using UnityEngine.UI;

public class UIPreviewHitObject : MonoBehaviour
{

    // Bools
    private bool paused; // Controls pausing animations/keeping the hit object on screen when paused

    // UI
    public Image previewHitObjectGlowImage; // Glow component of the preview hit object
    public Image previewHitObjectInnerImage; // Inner component of the preview hit object

    // Floats
    public float timer; // Timer 
    private float deactivateTime; // Time to deactivate the hit object

    // Properties

    public float Timer
    {
        set { timer = value; }
    }

    public bool Paused
    {
        set { paused = value; }
    }

    // Use this for initialization
    void Start()
    {
        timer = 0f;
        deactivateTime = 1.20f;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {

        // If the preview hit object is active and not paused
        if (paused == false)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // If it is time to deactivate the preview hit object
            if (timer >= deactivateTime)
            {
                // Deactivate the gameobject
                Destroy(this.gameObject);
            }
        }
    }
}

