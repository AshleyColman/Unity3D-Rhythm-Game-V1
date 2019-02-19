using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectSpawner : MonoBehaviour {

    public float timer;
    public GameObject hitObject;
    public Vector3 position;
    public float positionX;
    public float spawnTime; 

	// Use this for initialization
	void Start () {
        timer = 0;
        spawnTime = 1.5f;
        positionX = -200f;
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime; // Increment timer


        if (timer >= spawnTime)
        {
            // New position
            positionX += 80;
            position = new Vector3(positionX, 20, 0);
            SpawnHitObject(position);

            timer = 0;
        }
	}


    public void SpawnHitObject(Vector3 positionPass)
    {
        Instantiate(hitObject, positionPass, Quaternion.Euler(0, 45, 0));
    }

}
