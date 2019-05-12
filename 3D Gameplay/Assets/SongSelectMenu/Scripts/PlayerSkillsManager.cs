using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillsManager : MonoBehaviour {

    // The song select text that updates when a mod has been selected
    public TextMeshProUGUI modSelectedText;


    // FADE SPEED SKILL
    public float fadeSpeedSlow = 2f, fadeSpeedNormal = 1f, fadeSpeedFast = 0.5f;
    public int fadeSpeedSelectedIndex = 1;
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

    public TextMeshProUGUI fadeSpeedText;






    // TRIPLE TIME
    public bool tripleTimeSelected;

    // DOUBLE TIME
    public bool doubleTimeSelected;

    // HALF TIME
    public bool halfTimeSelected;

    // JUDGEMENT PLUS
    public bool judgementPlusSelected;

    // NO FAIL
    public bool noFailSelected;

    // INSTANT DEATH
    public bool instantDeathSelected;


    // ERI MOD KEYS BUTTONS
    // SELECTED KEYS
    public Image tripleTimeSelectedKey;
    public Image doubleTimeSelectedKey;
    public Image halfTimeSelectedKey;
    public Image judgementPlusSelectedKey;
    public Image noFailSelectedKey;
    public Image instantDeathSelectedKey;
    // DISABLED KEYS
    public Image tripleTimeDisabledKey;
    public Image doubleTimeDisabledKey;
    public Image halfTimeDisabledKey;
    public Image judgementPlusDisabledKey;
    public Image noFailDisabledKey;
    public Image instantDeathDisabledKey;

    // Reference required to change the song pitch when in gameplay
    SongProgressBar songProgressBar;

    private void Start()
    {
        discoSkillSelected = false;
        fadeSkillSelected = false;
        doubleTimeSelected = false;
        tripleTimeSelected = false;
        halfTimeSelected = false;
        judgementPlusSelected = false;
        noFailSelected = false;
        instantDeathSelected = false;

        PlayFadeSpeedSelectedAnimation();
    }

    void Update()
    {

        // Find the current level
        levelChanger = FindObjectOfType<LevelChanger>();

        // Get the player skills controller
        GameObject[] playerSkillsManager = GameObject.FindGameObjectsWithTag("PlayerSkillsManager");

        // Only update the keys if in song select scene
        if (levelChanger.currentLevelIndex == levelChanger.songSelectSceneIndex)
        {
            // Update the mod keys selected for Eri
            UpdateSelectedModKeys();
        }

        // If the song select, gameplay or results scene do not destroy but destroy for all other scenes
        if (levelChanger.currentLevelIndex == levelChanger.songSelectSceneIndex || levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex || levelChanger.currentLevelIndex == levelChanger.resultsSceneIndex)
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
        if (levelChanger.currentLevelIndex == levelChanger.gameplaySceneIndex)
        {
            // Get the reference to the song progress bar
            if (songProgressBar == null)
            {
                songProgressBar = FindObjectOfType<SongProgressBar>();
            }
            
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


            if (tripleTimeSelected == true)
            {
                // Enable triple time if triple time is selected
                EnableTripleTimeInGameplay();
            }
            else if (doubleTimeSelected == true)
            {
                // Enable double time if double time is selected
                EnableDoubleTimeInGameplay();
            }
            else if (halfTimeSelected == true)
            {
                // Enable half time if half time is selected
                EnableHalfTimeInGameplay();
            }

        }

        // If not in the gameplay scene 
        if (levelChanger.currentLevelIndex != levelChanger.gameplaySceneIndex)
        {
            // Reset the game speed to 1
            ResetGameSpeedToNormal();

            if (songProgressBar != null)
            {
                ResetSongPitchToNormal();
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

    // Reset timescale back to normal
    private void ResetGameSpeedToNormal()
    {
        // Change the game speed
        Time.timeScale = 1.0f;
    }

    // Reset the song pitch to normal
    private void ResetSongPitchToNormal()
    {
        // Change the song pitch
        songProgressBar.songAudioSource.pitch = 1.00f;
    }

    // Enable triple time in gameplay
    private void EnableTripleTimeInGameplay()
    {
        // Set the gameplay speed to x2
        Time.timeScale = 2.0f;

        // Also update the pitch of the song when in gameplay
        songProgressBar.songAudioSource.pitch = 2.00f;
    }

    // Enable double time in gameplay
    private void EnableDoubleTimeInGameplay()
    {
        // Set the gameplay speed to x1.5
        Time.timeScale = 1.5f;

        songProgressBar.songAudioSource.pitch = 1.5f;
    }

    // Enable half time in gameplay
    private void EnableHalfTimeInGameplay()
    {
        // Set the gameplay speed to x0.75
        Time.timeScale = 0.75f;

        songProgressBar.songAudioSource.pitch = 0.75f;
    }




    // Reset equiped skills
    public void ResetEquipedSkills()
    {
        fadeSkillSelected = false;
        discoSkillSelected = false;
        tripleTimeSelected = false;
        doubleTimeSelected = false;
        halfTimeSelected = false;
        judgementPlusSelected = false;
        noFailSelected = false;
        instantDeathSelected = false;
        UpdateModSelectedText("");
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

    // Update mod selected text
    private void UpdateModSelectedText(string modSelectedPass)
    {
        modSelectedText.text = modSelectedPass;
    }

    // Equip half time skill
    public void EquipHalfTimeSkill()
    {
        tripleTimeSelected = false;
        doubleTimeSelected = false;
        judgementPlusSelected = false;
        noFailSelected = false;
        instantDeathSelected = false;

        halfTimeSelected = true;

        UpdateModSelectedText("Half Time");
    }

    // Equip double time skill
    public void EquipDoubleTimeSkill()
    {
        halfTimeSelected = false;
        tripleTimeSelected = false;
        judgementPlusSelected = false;
        noFailSelected = false;
        instantDeathSelected = false; 

        doubleTimeSelected = true;

        UpdateModSelectedText("Double Time");
    }

    // Equip triple time skill
    public void EquipTripleTimeSkill()
    {
        halfTimeSelected = false;
        doubleTimeSelected = false;
        noFailSelected = false;
        instantDeathSelected = false;
        judgementPlusSelected = false;

        tripleTimeSelected = true;

        UpdateModSelectedText("Triple Time");
    }

    // Equip judgement plus skill
    public void EquipJudgementPlusSkill()
    {
        halfTimeSelected = false;
        doubleTimeSelected = false;
        tripleTimeSelected = false;
        noFailSelected = false;
        instantDeathSelected = false;

        judgementPlusSelected = true;
        UpdateModSelectedText("Judgement Plus");
    }

    // Equip no fail skill
    public void EquipNoFailSkill()
    {
        halfTimeSelected = false;
        doubleTimeSelected = false;
        tripleTimeSelected = false;
        judgementPlusSelected = false;
        instantDeathSelected = false;

        noFailSelected = true;
        UpdateModSelectedText("No Fail");
    }

    // Equip instant death skill
    public void EquipInstantDeathSkill()
    {
        halfTimeSelected = false;
        doubleTimeSelected = false;
        tripleTimeSelected = false;
        judgementPlusSelected = false;
        noFailSelected = false;

        instantDeathSelected = true;
        UpdateModSelectedText("Instant Death");
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
            fadeSpeedText.text = "1";
        }
        else if (fadeSpeedSelectedIndex == 1)
        {
            fadeSpeedAnimator.Play("FadeSpeedNormal");
            fadeSpeedText.text = "2";
        }
        else if (fadeSpeedSelectedIndex == 2)
        {
            fadeSpeedAnimator.Play("FadeSpeedFast");
            fadeSpeedText.text = "3";
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

    // Update the mod keys selected for Eri based on whats been selected
    private void UpdateSelectedModKeys()
    {
        // CHECK TRIPLE TIME
        if (tripleTimeSelected == true)
        {
            tripleTimeSelectedKey.gameObject.SetActive(true);
            tripleTimeDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            tripleTimeSelectedKey.gameObject.SetActive(false);
            tripleTimeDisabledKey.gameObject.SetActive(true);
        }


        // CHECK DOUBLE TIME
        if (doubleTimeSelected == true)
        {
            doubleTimeSelectedKey.gameObject.SetActive(true);
            doubleTimeDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            doubleTimeSelectedKey.gameObject.SetActive(false);
            doubleTimeDisabledKey.gameObject.SetActive(true);
        }


        // CHECK HALF TIME
        if (halfTimeSelected == true)
        {
            halfTimeSelectedKey.gameObject.SetActive(true);
            halfTimeDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            halfTimeSelectedKey.gameObject.SetActive(false);
            halfTimeDisabledKey.gameObject.SetActive(true);
        }


        // CHECK JUDGEMENT+ 
        if (judgementPlusSelected == true)
        {
            judgementPlusSelectedKey.gameObject.SetActive(true);
            judgementPlusDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            judgementPlusSelectedKey.gameObject.SetActive(false);
            judgementPlusDisabledKey.gameObject.SetActive(true);
        }


        // CHECK NO FAIL
        if (noFailSelected == true)
        {
            noFailSelectedKey.gameObject.SetActive(true);
            noFailDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            noFailSelectedKey.gameObject.SetActive(false);
            noFailDisabledKey.gameObject.SetActive(true);
        }


        // CHECK INSTANT DEATH
        if (instantDeathSelected == true)
        {
            instantDeathSelectedKey.gameObject.SetActive(true);
            instantDeathDisabledKey.gameObject.SetActive(false);
        }
        else
        {
            instantDeathSelectedKey.gameObject.SetActive(false);
            instantDeathDisabledKey.gameObject.SetActive(true);
        }
    }
}
