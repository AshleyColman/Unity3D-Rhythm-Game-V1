using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    public float fadeSpeed = 1f;
    private Material Material;
    // Value used to know when spawned
    private float spawnTime;

    // Use this for initialization
    void Start()
    {
        // Retrieve all the materials attached to the renderer
        Material = GetComponent<Renderer>().material;
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the alpha according to the current time and the time the object has spawned
        SetAlpha((Time.time - spawnTime) * fadeSpeed);
    }

    void SetAlpha(float alpha)
    {

            Color color = Material.color;
            color.a = Mathf.Clamp(alpha, 0, 1);
            Material.color = color;

    }

}
