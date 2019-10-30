using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatmapButton : MonoBehaviour
{
    private int beatmapButtonIndex;
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

    // Use this for initialization
    void Start()
    {
        songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        songSelectManager = FindObjectOfType<SongSelectManager>();
        beatmapRanking = FindObjectOfType<BeatmapRanking>();
        editSelectSceneSongSelectManager = FindObjectOfType<EditSelectSceneSongSelectManager>();
        songSelectPanel = FindObjectOfType<SongSelectPanel>();
    }

    // Load the beatmap assigned to the button when clicked
    public void LoadBeatmap()
    {
        if (songSelectMenuFlash == null)
        {
            songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
        }

        songSelectMenuFlash.LoadBeatmapButtonSong(beatmapButtonIndex);

        PlaySongPreview();

        songSelectPanel.selectedBeatmapCountText.text = (beatmapButtonIndex + 1).ToString();
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
    public void SetBeatmapButtonIndex(int beatmapButtonIndexPass)
    {
        beatmapButtonIndex = beatmapButtonIndexPass;
    }

    // Stop all coroutines in the beatmap ranking script
    public void StopBeatmapRankingCoroutines()
    {
        // Stop beatmap leaderboard ranking loads
        beatmapRanking.StopAllCoroutines();
    }

}
