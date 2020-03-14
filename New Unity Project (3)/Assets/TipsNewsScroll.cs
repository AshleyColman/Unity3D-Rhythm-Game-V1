using UnityEngine;
using TMPro;

public class TipsNewsScroll : MonoBehaviour
{
    #region Variables
    // Text
    public TextMeshProUGUI tipsText, characterMenuTipsText;

    // Animation
    private Animator tipsTextAnimator, characterMenuTipsTextAnimator;

    // String
    private string[] tips = new string[5];
    private string[] characterMenuTips = new string[10];

    // Integer
    private int textResetTime, songSelectTipIndex, characterTipIndex;
    private float timer;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Initialize
        timer = 0f;
        textResetTime = 5;
        songSelectTipIndex = 0;
        tipsTextAnimator = tipsText.GetComponent<Animator>();
        characterMenuTipsTextAnimator = characterMenuTipsText.GetComponent<Animator>();

        // Song Select menu
        tips[0] = "Welcome to the game!";
        tips[1] = "Updates / News / Tips will be displayed here";
        tips[2] = "If you enounter a bug please report it in the discord";
        tips[3] = "Please leave suggestions in the discord channel";
        tips[4] = "You can create your own beatmap by using the editor";

        // Character skill menu
        characterMenuTips[0] = "This menu allows you to equip character skills";
        characterMenuTips[1] = "Equiping skills allows you to change the score multiplier to increase or decrease your max potential score";
        characterMenuTips[2] = "Difficulty increase skills will increase your multiplier, whilst making gameplay more difficult";
        characterMenuTips[3] = "Difficulty decrease skills will decrease your multiplier, whilst making gameplay easier";
        characterMenuTips[4] = "Rank skills do not impact the multiplier, they allow you to set a goal rank to try and achieve";
        characterMenuTips[5] = "Fun skills do not impact the multiplier, they're just for fun";
        characterMenuTips[6] = "You can combine skills from different classes as long as they're not the same type";
        characterMenuTips[7] = "If you have an idea for a new skill message Ashley#3286 on discord";


        // Set default text
        tipsText.text = tips[0];
        // Play animation
        tipsTextAnimator.Play("GameplayTipsScroll_Animation", 0, 0f);
    }

    private void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // If time has reached
        if (timer >= textResetTime)
        {
            // Song select menu
            switch (scriptManager.menuManager.songSelectMenu.gameObject.activeSelf)
            {
                case true:
                    // Update song select tips text
                    ChangeSongSelectTipText();
                    // Reset timer
                    timer = 0f;
                    break;
                case false:
                    break;
            }

            // Character skill menu
            switch (scriptManager.playerSkillsManager.characterPanel.gameObject.activeSelf)
            {
                case true:
                    // Update character panel tips text
                    ChangeCharacterSkillTipsText();
                    // Reset timer
                    timer = 0f;
                    break;
                case false:
                    break;
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

        characterMenuTipsTextAnimator.Play("GameplayTipsScroll_Animation", 0, 0f);

        characterMenuTipsText.text = characterMenuTips[characterTipIndex];
        characterTipIndex++;
    }

    // Update song select tips text
    private void ChangeSongSelectTipText()
    {
        // Error check index
        if (songSelectTipIndex >= tips.Length)
        {
            songSelectTipIndex = 0;
        }

        tipsTextAnimator.Play("GameplayTipsScroll_Animation", 0, 0f);

        tipsText.text = tips[songSelectTipIndex];
        songSelectTipIndex++;
    }
    #endregion
}
