using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour {

    public Image mouseCursorImage;
    public Color highlightedColor, normalColor;

    public void HighlightMouseCursor()
    {
        mouseCursorImage.color = highlightedColor;
    }

    public void NormalMouseColor()
    {
        mouseCursorImage.color = normalColor;
    }
}
