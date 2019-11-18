using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIColorManager : MonoBehaviour
{
    // Color blocks
    private ColorBlock dropDownColorBlock, dropDownItemColorBlock, buttonColorBlock;
    public Color dropDownNormalColor, dropDownHighlightedColor, dropDownPressedColor, dropDownSelectedColor;
    public Color solidBlackColor, invisibleColor;

    private void Start()
    {
        dropDownColorBlock.colorMultiplier = 1;
        dropDownItemColorBlock.colorMultiplier = 1;
        buttonColorBlock.colorMultiplier = 1;
    }

    public Color DropDownHighlightedColor
    {
        set { dropDownHighlightedColor = value; }
    }

    // Update drop down colors
    public void UpdateDropDownColors(TMP_Dropdown _dropdown)
    {
        // Update colorblock
        UpdateDropDownColorBlock();
        // Update colorblock
        UpdateDropDownItemColorBlock();
        // Update color block
        UpdateButtonColorBlock();

        // UPdate drop down list color button
        _dropdown.colors = dropDownColorBlock;

        // Update item template for drop down list children
        _dropdown.template.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().colors = dropDownItemColorBlock;
    }

    // Update the color block colors
    private void UpdateButtonColorBlock()
    {
        buttonColorBlock.normalColor = invisibleColor;
        buttonColorBlock.highlightedColor = dropDownHighlightedColor;
        buttonColorBlock.pressedColor = dropDownHighlightedColor;
        buttonColorBlock.selectedColor = solidBlackColor;
    }

    public void UpdateButtonColors(Button _button)
    {
        _button.colors = buttonColorBlock;
    }

    // Update the color block colors
    private void UpdateDropDownColorBlock()
    {
        dropDownColorBlock.normalColor = dropDownNormalColor;
        dropDownColorBlock.highlightedColor = dropDownHighlightedColor;
        dropDownColorBlock.pressedColor = dropDownHighlightedColor;
        dropDownColorBlock.selectedColor = solidBlackColor;
    }

    // Update the color block colors
    private void UpdateDropDownItemColorBlock()
    {
        dropDownItemColorBlock.normalColor = dropDownNormalColor;
        dropDownItemColorBlock.highlightedColor = dropDownHighlightedColor;
        dropDownItemColorBlock.pressedColor = solidBlackColor;
        dropDownItemColorBlock.selectedColor = solidBlackColor;
    }
}
