using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Animator menuManagerAnimator;

    public GameObject startMenu, mainMenu, songSelectMenu, gameplay, editor, editorSongSelectMenu, results;

    public GameObject spinningLights;

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
}
