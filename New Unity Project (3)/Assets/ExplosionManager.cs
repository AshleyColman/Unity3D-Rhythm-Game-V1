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
    public Dictionary<string, Queue<ExplosionDeactivate>> poolDictionary;

    // Use this for initialization
    void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        poolDictionary = new Dictionary<string, Queue<ExplosionDeactivate>>();

        foreach (Pool pool in pools)
        {
            Queue<ExplosionDeactivate> objectPool = new Queue<ExplosionDeactivate>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(scriptManager.loadAndRunBeatmap.canvas.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(false);

                objectPool.Enqueue(obj.GetComponent<ExplosionDeactivate>());
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Spawn explosion from the pool
    private void SpawnFromPool(string _tag, Vector3 _position, int _judgementScore, Color _colorImageColor)
    {
        if (poolDictionary.ContainsKey(_tag) == true)
        {
            ExplosionDeactivate objectToSpawnScript = poolDictionary[_tag].Dequeue();

            // Assign image color
            objectToSpawnScript.Color = _colorImageColor;

            // Activate gameobject
            objectToSpawnScript.gameObject.SetActive(true);

            // Assign position
            objectToSpawnScript.transform.position = _position;

            // Update text
            switch (_tag)
            {
                case (Constants.KEY_HIT_OBJECT_TYPE_KEY1_TAG + "_" + Constants.HIT_TAG):
                    objectToSpawnScript.keyText.text = Constants.KEY_HIT_OBJECT_TYPE_KEY1_CHAR;
                    objectToSpawnScript.keyText.color = _colorImageColor;
                    break;
                case (Constants.KEY_HIT_OBJECT_TYPE_KEY2_TAG + "_" + Constants.HIT_TAG):
                    objectToSpawnScript.keyText.text = Constants.KEY_HIT_OBJECT_TYPE_KEY2_CHAR;
                    objectToSpawnScript.keyText.color = _colorImageColor;
                    break;
            }

            poolDictionary[_tag].Enqueue(objectToSpawnScript);
        }
    }

    // Spawn hit explosion
    public void SpawnExplosion(string _type, string _action, Vector3 _position, int _judgementScore, Color _colorImageColor)
    {
        // Create tag based on the type and action
        string tag = _type + "_" + _action;

        // Activate the correct coloured explosion at the hit objects position
        SpawnFromPool(tag, _position, _judgementScore, _colorImageColor);
    }
}
