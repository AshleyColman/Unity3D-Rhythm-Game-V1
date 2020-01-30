using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSFXManager : MonoBehaviour
{
    public AudioClip[] soundEffectArray = new AudioClip[1];
    public AudioSource soundEffectAudioSource;
    private float soundEffectVolume;

    private void Start()
    {
        soundEffectVolume = 1f;
    }

    public void PlaySoundEffect(int _index)
    {
        soundEffectAudioSource.PlayOneShot(soundEffectArray[_index], soundEffectVolume);
    }
}
