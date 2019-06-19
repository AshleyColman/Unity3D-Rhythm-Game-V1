using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyPressManager : MonoBehaviour {

    public Animator UIKeyPressAnimatorS; // Animate the S key
    public Animator UIKeyPressAnimatorD; // Animate the D key
    public Animator UIKeyPressAnimatorF; // Animate the F key

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
        // If the green key has been pressed
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Z))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorS);
        }
        if (Input.GetKeyUp(KeyCode.S)|| Input.GetKeyUp(KeyCode.Z))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorS);
        }

        // If the yellow key has been pressed
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.X))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorD);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.X))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorD);
        }

        // If the orange key has been pressed
        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.C))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorF);
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.C))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorF);
        }


        // If the blue key has been pressed
        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.M))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorJ);
        }
        if (Input.GetKeyUp(KeyCode.J)|| Input.GetKeyUp(KeyCode.M))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorJ);
        }

        // If the purple key has been pressed
        if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Comma))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorK);
        }
        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Comma))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorK);
        }

        // If the red key has been pressed
        if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Period))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorL);
        }
        if (Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.Period))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorL);
        }

    }

    // Play the animation
    public void PlayUIKeyHeldAnimation(Animator animatorPass)
    {

        if (animatorPass == UIKeyPressAnimatorS)
        {
            animatorPass.Play("HeldS");
        }
        if (animatorPass == UIKeyPressAnimatorD)
        {
            animatorPass.Play("HeldD");
        }
        if (animatorPass == UIKeyPressAnimatorF)
        {
            animatorPass.Play("HeldF");
        }
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
        if (animatorPass == UIKeyPressAnimatorS)
        {
            animatorPass.Play("ReleaseS");
        }
        if (animatorPass == UIKeyPressAnimatorD)
        {
            animatorPass.Play("ReleaseD");
        }
        if (animatorPass == UIKeyPressAnimatorF)
        {
            animatorPass.Play("ReleaseF");
        }
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
