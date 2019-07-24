using UnityEngine;

public class DestroyEffect : MonoBehaviour {

    // Integers
    private float timer, destroyTime; // Hit object destroy time and timer

    // Use this for initialization
    void Start()
    {
        // Initialize
        destroyTime = 1.2f;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= destroyTime)
        {
            DestroyHitObject();
        }
    }

    // Destory 
    public void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
