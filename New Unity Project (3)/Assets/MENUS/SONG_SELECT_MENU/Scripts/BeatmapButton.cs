using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatmapButton : MonoBehaviour
{
    #region Variables

    // Button
    public Button beatmapButton;

    // Image
    public Image beatmapImage;

    // Text
    public TextMeshProUGUI easyDifficultyLevelText, normalDifficultyLevelText, hardDifficultyLevelText, songNameText, artistText,
        beatmapCreatorText, newText;

    // Integers
    private int easyDifficultyButtonIndex, normalDifficultyButtonIndex, hardDifficultyButtonIndex, allDifficultyButtonIndex, defaultButtonIndex,
        searchedBeatmapButtonIndex, beatmapButtonIndex;

    // Bools
    private bool hasEasyDifficulty, hasNormalDifficulty, hasHardDifficulty;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool HasEasyDifficulty
    {
        get { return hasEasyDifficulty; }
        set { hasEasyDifficulty = value; }
    }

    public bool HasNormalDifficulty
    {
        get { return hasNormalDifficulty; }
        set { hasNormalDifficulty = value; }
    }

    public bool HasHardDifficulty
    {
        get { return hasHardDifficulty; }
        set { hasHardDifficulty = value; }
    }

    public int BeatmapButtonIndex
    {
        get { return beatmapButtonIndex; }
        set { beatmapButtonIndex = value; }
    }

    public int DefaultButtonIndex
    {
        set { defaultButtonIndex = value; }
    }

    public int EasyDifficultyButtonIndex
    {
        set { easyDifficultyButtonIndex = value; }
    }

    public int NormalDifficultyButtonIndex
    {
        set { normalDifficultyButtonIndex = value; }
    }

    public int HardDifficultyButtonIndex
    {
        set { hardDifficultyButtonIndex = value; }
    }

    public int AllDifficultyButtonIndex
    {
        set { allDifficultyButtonIndex = value; }
    }

    public int SearchedBeatmapButtonIndex
    {
        set { searchedBeatmapButtonIndex = value; }
    }
    #endregion

    #region Functions
    // Use this for initialization
    void Start()
    {
        // Initialize
        ReferenceScriptManager();
    }

    // Reference the scriptmanager
    private void ReferenceScriptManager()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }
    }

    // Keep this beatmap button selected by deactivating it
    public void KeepBeatmapButtonSelected()
    {
        // Reference the scriptmanager
        ReferenceScriptManager();

        // Unselect last selected button
        scriptManager.songSelectPanel.UnselectLastSelectedBeatmapButton();

        // Update reference to the last selected beatmap button to reactivate when another button is pressed
        scriptManager.songSelectPanel.UpdateLastSelectedBeatmapButton(beatmapButton);
    }

    // Load the beatmap assigned to the button when clicked
    public void LoadBeatmap()
    {
        if (scriptManager.songSelectMenuFlash == null)
        {
            scriptManager.songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        }

        scriptManager.songSelectMenuFlash.LoadBeatmapButtonSong(beatmapButtonIndex);


        // Get the list to sort the index by based on the current sorting
        switch (scriptManager.songSelectPanel.CurrentDifficultySorting)
        {
            case "default":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (defaultButtonIndex + 1) + " / " +
                     scriptManager.songSelectManager.beatmapDirectories.Length + " ]";
                break;
            case "easy":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (easyDifficultyButtonIndex + 1) + " / " +
                     scriptManager.songSelectPanel.easyDifficultySortedBeatmapButtonList.Count + " ]";
                break;
            case "normal":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (normalDifficultyButtonIndex + 1) + " / " +
                     scriptManager.songSelectPanel.normalDifficultySortedBeatmapButtonList.Count + " ]";
                break;
            case "hard":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (hardDifficultyButtonIndex + 1) + " / " +
                     scriptManager.songSelectPanel.hardDifficultySortedBeatmapButtonList.Count + " ]";
                break;
            case "all":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (allDifficultyButtonIndex + 1) + " / " +
                     scriptManager.songSelectPanel.allDifficultySortedBeatmapButtonList.Count + " ]";
                break;
            case "searched":
                scriptManager.songSelectPanel.selectedBeatmapCountText.text = "[ " + (searchedBeatmapButtonIndex + 1) + " / " +
                     scriptManager.songSelectPanel.searchedBeatmapsList.Count + " ]";
                break;
        }
    }

    // Play the song preview when clicked
    private void PlaySongPreview()
    {
        scriptManager.songSelectPreview.PlaySongPreview();
    }

    // Set the beatmap butotn index during instantiation
    public void SetBeatmapButtonIndex(int _beatmapButtonIndex)
    {
        beatmapButtonIndex = _beatmapButtonIndex;
    }

    // Stop all coroutines in the beatmap ranking script
    public void StopBeatmapRankingCoroutines()
    {
        // Stop beatmap leaderboard ranking loads
        scriptManager.beatmapRanking.StopAllCoroutines();
    }
    #endregion
}
