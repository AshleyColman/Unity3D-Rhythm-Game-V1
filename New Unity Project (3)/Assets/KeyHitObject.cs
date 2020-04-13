using UnityEngine;
using TMPro;

public class KeyHitObject : HitObject
{
    #region Variables
    // Keycodes
    private KeyCode objectKey, alternateKey;
    #endregion

    #region Functions
    protected override void OnEnable()
    {
        base.OnEnable();
        AssignKeyType();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckInput()
    {
        if (canBeHit == true)
        {
            switch (tag)
            {
                case Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG:
                    CheckEitherInput();
                    break;
                case Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG:
                    CheckEitherInput();
                    break;
                case Constants.KEY_HIT_OBJECT_TYPE_KEY3_TAG:
                    CheckBothInput();
                    break;
                case Constants.KEY_HIT_OBJECT_TYPE_KEY4_TAG:
                    CheckBothInput();
                    break;
            }
        }
    }

    // Check either input keys
    private void CheckEitherInput()
    {
        if (Input.GetKeyDown(objectKey) || Input.GetKeyDown(alternateKey))
        {
            if (hitObjectHit == false)
            {
                CheckJudgements();
            }
        }
    }

    // Check both input keys
    private void CheckBothInput()
    {
        if (Input.GetKeyDown(objectKey) && Input.GetKeyDown(alternateKey))
        {
            if (hitObjectHit == false)
            {
                CheckJudgements();
            }
        }
    }

    private void AssignKeyType()
    {
        switch (tag)
        {
            case Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_ALTERNATE_KEYCODE;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_ALTERNATE_KEYCODE;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY3_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_ALTERNATE_KEYCODE;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY4_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_ALTERNATE_KEYCODE;
                break;
        }
    }
    #endregion
}
