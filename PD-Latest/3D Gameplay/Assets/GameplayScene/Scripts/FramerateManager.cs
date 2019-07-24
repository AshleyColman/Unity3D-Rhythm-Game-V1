using UnityEngine;

public class FramerateManager : MonoBehaviour {

    void Awake()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 120;
    }
}
