using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Networking;

public class SetupBeatmap : MonoBehaviour
{
    // Audio
    private AudioClip beatmapFileAudioClip;

    // Gameobject
    public GameObject setupBeatmapPanel, folderSetupPanel, fileSetupPanel, timingSetupPanel;

    // Image
    public Image audioFileLoadingIconImage, backgroundImageLoadingIconImage;

    // Text
    public TextMeshProUGUI defaultAudioFileText, defaultBackgroundImageText, existsAudioFileText, existsBackgroundImageText;

    // Dropdown
    public TMP_Dropdown difficultyDropdown;

    // Input field
    public TMP_InputField songNameInputField, artistNameInputField;

    // Button
    public Button createFolderButton, fileSetupContinueButton;

    // String
    private string beatmapDifficulty, beatmapCreatedDate, folderDirectory, beatmapFolderName, songName, artistName, creatorName, creatorMessage;
    private const string EASYDIFFICULTY = "EASY", ADVANCEDDIFFICULTY = "ADVANCED", EXTRADIFFICULTY = "EXTRA", AUDIONAME = "audio", AUDIOTYPE = ".ogg",
        IMAGENAME = "img", IMAGETYPE = ".png";

    // Bool
    private bool folderCreated, audioFileFound, backgroundImageFound, filesUpdated;

    // Float
    private float timer, songPreviewStartTime;
    private const float FILECHECKTIME = 2f;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public AudioClip BeatmapFileAudioClip
    {
        get { return beatmapFileAudioClip; }
    }

    public string FolderDirectory
    {
        get { return folderDirectory; }
    }

    public string BeatmapDifficulty
    {
        get { return beatmapDifficulty; }
    }

    public string SongName
    {
        get { return songName; }
    }

    public string ArtistName
    {
        get { return artistName; }
    }

    public string CreatorName
    {
        get { return creatorName; }
    }

    public float SongPreviewStartTime
    {
        get { return songPreviewStartTime; }
    }

    public string BeatmapCreatedDate
    {
        get { return beatmapCreatedDate; }
    }

    private void Start()
    {
        folderCreated = false;
        audioFileFound = false;
        backgroundImageFound = false;
        filesUpdated = false;
        songPreviewStartTime = 0;

        // Get the user logged in and set the creator to that user
        if (MySQLDBManager.loggedIn)
        {
            creatorName = MySQLDBManager.username;
        }
        else
        {
            creatorName = "GUEST";
        }

        scriptManager = FindObjectOfType<ScriptManager>();
    }

    private void Update()
    {
        // Check the setup files in the beatmap folder to see if they exist
        CheckSetupFiles();
    }

    // Check the setup files in the beatmap folder to see if they exist
    private void CheckSetupFiles()
    {
        if (fileSetupPanel.gameObject.activeSelf == true)
        {
            timer += Time.deltaTime;

            if (timer > FILECHECKTIME)
            {
                CheckIfAudioFileExists();
                CheckIfImageFileExists();

                if (filesUpdated == false)
                {
                    if (audioFileFound == true && backgroundImageFound == true)
                    {
                        // Load audio
                        GetBeatmapAudio();
                        // Load background image
                        scriptManager.backgroundManager.LoadImageOnly(folderDirectory);
                        // Enable continue button
                        fileSetupContinueButton.interactable = true;
                        // Files updated
                        filesUpdated = true;
                    }
                    else
                    {
                        // Disable continue button
                        fileSetupContinueButton.interactable = false;
                    }
                }

                timer = 0f;
            }
        }
    }

    public void GetBeatmapAudio()
    {
        if (File.Exists(folderDirectory + AUDIONAME + AUDIOTYPE))
        {
            StartCoroutine(GetAudioFile());
        }
        else
        {
            scriptManager.messagePanel.DisplayMessage("BEATMAP AUDIO FILE NOT FOUND", "RED");
        }
    }

