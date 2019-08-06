using UnityEngine;

public class GridPoint : MonoBehaviour {

    // Gameobjects
    public GameObject hitObjectCursor; // Hit object cursor for placing hit objects on the grid
    // Scripts
    public GridSnapManager gridSnapManager;

    private MouseFollow mouseFollow;

    private void Start()
    {
        // Reference 
        mouseFollow = hitObjectCursor.GetComponent<MouseFollow>();
    }

    // Snap the hit object to position if the mouse has entered the grid button
    public void SnapHitObjectToPosition()
    {
        // If snapping is enabled
        if (gridSnapManager.snappingEnabled == true)
        {
            // Set the hit object position to the position of the grid button
            hitObjectCursor.transform.position = this.gameObject.transform.position;

            // Deactivate making the hit object snap
            mouseFollow.enabled = false;
        }
    }
}
