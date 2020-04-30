using UnityEngine;

public class FeverHitObject : HitObject
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

    private void AssignKeyType()
    {
        switch (tag)
        {
            case Constants.START_FEVER_HIT_OBJECT_TYPE_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                break;
            case Constants.PHRASE_FEVER_HIT_OBJECT_TYPE_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                break;
            case Constants.FEVER_HIT_OBJECT_TYPE_KEY1_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_ALTERNATE_KEYCODE;
                break;
            case Constants.FEVER_HIT_OBJECT_TYPE_KEY2_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                alternateKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_ALTERNATE_KEYCODE;
                break;
            default:
                break;
        }
    }

    protected override void CheckInput()
    {
        if (canBeHit == true)
        {
            if (Input.anyKeyDown)
            {
                switch (tag)
                {
                    case Constants.START_FEVER_HIT_OBJECT_TYPE_TAG:
                        CheckBothKeyInput();
                        break;
                    case Constants.PHRASE_FEVER_HIT_OBJECT_TYPE_TAG:
                        CheckBothKeyInput();
                        break;
                    case Constants.FEVER_HIT_OBJECT_TYPE_KEY1_TAG:
                        CheckEitherKeyInput();
                        break;
                    case Constants.FEVER_HIT_OBJECT_TYPE_KEY2_TAG:
                        CheckEitherKeyInput();
                        break;
                    default:
                        break;
                }


            }
        }
    }

    private void CheckBothKeyInput()
    {
        if (Input.GetKeyDown(objectKey) && Input.GetKeyDown(alternateKey))
        {
            if (hitObjectHit == false)
            {
                CheckJudgements();
            }
        }
    }

    private void CheckEitherKeyInput()
    {
        if (Input.GetKeyDown(objectKey) || Input.GetKeyDown(alternateKey))
        {
            if (hitObjectHit == false)
            {
                CheckJudgements();
            }
        }
    }

    protected override void MissedObject()
    {
        base.MissedObject();
        scriptManager.feverTimeManager.BreakFeverPhrase();
    }

    protected override void HasHit()
    {
        CheckAddPhrase();
        base.HasHit();
    }

    private void CheckAddPhrase()
    {
        switch (tag)
        {
            case Constants.PHRASE_FEVER_HIT_OBJECT_TYPE_TAG:
                scriptManager.feverTimeManager.AddPhrase(tag);
                break;
        }
    }
    #endregion
}
