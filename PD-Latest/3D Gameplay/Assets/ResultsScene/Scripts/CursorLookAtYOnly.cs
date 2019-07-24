using UnityEngine;

public class CursorLookAtYOnly : MonoBehaviour {

    public float sensitivity = 1;
    public float newRotationX;
    private string axisY;


    // Use this for initialization
    void Start () {
        // Initialize
        axisY = "Mouse Y";
	}
	
	// Update is called once per frame
	void Update () {

        newRotationX = transform.localEulerAngles.x - Input.GetAxis(axisY) * sensitivity;
        newRotationX = Mathf.Clamp(newRotationX, 0, 0.5f);

        gameObject.transform.localEulerAngles = new Vector3(newRotationX, 0, 0);

    }
}
