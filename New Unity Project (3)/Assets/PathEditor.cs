using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
public class PathEditor : MonoBehaviour
{
    private bool displayControlPoints = true;

    private const float segmentSelectDistanceThreshold = .1f;
    private int selectedSegmentIndex = -1;

    public GameObject anchorPointPrefab, controlPointPrefab;
    public Transform anchorPointSpawnCanvas, controlPointSpawnCanvas;

    public List<BezierPoint> pointScriptList = new List<BezierPoint>();

    public List<UILineRenderer> lineRendererList = new List<UILineRenderer>();

    public List<Vector2> pointlist = new List<Vector2>();

    private List<int> nullObjectList = new List<int>();

    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        for (int i = 0; i < scriptManager.createdPath.NumPoints; i++)
        {
            if (i % 3 == 0 || displayControlPoints == true)
            {
                if (i % 3 == 0)
                {
                    InstantiateNewPoint("ANCHOR", scriptManager.createdPath.points[i], i);
                }
                else
                {
                    InstantiateNewPoint("CONTROL", scriptManager.createdPath.points[i], i);
                }
            }
        }

        scriptManager.roadCreator.UpdateRoad();
    }

    private void Update()
    {
        //DrawLinesBetweenPoints();

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddNewPoint();
        }
    }

    private void DrawLinesBetweenPoints()
    {
        /*
        for (int i = 0; i < scriptManager.createdPath.NumSegments; i++)
        {
            Vector2[] points = scriptManager.createdPath.GetPointsInSegment(i);


            pointlist.Add(points[0]);
            pointlist.Add(points[1]);

            lineRenderer1.Points = pointlist.ToArray();

            pointlist.Clear();
        }
        */

    }

    public void UpdateAllPointPositions()
    {
        for (int i = 0; i < pointScriptList.Count; i++)
        {
            pointScriptList[i].transform.localPosition = scriptManager.createdPath.points[i];
        }
    }

    public void UpdatePointListIndex()
    {
        for (int i = 0; i < pointScriptList.Count; i++)
        {
            pointScriptList[i].Index = i;
        }
    }

    public void InstantiateNewPoint(string _type, Vector3 _position, int _index)
    {
        GameObject pointGameObject = new GameObject();

        switch (_type)
        {
            case "ANCHOR":
                pointGameObject = Instantiate(anchorPointPrefab, Vector3.zero, Quaternion.identity, anchorPointSpawnCanvas);
                break;
            case "CONTROL":
                pointGameObject = Instantiate(controlPointPrefab, Vector3.zero, Quaternion.identity, controlPointSpawnCanvas);
                break;
        }

        pointGameObject.transform.localPosition = _position;
        BezierPoint bezierPointScript = pointGameObject.GetComponent<BezierPoint>();
        bezierPointScript.Index = _index;
        pointScriptList.Add(bezierPointScript);
    }

    // Delete point
    public void DeletePoint(int _index)
    {
        Destroy(pointScriptList[_index].gameObject);
        pointScriptList.RemoveAt(_index);
        //scriptManager.createdPath.points.RemoveAt(_index);
    }

    public void RemoveNullFromPointScriptList()
    {
        int nullObjectIndex = 0;
        nullObjectList.Clear();

        for (int i = 0; i < pointScriptList.Count; i++)
        {
            if (pointScriptList[i] == null)
            {
                nullObjectIndex = i;
                nullObjectList.Add(nullObjectIndex);
            }
        }

        // Reset index
        nullObjectIndex = 0;

        // Remove all null objects
        for (int i = nullObjectList.Count - 1; i > -1; i--)
        {
            // Get the null object index from the list
            nullObjectIndex = nullObjectList[i];

            // Remove the script object from the list
            pointScriptList.RemoveAt(nullObjectIndex);
        }

        // Clear the list for next time
        nullObjectList.Clear();
    }

    private void AddNewPoint()
    {
        /*
        if (selectedSegmentIndex != -1)
        {
            scriptManager.createdPath.SplitSegment(MousePositionToWorld(), selectedSegmentIndex);
        }
        else if (!scriptManager.createdPath.IsClosed)
        {
            scriptManager.createdPath.AddSegment(MousePositionToWorld());
        }
        */

        scriptManager.createdPath.AddSegment(MousePositionToWorld());
        scriptManager.roadCreator.UpdateRoad();
    }

    // Convert mouse position to world click position
    public Vector3 MousePositionToWorld()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        return mousePos;
    }
}

