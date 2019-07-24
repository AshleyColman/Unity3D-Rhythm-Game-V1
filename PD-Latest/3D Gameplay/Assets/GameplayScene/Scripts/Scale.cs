using System.Collections;
using UnityEngine;

public class Scale : MonoBehaviour {

    // Bools
    private bool shouldLerp;

    // Integers
    private float timeStartedLerping;
    private float lerpTime;
    private float timer;
    private float perfectTime; // The perfect time to hit the hit object/max scale size for inner
    private float positionX;

    // Vectors

    private Vector3 maxScale, minScale;

    // Scripts
    private PlayerSkillsManager playerSkillsManager; // Reference to the player skills manager to get the scale speed for the hit objects



    private void OnEnable()
    {
        timeStartedLerping = 0;
        timer = 0;
        transform.localScale = minScale;
    }

    // Use this for initialization
    void Start () {

        // Initialize
        shouldLerp = false;
        timeStartedLerping = 0;
        maxScale = new Vector3(4, 1, 4);
        minScale = new Vector3(0, 0, 0);

        // Reference
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
    public Vector3 Lerp(Vector3 _minScale, Vector3 _maxScale, float _timeStartedLerping, float lerpTime)
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
