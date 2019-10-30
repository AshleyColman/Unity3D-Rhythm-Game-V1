using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsNewsScroll : MonoBehaviour
{

    private string[] tips = new string[28];
    public TextMeshProUGUI tipsText;
    private int textResetTime;
    private float timer;
    private int randomNumber;

    public Animator tipTextAnimator;

    private void Start()
    {
        timer = 0f;
        textResetTime = 30;


        tipsText.text = "Game currently still in development so you may enounter bugs";


        tips[0] = "Game currently still in development so you may enounter bugs";
        tips[1] = "Got suggestions? Let me know on discord #Ashley3286";
        tips[2] = "Don't start with difficult songs, start easy and work your way up";
        tips[3] = "You can change your fade speed in the options menu";
        tips[4] = "A slower fade speed can be useful when starting out";
        tips[5] = "If you're struggling try a faster fade speed, it might just help";
        tips[6] = "Your fever time bar fills up every 25 notes hit regardless of combo";
        tips[7] = "Your fever time bar has 4 stages impacting how long fever time will last";
        tips[8] = "A fever time bar at the 1st stage will last 2 measures of the song";
        tips[9] = "A fever time bar at the 2nd stage will last 4 measures of the song";
        tips[10] = "A fever time bar at the 3rd stage will last 6 measures of the song";
        tips[11] = "A fever time bar at the 4th stage will last 8 measures of the song";
        tips[12] = "Notes hit during fever time recieve a x2 score bonus, activate where the most notes are";
        tips[13] = "Plan your fever time activations, they can make or break your score";
        tips[14] = "Having trouble passing songs? Equip Eri's 'NO FAIL' or 'HALF SPEED' skills";
        tips[15] = "Make gameplay harder by equiping Eri's speed increase skills";
        tips[16] = "You can change your profile image by visiting the options menu";
        tips[17] = "You can view other player profiles by clicking their profile image on the beatmap ranking leaderboard";
        tips[18] = "The more keys the harder, so if you're struggling stick to the lower key beatmaps";
        tips[19] = "You can change your hit sounds and volume in the options menu";
        tips[20] = "Want to create your own beatmap for others to play? Visit the editor from the main menu";
        tips[21] = "View your overall rankings by hovering over the 'Beatmap Ranking' text and clicking";
        tips[22] = "Meet other players and get more beatmaps by joining the discord channel";
        tips[23] = "Fever time duration is not the same for every song, a measure in a song goes off the BPM";
        tips[24] = "Fever time stages impact duration, 1st: 2 measures, 2nd: 4 measures, 3rd: 6 measures, 4th: 8 measures. A measure goes off song BPM";
        tips[25] = "Menu and character artwork by Zeri (ZeriStudio), thanks! :)";
        tips[26] = "Don't find existing beatmaps fun? Make your own";
        tips[27] = "Request songs to be added into the game through discord";
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= textResetTime)
        {
            ChangeText();

            timer = 0f;
        }
    }

    private void ChangeText()
    {
        randomNumber = Random.Range(0, tips.Length);

        tipsText.text = tips[randomNumber];

        tipTextAnimator.Play("TipTextAnimation", 0, 0f);
    }

}
