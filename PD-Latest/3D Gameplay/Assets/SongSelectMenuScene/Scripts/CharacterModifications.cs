using UnityEngine;

public class CharacterModifications : MonoBehaviour {

    // Gameobjects
    public GameObject leaderboard; // Leaderboard to disable when activating the characterModificationPanel
    public GameObject characterModificationsPanel; // The character modifications panel

    // Bools
    private bool characterModificationsPanelActive; // Used for controlling the visiblity of the panel


    void Start () {

        // Initialize
        characterModificationsPanelActive = false;
    }

    // Turn on or off the character modifications panel
    public void toggleCharacterModificationsPanel()
    {
        // If the character panel is currently not active
        if (characterModificationsPanelActive == false)
        {
            // Activate the character panel
            characterModificationsPanel.gameObject.SetActive(true);
            // Disable the leaderboard panel
            leaderboard.gameObject.SetActive(false);
            // Set to true
            characterModificationsPanelActive = true;
        }
        else if (characterModificationsPanelActive == true)
        {
            // Turn off the character panel
            characterModificationsPanel.gameObject.SetActive(false);
            // Activate the leaderboard panel
            leaderboard.gameObject.SetActive(true);
            // Set to false
            characterModificationsPanelActive = false;
        }
    }
}
