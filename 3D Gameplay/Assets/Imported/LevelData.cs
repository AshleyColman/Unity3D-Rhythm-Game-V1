using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    // Script creates sample square data for start times of a list of squares
    public GameObject[] squares; // The game object that will get spawned
    public GameObject[] closingSquares; // The game object that will also get spawned
    public Transform squarePosition; // The spawn position for the squares
    public Transform closingSquarePosition; // The spawn position for the closingSquares
    public int squareID; // The square ID for spawning the right square

    public float[] squareStartTime; // The time to spawn the squares
    private int assignTime; 
    public 

	// Use this for initialization
	void Start () {

        assignTime = 0;

        // Level data for squares, loops through squares in the array and assigns a start time to each
        for (int iCount = 0; iCount > squares.Length; iCount++)
        {
            squareStartTime[iCount] = assignTime + 1;
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    void Spawn()
    {
        // If the timer is at the current squaresID time to spawn...

          //  Instantiate(squares[squareID], squarePosition.position, squarePosition.rotation);
        
    }
}
