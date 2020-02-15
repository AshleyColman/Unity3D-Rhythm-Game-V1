using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditableHitObject : MonoBehaviour
{
    public Image selectedImage, colorImage, diamondReflectionImage, squareReflectionImage;

    public TextMeshProUGUI numberText;

    private DestroyTimelineObject referencedTimelineHitObjectScript, previousReferencedTimelineHitObjectScript; // Timeline object script that the editable hit object is tied to - timeline object

    private int objectIndex; // Timeline object index

    private bool followCursorPosition;


    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public int ObjectIndex
    {
        set { objectIndex = value; }
        get { return objectIndex; }
    }

    private void OnEnable()
    {
        if (scriptManager == null)
        {
            scriptManager = FindObjectOfType<ScriptManager>();
        }

        followCursorPosition = false;

        // Get and set the position
        SetupEditorObject();
    }

    private void OnDisable()
    {
        // Reset index
        objectIndex = 0;
    }

    // Use this for initialization
    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for position change/save 
        CheckInputForPositionChange();

        if (followCursorPosition == true)
        {
            // Follow the cursors position
            this.gameObject.transform.position = scriptManager.cursorHitObject.positionObject.transform.position;
        }
    }

    // Update the reference to the current timeline object script
    public void UpdateReferenceToTimelineObject(GameObject _gameObject)
    {
        referencedTimelineHitObjectScript = _gameObject.GetComponent<DestroyTimelineObject>();

        // Set toggle on
        referencedTimelineHitObjectScript.SetToggleOn();
        referencedTimelineHitObjectScript.CheckToggle();
    }

    // Set the toggle off than update the color block 
    public void ResetTimelineObject()
    {
        if (referencedTimelineHitObjectScript != null)
        {
            if (referencedTimelineHitObjectScript != previousReferencedTimelineHitObjectScript)
            {
                referencedTimelineHitObjectScript.SetToggleOff();
                referencedTimelineHitObjectScript.CheckToggle();

                previousReferencedTimelineHitObjectScript = referencedTimelineHitObjectScript;
            }
        }
    }

    // Setup the editor hit object 
    public void SetupEditorObject()
    {
        if (scriptManager.placedObject.hitObjectList.Count != 0)
        {
            // Set the position
            this.gameObject.transform.position = scriptManager.placedObject.hitObjectList[objectIndex].HitObjectPosition;

            // Update the text
            UpdateNumberText();
        }
    }

    // Update the object index and text value
    public void UpdateEditableObjectIndex(int _value)
    {
        // Update the object index
        objectIndex = _value;

        // Update the text 
        UpdateNumberText();
    }

    // Update the number text
    public void UpdateNumberText()
    {
        numberText.text = objectIndex.ToString();
    }


    // Check for position change on key press
    private void CheckInputForPositionChange()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (followCursorPosition == false)
            {
                // Enable cursor position follow
                followCursorPosition = true;
                // Enable outer selected color
                selectedImage.color = scriptManager.colorManager.selectedColor;
            }
            else if (followCursorPosition == true)
            {
                // Disable cursor position follow
                followCursorPosition = false;
                // Update the save position for the hit object index selected
                scriptManager.placedObject.hitObjectList[objectIndex].HitObjectPosition = this.gameObject.transform.position;
                // Update the hit objects position
                scriptManager.editorBottomMenu.UpdatePositionText();
                // Reset selected color
                selectedImage.color = scriptManager.colorManager.blackColor;
            }
        }
    }
}

