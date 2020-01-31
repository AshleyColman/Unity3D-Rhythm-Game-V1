using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorManager : MonoBehaviour
{
    // Gameobjects
    public GameObject rotateLine, positionRotateLine;

    // Bools
    private bool shouldLerp;

    // Integers
    public List<float> beatsnapRotationList = new List<float>();
    private const float STARTROTATIONVALUE = 0f;
    private float perTickRotationValue = 25f;
    private float rotationValueToAdd;
    private float currentRotationValue;
    public float startRotationZ, endRotationZ;
    private float timer;
    public float timeToReachTarget;

    // Quaternion
    public Quaternion startRotation, endRotation;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public List<float> BeatsnapRotationList
    {
        get { return beatsnapRotationList; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Initialize
        timeToReachTarget = 0f;
        timer = 0f;
        rotationValueToAdd = 0f;
        currentRotationValue = 0f;
        startRotationZ = 0f;
        endRotationZ = 0f;
        startRotation = Quaternion.Euler(0, 0, 0);
        endRotation = Quaternion.Euler(0, 0, 0);

        shouldLerp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLerp == true)
        {
            if (beatsnapRotationList.Count != 0 && scriptManager.metronomePro.CurrentTick < scriptManager.metronomePro.songTickTimes.Count)
            {
                LerpToNextRotation();
            }
        }
    }

    // Rotation the grid to the value passed
    public void UpdateRotateGridRotation(float _rotationZ)
    {
        rotateLine.transform.rotation = new Quaternion(0, 0, _rotationZ, 0);
    }

    // Rotate the grid to the current tick rotation
    public void RotateGridToCurrentTickRotation()
    {
        // Get current tick rotation value
        float rotationValueZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick];
        // Update the rotation to the new value
        rotateLine.transform.rotation = Quaternion.Euler(0, 0, rotationValueZ);
    }

    public void ToggleLerpOn()
    {
        shouldLerp = true;
    }

    public void ToggleLerpOff()
    {
        shouldLerp = false;
    }

    public void ResetLerpVariables()
    {
        if (beatsnapRotationList.Count != 0)
        {
            if (scriptManager.metronomePro.CurrentTick + 1 < scriptManager.metronomePro.songTickTimes.Count)
            {
                // Get starting rotation (current tick rotation)
                startRotationZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick];

                // Get ending rotation (next tick rotation)
                endRotationZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick + 1];

                // Create vector for starting rotation
                startRotation = Quaternion.Euler(0, 0, startRotationZ);

                // Create vector for ending rotation
                endRotation = Quaternion.Euler(0, 0, endRotationZ);

                // Reset timer
                timer = 0f;
            }
        }
    }

    // Update the time to reach target for lerping rotation
    public void UpdateTimeToReachTarget()
    {
        // Update lerping time
        timeToReachTarget = (float)(scriptManager.metronomePro.songTickTimes[1] - scriptManager.metronomePro.songTickTimes[0]);
    }

    private void LerpToNextRotation()
    {
        // Increment timer
        timer += Time.deltaTime / timeToReachTarget;

        // Lerp rotation
        rotateLine.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timer);
    }

    // ROTATIONS SETUP
    public void CalculateRotations()
    {
        // Reset
        beatsnapRotationList.Clear();
        rotationValueToAdd = 0;
        currentRotationValue = 0;

        float divisionPerTickRotationValue = 0f;

        // Calculate the per tick rotation value based on the beat division being used
        switch (scriptManager.metronomePro.division)
        {
            case 0:
                // 1/1
                // Keep the same as the per tick rotation value
                divisionPerTickRotationValue = perTickRotationValue;
                break;
            case 1:
                // 1/2
                // Half the per tick rotation value
                divisionPerTickRotationValue = perTickRotationValue / 2;
                break;
            case 2:
                // 1/3
                divisionPerTickRotationValue = perTickRotationValue / 3;
                break;
            case 3:
                // 1/4
                divisionPerTickRotationValue = perTickRotationValue / 4;
                break;
        }


        for (int i = 0; i < scriptManager.metronomePro.songTickTimes.Count; i++)
        {
            if (i == 0)
            {
                // Add 0 at start of list
                beatsnapRotationList.Add(currentRotationValue);
            }
            else
            {
                // Add per tick rotation value onto the current rotation value
                rotationValueToAdd = currentRotationValue += divisionPerTickRotationValue;
            }

            // Add to list of rotations
            beatsnapRotationList.Add(currentRotationValue);
        }
    }

}
