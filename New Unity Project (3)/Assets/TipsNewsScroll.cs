using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsNewsScroll : MonoBehaviour
{

    private string[] tips = new string[3];
    public TextMeshProUGUI tipsText;
    private int textResetTime;
    private float timer;
    private int randomNumber;

    public Animator tipTextAnimator;

    private void Start()
    {
        timer = 0f;
        textResetTime = 5;


        tips[0] = "WELCOME TO THE GAME";
        tips[1] = "Game currently in development";
        tips[2] = "Report bugs to Ashley on discord";

        tipsText.text = tips[0];
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
    }

}
