using UnityEngine;

public class CursorLookAt : MonoBehaviour {

    private float sensitivity = 1;
    private float newRotationY;
    private float newRotationX;
    private string axisX;
    private string axisY;

    private void Start()
    {
        axisX = "Mouse X";
        axisY = "Mouse Y";
    }

    // Update is called once per frame
    void Update () {

        // Get the angles and restrict movement after certain amount
        newRotationY = transform.localEulerAngles.y + Input.GetAxis(axisX) * sensitivity;
        newRotationY = Mathf.Clamp(newRotationY, 0, 0.5f);
        newRotationX = transform.localEulerAngles.x - Input.GetAxis(axisY) * sensitivity;
        newRotationX = Mathf.Clamp(newRotationX, 0, 0.5f);

        gameObject.transform.localEulerAngles = new Vector3(newRotationX, newRotationY, 0);
    }
}
