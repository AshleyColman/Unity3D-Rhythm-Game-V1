using UnityEngine;

public class Fade : MonoBehaviour {

    // Materials
    public Material material; // The material to fade

    // Integers
    private float fadeSpeedSelected; // The fade speed
    private float spawnTime; // Value used to know when spawned
    private float timer;
    private float fadeSpeed;   // The fade speed

    // Bools
    private bool pauseFadeTransition; // Used to control whether to fade or pause the fade at the current fade value

    // Scripts
    private PlayerSkillsManager playerSkillsManager; // Reference to the player skills manager to get the fade speed selected from the song select scene
    private LevelChanger levelChanger; // Reference to the level changer script
    private PlacedObject placedObject; // Reference to the PlacedObject script
    private FeverTimeManager feverTimeManager;

    Color color; 

    // Initialize every time the object is reactivated
    private void OnEnable()
    {
        spawnTime = 0f;
        timer = 0f;

        if (material == null)
        {
            material = GetComponent<Renderer>().material; // Retrieve all the materials attached to the renderer
        }

        SetAlpha(0);
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        material = GetComponent<Renderer>().material; // Retrieve all the materials attached to the renderer
        spawnTime = 0f;
        timer = 0f;

        // Reference
        levelChanger = FindObjectOfType<LevelChanger>();

        // Do a check on the level changer index, if in gameplay get the fade speed selected otherwise set the fade speed to default of normal
        if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex)
        {
            playerSkillsManager = FindObjectOfType<PlayerSkillsManager>(); // Get the reference for scale speed
            fadeSpeedSelected = playerSkillsManager.GetFadeSpeedSelected(); // Get the fade speed selected

            feverTimeManager = FindObjectOfType<FeverTimeManager>();
        }
        else
        {
            // Set fade speed to default "normal"
            fadeSpeedSelected = 1f;
        }

        // Get the PlacedObject script in the scene if in the Editor scene
        if (levelChanger.CurrentLevelIndex == levelChanger.EditorSceneIndex)
        {
            placedObject = FindObjectOfType<PlacedObject>();
        }

        // Set the fade speed for this hit object
        SetFadeSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        // If on the gameplay scene fade when the note appears
        if (levelChanger.CurrentLevelIndex == levelChanger.GameplaySceneIndex)
        {
            // Check if fever time has been activated or deactivated, change the material on the frame that is changes
            //CheckFeverTimeActivationChanges();

            // Update the material reference
            UpdateMaterialReference();

            // Increment timer
            timer += Time.deltaTime;

            // Set the alpha according to the current time and the time the object has spawned
            SetAlpha((timer - spawnTime) * fadeSpeed);
        }
    }

    /*
    // Check if fever time has been activated or deactivated, change the material on the frame that is changes
    private void CheckFeverTimeActivationChanges()
    {
        // Check if the current fever time is activated, but the previous frame was deactivated
        // If the values aren't the same fever time has been activated/deactivated so we can update the material reference once
        if (feverTimeManager.FeverTimeActivated == true && feverTimeManager.FeverTimeActivatedPrevious == false)
        {
            // Fever time has been activated
            // Update the material reference
            UpdateMaterialReference();
        }
        else if (feverTimeManager.FeverTimeActivated == false && feverTimeManager.FeverTimeActivatedPrevious == true)
        {
            // Fever time has been deactivated
            // Update the material reference
            UpdateMaterialReference();
        }
    }
    */

    // Fever time will change material references - during and after update the material
    public void UpdateMaterialReference()
    {
        // Retrieve all the materials attached to the renderer
        material = GetComponent<Renderer>().material;
    }

    // Fade in
    void SetAlpha(float alpha)
    {
        color = material.color;
        color.a = Mathf.Clamp(alpha, 0, 1);
        material.color = color;
    }

    // Set the fade speed based on the fade speed selected
    private void SetFadeSpeed()
    {
        if (fadeSpeedSelected == 2f)
        {
            // If the slow speed has been selected it will take half the time of a 1 second fade in hit object, so 0.5 speed
            fadeSpeed = 0.5f;
        }
        else if (fadeSpeedSelected == 1f)
        {
            // Take 1 second to fade in for a 1 second hit object
            fadeSpeed = 1f;
        }
        else if (fadeSpeedSelected == 0.5f)
        {
            // If the fast speed has been seleced it will have twice the fade speed as the normal fade speed, so a speed of 2
            fadeSpeed = 2f;
        }
    }
}
