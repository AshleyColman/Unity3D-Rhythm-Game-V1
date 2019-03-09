using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialTimeManager : MonoBehaviour {

    public Animator backgroundImageAnimation; // Animate the background image
    public SongProgressBar songProgressBar; // For checking the song time 
    private float songTime = 0f;
    public Image backgroundImage;
    private float specialTimeStart;
    private float specialTimeEnd;
    public bool isSpecialTime = false;
    private bool specialTimesLoaded;
    // Use this for initialization
    void Start () {
        specialTimesLoaded = false;
        specialTimeStart = 0;
        specialTimeEnd = 0;
        backgroundImage.enabled = false;
        songProgressBar = FindObjectOfType<SongProgressBar>();
    }
	
	// Update is called once per frame
	void Update () {

        if (specialTimeStart == 0 && specialTimeEnd == 0)
        {
            // Get the special time start and end from the database load
            specialTimeStart = Database.database.LoadedSpecialTimeStart;
            specialTimeEnd = Database.database.LoadedSpecialTimeEnd;
        }
        else
        {
            // The special times have been loaded
            specialTimesLoaded = true;
        }

        

        // Get the song time
        songTime = songProgressBar.songAudioSource.time;

        Debug.Log("specialTimeloaded = " + specialTimesLoaded);
        // If it is the special time 
        if (specialTimesLoaded == true && songTime >= specialTimeStart && songTime <= specialTimeEnd)
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
