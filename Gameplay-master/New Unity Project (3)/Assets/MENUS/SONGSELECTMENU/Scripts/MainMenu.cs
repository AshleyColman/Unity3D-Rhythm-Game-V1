using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI modeDescriptionText;

    private string quickplayTextValue, editorTextValue, rankingsTextValue, settingsTextValue, discordTextValue, exitTextValue;
    private KeyCode quickplayModeKey, editorModeKey, rankingsModeKey, settingsModeKey, discordModeKey, exitModeKey;

    // Scripts
    ScriptManager scriptManager;

    // Start is called before the first frame update
    void Start()
    {
        quickplayTextValue = "PLAY THE RHYTHM GAME";
        editorTextValue = "EDIT OR CREATE YOUR OWN BEATMAP";
        rankingsTextValue = "SEE GAME RANKINGS";
        settingsTextValue = "CONFIGURE SETTINGS";
        discordTextValue = "JOIN THE DISCORD CHANNEL";
        exitTextValue = "THANKS FOR PLAYING";

        quickplayModeKey = KeyCode.Alpha1;
        editorModeKey = KeyCode.Alpha2;
        rankingsModeKey = KeyCode.Alpha3;
        settingsModeKey = KeyCode.Alpha4;
        discordModeKey = KeyCode.Alpha5;
        exitModeKey = KeyCode.Alpha6;

        modeDescriptionText.text = quickplayTextValue;

        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();


        scriptManager.backgroundManager.img.gameObject.SetActive(false);
        scriptManager.backgroundManager.img2.gameObject.SetActive(false);
        scriptManager.backgroundManager.videoPlayer.gameObject.SetActive(false);
        scriptManager.backgroundManager.videoPlayer.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(quickplayModeKey))
        {
            // Transition to song select menu
            scriptManager.menuManager.MainMenuToSongSelectMenu();
        }

        if (Input.GetKeyDown(editorModeKey))
        {

        }

        if (Input.GetKeyDown(rankingsModeKey))
        {

        }

        if (Input.GetKeyDown(settingsModeKey))
        {

        }

        if (Input.GetKeyDown(discordModeKey))
        {
            OpenDiscord();
        }

        if (Input.GetKeyDown(exitModeKey))
        {
            ExitGame();
        }
    }

    // Open discord the discord URL
    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/zDneB5c");
    }

    // Update the mode description text
    public void UpdateModeDescriptionText(string _buttonSelected)
    {
        // Update based on the button hovered over
        switch (_buttonSelected)
        {
            case "QUICKPLAY":
                modeDescriptionText.text = quickplayTextValue;
                break;
            case "EDITOR":
                modeDescriptionText.text = editorTextValue;
                break;
            case "RANKINGS":
                modeDescriptionText.text = rankingsTextValue;
                break;
            case "SETTINGS":
                modeDescriptionText.text = settingsTextValue;
                break;
            case "DISCORD":
                modeDescriptionText.text = discordTextValue;
                break;
            case "EXIT":
                modeDescriptionText.text = exitTextValue;
                break;
        }
    }

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
