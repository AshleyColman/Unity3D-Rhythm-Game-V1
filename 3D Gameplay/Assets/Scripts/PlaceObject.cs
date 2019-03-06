using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour {

    public GameObject editorHitObject;


    public void Update()
    {

    }

    public void InstantiateEditorHitObject()
    {
        Instantiate(editorHitObject, transform.position, Quaternion.Euler(0, 45, 0));
    }
}
