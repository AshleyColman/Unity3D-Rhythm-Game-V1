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

    public void HighlightSelectedModeButton(int _selectedButtonIndex)
    {
        switch (_selectedButtonIndex)
        {
            case 1:
                homeButtonText.fontStyle = FontStyles.Bold;
                homeButtonText.color = selectedColor;
                homeButtonIcon.color = selectedColor;
                break;
        }
    }
}
