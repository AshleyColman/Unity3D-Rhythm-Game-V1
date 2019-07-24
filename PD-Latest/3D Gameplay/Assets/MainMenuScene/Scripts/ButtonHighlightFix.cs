using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightFix : MonoBehaviour {

    private string axisX, axisY;

    private void Start()
    {
        axisX = "Mouse X";
        axisY = "Mouse Y";
    }

    void Update()
    {
        //if mouse has moved clear selection
        if ((Input.GetAxis(axisX) != 0) || (Input.GetAxis(axisY) != 0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

}
