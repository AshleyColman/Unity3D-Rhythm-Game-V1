using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public BackgroundManager backgroundManager;
    public BeatmapRanking beatmapRanking; 
    public SongSelectPanel songSelectPanel;
    public SongSelectPreview songSelectPreview;
    public MainMenu mainMenu;
    public SongSelectMenuFlash songSelectMenuFlash;
    public MessagePanel messagePanel;
    public TipsNewsScroll tipsNewsScroll;
    public HitSoundDatabase hitSoundDatabase;
    public LoadAndRunBeatmap loadAndRunBeatmap;
    public MenuManager menuManager;
    public MetronomeForEffects metronomeForEffects;
    public PlayerProfile playerProfile;
    public PlayerSkillsManager playerSkillsManager;
    public RhythmVisualizatorPro rhythmVisualizatorPro;
    public SongProgressBar songProgressBar;
    public SongSelectManager songSelectManager;
    public UploadPlayerImage uploadPlayerImage;
    public SongDatabase songDatabase;
    public LoadLastBeatmapManager loadLastBeatmapManager;
    public BlurShaderManager blurShaderManager;
    public UIColorManager uiColorManager;

    // Start is called before the first frame update
    void Start()
    {
        /*
    backgroundManager = FindObjectOfType<BackgroundManager>();
    beatmapRanking = FindObjectOfType<BeatmapRanking>();
    songSelectPanel = FindObjectOfType<SongSelectPanel>();
    songSelectPreview = FindObjectOfType<SongSelectPreview>();
    mainMenu = FindObjectOfType<MainMenu>();
    songSelectMenuFlash = FindObjectOfType<SongSelectMenuFlash>();
    messagePanel = FindObjectOfType<MessagePanel>();
    tipsNewsScroll = FindObjectOfType<TipsNewsScroll>();
    hitSoundDatabase = FindObjectOfType<HitSoundDatabase>();
    loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>();
    metronomeForEffects = FindObjectOfType<MetronomeForEffects>();
    playerProfile = FindObjectOfType<PlayerProfile>();
    playerSkillsManager = FindObjectOfType<PlayerSkillsManager>();
    rhythmVisualizatorPro = FindObjectOfType<RhythmVisualizatorPro>();
    songProgressBar = FindObjectOfType<SongProgressBar>();
    songSelectManager = FindObjectOfType<SongSelectManager>();
    uploadPlayerImage = FindObjectOfType<UploadPlayerImage>();
    */
    }
}
