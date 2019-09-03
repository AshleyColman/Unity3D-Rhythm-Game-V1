using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSelectToEditorManager : MonoBehaviour {

    private bool editingExistingFile; // Controls whether an existing file is being edited in the editor 

    private bool hasLoadedExistingFileInformation; // Has all the file information and objects been instantiated

    private LevelChanger levelChanger;



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {

        levelChanger = FindObjectOfType<LevelChanger>();

        editingExistingFile = false;
        hasLoadedExistingFileInformation = false;

    }
	
	// Update is called once per frame
	void Update () {
	
        if (levelChanger.CurrentLevelIndex == levelChanger.EditSelectSceneIndex || levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


        if (levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex && hasLoadedExistingFileInformation == false)
        {
            LoadExistingFileInformation();
        }

	}

    // Set editing existing file to true
    public void IsEditingExistingFile()
    {
        editingExistingFile = true;
    }

    // Load the existing file information
    public void LoadExistingFileInformation()
    {
        hasLoadedExistingFileInformation = true;
    }
}
