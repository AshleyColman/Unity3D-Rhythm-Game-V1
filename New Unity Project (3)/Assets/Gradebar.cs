using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gradebar : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI gradeTextS, gradeTextA, gradeTextB, gradeTextC, gradeTextD, gradeTextE, gradeTextF, previousGradeText;

    // Slider
    public Slider gradeSlider;

    // Script
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Update slider value
    public void UpdateGradeSlider()
    {
        gradeSlider.value = scriptManager.playInformation.CurrentPercentage;
    }

    // Update grade achieved
    public void UpdateGradeAchieved(string _grade)
    {
        // Deactivate previous grade achieved text
        previousGradeText.gameObject.SetActive(false);

        // Activate new grade achieved gameobject
        switch (_grade)
        {
            case Constants.GRADE_S:
                gradeTextS.gameObject.SetActive(true);
                previousGradeText = gradeTextS;
                break;
            case Constants.GRADE_A:
                gradeTextA.gameObject.SetActive(true);
                previousGradeText = gradeTextA;
                break;
            case Constants.GRADE_B:
                gradeTextB.gameObject.SetActive(true);
                previousGradeText = gradeTextB;
                break;
            case Constants.GRADE_C:
                gradeTextC.gameObject.SetActive(true);
                previousGradeText = gradeTextC;
                break;
            case Constants.GRADE_D:
                gradeTextD.gameObject.SetActive(true);
                previousGradeText = gradeTextD;
                break;
            case Constants.GRADE_E:
                gradeTextE.gameObject.SetActive(true);
                previousGradeText = gradeTextE;
                break;
            case Constants.GRADE_F:
                gradeTextF.gameObject.SetActive(true);
                previousGradeText = gradeTextF;
                break;
        }
    }
    #endregion
}
