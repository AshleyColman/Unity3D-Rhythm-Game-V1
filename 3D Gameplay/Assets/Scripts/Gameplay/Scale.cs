using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour {

    // Lerp Variables
    private bool shouldLerp = false; // Starts the lerp

    private Vector3 maxScale; // Max scale value
    private Vector3 minScale; // Min scale value

    public float timeStartedLerping; // The time started lerping
    private float lerpTime;
    private float timeToStartLerping;

    // Timer and spawn variables
    public float timer;
    private float perfectTime; // The perfect time to hit the hit object/max scale size for inner

    public Vector3 objectScale;

    public float positionX;

    // Reference to the player skills manager to get the scale speed for the hit objects
    private PlayerSkillsManager playerSkillsManager;


	// Use this for initialization
	void Start () {
        timeStartedLerping = 0;
        timeToStartLerping = 0;
        objectScale = transform.localScale;
        maxScale = new Vector3(4, 1, 4);
        minScale = new Vector3(0, 0, 0);

        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>(); // Get the reference for scale speed

        if (playerSkillsManager != null)
        {
            lerpTime = playerSkillsManager.GetFadeSpeedSelected(); // Get the fade speed selected such as 2 for slow, 1 for normal and 0.5f for fast
        }

        perfectTime = (lerpTime + 0.2f); // Perfect time is the time the object is destroyed, this is 0.2 seconds from the hit time
    }

    // Update is called once per frame
    void Update () {
        
        // Increments timer
        timer += Time.deltaTime;

        // Increment the time since spawned
        timeStartedLerping += Time.deltaTime;

        // Lerp scale
        transform.localScale = Lerp(minScale, maxScale, timeStartedLerping, lerpTime);

        // Delay the inner ring at max size for a few extra time
        if (timer >= perfectTime)
        {
            StartCoroutine(DelayInner());
        }

    }
    
    // Lerping scale function
    public Vector3 Lerp(Vector3 minScale, Vector3 maxScale, float timeStartedLerping, float lerpTime)
    {
        float timeSinceStarted = timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(minScale, maxScale, percentageComplete);

        return result;
    }
    
    // Delays the inner for a few seconds keeping it at max scale
    IEnumerator DelayInner()
    {
        transform.localScale = maxScale;
        yield return new WaitForSeconds(.2f);
    }
    
}
