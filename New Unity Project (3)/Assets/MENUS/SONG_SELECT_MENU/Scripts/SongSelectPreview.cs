using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

public class SongSelectPreview : MonoBehaviour
{
    // UI
    public Image songPlayerBar;
    public Slider songPlayerSlider;

    // Audio
    public AudioSource songAudioSource;
    public AudioClip beatmapFileAudioClip;

    // Integers
    private float songVolume = 1f;
    private float songAudioSourceTime;

    // Bools
    private bool active;
    private bool playing = false;

    // Strings
    private string filePath, completeAudioFilePath;
    private const string audioType = ".ogg", audioName = "audio";

    // Scripts
    private ScriptManager scriptManager;

    void Start()
    {
        // Get the reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Play the song preview
    public void PlaySongPreview()
    {
        songAudioSource.volume = songVolume;
        songAudioSource.Play();
        playing = true;
        active = true;
    }

    public void GetBeatmapAudio(string _filePath, float _songPreviewStartTime)
    {
        // Get the file address
        filePath = _filePath + @"\";
        completeAudioFilePath = filePath + audioName + audioType;

        if (File.Exists(completeAudioFilePath))
        {
            StartCoroutine(GetAudioFile(_filePath, _songPreviewStartTime));
        }
        else
        {
            scriptManager.messagePanel.DisplayMessage("BEATMAP AUDIO FILE NOT FOUND", scriptManager.uiColorManager.offlineColorSolid);
        }
    }

    private IEnumerator GetAudioFile(string _filePath, float _songPreviewStartTime)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + completeAudioFilePath, AudioType.OGGVORBIS))
        {
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                scriptManager.messagePanel.DisplayMessage("BEATMAP AUDIO FILE NOT FOUND", scriptManager.uiColorManager.offlineColorSolid);
            }
            else
            {
                beatmapFileAudioClip = DownloadHandlerAudioClip.GetContent(www);

                yield return new WaitForSeconds(0.05f);

                // Assign the clip
                songAudioSource.clip = beatmapFileAudioClip;

                // Play audio clip
                if (_songPreviewStartTime >= 0 && _songPreviewStartTime < beatmapFileAudioClip.length)
                {
                    songAudioSource.volume = songVolume;
                    songAudioSource.time = _songPreviewStartTime;
                    songAudioSource.Play();
                    playing = true;
                    active = true;

                    // Update metronome timing information for rhythm animations
                    scriptManager.metronomeForEffects.CalculateIntervals();
                    scriptManager.metronomeForEffects.CalculateActualStep();
                    scriptManager.metronomeForEffects.CalculateCurrentTick();
                }
            }
        }
    }
}