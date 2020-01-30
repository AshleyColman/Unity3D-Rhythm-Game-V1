using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public Animator messagePanelAnimator;
    public Image messagePanelImage;
    public TextMeshProUGUI messageText;

    public Color purpleColor, blueColor, pinkColor, yellowColor, blackColor, redColor;


    private string defaultSortingMessageValue, easyDifficultySortingMessageValue, advancedDifficultySortingMessageValue, extraDifficultySortingMessageValue,
        allDifficultySortingMessageValue, artistSortingMessageValue, creatorSortingMessageValue, songNameSortingMessageValue, levelSortingMessageValue,
        videoToggleOnMessageValue, videoToggleOffMessageValue, easyDifficultyToggleOnMessageValue, easyDifficultyToggleOffMessageValue,
        advancedDifficultyToggleOnMessageValue, advancedDifficultyToggleOffMessageValue, extraDifficultyToggleOnMessageValue, 
        extraDifficultyToggleOffMessageValue;

    private string characterDifficultyIncreaseListMessage, characterDifficultyDecreaseListMessage, characterRankListMessage;

    private string maxEquipedSkillsMessage;

    private void Start()
    {
        defaultSortingMessageValue = "DEFAULT A-Z";
        easyDifficultySortingMessageValue = "EASY DIFFICULTY";
        advancedDifficultySortingMessageValue = "NORMAL DIFFICULTY";
        extraDifficultySortingMessageValue = "HARD DIFFICULTY";
        allDifficultySortingMessageValue = "ALL DIFFICULTIES";
        artistSortingMessageValue = "ARTIST A-Z";
        creatorSortingMessageValue = "CREATOR A-Z";
        songNameSortingMessageValue = "SONG A-Z";
        levelSortingMessageValue = "LEVEL 1-10";
        videoToggleOnMessageValue = "VIDEO ON";
        videoToggleOffMessageValue = "VIDEO OFF";
        easyDifficultyToggleOffMessageValue = "EASY LEVELS OFF";
        easyDifficultyToggleOnMessageValue = "EASY LEVELS ON";
        advancedDifficultyToggleOffMessageValue = "NORMAL LEVELS OFF";
        advancedDifficultyToggleOnMessageValue = "NORMAL LEVELS ON";
        extraDifficultyToggleOffMessageValue = "HARD LEVELS OFF";
        extraDifficultyToggleOnMessageValue = "HARD LEVELS ON";
        characterDifficultyIncreaseListMessage = "DIFFICULTY INCREASE SKILLS";
        characterDifficultyDecreaseListMessage = "DIFFICULTY DECREASE SKILLS";
        characterRankListMessage = "RANK SKILLS";
        maxEquipedSkillsMessage = "CANNOT EXCEED 4 EQUIPED SKILLS";
    }

    public void DisplayMessage(string _message, string _color)
    {
        switch (_color)
        {
            case "RED":
                messagePanelImage.color = redColor;
                break;
            case "BLACK":
                messagePanelImage.color = blackColor;
                break;
            case "PURPLE":
                messagePanelImage.color = purpleColor;
                break;
            case "YELLOW":
                messagePanelImage.color = yellowColor;
                break;
            case "BLUE":
                messagePanelImage.color = blueColor;
                break;
        }

        messageText.text = _message;

        PlayMessagePanelAnimation();
    }

    public void DisplayUnquipedSkillMessage(string _message)
    {
        messagePanelImage.color = redColor;
        messageText.text = _message;
        PlayMessagePanelAnimation();
    }

    public void DisplayMaxEquipedSkillsMessage()
    {
        messagePanelImage.color = redColor;
        messageText.text = maxEquipedSkillsMessage;
        PlayMessagePanelAnimation();
    }

    public void DisplayCharacterDifficultyIncreaseListMessage()
    {
        messagePanelImage.color = purpleColor;
        messageText.text = characterDifficultyIncreaseListMessage;
        PlayMessagePanelAnimation();
    }

    public void DisplayCharacterDifficultyDecreaseListMessage()
    {
        messagePanelImage.color = purpleColor;
        messageText.text = characterDifficultyDecreaseListMessage;
        PlayMessagePanelAnimation();
    }

    public void DisplayCharacterRankListMessage()
    {
        messagePanelImage.color = purpleColor;
        messageText.text = characterRankListMessage;
        PlayMessagePanelAnimation();
    }

    public void DisplayDefaultSortingMessage()
    {
        messagePanelImage.color = purpleColor;
        messageText.text = defaultSortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayVideoToggleOffMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = videoToggleOffMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayVideoToggleOnMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = videoToggleOnMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayEasyDifficultyToggleOffMessageValue()
    {
        messagePanelImage.color = pinkColor;
        messageText.text = easyDifficultyToggleOffMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayEasyDifficultyToggleOnMessageValue()
    {
        messagePanelImage.color = pinkColor;
        messageText.text = easyDifficultyToggleOnMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayAdvancedDifficultyToggleOffMessageValue()
    {
        messagePanelImage.color = blueColor;
        messageText.text = advancedDifficultyToggleOffMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayAdvancedDifficultyToggleOnMessageValue()
    {
        messagePanelImage.color = blueColor;
        messageText.text = advancedDifficultyToggleOnMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayExtraDifficultyToggleOffMessageValue()
    {
        messagePanelImage.color = yellowColor;
        messageText.text = extraDifficultyToggleOffMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayExtraDifficultyToggleOnMessageValue()
    {
        messagePanelImage.color = yellowColor;
        messageText.text = extraDifficultyToggleOnMessageValue;
        PlayMessagePanelAnimation();
    }


    public void DisplayEasyDifficultySortingMessage()
    {
        messagePanelImage.color = pinkColor;
        messageText.text = easyDifficultySortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayAdvancedDifficultySortingMessage()
    {
        messagePanelImage.color = blueColor;
        messageText.text = advancedDifficultySortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayExtraDifficultySortingMessage()
    {
        messagePanelImage.color = yellowColor;
        messageText.text = extraDifficultySortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayAllDifficultySortingMessage()
    {
        Debug.Log("1");
        messagePanelImage.color = redColor;
        messageText.text = allDifficultySortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplaySongNameSortingMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = songNameSortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayLevelSortingMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = levelSortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void DisplayCreatorSortingMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = creatorSortingMessageValue;
        PlayMessagePanelAnimation();
    }



    public void DisplayArtistSortingMessage()
    {
        messagePanelImage.color = blackColor;
        messageText.text = artistSortingMessageValue;
        PlayMessagePanelAnimation();
    }

    public void PlayMessagePanelAnimation()
    {
        messagePanelAnimator.Play("MessagePanel_Animation", 0, 0f);
    }

}
