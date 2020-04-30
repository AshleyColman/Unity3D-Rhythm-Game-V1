using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExplosionDeactivate : MonoBehaviour
{
    #region Variables
    // UI
    public Image colorImage;

    // Particle
    public ParticleSystem particleExplosion;
    private ParticleSystem.MainModule particleExplosionMain;

    // Color
    private Color color;

    // Integers
    private float timer;
    #endregion

    #region Properties
    // Properties
    public Color Color
    {
        set { color = value; }
    }
    #endregion

    #region Functions
    // Reset the timer when reactivated
    private void OnEnable()
    {
        timer = 0f;
        UpdateImageColor();
        UpdateExplosionColor();
    }

    private void Awake()
    {
        if (particleExplosion != null)
        {
            particleExplosionMain = particleExplosion.main;
        }
    }

    private void Start()
    {
        // Initialize
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= Constants.EXPLOSION_DEACTIVATE_TIME)
        {
            Deactivate();
        }
    }

    // Update color image color
    private void UpdateImageColor()
    {
        colorImage.color = color;
    }

    // Update explosion color
    private void UpdateExplosionColor()
    {
        switch (tag)
        {
            case Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG:
                particleExplosionMain.startColor = new ParticleSystem.MinMaxGradient(color, Color.white);
                break;
            case Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG:
                particleExplosionMain.startColor = new ParticleSystem.MinMaxGradient(color, Color.white);
                break;
        }
    }

    // Deactivate
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
