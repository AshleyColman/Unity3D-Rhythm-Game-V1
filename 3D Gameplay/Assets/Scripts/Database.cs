using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class Database : MonoBehaviour {

    public static Beatmap beatmap;
    public static Database database;
    public BeatmapSetup beatmapSetup;
    public LeaderboardCreate leaderboardCreate;
    public PlacedObject placedObject;


    string FILE_EXTENSION = ".dia";

    public float hitObjectPositionX;
    public float hitObjectPositionY;
    public float hitObjectPositionZ;

    // List of positions of the objects
    public List<float> PositionX = new List<float>();
    public List<float> PositionY = new List<float>();
    public List<float> PositionZ = new List<float>();

    // Loaded positions of the objects
    // List of positions of the objects
    public List<float> LoadedPositionX = new List<float>();
    public List<float> LoadedPositionY = new List<float>();
    public List<float> LoadedPositionZ = new List<float>();

    // List of spawn times of the objects
    public List<float> HitObjectSpawnTime = new List<float>();
    // Loaded list of spawn times
    public List<float> LoadedHitObjectSpawnTime = new List<float>();

    // List of object types
    public List<int> ObjectType = new List<int>();
    // Loaded of object types
    public List<int> LoadedObjectType = new List<int>();

    // Special time start and end 
    public float SpecialTimeStart;
    public float SpecialTimeEnd;
    // Loaded Special time start and end
    public float LoadedSpecialTimeStart;
    public float LoadedSpecialTimeEnd;

    // Beatmap leaderboard table name
    public string leaderboardTableName;
    // Loaded leaderboard table name
    public string loadedLeaderboardTableName;

    // Beatmap setup variables
    public string songName;
    public string songArtist;
    public string beatmapCreator;
    public string beatmapDifficulty;
    public string beatmapFolderDirectory;
    public string beatmapEasyDifficultyLevel;
    public string beatmapAdvancedDifficultyLevel;
    public string beatmapExtraDifficultyLevel;
    public int songClipChosenIndex;
    public float songPreviewStartTime;

    // Loaded beatmap variables
    public string loadedSongName;
    public string loadedSongArtist;
    public string loadedBeatmapCreator;
    public string loadedBeatmapDifficulty;
    public string loadedBeatmapFolderDirectory;
    public string loadedbeatmapEasyDifficultyLevel;
    public string loadedbeatmapAdvancedDifficultyLevel;
    public string loadedbeatmapExtraDifficultyLevel;
    public int loadedSongClipChosenIndex;
    public float loadedSongPreviewStartTime;

    // Keys pressed for beatmap
    public bool pressedKeyS;
    public bool pressedKeyD;
    public bool pressedKeyF;
    public bool pressedKeyJ;
    public bool pressedKeyK;
    public bool pressedKeyL;

    // Loaded keys pressed for beatmap
    public bool loadedPressedKeyS;
    public bool loadedPressedKeyD;
    public bool loadedPressedKeyF;
    public bool loadedPressedKeyJ;
    public bool loadedPressedKeyK;
    public bool loadedPressedKeyL;

    

    private void Awake()
    {
        database = this;
        beatmap = new Beatmap();
    }

    private void Start()
    {
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
        placedObject = FindObjectOfType<PlacedObject>();
    }
    public void Save()
    {


        // Get the difficulty name that the user has inputted
        beatmapDifficulty = beatmapSetup.beatmapDifficulty;
        // Get the beatmap directory for saving to the right beatmap folder
        beatmapFolderDirectory = beatmapSetup.folderDirectory;
        Debug.Log("beatmapsavedirectory: " + beatmapFolderDirectory);
        // Create new beatmap folder with the name provided
        Stream stream = File.Open(beatmapFolderDirectory + beatmapDifficulty + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        // Save the list of positions for all objects to the file?
        for (int i = 0; i < PositionX.Count; i++)
        {
            beatmap.PositionX.Add(PositionX[i]);
            beatmap.PositionY.Add(PositionY[i]);
            beatmap.PositionZ.Add(PositionZ[i]);
        }

        // Save list of all spawn times
        for (int i = 0; i < HitObjectSpawnTime.Count; i++)
        {
            beatmap.HitObjectSpawnTime.Add(HitObjectSpawnTime[i]);
        }

        // Save all object types
        for (int i = 0; i < ObjectType.Count; i++)
        {
            beatmap.ObjectType.Add(ObjectType[i]);
        }

        // Save special time start and load
        beatmap.SpecialTimeStart = SpecialTimeStart;
        beatmap.SpecialTimeEnd = SpecialTimeEnd;

        // Get beatmap information from the setup
        songName = beatmapSetup.songName;
        songArtist = beatmapSetup.songArtist;
        beatmapCreator = beatmapSetup.beatmapCreator;
        beatmapEasyDifficultyLevel = beatmapSetup.beatmapEasyDifficultyLevel;
        beatmapAdvancedDifficultyLevel = beatmapSetup.beatmapAdvancedDifficultyLevel;
        beatmapExtraDifficultyLevel = beatmapSetup.beatmapExtraDifficultyLevel;
        songClipChosenIndex = beatmapSetup.songClipChosenIndex;
        songPreviewStartTime = beatmapSetup.songPreviewStartTime;

        // Save beatmap information
        beatmap.songName = songName;
        beatmap.songArtist = songArtist;
        beatmap.beatmapCreator = beatmapCreator;
        beatmap.beatmapDifficulty = beatmapDifficulty;
        beatmap.beatmapFolderDirectory = beatmapFolderDirectory;
        beatmap.beatmapEasyDifficultyLevel = beatmapEasyDifficultyLevel;
        beatmap.beatmapAdvancedDifficultyLevel = beatmapAdvancedDifficultyLevel;
        beatmap.beatmapExtraDifficultyLevel = beatmapExtraDifficultyLevel;
        beatmap.songClipChosenIndex = songClipChosenIndex;
        beatmap.songPreviewStartTime = songPreviewStartTime;

        // Save leaderboard table name
        beatmap.leaderboardTableName = leaderboardTableName;

        // Get the keys pressed for the beatmap
        pressedKeyS = placedObject.pressedKeyS;
        pressedKeyD = placedObject.pressedKeyD;
        pressedKeyF = placedObject.pressedKeyF;
        pressedKeyJ = placedObject.pressedKeyJ;
        pressedKeyK = placedObject.pressedKeyK;
        pressedKeyL = placedObject.pressedKeyL;
        // Save the keys used for the map
        beatmap.pressedKeyS = pressedKeyS;
        beatmap.pressedKeyD = pressedKeyD;
        beatmap.pressedKeyF = pressedKeyF;
        beatmap.pressedKeyJ = pressedKeyJ;
        beatmap.pressedKeyK = pressedKeyK;
        beatmap.pressedKeyL = pressedKeyL;


        bf.Serialize(stream, beatmap);
        stream.Close();
    }

    // Load the beatmap difficulty level only
    public void LoadBeatmapDifficultyLevel(string beatmapFolderDirectoryPass, string beatmapDifficultyPass)
    {
        // Load the folder directory to load the map 
        beatmapFolderDirectory = beatmapFolderDirectoryPass + @"\";
        // Load the beatmap difficulty
        beatmapDifficulty = beatmapDifficultyPass;

        FileStream stream = File.Open(beatmapFolderDirectory + beatmapDifficulty + FILE_EXTENSION, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap = (Beatmap)bf.Deserialize(stream);
        stream.Close();


        // Load the beatmap difficulty level
        loadedbeatmapEasyDifficultyLevel = beatmap.beatmapEasyDifficultyLevel;
        loadedbeatmapAdvancedDifficultyLevel = beatmap.beatmapAdvancedDifficultyLevel;
        loadedbeatmapExtraDifficultyLevel = beatmap.beatmapExtraDifficultyLevel;
    }

    public void Load(string beatmapFolderDirectoryPass, string beatmapDifficultyPass)
    {
        // Load the folder directory to load the map 
        beatmapFolderDirectory = beatmapFolderDirectoryPass + @"\";
        // Load the beatmap difficulty
        beatmapDifficulty = beatmapDifficultyPass;

            FileStream stream = File.Open(beatmapFolderDirectory + beatmapDifficulty + FILE_EXTENSION, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

        beatmap = (Beatmap)bf.Deserialize(stream);
            stream.Close();

        // Load the list of positions for all objects to the file?
        for (int i = 0; i < beatmap.PositionX.Count; i++)
        {
            LoadedPositionX.Add(beatmap.PositionX[i]);
            LoadedPositionY.Add(beatmap.PositionY[i]);
            LoadedPositionZ.Add(beatmap.PositionZ[i]);
        }

        // Load the spawn times
        for (int i = 0; i < beatmap.HitObjectSpawnTime.Count; i++)
        {
            LoadedHitObjectSpawnTime.Add(beatmap.HitObjectSpawnTime[i]);
        }

        // Load object types
        for (int i = 0; i < beatmap.ObjectType.Count; i++)
        {
            LoadedObjectType.Add(beatmap.ObjectType[i]);
        }

        // Load special time start and end
        LoadedSpecialTimeStart = beatmap.SpecialTimeStart;
        LoadedSpecialTimeEnd = beatmap.SpecialTimeEnd;

        // Load beatmap information
        loadedSongName = beatmap.songName;
        loadedSongArtist = beatmap.songArtist;
        loadedBeatmapCreator = beatmap.beatmapCreator;
        loadedBeatmapDifficulty = beatmap.beatmapDifficulty;
        loadedbeatmapEasyDifficultyLevel = beatmap.beatmapEasyDifficultyLevel;
        loadedbeatmapAdvancedDifficultyLevel = beatmap.beatmapAdvancedDifficultyLevel;
        loadedbeatmapExtraDifficultyLevel = beatmap.beatmapExtraDifficultyLevel;
        loadedSongClipChosenIndex = beatmap.songClipChosenIndex;
        loadedSongPreviewStartTime = beatmap.songPreviewStartTime;

        // Load keys pressed
        loadedPressedKeyS = beatmap.pressedKeyS;
        loadedPressedKeyD = beatmap.pressedKeyD;
        loadedPressedKeyF = beatmap.pressedKeyF;
        loadedPressedKeyJ = beatmap.pressedKeyJ;
        loadedPressedKeyK = beatmap.pressedKeyK;
        loadedPressedKeyL = beatmap.pressedKeyL;

        // Load beatmap table name for leaderboards
        loadedLeaderboardTableName = beatmap.leaderboardTableName;
}

    // Clear all loaded items, used in the song select screen to remove all loaded when selecting a new difficulty
    public void Clear()
    {
        LoadedPositionX.Clear();
        LoadedPositionY.Clear();
        LoadedPositionZ.Clear();
        LoadedHitObjectSpawnTime.Clear();
        LoadedObjectType.Clear();

        LoadedSpecialTimeStart = 0;
        LoadedSpecialTimeEnd = 0;

        loadedSongName = "";
        loadedSongArtist = "";
        loadedBeatmapCreator = "";
        loadedbeatmapEasyDifficultyLevel = "";
        loadedbeatmapAdvancedDifficultyLevel = "";
        loadedbeatmapExtraDifficultyLevel = "";
        loadedSongPreviewStartTime = 0;

        loadedSongClipChosenIndex = 0;
        loadedLeaderboardTableName = "";

        loadedPressedKeyS = false;
        loadedPressedKeyD = false;
        loadedPressedKeyF = false;
        loadedPressedKeyJ = false;
        loadedPressedKeyK = false;
        loadedPressedKeyL = false;
    }

    // Clear all placed objects in the editor
    public void ClearEditor()
    {
        PositionX.Clear();
        PositionY.Clear();
        PositionZ.Clear();
        HitObjectSpawnTime.Clear();
        ObjectType.Clear();

        //SpecialTimeStart = 0;
        //SpecialTimeEnd = 0;


        pressedKeyS = false;
        pressedKeyD = false;
        pressedKeyF = false;
        pressedKeyJ = false;
        pressedKeyK = false;
        pressedKeyL = false;
    }
}
