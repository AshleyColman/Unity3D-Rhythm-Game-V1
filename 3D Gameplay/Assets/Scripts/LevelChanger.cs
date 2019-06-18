using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChanger : MonoBehaviour {

    public Animator animator;
    private int levelToLoad;
    public int currentLevelIndex;
    private bool hasBackLevel;
    public AudioClip backSound;

    // Level indexes
    public int mainMenuSceneIndex = 0;
    public int editorSceneIndex = 1;
    public int songSelectSceneIndex = 2;
    public int gameplaySceneIndex = 3;
    public int resultsSceneIndex = 4;
    public int overallLeaderboardSceneIndex = 5;

    void Start()
    {
        hasBackLevel = false;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

	// Update is called once per frame
	void Update () {
        // Get the current level index
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Find the object for menu back sound effect
        AudioSource menuSFXAudioSource = GameObject.FindGameObjectWithTag("MenuSFXAudioSource").GetComponent<AudioSource>();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Get the previous level index based on the current level index
            BackLevelToLoad();

            if (hasBackLevel == true)
            {
                // Play back sound effect
                menuSFXAudioSource.PlayOneShot(backSound);

                // Fade animation
                FadeToLevel(levelToLoad);
            }
            
        }

	}

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

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
