using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorBottomMenu : MonoBehaviour
{
    public TextMeshProUGUI objectIDText, objectSpawnTimeText, objectPositionText, currentMousePositionText;
    public TMP_Dropdown animationDropdown, objectTypeDropdown, objectSoundDropdown;

    private Vector3 previousFrameMousePosition;

    private const string DEFAULT_STRING = "-";

    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
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
        animationDropdown.interactable = false;
        objectTypeDropdown.interactable = false;
        objectSoundDropdown.interactable = false;
    }

    // Update bottom menu with hit object information
    public void UpdateBottomMenu(int _objectID, float _spawnTime, int _animation, int _objectType, int _objectSound)
    {
        objectIDText.text = "OBJECT: " + _objectID.ToString();
        objectSpawnTimeText.text = _spawnTime.ToString();
        // Display the editable hit objects local transform to give accurate local position values
        objectPositionText.text = "x: " + scriptManager.editableHitObject.transform.localPosition.x.ToString("F2") + 
            " y: " + scriptManager.editableHitObject.transform.localPosition.y.ToString("F2");

        animationDropdown.interactable = true;
        objectTypeDropdown.interactable = true;
        objectSoundDropdown.interactable = true;

        animationDropdown.value = _animation;
        objectTypeDropdown.value = _objectType;
        objectSoundDropdown.value = _objectSound;
    }
}
