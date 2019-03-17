using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSongSelected : MonoBehaviour {

    public LevelChanger levelChanger;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {

        // Get the reference when in the gameplay scene
        levelChanger = FindObjectOfType<LevelChanger>();

        // Dont destroy this object when in the song select scene or gameplay scene so we can load the level selected
        if (levelChanger.currentLevelIndex == 3 || levelChanger.currentLevelIndex == 4)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
