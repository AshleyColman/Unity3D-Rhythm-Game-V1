using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public GameObject explosion; // Particle system object

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Spawn explosion
    public void SpawnExplosion(Vector3 positionPass)
    {
        Instantiate(explosion, positionPass, Quaternion.Euler(90, 0, -45)); // Instantiate particle system
    }


}
