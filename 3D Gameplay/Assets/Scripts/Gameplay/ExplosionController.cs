using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public GameObject blueHitExplosion, purpleHitExplosion, redHitExplosion, greenHitExplosion, yellowHitExplosion, orangeHitExplosion; // Hit explosions
    public GameObject blueMissExplosion, purpleMissExplosion, redMissExplosion, greenMissExplosion, yellowMissExplosion, orangeMissExplosion; // Hit explosions
    public GameObject specialHitExplosion; // Special hit explosion

    public LoadAndRunBeatmap loadAndRunBeatmap; // Reference required to check for special time explosions

    // Use this for initialization
    void Start () {
        loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>(); // Set the reference 
	}

    // Spawn hit explosion
    public void SpawnHitExplosion(Vector3 positionPass, string objectTagPass)
    {
        // OBJECTS HIT EXPLOSIONS

        // Check if special time, if it is spawn a special explosion, if not check the object tag and spawn the correct colored explosion for normal objects
        if (loadAndRunBeatmap.isSpecialTime == true)
        {
            // instantiate special hit explosion
            Instantiate(specialHitExplosion, positionPass, Quaternion.Euler(90, 0, -45)); // Spawn special miss explosion for special miss notes
        }
        else
        {
            // Spawn correct colored explosion based on the hit object color hit
            switch (objectTagPass)
            {
                case "Blue":
                    Instantiate(blueHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
                case "Purple":
                    Instantiate(purpleHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
                case "Red":
                    Instantiate(redHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
                case "Green":
                    Instantiate(greenHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
                case "Orange":
                    Instantiate(orangeHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
                case "Yellow":
                    Instantiate(yellowHitExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                    break;
            }
        }
    }

    // Spawn miss explosion
    public void SpawnMissExplosion(Vector3 positionPass, string objectTagPass)
    {
        // OBJECTS MISS EXPLOSIONS

        // Spawn correct colored explosion based on the hit object color hit
        switch (objectTagPass)
        {
            case "Blue":
                Instantiate(blueMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
            case "Purple":
                Instantiate(purpleMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
            case "Red":
                Instantiate(redMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
            case "Green":
                Instantiate(greenMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
            case "Orange":
                Instantiate(orangeMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
            case "Yellow":
                Instantiate(yellowMissExplosion, positionPass, Quaternion.Euler(90, 0, -45));
                break;
        }
    }
}




