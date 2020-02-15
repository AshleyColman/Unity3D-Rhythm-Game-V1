using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BezierPoint : MonoBehaviour, IPointerClickHandler
{
    private int index;

    private const float segmentSelectDistanceThreshold = .1f;
    private int selectedSegmentIndex = -1;

    private ScriptManager scriptManager;

    public int Index
    {
        set { index = value; }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DeleteAnchorPoint();
        }
    }

    public void MoveAnchorPoint()
    {
        if (Input.GetMouseButton(0))
        {
            this.transform.localPosition = scriptManager.pathEditor.MousePositionToWorld();

            if (index == 0)
            {
                scriptManager.follower.transform.localPosition = scriptManager.pathEditor.MousePositionToWorld();
            }

            scriptManager.createdPath.MovePoint(index, transform.localPosition);

            scriptManager.pathEditor.UpdateAllPointPositions();

            scriptManager.roadCreator.UpdateRoad();
        }
    }

    private void DeleteAnchorPoint()
    {
        scriptManager.createdPath.DeleteSegment(index);
    }
}