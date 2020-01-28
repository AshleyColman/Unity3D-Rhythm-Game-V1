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
    public GameObject gridSizeButton, distanceSnapSizeButton, gridLayout;
    public List<GameObject> gridPointObjectList = new List<GameObject>(); // List of all grid point gameobjects
    public List<GameObject> positionGridPointObjectList = new List<GameObject>(); // List of invisible grid point gameobjects

    // Ints
    private int gridSizeX, gridSizeY;
    private const int maxGridSize = 200, minGridSize = 50, gridValue = 10;
    private int gridSnapPointIndex;
   
    // Scripts
    private ScriptManager scriptManager;

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

        gridSnapPointIndex = 0;

        // Snap cursor hit object to 1st gridpoint
        scriptManager.cursorHitObject.transform.SetParent(gridPointObjectList[0].transform);
        scriptManager.cursorHitObject.transform.position = gridPointObjectList[0].transform.position;
    }

    private void Update()
    {
        // Using arrow keys change position to the next gridpoint
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Decrement gridsnap point index
            gridSnapPointIndex--;

            // Error check
            if (gridSnapPointIndex < 0)
            {
                gridSnapPointIndex = 0;
            }

            // Change to next gridsnap
            scriptManager.cursorHitObject.transform.SetParent(gridPointObjectList[gridSnapPointIndex].transform);
            scriptManager.cursorHitObject.transform.position = gridPointObjectList[gridSnapPointIndex].transform.position;
        }

        // Using arrow keys change position to the next gridpoint
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Increment gridsnap point index
            gridSnapPointIndex++;

            // Error check
            if (gridSnapPointIndex > gridPointObjectList.Count - 1)
            {
                gridSnapPointIndex = gridPointObjectList.Count - 1;
            }

            // Change to next gridsnap
            scriptManager.cursorHitObject.transform.SetParent(gridPointObjectList[gridSnapPointIndex].transform);
            scriptManager.cursorHitObject.transform.position = gridPointObjectList[gridSnapPointIndex].transform.position;
        }
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