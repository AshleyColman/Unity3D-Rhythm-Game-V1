using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsManager : MonoBehaviour {

    // FADE SPEED SKILL
    public float fadeSpeedSlow = 2f, fadeSpeedNormal = 1f, fadeSpeedFast = 0.5f;
    public int fadeSpeedSelectedIndex = 0;
    public string fadeSpeedSelected;
    public Animator fadeSpeedAnimator;

    // FADE SKILL
    public bool fadeSkillSelected;
    private GameObject fadeSkillImage;

    // DISCO SKILL
    public bool discoSkillSelected;
    private GameObject discoSkillCamera; // The disco animated camera
    private GameObject mainCamera; // The main default camera if the disco skill isn't selected
    // Level changer
    public LevelChanger levelChanger;

    private void Start()
    {
        discoSkillSelected = false;
        fadeSkillSelected = false;
    }

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

        // Destroy this game object if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(this.gameObject);
        }

        // If in the gameplay scene
        if (levelChanger.currentLevelIndex == 4)
        {
            // FADE SKILL
            if (fadeSkillImage == null)
            {
                fadeSkillImage = GameObject.FindGameObjectWithTag("FadeSkillBackground");
                fadeSkillImage.gameObject.SetActive(false);
            }

            if (fadeSkillSelected == true && Input.GetKeyDown(KeyCode.Space))
            {
                fadeSkillImage.gameObject.SetActive(true);
            }


            // DISCO SKILL

            // Find the disco camera
            if (discoSkillCamera == null)
            {
                discoSkillCamera = GameObject.FindGameObjectWithTag("DiscoSkillCamera");
            }

            // Find the normal camera
            if (mainCamera == null)
            {
                // Disable the main camera
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            if (discoSkillSelected == true)
            {
                // Destroy the normal camera
                Destroy(mainCamera);
            }
            else
            {
                // Destroy the disco camera
                Destroy(discoSkillCamera);
            }
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

    // Reset equiped skills
    public void ResetEquipedSkills()
    {
        fadeSkillSelected = false;
        discoSkillSelected = false;
    }

    // Equip the disco skill
    public void EquipDiscoSkill()
    {
        discoSkillSelected = true;
    }

    // Equip the fade skill
    public void EquipFadeSkill()
    {
        fadeSkillSelected = true;
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
