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
    public const int COMBO_BREAK = 5;
    public const int DEFAULT_MULTIPLIER = 1;
    public const int MULTIPLIER_COMBO_2X = 10, MULTIPLIER_COMBO_3X = 20, MULTIPLIER_COMBO_4X = 30;
    public const int STRING_LENGTH_1 = 1, STRING_LENGTH_2 = 2, STRING_LENGTH_3 = 3, STRING_LENGTH_4 = 4,
        STRING_LENGTH_5 = 5, STRING_LENGTH_6 = 6, STRING_LENGTH_7 = 7, STRING_LENGTH_8 = 8;
    public const int PERFECT_SCORE_VALUE = 500, GOOD_SCORE_VALUE = 250, EARLY_SCORE_VALUE = 100, MISS_SCORE_VALUE = 0;
    public const int MOUSE_HIT_OBJECT_TYPE_LEFT = 0;
    public const int MOUSE_HIT_OBJECT_TYPE_RIGHT = 1;
    public const int MOUSE_HIT_OBJECT_TYPE_UP = 2;
    public const int MOUSE_HIT_OBJECT_TYPE_DOWN = 3;
    public const int KEY_HIT_OBJECT_TYPE_KEY1 = 4;
    public const int KEY_HIT_OBJECT_TYPE_KEY2 = 5;
    public const int KEY_HIT_OBJECT_TYPE_KEY3 = 6;
    public const int KEY_HIT_OBJECT_TYPE_KEY4 = 7;
    public const float RESET_MOUSE_LEFT_POS_X = 100f;
    public readonly static float RESET_MOUSE_RIGHT_POS_X = Screen.width - 100f;
    public readonly static float RESET_MOUSE_UP_POS_Y = Screen.height - 100f;
    public readonly static float RESET_MOUSE_DOWN_POS_Y = 100f;
    public const float EXPLOSION_DEACTIVATE_TIME = 1f;
    public const float MULTIPLIER_PER_NOTE_SLIDER_VALUE = 0.033f;
    public const string MULTIPLIER_1X_STRING = "1x", MULTIPLIER_2X_STRING = "2x", MULTIPLIER_3X_STRING = "3x",
        MULTIPLIER_4X_STRING = "4x", MULTIPLIER_6X_STRING = "6x",
        MULTIPLIER_8X_STRING = "8x";
    public const string MOUSE_HIT_OBJECT_TYPE_LEFT_TAG = "LEFT";
    public const string MOUSE_HIT_OBJECT_TYPE_RIGHT_TAG = "RIGHT";
    public const string MOUSE_HIT_OBJECT_TYPE_UP_TAG = "UP";
    public const string MOUSE_HIT_OBJECT_TYPE_DOWN_TAG = "DOWN";
    public const string KEY_HIT_OBJECT_TYPE_KEY1_TAG = "KEY1";
    public const string KEY_HIT_OBJECT_TYPE_KEY2_TAG = "KEY2";
    public const string KEY_HIT_OBJECT_TYPE_KEY3_TAG = "KEY3";
    public const string KEY_HIT_OBJECT_TYPE_KEY4_TAG = "KEY4";
    public const string KEY_HIT_OBJECT_TYPE_KEY1_CHAR = "D F";
    public const string KEY_HIT_OBJECT_TYPE_KEY2_CHAR = "J K";
    public const string HIT_TAG = "HIT";
    public const string MISS_TAG = "MISS";
    public const string ZERO_SCORE_PREFIX_7 = "0000000";
    public const string ZERO_SCORE_PREFIX_6 = "000000";
    public const string ZERO_SCORE_PREFIX_5 = "00000";
    public const string ZERO_SCORE_PREFIX_4 = "0000";
    public const string ZERO_SCORE_PREFIX_3 = "000";
    public const string ZERO_SCORE_PREFIX_2 = "00";
    public const string ZERO_SCORE_PREFIX_1 = "0";
    public const string EARLY_SCORE_STRING = "100", GOOD_SCORE_STRING = "250", PERFECT_SCORE_STRING = "500";
    public const char COMBO_PREFIX = 'x', PERCENTAGE_PREFIX = '%', PLUS_PREFIX = '+';
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE = KeyCode.D;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY1_ALTERNATE_KEYCODE = KeyCode.F;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE = KeyCode.J;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY2_ALTERNATE_KEYCODE = KeyCode.K;
    #endregion
}
