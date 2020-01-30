using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public PathPlacer pathPlacer;
    public float speed = 5;
    float distanceTravelled;

    public const float positionZ = 0f;
    public Vector3 startPosition, endPosition;
    private float timer;
    public float timeToReachTarget;
    bool shouldLerp;
    int currentPointIndex;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldLerp = true;
        }

        if (pathCreator != null)
        {
            //distanceTravelled += speed * Time.deltaTime;
            if (shouldLerp == true)
            {
                LerpToNextRotation();
            }
        }
    }

    // Set transform position to the first point position
    public void SetToStartPosition()
    {
        this.gameObject.transform.localPosition = new Vector3(pathPlacer.points[0].x, pathPlacer.points[0].y, positionZ);
    }

    private void LerpToNextRotation()
    {
        if ((currentPointIndex + 1) >= (pathPlacer.points.Length - 1))
        {
            currentPointIndex = 0;
        }

        if (timer >= timeToReachTarget)
        {
            timer = 0;
            currentPointIndex++;
        }

        // Get start and end position based on the current point index
        startPosition = pathPlacer.points[currentPointIndex];
        endPosition = pathPlacer.points[currentPointIndex + 1];

        // Create vector3 position
        startPosition = new Vector3(startPosition.x, startPosition.y, positionZ);
        endPosition = new Vector3(endPosition.x, endPosition.y, positionZ);



        // Increment timer
        timer += Time.deltaTime / timeToReachTarget;

        // Lerp rotation
        this.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer);
    }

}
