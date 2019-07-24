using UnityEngine;

public class HitSoundDatabase : MonoBehaviour {

    // The list of hitsounds that are played during gameplay and selected during song select menu
    public AudioClip[] hitSoundClip;
    public AudioClip missSoundClip;

    // Don't destroy the object
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
