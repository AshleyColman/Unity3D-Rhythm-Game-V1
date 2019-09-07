using UnityEngine;
using TMPro;

public class GridSnapManager : MonoBehaviour
{
    // Bools
    public bool snappingEnabled; // Controls whether the hit object is snapped to nearest grid points

    // Gameobjects
    public GameObject grid80Diamond, grid70Diamond, grid70Point, grid60Point; // Grid point options

    public TMP_Dropdown gridDropDown; // Selects the grid to display

    public int selectedGridIndex;

    public bool SnappingEnabled
    {
        get { return snappingEnabled; }
    }

    private void Start()
    {
        // Set snapping to default
        snappingEnabled = true;

        // Set grid drop down value
        gridDropDown.value = 2;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Increment the grid index
            selectedGridIndex++;

            // Activate the next grid
            ActivateNextGrid();
        }
    }

    // Disable all grids
    private void DeactivateAllGrids()
    {
        grid80Diamond.gameObject.SetActive(false);
        grid70Diamond.gameObject.SetActive(false);
        grid70Point.gameObject.SetActive(false);
        grid60Point.gameObject.SetActive(false);
    }

    // Activate the next grid based on the index
    public void ActivateNextGrid()
    {
        // If greater than the max grids available
        if (selectedGridIndex > 4)
        {
            // Reset
            selectedGridIndex = 0;
        }

        switch (selectedGridIndex)
        {
            case 0:
                selectedGridIndex = 0;
                grid70Diamond.gameObject.SetActive(false);
                grid70Point.gameObject.SetActive(false);
                grid60Point.gameObject.SetActive(false);
                grid80Diamond.gameObject.SetActive(false);
                snappingEnabled = false;
                break;
            case 1:
                selectedGridIndex = 1;
                grid80Diamond.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 2:
                selectedGridIndex = 2;
                grid70Diamond.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 3:
                selectedGridIndex = 3;
                grid70Point.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 4:
                selectedGridIndex = 4;
                grid60Point.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
        }

        // Update the activated grid drop down value
        gridDropDown.value = selectedGridIndex;
    }
    // Activate grid point selected
    public void ActivateGridSelected()
    {
        // Deactivate all grids 
        DeactivateAllGrids();

        // Activate the grid based on the dropdown selected
        switch (gridDropDown.value)
        {
            case 0:
                selectedGridIndex = 0;
                grid70Diamond.gameObject.SetActive(false);
                grid70Point.gameObject.SetActive(false);
                grid60Point.gameObject.SetActive(false);
                grid80Diamond.gameObject.SetActive(false);
                snappingEnabled = false;
                break;
            case 1:
                selectedGridIndex = 1;
                grid80Diamond.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 2:
                selectedGridIndex = 2;
                grid70Diamond.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 3:
                selectedGridIndex = 3;
                grid70Point.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
            case 4:
                selectedGridIndex = 4;
                grid60Point.gameObject.SetActive(true);
                snappingEnabled = true;
                break;
        }
    }
}
