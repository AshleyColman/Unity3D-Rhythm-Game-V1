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

    public float SpecialTimeStart;
    public float SpecialTimeEnd;

    public string songName;
    public string songArtist;
    public string beatmapCreator;
    public string beatmapDifficulty;
    public string beatmapFolderDirectory;

    public string beatmapAdvancedDifficultyLevel;
    public string beatmapExtraDifficultyLevel;
    public int songClipChosenIndex;
}
