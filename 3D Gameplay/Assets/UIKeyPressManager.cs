using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyPressManager : MonoBehaviour {

    public Animator UIKeyPressAnimatorJ; // Animate the J key
    public Animator UIKeyPressAnimatorK; // Animate the K key
    public Animator UIKeyPressAnimatorL; // Animate the L key

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the blue key has been pressed
        if (Input.GetKey(KeyCode.J))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorJ);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorJ);
        }

        // If the purple key has been pressed
        if (Input.GetKey(KeyCode.K))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorK);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorK);
        }

        // If the red key has been pressed
        if (Input.GetKey(KeyCode.L))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorL);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorL);
        }

    }

    // Play the animation
    public void PlayUIKeyHeldAnimation(Animator animatorPass)
    {
        if (animatorPass == UIKeyPressAnimatorJ)
        { 
            animatorPass.Play("HeldJ");
        }
        if (animatorPass == UIKeyPressAnimatorK)
        {
            animatorPass.Play("HeldK");
        }
        if (animatorPass == UIKeyPressAnimatorL)
        {
            animatorPass.Play("HeldL");
        }



    }
    public void PlayUIKeyReleaseAnimation(Animator animatorPass)
    {
        if (animatorPass == UIKeyPressAnimatorJ)
        {
            animatorPass.Play("ReleaseJ");
        }
        if (animatorPass == UIKeyPressAnimatorK)
        {
            animatorPass.Play("ReleaseK");
        }
        if (animatorPass == UIKeyPressAnimatorL)
        {
            animatorPass.Play("ReleaseL");
        }
    }
}
