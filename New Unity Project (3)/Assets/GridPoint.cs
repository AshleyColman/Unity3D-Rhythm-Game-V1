using UnityEngine;

public class GridPoint : MonoBehaviour
{
    // Scripts
    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Snap the hit object to position if the mouse has entered the grid button
    public void SnapHitObjectToPosition()
    {
        // Set the hit object position to the position of the grid button
        scriptManager.cursorHitObject.transform.position = this.gameObject.transform.position;
    }
}
