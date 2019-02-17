using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour {

    // Lerp Variables
    private bool shouldLerp = false; // Starts the lerp

    public Vector3 maxScale; // Max scale value
    public Vector3 minScale; // Min scale value

    public float timeStartedLerping; // The time started lerping
    public float lerpTime;

    // Timer and spawn variables
    public float timer;
    private float perfectTime = 1f; // The perfect time to hit the hit object/max scale size for inner

    public Vector3 objectScale;

	// Use this for initialization
	void Start () {
        shouldLerp = true;
	}

    // Update is called once per frame
    void Update () {


        // Increments timer
        timer += Time.deltaTime;

        // Starts the lerp from maxScale to minScale and resets the timer
        if (timer >= lerpTime)
            {
                StartLerping();
            }

        // Started the lerping by changing the scale
        if (shouldLerp)
        {
           transform.localScale = Lerp(minScale, maxScale, timeStartedLerping, lerpTime);
           objectScale = transform.localScale;
        }

        // Delay the inner ring at max size for a few extra time
        if (timer >= perfectTime)
        {
            StartCoroutine(DelayInner());
        }

    }

    // Lerping scale function
    public Vector3 Lerp(Vector3 maxScale, Vector3 minScale, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(maxScale, minScale, percentageComplete);

        return result;
    }

    // Sets the time it started lerping, sets should lerp to true
    private void StartLerping()
    {
        timeStartedLerping = Time.time;

        shouldLerp = true;
    }

    // Delays the inner for a few seconds keeping it at max scale
    IEnumerator DelayInner()
    {
        transform.localScale = maxScale;
        yield return new WaitForSeconds(.2f);
    }


}
