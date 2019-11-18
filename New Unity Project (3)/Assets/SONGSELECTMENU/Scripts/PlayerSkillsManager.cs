using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerSkillsManager : MonoBehaviour
{
    // Scripts
    private ScriptManager scriptManager;

    // Gameobjects
    public GameObject characterScorePlusList, characterScoreMinusList, characterRankList;
    public GameObject characterPanel;

    // UI
    public Button songSelectCharacterButton;

    // Animation
    public Animator characterEriAnimator;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Activate character panel
    public void ActivateCharacterPanel()
    {
        // Activate character panel
        characterPanel.gameObject.SetActive(true);
        // Activate blur in animation
        scriptManager.blurShaderManager.ActivateBlurInAnimation();
    }

    // Activate coroutine to deactivate character panel on click
    public void DeactivateCharacterPanelOnClick()
    {
        // Deactivate character panel
        StartCoroutine(DeactivateCharacterPanel());
    }

    // Deactivate character panel
    private IEnumerator DeactivateCharacterPanel()
    {
        // Activate blur out animation
        scriptManager.blurShaderManager.ActivateBlurOutAnimation();

        // Play character outro animation
        characterEriAnimator.Play("CharacterOutro_Animation", 0, 0f);

        // Wait for animation to finish
        yield return new WaitForSeconds(1f);

        // Activate character panel
        characterPanel.gameObject.SetActive(false);
    }



    // Deactivate all character skill lists
    public void DeactivateCharacterSkillLists()
    {
        characterScorePlusList.gameObject.SetActive(false);
        characterScoreMinusList.gameObject.SetActive(false);
        characterRankList.gameObject.SetActive(false);
    }

    public void ActivateCharacterSkillScorePlusList()
    {
        DeactivateCharacterSkillLists();

        characterScorePlusList.gameObject.SetActive(true);

        scriptManager.messagePanel.DisplayCharacterSkillScorePlusListOnMessage();
    }

    public void ActivateCharacterSkillScoreMinusList()
    {
        DeactivateCharacterSkillLists();

        characterScoreMinusList.gameObject.SetActive(true);

        scriptManager.messagePanel.DisplayCharacterSkillScoreMinusListOnMessage();
    }

    public void ActivateCharacterRankList()
    {
        DeactivateCharacterSkillLists();

        characterRankList.gameObject.SetActive(true);

        scriptManager.messagePanel.DisplayCharacterSkillRankListOnMessage();
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
        scriptManager.songProgressBar.songAudioSource.pitch = 1.00f;
    }

    // Enable double time in gameplay
    private void EnableDoubleTimeInGameplay()
    {
        // Set the gameplay speed to x1.5
        Time.timeScale = 1.5f;

        scriptManager.songProgressBar.songAudioSource.pitch = 1.5f;
    }

    // Enable half time in gameplay
    private void EnableHalfTimeInGameplay()
    {
        // Set the gameplay speed to x0.75
        Time.timeScale = 0.80f;

        scriptManager.songProgressBar.songAudioSource.pitch = 0.80f;
    }
}
