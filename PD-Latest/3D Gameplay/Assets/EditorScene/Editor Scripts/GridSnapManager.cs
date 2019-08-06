using UnityEngine;
using TMPro;

public class GridSnapManager : MonoBehaviour
{
    // Bools
    public bool snappingEnabled; // Controls whether the hit object is snapped to nearest grid points

    // Gameobjects
    public GameObject grid70Diamond, grid70Point; // Grid point options

    public TMP_Dropdown gridDropDown; // Selects the grid to display

    private void Start()
    {
        // Set snapping to default
        snappingEnabled = true;
    }

    // Disable all grids
    private void DeactivateAllGrids()
    {
        grid70Diamond.gameObject.SetActive(false);
        grid70Point.gameObject.SetActive(false);
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
                // Activate grid 1
                grid70Diamond.gameObject.SetActive(true);
                break;
            case 1:
                grid70Point.gameObject.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (snappingEnabled == true)
            {
                snappingEnabled = false;
            }
            else if (snappingEnabled == false)
            {
                snappingEnabled = true;
            }
        }
    }
}
