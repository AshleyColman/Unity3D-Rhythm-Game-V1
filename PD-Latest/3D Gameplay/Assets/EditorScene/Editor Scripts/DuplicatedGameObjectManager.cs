using UnityEngine;

public class DuplicatedGameObjectManager : MonoBehaviour {

    private GameObject[] hitSoundDatabaseArray;
    private GameObject[] songDatabaseArray;

    private void Start()
    {
        // Initialize
        hitSoundDatabaseArray = GameObject.FindGameObjectsWithTag("HitSoundDatabase"); // Get all hit sound database objects and put in array 
        songDatabaseArray = GameObject.FindGameObjectsWithTag("SongDatabase"); // Get all song database objects and put in array 

        // Functions
        CheckSongDatabaseArray(); // Check the length of the song database array
        CheckHitSoundDatabaseArray(); // Check the length of the hit sound database array
    }

    // Check the length of the song database array
    private void CheckSongDatabaseArray()
    {
        // If there is more than 1 song database in the scene destroy the latest one so there is only one
        if (songDatabaseArray.Length > 1)
        {
            // Destroy the duplicate
            Destroy(songDatabaseArray[1].gameObject);
        }
    }

    // Check the length of the hit sound database array
    private void CheckHitSoundDatabaseArray()
    {
        // If there is more than 1 hit sound database in the scene destroy the latest one so there is only one
        if (hitSoundDatabaseArray.Length > 1)
        {
            // Destroy the duplicate
            Destroy(hitSoundDatabaseArray[1].gameObject);
        }
    }
}
