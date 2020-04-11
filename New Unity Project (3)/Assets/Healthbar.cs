using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    #region Variables
    // Integer
    private const int PERFECT_HEALTH_VALUE = 10, GOOD_HEALTH_VALUE = 5, EARLY_HEALTH_VALUE = -5, MISS_HEALTH_VALUE = -10;

    // Slider
    public Image healthbarSlider;
    #endregion
}
