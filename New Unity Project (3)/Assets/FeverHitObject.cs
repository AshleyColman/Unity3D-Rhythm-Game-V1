using UnityEngine;
using UnityEngine.UI;

public class FeverHitObject : KeyHitObject
{
    #region Variables
    // Image
    public Image approachImage;

    // Int
    private int feverPhraseObjectIndex;
    #endregion

    #region Properties
    public int FeverPhraseObjectIndex
    {
        get { return feverPhraseObjectIndex; }
        set { feverPhraseObjectIndex = value; }
    }
    #endregion

    #region Functions
    protected override void OnEnable()
    {
        base.OnEnable();
        AssignApproachColor();
    }

    protected override void PlayApproachAnimation()
    {
        hitObjectAnimator.Play("HitObject_FadeIn_OuterApproach_Animation", 0, 0f);
    }

    protected override void AssignColor()
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
    }

    // Assign approach color based on whether the fever phrase has been broken or not
    private void AssignApproachColor()
    {
        if (scriptManager.loadAndRunBeatmap.FeverPhraseArr != null)
        {
            if (scriptManager.loadAndRunBeatmap.FeverPhraseArr.Length != 0)
            {
                switch (scriptManager.loadAndRunBeatmap.FeverPhraseArr[scriptManager.loadAndRunBeatmap.FeverPhraseToCheck].PhraseBroken)
                {
                    case true:
                        AssignUnactive();
                        break;
                    case false:
                        AssignActive();
                        break;
                }
            }
        }
    }

    public void AssignUnactive()
    {
        approachImage.color = scriptManager.uiColorManager.solidWhiteColor;
        outlineImage.color = scriptManager.uiColorManager.solidWhiteColor;
    }

    private void AssignActive()
    {
        switch (tag)
        {
            case Constants.HIT_OBJECT_TYPE_KEY_D_TAG:
                approachImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_D;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_F_TAG:
                approachImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_F;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_J_TAG:
                approachImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_J;
                break;
            case Constants.HIT_OBJECT_TYPE_KEY_K_TAG:
                approachImage.color = scriptManager.uiColorManager.HIT_OBJECT_COLOR_KEY_K;
                break;
        }

        outlineImage.color = scriptManager.uiColorManager.solidWhiteColor;
    }

    protected override void HasHit()
    {
        scriptManager.loadAndRunBeatmap.UpdateFeverPhrase(true, feverPhraseObjectIndex);
        base.HasHit();
    }

    protected override void MissedObject()
    {
        scriptManager.loadAndRunBeatmap.UpdateFeverPhrase(false, feverPhraseObjectIndex);
        base.MissedObject();
    }
    #endregion
}
