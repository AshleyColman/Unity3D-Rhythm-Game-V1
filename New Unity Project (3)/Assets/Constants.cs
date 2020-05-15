using UnityEngine;

public static class Constants
{
    #region Mouse
    // Int
    public const int MAX_MOUSE_LEFT_POS_X = 0;
    public readonly static int MAX_MOUSE_RIGHT_POS_X = Screen.width;
    public readonly static int MAX_MOUSE_UP_POS_Y = Screen.height;
    public const int MAX_MOUSE_DOWN_POS_Y = 0;
    #endregion

    #region Gameplay
    // Int
    public const int COMBO_BREAK = 5;
    public const int DEFAULT_MULTIPLIER = 1;
    public const int MULTIPLIER_COMBO_2X = 10, MULTIPLIER_COMBO_3X = 20, MULTIPLIER_COMBO_4X = 30;
    public const int STRING_LENGTH_1 = 1, STRING_LENGTH_2 = 2, STRING_LENGTH_3 = 3, STRING_LENGTH_4 = 4,
        STRING_LENGTH_5 = 5, STRING_LENGTH_6 = 6, STRING_LENGTH_7 = 7, STRING_LENGTH_8 = 8;
    public const int MAXPLUS_SCORE_VALUE = 550, MAX_SCORE_VALUE = 500, GREAT_SCORE_VALUE = 350, EARLY_SCORE_VALUE = 0, 
        LATE_SCORE_VALUE = 50, MISS_SCORE_VALUE = 0;
    public const int MOUSE_HIT_OBJECT_TYPE_LEFT = 0, MOUSE_HIT_OBJECT_TYPE_RIGHT = 1, MOUSE_HIT_OBJECT_TYPE_UP = 2,
        MOUSE_HIT_OBJECT_TYPE_DOWN = 3, HIT_OBJECT_TYPE_KEY_D = 4, HIT_OBJECT_TYPE_KEY_F = 5, HIT_OBJECT_TYPE_KEY_J = 6,
        HIT_OBJECT_TYPE_KEY_K = 7, FEVER_HIT_OBJECT_TYPE_KEY_D = 8, FEVER_HIT_OBJECT_TYPE_KEY_F = 9, 
        FEVER_HIT_OBJECT_TYPE_KEY_J = 10, FEVER_HIT_OBJECT_TYPE_KEY_K = 11;

    public const int NO_FAIL_TIMER_DURATION = 10;
    public const int STREAK_INTERVAL = 25;

    // Float
    public const float RED_HEALTH_VALUE = 35f, ORANGE_HEALTH_VALUE = 70f, GREEN_HEALTH_VALUE = 100f, MISS_HEALTH_VALUE = -5f,
        MAXPLUS_HEALTH_VALUE = 3, MAX_HEALTH_VALUE = 2, GREAT_HEALTH_VALUE = 1, LATE_HEALTH_VALUE = 1;
    public const float RESET_MOUSE_LEFT_POS_X = 100f;
    public const float SCORE_LERP_DURATION = 0.2f, NOTE_LIGHT_LERP_DURATION = 0.2f;
    public const float FEVER_FILL_1 = 0.25f, FEVER_FILL_2 = 0.5f, FEVER_FILL_3 = 0.75f, FEVER_FILL_4 = 1.0f,
    PER_NOTE_FILL = 0.01f, DEACTIVATE_FEVER_VALUE = 0.0f;
    public const float LOCK_NOTE_DURATION = 0.2f;
    public readonly static float RESET_MOUSE_RIGHT_POS_X = Screen.width - 100f, RESET_MOUSE_UP_POS_Y = Screen.height - 100f, 
        RESET_MOUSE_DOWN_POS_Y = 100f;
    public const float EXPLOSION_DEACTIVATE_TIME = 1f;
    public const float HIT_OBJECT_HIT_TIME = 1f, JUDGEMENT_START_TIME_EARLY = 0f, JUDGEMENT_END_TIME_EARLY = 0.88f,
        JUDGEMENT_START_TIME_GREAT = 0.88f, JUDGEMENT_END_TIME_GREAT = 0.96f, JUDGEMENT_START_TIME_MAX = 0.96f,
        JUDGEMENT_END_TIME_MAX = 1.04f, JUDGEMENT_START_TIME_MAXPLUS = 0.98f, JUDGEMENT_END_TIME_MAXPLUS = 1.02f,
        JUDGEMENT_START_TIME_LATE = 1.04f, JUDGEMENT_END_TIME_LATE = 1.1f, MISS_TIME = 1.1f;

