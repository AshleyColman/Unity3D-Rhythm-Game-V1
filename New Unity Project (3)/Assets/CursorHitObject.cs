using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHitObject : MonoBehaviour
{
    private Quaternion diamondRotation = Quaternion.Euler(0, 0, 45);
    private Quaternion squareRotation = Quaternion.Euler(0, 0, 0);

    private ScriptManager scriptManager;

    // Properties
    public Quaternion DiamondRotation
    {
        get { return diamondRotation; }
    }

    public Quaternion SquareRotation
    {
        get { return squareRotation; }
    }

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        this.transform.position = scriptManager.mouseFollow.transform.position;
    }

    public void SetToDiamondRotation()
    {
        this.transform.rotation = diamondRotation;
    }

    public void SetToSquareRotation()
    {
        this.transform.rotation = squareRotation;
    }

}
