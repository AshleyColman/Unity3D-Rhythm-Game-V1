using UnityEngine;
using TMPro;

public class KeyHitObject : HitObject
{
    #region Variables
    // Keycodes
    private KeyCode objectKey;

    // Text
    public TextMeshProUGUI keyText;
    #endregion

    #region Functions
    protected override void OnEnable()
    {
        base.OnEnable();
        AssignKeyType();
        keyText.color = colorImage.color;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckInput()
    {
        if (canBeHit == true)
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

    private void AssignKeyType()
    {
        switch (tag)
        {
            case Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE;
                keyText.text = Constants.KEY_HIT_OBJECT_TYPE_KEY1_CHAR;
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG:
                objectKey = Constants.KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE;
                keyText.text = Constants.KEY_HIT_OBJECT_TYPE_KEY2_CHAR;
                break;
        }
    }
    #endregion
}
