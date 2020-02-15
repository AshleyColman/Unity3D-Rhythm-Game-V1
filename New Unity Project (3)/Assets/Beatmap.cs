using System;
using System.Collections.Generic;

// BEATMAP FILE
[Serializable]
public class Beatmap
{
    // HIT OBJECT
    private List<float> hitObjectSpawnTime = new List<float>();
    private List<float> positionX = new List<float>();
    private List<float> positionY = new List<float>();
    private List<float> positionZ = new List<float>();
    private List<int> objectType = new List<int>();
    private List<int> soundType = new List<int>();
    private List<int> animationType = new List<int>();

    // PATH
    private List<float> pointPositionX = new List<float>();
    private List<float> pointPositionY = new List<float>();
    private bool isClosed;
    private bool autoSetControlPoints;
    private float spacing;
    private float resolution;
    private int beatInterval;

    // SONG INFORMATION
    private string songName;
    private string songArtist;
    private string beatmapCreator;
    private string beatmapFolderDirectory;
    private string beatmapCreatedDate;
    private string beatmapCreatorMessage;
    private string beatmapDifficulty;
    private string beatmapDifficultyLevel;
    private float songPreviewStartTime;

    // BEATMAP
    private string leaderboardTableName;
    private int keyMode;

    // ONLINE
    private bool beatmapRanked;

    // TIMING
    private float bpm;
    private float offsetMS;

    // Properties

    public List<int> SoundType
    {
        get { return soundType; }
        set { soundType = value; }
    }

    public List<int> AnimationType
    {
        get { return animationType; }
        set { animationType = value; }
    }

    public List<float> HitObjectSpawnTime
    {
        get { return hitObjectSpawnTime; }
        set { hitObjectSpawnTime = value; }
    }

    public List<float> PositionX
    {
        get { return positionX; }
        set { positionX = value; }
    }

    public List<float> PositionY
    {
        get { return positionY; }
        set { positionY = value; }
    }

    public List<float> PositionZ
    {
        get { return positionZ; }
        set { positionZ = value; }
    }

    public List<int> ObjectType
    {
        get { return objectType; }
        set { objectType = value; }
    }

    public List<float> PointPositionX
    {
        get { return pointPositionX; }
        set { pointPositionX = value; }
    }

    public List<float> PointPositionY
    {
        get { return pointPositionY; }
        set { pointPositionY = value; }
    }

    public bool IsClosed
    {
        get { return isClosed; }
        set { isClosed = value; }
    }

    public bool AutoSetControlPoints
    {
        get { return autoSetControlPoints; }
        set { autoSetControlPoints = value; }
    }

    public float Spacing
    {
        get { return spacing; }
        set { spacing = value; }
    }

    public float Resolution
    {
        get { return resolution; }
        set { resolution = value; }
    }

    public int BeatInterval
    {
        get { return beatInterval; }
        set { beatInterval = value; }
    }

    public string SongName
    {
        get { return songName; }
        set { songName = value; }
    }

    public string SongArtist
    {
        get { return songArtist; }
        set { songArtist = value; }
    }

    public string BeatmapCreator
    {
        get { return beatmapCreator; }
        set { beatmapCreator = value; }
    }

    public string BeatmapFolderDirectory
    {
        get { return beatmapFolderDirectory; }
        set { beatmapFolderDirectory = value; }
    }

    public string BeatmapCreatedDate
    {
        get { return beatmapCreatedDate; }
        set { beatmapCreatedDate = value; }
    }

    public string BeatmapCreatorMessage
    {
        get { return beatmapCreatorMessage; }
        set { beatmapCreatorMessage = value; }
    }

    public string BeatmapDifficulty
    {
        get { return beatmapDifficulty; }
        set { beatmapDifficulty = value; }
    }

    public string BeatmapDifficultyLevel
    {
        get { return beatmapDifficultyLevel; }
        set { beatmapDifficultyLevel = value; }
    }

    public float SongPreviewStartTime
    {
        get { return songPreviewStartTime; }
        set { songPreviewStartTime = value; }
    }

    public string LeaderboardTableName
    {
        get { return leaderboardTableName; }
        set { leaderboardTableName = value; }
    }

    public int KeyMode
    {
        get { return keyMode; }
        set { keyMode = value; }
    }

    public bool BeatmapRanked
    {
        get { return beatmapRanked; }
        set { beatmapRanked = value; }
    }

    public float Bpm
    {
        get { return bpm; }
        set { bpm = value; }
    }

    public float OffsetMS
    {
        get { return offsetMS; }
        set { offsetMS = value; }
    }
}
