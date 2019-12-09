using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSelectToEditorManager : MonoBehaviour
{

    public bool editingExistingFile; // Controls whether an existing file is being edited in the editor 

    public bool hasLoadedExistingFileInformation; // Has all the file information and objects been instantiated

    int objectType;
    float spawnTime;
    float positionX, positionY, positionZ;
    Vector3 position;

    GameObject editorManager; // Editor manager in the editor scene

    PlacedObject placedObject;
    BeatmapSetup beatmapSetup;
    EditorUIManager editorUIManager;
    BeatsnapManager beatsnapManager;
    MetronomePro metronomePro;
    MetronomePro_Player metronomePro_Player;

    public bool EditingExistingFile
    {
        get { return editingExistingFile; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        editingExistingFile = false;
        hasLoadedExistingFileInformation = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex && editingExistingFile == true && hasLoadedExistingFileInformation == false)
        {
            LoadExistingFileInformation();
        }
        */
    }

    // Set editing existing file to true
    public void IsEditingExistingFile()
    {
        editingExistingFile = true;
    }

    // Update all the beatmap setup information
    private void UpdateBeatmapSetupInformation()
    {
        beatmapSetup.BeatmapCreatedDate = Database.database.LoadedBeatmapCreatedDate;
        beatmapSetup.BeatmapEasyDifficultyLevel = Database.database.LoadedBeatmapEasyDifficultyLevel;
        beatmapSetup.BeatmapAdvancedDifficultyLevel = Database.database.LoadedBeatmapAdvancedDifficultyLevel;
        beatmapSetup.BeatmapExtraDifficultyLevel = Database.database.LoadedBeatmapExtraDifficultyLevel;
        beatmapSetup.SongName = Database.database.LoadedSongName;
        beatmapSetup.SongArtist = Database.database.LoadedSongArtist;
        //beatmapSetup.SongClipChosenIndex = Database.database.LoadedSongClipChosenIndex;
        //beatmapSetup.SongPreviewStartTime = Database.database.LoadedSongPreviewStartTime;
        beatmapSetup.BeatmapDifficulty = Database.database.LoadedBeatmapDifficulty;
    }

    // Update song timing information
    private void UpdateSongTimingInformation()
    {
        int baseValue, stepValue;
        float bpmValue, offsetValue;
        baseValue = 4;
        stepValue = 4;
        bpmValue = Database.database.LoadedBPM;
        offsetValue = Database.database.LoadedOffsetMS;
        //metronomePro.GetSongData(bpmValue, offsetValue, baseValue, stepValue);
    }

    // Update beatsnaps
    private void UpdateBeatsnaps()
    {
        beatsnapManager.SetupBeatsnaps();
        beatsnapManager.CalculateBeatsnapSliderListValues();
    }

    // Destroy this game object
    public void DestroyEditSelectToEditorManager()
    {
        Destroy(this.gameObject);
    }

    // Load the existing file information
    public void LoadExistingFileInformation()
    {
        // Get reference to the editor game object
        editorManager = GameObject.FindGameObjectWithTag("EditorManager");
        // Get reference to the database script attached to the editor manager gameobject
        Database editorManagerDatabaseScript = editorManager.GetComponent<Database>();
        // Disable the database script attached the editor manager game object
        Destroy(editorManagerDatabaseScript);

        // Set the database reference to this gameobject
        Database.database = this.GetComponent<Database>();

        // Get reference to all editor scripts
        placedObject = FindObjectOfType<PlacedObject>();
        beatmapSetup = FindObjectOfType<BeatmapSetup>();
        editorUIManager = FindObjectOfType<EditorUIManager>();
        beatsnapManager = FindObjectOfType<BeatsnapManager>();
        metronomePro = FindObjectOfType<MetronomePro>();
        metronomePro_Player = FindObjectOfType<MetronomePro_Player>();

        // Update all beatmap setup information
        UpdateBeatmapSetupInformation();

        // Select the song automatically
        //metronomePro_Player.GetSongSelected(beatmapSetup.SongClipChosenIndex);

        // Update the bpm, offset, base and step information
        UpdateSongTimingInformation();

        // Enable tool panels
        editorUIManager.EnableToolButtons();
        // Change selected song text color to white
        editorUIManager.ChangeSelectSongTextColor();

        // Update beatsnaps
        UpdateBeatsnaps();

        // Instantiate all timeline objects
        InstantiateTimelineObjects();


        hasLoadedExistingFileInformation = true;
    }

    // Instantiate all timeline hit objects from the saved file
    private void InstantiateTimelineObjects()
    {
        for (int i = 0; i < Database.database.loadedPositionX.Count; i++)
        {
            objectType = Database.database.loadedObjectType[i];
            spawnTime = Database.database.loadedHitObjectSpawnTime[i];
            positionX = Database.database.loadedPositionX[i];
            positionY = Database.database.loadedPositionY[i];
            positionZ = Database.database.loadedPositionZ[i];
            position = new Vector3(positionX, positionY, positionZ);

            // Instantiate timeline hit object based on the saved information
            placedObject.LoadEditorHitObjectFromFile(objectType, spawnTime, position);
        }
    }
}
