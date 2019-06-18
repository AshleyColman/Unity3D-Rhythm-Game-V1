using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridObjectPlacer : MonoBehaviour {

    private GridSnapManager grid;
    public GameObject editorGhostObject;
    public Vector3 editorGhostPosition;
    public GameObject editorCursor;
    public Vector3 editorCursorPosition;
    public bool snappingEnabled;

    // Mouse follow variables for the editor ghost object
    private float distance = 500f;
    public Vector3 pos;

    private void Awake()
    {
        grid = FindObjectOfType<GridSnapManager>();
    }

    private void Start()
    {
        snappingEnabled = true;
    }

    private void Update()
    {
        editorGhostObject = GameObject.FindGameObjectWithTag("EditorHitObject");

        if (editorGhostObject != null)
        {
            if (snappingEnabled == true)
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    float x = hitInfo.point.x;
                    float y = 0;
                    float z = hitInfo.point.z;
                    Vector3 clickPoint = new Vector3(x, y, z);
                    SnapGhostEditorObject(clickPoint);
                }
            }
            else
            {
                // Allow the ghost editor hit object to move to the mouse freely
                FreeMoveGhostEditorObject();
            }
        }



    }

    // Snap editor hit object ghost to the grid position
    private void SnapGhostEditorObject(Vector3 clickPointPass)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPointPass);
        editorGhostObject.transform.position = finalPosition;
    }

    // Allow the hit object ghost to be placed anywhere
    private void FreeMoveGhostEditorObject()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        pos = r.GetPoint(distance);
        editorGhostObject.transform.position = pos;
    }
}
