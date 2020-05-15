using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Variables
    // Animator
    private Animator cameraAnimator;
    #endregion

    #region Functions
    private void Start()
    {
        // Initialize
        cameraAnimator = this.gameObject.GetComponent<Animator>();
    }

    public void PlayCameraShakeAnimation()
    {
        cameraAnimator.Play("Camera_Shake_1_Animation", 0, 0f);
    }
    #endregion
}
