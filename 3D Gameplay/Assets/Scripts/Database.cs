using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Database : MonoBehaviour {

    public static Beatmap beatmap;
    public static Database database;

    string SAVE_FILE = "/SAVEGAME";
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

    private void Awake()
    {
        database = this;
        beatmap = new Beatmap();
    }

    public void Save()
    {
        Stream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        /*
        beatmap.hitObjectPositionX = hitObjectPositionX;
        beatmap.hitObjectPositionY = hitObjectPositionY;
        beatmap.hitObjectPositionZ = hitObjectPositionZ;
        */

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

        bf.Serialize(stream, beatmap);
        stream.Close();
    }
    
    public void Load()
    {
        //if (File.Exists(Application.persistentDataPath + SAVE_FILE + FILE_EXTENSION))
        //{
            FileStream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            beatmap = (Beatmap)bf.Deserialize(stream);
            stream.Close();

        Debug.Log("loading");
        Debug.Log("saved file list size: " + beatmap.PositionX.Count);
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

    }
}