    // String
    public const string MULTIPLIER_1X_STRING = "1x", MULTIPLIER_2X_STRING = "2x", MULTIPLIER_3X_STRING = "3x",
        MULTIPLIER_4X_STRING = "4x", MULTIPLIER_6X_STRING = "6x",
        MULTIPLIER_8X_STRING = "8x";
    public const string STREAK_PREFIX = " NOTE STREAK!";
    public const string MOUSE_HIT_OBJECT_TYPE_LEFT_TAG = "LEFT", MOUSE_HIT_OBJECT_TYPE_RIGHT_TAG = "RIGHT", 
        MOUSE_HIT_OBJECT_TYPE_UP_TAG = "UP", MOUSE_HIT_OBJECT_TYPE_DOWN_TAG = "DOWN";
    public const string HIT_OBJECT_TYPE_KEY_F_TAG = "KEY_F", HIT_OBJECT_TYPE_KEY_D_TAG = "KEY_D",
        HIT_OBJECT_TYPE_KEY_J_TAG = "KEY_J", HIT_OBJECT_TYPE_KEY_K_TAG = "KEY_K";
    public const string HIT_TAG = "HIT", MISS_TAG = "MISS";
    public const string ZERO_SCORE_PREFIX_7 = "0000000", ZERO_SCORE_PREFIX_6 = "000000", ZERO_SCORE_PREFIX_5 = "00000",
        ZERO_SCORE_PREFIX_4 = "0000", ZERO_SCORE_PREFIX_3 = "000", ZERO_SCORE_PREFIX_2 = "00", ZERO_SCORE_PREFIX_1 = "0";
    public const string TOTAL_EARLY_PREFIX = "E: ", TOTAL_GOOD_PREFIX = "G: ", TOTAL_PERFECT_PREFIX = "P: ",
        TOTAL_MISS_PREFIX = "M: ";
    public const string HEALTH_GUARD_STRING = "HEALTH LOCKED ", LOCKED_HEALTH_STRING = "X";
    public const string MAXPLUS_SCORE_STRING = "550", MAX_SCORE_STRING = "500", GREAT_SCORE_STRING = "350",
        LATE_SCORE_STRING = "50", EARLY_SCORE_STRING = "0";
    public const string MAXPLUS_JUDGEMENT = "MAX+", MAX_JUDGEMENT = "MAX", GREAT_JUDGEMENT = "GREAT",
        LATE_JUDGEMENT = "LATE", EARLY_JUDGEMENT = "EARLY", MISS_JUDGEMENT = "MISS";
    public const string FEVER_NOTES_REMAINING_STRING = "FEVER NOTES REMAINING ";
    public const string PERCENTAGE_TYPE_INCREASE = "INCREASE", PERCENTAGE_TYPE_DECREASE = "DECREASE";
    public const string BEING_PLAYED_BY_STRING = "Being played by ";
    // Char
    public const char COMBO_PREFIX = 'x', PERCENTAGE_PREFIX = '%', PLUS_PREFIX = '+', NEGATIVE_PREFIX = '-';

    // Keycode
    public const KeyCode HIT_OBJECT_TYPE_KEY_D_KEYCODE = KeyCode.D, HIT_OBJECT_TYPE_KEY_F_KEYCODE = KeyCode.F,
        HIT_OBJECT_TYPE_KEY_J_KEYCODE = KeyCode.J, HIT_OBJECT_TYPE_KEY_K_KEYCODE = KeyCode.K, 
        FEVER_ACTIVATION_KEYCODE = KeyCode.Space;
    #endregion

    #region SongSelect
    // Float
    public const float LEFT_SIDE_GRADIENT_IMAGE_ALPHA = 0.5f;
    
    // String
    public const string LEVEL_PREFIX = "LVL";
    public const string EASY_DIFFICULTY = "easy", NORMAL_DIFFICULTY = "normal", HARD_DIFFICULTY = "hard";
    public const string GRADE_S = "S", GRADE_A = "A", GRADE_B = "B", GRADE_C = "C", GRADE_D = "D", GRADE_E = "E", GRADE_F = "F",
        GRADE_TBD = "TBD";
    public const string BEATMAP_FOLDER = "Beatmaps";

    // Int
    public const int GRADE_S_REQUIRED = 100, GRADE_A_REQUIRED = 90, GRADE_B_REQUIRED = 80, GRADE_C_REQUIRED = 70,
        GRADE_D_REQUIRED = 60, GRADE_E_REQUIRED = 50, GRADE_F_REQUIRED = 40, GRADE_TBD_REQUIRED = 0;
    #endregion
}
