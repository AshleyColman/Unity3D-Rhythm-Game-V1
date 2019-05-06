using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class MenuMusicController : MonoBehaviour
{
    public AudioSource songAudioSource;

    private bool previouslyPaused = false;


    void Update()
    {
        
        GameObject[] metronomeEffectsController = GameObject.FindGameObjectsWithTag("MetronomeEffects");


        if (metronomeEffectsController.Length > 1)
        {
            Destroy(metronomeEffectsController[1].gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        

        LevelChanger levelChanger = FindObjectOfType<LevelChanger>();

        // destroy to stop playing on certain scenes
        if (levelChanger.currentLevelIndex > 1)
        {
            Destroy(gameObject);
        }

    }
}



