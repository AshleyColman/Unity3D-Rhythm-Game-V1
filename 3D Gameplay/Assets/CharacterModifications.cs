using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterModifications : MonoBehaviour {

    // The character modifications panel
    public GameObject characterModificationsPanel;
    // Used for controlling the visiblity of the panel
    private bool characterModificationsPanelActive;

    public Animator characterModifcationAnimator;

	// Use this for initialization
	void Start () {
        characterModificationsPanelActive = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Turn on or off the character modifications panel
    public void toggleCharacterModificationsPanel()
    {
        if (characterModificationsPanelActive == false)
        {
            //characterModifcationAnimator.Play("CharacterModificationsPanelActivateAnimation");
            characterModificationsPanel.gameObject.SetActive(true);
            characterModificationsPanelActive = true;
        }
        else if (characterModificationsPanelActive == true)
        {
            //StartCoroutine(DeactivateCharacterModificationPanel());
            characterModificationsPanel.gameObject.SetActive(false);
            characterModificationsPanelActive = false;
        }
    }

    IEnumerator DeactivateCharacterModificationPanel()
    {
        characterModifcationAnimator.Play("CharacterModificationsPanelDeactivateAnimation");
        yield return new WaitForSeconds(0.50f);
    }
}
