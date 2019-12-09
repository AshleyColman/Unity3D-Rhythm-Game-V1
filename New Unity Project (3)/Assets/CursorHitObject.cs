using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHitObject : MonoBehaviour
{
    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        this.transform.position = scriptManager.mouseFollow.transform.position;
    }

}
