using UnityEngine;

public static class Constants
{
    #region Mouse
    public const int MAX_MOUSE_LEFT_POS_X = 0;
    public readonly static int MAX_MOUSE_RIGHT_POS_X = Screen.width;
    public readonly static int MAX_MOUSE_UP_POS_Y = Screen.height;
    public const int MAX_MOUSE_DOWN_POS_Y = 0;
    #endregion

    #region Gameplay
    public const int STRING_LENGTH_1 = 1, STRING_LENGTH_2 = 2, STRING_LENGTH_3 = 3, STRING_LENGTH_4 = 4,
        STRING_LENGTH_5 = 5, STRING_LENGTH_6 = 6, STRING_LENGTH_7 = 7, STRING_LENGTH_8 = 8;
    public const int PERFECT_SCORE_VALUE = 500, GOOD_SCORE_VALUE = 250, EARLY_SCORE_VALUE = 100, MISS_SCORE_VALUE = 0;
    public const int MOUSE_HIT_OBJECT_TYPE_LEFT = 0;
    public const int MOUSE_HIT_OBJECT_TYPE_RIGHT = 1;
    public const int MOUSE_HIT_OBJECT_TYPE_UP = 2;
    public const int MOUSE_HIT_OBJECT_TYPE_DOWN = 3;
    public const int KEY_HIT_OBJECT_TYPE_KEY1 = 4;
    public const int KEY_HIT_OBJECT_TYPE_KEY2 = 5;
    public const float RESET_MOUSE_LEFT_POS_X = 100f;
    public readonly static float RESET_MOUSE_RIGHT_POS_X = Screen.width - 100f;
    public readonly static float RESET_MOUSE_UP_POS_Y = Screen.height - 100f;
    public readonly static float RESET_MOUSE_DOWN_POS_Y = 100f;
    public const float EXPLOSION_DEACTIVATE_TIME = 1f;
    public const string MOUSE_HIT_OBJECT_TYPE_LEFT_TAG = "LEFT";
    public const string MOUSE_HIT_OBJECT_TYPE_RIGHT_TAG = "RIGHT";
    public const string MOUSE_HIT_OBJECT_TYPE_UP_TAG = "UP";
    public const string MOUSE_HIT_OBJECT_TYPE_DOWN_TAG = "DOWN";
    public const string KEY_HIT_OBJECT_TYPE_KEY1_TAG = "KEY1";
    public const string KEY_HIT_OBJECT_TYPE_KEY2_TAG = "KEY2";
    public const string KEY_HIT_OBJECT_TYPE_KEY1_CHAR = "D";
    public const string KEY_HIT_OBJECT_TYPE_KEY2_CHAR = "F";
    public const string HIT_TAG = "HIT";
    public const string MISS_TAG = "MISS";
    public const string ZERO_SCORE_PREFIX_7 = "0000000";
    public const string ZERO_SCORE_PREFIX_6 = "000000";
    public const string ZERO_SCORE_PREFIX_5 = "00000";
    public const string ZERO_SCORE_PREFIX_4 = "0000";
    public const string ZERO_SCORE_PREFIX_3 = "000";
    public const string ZERO_SCORE_PREFIX_2 = "00";
    public const string ZERO_SCORE_PREFIX_1 = "0";
    public const char COMBO_PREFIX = 'x', PERCENTAGE_PREFIX = '%';
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE = KeyCode.D;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE = KeyCode.F;
    #endregion
}
