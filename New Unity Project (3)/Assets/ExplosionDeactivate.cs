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
    public TextMeshProUGUI text, shadowText;

    // Color
    private Color color;

    // Integers
    private float timer, deactivateTime; // Explosion deactivation time and timer
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
        UpdateColorImageColor();
    }

    // Use this for initialization
    void Start()
    {
        // Initialize
        deactivateTime = 1f;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= deactivateTime)
        {
            Deactivate();
        }
    }

    // Update color image color
    private void UpdateColorImageColor()
    {
        colorImage.color = color;
    }

    // Deactivate
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    // Update text with value passed (1000, 500, 250, miss)
    public void UpdateText(string _text)
    {
        text.text = _text;
        shadowText.text = _text;
    }
    #endregion
}
