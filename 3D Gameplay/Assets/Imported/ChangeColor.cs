using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    // The default material color
    public Material defaultMaterial;
    // The changed material color
    public Material changedMaterial;

    public Renderer rend;




	// Use this for initialization
	void Start () {

        rend = GetComponent<Renderer>(); // Get the renderer
        rend.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        // If button is pressed change color of object
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rend.material = changedMaterial;
        }
        
        // If button is released change color of object to original
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rend.material = defaultMaterial;
        }

	}
}
