using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicController : MonoBehaviour {

    private bool previouslyPaused = false;

    void Awake()
    {

    }

    void Update()
    {
        GameObject[] menuMusicController = GameObject.FindGameObjectsWithTag("MenuMusicController");
        AudioSource menuMusicAudioSource = GameObject.FindGameObjectWithTag("MenuMusicController").GetComponent<AudioSource>();
        
        
        if (menuMusicController.Length > 1)
        {
            Destroy(menuMusicController[1].gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    
        LevelChanger levelChanger = FindObjectOfType<LevelChanger>();

        Debug.Log("currentlevel index" + levelChanger.currentLevelIndex);
        Debug.Log("paused: " + previouslyPaused);
        // destroy to stop playing on certain scenes
        if (levelChanger.currentLevelIndex == 2 || levelChanger.currentLevelIndex == 3)
        {
            menuMusicAudioSource.Stop();
            previouslyPaused = true;
        }

        if (levelChanger.currentLevelIndex == 0 || levelChanger.currentLevelIndex == 1)
        {
            if (previouslyPaused == true)
            {
                menuMusicAudioSource.Play();
                previouslyPaused = false;
            }
            
        }

        Debug.Log("endpaused: " + previouslyPaused);
    }

}
