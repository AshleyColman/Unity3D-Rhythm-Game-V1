using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private int currentSceneIndex;
    private const int menuSceneIndex = 0, editorSceneIndex = 1, gameplaySceneIndex = 2;

    public int MenuSceneIndex
    {
        get { return menuSceneIndex; }
    }

    public int EditorSceneIndex
    {
        get { return editorSceneIndex; }
    }

    public int CurrentSceneIndex
    {
        get { return currentSceneIndex; }
    }

    public int GameplaySceneIndex
    {
        get { return gameplaySceneIndex; }
    }

    private void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }
}
