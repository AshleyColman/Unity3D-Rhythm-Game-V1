using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIColorManager : MonoBehaviour
{
    // Color blocks
    private ColorBlock dropDownColorBlock, dropDownItemColorBlock, buttonColorBlock, scrollbarColorBlock;
    public Color dropDownPressedColor, dropDownSelectedColor;
    public Color solidBlackColor, blackColor08, invisibleColor, difficultyColor, solidWhiteColor, difficultyColor08;
    public Color onlineColor08, onlineColor09, onlineColorSolid, offlineColor08, offlineColor09, offlineColorSolid, orangeColor08;
    public Color selectedColor, sPlusRankColor, sRankColor, aRankColor, bRankColor, cRankColor, dRankColor, eRankColor, fRankColor;
    public Color easyDifficultyColor, normalDifficultyColor, hardDifficultyColor, purpleColor;
    public Color HIT_OBJECT_MOUSE_COLOR_LEFT, HIT_OBJECT_MOUSE_COLOR_RIGHT, HIT_OBJECT_MOUSE_COLOR_UP,
        HIT_OBJECT_MOUSE_COLOR_DOWN, HIT_OBJECT_COLOR_KEY_D, HIT_OBJECT_COLOR_KEY_F, HIT_OBJECT_COLOR_KEY_J,
        HIT_OBJECT_COLOR_KEY_K;

    // Gradients
    public TMP_ColorGradient sPlusColorGradient, sColorGradient, aColorGradient, bColorGradient, cColorGradient, dColorGradient, eColorGradient,
        fColorGradient, maxPlusColorGradient, maxColorGradient, greatColorGradient, lateColorGradient;

    private void Start()
    {
        dropDownColorBlock.colorMultiplier = 1;
        dropDownItemColorBlock.colorMultiplier = 1;
        buttonColorBlock.colorMultiplier = 1;
        scrollbarColorBlock.colorMultiplier = 1;
    }

    // Set the grade color gradient based on the grade passed
    public TMP_ColorGradient SetGradeColorGradient(string _grade)
    {
        switch (_grade)
        {
            case "S+":
                return sPlusColorGradient;
            case "S":
                return sColorGradient;
            case "A":
                return aColorGradient;
            case "B":
                return bColorGradient;
            case "C":
                return cColorGradient;
            case "D":
                return dColorGradient;
            case "E":
                return eColorGradient;
            default:
                return fColorGradient;
        }
    }

    // Set the grade color based on the grade passed
    public Color SetGradeColor(string _grade)
    {
        switch (_grade)
        {
            case "S+":
                return sPlusRankColor;
            case "S":
                return sRankColor;
            case "A":
                return aRankColor;
            case "B":
                return bRankColor;
            case "C":
                return cRankColor;
            case "D":
                return dRankColor;
            case "E":
                return eRankColor;
            default:
                return fRankColor;
        }
    }

    // Update the colorblock for difficulty selected colors
    public void UpdateDifficultyColorBlocks()
    {
        // Update difficulty color 08
        var tempColor = difficultyColor;
        tempColor.a = 0.8f;
        difficultyColor08 = tempColor;

        // Update colorblock
        UpdateDropDownColorBlock();
        // Update colorblock
        UpdateDropDownItemColorBlock();
        // Update colorblock
        UpdateButtonColorBlock();
        // Update colorblock
        UpdateScrollbarColorBlock();
    }

    // Update drop down colors
    public void UpdateDropDownColors(TMP_Dropdown _dropdown)
    {
        // Update drop down list color button
        _dropdown.colors = dropDownColorBlock;

        // Update item template for drop down list children
        _dropdown.template.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().colors = dropDownItemColorBlock;
    }

    // Update scroll bar color block
    public void UpdateScrollbarColorBlock()
    {
        scrollbarColorBlock.normalColor = solidWhiteColor;
        scrollbarColorBlock.highlightedColor = difficultyColor;
        scrollbarColorBlock.pressedColor = difficultyColor;
        scrollbarColorBlock.selectedColor = difficultyColor;
    }

    // Update scroll bar colors
    public void UpdateScrollbarColors(Scrollbar _scrollbar)
    {
        _scrollbar.colors = scrollbarColorBlock;
    }

    // Update button color block
    public void UpdateButtonColorBlock()
    {
        buttonColorBlock.normalColor = blackColor08;
        buttonColorBlock.highlightedColor = difficultyColor08;
        buttonColorBlock.pressedColor = difficultyColor;
        buttonColorBlock.selectedColor = difficultyColor08;
    }

    // Update gradient buttons colors
    public void UpdateButtonColors(Button _button)
    {
        _button.colors = buttonColorBlock;
    }

    // Update the color block colors
    private void UpdateDropDownColorBlock()
    {
        dropDownColorBlock.normalColor = blackColor08;
        dropDownColorBlock.highlightedColor = difficultyColor08;
        dropDownColorBlock.pressedColor = difficultyColor;
        dropDownColorBlock.selectedColor = difficultyColor08;
    }

    // Update the color block colors
    private void UpdateDropDownItemColorBlock()
    {
        dropDownItemColorBlock.normalColor = blackColor08;
        dropDownItemColorBlock.highlightedColor = difficultyColor08;
        dropDownItemColorBlock.pressedColor = difficultyColor;
        dropDownItemColorBlock.selectedColor = difficultyColor08;
    }

    // Update dropdown colors for the dropdown passed (change other functions in this script to rely on this one)
    public void UpdateDropdownColors(TMP_Dropdown _dropdown, Color _normalColor, Color _highlightedColor, Color _pressedColor, Color _selectedColor)
    {
        ColorBlock colorBlock = new ColorBlock();
        colorBlock.colorMultiplier = 1;

        colorBlock.normalColor = _normalColor;
        colorBlock.highlightedColor = _highlightedColor;
        colorBlock.pressedColor = _pressedColor;
        colorBlock.selectedColor = _selectedColor;

        _dropdown.colors = colorBlock;

        // Update template
        colorBlock.normalColor = blackColor08;
        _dropdown.template.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().colors = colorBlock;
    }
}
