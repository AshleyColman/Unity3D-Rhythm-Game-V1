using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI toolTipText;
    public RectTransform backgroundRectTransform;
    public GameObject container;


    private float PADDING_SIZE = 5f, OFFSET = 25;
    private Vector2 backgroundSize;

    private string currentPositionType;

    private const string POSITION_TYPE_BELOW = "BELOW", POSITION_TYPE_ABOVE = "ABOVE";

    private void Update()
    {
        switch (currentPositionType)
        {
            case POSITION_TYPE_ABOVE:
                transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25f, Input.mousePosition.z);
                break;
            case POSITION_TYPE_BELOW:
                transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 25f, Input.mousePosition.z);
                break;
            default:
                transform.position = Input.mousePosition;
                break;
        }

    }

    public void ShowToolTipFromBelow(string _text)
    {
        currentPositionType = POSITION_TYPE_BELOW;

        ShowToolTip(_text);
    }

    public void ShowToolTipFromAbove(string _text)
    {
        currentPositionType = POSITION_TYPE_ABOVE;

        ShowToolTip(_text);
    }

    public void ShowToolTip(string _text)
    {
        container.gameObject.SetActive(true);

        toolTipText.text = _text;

        // Calcualte size for background
        backgroundSize = new Vector2(toolTipText.preferredWidth + PADDING_SIZE * 2, toolTipText.preferredHeight + PADDING_SIZE * 2);

        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideToolTip()
    {
        container.gameObject.SetActive(false);
    }
}
