using UnityEngine;

public class GameplayDifficultyPanel : MonoBehaviour {

    // Scripts
    private GameObject difficultyPanel; // Gameplay difficulty panel game object / easy / advanced / extra
    private FeverTimeManager feverTimeManager; // Fever time manager for fever time control

    private void Start()
    {
        // Reference
        difficultyPanel = this.gameObject; // Get the reference to the gameplayDifficultyPanel gameobject
        feverTimeManager = FindObjectOfType<FeverTimeManager>(); // Get the reference to the fever time manager
    }

    private void Update()
    {
        // Check if fever time
        CheckIfFeverTime();
    }

    // Check if fever time
    private void CheckIfFeverTime()
    {
        // If fever time is active
        if (feverTimeManager.FeverTimeActivated == true && difficultyPanel.gameObject.activeSelf == true)
        {
            // Deactivate difficulty panel
            difficultyPanel.gameObject.SetActive(false);
        }
        else if (feverTimeManager.FeverTimeActivated == false && difficultyPanel.gameObject.activeSelf == false)
        {
            // Activate difficulty panel
            difficultyPanel.gameObject.SetActive(true);
        }
    }
}
