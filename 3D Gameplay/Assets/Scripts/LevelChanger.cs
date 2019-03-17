using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChanger : MonoBehaviour {

    public Animator animator;
    private int levelToLoad;
    public int currentLevelIndex;
    private bool hasBackLevel;
    public AudioClip backSound;

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
        if (currentLevelIndex == 0)
        {
            hasBackLevel = false;
        }
        if (currentLevelIndex == 1)
        {
            levelToLoad = 0;
            hasBackLevel = true;
        }
        if (currentLevelIndex == 2)
        {
            levelToLoad = 1;
            hasBackLevel = true;
        }
        if (currentLevelIndex == 3)
        {
            levelToLoad = 1;
            hasBackLevel = true;
        }
        if (currentLevelIndex == 4)
        {
            levelToLoad = 3;
            hasBackLevel = true;
        }
        if (currentLevelIndex == 5)
        {
            levelToLoad = 3;
            hasBackLevel = true;
        }
    }
}
