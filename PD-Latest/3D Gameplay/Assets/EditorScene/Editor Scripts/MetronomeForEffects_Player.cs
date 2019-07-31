
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MetronomeForEffects_Player : MonoBehaviour
{
    public bool active;
    bool playing = false;
    public float bpm = 175f;
    public float offsetMS = 280;
    private bool previouslyPaused = false;

    void Start()
    {
        // Send Song Data to Metronome
        SendSongData();
    }

    // Sends Song Data to Metronome Pro script
    public void SendSongData()
    {
        FindObjectOfType<MetronomeForEffects>().GetSongData(bpm, offsetMS);
    }
}


