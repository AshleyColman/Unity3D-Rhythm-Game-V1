using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour {

    public const float positionZ = 0f;
    public float spacing = .1f;
    public float resolution = 1;
    private int beatInterval = 2;
    private int currentBeat = 0;
    public Vector2[] points;

    public GameObject tickPointGameObject;
    public Transform tickPointSpawnCanvas;

    public GameObject beatPointGameObject;
    public Transform beatPointSpawnCanvas;

    private ScriptManager scriptManager;

    void Start () {

        scriptManager = FindObjectOfType<ScriptManager>();

        // Reference all evenly calculated points
        points = scriptManager.pathCreator.createdPath.CalculateEvenlySpacedPoints(spacing, resolution);

        // Set start position of follower object
        scriptManager.follower.SetToStartPosition();

        // Spawn tick points along the slider
        SpawnTickPoints();

        // Spawn beat points along the slider
        SpawnBeatPoints();
    }
	
    // Spawn tick points along the slider
    private void SpawnTickPoints()
    {
        foreach (Vector2 p in points)
        {
            GameObject gameObject = Instantiate(tickPointGameObject, tickPointSpawnCanvas);
            gameObject.transform.localScale = Vector3.one * spacing * .5f;
            gameObject.transform.localPosition = new Vector3(p.x, p.y, positionZ);
        }
    }

    // Spawn beat points along the slider
    private void SpawnBeatPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            // Increment current beat
            currentBeat++;

            // If time to spawn a new beat object
            if (currentBeat >= beatInterval)
            {
                GameObject gameObject = Instantiate(beatPointGameObject, beatPointSpawnCanvas);
                //gameObject.transform.localScale = Vector3.one * spacing * .5f;
                gameObject.transform.localPosition = new Vector3(points[i].x, points[i].y, positionZ);

                // Reset 
                currentBeat = 0;
            }
        }
    }
	
}
