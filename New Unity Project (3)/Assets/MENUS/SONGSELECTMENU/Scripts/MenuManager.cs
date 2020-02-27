﻿using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator menuManagerAnimator;

    public GameObject startMenu, mainMenu, songSelectMenu, gameplay, editor, editorSongSelectMenu, results, overallRankingMenu, downloadMenu;

    public GameObject currentActiveMenu;

    private void Start()
    {
        // TESTING - REMOVE
        currentActiveMenu = songSelectMenu;
    }

    public void StartMenuToMainMenu()
    {
        menuManagerAnimator.Play("StartMenuToMainMenu_Animation", 0, 0f);
    }

    public void MainMenuToSongSelectMenu()
    {
        menuManagerAnimator.Play("MainMenuToSongSelectMenu_Animation", 0, 0f);
    }

    // Display new menu
    public void ActivateModeMenu(string _menu)
    {
        // Deactivate current active menu
        currentActiveMenu.gameObject.SetActive(false);

        // Activate the menu based on the menu string passed
        switch (_menu)
        {
            case "RHYTHM GAME":
                songSelectMenu.gameObject.SetActive(true);
                currentActiveMenu = songSelectMenu;
                break;
            case "OVERALL RANKING":
                overallRankingMenu.gameObject.SetActive(true);
                currentActiveMenu = overallRankingMenu;
                break;
            case "DOWNLOAD":
                downloadMenu.gameObject.SetActive(true);
                currentActiveMenu = downloadMenu;
                break;
        }
    }
}
