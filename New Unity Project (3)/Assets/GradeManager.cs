using UnityEngine;

public class GradeManager : MonoBehaviour
{
    #region Functions
    // Calculate and return grade
    public string CalculateGrade(float _percentage)
    {
        if (_percentage == Constants.GRADE_S_REQUIRED)
        {
            return Constants.GRADE_S;
        }
        else if (_percentage >= Constants.GRADE_A_REQUIRED && _percentage < Constants.GRADE_S_REQUIRED)
        {
            return Constants.GRADE_A;
        }
        else if (_percentage >= Constants.GRADE_B_REQUIRED && _percentage < Constants.GRADE_A_REQUIRED)
        {
            return Constants.GRADE_B;
        }
        else if (_percentage >= Constants.GRADE_C_REQUIRED && _percentage < Constants.GRADE_B_REQUIRED)
        {
            return Constants.GRADE_C;
        }
        else if (_percentage >= Constants.GRADE_D_REQUIRED && _percentage < Constants.GRADE_C_REQUIRED)
        {
            return Constants.GRADE_D;
        }
        else if (_percentage >= Constants.GRADE_E_REQUIRED && _percentage < Constants.GRADE_D_REQUIRED)
        {
            return Constants.GRADE_E;
        }
        else
        {
            return Constants.GRADE_F;
        }
    }
    #endregion
}

