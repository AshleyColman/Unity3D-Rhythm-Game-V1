using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongProgressBar : MonoBehaviour
{

    // UI
    public TextMeshProUGUI actualPosition;
    public TextMeshProUGUI songTotalDuration;
    public Image playAndPauseButton;
    public Image songPlayerBar;
    public Slider songPlayerSlider;

    // Audio
    public AudioSource songAudioSource;

    // Integers
    private float songVolume;
    private float amount;
    private float songAudioSourceTime;
    private float dsptimesong;
    private float songTimePosition; // The current position of the song (in seconds)
    private sbyte tripleTimeSongMultiplier; // Multipliers for multiplying the current song time
    private float doubleTimeSongMultiplier, halfTimeSongMultiplier; // Multipliers for multiplying the current song time

    private int songClipChosenIndex;

    // Bools
    private bool active;
    private bool playing;
    private bool hasPressedSpacebar; // Used for tracking if the spacebar has been pressed which starts the song, prevents restarting of the song if spacebar is pressed again

    // Scripts
    private ScriptManager scriptManager;

    // Properties

    public float SongTimePosition
    {
        get { return songTimePosition; }
    }


    void Start()
    {
        // Initialize
        songVolume = 0.5f;
        hasPressedSpacebar = false;
        tripleTimeSongMultiplier = 2;
        doubleTimeSongMultiplier = 1.5f;
        halfTimeSongMultiplier = 1.25f;


        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    void Update()
    {
        /*
        // Editor scene
        if (levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex)
        {
            // If setting up the beatmap has finished and the spacebar has been pressed
            if (hasPressedSpacebar == false)
            {
                // Play song when user press Space button
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlaySong();
                }
            }
        }
        else
        {
            // Gameplay and results scene
            if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex || levelChanger.CurrentLevelIndex == levelChanger.ResultsSceneIndex)
            {
                // Check for starting the song key input
                if (Input.GetKeyDown(KeyCode.Space) && hasPressedSpacebar == false && loadAndRunBeatmap.LevelChangerAnimationTimer >= 2f)
                {
                    // Spacebar has been pressed
                    hasPressedSpacebar = true;

                    // Set the song to the song loaded
                    songClipChosenIndex = Database.database.LoadedSongClipChosenIndex;
                    songAudioSource.clip = songDatabase.songClip[songClipChosenIndex];
                    songAudioSource.volume = songVolume;

                    // Play the song
                    songAudioSource.Play();
                    playing = true;
                    active = true;

                    // Record the time when the song starts
                    dsptimesong = (float)AudioSettings.dspTime;
                }


                // If the spacebar has been pressed start tracking the song time
                if (hasPressedSpacebar == true)
                {
                    //calculate the position in seconds
                    songTimePosition = (float)(AudioSettings.dspTime - dsptimesong);

                    // Check mods used and multiply the dsp song time by the mod used 
                    switch (playerSkillsManager.ModSelected)
                    {
                        case "TRIPLE TIME":
                            songTimePosition = songTimePosition * tripleTimeSongMultiplier;
                            break;
                        case "DOUBLE TIME":
                            songTimePosition = songTimePosition * doubleTimeSongMultiplier;
                            break;
                        case "HALF TIME":
                            songTimePosition = songTimePosition / halfTimeSongMultiplier;
                            break;
                    }
                }

                // Dont destroy the song audio source in gameplay to results page to continue the song playing after the gameplay has ended
                DontDestroyOnLoad(this.gameObject);
            }
        }
        */

        // If the song player bar exists update it, if not (such as on results scene) don't update
        if (songPlayerBar != null)
        {
            if (active)
            {
                if (playing)
                {
                    if (songAudioSource.isPlaying)
                    {
                        amount = (songAudioSource.time) / (songAudioSource.clip.length);
                        songPlayerBar.fillAmount = amount;
                    }
                }
            }
        }
    }

    public static class UtilityMethods
    {
        public static string FromSecondsToMinutesAndSeconds(float seconds)
        {
            int sec = (int)(seconds % 60f);
            int min = (int)((seconds / 60f) % 60f);

            string minSec = min.ToString("D2") + ":" + sec.ToString("D2");
            return minSec;
        }
    }

    // Play the song
    public void PlaySong()
    {
        // Has pressed the spacebar
        hasPressedSpacebar = true;
        // Play song
        songAudioSource.clip = scriptManager.songDatabase.songClip[songClipChosenIndex];
        songAudioSource.volume = songVolume;
        songAudioSource.Play();
        playing = true;
        active = true;
    }

    // Get the song chosen to load 
    public void GetSongChosen(int _songChosenIndex)
    {
        songClipChosenIndex = _songChosenIndex;
    }

    // Reset the song in the editor if the reset button is pressed
    public void ResetSongInEditor()
    {
        // Stop the current song
        songAudioSource.Stop();
        songAudioSource.time = 0f;
        // Reset the hasPressedSpacebar
        hasPressedSpacebar = false;
        // Reset amount of playbar 
        amount = 0f;
        songPlayerBar.fillAmount = amount;
        // Reset actual position text on playbar
        actualPosition.text = "0:00";
    }
}