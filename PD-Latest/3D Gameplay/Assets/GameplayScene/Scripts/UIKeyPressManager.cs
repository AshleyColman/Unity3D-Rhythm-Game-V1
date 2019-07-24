using UnityEngine;

public class UIKeyPressManager : MonoBehaviour {

    public Animator UIKeyPressAnimatorS; // Animate the S key
    public Animator UIKeyPressAnimatorD; // Animate the D key
    public Animator UIKeyPressAnimatorF; // Animate the F key

    public Animator UIKeyPressAnimatorJ; // Animate the J key
    public Animator UIKeyPressAnimatorK; // Animate the K key
    public Animator UIKeyPressAnimatorL; // Animate the L key


    // Update is called once per frame
    void Update()
    {
        // Check for key down/held input
        CheckForKeyInput();
    }

    // Check for key down/held input
    private void CheckForKeyInput()
    {
        // If the green key has been pressed
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Z))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorS, 1);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.Z))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorS, 1);
        }

        // If the yellow key has been pressed
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.X))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorD, 2);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.X))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorD, 2);
        }

        // If the orange key has been pressed
        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.C))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorF, 3);
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.C))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorF, 3);
        }


        // If the blue key has been pressed
        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.M))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorJ, 4);
        }
        if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.M))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorJ, 4);
        }

        // If the purple key has been pressed
        if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Comma))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorK, 5);
        }
        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Comma))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorK, 5);
        }

        // If the red key has been pressed
        if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Period))
        {
            // Play the animation
            PlayUIKeyHeldAnimation(UIKeyPressAnimatorL, 6);
        }
        if (Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.Period))
        {
            // Play the animation
            PlayUIKeyReleaseAnimation(UIKeyPressAnimatorL, 6);
        }
    }

    // Plays held key animations
    public void PlayUIKeyHeldAnimation(Animator _animator, int _keyNumberPressed)
    {
        switch (_keyNumberPressed)
        {
            case 1:
                _animator.Play("HeldS");
                break;
            case 2:
                _animator.Play("HeldD");
                break;
            case 3:
                _animator.Play("HeldF");
                break;
            case 4:
                _animator.Play("HeldJ");
                break;
            case 5:
                _animator.Play("HeldK");
                break;
            case 6:
                _animator.Play("HeldL");
                break;
        }
    }

    // Plays released key animationss
    public void PlayUIKeyReleaseAnimation(Animator _animator, int _keyNumberPressed)
    {
        switch (_keyNumberPressed)
        {
            case 1:
                _animator.Play("ReleaseS");
                break;
            case 2:
                _animator.Play("ReleaseD");
                break;
            case 3:
                _animator.Play("ReleaseF");
                break;
            case 4:
                _animator.Play("ReleaseJ");
                break;
            case 5:
                _animator.Play("ReleaseK");
                break;
            case 6:
                _animator.Play("ReleaseL");
                break;
        }
    }
}
