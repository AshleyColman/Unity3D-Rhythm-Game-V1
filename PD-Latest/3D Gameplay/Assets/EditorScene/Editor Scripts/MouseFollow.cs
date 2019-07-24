using UnityEngine;

public class MouseFollow : MonoBehaviour {

    // Floats
    private float distance;
    // Vectors
    private Vector3 pos, lastMousePosition;
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
        transform.position = pos;
    }
}
