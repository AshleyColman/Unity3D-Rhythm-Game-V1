using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region Variables
    // Gameobject
    public GameObject startMenu, mainMenu, songSelectMenu, gameplay, editor, editorSongSelectMenu, results, overallRankingMenu, downloadMenu,
        currentActiveMenu;

    // Dropdown
    public TMP_Dropdown modeDropdown;
    #endregion

    #region Functions
    private void Start()
    {
        // Get current active menu
        GetCurrentActiveMenu();
    }

    // Activate new menu
    public void ActivateModeMenu()
    {
        // Deactivate current active menu
        currentActiveMenu.gameObject.SetActive(false);

        // Activate the menu based on dropdown value
        switch (modeDropdown.value)
        {
            case 0:
                mainMenu.gameObject.SetActive(true);
                currentActiveMenu = mainMenu;
                break;
            case 1:
                songSelectMenu.gameObject.SetActive(true);
                currentActiveMenu = songSelectMenu;
                break;
            case 2:
                overallRankingMenu.gameObject.SetActive(true);
                currentActiveMenu = overallRankingMenu;
                break;
            case 3:
                downloadMenu.gameObject.SetActive(true);
                currentActiveMenu = downloadMenu;
                break;
        }
    }

    // Get current active menu
    private void GetCurrentActiveMenu()
    {
        if (mainMenu.gameObject.activeSelf == true)
        {
            currentActiveMenu = mainMenu;
        }
        else if (songSelectMenu.gameObject.activeSelf == true)
        {
            currentActiveMenu = songSelectMenu;
        }
        else if (overallRankingMenu.gameObject.activeSelf == true)
        {
            currentActiveMenu = overallRankingMenu;
        }
        else if (downloadMenu.gameObject.activeSelf == true)
        {
            currentActiveMenu = downloadMenu;
        }
    }
    #endregion
}
