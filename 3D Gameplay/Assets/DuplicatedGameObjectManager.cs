using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicatedGameObjectManager : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        // Get all hit sound database objects and put in array 
        GameObject[] hitSoundDatabaseArray = GameObject.FindGameObjectsWithTag("HitSoundDatabase");

        // If there is more than 1 hit sound database in the scene destroy the latest one so there is only one
        if (hitSoundDatabaseArray.Length > 1)
        {
            Destroy(hitSoundDatabaseArray[1].gameObject);
        }


        // Get all song database objects and put in array 
        GameObject[] songDatabaseArray = GameObject.FindGameObjectsWithTag("SongDatabase");

        // If there is more than 1 song database in the scene destroy the latest one so there is only one
        if (songDatabaseArray.Length > 1)
        {
            Destroy(songDatabaseArray[1].gameObject);
        }
    }
}
