using UnityEngine;
using UnityEngine.UI;

public class SongSelectPreview : MonoBehaviour
{
    // UI
    public Image songPlayerBar;
    public Slider songPlayerSlider;

    // Audio
    public AudioSource songAudioSource;
    // Integers
    private float songVolume = 1f;
    private float amount;
    private float songAudioSourceTime;
    private int songClipChosenIndex;

    // Bools
    private bool active;
    private bool playing = false;

    // Scripts
    private SongDatabase songDatabase; // Required for loading all the songs in the game


    void Start()
    {
        // Get the reference
        songDatabase = FindObjectOfType<SongDatabase>();
    }

    // Update function is used to Update the Song Player Bar and Actual Position Text every frame and Player quick key buttons
    void Update()
    {
        /*
        if (active)
        {
            if (playing)
            {
                if (songAudioSource.isPlaying)
                {
                    // Update the song slider fill amount
                    amount = (songAudioSource.time) / (songAudioSource.clip.length);
                    songPlayerBar.fillAmount = amount;
                }
            }
        }
        */
    }

    // Get the song chosen to load 
    public void GetSongChosen(int _songChosenIndex)
    {
        // Get the index of the song chosens
        songClipChosenIndex = _songChosenIndex;
        // Play the song
        PlaySongPreview();
    }

    // Play the song preview
    public void PlaySongPreview()
    {
        // Play song
        songAudioSource.clip = songDatabase.songClip[songClipChosenIndex];
        songAudioSource.volume = songVolume;
        songAudioSource.Play();
        playing = true;
        active = true;
    }

    // Play the song preview
    public void PlaySongSelectScenePreview(float _songPreviewStartTime, int _songClipChosenIndex)
    {
        if (songDatabase == null)
        {
            // Get the reference
            songDatabase = FindObjectOfType<SongDatabase>();
        }
        // Play song
        songAudioSource.clip = songDatabase.songClip[_songClipChosenIndex];
        songAudioSource.volume = songVolume;

        if (_songPreviewStartTime >= 0 && _songPreviewStartTime < songDatabase.songClip[_songClipChosenIndex].length)
        {
            songAudioSource.time = _songPreviewStartTime;
        }

        songAudioSource.Play();
        playing = true;
        active = true;
    }
}