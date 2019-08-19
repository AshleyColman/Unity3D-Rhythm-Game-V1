using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Menu : MonoBehaviour {

    // UI
    public TextMeshProUGUI modeDescriptionText; // Mode description text
    public TextMeshProUGUI beatmapCountText; // Beatmap count text
    public Button playModeButton; // Play mode button

    // Strings
    private string playTextValue; // Play message
    private string createTextValue; // Create message
    private string discordTextValue; // Discord message
    private string exitTextValue; // Exit message
    private string noBeatmapsValue; // No beatmaps message
    private string[] beatmapDirectories; // Beatmap directories 

    // Integers
    private ushort beatmapDirectoryCount; // Total number of beatmaps found in the C drive
    private int frameInterval; // Frame count

    private void Start()
    {
        // Initialize
        playTextValue = "Play on user created beatmaps";
        createTextValue = "Create your own beatmap for others to play";
        discordTextValue = "Join the discord server";
        exitTextValue = "Thank you for playing";
        noBeatmapsValue = "No beatmaps found";
        frameInterval = 60;
    }

    void Update()
    {
        // Run every x frames
        if (Time.frameCount % frameInterval == 0)
        {
            // Check the beatmaps in the beatmap folder
            CheckBeatmapCount();
        }
    }

    // Check the beatmaps in the beatmap folder
    private void CheckBeatmapCount()
    {
        // Check the beatmap directories in the C drive
        beatmapDirectories = Directory.GetDirectories(@"C:\Beatmaps");

        // Get the number of folders found within the beatmaps folder
        beatmapDirectoryCount = (ushort)beatmapDirectories.Length;

        // Check if there are 0 beatmaps in the folder
        if (beatmapDirectoryCount <= 0)
        {
            // Disable the PLAY button if no beatmaps have been found
            playModeButton.interactable = false;

            // Enable "You have no beatmaps" message
            beatmapCountText.text = noBeatmapsValue;
        }
        else
        {
            // Enable the PLAY button if beatmaps are found
            playModeButton.interactable = true;

            // Enable "Total beatmaps" message
            beatmapCountText.text = "You have " + beatmapDirectoryCount + " beatmaps";
        }
    }

    // Open discord the discord URL
    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/zDneB5c");
    }

    // Update the mode description text
    public void UpdateModeDescriptionText(string buttonSelectedPass)
    {
        // Update based on the button hovered over
        switch (buttonSelectedPass)
        {
            case "PLAY":
                modeDescriptionText.text = playTextValue;
                break;
            case "CREATE":
                modeDescriptionText.text = createTextValue;
                break;
            case "DISCORD":
                modeDescriptionText.text = discordTextValue;
                break;
            case "EXIT":
                modeDescriptionText.text = exitTextValue;
                break;
        }
    }

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }

}
