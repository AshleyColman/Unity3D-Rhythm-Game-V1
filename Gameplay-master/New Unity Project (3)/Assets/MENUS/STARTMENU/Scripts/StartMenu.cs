using UnityEngine;


public class StartMenu : MonoBehaviour
{
    // Animations
    public Animator startMenuAnimator, menuManagerAnimator;
    // Bools
    private bool startedGame;

    private void Start()
    {
        startedGame = false;
    }

    private void Update()
    {
        if (Input.anyKeyDown && startedGame == false)
        {
            startedGame = true;

            // Play first click animation
            // Show create or login panel
            startMenuAnimator.Play("EnterGame_Animation", 0, 0f);
        }
    }

    public void ShowCreateProfileMenu()
    {
        startMenuAnimator.Play("ShowCreateProfileMenu_Animation", 0, 0f);
    }

    public void HideCreateProfileMenu()
    {
        startMenuAnimator.Play("HideCreateProfileMenu_Animation", 0, 0f);
    }

    public void CreateToLogin()
    {
        startMenuAnimator.Play("CreateToLogin_Animation", 0, 0f);
    }

    public void ShowLoginProfileMenu()
    {
        startMenuAnimator.Play("ShowLoginProfileMenu_Animation", 0, 0f);
    }

    public void HideLoginProfileMenu()
    {
        startMenuAnimator.Play("HideLoginProfileMenu_Animation", 0, 0f);
    }
        

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
