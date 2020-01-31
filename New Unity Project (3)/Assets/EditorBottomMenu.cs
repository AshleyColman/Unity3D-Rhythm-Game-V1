using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EditorBottomMenu : MonoBehaviour
{
    public TextMeshProUGUI objectIDText, objectSpawnTimeText, objectPositionText, currentMousePositionText, deleteButtonText;
    public TMP_Dropdown animationDropdown, objectTypeDropdown, objectSoundDropdown;

    public GameObject defaultDropdownButtons, hitObjectPropertyDropdowns;

    public Button deleteButton;

    private Vector3 previousFrameMousePosition;

    private const string DEFAULT_STRING = "-", DELETE_STRING = "DELETE";

    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();

        ResetBottomMenu();
    }

    private void Update()
    {
        if (previousFrameMousePosition != scriptManager.mouseFollow.transform.localPosition)
        {
            currentMousePositionText.text = "x: " + scriptManager.mouseFollow.transform.localPosition.x.ToString("F2") + " y: "
                + scriptManager.mouseFollow.transform.localPosition.y.ToString("F2");
        }

        previousFrameMousePosition = scriptManager.mouseFollow.transform.localPosition;
    }

    // Reset bottom menu
    public void ResetBottomMenu()
    {
        objectIDText.text = DEFAULT_STRING;
        objectSpawnTimeText.text = DEFAULT_STRING;
        objectPositionText.text = DEFAULT_STRING;
        deleteButtonText.text = DEFAULT_STRING;

        deleteButton.interactable = false;

        if (defaultDropdownButtons.activeSelf == false)
        {
            defaultDropdownButtons.gameObject.SetActive(true);
        }

        if (hitObjectPropertyDropdowns.activeSelf == true)
        {
            hitObjectPropertyDropdowns.gameObject.SetActive(false);
        }
    }

    // Update bottom menu with hit object information
    public void UpdateBottomMenu(int _objectID, float _spawnTime, int _animation, int _objectType, int _objectSound)
    {
        objectIDText.text = "OBJECT: " + _objectID.ToString();
        objectSpawnTimeText.text = _spawnTime.ToString();
        // Display the editable hit objects local transform to give accurate local position values
        objectPositionText.text = "x: " + scriptManager.editableHitObject.transform.localPosition.x.ToString("F2") + 
            " y: " + scriptManager.editableHitObject.transform.localPosition.y.ToString("F2");

        deleteButtonText.text = DELETE_STRING;

        deleteButton.interactable = true;

        if (defaultDropdownButtons.activeSelf == true)
        {
            defaultDropdownButtons.gameObject.SetActive(false);
        }

        if (hitObjectPropertyDropdowns.activeSelf == false)
        {
            hitObjectPropertyDropdowns.gameObject.SetActive(true);
        }

        animationDropdown.value = _animation;
        objectTypeDropdown.value = _objectType;
        objectSoundDropdown.value = _objectSound;
    }

    // Update bottom menu with the updated spawn time
    public void UpdateSpawnTimeText(float _spawnTime)
    {
        objectSpawnTimeText.text = _spawnTime.ToString();
    }

    // Update bottom menu with the updated object id
    public void UpdateIDText(int _objectID)
    {
        objectIDText.text = _objectID.ToString();
    }

    public void UpdatePositionText()
    {
        objectPositionText.text = "x: " + scriptManager.editableHitObject.transform.localPosition.x.ToString("F2") + 
            " y: " + scriptManager.editableHitObject.transform.localPosition.y.ToString("F2");
    }
}
