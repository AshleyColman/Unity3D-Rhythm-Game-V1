using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    #region Variables
    // Animation
    public Animator followerAnimator;

    // Integer
    private const float positionZ = 0f;
    private float timer;
    public float timeToReachTarget;
    private float distanceTravelled;

    // Vector
    private Vector3 startPosition, endPosition;

    // Bool
    private bool shouldLerp;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    // Properties
    public bool ShouldLerp
    {
        get { return shouldLerp; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        GetReferenceToScriptManager();
        timeToReachTarget = 1f;
    }

    void Update()
    {
        if (shouldLerp == true)
        {
            LerpToNextPoint();
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

    // Snap the position of the follower to the current tick point - 1 position
    public void SnapToCurrentPointPosition()
    {
        // Allow the position to be placed if the point on the line exists in the editor (only spawn if length is long enough)
        if (scriptManager.metronomePro.CurrentTick < scriptManager.pathPlacer.points.Length)
        {
            // Current tick - 1 as you cannot place a note on tick 0 as no tick is placed on the timeline
            this.transform.localPosition = scriptManager.pathPlacer.points[scriptManager.metronomePro.CurrentTick];
        }
    }

    // Set transform position to the first point position
    public void SetToStartPosition()
    {
        GetReferenceToScriptManager();

        this.gameObject.transform.localPosition = new Vector3(scriptManager.pathPlacer.points[0].x, scriptManager.pathPlacer.points[0].y, positionZ);
    }

    private void LerpToNextPoint()
    {
        // IF CURRENT TICK > TOTAL LENGTH OF TICK COUNT BASED ON LENGTH OF THE PATH
        if (scriptManager.metronomePro.CurrentTick < scriptManager.pathPlacer.points.Length)
        {
            // Get start and end position based on the current point index
            startPosition = scriptManager.pathPlacer.points[scriptManager.metronomePro.CurrentTick - 1];
            endPosition = scriptManager.pathPlacer.points[scriptManager.metronomePro.CurrentTick];

            // Create vector3 position
            startPosition = new Vector3(startPosition.x, startPosition.y, positionZ);
            endPosition = new Vector3(endPosition.x, endPosition.y, positionZ);

            // Increment timer
            timer += Time.deltaTime / timeToReachTarget;

            // Lerp rotation
            this.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer);
        }
        else
        {
            ToggleLerpOff();
        }
    }

    public void SetTimeToReachTarget()
    {
        timeToReachTarget = (float)(scriptManager.metronomePro.songTickTimes[1] - scriptManager.metronomePro.songTickTimes[0]);
    }

    public void UpdateLerpToNextObject()
    {
        timer = 0;
    }

    // Play the follower hit animation
    public void PlayFollowerHitAnimation()
    {
        followerAnimator.Play("Follower_Hit_Animation", 0, 0f);
    }
    #endregion
}
