using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParticles : MonoBehaviour {

    // Floats
    private float distance;
    // Vectors
    private Vector3 pos, lastMousePosition, newPos;
    // Rays
    private Ray ray; 

    private void Start()
    {
        // Initialize
        distance = 500f;
        lastMousePosition = Vector3.zero;
    }

    private void Update()
    {
        // Check if the mouse has moved / is the same as the previous mouse position in the previous frame
        if (Input.mousePosition != lastMousePosition)
        {
            // Update the cursor position
            UpdateCursorPosition();
        }

        // Set the last mouse position to the current mouse position to compare against next frame
        lastMousePosition = Input.mousePosition;
    }

    // Update the cursor position
    private void UpdateCursorPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        pos = ray.GetPoint(distance);
        newPos = new Vector3(pos.x, pos.y, 0);
        transform.position = newPos;
    }
}
