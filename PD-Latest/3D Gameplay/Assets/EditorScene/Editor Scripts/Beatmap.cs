using System;
using System.Collections.Generic;
[Serializable]
public class Beatmap
{
    public List<float> HitObjectSpawnTime = new List<float>();

    public List<float> PositionX = new List<float>();
    public List<float> PositionY = new List<float>();
    public List<float> PositionZ = new List<float>();

    public List<int> ObjectType = new List<int>();

    public string songName;
    public string songArtist;
    public string beatmapCreator;
    public string beatmapDifficulty;
    public string beatmapFolderDirectory;

    public string beatmapEasyDifficultyLevel;
    public string beatmapAdvancedDifficultyLevel;
    public string beatmapExtraDifficultyLevel;
    public int songClipChosenIndex;

    public string leaderboardTableName;

    public bool pressedKeyS;
    public bool pressedKeyD;
    public bool pressedKeyF;
    public bool pressedKeyJ;
    public bool pressedKeyK;
    public bool pressedKeyL;

    public bool beatmapRanked;

    public float songPreviewStartTime;



    // Timing information for the beatmap from the metronome
    public float BPM;
    public float offsetMS;
}
