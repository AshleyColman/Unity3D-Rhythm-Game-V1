using UnityEngine;
using UnityEngine.UI;

public class MouseFollow : MonoBehaviour {

    public Image cursorImage;

    private void Update()
    {
        SetToCurrentMousePosition();

        if (Input.mousePosition.x < Constants.MAX_MOUSE_LEFT_POS_X)
        {
            this.gameObject.transform.position = new Vector3(Constants.MAX_MOUSE_LEFT_POS_X, Input.mousePosition.y);
        }


        if (Input.mousePosition.x > Screen.width)
        {
            this.gameObject.transform.position = new Vector3(Screen.width, Input.mousePosition.y);
        }

    }

    private void SetToCurrentMousePosition()
    {
        this.gameObject.transform.position = Input.mousePosition;
    }
}
