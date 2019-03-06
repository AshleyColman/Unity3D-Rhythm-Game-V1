using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Database : MonoBehaviour {

    public static Beatmap beatmap;

    string SAVE_FILE = "/SAVEGAME";
    string FILE_EXTENSION = ".dia";

    public string _playername;
    public int _level;

    private void Awake()
    {
        beatmap = new Beatmap();
    }

    public void Save()
    {
        Stream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap.playername = _playername;
        beatmap.level = _level;

        bf.Serialize(stream, beatmap);
        stream.Close();
    }
    
    public void Load()
    {
        Stream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();

        beatmap = (Beatmap)bf.Deserialize(stream);
        stream.Close();

        _playername = beatmap.playername;
        _level = beatmap.level;
    }
}
