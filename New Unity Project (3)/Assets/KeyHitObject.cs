using UnityEngine;
using UnityEngine.UI;

public class KeyHitObject : HitObject
{
    #region Variables
    // Image
    public Image outlineImage, innerImage;

    // Keycodes
    public KeyCode objectKey;
    public KeyCode[] incorrectKeys;
    #endregion

    #region Functions
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();

        AssignKeyType();
        AssignIncorrectKeys();
    }

    protected override void AssignColor()
    {
        switch (scriptManager.feverTimeManager.FeverActivated)
        {
            case true:
                AssignFeverColors();
                break;
            case false:
                AssignNormalColors();
                break;
        }
    }

    public override void AssignFeverColors()
    {
        switch (tag)
        {
            case Constants.HIT_OBJECT_TYPE_KEY_D_TAG:
                outlineImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_D;
                innerImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_D;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_F_TAG:
                outlineImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_F;
                innerImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_F;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_J_TAG:
                outlineImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_J;
                innerImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_J;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_K_TAG:
                outlineImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_K;
                innerImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_K;
                break;
        }

        colorImage.color = scriptManager.uiColorManager.solidWhiteColor;
    }

    public override void AssignNormalColors()
    {
        switch (tag)
        {
            case Constants.HIT_OBJECT_TYPE_KEY_D_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_D;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_F_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_F;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_J_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_J;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_K_TAG:
                colorImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_K;
                break;
        }

        outlineImage.color = scriptManager.uiColorManager.solidWhiteColor;
        innerImage.color = scriptManager.uiColorManager.solidWhiteColor;
    }

    protected override void CheckInput()
    {
        if (canBeHit == true)
        {
            if (Input.anyKeyDown)
            {
                CheckIncorrectInput();
                if (scriptManager.loadAndRunBeatmap.notesLocked == false)
                {
                    if (Input.GetKeyDown(objectKey))
                    {
                        if (hitObjectHit == false)
                        {
                            CheckJudgements();
                        }
                    }
                }
            }
        }
    }

    // Check for incorrect keys being pressed
    protected override void CheckIncorrectInput()
    {
        for (int i = 0; i < incorrectKeys.Length; i++)
        {
            if (Input.GetKeyDown(incorrectKeys[i]))
            {
                scriptManager.loadAndRunBeatmap.ResetLockNoteTimer();
                break;
            }
        }
    }

    protected override void PlayApproachAnimation()
    {
        hitObjectAnimator.Play("HitObject_FadeIn_Animation", 0, 0f);
    }

    // Assign keys
    protected virtual void AssignKeyType()
    {
        switch (tag)
        {
            case Constants.HIT_OBJECT_TYPE_KEY_D_TAG:
                objectKey = Constants.HIT_OBJECT_TYPE_KEY_D_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_F_TAG:
                objectKey = Constants.HIT_OBJECT_TYPE_KEY_F_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_J_TAG:
                objectKey = Constants.HIT_OBJECT_TYPE_KEY_J_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_K_TAG:
                objectKey = Constants.HIT_OBJECT_TYPE_KEY_K_KEYCODE;
                break;
        }
    }

    protected virtual void AssignIncorrectKeys()
    {
        incorrectKeys = new KeyCode[3];

        switch (tag)
        {
            case Constants.HIT_OBJECT_TYPE_KEY_D_TAG:
                incorrectKeys[0] = Constants.HIT_OBJECT_TYPE_KEY_F_KEYCODE;
                incorrectKeys[1] = Constants.HIT_OBJECT_TYPE_KEY_J_KEYCODE;
                incorrectKeys[2] = Constants.HIT_OBJECT_TYPE_KEY_K_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_F_TAG:
                incorrectKeys[0] = Constants.HIT_OBJECT_TYPE_KEY_D_KEYCODE;
                incorrectKeys[1] = Constants.HIT_OBJECT_TYPE_KEY_J_KEYCODE;
                incorrectKeys[2] = Constants.HIT_OBJECT_TYPE_KEY_K_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_J_TAG:
                incorrectKeys[0] = Constants.HIT_OBJECT_TYPE_KEY_D_KEYCODE;
                incorrectKeys[1] = Constants.HIT_OBJECT_TYPE_KEY_F_KEYCODE;
                incorrectKeys[2] = Constants.HIT_OBJECT_TYPE_KEY_K_KEYCODE;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_K_TAG:
                incorrectKeys[0] = Constants.HIT_OBJECT_TYPE_KEY_D_KEYCODE;
                incorrectKeys[1] = Constants.HIT_OBJECT_TYPE_KEY_J_KEYCODE;
                incorrectKeys[2] = Constants.HIT_OBJECT_TYPE_KEY_F_KEYCODE;
                break;
        }
    }
    #endregion
}
