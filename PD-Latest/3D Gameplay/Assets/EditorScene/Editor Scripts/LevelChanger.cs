using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    // Audio
    public AudioClip backSound;
    public AudioSource menuSFXAudioSource;

    // Animation
    public Animator animator;

    // Integers
    private int mainMenuSceneIndex, editorSceneIndex, songSelectSceneIndex, gameplaySceneIndex, resultsSceneIndex, overallLeaderboardSceneIndex,
        currentLevelIndex, lastFrameLevelIndex, levelToLoad;

    private float fadeOutTimer;

    private bool canFadeOut;

    // Bool
    private bool hasBackLevel;

    // Keycodes
    private KeyCode goToPreviousSceneKey;


    // Properties

    public int MainMenuSceneIndex
    {
        get { return mainMenuSceneIndex; }
    }
    
    public int EditorSceneIndex
    {
        get { return editorSceneIndex; }
    }

    public int GameplaySceneIndex
    {
        get { return gameplaySceneIndex; }
    }

    public int ResultsSceneIndex
    {
        get { return resultsSceneIndex; }
    }

    public int OverallLeaderboardSceneIndex
    {
        get { return overallLeaderboardSceneIndex; }
    }

    public int CurrentLevelIndex
    {
        get { return currentLevelIndex; }
    }

    public int LastFrameLevelIndex
    {
        get { return lastFrameLevelIndex; }
    }

    public int SongSelectSceneIndex
    {
        get { return songSelectSceneIndex; }
    }




    void Start()
    {
        // Initialize
        mainMenuSceneIndex = 0;
        editorSceneIndex = 1;
        songSelectSceneIndex = 2;
        gameplaySceneIndex = 3;
        resultsSceneIndex = 4;
        overallLeaderboardSceneIndex = 5;
        levelToLoad = 0;
        hasBackLevel = false;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        goToPreviousSceneKey = KeyCode.Escape;

        PlaySceneLoadInAnimation();
    }

    private void PlaySceneLoadInAnimation()
    {
        switch(currentLevelIndex)
        {
            case 0:
                animator.Play("LevelChanger_White_FadeIn");
            break;
        }

    }


    // Update is called once per frame
    void Update()
    {
        // Get the current level index
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Check for input on returning to the previous scene
        CheckForInput();

        // Update the last frame level index as the current frame
        lastFrameLevelIndex = currentLevelIndex;


        // If we can fade out
        if (canFadeOut == true)
        {
            // Increment fade time
            fadeOutTimer += Time.deltaTime;


            // Check if time to load next level
            if (fadeOutTimer >= 1)
            {
                // Fade to next level
                OnFadeComplete();
            }
        }
    }

    // Check for input on returning to the previous scene
    private void CheckForInput()
    {
        // If the previous scene activation key has been pressed
        if (Input.GetKeyDown(goToPreviousSceneKey))
        {
            // Get the previous level index based on the current level index
            BackLevelToLoad();

            // If there is a scene to return to
            if (hasBackLevel == true)
            {
                // Play back sound effect
                menuSFXAudioSource.PlayOneShot(backSound);

                // Fade animation
                FadeToLevel(levelToLoad);
            }
        }
    }

    // Fade to the next scene
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;

        PlaySceneLoadOutAnimation();

        canFadeOut = true;
    }

    private void PlaySceneLoadOutAnimation()
    {
        switch (currentLevelIndex)
        {
            case 0:
                animator.Play("LevelChanger_MainMenu_FadeOut");
                break;
        }

    }

    // On fade transition complete
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Check if the previous scene can be loaded based on the current scene
    public void BackLevelToLoad()
    {
        if (currentLevelIndex == mainMenuSceneIndex)
        {
            hasBackLevel = false;
        }

        if (currentLevelIndex == editorSceneIndex)
        {
            levelToLoad = mainMenuSceneIndex;
            hasBackLevel = true;
        }
        if (currentLevelIndex == songSelectSceneIndex)
        {
            levelToLoad = mainMenuSceneIndex;
            hasBackLevel = true;
        }
        if (currentLevelIndex == gameplaySceneIndex)
        {
            levelToLoad = songSelectSceneIndex;
            hasBackLevel = true;
        }
        if (currentLevelIndex == resultsSceneIndex)
        {
            levelToLoad = songSelectSceneIndex;
            hasBackLevel = true;
        }
        if (currentLevelIndex == overallLeaderboardSceneIndex)
        {
            levelToLoad = songSelectSceneIndex;
            hasBackLevel = true;
        }
    }
}
