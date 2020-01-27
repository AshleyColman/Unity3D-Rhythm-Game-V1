using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FeverTimeManager : MonoBehaviour
{
    public TextMeshProUGUI feverTimeValueText;
    public Slider feverTimeSlider, sideFeverTimeSlider;
    private const float feverTimeValue25 = 0.25f, feverTimeValue50 = 0.5f, feverTimeValue75 = 0.75f, feverTimeValue100 = 1.0f;
    private const float perNoteFillAmount = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        feverTimeSlider.value = 0.5f;
        sideFeverTimeSlider.value = 0.5f;
        UpdateFeverText();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DecreaseFeverSlider();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ResetFeverSlider();
        }
    }

    public void FillFeverSlider()
    {
        if ((feverTimeSlider.value += perNoteFillAmount) <= 1f)
        {
            feverTimeSlider.value += perNoteFillAmount;
            sideFeverTimeSlider.value = feverTimeSlider.value;
            UpdateFeverText();
        }
    }

    public void DecreaseFeverSlider()
    {
        feverTimeSlider.value -= perNoteFillAmount;
        sideFeverTimeSlider.value = feverTimeSlider.value;
        UpdateFeverText();
    }

    public void ResetFeverSlider()
    {
        feverTimeSlider.value = 0f;
        sideFeverTimeSlider.value = feverTimeSlider.value;
        UpdateFeverText();
    }

    public void UpdateFeverText()
    {
        feverTimeValueText.text = (feverTimeSlider.value * 100).ToString("F2") + "%" + '\n' + "FEVER";
    }
}
