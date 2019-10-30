using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillsManager : MonoBehaviour
{
    // UI
    public TextMeshProUGUI scoreMultiplierText, shadowScoreMultiplierText; // Score multiplier text in character select panel
    public TextMeshProUGUI modNameText, modDescriptionText;
    public Image tripleTimeSelectedKey, doubleTimeSelectedKey, halfTimeSelectedKey, noFailSelectedKey, instantDeathSelectedKey;
    public Image characterImageKey;

    public GameObject optionsPanel, songSelectPanel;
    public Scrollbar songSelectPanelScrollBar;

    public TextMeshProUGUI optionPanelText;

    // Gameobjects
    GameObject[] playerSkillsManagerArray; // Array of playerSkillsManager game objects

    // Animation    
    public Animator fadeSpeedAnimator; // Fade speed previous animator

    // Integers
    private float fadeSpeedSlow, fadeSpeedNormal, fadeSpeedFast; // Fade speed values
    private int scoreMultiplier; // Score multiplier values
    private int fadeSpeedSelectedIndex; // Fade speed index selected

    // Strings
    private string fadeSpeedSelected, scoreMultiplier075, scoreMultiplier100, scoreMultiplier105, scoreMultiplier110; // The fade speed selected, score multipliers
    private string tripleTimeTextValue, doubleTimeTextValue, halfTimeTextValue, judgementPlusTextValue, noFailTextValue,
        instantDeathTextValue; // Mod selected text values
    private string modSelected; // The mod selected


    // Bools
    private bool gameplayModKeysUpdated; // Controls updating the gameplay mod keys

    // Color
    public Color goodMultiplierColor, okayMultiplierColor, badMultiplierColor, defaultMultiplierColor; // Multiplier text colors

    public Color purpleColor, redColor, greenColor, orangeColor, yellowColor, blueColor, blackColor;

    // Scripts
    public SongProgressBar songProgressBar; // Reference required to change the song pitch when in gameplay

    // Keycodes
    private KeyCode destroyKey; // Key that destroys this gameobject





    // Properties

    // Get the mod selected
    public string ModSelected
    {
        get { return modSelected; }
    }

    // Get the no fail mod value
    public string NoFailTextValue
    {
        get { return noFailTextValue; }
    }

    // Get the scoremultipler
    public int ScoreMultiplier
    {
        get { return scoreMultiplier; }
    }

    // Get the fade speed selected
    public string FadeSpeedSelected
    {
        get { return fadeSpeedSelected; }
    }


    private void Start()
    {
        // Initialize
        fadeSpeedSelectedIndex = 1;
        fadeSpeedSlow = 2f;
        fadeSpeedNormal = 1f;
        fadeSpeedFast = 0.5f;
        scoreMultiplier = 100;
        scoreMultiplier075 = "x0.75";
        scoreMultiplier100 = "x1.00";
        scoreMultiplier105 = "x1.05";
        scoreMultiplier110 = "x1.10";
        modSelected = "";
        tripleTimeTextValue = "TRIPLE TIME";
        doubleTimeTextValue = "DOUBLE TIME";
        halfTimeTextValue = "HALF TIME";
        noFailTextValue = "NO FAIL";
        judgementPlusTextValue = "JUDGEMENT+";
        instantDeathTextValue = "INSTANT DEATH";
        destroyKey = KeyCode.Escape;

        optionPanelText.text = "";
        modNameText.text = "";
        modDescriptionText.text = "";

        characterImageKey.color = blackColor;

        // Functions
        LoadPlayerPrefsFadeSpeedSelectedIndex(); // Load the selected fade speed index from player prefs if it exists
    }

    public void UpdateOptionPanelText(string _option)
    {
        switch (_option)
        {
            case "CHARACTER":
                optionPanelText.text = "Equip character skills that impact gameplay and scoring";
                break;
            case "INCREASE VOLUME":
                optionPanelText.text = "Increase hit sound volume";
                break;
            case "DECREASE VOLUME":
                optionPanelText.text = "Decrease hit sound volume";
                break;
            case "NEXT SOUND":
                optionPanelText.text = "Next hit sound";
                break;
            case "PREVIOUS SOUND":
                optionPanelText.text = "Previous hit sound";
                break;
            case "PROFILE":
                optionPanelText.text = "Change your profile image";
                break;
            case "FADE":
                optionPanelText.text = "Change hit object fade speed";
                break;
            default:
                optionPanelText.text = "Choose an option";
                break;
        }
    }

    public void ShowOptions()
    {
        if (optionsPanel.activeSelf == true)
        {
            songSelectPanel.gameObject.SetActive(true);
            songSelectPanelScrollBar.gameObject.SetActive(true);
            optionsPanel.gameObject.SetActive(false);
        }
        else
        {
            songSelectPanel.gameObject.SetActive(false);
            songSelectPanelScrollBar.gameObject.SetActive(false);
            optionsPanel.gameObject.SetActive(true);
            optionPanelText.text = "Choose an option";
            PlayFadeSpeedSelectedAnimation();
        }
    }

    // Enable one of the speed mods during gameplay based on the mod selected
    private void EnableSpeedModsInGameplay()
    {
        switch (modSelected)
        {
            case "TRIPLE TIME":
                // Enable triple time if triple time is selected
                EnableTripleTimeInGameplay();
                break;
            case "DOUBLE TIME":
                // Enable double time if double time is selected
                EnableDoubleTimeInGameplay();
                break;
            case "HALF TIME":
                // Enable half time if half time is selected
                EnableHalfTimeInGameplay();
                break;
            default:
                break;
        }
    }

    // Update the scoreMultiplierText
    private void UpdateScoreMultiplierText(int scoreMultiplierValuePass)
    {
        // Add the new score multiplier to the existing multiplier
        scoreMultiplier = scoreMultiplierValuePass;

        // Original note values per score are single values such as 1, 2 and 5
        // Because we cannot submit float and use 1.00 or 0.50 for multiplier we use x100 as default and x50 for (0.5) when multipliers applied
        // So 1.00 = x100
        // 5 x 100 = 500 points per note
        // 5 x 50 (half time) = 250 points per note

        switch (scoreMultiplier)
        {
            case 75:
                scoreMultiplierText.text = scoreMultiplier075;
                shadowScoreMultiplierText.text = scoreMultiplier075;
                scoreMultiplierText.color = badMultiplierColor;
                break;
            case 100:
                scoreMultiplierText.text = scoreMultiplier100;
                shadowScoreMultiplierText.text = scoreMultiplier100;
                scoreMultiplierText.color = defaultMultiplierColor;
                break;
            case 105:
                scoreMultiplierText.text = scoreMultiplier105;
                shadowScoreMultiplierText.text = scoreMultiplier105;
                scoreMultiplierText.color = okayMultiplierColor;
                break;
            case 110:
                scoreMultiplierText.text = scoreMultiplier110;
                shadowScoreMultiplierText.text = scoreMultiplier110;
                scoreMultiplierText.color = goodMultiplierColor;
                break;
        }
    }

    // Reset timescale back to normal
    private void ResetGameSpeedToNormal()
    {
        // Change the game speed
        Time.timeScale = 1.0f;
    }

    // Reset the song pitch to normal
    private void ResetSongPitchToNormal()
    {
        // Change the song pitch
        songProgressBar.songAudioSource.pitch = 1.00f;
    }

    // Enable triple time in gameplay
    private void EnableTripleTimeInGameplay()
    {
        // Set the gameplay speed to x2
        Time.timeScale = 2.0f;

        // Also update the pitch of the song when in gameplay
        songProgressBar.songAudioSource.pitch = 2.00f;
    }

    // Enable double time in gameplay
    private void EnableDoubleTimeInGameplay()
    {
        // Set the gameplay speed to x1.5
        Time.timeScale = 1.5f;

        songProgressBar.songAudioSource.pitch = 1.5f;
    }

    // Enable half time in gameplay
    private void EnableHalfTimeInGameplay()
    {
        // Set the gameplay speed to x0.75
        Time.timeScale = 0.80f;

        songProgressBar.songAudioSource.pitch = 0.80f;
    }

    // Reset equiped skills
    public void ResetEquipedSkills()
    {
        modSelected = "";

        // Reset the multiplier score text with default value
        UpdateScoreMultiplierText(100);

        UpdateSelectedModKeys();
    }

    // Equip half time skill
    public void EquipHalfTimeSkill()
    {
        modSelected = "HALF TIME";

        UpdateScoreMultiplierText(75);

        UpdateSelectedModKeys();
    }

    // Equip double time skill
    public void EquipDoubleTimeSkill()
    {
        modSelected = "DOUBLE TIME";

        UpdateScoreMultiplierText(105);

        UpdateSelectedModKeys();
    }

    // Equip triple time skill
    public void EquipTripleTimeSkill()
    {
        modSelected = "TRIPLE TIME";

        tripleTimeSelectedKey.gameObject.SetActive(true);

        UpdateScoreMultiplierText(110);

        UpdateSelectedModKeys();
    }

    // Equip judgement plus skill
    public void EquipJudgementPlusSkill()
    {
        modSelected = "JUDGEMENT+";

        UpdateScoreMultiplierText(105);

        UpdateSelectedModKeys();
    }

    // Equip no fail skill
    public void EquipNoFailSkill()
    {
        modSelected = "NO FAIL";

        UpdateScoreMultiplierText(75);

        UpdateSelectedModKeys();
    }

    // Equip instant death skill
    public void EquipInstantDeathSkill()
    {
        modSelected = "INSTANT DEATH";

        UpdateScoreMultiplierText(100);

        UpdateSelectedModKeys();
    }

    // Increase the fade speed selected as the button preview has been pressed +
    public void IncrementFadeSpeedSelected()
    {
        if (fadeSpeedSelectedIndex == 2)
        {
            // Reset to 0 for slow speed for looping
            fadeSpeedSelectedIndex = 0;
        }
        else
        {
            // Increment the fade speed 
            fadeSpeedSelectedIndex++;
        }

        // Save the fade speed index in player prefs
        SetPlayerPrefsFadeSpeedSelectedIndex();
        // Play the fade speed animation
        PlayFadeSpeedSelectedAnimation();
    }

    // Decrease the fade speed selected as the button preview has been pressed -
    public void DecrementFadeSpeedSelected()
    {
        if (fadeSpeedSelectedIndex != 0)
        {
            // Decrement the fade speed selected
            fadeSpeedSelectedIndex--;
            // Save the fade speed selected index in player prefs
            SetPlayerPrefsFadeSpeedSelectedIndex();
            // Play the fade speed animation
            PlayFadeSpeedSelectedAnimation();
        }
    }

    // Set player prefs fade speed selected index
    private void SetPlayerPrefsFadeSpeedSelectedIndex()
    {
        PlayerPrefs.SetInt("fadeSpeedSelectedIndex", fadeSpeedSelectedIndex);
        PlayerPrefs.Save();
    }

    // Load the fade speed selected index from the player prefs loading
    private void LoadPlayerPrefsFadeSpeedSelectedIndex()
    {
        if (PlayerPrefs.HasKey("fadeSpeedSelectedIndex"))
        {
            fadeSpeedSelectedIndex = PlayerPrefs.GetInt("fadeSpeedSelectedIndex");

            PlayFadeSpeedSelectedAnimation();
        }
    }

    // Check fade speed selected and play animation in song select scene
    public void PlayFadeSpeedSelectedAnimation()
    {
        if (fadeSpeedAnimator != null)
        {
            if (optionsPanel.gameObject.activeSelf == true)
            {
                switch (fadeSpeedSelectedIndex)
                {
                    case 0:
                        fadeSpeedAnimator.Play("FadeSpeedSlow");
                        break;
                    case 1:
                        fadeSpeedAnimator.Play("FadeSpeedNormal");
                        break;
                    case 2:
                        fadeSpeedAnimator.Play("FadeSpeedFast");
                        break;
                }
            }
        }
    }

    // Fade speed selected in the song select 
    public float GetFadeSpeedSelected()
    {
        switch (fadeSpeedSelectedIndex)
        {
            case 0:
                // Set fade speed selected string
                fadeSpeedSelected = "SLOW";
                // Return the slow fade speed
                return fadeSpeedSlow;
            case 1:
                // Set fade speed selected string
                fadeSpeedSelected = "NORMAL";
                // Return the normal fade speed
                return fadeSpeedNormal;
            case 2:
                // Set fade speed selected string
                fadeSpeedSelected = "FAST";
                // Return the fast fade speed
                return fadeSpeedFast;
            default:
                return 0;
        }
    }

    // Updates the title and description text onhover
    public void UpdateModText(string _mod)
    {
        switch (_mod)
        {
            case "TRIPLE TIME":
                modNameText.text = "TRIPLE TIME";
                modDescriptionText.text = "Makes the song play very fast";
                break;
            case "DOUBLE TIME":
                modNameText.text = "DOUBLE TIME";
                modDescriptionText.text = "Makes the song play faster";
                break;
            case "HALF TIME":
                modNameText.text = "HALF TIME";
                modDescriptionText.text = "Makes the song play slower";
                break;
            case "NO FAIL":
                modNameText.text = "NO FAIL";
                modDescriptionText.text = "Prevents you from failing";
                break;
            case "INSTANT DEATH":
                modNameText.text = "INSTANT DEATH";
                modDescriptionText.text = "Fail if judgement below PERFECT";
                break;
            case "RESET":
                modNameText.text = "";
                modDescriptionText.text = "";
                break;

        }
    }

    // Update the mod keys selected for Eri based on whats been selected
    private void UpdateSelectedModKeys()
    {
        // Deactivate all mod keys first
        tripleTimeSelectedKey.gameObject.SetActive(false);
        doubleTimeSelectedKey.gameObject.SetActive(false);
        halfTimeSelectedKey.gameObject.SetActive(false);
        noFailSelectedKey.gameObject.SetActive(false);
        instantDeathSelectedKey.gameObject.SetActive(false);
        characterImageKey.color = blackColor;


        // Activate the mod key based on the mod selected
        switch (modSelected)
        {
            case "TRIPLE TIME":
                tripleTimeSelectedKey.gameObject.SetActive(true);
                characterImageKey.color = greenColor;
                break;
            case "DOUBLE TIME":
                doubleTimeSelectedKey.gameObject.SetActive(true);
                characterImageKey.color = orangeColor;
                break;
            case "HALF TIME":
                halfTimeSelectedKey.gameObject.SetActive(true);
                characterImageKey.color = blueColor;
                break;
            case "NO FAIL":
                noFailSelectedKey.gameObject.SetActive(true);
                characterImageKey.color = purpleColor;
                break;
            case "INSTANT DEATH":
                instantDeathSelectedKey.gameObject.SetActive(true);
                characterImageKey.color = redColor;
                break;
        }
    }
}