    private IEnumerator GetAudioFile()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + folderDirectory + AUDIONAME + AUDIOTYPE, AudioType.OGGVORBIS))
        {
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                scriptManager.messagePanel.DisplayMessage("BEATMAP AUDIO FILE NOT FOUND", "RED");
            }
            else
            {
                beatmapFileAudioClip = DownloadHandlerAudioClip.GetContent(www);

                yield return new WaitForSeconds(0.05f);

                // Assign the clip to the rhythm visualizator
                scriptManager.rhythmVisualizatorPro.audioSource.Stop();
                scriptManager.rhythmVisualizatorPro.audioSource.clip = beatmapFileAudioClip;
                scriptManager.rhythmVisualizatorPro.audioSource.Play();

                // Update metronome timing
                scriptManager.metronomePro.Stop();
                scriptManager.metronomePro.CalculateIntervals();
                scriptManager.metronomePro.CalculateActualStep();

                // Sort the beatsnaps
                scriptManager.beatsnapManager.SortBeatsnaps();
            }
        }
    }

    // Open discord the discord URL
    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/zDneB5c");
    }

    // Save the song preview start time
    public void GetSongPreviewStartTime(float _time)
    {
        songPreviewStartTime = _time;

        // Update the song preview start time text
        // text = UtilityMethods.FromSecondsToMinutesAndSeconds(songPreviewStartTime);
    }

    // Continue from the file setup panel
    public void FileSetupPanelContinue()
    {
        fileSetupPanel.gameObject.SetActive(false);
        setupBeatmapPanel.gameObject.SetActive(false);
        timingSetupPanel.gameObject.SetActive(true);

        // Play metronome
        scriptManager.metronomePro.Play();

        // Move the timeline to the middle of the screen for timing setup
        scriptManager.timelineScript.SetTimingSetupTimlinePosition();

        // Calculate beatsnap slider values
        scriptManager.beatsnapManager.CalculateBeatsnapSliderListValues();
    }

    // Continue from the timing setup panel
    public void TimingSetupContinue()
    {
        timingSetupPanel.gameObject.SetActive(false);
        scriptManager.timelineScript.SetDefaultTimelinePosition();
        scriptManager.cursorHitObject.gameObject.SetActive(true);


        // Update gridsnap
        scriptManager.rotatorManager.CalculateRotations();
        scriptManager.rotatorManager.UpdateTimeToReachTarget();
        scriptManager.rotatorManager.ToggleLerpOn();
    }

    public void CheckSetupPanelInputFieldLength()
    {
        if (folderCreated == false)
        {
            if (songNameInputField.text.Length > 0 && artistNameInputField.text.Length > 0 && difficultyDropdown.value != 0)
            {
                createFolderButton.interactable = true;
            }
            else
            {
                createFolderButton.interactable = false;
            }
        }
    }



    public void CreateFolder()
    {
        // Create folder

        // Update the information
        songName = songNameInputField.text;
        artistName = artistNameInputField.text;

        // Create folder name
        beatmapFolderName = creatorName + "_" + songName + "_" + artistName;

        // Create a new folder for the beatmap
        var folder = Directory.CreateDirectory(@"C:\Beatmaps\" + beatmapFolderName);
        // Save the folder directory to save the files into later
        folderDirectory = @"C:\Beatmaps\" + beatmapFolderName + @"\";

        // Get the current date
        GetCurrentDate();

        createFolderButton.interactable = false;
        folderCreated = true;


        // Change active panel
        folderSetupPanel.gameObject.SetActive(false);
        fileSetupPanel.gameObject.SetActive(true);
    }

    public void OpenFolderDirectory()
    {
        //string path = @"c:\";
        System.Diagnostics.Process.Start(folderDirectory);
    }

    // Get the beatmap difficulty selected
    public void GetDifficultySelected()
    {
        switch (difficultyDropdown.value)
        {
            case 1:
                beatmapDifficulty = EASYDIFFICULTY;
                break;
            case 2:
                beatmapDifficulty = ADVANCEDDIFFICULTY;
                break;
            case 3:
                beatmapDifficulty = EXTRADIFFICULTY;
                break;
            default:
                beatmapDifficulty = EASYDIFFICULTY;
                break;
        }
    }

    // Get the current date
    public void GetCurrentDate()
    {
        beatmapCreatedDate = DateTime.Now.ToString("MM/dd/yyyy h:mm");
    }

    public void CheckIfAudioFileExists()
    {
        if (File.Exists(folderDirectory + AUDIONAME + AUDIOTYPE))
        {
            audioFileLoadingIconImage.color = scriptManager.colorManager.selectedColor;
            defaultAudioFileText.gameObject.SetActive(false);
            existsAudioFileText.gameObject.SetActive(true);
            audioFileFound = true;
        }
        else
        {
            audioFileLoadingIconImage.color = scriptManager.colorManager.whiteColor;
            defaultAudioFileText.gameObject.SetActive(true);
            existsAudioFileText.gameObject.SetActive(false);
            audioFileFound = false;
            filesUpdated = false;
        }
    }

    public void CheckIfImageFileExists()
    {
        if (File.Exists(folderDirectory + IMAGENAME + IMAGETYPE))
        {
            backgroundImageLoadingIconImage.color = scriptManager.colorManager.selectedColor;
            defaultBackgroundImageText.gameObject.SetActive(false);
            existsBackgroundImageText.gameObject.SetActive(true);
            backgroundImageFound = true;
        }
        else
        {
            backgroundImageLoadingIconImage.color = scriptManager.colorManager.whiteColor;
            defaultBackgroundImageText.gameObject.SetActive(true);
            existsBackgroundImageText.gameObject.SetActive(false);
            backgroundImageFound = false;
            filesUpdated = false;
        }
    }
}
