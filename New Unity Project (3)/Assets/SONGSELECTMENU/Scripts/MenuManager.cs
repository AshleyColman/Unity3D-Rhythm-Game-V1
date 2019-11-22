using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator menuManagerAnimator;

    public GameObject startMenu, mainMenu, songSelectMenu, gameplay, editor, editorSongSelectMenu, results, overallRankingMenu;

    public GameObject spinningLights;

    private GameObject currentActiveMenu;

    private void Start()
    {
        // TESTING - REMOVE
        currentActiveMenu = songSelectMenu;
    }

    private void Update()
    {
        if (startMenu.gameObject.activeSelf == true || mainMenu.gameObject.activeSelf == true)
        {
            spinningLights.gameObject.SetActive(true);
        }
        else
        {
            if (spinningLights.gameObject.activeSelf == true)
            {
                spinningLights.gameObject.SetActive(false);
            }
        }
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
        Debug.Log(currentActiveMenu);
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
        }
    }
}
