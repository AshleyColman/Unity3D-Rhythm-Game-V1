using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    #region Variables
    // Animation
    public Animator messagePanelAnimator;

    // Image
    public Image messagePanelImage;

    // Text
    public TextMeshProUGUI messageText;
    #endregion

    #region Functions
    #endregion

    // Display message
    public void DisplayMessage(string _message, Color _color)
    {
        // Set color
        messagePanelImage.color = _color;

        // Set text
        messageText.text = _message;

        // Activate 
        messagePanelAnimator.gameObject.SetActive(true);

        // Play animation
        messagePanelAnimator.Play("MessagePanel_Animation", 0, 0f);
    }
}
