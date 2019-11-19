using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerSkillsManager : MonoBehaviour
{
    // Scripts
    private ScriptManager scriptManager;

    // Gameobjects
    public GameObject difficultyIncreaseList, difficultyDecreaseList, rankList, equipedSkillList;
    public GameObject characterPanel;

    // Bools
    private bool speedSkillEquiped, rankSkillEquiped;

    // Ints
    private int previousDropdownValue;
    private int equipedSkills;
    private const int maxEquipedSkills = 4;

    // Floats
    private const float speed200Multiplier = 1.05f, speed175Multiplier = 1.05f, speed150Multiplier = 1.05f, speed125Multiplier = 1.05f,
        hiddenMultiplier = 1.05f, flashlightMultiplier = 1.05f, minesMultiplier = 1.05f, colorShuffleMultiplier = 1.05f;
    private float speed75Multiplier = 1.05f, speed50Multiplier = 1.05f, noFailMultiplier = 1.05f, rankSkillMultiplier = 0f;
    private float multiplierTotal;

    // strings
    private const string speed200Skill = "200X SPEED", speed175Skill = "175X SPEED", speed150Skill = "150X SPEED", speed125Skill = "125X SPEED",
        hiddenSkill = "HIDDEN", flashlightSkill = "FLASHLIGHT", minesSkill = "MINES", colorShuffleSkill = "COLOR SHUFFLE", speed75Skill = "75X SPEED",
        speed50Skill = "50X SPEED", noFailSkill = "NO FAIL", percent100Skill = "100%", sRankSkill = "S RANK", aRankSkill = "A RANK",
        bRankSkill = "B RANK", cRankSkill = "C RANK";

    private string unquipedSkillMessage;

    // UI
    public Button[] equipedSkillButtonArray = new Button[4];
    public TextMeshProUGUI[] equipedSkillTextArray = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] equipedSkillMultiplierTextArray = new TextMeshProUGUI[4];
    public TextMeshProUGUI multiplierText;
    public Button songSelectCharacterButton;
    public TMP_Dropdown skillSortingDropdown;


    // Animation
    public Animator characterEriAnimator;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        equipedSkills = 0;
        multiplierTotal = 1f;
        speedSkillEquiped = false;
        rankSkillEquiped = false;
    }

    // Activate character panel
    public void ActivateCharacterPanel()
    {
        // Activate character panel
        characterPanel.gameObject.SetActive(true);
        // Activate blur in animation
        scriptManager.blurShaderManager.ActivateBlurInAnimation();
    }

    // Activate coroutine to deactivate character panel on click
    public void DeactivateCharacterPanelOnClick()
    {
        // Deactivate character panel
        StartCoroutine(DeactivateCharacterPanel());
    }

    // Deactivate character panel
    private IEnumerator DeactivateCharacterPanel()
    {
        // Activate blur out animation
        scriptManager.blurShaderManager.ActivateBlurOutAnimation();

        // Play character outro animation
        characterEriAnimator.Play("CharacterOutro_Animation", 0, 0f);

        // Wait for animation to finish
        yield return new WaitForSeconds(1f);

        // Activate character panel
        characterPanel.gameObject.SetActive(false);
    }

    // Deactivate current active list
    private void DeactivateCurrentActiveSkillList()
    {
        switch (previousDropdownValue)
        {
            case 0:
                difficultyIncreaseList.gameObject.SetActive(false);
                break;
            case 1:
                difficultyDecreaseList.gameObject.SetActive(false);
                break;
            case 2:
                rankList.gameObject.SetActive(false);
                break;
        }
    }

    // Activate the new skill list 
    public void ActivateSkillList()
    {
        // Deactivate current active list
        DeactivateCurrentActiveSkillList();

        // Get current drop down value and enable list
        switch (skillSortingDropdown.value)
        {
            case 0:
                difficultyIncreaseList.gameObject.SetActive(true);
                scriptManager.messagePanel.DisplayCharacterDifficultyIncreaseListMessage();
                break;
            case 1:
                difficultyDecreaseList.gameObject.SetActive(true);
                scriptManager.messagePanel.DisplayCharacterDifficultyDecreaseListMessage();
                break;
            case 2:
                rankList.gameObject.SetActive(true);
                scriptManager.messagePanel.DisplayCharacterRankListMessage();
                break;
        }

        // Update previous dropdown value
        previousDropdownValue = skillSortingDropdown.value;
    }

    // Equip new skill
    public void EquipSkill(string _skill)
    {
        switch (_skill)
        {
            case speed200Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed200Skill, speed200Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case speed175Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed175Skill, speed175Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case speed150Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed150Skill, speed150Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case speed125Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed125Skill, speed125Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case hiddenSkill:
                DisplayEquipedSkillButton(hiddenSkill, hiddenMultiplier);
                break;
            case flashlightSkill:
                DisplayEquipedSkillButton(flashlightSkill, flashlightMultiplier);
                break;
            case minesSkill:
                DisplayEquipedSkillButton(minesSkill, minesMultiplier);
                break;
            case colorShuffleSkill:
                DisplayEquipedSkillButton(colorShuffleSkill, colorShuffleMultiplier);
                break;
            case speed75Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed75Skill, speed75Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case speed50Skill:
                if (speedSkillEquiped == false)
                {
                    DisplayEquipedSkillButton(speed50Skill, speed50Multiplier);
                    speedSkillEquiped = true;
                }
                else
                {
                    scriptManager.messagePanel.DisplayMessage("ANOTHER SPEED SKILL HAS ALREADY BEEN EQUIPED", "RED");
                }
                break;
            case noFailSkill:
                DisplayEquipedSkillButton(noFailSkill, noFailMultiplier);
                break;
        }
    }

    // Equip a rank skill
    public void EquipRankSkill(string _skill)
    {
        if (rankSkillEquiped == false)
        {
            switch (_skill)
            {
                case percent100Skill:
                    DisplayEquipedRankSkillButton(percent100Skill, rankSkillMultiplier);
                    break;
                case sRankSkill:
                    DisplayEquipedRankSkillButton(sRankSkill, rankSkillMultiplier);
                    break;
                case aRankSkill:
                    DisplayEquipedRankSkillButton(aRankSkill, rankSkillMultiplier);
                    break;
                case bRankSkill:
                    DisplayEquipedRankSkillButton(bRankSkill, rankSkillMultiplier);
                    break;
                case cRankSkill:
                    DisplayEquipedRankSkillButton(cRankSkill, rankSkillMultiplier);
                    break;
            }

            rankSkillEquiped = true;
        }
        else
        {
            scriptManager.messagePanel.DisplayMessage("ANOTHER RANK SKILL HAS ALREADY BEEN EQUIPED", "RED");
        }
    }

    // Display a rank skill
    private void DisplayEquipedRankSkillButton(string _skill, float _multiplier)
    {
        // If 4 skills have not been assigned
        if (equipedSkills != maxEquipedSkills)
        {
            // Update text for next equiped skill button
            equipedSkillTextArray[equipedSkills].text = _skill;
            equipedSkillMultiplierTextArray[equipedSkills].text = "NONE";
            equipedSkillButtonArray[equipedSkills].gameObject.SetActive(true);

            // Increment equi
            equipedSkills++;
        }
        else
        {
            // Display warning message
            scriptManager.messagePanel.DisplayMaxEquipedSkillsMessage();
        }
    }

    // Display new equiped skill button in the equiped skills list
    private void DisplayEquipedSkillButton(string _skill, float _multiplier)
    {
        // If 4 skills have not been assigned
        if (equipedSkills != maxEquipedSkills)
        {
            // Update text for next equiped skill button
            equipedSkillTextArray[equipedSkills].text = _skill;
            equipedSkillMultiplierTextArray[equipedSkills].text = _multiplier + "x";
            equipedSkillButtonArray[equipedSkills].gameObject.SetActive(true);

            // Calculate total multiplier
            AddToTotalMultiplier(_multiplier);

            // Update total multiplier text
            UpdateTotalMultiplierText();

            // Increment equi
            equipedSkills++;
        }
        else
        {
            // Display warning message
            scriptManager.messagePanel.DisplayMaxEquipedSkillsMessage();
        }
    }

    // Unequip a skill in the equiped skills list
    public void UnequipSkillButton(int _buttonIndex)
    {
        // Get the skill button selected, get the skill value from its text value
        switch (equipedSkillTextArray[_buttonIndex].text)
        {
            case speed200Skill:
                DeactivateEquipedSkillButton(speed200Skill, speed200Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case speed175Skill:
                DeactivateEquipedSkillButton(speed175Skill, speed175Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case speed150Skill:
                DeactivateEquipedSkillButton(speed150Skill, speed150Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case speed125Skill:
                DeactivateEquipedSkillButton(speed125Skill, speed125Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case hiddenSkill:
                DeactivateEquipedSkillButton(hiddenSkill, hiddenMultiplier, _buttonIndex);
                break;
            case flashlightSkill:
                DeactivateEquipedSkillButton(flashlightSkill, flashlightMultiplier, _buttonIndex);
                break;
            case minesSkill:
                DeactivateEquipedSkillButton(minesSkill, minesMultiplier, _buttonIndex);
                break;
            case colorShuffleSkill:
                DeactivateEquipedSkillButton(colorShuffleSkill, colorShuffleMultiplier, _buttonIndex);
                break;
            case speed75Skill:
                DeactivateEquipedSkillButton(speed75Skill, speed75Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case speed50Skill:
                DeactivateEquipedSkillButton(speed50Skill, speed50Multiplier, _buttonIndex);
                speedSkillEquiped = false;
                break;
            case noFailSkill:
                DeactivateEquipedSkillButton(noFailSkill, noFailMultiplier, _buttonIndex);
                break;
            case percent100Skill:
                DeactivateEquipedSkillButton(percent100Skill, rankSkillMultiplier, _buttonIndex);
                rankSkillEquiped = false;
                break;
            case sRankSkill:
                DeactivateEquipedSkillButton(sRankSkill, rankSkillMultiplier, _buttonIndex);
                rankSkillEquiped = false;
                break;
            case aRankSkill:
                DeactivateEquipedSkillButton(aRankSkill, rankSkillMultiplier, _buttonIndex);
                rankSkillEquiped = false;
                break;
            case bRankSkill:
                DeactivateEquipedSkillButton(bRankSkill, rankSkillMultiplier, _buttonIndex);
                rankSkillEquiped = false;
                break;
            case cRankSkill:
                DeactivateEquipedSkillButton(cRankSkill, rankSkillMultiplier, _buttonIndex);
                rankSkillEquiped = false;
                break;
        }
    }

    // Deactivate the equiped skill button
    private void DeactivateEquipedSkillButton(string _skill, float _multiplier, int _buttonIndex)
    {
        // Deactivate skill button
        equipedSkillButtonArray[_buttonIndex].gameObject.SetActive(false);

        unquipedSkillMessage = "REMOVED: " + equipedSkillTextArray[_buttonIndex].text;

        // Display message
        scriptManager.messagePanel.DisplayUnquipedSkillMessage(unquipedSkillMessage);

        // Decrement equipped skills count
        equipedSkills--;

        // Calculate total multiplier
        SubtractFromTotalMultiplier(_multiplier);

        // Update total multiplier text
        UpdateTotalMultiplierText();
    }

    // Reset all equiped skills
    public void ResetAllEquipedSkills()
    {
        // Deactivate all equiped skill buttons
        for (int i = 0; i < equipedSkillButtonArray.Length; i++)
        {
            equipedSkillButtonArray[i].gameObject.SetActive(false);
        }

        // Display message
        scriptManager.messagePanel.DisplayUnquipedSkillMessage("ALL SKILLS REMOVED");

        // Reset equiped skills value
        equipedSkills = 0;

        // Update multiplier value
        multiplierTotal = 1f;

        // Update multiplier text
        UpdateTotalMultiplierText();

        // Reset bools
        rankSkillEquiped = false;
        speedSkillEquiped = false;
    }

    //  Subtract from the total multiplier
    private void SubtractFromTotalMultiplier(float _skillMultiplier)
    {
        multiplierTotal -= _skillMultiplier;

        // Value fix
        if (equipedSkills == 0)
        {
            multiplierTotal = 1f;
        }
    }

    // Add to the total multiplier
    private void AddToTotalMultiplier(float _skillMultiplier)
    {
        multiplierTotal +=  _skillMultiplier;
    }
       
    // Update the total multiplier text
    private void UpdateTotalMultiplierText()
    {
        multiplierText.text = multiplierTotal.ToString() + "x";
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
        scriptManager.songProgressBar.songAudioSource.pitch = 1.00f;
    }

    // Enable double time in gameplay
    private void EnableDoubleTimeInGameplay()
    {
        // Set the gameplay speed to x1.5
        Time.timeScale = 1.5f;

        scriptManager.songProgressBar.songAudioSource.pitch = 1.5f;
    }

    // Enable half time in gameplay
    private void EnableHalfTimeInGameplay()
    {
        // Set the gameplay speed to x0.75
        Time.timeScale = 0.80f;

        scriptManager.songProgressBar.songAudioSource.pitch = 0.80f;
    }
}
