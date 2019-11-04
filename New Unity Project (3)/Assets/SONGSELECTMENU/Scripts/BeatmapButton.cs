using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatmapButton : MonoBehaviour
{
    private int beatmapButtonIndex;

    private int easyDifficultyButtonIndex, advancedDifficultyButtonIndex, extraDifficultyButtonIndex, allDifficultyButtonIndex, defaultButtonIndex,
        searchedBeatmapButtonIndex;

    private bool hasEasyDifficulty, hasAdvancedDifficulty, hasExtraDifficulty;


    public Image overlayColorImage;
    public GameObject easyDifficultyImage, advancedDifficultyImage, extraDifficultyImage;
    public TextMeshProUGUI easyDifficultyLevelText, advancedDifficultyLevelText, extraDifficultyLevelText;
    public TextMeshProUGUI songNameText, artistText, beatmapCreatorText;

    private SongSelectMenuFlash songSelectMenuFlash;
    private SongSelectManager songSelectManager;
    private BeatmapRanking beatmapRanking;
    private EditSelectSceneSongSelectManager editSelectSceneSongSelectManager;
    private SongSelectPanel songSelectPanel;

    // Properties
    public bool HasEasyDifficulty
    {
        get { return hasEasyDifficulty; }
        set { hasEasyDifficulty = value; }
    }

    public bool HasAdvancedDifficulty
    {
        get { return hasAdvancedDifficulty; }
        set { hasAdvancedDifficulty = value; }
    }

    public bool HasExtraDifficulty
    {
        get { return hasExtraDifficulty; }
        set { hasExtraDifficulty = value; }
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

    public int AdvancedDifficultyButtonIndex
    {
        set { advancedDifficultyButtonIndex = value; }
    }

    public int ExtraDifficultyButtonIndex
    {
        set { extraDifficultyButtonIndex = value; }
    }

    public int AllDifficultyButtonIndex
    {
        set { allDifficultyButtonIndex = value; }
    }

    public int SearchedBeatmapButtonIndex
    {
        set { searchedBeatmapButtonIndex = value; }
    }

    // Use this for initialization
    void Start()
    {
        songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        songSelectManager = FindObjectOfType<SongSelectManager>();
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
        editSelectSceneSongSelectManager = FindObjectOfType<EditSelectSceneSongSelectManager>();
        songSelectPanel = FindObjectOfType<SongSelectPanel>();
    }

    // Keep button selected
    public void KeepButtonSelected()
    {
        this.gameObject.GetComponent<Button>().Select();
    }

    // Load the beatmap assigned to the button when clicked
    public void LoadBeatmap()
    {
        if (songSelectMenuFlash == null)
        {
            songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        }

        songSelectMenuFlash.LoadBeatmapButtonSong(beatmapButtonIndex);



        // Get the list to sort the index by based on the current sorting
        switch (songSelectPanel.CurrentDifficultySorting)
        {
            case "default":
                songSelectPanel.selectedBeatmapCountText.text = (defaultButtonIndex + 1).ToString();
                break;
            case "easy":
                songSelectPanel.selectedBeatmapCountText.text = (easyDifficultyButtonIndex + 1).ToString();
                break;
            case "advanced":
                songSelectPanel.selectedBeatmapCountText.text = (advancedDifficultyButtonIndex + 1).ToString();
                break;
            case "extra":
                songSelectPanel.selectedBeatmapCountText.text = (extraDifficultyButtonIndex + 1).ToString();
                break;
            case "all":
                songSelectPanel.selectedBeatmapCountText.text = (allDifficultyButtonIndex + 1).ToString();
                break;
            case "searched":
                songSelectPanel.selectedBeatmapCountText.text = (searchedBeatmapButtonIndex + 1).ToString();
                break;
        }
    }

    // Load the beatmap assigned to the button when clicked
    public void LoadEditSelectSceneBeatmap()
    {
        if (songSelectMenuFlash == null)
        {
            songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        }

        songSelectMenuFlash.LoadEditSelectSceneBeatmapButtonSong(beatmapButtonIndex);

        PlayEditSelectSceneSongPreview();
    }

    // Play the song preview when clicked
    private void PlaySongPreview()
    {
        songSelectManager.PlaySongPreview();
    }

    // Play the song preview when clicked
    private void PlayEditSelectSceneSongPreview()
    {
        editSelectSceneSongSelectManager.PlaySongPreview();
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
        beatmapRanking.StopAllCoroutines();
    }

}
