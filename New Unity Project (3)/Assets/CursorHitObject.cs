using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorHitObject : MonoBehaviour
{
    private float angleRad, angleDeg;
    private float previousPositionObjectPositionX, previousPositionObjectPositionY;
    private const int DISTANCE_VALUE = 10, MAX_DISTANCE_VALUE = 250, MIN_DISTANCE_VALUE = 0;

    private int currentDistanceSnappingValue;

    public TextMeshProUGUI distanceSnappingText, positionObjectPositionText;

    public GameObject positionObject, positionObjectMask; // Object for setting positions at for hit objects
    public GameObject distanceSnappingGhostObject, distanceSnappingGhostObjectMask;
    public GameObject borderObject, borderImage;

    public GameObject cursorHitObjectRaycastObject;

    private const KeyCode increaseDistanceKey = KeyCode.Alpha1, decreaseDistanceKey = KeyCode.Alpha2;

    private bool followMouse, placedStartingDistanceSnapPosition;

    public CanvasGroup canvasGroup;

    public Button cursorHitObjectButton;

    private ScriptManager scriptManager;

    // Properties

    public bool FollowMouse
    {
        set { followMouse = value; }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        angleRad = 0;
        angleDeg = 0;
        previousPositionObjectPositionX = 0;
        previousPositionObjectPositionY = 0;
        //followMouse = true;
        followMouse = false;
        placedStartingDistanceSnapPosition = false;
    }

    void Update()
    {
        if (scriptManager.gridsnapManager.snappingDropdown.value == 2)
        {
            // Update the distance snap object position text
            UpdatePositionObjectPositionText();

            if (placedStartingDistanceSnapPosition == true)
            {
                // Allow increase distance 
                if (Input.GetKeyDown(increaseDistanceKey))
                {
                    ChangeDistanceSnapping("+");
                }
                // Allow decrease distance
                if (Input.GetKeyDown(decreaseDistanceKey))
                {
                    ChangeDistanceSnapping("-");
                }
            }
        }

        // Allow the cursor to follow the mouse
        if (followMouse == true)
        {
            SetCursorPositionToMousePosition();
        }
    }

    // Disable the text elements on the cursor hit object
    public void DisableCursotHitObjectTextElements()
    {
        if (positionObjectPositionText.gameObject.activeSelf == true)
        {
            positionObjectPositionText.gameObject.SetActive(false);
        }

        if (distanceSnappingText.gameObject.activeSelf == true)
        {
            distanceSnappingText.gameObject.SetActive(false);
        }
    }

    // Enable the text elements on the cursor hit object
    public void EnableCursotHitObjectTextElements()
    {
        if (positionObjectPositionText.gameObject.activeSelf == false)
        {
            positionObjectPositionText.gameObject.SetActive(true);
        }

        if (distanceSnappingText.gameObject.activeSelf == false)
        {
            distanceSnappingText.gameObject.SetActive(true);
        }
    }
    // Enable the raycast objects for the cursor hit object
    public void EnableCursorHitObjectRaycast()
    {
        if (cursorHitObjectRaycastObject.activeSelf == false)
        {
            cursorHitObjectRaycastObject.SetActive(true);
        }
    }

    // Disable the raycast objects for the cursor hit object
    public void DisableCursorHitObjectRaycast()
    {
        if (cursorHitObjectRaycastObject.activeSelf == true)
        {
            cursorHitObjectRaycastObject.SetActive(false);
        }
    }

    // Control interactability for the cursor hit object button
    public void CursorHitObjectButtonInteractable(bool _interactable)
    {
        cursorHitObjectButton.interactable = _interactable;
    }

    public void UpdatePositionObjectPositionText()
    {
        if (positionObject.transform.position.x != previousPositionObjectPositionX ||
            positionObject.transform.position.y != previousPositionObjectPositionY)
        {
            positionObjectPositionText.text = "x: " + positionObject.transform.position.x.ToString("F2") + '\n' + 
            "y: " + positionObject.transform.position.y.ToString("F2");

            previousPositionObjectPositionX = positionObject.transform.position.x;
            previousPositionObjectPositionY = positionObject.transform.position.y;
        }
    }

    public void ToggleDistanceSnap()
    {
        switch (placedStartingDistanceSnapPosition)
        {
            case true:
                EnableStartingDistanceSnapPosition();
                break;
            case false:
                DisableStartingDistanceSnapPosition();
                break;
        }
    }

    public void EnableStartingDistanceSnapPosition()
    {
        // Set to false
        placedStartingDistanceSnapPosition = false;
        // Allow mouse follow
        followMouse = true;
    }

    // Enable distance snapping after the first position has been placed
    public void DisableStartingDistanceSnapPosition()
    {
        placedStartingDistanceSnapPosition = true;
        followMouse = false;
    }

    // Reset the distance snap value
    public void ResetDistanceSnapValue()
    {
        currentDistanceSnappingValue = MIN_DISTANCE_VALUE;

        UpdateDistanceSnappingText();

        positionObject.transform.localPosition = new Vector3(MIN_DISTANCE_VALUE, positionObject.transform.localPosition.y, positionObject.transform.localPosition.z);
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

        UpdateDistanceSnappingText();

        positionObject.transform.localPosition = new Vector3(xPos, positionObject.transform.localPosition.y, positionObject.transform.localPosition.z);
    }

    // Update the distance snapping text with the current snapping value
    private void UpdateDistanceSnappingText()
    {
        distanceSnappingText.text = "DISTANCE SNAP " + currentDistanceSnappingValue.ToString();
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
}
