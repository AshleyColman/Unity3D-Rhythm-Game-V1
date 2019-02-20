using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectSpawner : MonoBehaviour {

    public float timer;
    public GameObject[] hitObject = new GameObject[6];
    public Vector3 position;
    public float positionX;
    public float spawnTime;
    private int hitObjectType;
    private string hitObjectTag;
	// Use this for initialization
	void Start () {
        timer = 0;
        spawnTime = 1.5f;
        positionX = -350f;
        hitObjectType = 0;
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime; // Increment timer

        if (timer >= spawnTime)
        {
            // New position
            positionX += 80;
            position = new Vector3(positionX, 20, 0);
            SpawnHitObject(position, hitObjectType);
            // Increment color and type of hitobject
            hitObjectType += 1;

            timer = 0;
        }
	}


    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0));
    }

}
