using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public GameObject blueHitExplosion, purpleHitExplosion, redHitExplosion, greenHitExplosion, yellowHitExplosion, orangeHitExplosion; // Hit explosions
    public GameObject blueMissExplosion, purpleMissExplosion, redMissExplosion, greenMissExplosion, yellowMissExplosion, orangeMissExplosion; // Hit explosions
    public GameObject specialHitExplosion; // Special hit explosion

    private string feverExplosionTag;
    private string feverExplosionToSpawnName;
    private FeverTimeManager feverTimeManager; // Fever time manager for controlling fever time explosions

    public Transform canvas;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;



    // Use this for initialization
    void Start () {

        // Initialize
        feverExplosionTag = "Fever";

        // Get the reference to the fever time manager
        feverTimeManager = FindObjectOfType<FeverTimeManager>();





        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(canvas.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    // Spawn explosion from the pool
    private void SpawnFromPool(string _tag, Vector3 _position)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            GameObject objectToSpawn = poolDictionary[_tag].Dequeue();

            objectToSpawn.gameObject.SetActive(true);


            //objectToSpawn.transform.rotation = Quaternion.Euler(90, 0, -45);
            objectToSpawn.transform.rotation = Quaternion.Euler(90, 0, 180);

            objectToSpawn.transform.position = _position;

            poolDictionary[_tag].Enqueue(objectToSpawn);
        }
    }


    // Spawn hit explosion
    public void SpawnExplosion(Vector3 _position, string _objectTag)
    {
        // Check if fever time, if it is spawn a special explosion, if not check the object tag and spawn the correct colored explosion for normal objects
        if (feverTimeManager.FeverTimeActivated == true)
        {
            // Set the name
            feverExplosionToSpawnName = feverExplosionTag + _objectTag;
            // instantiate special hit explosion
            SpawnFromPool(feverExplosionToSpawnName, _position); // Spawn special miss explosion for special miss notes
        }
        else
        {
            // Activate the correct coloured explosion at the hit objects position
            SpawnFromPool(_objectTag, _position);
        }
    }
}




