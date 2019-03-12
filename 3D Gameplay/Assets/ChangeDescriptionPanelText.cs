using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDescriptionPanelText : MonoBehaviour {

    public Text descriptionPanelText; // The description panel text to change and update
    private string PLAYText, MULTIPLAYERText, EDITORText, SETTINGSText, EXITText;

    void Start()
    {
        PLAYText = "Compete against others on user created beatmaps";
        MULTIPLAYERText = "Play against others online";
        EDITORText = "Create your own beatmap";
        SETTINGSText = "Customize the game to your preference";
        EXITText = "Exit the game";
    }
	public void UpdateDescriptionPanelText(string buttonSelectedPass)
    {
        if (buttonSelectedPass == "PLAY")
        {
            descriptionPanelText.text = PLAYText;
        }
        if (buttonSelectedPass == "MULTIPLAYER")
        {
            descriptionPanelText.text = MULTIPLAYERText;
        }
        if (buttonSelectedPass == "EDITOR")
        {
            descriptionPanelText.text = EDITORText;
        }
        if (buttonSelectedPass == "SETTINGS")
        {
            descriptionPanelText.text = SETTINGSText;
        }
        if (buttonSelectedPass == "EXIT")
        {
            descriptionPanelText.text = EXITText;
        }
    }
}
