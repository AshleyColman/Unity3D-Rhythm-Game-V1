using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneEnterGame : MonoBehaviour {

    LevelChanger levelChanger;
    public AudioSource MenuSFXAudioSource;
    public AudioClip MenuSFXMenuSourceClip;
    bool hasPressedEnter;
    // Use this for initialization
    void Start () {
        hasPressedEnter = false;
        levelChanger = FindObjectOfType<LevelChanger>();
        MenuSFXAudioSource.clip = MenuSFXMenuSourceClip;
    }

    void Update()
    {
        // If enter is pressed load the next scene and play sound
        if (Input.GetKey("return") && hasPressedEnter == false)
        {
            MenuSFXAudioSource.PlayOneShot(MenuSFXMenuSourceClip);
            levelChanger.FadeToLevel(6);
            hasPressedEnter = true;
        }
    }
}
