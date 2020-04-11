using UnityEngine;

public class MouseHitObject : HitObject
{
    #region Variables
    // Float
    private float hitPos;

    // Bool
    private bool animationPlayed;
   
    // String
    private const string TAG_RIGHT = "RIGHT", TAG_LEFT = "LEFT", TAG_UP = "UP", TAG_DOWN = "DOWN";

    // Animator
    private Animator cameraAnimator;
    #endregion

    #region Functions
    protected override void OnEnable()
    {
        base.OnEnable();
        animationPlayed = false;
    }

    protected override void Start()
    {
        base.Start();

        animationPlayed = false;

        cameraAnimator = Camera.main.GetComponent<Animator>();
    }

    protected override void CheckInput()
    {
        if (canBeHit == true)
        {
            switch (tag)
            {
                case TAG_RIGHT:
                    if (Input.mousePosition.x >= Constants.RESET_MOUSE_RIGHT_POS_X)
                    {
                        if (hitObjectHit == false)
                        {
                            CheckJudgements();
                            PlaySwipeAnimation();
                            UpdateLatestHitMouseHitObject();
                            scriptManager.loadAndRunBeatmap.ResetMouse();
                        }
                    }
                    break;
                case TAG_LEFT:
                    if (Input.mousePosition.x <= Constants.RESET_MOUSE_LEFT_POS_X)
                    {
                        if (hitObjectHit == false)
                        {
                            CheckJudgements();
                            PlaySwipeAnimation();
                            UpdateLatestHitMouseHitObject();
                            scriptManager.loadAndRunBeatmap.ResetMouse();
                        }
                    }
                    break;
                case TAG_UP:
                    if (Input.mousePosition.y >= Constants.RESET_MOUSE_UP_POS_Y)
                    {
                        if (hitObjectHit == false)
                        {
                            CheckJudgements();
                            PlaySwipeAnimation();
                            UpdateLatestHitMouseHitObject();
                            scriptManager.loadAndRunBeatmap.ResetMouse();
                        }
                    }
                    break;
                case TAG_DOWN:
                    if (Input.mousePosition.y <= Constants.RESET_MOUSE_DOWN_POS_Y)
                    {
                        if (hitObjectHit == false)
                        {
                            CheckJudgements();
                            PlaySwipeAnimation();
                            UpdateLatestHitMouseHitObject();
                            scriptManager.loadAndRunBeatmap.ResetMouse();
                        }
                    }
                    break;
            }
        }
    }

    // Play swipe animation
    private void PlaySwipeAnimation()
    {
        if (animationPlayed == false)
        {
            switch (tag)
            {
                case TAG_RIGHT:
                    cameraAnimator.Play("Camera_Swipe_Right_Animation", 0, 0f);
                    break;
                case TAG_LEFT:
                    cameraAnimator.Play("Camera_Swipe_Left_Animation", 0, 0f);
                    break;
                case TAG_UP:
                    cameraAnimator.Play("Camera_Swipe_Up_Animation", 0, 0f);
                    break;
                case TAG_DOWN:
                    cameraAnimator.Play("Camera_Swipe_Down_Animation", 0, 0f);
                    break;
            }

            animationPlayed = true;
        }
    }

    private void UpdateLatestHitMouseHitObject()
    {
        switch (tag)
        {
            case TAG_RIGHT:
                scriptManager.loadAndRunBeatmap.LastHitMouseHitObject = Constants.MOUSE_HIT_OBJECT_TYPE_RIGHT;
                break;
            case TAG_LEFT:
                scriptManager.loadAndRunBeatmap.LastHitMouseHitObject = Constants.MOUSE_HIT_OBJECT_TYPE_LEFT;
                break;
            case TAG_UP:
                scriptManager.loadAndRunBeatmap.LastHitMouseHitObject = Constants.MOUSE_HIT_OBJECT_TYPE_UP;
                break;
            case TAG_DOWN:
                scriptManager.loadAndRunBeatmap.LastHitMouseHitObject = Constants.MOUSE_HIT_OBJECT_TYPE_DOWN;
                break;
        }

    }
    #endregion
}
