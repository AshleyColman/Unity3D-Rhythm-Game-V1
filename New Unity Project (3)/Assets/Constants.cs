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
    public const int KEY_HIT_OBJECT_TYPE_KEY1 = 4, KEY_HIT_OBJECT_TYPE_KEY2 = 5, KEY_HIT_OBJECT_TYPE_KEY3 = 6, 
        KEY_HIT_OBJECT_TYPE_KEY4 = 7;
    public const int START_FEVER_HIT_OBJECT_TYPE = 8, PHRASE_FEVER_HIT_OBJECT_TYPE = 9, FEVER_HIT_OBJECT_TYPE_KEY1 = 10,
        FEVER_HIT_OBJECT_TYPE_KEY2 = 11;
    public const int NO_FAIL_TIMER_DURATION = 10;
    public const int STREAK_INTERVAL = 5;
    public const float RED_HEALTH_VALUE = 35f, ORANGE_HEALTH_VALUE = 70f, GREEN_HEALTH_VALUE = 100f;
    public const float MISS_HEALTH_VALUE = -5f, PERFECT_HEALTH_VALUE = 5f;
    public const float RESET_MOUSE_LEFT_POS_X = 100f;
    public const float SCORE_LERP_DURATION = 0.2f, NOTE_LIGHT_LERP_DURATION = 0.2f;
    public const float FEVER_FILL_1 = 0.25f, FEVER_FILL_2 = 0.5f, FEVER_FILL_3 = 0.75f, FEVER_FILL_4 = 1.0f,
    PER_NOTE_FILL = 0.01f, DEACTIVATE_FEVER_VALUE = 0.0f;
    public readonly static float RESET_MOUSE_RIGHT_POS_X = Screen.width - 100f;
    public readonly static float RESET_MOUSE_UP_POS_Y = Screen.height - 100f;
    public readonly static float RESET_MOUSE_DOWN_POS_Y = 100f;
    public const float EXPLOSION_DEACTIVATE_TIME = 1f;
    public const string MULTIPLIER_1X_STRING = "1x", MULTIPLIER_2X_STRING = "2x", MULTIPLIER_3X_STRING = "3x",
        MULTIPLIER_4X_STRING = "4x", MULTIPLIER_6X_STRING = "6x",
        MULTIPLIER_8X_STRING = "8x";
    public const string STREAK_PREFIX = " NOTE STREAK!";
    public const string MOUSE_HIT_OBJECT_TYPE_LEFT_TAG = "LEFT";
    public const string MOUSE_HIT_OBJECT_TYPE_RIGHT_TAG = "RIGHT";
    public const string MOUSE_HIT_OBJECT_TYPE_UP_TAG = "UP";
    public const string MOUSE_HIT_OBJECT_TYPE_DOWN_TAG = "DOWN";
    public const string KEY_HIT_OBJECT_TYPE_KEY1_TAG = "KEY1", KEY_HIT_OBJECT_TYPE_KEY2_TAG = "KEY2", 
        KEY_HIT_OBJECT_TYPE_KEY3_TAG = "KEY3", KEY_HIT_OBJECT_TYPE_KEY4_TAG = "KEY4";
    public const string START_FEVER_HIT_OBJECT_TYPE_TAG = "START", PHRASE_FEVER_HIT_OBJECT_TYPE_TAG = "PHRASE",
        FEVER_HIT_OBJECT_TYPE_KEY1_TAG = "FEVER_KEY1", FEVER_HIT_OBJECT_TYPE_KEY2_TAG = "FEVER_KEY2";
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
    public const string TOTAL_EARLY_PREFIX = "E: ", TOTAL_GOOD_PREFIX = "G: ", TOTAL_PERFECT_PREFIX = "P: ",
        TOTAL_MISS_PREFIX = "M: ";
    public const string HEALTH_GUARD_STRING = "GUARD ", HEALTH_STRING = "HEALTH ";
    public const string EARLY_SCORE_STRING = "100", GOOD_SCORE_STRING = "250", PERFECT_SCORE_STRING = "500";
    public const string PERFECT_JUDGEMENT = "PERFECT", GOOD_JUDGEMENT = "GOOD", EARLY_JUDGEMENT = "EARLY", MISS_JUDGEMENT = "MISS";
    public const char COMBO_PREFIX = 'x', PERCENTAGE_PREFIX = '%', PLUS_PREFIX = '+', NEGATIVE_PREFIX = '-';
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY1_KEYCODE = KeyCode.F;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY1_ALTERNATE_KEYCODE = KeyCode.D;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY2_KEYCODE = KeyCode.J;
    public const KeyCode KEY_HIT_OBJECT_TYPE_KEY2_ALTERNATE_KEYCODE = KeyCode.K;
    public const KeyCode FEVER_ACTIVATION_KEY = KeyCode.T;
    #endregion

    #region SongSelect
    public const float LEFT_SIDE_GRADIENT_IMAGE_ALPHA = 0.5f;
    public const string EASY_DIFFICULTY = "easy", NORMAL_DIFFICULTY = "normal", HARD_DIFFICULTY = "hard";
    public const string GRADE_S = "S", GRADE_A = "A", GRADE_B = "B", GRADE_C = "C", GRADE_D = "D", GRADE_E = "E", GRADE_F = "F";
    public const int GRADE_S_REQUIRED = 100, GRADE_A_REQUIRED = 90, GRADE_B_REQUIRED = 80, GRADE_C_REQUIRED = 70,
        GRADE_D_REQUIRED = 60, GRADE_E_REQUIRED = 50;
    #endregion
}
