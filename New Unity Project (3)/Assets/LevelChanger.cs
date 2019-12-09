using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private int currentSceneIndex;
    private const int menuSceneIndex = 0, gameplaySceneIndex = 1;

    public int MenuSceneIndex
    {
        get { return menuSceneIndex; }
    }

    public int GameplaySceneIndex
    {
        get { return gameplaySceneIndex; }
    }

    public int CurrentSceneIndex
    {
        get { return currentSceneIndex; }
    }

    private void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
