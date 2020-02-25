using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FeverTimeManager : MonoBehaviour
{
    public Slider feverTimeSlider;
    private const float feverTimeValue25 = 0.25f, feverTimeValue50 = 0.5f, feverTimeValue75 = 0.75f, feverTimeValue100 = 1.0f;
    private const float perNoteFillAmount = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        feverTimeSlider.value = 0f;
    }

    public void FillFeverSlider()
    {
        if ((feverTimeSlider.value += perNoteFillAmount) <= 1f)
        {
            feverTimeSlider.value += perNoteFillAmount;
        }
    }

    public void DecreaseFeverSlider()
    {
        feverTimeSlider.value -= perNoteFillAmount;
    }

    public void ResetFeverSlider()
    {
        feverTimeSlider.value = 0f;
    }
}
