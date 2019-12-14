using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditableHitObject : MonoBehaviour
{
    public Image selectedImage, colorImage, diamondReflectionImage, squareReflectionImage;

    public TextMeshProUGUI numberText;

    private DestroyTimelineObject timelineHitObjectScript;

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

        // Check for type change
        CheckInputForObjectTypeChange();

        if (followCursorPosition == true)
        {
            // Follow the cursors position
            this.gameObject.transform.position = scriptManager.cursorHitObject.transform.position;
        }
    }

    // Setup the editor hit object 
    public void SetupEditorObject()
    {
        if (scriptManager.placedObject.hitObjectList.Count != 0)
        {
            // Set the position
            this.gameObject.transform.position = scriptManager.placedObject.hitObjectList[objectIndex].hitObjectPosition;

            // Set the rotation based on the hit object type
            // Set the color based on the hit object type
            switch (scriptManager.placedObject.hitObjectList[objectIndex].hitObjectType)
            {
                case 0:
                    DisplayLeftHitObject();
                    break;
                case 1:
                    DisplayRightHitObject();
                    break;
            }

            // Update the text
            UpdateNumberText();
        }
    }

    // Display the editable hit object as left hit object
    private void DisplayLeftHitObject()
    {
        // Left hit object square
        colorImage.color = scriptManager.colorManager.pinkColor;
        this.transform.rotation = scriptManager.cursorHitObject.SquareRotation;
        numberText.transform.rotation = Quaternion.Euler(0, 0, 0);
        squareReflectionImage.gameObject.SetActive(true);
        diamondReflectionImage.gameObject.SetActive(false);
    }

    // Display the editable hit object as right hit object
    private void DisplayRightHitObject()
    {
        // Right hit object diamond
        colorImage.color = scriptManager.colorManager.purpleColor;
        this.transform.rotation = scriptManager.cursorHitObject.DiamondRotation;
        numberText.transform.rotation = Quaternion.Euler(0, 0, 0);
        squareReflectionImage.gameObject.SetActive(false);
        diamondReflectionImage.gameObject.SetActive(true);
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


    // Check input for changing the hit object type
    private void CheckInputForObjectTypeChange()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Switch to the other hit object on key press
            switch (scriptManager.placedObject.hitObjectList[objectIndex].hitObjectType)
            {
                case 0:
                    // Was square, change to diamond
                    // Update the type for this hit object
                    scriptManager.placedObject.hitObjectList[objectIndex].hitObjectType = 1;
                    DisplayRightHitObject();
                    break;
                case 1:
                    // Was diamond, change to square
                    // Update the type for this hit object
                    scriptManager.placedObject.hitObjectList[objectIndex].hitObjectType = 0;
                    DisplayLeftHitObject();
                    break;
            }
        }
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
                scriptManager.placedObject.hitObjectList[objectIndex].hitObjectPosition = this.gameObject.transform.position;
                // Reset selected color
                selectedImage.color = scriptManager.colorManager.blackColor;
            }
        }
    }
}

