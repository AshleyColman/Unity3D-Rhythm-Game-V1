using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    private float fadeSpeed = 1f;
    private Material Material;
    // Value used to know when spawned
    private float maxOpacity;
    public float timer;

    // Use this for initialization
    void Start()
    {
        // Retrieve all the materials attached to the renderer
        Material = GetComponent<Renderer>().material;
        maxOpacity = 1f;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Set the alpha according to the current time and the time the object has spawned
        SetAlpha((maxOpacity - timer) * fadeSpeed);
    }

    // Fade in
    void SetAlpha(float alpha)
    {
        Color color = Material.color;
        color.a = Mathf.Clamp(alpha, 0, 1);
        Material.color = color;
    }
}
