using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongDifficultyButtons : MonoBehaviour {

    public ParticleSystem advancedButtonGlow;
    public ParticleSystem easyButtonGlow;
    public ParticleSystem extraButtonGlow;

    public void EnableAdvancedButtonGlow()
    {
        advancedButtonGlow.gameObject.SetActive(true);
    }

    public void EnableEasyButtonGlow()
    {
        easyButtonGlow.gameObject.SetActive(true);
    }

    public void EnableExtradButtonGlow()
    {
        extraButtonGlow.gameObject.SetActive(true);
    }

    public void DisableAdvancedButtonGlow()
    {
        advancedButtonGlow.gameObject.SetActive(false);
    }

    public void DisableEasyButtonGlow()
    {
        easyButtonGlow.gameObject.SetActive(false);
    }

    public void DisableExtraButtonGlow()
    {
        extraButtonGlow.gameObject.SetActive(false);
    }
}
