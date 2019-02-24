using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongData : MonoBehaviour {

    private float BPM = 178;
    private float BPS = 0;
    private string SongTitle = "Pure Ruby";
    private float offset = 0;

    public float timer;
    public GameObject[] hitObject = new GameObject[7];
    public float spawnTime;
    private int hitObjectType;
    private string hitObjectTag;

    private Vector3[] Positions = new Vector3[20];
    private int increment = 0;

    // Use this for initialization
    void Start () {

        // BPS
        BPS = (60/BPM);
    

        // Get beats per second
        // Find out spawn time of each
        //60 bpm = 1 beat per second = 1 second spawn
        //120bpm = 2 beat per second = 0.5 second spawn
        //
        offset = 0.630f;

        

        // Positions
        Positions[0] = new Vector3(0, 0, 0);
        Positions[1] = new Vector3(50, 0, 0);
        Positions[2] = new Vector3(100, 0, 0);
        Positions[3] = new Vector3(150, 0, 0);
        Positions[4] = new Vector3(200, 0, 0);
        Positions[5] = new Vector3(250, 0, 0);
        Positions[6] = new Vector3(0, 0, 0);
        Positions[7] = new Vector3(0, 0, 50);
        Positions[8] = new Vector3(0, 0, 100);
        Positions[9] = new Vector3(0, 0, 150);
        Positions[10] = new Vector3(0, 0, 200);
        Positions[11] = new Vector3(0, 0, 250);
        Positions[12] = new Vector3(0, 0, 0);
        Positions[13] = new Vector3(50, 0, 0);
        Positions[14] = new Vector3(100, 0, 0);
        Positions[15] = new Vector3(150, 0, 0);
        Positions[16] = new Vector3(200, 0, 0);
        Positions[17] = new Vector3(0, 0, 0);
        Positions[18] = new Vector3(0, 0, 50);
        Positions[19] = new Vector3(0, 0, 100);

        spawnTime = BPS;
        hitObjectType = 0;

    }
	
	// Update is called once per frame
	void Update () {

        // Increment timer
        timer += Time.deltaTime;

        // Spawn a note every quarter note
        if (timer >= spawnTime)
        {
            increment += 1;
            SpawnHitObject(Positions[increment], hitObjectType);
            // Increment color and type of hitobject
            // hitObjectType += 1;

            timer = 0;
        }
    }

    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0));
    }
}
