using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsNewsScroll : MonoBehaviour
{

    // SONG SELECT MENU TIPS
    private string[] tips = new string[3];
    public TextMeshProUGUI tipsText;
    private int textResetTime;
    private float timer;
    private int randomNumber;

    // CHARACTER MENU TIPS
    private string[] characterMenuTips = new string[10];
    public TextMeshProUGUI characterMenuTipsText;
    private int characterTipIndex;

    // Scripts
    private ScriptManager scriptManager;
    
    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // SONG SELECT MENU
        timer = 0f;
        textResetTime = 5;

        tips[0] = "WELCOME TO THE GAME";
        tips[1] = "Game currently in development";
        tips[2] = "Report bugs to Ashley on discord";

        tipsText.text = tips[0];


        // CHARACTER MENU
        characterMenuTips[0] = "This menu allows you to equip character skills";
        characterMenuTips[1] = "Equiping skills allows you to change the score multiplier to increase or decrease your max potential score";
        characterMenuTips[2] = "Difficulty increase skills will increase your multiplier, whilst making gameplay more difficult";
        characterMenuTips[3] = "Difficulty decrease skills will decrease your multiplier, whilst making gameplay easier";
        characterMenuTips[4] = "Rank skills do not impact the multiplier, they allow you to set a goal rank to try and achieve";
        characterMenuTips[5] = "Fun skills do not impact the multiplier, they're just for fun";
        characterMenuTips[6] = "You can combine skills from different classes as long as they're not the same type";
        characterMenuTips[7] = "If you have an idea for a new skill message Ashley#3286 on discord";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= textResetTime)
        {
            if (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf == true)
            {
                // If character panel is enabled
                if (scriptManager.playerSkillsManager.characterPanel.gameObject.activeSelf == true)
                {
                    // Update character panel tips text
                    ChangeCharacterSkillTipsText();
                }
                else
                {
                    // Update song select tips text
                    ChangeText();
                }

                // Reset timer
                timer = 0f;
            }
        }
    }

    // Update character panel tips text
    private void ChangeCharacterSkillTipsText()
    {
        // Error check index
        if (characterTipIndex >= characterMenuTips.Length)
        {
            characterTipIndex = 0;
        }

        characterMenuTipsText.text = characterMenuTips[characterTipIndex];
        characterTipIndex++;
    }

    // Update song select tips text
    private void ChangeText()
    {
        randomNumber = Random.Range(0, tips.Length);

        tipsText.text = tips[randomNumber];
    }
}
