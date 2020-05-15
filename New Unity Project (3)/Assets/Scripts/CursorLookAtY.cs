using UnityEngine;

public class CursorLookAtY : MonoBehaviour {

    public float sensitivity = 0.01f;
    private float newRotationX, newRotationY;
    private string axisY, axisX;

    // Use this for initialization
    void Start()
    {
        // Initialize
        axisY = "Mouse Y";
        axisX = "Mouse X";
    }

    // Update is called once per frame
    void Update()
    {
        newRotationX = transform.localEulerAngles.x - Input.GetAxis(axisY) * sensitivity;
        newRotationX = Mathf.Clamp(newRotationX, 0f, 2f);

        newRotationY = transform.localEulerAngles.y + Input.GetAxis(axisX) * sensitivity;
        newRotationY = Mathf.Clamp(newRotationY, 0f, 2f);

        gameObject.transform.localEulerAngles = new Vector3(newRotationX, newRotationY, 0);
    }
}
