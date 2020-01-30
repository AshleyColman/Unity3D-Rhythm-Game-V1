using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIColorManager : MonoBehaviour
{
    // Color blocks
    private ColorBlock dropDownColorBlock, dropDownItemColorBlock, tickBoxButtonColorBlock, gradientButtonColorBlock, scrollbarColorBlock;
    public Color dropDownPressedColor, dropDownSelectedColor;
    public Color solidBlackColor, blackColor08, invisibleColor, difficultyColor, whiteColor, difficultyColor08;

    private void Start()
    {
        dropDownColorBlock.colorMultiplier = 1;
        dropDownItemColorBlock.colorMultiplier = 1;
        tickBoxButtonColorBlock.colorMultiplier = 1;
        gradientButtonColorBlock.colorMultiplier = 1;
        scrollbarColorBlock.colorMultiplier = 1;
    }

    // Update drop down colors
    public void UpdateDropDownColors(TMP_Dropdown _dropdown)
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
        UpdateTickBoxButtonColorBlock();
        // Update colorblock
        UpdateGradientButtonColorBlock();
        // Update colorblock
        UpdateScrollbarColorBlock();

        // UPdate drop down list color button
        _dropdown.colors = dropDownColorBlock;

        // Update item template for drop down list children
        _dropdown.template.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().colors = dropDownItemColorBlock;
    }

    // Update scroll bar color block
    public void UpdateScrollbarColorBlock()
    {
        scrollbarColorBlock.normalColor = whiteColor;
        scrollbarColorBlock.highlightedColor = difficultyColor;
        scrollbarColorBlock.pressedColor = difficultyColor;
        scrollbarColorBlock.selectedColor = difficultyColor;
    }

    // Update scroll bar colors
    public void UpdateScrollbarColors(Scrollbar _scrollbar)
    {
        _scrollbar.colors = scrollbarColorBlock;
    }

    // Update gradient button color block
    public void UpdateGradientButtonColorBlock()
    {
        gradientButtonColorBlock.normalColor = invisibleColor;
        gradientButtonColorBlock.highlightedColor = difficultyColor08;
        gradientButtonColorBlock.pressedColor = difficultyColor;
        gradientButtonColorBlock.selectedColor = difficultyColor08;
    }

    // Update gradient buttons colors
    public void UpdateGradientButtonColors(Button _button)
    {
        _button.colors = gradientButtonColorBlock;
    }

    // Update tick box colors
    public void UpdateTickBoxButtonColorBlock()
    {
        tickBoxButtonColorBlock.normalColor = blackColor08;
        tickBoxButtonColorBlock.highlightedColor = difficultyColor08;
        tickBoxButtonColorBlock.pressedColor = difficultyColor;
        tickBoxButtonColorBlock.selectedColor = difficultyColor08;
    }

    // Update tick box button
    public void UpdateTickBoxButtonColors(Button _button)
    {
        _button.colors = tickBoxButtonColorBlock;
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
}
