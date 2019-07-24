using UnityEngine;

public class ChangeColor : MonoBehaviour {

    // The default material color
    public Material defaultMaterial;
    // The changed material color
    public Material changedMaterial;
    // Renderer
    public Renderer rend;

    // Keycodes
    private KeyCode feverTimeActivateKey; // Key that activates fevertime - changes material

	// Use this for initialization
	void Start () {

        // Initialize
        rend = GetComponent<Renderer>(); // Get the renderer
        rend.enabled = true; // Enable at the start
	}
	
	// Update is called once per frame
	void Update () {

        // If button is pressed change color of object
        if (Input.GetKeyDown(feverTimeActivateKey))
        {
            // Change the material
            rend.material = changedMaterial;
        }
        
        // If button is released change color of object to original
        if (Input.GetKeyUp(feverTimeActivateKey))
        {
            // Change the material back to default
            rend.material = defaultMaterial;
        }
	}
}
