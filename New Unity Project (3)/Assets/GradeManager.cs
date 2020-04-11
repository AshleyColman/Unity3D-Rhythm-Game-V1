using UnityEngine;

public class GradeManager : MonoBehaviour
{
    #region Variables
    private const string GRADE_S = "S", GRADE_A = "A", GRADE_B = "B", GRADE_C = "C", GRADE_D = "D", GRADE_E = "E", GRADE_F = "F";
    private const int GRADE_S_REQUIRED = 100, GRADE_A_REQUIRED = 90, GRADE_B_REQUIRED = 80, GRADE_C_REQUIRED = 70,
        GRADE_D_REQUIRED = 60, GRADE_E_REQUIRED = 50;
    #endregion

    #region Properties
    public int GradeSRequired { get { return GRADE_S_REQUIRED; } }
    public int GradeARequired { get { return GRADE_A_REQUIRED; } }
    public int GradeBRequired { get { return GRADE_B_REQUIRED; } }
    public int GradeCRequired { get { return GRADE_C_REQUIRED; } }
    public int GradeDRequired { get { return GRADE_D_REQUIRED; } }
    public int GradeERequired { get { return GRADE_E_REQUIRED; } }
    #endregion

    #region Functions
    // Calculate and return grade
    public string CalculateGrade(float _percentage)
    {
        if (_percentage == GRADE_S_REQUIRED)
        {
            return GRADE_S;
        }
        else if (_percentage >= GRADE_A_REQUIRED && _percentage < GRADE_S_REQUIRED)
        {
            return GRADE_A;
        }
        else if (_percentage >= GRADE_B_REQUIRED && _percentage < GRADE_A_REQUIRED)
        {
            return GRADE_B;
        }
        else if (_percentage >= GRADE_C_REQUIRED && _percentage < GRADE_B_REQUIRED)
        {
            return GRADE_C;
        }
        else if (_percentage >= GRADE_D_REQUIRED && _percentage < GRADE_C_REQUIRED)
        {
            return GRADE_D;
        }
        else if (_percentage >= GRADE_E_REQUIRED && _percentage < GRADE_D_REQUIRED)
        {
            return GRADE_E;
        }
        else
        {
            return GRADE_F;
        }
    }

    // Check the grade achieved
    public string CheckGradeAchieved(string _gradeAchieved, float _percentage)
    {
        switch (_gradeAchieved)
        {
            case GRADE_S:
                return GRADE_S;
                break;
            case GRADE_A:
                if (_percentage == GRADE_S_REQUIRED)
                {
                    return GRADE_S;
                }
                else
                {
                    return GRADE_A;
                }
                break;
            case GRADE_B:
                if (_percentage >= GRADE_A_REQUIRED && _percentage < GRADE_S_REQUIRED)
                {
                    return GRADE_A;
                }
                else
                {
                    return GRADE_B;
                }
                break;
            case GRADE_C:
                if (_percentage >= GRADE_B_REQUIRED && _percentage < GRADE_A_REQUIRED)
                {
                    return GRADE_B;
                }
                else
                {
                    return GRADE_C;
                }
                break;
            case GRADE_D:
                if (_percentage >= GRADE_C_REQUIRED && _percentage < GRADE_B_REQUIRED)
                {
                    return GRADE_C;
                }
                else
                {
                    return GRADE_D;
                }
                break;
            case GRADE_E:
                if (_percentage >= GRADE_D_REQUIRED && _percentage < GRADE_B_REQUIRED)
                {
                    return GRADE_D;
                }
                else
                {
                    return GRADE_E;
                }
                break;
            case GRADE_F:
                if (_percentage >= GRADE_E_REQUIRED && _percentage < GRADE_D_REQUIRED)
                {
                    return GRADE_E;
                }
                else
                {
                    return GRADE_F;
                }
                break;
            default:
                return _gradeAchieved;
                break;
        }
    }
    #endregion
}

