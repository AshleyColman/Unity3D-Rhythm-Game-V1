using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialTimeManager : MonoBehaviour {

    public Animator backgroundImageAnimation; // Animate the background image
    public SongProgressBar songProgressBar; // For checking the song time 
    private float songTime = 0f;
    public Image backgroundImage;
    private float specialTimeStart = 20f;
    private float specialTimeEnd = 40f;
    public bool isSpecialTime = false;

    // Use this for initialization
    void Start () {
        backgroundImage.enabled = false;
        songProgressBar = FindObjectOfType<SongProgressBar>();
    }
	
	// Update is called once per frame
	void Update () {
        // Get the song time
        songTime = songProgressBar.songAudioSource.time;

        // If it is the special time 
        if (songTime >= specialTimeStart && songTime <= specialTimeEnd)
        {
            isSpecialTime = true;
            ActivateBorder();
        }
        else
        {
            isSpecialTime = false;
            DeActivateBorder();
        }
        
    }

    public void BackgroundAnimation()
    {
        backgroundImageAnimation.Play("BackgroundImage");
    }

    public void ActivateBorder()
    {
        backgroundImage.enabled = true;
    }

    public void DeActivateBorder()
    {
        backgroundImage.enabled = false;
    }
}
