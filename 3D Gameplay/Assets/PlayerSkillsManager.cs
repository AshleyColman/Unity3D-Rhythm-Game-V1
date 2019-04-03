using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsManager : MonoBehaviour {

    // Fade speed skill
    public float fadeSpeedSlow = 2f, fadeSpeedNormal = 1f, fadeSpeedFast = 0.5f;
    public int fadeSpeedSelectedIndex = 0;
    public string fadeSpeedSelected;

    // Level changer
    public LevelChanger levelChanger;

    // Fade speed animation
    public Animator fadeSpeedAnimator;

    void Update()
    {
        // Find the current level
        levelChanger = FindObjectOfType<LevelChanger>();

        // Get the player skills controller
        GameObject[] playerSkillsManager = GameObject.FindGameObjectsWithTag("PlayerSkillsManager");


        // If the song select, gameplay or results scene do not destroy but destroy for all other scenes
        if (levelChanger.currentLevelIndex == 3 || levelChanger.currentLevelIndex == 4 || levelChanger.currentLevelIndex == 5)
        {
            // Dont destroy the manager
            DontDestroyOnLoad(this.gameObject);
        }

        // However if escape is pressed delete the manager
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(this.gameObject);
        }


        // Check if there are more than 1 hit sound controllers
        if (playerSkillsManager.Length > 1)
        {
            // Destroy is there is more than 1
            Destroy(playerSkillsManager[1].gameObject);
        }
        else
        {
            // Do not destroy any
            DontDestroyOnLoad(this.gameObject);
        }
    }



    // Increase the fade speed selected as the button preview has been pressed +
    public void IncrementFadeSpeedSelected()
    {
        if (fadeSpeedSelectedIndex == 2)
        {
            // Do not increment
        }
        else
        {
            fadeSpeedSelectedIndex++;
            PlayFadeSpeedSelectedAnimation();
        }
    }


    // Decrease the fade speed selected as the button preview has been pressed -
    public void DecrementFadeSpeedSelected()
    {
        if (fadeSpeedSelectedIndex == 0)
        {
            // Do not decrement
        }
        else
        {
            fadeSpeedSelectedIndex--;
            PlayFadeSpeedSelectedAnimation();
        }
    }

    // Check fade speed selected and play animation in song select scene
    public void PlayFadeSpeedSelectedAnimation()
    {
        if (fadeSpeedSelectedIndex == 0)
        {
            fadeSpeedAnimator.Play("FadeSpeedSlow");
        }
        else if (fadeSpeedSelectedIndex == 1)
        {
            fadeSpeedAnimator.Play("FadeSpeedNormal");
        }
        else if (fadeSpeedSelectedIndex == 2)
        {
            fadeSpeedAnimator.Play("FadeSpeedFast");
        }
    }

    // Fade speed selected in the song select 
    public float GetFadeSpeedSelected()
    {
        if (fadeSpeedSelectedIndex == 0)
        {
            // Set fade speed selected string
            fadeSpeedSelected = "SLOW";
            // Return the slow fade speed
            return fadeSpeedSlow;
        }
        else if (fadeSpeedSelectedIndex == 1)
        {
            // Set fade speed selected string
            fadeSpeedSelected = "NORMAL";
            // Return the normal fade speed
            return fadeSpeedNormal;
        }
        else if (fadeSpeedSelectedIndex == 2)
        {
            // Set fade speed selected string
            fadeSpeedSelected = "FAST";
            // Return the fast fade speed
            return fadeSpeedFast;
        }
        else
        {
            // Error no fade speed set
            return 0;
        }
    }
}
