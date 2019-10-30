using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Database : MonoBehaviour
{

    // Scripts
    public static Beatmap beatmap;
    public static Database database;
    private BeatmapSetup beatmapSetup;
    private LeaderboardCreate leaderboardCreate;
    private PlacedObject placedObject;
    private MetronomePro metronomePro;

    // Strings
    private string FILE_EXTENSION = ".dia";
    private string leaderboardTableName; // Beatmap leaderboard table name
    private string beatmapDifficulty; // Beatmap difficulty
    private string beatmapFolderDirectory; // Beatmap folder directory location

    // Integers
    public List<float> positionX = new List<float>(); // X position of the hit object
    public List<float> positionY = new List<float>(); // Y position of the hit object
    public List<float> positionZ = new List<float>(); // Z position of the hit object                      
    public List<float> hitObjectSpawnTime = new List<float>();// List of spawn times of the objects
    public List<int> objectType = new List<int>(); // List of object types

    // Loaded integers
    public List<float> loadedPositionX = new List<float>(); // Loaded X position of the hit object
    public List<float> loadedPositionY = new List<float>(); // Loaded Y position of the hit object
    public List<float> loadedPositionZ = new List<float>(); // Loaded Z position of the hit object
    public List<float> loadedHitObjectSpawnTime = new List<float>(); // Loaded list of spawn times               
    private float loadedSongPreviewStartTime; // Loaded song preview time
    public List<int> loadedObjectType = new List<int>(); // Loaded of object types
    private int loadedSongClipChosenIndex; // Loaded song index
    private float loadedBPM, loadedOffsetMS; // Loaded bpm and offset

    // Loaded strings
    public string loadedLeaderboardTableName; // Loaded leaderboard table name
    private string loadedSongName; // Loaded beatmap song name
    private string loadedSongArtist; // Loaded beatmap song artist
    private string loadedBeatmapCreator; // Loaded beatmap creator
    public string loadedBeatmapDifficulty; // Loaded beatmap difficulty
    private string loadedBeatmapFolderDirectory; // Loaded beatmap directory location
    private string loadedBeatmapEasyDifficultyLevel; // Loaded easy level
    private string loadedBeatmapAdvancedDifficultyLevel; // Loaded advanced level
    private string loadedBeatmapExtraDifficultyLevel; // Loaded extra level
    private string loadedBeatmapCreatedDate; // Loaded date of beatmap creation
    private int loadedKeyMode; // Key mode number

    // Bools
    private bool pressedKeyS, pressedKeyD, pressedKeyF, pressedKeyJ, pressedKeyK, pressedKeyL; // Keys pressed for beatmap

    // Loaded bools
    private bool loadedPressedKeyS, loadedPressedKeyD, loadedPressedKeyF, loadedPressedKeyJ, loadedPressedKeyK, loadedPressedKeyL; // Loaded keys pressed for beatmap


    // Properties

    public int KeyMode
    {
        get { return KeyMode; }
    }

    public string LoadedBeatmapCreatedDate
    {
        get { return loadedBeatmapCreatedDate; }
    }

    public string LoadedBeatmapEasyDifficultyLevel
    {
        get { return loadedBeatmapEasyDifficultyLevel; }
    }

    public string LoadedBeatmapAdvancedDifficultyLevel
    {
        get { return loadedBeatmapAdvancedDifficultyLevel; }
    }

    public string LoadedBeatmapExtraDifficultyLevel
    {
        get { return loadedBeatmapExtraDifficultyLevel; }
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

    public int LoadedSongClipChosenIndex
    {
        get { return loadedSongClipChosenIndex; }
    }

    public float LoadedSongPreviewStartTime
    {
        get { return loadedSongPreviewStartTime; }
    }

    public bool LoadedPressedKeyS
    {
        get { return loadedPressedKeyS; }
    }

    public bool LoadedPressedKeyD
    {
        get { return loadedPressedKeyD; }
    }

    public bool LoadedPressedKeyF
    {
        get { return loadedPressedKeyF; }
    }

    public bool LoadedPressedKeyJ
    {
        get { return loadedPressedKeyJ; }
    }

    public bool LoadedPressedKeyK
    {
        get { return loadedPressedKeyK; }
    }

    public bool LoadedPressedKeyL
    {
        get { return loadedPressedKeyL; }
    }


    private void Awake()
    {
        database = this;
        beatmap = new Beatmap();
    }

    private void Start()
    {
        // Reference
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
        placedObject = FindObjectOfType<PlacedObject>();
        metronomePro = FindObjectOfType<MetronomePro>();
    }

    public void Save()
    {
        // Get reference if null
        // Reference
        if (beatmapSetup == null)
        {
            beatmapSetup = FindObjectOfType<BeatmapSetup>();
        }

        if (placedObject == null)
        {
            placedObject = FindObjectOfType<PlacedObject>();
        }

        if (metronomePro == null)
        {
            metronomePro = FindObjectOfType<MetronomePro>();
        }

        // Get the difficulty name that the user has inputted
        // Get the beatmap directory for saving to the right beatmap folder

        // Create new beatmap folder with the name provided
        Stream stream = File.Open(beatmapSetup.FolderDirectory + beatmapSetup.BeatmapDifficulty + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        // Save the list of beatmap information for all hit objects
        for (int i = 0; i < positionX.Count; i++)
        {
            beatmap.PositionX.Add(positionX[i]);
            beatmap.PositionY.Add(positionY[i]);
            beatmap.PositionZ.Add(positionZ[i]);
            beatmap.HitObjectSpawnTime.Add(hitObjectSpawnTime[i]);
            beatmap.ObjectType.Add(objectType[i]);
        }

        // Save beatmap information
        beatmap.songName = beatmapSetup.SongName;
        beatmap.songArtist = beatmapSetup.SongArtist;
        beatmap.beatmapCreator = beatmapSetup.BeatmapCreator;
        beatmap.beatmapDifficulty = beatmapDifficulty;
        beatmap.beatmapFolderDirectory = beatmapFolderDirectory;
        beatmap.beatmapEasyDifficultyLevel = beatmapSetup.BeatmapEasyDifficultyLevel;
        beatmap.beatmapAdvancedDifficultyLevel = beatmapSetup.BeatmapAdvancedDifficultyLevel;
        beatmap.beatmapExtraDifficultyLevel = beatmapSetup.BeatmapExtraDifficultyLevel;
        beatmap.songClipChosenIndex = beatmapSetup.SongClipChosenIndex;
        beatmap.songPreviewStartTime = beatmapSetup.SongPreviewStartTime;
        beatmap.beatmapCreatedDate = beatmapSetup.BeatmapCreatedDate;

        // Timing information for the beatmap from the metronome
        beatmap.BPM = metronomePro.Bpm;
        beatmap.offsetMS = metronomePro.OffsetMS;

        // Save leaderboard table name
        beatmap.leaderboardTableName = leaderboardTableName;

        // Save the keys used for the map
        beatmap.pressedKeyS = placedObject.PressedKeyS;
        beatmap.pressedKeyD = placedObject.PressedKeyD;
        beatmap.pressedKeyF = placedObject.PressedKeyF;
        beatmap.pressedKeyJ = placedObject.PressedKeyJ;
        beatmap.pressedKeyK = placedObject.PressedKeyK;
        beatmap.pressedKeyL = placedObject.PressedKeyL;
        beatmap.keyMode = placedObject.KeyMode;

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
        loadedBeatmapEasyDifficultyLevel = beatmap.beatmapEasyDifficultyLevel;
        loadedBeatmapAdvancedDifficultyLevel = beatmap.beatmapAdvancedDifficultyLevel;
        loadedBeatmapExtraDifficultyLevel = beatmap.beatmapExtraDifficultyLevel;
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
        }

        // Load beatmap information
        loadedSongName = beatmap.songName;
        loadedSongArtist = beatmap.songArtist;
        loadedBeatmapCreator = beatmap.beatmapCreator;
        loadedBeatmapEasyDifficultyLevel = beatmap.beatmapEasyDifficultyLevel;
        loadedBeatmapAdvancedDifficultyLevel = beatmap.beatmapAdvancedDifficultyLevel;
        loadedBeatmapExtraDifficultyLevel = beatmap.beatmapExtraDifficultyLevel;
        loadedSongClipChosenIndex = beatmap.songClipChosenIndex;
        loadedSongPreviewStartTime = beatmap.songPreviewStartTime;
        loadedBeatmapCreatedDate = beatmap.beatmapCreatedDate;

        // Timing information for the beatmap from the metronome
        loadedBPM = beatmap.BPM;
        loadedOffsetMS = beatmap.offsetMS;

        // Load keys pressed
        loadedPressedKeyS = beatmap.pressedKeyS;
        loadedPressedKeyD = beatmap.pressedKeyD;
        loadedPressedKeyF = beatmap.pressedKeyF;
        loadedPressedKeyJ = beatmap.pressedKeyJ;
        loadedPressedKeyK = beatmap.pressedKeyK;
        loadedPressedKeyL = beatmap.pressedKeyL;
        loadedKeyMode = beatmap.keyMode;

        // Load beatmap table name for leaderboards
        loadedLeaderboardTableName = beatmap.leaderboardTableName;
    }

    // Clear all loaded items, used in the song select screen to remove all loaded when selecting a new difficulty
    public void Clear()
    {
        loadedPositionX.Clear();
        loadedPositionY.Clear();
        loadedPositionZ.Clear();
        loadedHitObjectSpawnTime.Clear();
        loadedObjectType.Clear();
        loadedLeaderboardTableName = "";
        loadedSongName = "";
        loadedSongArtist = "";
        loadedBeatmapCreator = "";
        loadedBeatmapEasyDifficultyLevel = "";
        loadedBeatmapAdvancedDifficultyLevel = "";
        loadedBeatmapExtraDifficultyLevel = "";
        loadedBeatmapCreatedDate = "";
        loadedSongPreviewStartTime = 0;
        loadedBPM = 0;
        loadedOffsetMS = 0;
        loadedSongClipChosenIndex = 0;
        loadedPressedKeyS = false;
        loadedPressedKeyD = false;
        loadedPressedKeyF = false;
        loadedPressedKeyJ = false;
        loadedPressedKeyK = false;
        loadedPressedKeyL = false;
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

        pressedKeyS = false;
        pressedKeyD = false;
        pressedKeyF = false;
        pressedKeyJ = false;
        pressedKeyK = false;
        pressedKeyL = false;
    }
}