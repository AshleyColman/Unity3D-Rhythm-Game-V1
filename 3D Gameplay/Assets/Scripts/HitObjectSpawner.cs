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

    public List<GameObject> spawnedList = new List<GameObject>();
    private int earliestIndex; // The current earliest note
    private bool hasHit;
    private bool startCheck;
    private int sizeOfList;
    private int nextIndex;

	// Use this for initialization
	void Start () {
        timer = 0;
        spawnTime = 1.5f;
        positionX = -350f;
        hitObjectType = 0;
        earliestIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;
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
            //hitObjectType += 1;

            timer = 0;
        }






        // Set earliest index object to earliest is true
        // Check if earliest has been hit
        // If it has been hit
        // Check if another object is in the list to move up
        // Assign is earliest to next object

        // Size of list
        sizeOfList = spawnedList.Count;
        // Next index required to increment for check
        nextIndex = earliestIndex + 2;

        if (startCheck == true)
        {
            // If the index object exists
            if (spawnedList[earliestIndex] != null)
            {
                spawnedList[earliestIndex].GetComponent<TimingAndScore>().isEarliest = true;
                Debug.Log("checking");
                Debug.Log("sizeOfList: " + sizeOfList);
                Debug.Log("nextIndex: " + nextIndex);
                            
            }

            if (spawnedList[earliestIndex] == null && sizeOfList == nextIndex)
            {
                earliestIndex++;
                Debug.Log("is null and incremented");
                Debug.Log("sizeOfList: " + sizeOfList);
                Debug.Log("nextIndex: " + nextIndex);
            }

        }
        

    }

    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
    }



}
