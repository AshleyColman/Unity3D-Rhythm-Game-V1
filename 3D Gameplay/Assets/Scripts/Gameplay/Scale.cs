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
    private float perfectTime = 1.2f; // The perfect time to hit the hit object/max scale size for inner

    public Vector3 objectScale;

    public float positionX;

	// Use this for initialization
	void Start () {
        timeStartedLerping = 0;
        lerpTime = 1;
        timeToStartLerping = 0;
        objectScale = transform.localScale;
        maxScale = new Vector3(5, 1, 5);
        minScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update () {
        
        // Increments timer
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);
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
