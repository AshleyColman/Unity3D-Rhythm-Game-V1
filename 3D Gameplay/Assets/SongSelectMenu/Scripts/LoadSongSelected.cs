using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSongSelected : MonoBehaviour {

    public LevelChanger levelChanger;
    public LeaderboardManager leaderboardManager;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {

        // Get the reference when in the gameplay scene
        levelChanger = FindObjectOfType<LevelChanger>();

        // Dont destroy this object when in the song select scene or gameplay scene so we can load the level selected
        if (levelChanger.currentLevelIndex == 3 || levelChanger.currentLevelIndex == 4 || levelChanger.currentLevelIndex == 5)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // If on the results screen and score has uploaded delete game object so it doesn't have another spawn when back to song select screen
        if (levelChanger.currentLevelIndex == 5)
        {
            // Get the reference
            leaderboardManager = FindObjectOfType<LeaderboardManager>();

            // If the scores have been uploaded delete this object
            if (leaderboardManager.notChecked == false)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
