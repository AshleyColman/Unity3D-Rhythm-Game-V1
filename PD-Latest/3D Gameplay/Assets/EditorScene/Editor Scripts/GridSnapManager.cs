using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapManager : MonoBehaviour {

    public float size = 5f;
    private float gridPointSize = 320f;
    public GameObject gridPointObject;
    public bool snappingEnabled;
    private GridObjectPlacer gridObjectPlacer;

    private void Start()
    {
        gridObjectPlacer = FindObjectOfType<GridObjectPlacer>();
        snappingEnabled = true;
        DrawGridPoints();
    }

    private void Update()
    {
        // Enable snapping
        if (snappingEnabled == true)
        {
            // If scroll up
            if (Input.mouseScrollDelta.y > 0)
            {
                // Increase the grid size by x amount
                IncreaseGridPointSize();
                // Draw the new grid points
                DrawGridPoints();
            }

            // If scroll down
            if (Input.mouseScrollDelta.y < 0)
            {
                // Decrease the grid size by x amount
                DecreaseGridPointSize();
                // Draw the new grid points
                DrawGridPoints();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (snappingEnabled == true)
            {
                gridObjectPlacer.snappingEnabled = false;
                snappingEnabled = false;
            }
            else if (snappingEnabled == false)
            {
                // Redraw the grid points
                DrawGridPoints();
                gridObjectPlacer.snappingEnabled = true;
                snappingEnabled = true;
            }
        }

    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    // Draw the grid points on the editor screen
    public void DrawGridPoints()
    {
        for (float x = transform.position.x; x < gridPointSize; x += size)
        {
            for (float z = transform.position.z; z < 100; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Instantiate(gridPointObject, point, Quaternion.Euler(0, 45, 0));
            }
        }
    }

    // Increase the grid point size
    private void IncreaseGridPointSize()
    {
        size = size + 5;

        // Check if it's 0, if it is set to 10
        if (size >= 120)
        {
            size = 120;
        }

    }


    // Decrease the grid point size
    private void DecreaseGridPointSize()
    {
        size = size - 5;

        // Check if it's 0, if it is set to 10
        if (size <= 5)
        {
            size = 10;
        }
    }

    
}
