using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGridPoint : MonoBehaviour {

   
    private void Update()
    {
        // If the scroll wheel is scrolled up or down destroy the grid point object, or if the tab button is pressed
        if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.Tab))
        {
            // Destroy
            Destroy(this.gameObject);
        }

    }
}
