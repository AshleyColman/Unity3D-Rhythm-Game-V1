using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider songVolumeSlider;

    private ScriptManager scriptManager;

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Set default to 1
        scriptManager.rhythmVisualizatorPro.audioSource.volume = songVolumeSlider.value;
    }

    // Update the song volume
    public void UpdateSongVolume()
    {
        scriptManager.rhythmVisualizatorPro.audioSource.volume = songVolumeSlider.value;
    }
}
