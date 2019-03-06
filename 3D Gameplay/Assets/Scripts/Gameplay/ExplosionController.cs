using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public GameObject[] explosion = new GameObject[7]; // Particle system objects


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    // Spawn explosion
    public void SpawnExplosion(Vector3 positionPass, string objectTagPass)
    {
        // Spawn blue explosion for blue notes
        if (objectTagPass == "Blue")
        {
            Instantiate(explosion[0], positionPass, Quaternion.Euler(90, 0, -45)); // Instantiate blue particle system
        }
        // Spawn blue explosion for green notes
        else if (objectTagPass == "Green")
        {
            Instantiate(explosion[1], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for orange notes
        else if (objectTagPass == "Orange")
        {
            Instantiate(explosion[2], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for pink notes
        else if (objectTagPass == "Pink")
        {
            Instantiate(explosion[3], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for purple notes
        else if (objectTagPass == "Purple")
        {
            Instantiate(explosion[4], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for red notes
        else if (objectTagPass == "Red")
        {
            Instantiate(explosion[5], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for yellow notes
        else if (objectTagPass == "Yellow")
        {
            Instantiate(explosion[6], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for grey notes
        else if (objectTagPass == "Grey")
        {
            Instantiate(explosion[7], positionPass, Quaternion.Euler(90, 0, -45));
        }
        // Spawn blue explosion for grey notes
        else if (objectTagPass == "Miss")
        {
            Instantiate(explosion[8], positionPass, Quaternion.Euler(90, 0, -45));
        }

    }


}
