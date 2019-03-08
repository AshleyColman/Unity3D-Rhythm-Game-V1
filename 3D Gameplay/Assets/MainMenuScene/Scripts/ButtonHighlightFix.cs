using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightFix : MonoBehaviour {

    void Update()
    {
        //if mouse has moved clear selection
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            Debug.Log("Mouse Moved");
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

}
