
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MetronomeForEffects_Player : MonoBehaviour
{
    public bool active;
    bool playing = false;

    public double Bpm = 128;
    public double OffsetMS = 100;

    public int Step = 4;
    public int Base = 4;

    private bool previouslyPaused = false;

    void Start()
    {
        // Send Song Data to Metronome
        SendSongData();
    }

    // Sends Song Data to Metronome Pro script
    public void SendSongData()
    {
        FindObjectOfType<MetronomeForEffects>().GetSongData(Bpm, OffsetMS, Base, Step);
    }



}


