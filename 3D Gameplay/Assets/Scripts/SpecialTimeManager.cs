using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public float specialTimeWarningActivateTime; // The time to activate the specialTimeWarningText
    public float specialTimeWarningDeactivateTime; // The time to deactivate the specialTimeWarningText
    public TextMeshProUGUI specialTimeWarningText; // The specialTimeWarningText that appears just before special time starts
    public Animator specialTimeWarningAnimator; // The animator for the specialTimeWarningText
    public bool startSongTimer; // Start the song timer

    // Use this for initialization
    void Start () {
        specialTimesLoaded = false;
        specialTimeStart = 0;
        specialTimeEnd = 0;
        borderDisableTime = 0;
        specialTimeWarningActivateTime = 0;
        backgroundImage.enabled = false;
        songProgressBar = FindObjectOfType<SongProgressBar>();
        specialTimeWarningText.gameObject.SetActive(false); // Deactivate the specialTimeWarningText at the start
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
            // Assign the specialWarningActivateTime to be 3 seconds before special time begins
            specialTimeWarningActivateTime = (specialTimeStart - 4);
            // Assign the specialWarningDeactivateTime to be at the time special time begins
            specialTimeWarningDeactivateTime = specialTimeStart;
        }
        else
        {
            // The special times have been loaded
            specialTimesLoaded = true;
        }

       
        // If the space key has been pressed we start the song and song timer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startSongTimer = true;
        }

        if (startSongTimer == true)
        {
            // Update the song timer with the current song time
            songTime += Time.deltaTime;
        }


        // If if it time to activate the specialTimeWarningText but less than the deactivate time 
        if (songTime >= specialTimeWarningActivateTime && songTime <= specialTimeWarningDeactivateTime)
        {
            // Enable the warning text
            specialTimeWarningText.gameObject.SetActive(true);
            // Play the animation for the warning text
            StartCoroutine(AnimateSpecialTimeWarningText());
        }
        else
        {
            // Disable the text
            specialTimeWarningText.gameObject.SetActive(false);
        }

        // If it is the special time 
        if (specialTimesLoaded == true && songTime >= specialTimeStart && songTime <= specialTimeEnd)
        {
            Debug.Log("is special time set: " + songTime);

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

    // Play the animation for the specialTimeWarningText
    private IEnumerator AnimateSpecialTimeWarningText()
    {
        // Play the animation
        specialTimeWarningAnimator.Play("SpecialTimeWarningAnimation");
        // Wait for the animation to end before disabling
        yield return new WaitForSeconds(4f);
        // Disable the text
        specialTimeWarningText.gameObject.SetActive(false);
    }
}
