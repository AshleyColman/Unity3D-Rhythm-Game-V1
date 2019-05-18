using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsnapTimelineObject : MonoBehaviour {

    private float beatsnapTime;

    public float GetBeatsnapTime()
    {
        return beatsnapTime;
    }

    public void SetBeatsnapTime(float beatsnapTimePass)
    {
        beatsnapTime = beatsnapTimePass;
    }
}
