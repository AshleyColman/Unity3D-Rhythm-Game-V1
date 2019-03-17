using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelectMenuFlash : MonoBehaviour {

    public Animator songSelectFlashAnimator; // The flash animator
    public SongSelectManager songSelectManager; // Reference to the song select manager
    public string defaultBeatmapDifficulty; // Advanced is default
    public string extraBeatmapDifficulty; // Extra difficulty
    public string lastSelectedDifficulty; // The last selected difficulty on the current beatmap, so if extra was last selected allow the flash for advanced
    public bool hasPressedArrowKey; // Passed in the loading function to change the preview song each load but not for when hovering over the difficulties

    // Use this for initialization
    void Start () {

        hasPressedArrowKey = false;
        // Set the default beatmap difficulty to advanced
        defaultBeatmapDifficulty = "advanced";
        // Set the extra beatmap difficulty to extra
        extraBeatmapDifficulty = "extra";

        // Get the reference
        songSelectManager = FindObjectOfType<SongSelectManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Has pressed arrow key
            hasPressedArrowKey = true;
            // Flash the image
            FlashImage();
            // Load the next beatmap in the song select menu
            // Increase the current index by 1 so we go to the next song
            songSelectManager.selectedDirectoryIndex++;
            songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
            // Set the last selected difficulty to advanced
            lastSelectedDifficulty = defaultBeatmapDifficulty;
            // Set back to false
            hasPressedArrowKey = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Has pressed arrow key
            hasPressedArrowKey = true;
            // Flash the image
            FlashImage();
            // Load the next beatmap in the song select menu
            // Decrease the current index by 1 so we go to the next song
            songSelectManager.selectedDirectoryIndex--;
            songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
            // Set the last selected difficulty to advanced
            lastSelectedDifficulty = defaultBeatmapDifficulty;
            // Set back to false
            hasPressedArrowKey = false;
        }

    }

    // Animate the flash on screen
    public void FlashImage()
    {
        Debug.Log("Animated");
        songSelectFlashAnimator.Play("SongSelectMenuFlash");
    }
    
    // Select the Extra difficulty, update and flash
    public void LoadBeatmapExtraDifficulty()
    {
        // Flash image
        FlashImage();
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, extraBeatmapDifficulty, hasPressedArrowKey);
        // Set the last selected difficulty to extra
        lastSelectedDifficulty = extraBeatmapDifficulty;
    }

    // Select the Advanced difficulty, update and flash
    public void LoadBeatmapAdvancedDifficulty()
    {
        // Flash image
        FlashImage();
        // Load extra difficulty information and beatmap file from database
        songSelectManager.LoadBeatmapSongSelectInformation(songSelectManager.selectedDirectoryIndex, defaultBeatmapDifficulty, hasPressedArrowKey);
        // Set the last selected difficulty to Advanced
        lastSelectedDifficulty = defaultBeatmapDifficulty;
    }
}
