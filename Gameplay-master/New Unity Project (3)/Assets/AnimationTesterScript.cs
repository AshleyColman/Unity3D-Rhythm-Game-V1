using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTesterScript : MonoBehaviour
{
    public Animator cameraAnimator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cameraAnimator.Play("Camera_Shake_1_Animation", 0, 0f);
        }
    }
}
