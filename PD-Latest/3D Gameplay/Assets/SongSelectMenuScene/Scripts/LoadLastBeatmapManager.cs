using System.IO;
using UnityEngine;

public class LoadLastBeatmapManager : MonoBehaviour
{
    // Integers
    private int lastBeatmapDirectoryIndex; // Index of last beatmap loaded

    // Strings
    private string lastBeatmapDifficulty; // Last beatmap difficulty loaded
    private string[] beatmapDirectories; // Beatmap directories 
    private ushort beatmapDirectoryCount; // Number of beatmaps in the directory

    // Properties

    // Get the last beatmap directory index
    public int LastBeatmapDirectoryIndex
    {
        get { return lastBeatmapDirectoryIndex; }
    }

    // Get the last beatmap difficulty
    public string LastBeatmapDifficulty
    {
        get { return lastBeatmapDifficulty; }
    }


    private void Start()
    {
        // Load the last beatmap index
        LoadPlayerPrefsLastBeatmapIndex();

        // Do error check on it's value
        ErrorCheckLastBeatmapDirectory();

        // Load last beatmap difficulty
        LoadPlayerPrefsLastBeatmapDifficulty();
    }

    // Check if the last beatmap directory is valid
    private void ErrorCheckLastBeatmapDirectory()
    {
        // Check the beatmap directories in the C drive
        beatmapDirectories = Directory.GetDirectories(@"C:\Beatmaps");
        // Get the number of folders found within the beatmaps folder
        beatmapDirectoryCount = (ushort)beatmapDirectories.Length;

        // If the last loaded beatmap directory no longer exists - too high 
        if (lastBeatmapDirectoryIndex > beatmapDirectoryCount)
        {
            // Load the first beatmap in the beatmap directory if it exists
            if (beatmapDirectoryCount > 0)
            {
                lastBeatmapDirectoryIndex = 0;
            }
        }
        else
        {
            // Keep the last selected beatmap directory as normal 
        }
    }

    // Set the last selected beatmap index in the player prefs
    public void SetPlayerPrefsLastBeatmapIndex(int _lastBeatmapDirectoryIndex)
    {
        // Save the last beatmap directory on the system
        PlayerPrefs.SetInt("lastBeatmapDirectoryIndex", _lastBeatmapDirectoryIndex);
        PlayerPrefs.Save();
    }

    // Check if the last beatmap directory was saved, if saved load it
    private void LoadPlayerPrefsLastBeatmapIndex()
    {
        // If the variable can be found on the system
        if (PlayerPrefs.HasKey("lastBeatmapDirectoryIndex"))
        {
            // Load and store it 
            lastBeatmapDirectoryIndex = PlayerPrefs.GetInt("lastBeatmapDirectoryIndex");
        }
    }

    // Set the last selected beatmap difficulty
    public void SetPlayerPrefsLastBeatmapDifficulty(string _lastBeatmapDifficulty)
    {
        // Save the last beatmap difficulty on the system
        PlayerPrefs.SetString("lastBeatmapDifficulty", _lastBeatmapDifficulty);
        PlayerPrefs.Save();
    }

    // Check if the last beatmap directory was saved, if saved load it
    private void LoadPlayerPrefsLastBeatmapDifficulty()
    {
        // If the variable can be found on the system
        if (PlayerPrefs.HasKey("lastBeatmapDifficulty"))
        {
            // Load and store it 
            lastBeatmapDifficulty = PlayerPrefs.GetString("lastBeatmapDifficulty");
        }
    }
}