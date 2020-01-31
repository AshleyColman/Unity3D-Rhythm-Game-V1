using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModeMenu : MonoBehaviour
{
    public TextMeshProUGUI homeButtonText;
    public Image homeButtonIcon;
    public Color defaultColor, selectedColor;

    public Button titleModeButton, rhythmGameModeButton, downloadModeButton, rankingsModeButton, settingsModeButton, discordModeButton, exitModeButton,
        profileModeButton;

    private const int titleButtonIndex = 1, rhythmGameButtonIndex = 2, downloadButtonIndex = 3, rankingsButtonIndex = 4,
        settingsButtonindex = 5, discordButtonIndex = 6, exitButtonIndex = 7, profileButtonIndex = 8;

    private int selectedButtonIndex;

    private ColorBlock buttonColorBlock;

    private void Start()
    {
        selectedButtonIndex = 1; // Default to title mode

        // Select button at the start
        HighlightSelectedModeButton(1);
    }

    public void HighlightSelectedModeButton(int _selectedButtonIndex)
    {
        // Reset current selected colors first
        ResetSelectedButtonColors();

        switch (_selectedButtonIndex)
        {
            case titleButtonIndex:
                SetHighlightedButtonColorBlock(titleModeButton);
                break;
            case rhythmGameButtonIndex:
                SetHighlightedButtonColorBlock(rhythmGameModeButton);
                break;
            case downloadButtonIndex:
                SetHighlightedButtonColorBlock(downloadModeButton);
                break;
            case rankingsButtonIndex:
                SetHighlightedButtonColorBlock(rankingsModeButton);
                break;
            case settingsButtonindex:
                SetHighlightedButtonColorBlock(settingsModeButton);
                break;
            case discordButtonIndex:
                SetHighlightedButtonColorBlock(discordModeButton);
                break;
            case exitButtonIndex:
                SetHighlightedButtonColorBlock(exitModeButton);
                break;
            case profileButtonIndex:
                SetHighlightedButtonColorBlock(profileModeButton);
                break;
        }

        // Update selected button index
        selectedButtonIndex = _selectedButtonIndex;
    }

    public void ResetSelectedButtonColors()
    {
        switch (selectedButtonIndex)
        {
            case titleButtonIndex:
                ResetButtonColorBlock(titleModeButton);
                break;
            case rhythmGameButtonIndex:
                ResetButtonColorBlock(rhythmGameModeButton);
                break;
            case downloadButtonIndex:
                ResetButtonColorBlock(downloadModeButton);
                break;
            case rankingsButtonIndex:
                ResetButtonColorBlock(rankingsModeButton);
                break;
            case settingsButtonindex:
                ResetButtonColorBlock(settingsModeButton);
                break;
            case discordButtonIndex:
                ResetButtonColorBlock(discordModeButton);
                break;
            case exitButtonIndex:
                ResetButtonColorBlock(exitModeButton);
                break;
            case profileButtonIndex:
                ResetButtonColorBlock(profileModeButton);
                break;
        }
    }

    // Update the buttons color block so that it stays highlighted
    private void SetHighlightedButtonColorBlock(Button _button)
    {
        buttonColorBlock = _button.colors;
        buttonColorBlock.normalColor = buttonColorBlock.selectedColor;
        _button.colors = buttonColorBlock;
    }

    // Reset the button color block so it returns to unselected until clicked
    private void ResetButtonColorBlock(Button _button)
    {
        buttonColorBlock = _button.colors;
        buttonColorBlock.normalColor = defaultColor;
        _button.colors = buttonColorBlock;
    }
}
