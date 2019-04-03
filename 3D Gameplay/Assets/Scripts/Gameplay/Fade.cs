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

    // Use this for initialization
    void Start()
    {
        // Retrieve all the materials attached to the renderer
        Material = GetComponent<Renderer>().material;
        spawnTime = 0f;
        timer = 0f;

        playerSkillsManager = FindObjectOfType<PlayerSkillsManager>(); // Get the reference for scale speed
        fadeSpeedSelected = playerSkillsManager.GetFadeSpeedSelected(); // Get the fade speed selected such as 2 for slow, 1 for normal and 0.5f for fast

        // Set the fade speed for this hit object
        SetFadeSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Set the alpha according to the current time and the time the object has spawned
        SetAlpha((timer - spawnTime) * fadeSpeed);
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
