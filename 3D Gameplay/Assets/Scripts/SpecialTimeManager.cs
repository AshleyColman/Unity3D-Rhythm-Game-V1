using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialTimeManager : MonoBehaviour {

    public Animator backgroundImageAnimation; // Animate the background image
    public SongProgressBar songProgressBar; // For checking the song time 
    private float songTime = 0f;
    public Image backgroundImage;
    public float specialTimeStart;
    public float specialTimeEnd;
    public bool isSpecialTime = false;
    private bool specialTimesLoaded;
    public float borderDisableTime;

    // Use this for initialization
    void Start () {
        specialTimesLoaded = false;
        specialTimeStart = 0;
        specialTimeEnd = 0;
        borderDisableTime = 0;
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

            // Make the border disable 1.2 seconds after special time has ended
            borderDisableTime = (specialTimeEnd + 1.5f);
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
        }
        // Activate the border if within special time and below the additional 1 second borderDisableTime
        if (specialTimesLoaded == true && songTime >= specialTimeStart && songTime <= borderDisableTime)
        {
            ActivateBorder();
        }
        // Deactivate the border and special time to false if its below the special time or greater than the borderDisableTime
        if (songTime < specialTimeStart || songTime > borderDisableTime)
        {
            isSpecialTime = false;
            DeActivateBorder();
        }
        // If the time is greater than the specialTimeEnd set special time to false
        if (songTime > specialTimeEnd)
        {
            isSpecialTime = false;
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
