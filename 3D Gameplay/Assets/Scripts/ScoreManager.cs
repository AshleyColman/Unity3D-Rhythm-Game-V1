using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public int score;
    public Text scoreText;
    public int combo;
    public Text comboText;
    public string judgement;
    public Text judgementText;
    public Animator scoreAnimation; // Animate the score text
    public Animator comboAnimation; // Animate the combo text
    public Animator judgementAnimation; // Animate the judgement text

    // Use this for initialization
    void Start () {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        judgementText.text = judgement.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update the score text
    public void AddScore(int scorePass)
    {
        score += scorePass;
        scoreText.text = score.ToString();

        scoreAnimation.Play("GameplayTextAnimation");
    }

    // Update combo text
    public void AddCombo(int comboPass)
    {
        combo += comboPass;
        comboText.text = combo.ToString();

        comboAnimation.Play("GameplayTextAnimation");
    }

    // Update judgement text
    public void AddJudgement(string judgementPass)
    {
        judgementText.text = judgementPass.ToString();

        judgementAnimation.Play("GameplayTextAnimation");

        if (judgementPass == "EARLY")
        {
            judgementText.color = Color.red; 
        }
        else if (judgementPass == "GOOD")
        {
            judgementText.color = Color.blue;
        }
        else if (judgementPass == "PERFECT")
        {
            judgementText.color = Color.yellow;
        }
    }
}
