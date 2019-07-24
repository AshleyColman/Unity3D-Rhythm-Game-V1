using UnityEngine;

public class SongDatabase : MonoBehaviour {

    // The list of songs used in the editor, song select and gameplay
    public AudioClip[] songClip;

    // Don't destroy the object
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
