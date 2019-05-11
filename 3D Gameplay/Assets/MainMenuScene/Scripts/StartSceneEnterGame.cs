using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneEnterGame : MonoBehaviour {

    public GameObject MainMenuCanvas;
    public GameObject StartMenuCanvas;
    public AudioSource MenuSFXAudioSource;
    public AudioClip MenuSFXMenuSourceClip;
    bool hasPressedEnter;
    public Animator StartMenuCanvasAnimator;
    private MetronomeForEffects metronomeForEffects;
    // Use this for initialization
    void Start () {
        metronomeForEffects = FindObjectOfType<MetronomeForEffects>();
        hasPressedEnter = false;
        MenuSFXAudioSource.clip = MenuSFXMenuSourceClip;
    }

    void Update()
    {

        // If enter is pressed load the next scene and play sound
        if (Input.GetKey("return") && hasPressedEnter == false || Input.GetMouseButtonDown(0))
        {
            MenuSFXAudioSource.PlayOneShot(MenuSFXMenuSourceClip);
            StartCoroutine(PlayCanvasSwipAnimation());
            hasPressedEnter = true;
        }
    }

    private IEnumerator PlayCanvasSwipAnimation()
    {
        metronomeForEffects.enabled = false;
        StartMenuCanvasAnimator.Play("CanvasSwipAnimation");

        MainMenuCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        StartMenuCanvas.gameObject.SetActive(false);

        metronomeForEffects.enabled = true;
    }
}
