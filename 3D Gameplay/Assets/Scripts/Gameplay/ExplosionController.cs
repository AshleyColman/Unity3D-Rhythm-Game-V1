using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public GameObject[] explosion = new GameObject[10]; // Particle system objects
    public LoadAndRunBeatmap loadAndRunBeatmap; // Reference required to check for special time explosions

    // Use this for initialization
    void Start () {
        loadAndRunBeatmap = FindObjectOfType<LoadAndRunBeatmap>(); // Set the reference 
	}
	
	// Update is called once per frame
	void Update () {
	}

    // Spawn explosion
    public void SpawnExplosion(Vector3 positionPass, string objectTagPass)
    {
        // Check if special time, if it is spawn a special explosion, if not check the object tag and spawn the correct colored explosion for normal objects
        if (loadAndRunBeatmap.isSpecialTime == true)
        {
            if (objectTagPass == "Miss")
            {
                Instantiate(explosion[10], positionPass, Quaternion.Euler(90, 0, -45)); // Spawn special miss explosion for special miss notes
            }
            else
            {
                Instantiate(explosion[9], positionPass, Quaternion.Euler(90, 0, -45)); // Instantiate special explosion for special notes
            }
        }
        else
        {
            // Spawn blue explosion for blue notes
            if (objectTagPass == "Blue")
            {
                Instantiate(explosion[0], positionPass, Quaternion.Euler(90, 0, -45)); // Instantiate blue particle system
            }
            // Spawn green explosion for green notes
            else if (objectTagPass == "Green")
            {
                Instantiate(explosion[1], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn orange explosion for orange notes
            else if (objectTagPass == "Orange")
            {
                Instantiate(explosion[2], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn pink explosion for pink notes
            else if (objectTagPass == "Pink")
            {
                Instantiate(explosion[3], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn purple explosion for purple notes
            else if (objectTagPass == "Purple")
            {
                Instantiate(explosion[4], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn red explosion for red notes
            else if (objectTagPass == "Red")
            {
                Instantiate(explosion[5], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn yellow explosion for yellow notes
            else if (objectTagPass == "Yellow")
            {
                Instantiate(explosion[6], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn grey explosion for grey notes
            else if (objectTagPass == "Grey")
            {
                Instantiate(explosion[7], positionPass, Quaternion.Euler(90, 0, -45));
            }
            // Spawn miss explosion for grey notes
            else if (objectTagPass == "Miss")
            {
                Instantiate(explosion[8], positionPass, Quaternion.Euler(90, 0, -45));
            }
        }
    }


}
