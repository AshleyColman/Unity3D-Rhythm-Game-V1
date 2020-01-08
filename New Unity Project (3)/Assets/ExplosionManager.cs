using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    // Scripts
    private ScriptManager scriptManager;

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
    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();


        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(scriptManager.loadAndRunBeatmap.canvas.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
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

            objectToSpawn.transform.position = _position;

            poolDictionary[_tag].Enqueue(objectToSpawn);
        }
    }

    // Spawn hit explosion
    public void SpawnExplosion(Vector3 _position, string _objectTag)
    {
        // Activate the correct coloured explosion at the hit objects position
        SpawnFromPool(_objectTag, _position);
    }
}
