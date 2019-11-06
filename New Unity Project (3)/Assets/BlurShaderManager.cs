using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurShaderManager : MonoBehaviour
{
    float currentTime = 0f;
    float timeToMove = 2f;
    float lerpedShaderSpacingValue;
    float currentShaderSpacingValue;
    float shaderSpacingValueFadeOutValue;
    float shaderSpacingValueFadeInValue;

    bool moving;
    bool blurIn;
    bool blurOut;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize
        moving = false;
        blurIn = true;
        blurOut = true;
        shaderSpacingValueFadeOutValue = 0f;
        shaderSpacingValueFadeInValue = 10f;

        ActivateBlurOutAnimation();
    }

    void Update()
    {
        LerpBlurInAnimation();
        LerpBlurOutAnimation();
    }

    // Activate the animation
    public void ActivateBlurInAnimation()
    {
        blurIn = true;
        moving = true;
    }

    // Activate the animation
    public void ActivateBlurOutAnimation()
    {
        blurOut = true;
        moving = true;
    }

    // Blur the shader in
    private void LerpBlurInAnimation()
    {
        if (moving == true && blurIn == true)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                currentShaderSpacingValue = Mathf.Lerp(shaderSpacingValueFadeOutValue, shaderSpacingValueFadeInValue, currentTime / timeToMove);
                material.SetFloat("_Size", currentShaderSpacingValue);
            }
            else
            {
                currentTime = 0f;
                moving = false;
                blurIn = true;
            }
        }
    }

    // Blur the shader out
    private void LerpBlurOutAnimation()
    {
        if (moving == true && blurOut == true)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                currentShaderSpacingValue = Mathf.Lerp(shaderSpacingValueFadeOutValue, shaderSpacingValueFadeInValue, currentTime / timeToMove);
                material.SetFloat("_Size", currentShaderSpacingValue);
            }
            else
            {
                currentTime = 0f;
                moving = false;
                blurOut = false;
            }
        }
    }
}
