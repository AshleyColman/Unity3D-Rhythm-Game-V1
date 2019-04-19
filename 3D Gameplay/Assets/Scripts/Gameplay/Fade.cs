using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    // The fade speed
    public float fadeSpeed;
    // The material to fade
    private Material Material;

    // Value used to know when spawned
    private float spawnTime;
    public float timer;

    // Reference to the player skills manager to get the fade speed selected from the song select scene
    private PlayerSkillsManager playerSkillsManager;
    // The fade speed selected from the song select screen, required for doing fade calculations
    private float fadeSpeedSelected;

    // Reference to the level changer script
    private LevelChanger levelChanger;

    // Reference to the PlacedObject script
    private PlacedObject placedObject;

    // Used to control whether to fade or pause the fade at the current fade value
    private bool pauseFadeTransition;

    // Use this for initialization
    void Start()
    {
        // Retrieve all the materials attached to the renderer
        Material = GetComponent<Renderer>().material;
        spawnTime = 0f;
        timer = 0f;

        // Get the reference to the level changer
        levelChanger = FindObjectOfType<LevelChanger>();

        // Do a check on the level changer index, if in gameplay get the fade speed selected otherwise set the fade speed to default of normal
        if (levelChanger.currentLevelIndex == 4)
        {
            playerSkillsManager = FindObjectOfType<PlayerSkillsManager>(); // Get the reference for scale speed
            fadeSpeedSelected = playerSkillsManager.GetFadeSpeedSelected(); // Get the fade speed selected such as 2 for slow, 1 for normal and 0.5f for fast
        }
        else
        {
            // Set fade speed to default "normal"
            fadeSpeedSelected = 1f;
        }

        // Get the PlacedObject script in the scene if in the Editor scene
        if (levelChanger.currentLevelIndex == 2)
        {
            placedObject = FindObjectOfType<PlacedObject>();
        }

        // Set the fade speed for this hit object
        SetFadeSpeed();
    }

    // Update is called once per frame
    void Update()
    {

        // If in the Editor scene check if the preview beatmap is false
        if (levelChanger.currentLevelIndex == 2)
        {
            // If the beatmap preview is paused and no longer running
            if (placedObject.playBeatmapPreview == false)
            {
                // Pause the fade at the current value
                pauseFadeTransition = true;
            }
            else
            {
                pauseFadeTransition = false;
            }

            if (pauseFadeTransition == true)
            {
                // Do not continue fading and keep the fade at the current value
            }
            else
            {
                // Increment timer
                timer += Time.deltaTime;

                // Allow the preview hit object to continue to fade
                SetAlpha((timer - spawnTime) * fadeSpeed);
            }
        }



        // If on the gameplay scene fade when the note appears
        if (levelChanger.currentLevelIndex == 4)
        {
            // Increment timer
            timer += Time.deltaTime;

            // Set the alpha according to the current time and the time the object has spawned
            SetAlpha((timer - spawnTime) * fadeSpeed);
        }


    }

    // Fade in
    void SetAlpha(float alpha)
    {
            Color color = Material.color;
            color.a = Mathf.Clamp(alpha, 0, 1);
            Material.color = color;
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
