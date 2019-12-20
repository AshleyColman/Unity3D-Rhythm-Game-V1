using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorHitObject : MonoBehaviour
{
    private Quaternion diamondRotation = Quaternion.Euler(0, 0, 45);
    private Quaternion squareRotation = Quaternion.Euler(0, 0, 0);

    private ScriptManager scriptManager;

    private float angleRad, angleDeg;

    private const int DISTANCE_VALUE = 10, MAX_DISTANCE_VALUE = 250, MIN_DISTANCE_VALUE = 0;

    private int currentDistanceSnappingValue;

    public TextMeshProUGUI distanceSnappingText;

    public GameObject positionObject, positionObjectMask; // Object for setting positions at for hit objects
    public GameObject distanceSnappingGhostObject, distanceSnappingGhostObjectMask;

    private const KeyCode increaseDistanceKey = KeyCode.Alpha1, decreaseDistanceKey = KeyCode.Alpha2;

    public CanvasGroup canvasGroup;


    // Properties
    public Quaternion DiamondRotation
    {
        get { return diamondRotation; }
    }

    public Quaternion SquareRotation
    {
        get { return squareRotation; }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        angleRad = 0;
        angleDeg = 0;
    }

    void Update()
    {
        // 0 - NO SNAP
        // 1 - GRID SNAP
        // 2 - DISTANCE SNAP

        switch (scriptManager.gridsnapManager.snappingDropdown.value)
        {
            case 0:
                SetCursorPositionToMousePosition();
                break;
            case 1:

                break;
            case 2:
                if (Input.GetKeyDown(increaseDistanceKey))
                {
                    ChangeDistanceSnapping("+");
                }

                if (Input.GetKeyDown(decreaseDistanceKey))
                {
                    ChangeDistanceSnapping("-");
                }

                UpdateCursorRotation();
                break;
        }
    }

    // Set canvas group alpha to 0 to hide the cursor
    public void HideCursorHitObject()
    {
        canvasGroup.alpha = 0;
    }

    // Set canvas group alpha to 1 to enable
    public void EnableCursorHitObject()
    {
        canvasGroup.alpha = 1;
    }

    // Increase or decrease distance snapping value
    public void ChangeDistanceSnapping(string _operator)
    {
        float xPos = positionObject.transform.localPosition.x;

        switch (_operator)
        {
            case "+":
                if (xPos + DISTANCE_VALUE <= MAX_DISTANCE_VALUE)
                {
                    xPos = xPos += DISTANCE_VALUE;
                    currentDistanceSnappingValue += DISTANCE_VALUE;
                }
                break;
            case "-":
                if (xPos - DISTANCE_VALUE >= MIN_DISTANCE_VALUE)
                {
                    xPos = xPos -= DISTANCE_VALUE;
                    currentDistanceSnappingValue -= DISTANCE_VALUE;
                }
                break;
        }

        distanceSnappingText.text = "DISTANCE " + '\n' + currentDistanceSnappingValue.ToString();

        positionObject.transform.localPosition = new Vector3(xPos, positionObject.transform.localPosition.y, positionObject.transform.localPosition.z);
    }

    public void SetCursorPositionToMousePosition()
    {
        transform.position = Input.mousePosition;
    }

    public void UpdateDistanceSnapPosition()
    {
        // Update the main cursor objects position to be the distance snapped position
        transform.position = positionObject.transform.position;
    }

    private void DrawLineBetween()
    {
        Vector3 centerPos = new Vector3(positionObject.transform.position.x + distanceSnappingGhostObject.transform.position.x, 
            positionObject.transform.position.y + distanceSnappingGhostObject.transform.position.y) / 2f;

        //lineObject.transform.position = centerPos;
    }

    private void UpdateCursorRotation()
    {
        // Rotate the main cursor object
        angleRad = Mathf.Atan2(Input.mousePosition.y - transform.position.y, Input.mousePosition.x - transform.position.x);
        angleDeg = (180 / Mathf.PI) * angleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        // Keep the rotation for the position and distance snapping object at 0
        positionObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        distanceSnappingGhostObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        distanceSnappingText.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetToDiamondRotation()
    {
        // Set the masks rotation
        positionObjectMask.transform.rotation = diamondRotation;
        distanceSnappingGhostObjectMask.transform.rotation = diamondRotation;
    }

    public void SetToSquareRotation()
    {
        // Set the masks rotation
        positionObjectMask.transform.rotation = squareRotation;
        distanceSnappingGhostObjectMask.transform.rotation = squareRotation;
    }

}
