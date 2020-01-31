using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    private Vector3 startPosition, endPosition;
    private const float positionZ = 0f;
    private float timer;
    private float timeToReachTarget;
    private float speed = 5;
    private float distanceTravelled;
    private bool shouldLerp;
    private int currentPointIndex;

    private ScriptManager scriptManager;

    private void Start()
    {
        GetReferenceToScriptManager();
    }

    void Update()
    {
        if (shouldLerp == true)
        {
            LerpToNextRotation();
        }
    }

    // Turn lerp on
    public void ToggleLerpOn()
    {
        shouldLerp = true;
    }

    public void ToggleLerpOff()
    {
        shouldLerp = false;
    }

    // Get reference to the script manager
    private void GetReferenceToScriptManager()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
    }

    // Set transform position to the first point position
    public void SetToStartPosition()
    {
        GetReferenceToScriptManager();

        this.gameObject.transform.localPosition = new Vector3(scriptManager.pathPlacer.points[0].x, scriptManager.pathPlacer.points[0].y, positionZ);
    }

    private void LerpToNextRotation()
    {
        if ((currentPointIndex + 1) >= (scriptManager.pathPlacer.points.Length - 1))
        {
            currentPointIndex = 0;
        }

        /*
        if (timer >= timeToReachTarget)
        {
            timer = 0;
            currentPointIndex++;
        }
        */

        // Get start and end position based on the current point index
        startPosition = scriptManager.pathPlacer.points[currentPointIndex];
        endPosition = scriptManager.pathPlacer.points[currentPointIndex + 1];

        // Create vector3 position
        startPosition = new Vector3(startPosition.x, startPosition.y, positionZ);
        endPosition = new Vector3(endPosition.x, endPosition.y, positionZ);

        // Increment timer
        timer += Time.deltaTime / timeToReachTarget;

        // Lerp rotation
        this.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer);
    }

    public void SetTimeToReachTarget()
    {
        timeToReachTarget = (float)(scriptManager.metronomePro.songTickTimes[1] - scriptManager.metronomePro.songTickTimes[0]);
    }

    public void UpdateLerpToNextObject()
    {
        timer = 0;
        currentPointIndex++;
    }

}
