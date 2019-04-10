using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLastSelectedSong : MonoBehaviour
{

    private LevelChanger levelChanger;
    private SongSelectManager songSelectManager;
    public int selectedDirectoryIndex; // The index of the last selected song from the song select menu
    public int timesEnteredGameplayScene;

    private void Start()
    {
        timesEnteredGameplayScene = 0;
        selectedDirectoryIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        // Get all the game objects called loadLastSelectedSong and put into an array
        GameObject[] loadLastSelectedSong = GameObject.FindGameObjectsWithTag("LoadLastSelectedSong");

        // If there is more than 1 loadLastSelectedSong delete
        if (loadLastSelectedSong.Length > 1)
        {
            Destroy(loadLastSelectedSong[1].gameObject);
        }



        // Get the reference to the level changer
        levelChanger = FindObjectOfType<LevelChanger>();
        // Get the reference to the song select manager
        songSelectManager = FindObjectOfType<SongSelectManager>();

        // Do not destroy this object as it manages the song chosen
        DontDestroyOnLoad(this.gameObject);

    }

    // Save the song selected from the song select screen
    public void SaveSelectedDirectoryIndex(int selectedDirectoryIndexPass)
    {
        selectedDirectoryIndex = selectedDirectoryIndexPass;
    }

    // Load the last selected song in the song select screen
    public int LoadSelectedDirectoryIndex()
    {
        return selectedDirectoryIndex;
    }

    // Increment the times the player has selected a song by going into gameplay
    public void IncrementTimesEnteredGameplay()
    {
        timesEnteredGameplayScene++;
    }
}