using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathPlacer : MonoBehaviour {

    #region Variables
    private const float positionZ = 0f;
    private float spacing = 10f;
    private float resolution = 1;
    private int beatInterval = 1;
    private int currentBeat = 0;
    private const int NEARBY_POINT_LIST_SIZE = 10;
    public Vector2[] points;

    public GameObject tickPointGameObject;
    public Transform tickPointSpawnCanvas;

    public GameObject beatPointGameObject;
    public Transform beatPointSpawnCanvas;

    public GameObject nearbyPointPrefab;
    public List<GameObject> nearbyPointsList = new List<GameObject>();
    public Transform nearbyPointSpawnCanvas;

    public List<GameObject> instantiatedBeatPointList = new List<GameObject>();

    public TMP_InputField spacingInputField, resolutionInputField, beatIntervalInputField;

    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public float Spacing
    {
        get { return spacing; }
    }

    public float Resolution
    {
        get { return resolution; }
    }

    public int BeatInterval
    {
        get { return beatInterval; }
    }
    #endregion

    void Start () {

        scriptManager = FindObjectOfType<ScriptManager>();

        // Reference all evenly calculated points
        points = scriptManager.createdPath.CalculateEvenlySpacedPoints(spacing, resolution);

        // Set start position of follower object
        scriptManager.follower.SetToStartPosition();

        // Instantiate nearby point gameobjects
        InstantiateNearbyPoints();
    }

    // Instantiate nearby point gameobjects
    private void InstantiateNearbyPoints()
    {
        for (int i = 0; i < NEARBY_POINT_LIST_SIZE; i++)
        {
            GameObject gameobject = Instantiate(nearbyPointPrefab, Vector2.zero, Quaternion.identity, nearbyPointSpawnCanvas);

            nearbyPointsList.Add(gameobject);
        }
    }

    public void UpdateSpacing()
    {
        spacing = float.Parse(spacingInputField.text);
    }

    public void UpdateResolution()
    {
        resolution = float.Parse(resolutionInputField.text);
    }

    public void UpdateBeatInterval()
    {
        beatInterval = int.Parse(beatIntervalInputField.text);
    }

    public void UpdatePathPoints()
    {
        // Reference all evenly calculated points
        points = scriptManager.createdPath.CalculateEvenlySpacedPoints(spacing, resolution);

        // Set start position of follower object
        scriptManager.follower.SnapToCurrentPointPosition();

        UpdateNearbyPoints();

        // Spawn beat points along the slider
        //SpawnBeatPoints();
    }

    // Update nearby points
    public void UpdateNearbyPoints()
    {
        int pointIndex = scriptManager.metronomePro.CurrentTick - 5;

        for (int i = 0; i < nearbyPointsList.Count; i++)
        {
            if ((pointIndex + i) >= 0 && (pointIndex + i) < scriptManager.pathPlacer.points.Length)
            {
                if (nearbyPointsList[i].gameObject.activeSelf == false)
                {
                    nearbyPointsList[i].gameObject.SetActive(true);
                }
                nearbyPointsList[i].transform.localPosition = scriptManager.pathPlacer.points[pointIndex + i];
            }
            else
            {
                if (nearbyPointsList[i].gameObject.activeSelf == true)
                {
                    nearbyPointsList[i].gameObject.SetActive(false);
                }
            }
        }


        /*
        int pointIndex = scriptManager.metronomePro.CurrentTick - 1;

        for (int i = 0; i < nearbyPointsList.Count; i++)
        {
            if ((pointIndex + i) < scriptManager.pathPlacer.points.Length)
            {
                if (nearbyPointsList[i].gameObject.activeSelf == false)
                {
                    nearbyPointsList[i].gameObject.SetActive(true);
                }
                nearbyPointsList[i].transform.localPosition = scriptManager.pathPlacer.points[pointIndex + i];
            }
            else
            {
                if (nearbyPointsList[i].gameObject.activeSelf == true)
                {
                    nearbyPointsList[i].gameObject.SetActive(false);
                }
            }
        }
        */
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

                instantiatedBeatPointList.Add(gameObject);

                // Reset 
                currentBeat = 0;
            }
        }
    }

    // Destroy all beat points
    public void DestroyBeatPoints()
    {
        for (int i = 0; i < instantiatedBeatPointList.Count; i++)
        {
            Destroy(instantiatedBeatPointList[i].gameObject);
        }

        instantiatedBeatPointList.Clear();
    }
}
