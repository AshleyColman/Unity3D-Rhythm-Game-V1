using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridsnapManager : MonoBehaviour
{
    // Input field
    public TextMeshProUGUI gridSizeText;

    // Grid layout group
    public GridLayoutGroup gridLayoutGroup;

    // Dropdown
    public TMP_Dropdown snappingDropdown;

    // Gameobject 
    public GameObject gridSizeButton, distanceSnapSizeButton, gridLayout, rotateGrid;

    // Ints
    private int gridSizeX, gridSizeY;
    private const int maxGridSize = 200, minGridSize = 50, gridValue = 10;

    private List<float> beatsnapRotationList = new List<float>();
    private const float STARTROTATIONVALUE = 0f;
    private const float PERTICKROTATIONVALUE = 100f;
    private float rotationValueToAdd = 0f;
    private float currentRotationValue = 0f;
    public float startRotationZ, endRotationZ;

    public float lerpSpeed;
    public float timer;

    public bool shouldLerp;

    public Quaternion startRotation, endRotation;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public List<float> BeatsnapRotationList
    {
        get { return beatsnapRotationList; }
    }

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Set default layout values
        gridSizeX = 50;
        gridSizeY = 50;

        // Update the grid layout group
        gridLayoutGroup.cellSize = new Vector2(gridSizeX, gridSizeY);

        // Update snapping
        UpdateSnappingMethod();

        lerpSpeed = 1f;

        timer = 0f;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            // Get the difference between ticks, set it to the lerp time for rotations
            //lerpSpeed = (float)(scriptManager.metronomePro.songTickTimes[1] - scriptManager.metronomePro.songTickTimes[0]);

            if (shouldLerp == false)
            {
                shouldLerp = true;
            }
            else
            {
                shouldLerp = false;
            }
            
        }

        if (shouldLerp == true)
        {
            if (beatsnapRotationList.Count != 0 && scriptManager.metronomePro.CurrentTick < scriptManager.metronomePro.songTickTimes.Count)
            {
                timer += Time.deltaTime;

                LerpToNextRotation();
            }
        }

    }

    public void ResetLerpVariables()
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

    private void LerpToNextRotation()
    {
        // Lerp from start to end rotation over x seconds
        rotateGrid.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timer * lerpSpeed);
        
        //rotateGrid.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,0), Quaternion.Euler(0, 0, 300), Time.time * lerpSpeed);



        /*
        // Get starting rotation (current tick rotation)
        float startRotationZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick];
        // Get ending rotation (next tick rotation)
        float endRotationZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick + 1];


        // Create vector for starting rotation
        Quaternion startRotation = Quaternion.Euler(0, 0, startRotationZ);

        // Create vector for ending rotation
        Quaternion endRotation = Quaternion.Euler(0, 0, endRotationZ);

        // Lerp from start to end rotation over x seconds
        rotateGrid.transform.rotation = Quaternion.Slerp(startRotation, endRotation, Time.time * lerpSpeed);
                        */


    }

    // ROTATIONS SETUP
    public void CalculateRotations()
    {
        for (int i = 0; i < scriptManager.metronomePro.songTickTimes.Count; i++)
        {
            // Add per tick rotation value onto the current rotation value
            rotationValueToAdd = currentRotationValue += PERTICKROTATIONVALUE;

            // Add to list of rotations
            beatsnapRotationList.Add(currentRotationValue);
        }
    }

    // Rotation the grid to the value passed
    public void UpdateRotateGridRotation(float _rotationZ)
    {
        rotateGrid.transform.rotation = new Quaternion(0, 0, _rotationZ, 0);
    }

    // Rotate the grid to the current tick rotation
    public void RotateGridToCurrentTickRotation()
    {
        // Get current tick rotation value
        float rotationValueZ = beatsnapRotationList[scriptManager.metronomePro.CurrentTick];
        // Update the rotation to the new value
        rotateGrid.transform.rotation = Quaternion.Euler(0, 0, rotationValueZ);
    }

    public void UpdateSnappingMethod()
    {
        // 0 - NO SNAP
        // 1 - GRID SNAP
        // 2 - DISTANCE SNAP

        switch (snappingDropdown.value)
        {
            case 0:
                DeactivateDistanceSnapSizeButton();
                DeactivateGridSizeButton();
                scriptManager.cursorHitObject.ResetDistanceSnapValue();
                scriptManager.cursorHitObject.CursorHitObjectButtonInteractable(false);
                scriptManager.cursorHitObject.DisableCursorHitObjectRaycast();
                scriptManager.cursorHitObject.FollowMouse = true;
                scriptManager.cursorHitObject.DisableCursotHitObjectTextElements();
                gridLayout.gameObject.SetActive(false);
                break;
            case 1:
                DeactivateDistanceSnapSizeButton();
                ActivateGridSizeButton();
                scriptManager.cursorHitObject.ResetDistanceSnapValue();
                scriptManager.cursorHitObject.CursorHitObjectButtonInteractable(false);
                scriptManager.cursorHitObject.DisableCursorHitObjectRaycast();
                scriptManager.cursorHitObject.FollowMouse = false;
                scriptManager.cursorHitObject.DisableCursotHitObjectTextElements();
                //gridLayout.gameObject.SetActive(true);
                break;
            case 2:
                DeactivateGridSizeButton();
                ActivateDistanceSnapSizeButton();
                scriptManager.cursorHitObject.CursorHitObjectButtonInteractable(true);
                scriptManager.cursorHitObject.EnableCursorHitObjectRaycast();
                scriptManager.cursorHitObject.EnableStartingDistanceSnapPosition();
                scriptManager.cursorHitObject.EnableCursotHitObjectTextElements();
                gridLayout.gameObject.SetActive(false);
                break;
        }
    }

    private void DeactivateDistanceSnapSizeButton()
    {
        if (distanceSnapSizeButton.gameObject.activeSelf == true)
        {
            distanceSnapSizeButton.gameObject.SetActive(false);
        }
    }

    private void ActivateDistanceSnapSizeButton()
    {
        if (distanceSnapSizeButton.gameObject.activeSelf == false)
        {
            distanceSnapSizeButton.gameObject.SetActive(true);
        }
    }


    private void DeactivateGridSizeButton()
    {
        if (gridSizeButton.gameObject.activeSelf == true)
        {
            gridSizeButton.gameObject.SetActive(false);
        }
    }

    private void ActivateGridSizeButton()
    {
        if (gridSizeButton.gameObject.activeSelf == false)
        {
            gridSizeButton.gameObject.SetActive(true);
        }
    }

    // Update the grid layout with the text field values
    public void IncrementGridLayout()
    {
        if ((gridSizeX + gridValue) <= maxGridSize)
        {
            // Increment 
            gridSizeX += gridValue;

            // Apply to grid size y
            gridSizeY = gridSizeX;
        }

        gridSizeText.text = "GRID SIZE " + gridSizeX.ToString();

        gridLayoutGroup.cellSize = new Vector2(gridSizeX, gridSizeY);
    }

    // Update the grid layout with the text field values
    public void DecrementGridLayout()
    {
        if ((gridSizeX - gridValue) >= minGridSize)
        {
            // Decrement 
            gridSizeX -= gridValue;

            // Apply to grid size y
            gridSizeY = gridSizeX;
        }

        gridSizeText.text = "GRID SIZE " + gridSizeX.ToString();

        gridLayoutGroup.cellSize = new Vector2(gridSizeX, gridSizeY);
    }
}