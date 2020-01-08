using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Database : MonoBehaviour
{

    // Scripts
    public static Beatmap beatmap;
    public static Database database;
    private ScriptManager scriptManager;

    // Strings
    private string FILE_EXTENSION = ".dia";
    private string leaderboardTableName; // Beatmap leaderboard table name
    private string beatmapDifficulty; // Beatmap difficulty
    private string beatmapFolderDirectory; // Beatmap folder directory location

    // Integers
    private List<float> positionX = new List<float>(); // X position of the hit object
    private List<float> positionY = new List<float>(); // Y position of the hit object
    private List<float> positionZ = new List<float>(); // Z position of the hit object                      
    private List<float> hitObjectSpawnTime = new List<float>();// List of spawn times of the objects
    private List<int> objectType = new List<int>(); // List of object types
    private List<int> animationType = new List<int>(); // Animation type list
    private List<int> soundType = new List<int>(); // Sound type list

    // Loaded integers
    private List<float> loadedPositionX = new List<float>(); // Loaded X position of the hit object
    private List<float> loadedPositionY = new List<float>(); // Loaded Y position of the hit object
    private List<float> loadedPositionZ = new List<float>(); // Loaded Z position of the hit object
    private List<float> loadedHitObjectSpawnTime = new List<float>(); // Loaded list of spawn times               
    private List<int> loadedObjectType = new List<int>(); // Loaded of object types
    private List<int> loadedAnimationType = new List<int>(); // Animation type list
    private List<int> loadedSoundType = new List<int>(); // Sound type list
    private float loadedSongPreviewStartTime; // Loaded song preview time
    private float loadedBPM, loadedOffsetMS; // Loaded bpm and offset

    // Loaded strings
    private string loadedLeaderboardTableName; // Loaded leaderboard table name
    private string loadedSongName; // Loaded beatmap song name
    private string loadedSongArtist; // Loaded beatmap song artist
    private string loadedBeatmapCreator; // Loaded beatmap creator
    private string loadedBeatmapDifficulty; // Loaded beatmap difficulty
    private string loadedBeatmapFolderDirectory; // Loaded beatmap directory location
    private string loadedBeatmapDifficultyLevel;
    private string loadedBeatmapCreatedDate; // Loaded date of beatmap creation
    private int loadedKeyMode; // Key mode number

    // Properties

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

    public List<float> HitObjectSpawnTime
    {
        get { return hitObjectSpawnTime; }
        set { hitObjectSpawnTime = value; }
    }

    public List<int> ObjectType
    {
        get { return objectType; }
        set { objectType = value; }
    }

    public List<int> AnimationType
    {
        get { return animationType; }
        set { animationType = value; }
    }

    public List<int> SoundType
    {
        get { return soundType; }
        set { soundType = value; }
    }

    public List<float> LoadedPositionX
    {
        get { return loadedPositionX; }
        set { loadedPositionX = value; }
    }

    public List<float> LoadedPositionY
    {
        get { return loadedPositionY; }
        set { loadedPositionY = value; }
    }

    public List<float> LoadedPositionZ
    {
        get { return loadedPositionZ; }
        set { loadedPositionZ = value; }
    }

    public List<float> LoadedHitObjectSpawnTime
    {
        get { return loadedHitObjectSpawnTime; }
        set { loadedHitObjectSpawnTime = value; }
    }

    public List<int> LoadedObjectType
    {
        get { return loadedObjectType; }
        set { loadedObjectType = value; }
    }

    public List<int> LoadedAnimationType
    {
        get { return loadedAnimationType; }
        set { loadedAnimationType = value; }
    }

    public List<int> LoadedSoundType
    {
        get { return loadedSoundType; }
        set { loadedSoundType = value; }
    }

    public int KeyMode
    {
        get { return KeyMode; }
    }

    public string LoadedBeatmapCreatedDate
    {
        get { return loadedBeatmapCreatedDate; }
    }

    public string LoadedBeatmapDifficultyLevel
    {
        get { return loadedBeatmapDifficultyLevel; }
    }

    public string LoadedLeaderboardTableName
    {
        get { return loadedLeaderboardTableName; }
    }

    public string LoadedBeatmapFolderDirectory
    {
        get { return loadedBeatmapFolderDirectory; }
    }

    public string LoadedBeatmapDifficulty
    {
        get { return loadedBeatmapDifficulty; }
    }

    public string LoadedSongName
    {
        get { return loadedSongName; }
    }

    public string LoadedSongArtist
    {
        get { return loadedSongArtist; }
    }

    public string LoadedBeatmapCreator
    {
        get { return loadedBeatmapCreator; }
    }

    public string LeaderboardTableName
    {
        set { leaderboardTableName = value; }
    }

    public float LoadedBPM
    {
        get { return loadedBPM; }
    }

    public float LoadedOffsetMS
    {
        get { return loadedOffsetMS; }
    }

    public float LoadedSongPreviewStartTime
    {
        get { return loadedSongPreviewStartTime; }
    }

    private void Awake()
    {
        database = this;
        beatmap = new Beatmap();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    public void Save()
    {
        // Reference
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        // Get the difficulty name that the user has inputted
        // Get the beatmap directory for saving to the right beatmap folder

        // Create new beatmap folder with the name provided
        Stream stream = File.Open(scriptManager.setupBeatmap.FolderDirectory +
            scriptManager.setupBeatmap.BeatmapDifficulty + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        // Save the list of beatmap information for all hit objects
        for (int i = 0; i < positionX.Count; i++)
        {
            beatmap.PositionX.Add(positionX[i]);
            beatmap.PositionY.Add(positionY[i]);
            beatmap.PositionZ.Add(positionZ[i]);
            beatmap.HitObjectSpawnTime.Add(hitObjectSpawnTime[i]);
            beatmap.ObjectType.Add(objectType[i]);
            beatmap.AnimationType.Add(animationType[i]);
            beatmap.SoundType.Add(soundType[i]);
        }

        // Save beatmap information
        beatmap.SongName = scriptManager.setupBeatmap.SongName;
        beatmap.SongArtist = scriptManager.setupBeatmap.ArtistName;
        beatmap.BeatmapCreator = scriptManager.setupBeatmap.CreatorName;
        beatmap.BeatmapDifficulty = beatmapDifficulty;
        beatmap.BeatmapFolderDirectory = beatmapFolderDirectory;
        beatmap.BeatmapDifficulty = scriptManager.setupBeatmap.BeatmapDifficulty;
        beatmap.SongPreviewStartTime = scriptManager.setupBeatmap.SongPreviewStartTime;
        beatmap.BeatmapCreatedDate = scriptManager.setupBeatmap.BeatmapCreatedDate;

        // Timing information for the beatmap from the metronome
        beatmap.Bpm = scriptManager.metronomePro.Bpm;
        beatmap.OffsetMS = scriptManager.metronomePro.OffsetMS;

        // Save leaderboard table name
        beatmap.LeaderboardTableName = leaderboardTableName;

        // Save keymode
        beatmap.KeyMode = scriptManager.placedObject.KeyMode;

        bf.Serialize(stream, beatmap);
        stream.Close();
    }

    // Load the beatmap difficulty level only
    public void LoadBeatmapDifficultyLevel(string _beatmapFolderDirectory, string _beatmapDifficulty)
    {
        FileStream stream = File.Open(_beatmapFolderDirectory + @"\" + _beatmapDifficulty + FILE_EXTENSION, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap = (Beatmap)bf.Deserialize(stream);
        stream.Close();

        // Load the beatmap difficulty level
        loadedBeatmapDifficultyLevel = beatmap.BeatmapDifficultyLevel;
    }

    public void Load(string _beatmapFolderDirectory, string _beatmapDifficulty)
    {
        // Load the folder directory to load the map 
        loadedBeatmapFolderDirectory = _beatmapFolderDirectory + @"\";

        // Load the beatmap difficulty
        loadedBeatmapDifficulty = _beatmapDifficulty;

        FileStream stream = File.Open(loadedBeatmapFolderDirectory + loadedBeatmapDifficulty + FILE_EXTENSION, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap = (Beatmap)bf.Deserialize(stream);
        stream.Close();

        // Load the list of positions for all objects to the file?
        for (int i = 0; i < beatmap.PositionX.Count; i++)
        {
            loadedPositionX.Add(beatmap.PositionX[i]);
            loadedPositionY.Add(beatmap.PositionY[i]);
            loadedPositionZ.Add(beatmap.PositionZ[i]);
            loadedHitObjectSpawnTime.Add(beatmap.HitObjectSpawnTime[i]);
            loadedObjectType.Add(beatmap.ObjectType[i]);
            loadedAnimationType.Add(beatmap.AnimationType[i]);
            loadedSoundType.Add(beatmap.SoundType[i]);
        }

        // Load beatmap information
        loadedSongName = beatmap.SongName;
        loadedSongArtist = beatmap.SongArtist;
        loadedBeatmapCreator = beatmap.BeatmapCreator;
        loadedBeatmapDifficultyLevel = beatmap.BeatmapDifficultyLevel;
        loadedSongPreviewStartTime = beatmap.SongPreviewStartTime;
        loadedBeatmapCreatedDate = beatmap.BeatmapCreatedDate;

        // Timing information for the beatmap from the metronome
        loadedBPM = beatmap.Bpm;
        loadedOffsetMS = beatmap.OffsetMS;

        // Load key mode
        loadedKeyMode = beatmap.KeyMode;

        // Load beatmap table name for leaderboards
        loadedLeaderboardTableName = beatmap.LeaderboardTableName;

        Debug.Log(loadedSongName);
        Debug.Log(loadedPositionX.Count);
    }

    // Clear all loaded items, used in the song select screen to remove all loaded when selecting a new difficulty
    public void Clear()
    {
        loadedPositionX.Clear();
        loadedPositionY.Clear();
        loadedPositionZ.Clear();
        loadedHitObjectSpawnTime.Clear();
        loadedObjectType.Clear();
        loadedSoundType.Clear();
        loadedAnimationType.Clear();
        loadedLeaderboardTableName = "";
        loadedSongName = "";
        loadedSongArtist = "";
        loadedBeatmapCreator = "";
        loadedBeatmapDifficultyLevel = "";
        loadedBeatmapCreatedDate = "";
        loadedSongPreviewStartTime = 0;
        loadedBPM = 0;
        loadedOffsetMS = 0;
        loadedKeyMode = 0;
    }

    // Clear all placed objects in the editor
    public void ClearEditor()
    {
        positionX.Clear();
        positionY.Clear();
        positionZ.Clear();
        hitObjectSpawnTime.Clear();
        objectType.Clear();
        soundType.Clear();
        animationType.Clear();
    }
}