using UnityEngine;

public class LoadSongSelected : MonoBehaviour {

    // Scripts
    private LevelChanger levelChanger;
    private LeaderboardManager leaderboardManager;
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(this.gameObject);
        }
                    
        // Get the reference when in the gameplay scene
        levelChanger = FindObjectOfType<LevelChanger>();

        // Dont destroy this object when in the song select scene or gameplay scene so we can load the level selected
        if (levelChanger.CurrentLevelIndex == levelChanger.SongSelectSceneIndex || levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex 
            || levelChanger.CurrentLevelIndex == levelChanger.ResultsSceneIndex)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // If on the results screen and score has uploaded delete game object so it doesn't have another spawn when back to song select screen
        if (levelChanger.CurrentLevelIndex == levelChanger.ResultsSceneIndex)
        {
            // Get the reference
            leaderboardManager = FindObjectOfType<LeaderboardManager>();

            // If the scores have been uploaded delete this object
            if (leaderboardManager.NotChecked == false)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
