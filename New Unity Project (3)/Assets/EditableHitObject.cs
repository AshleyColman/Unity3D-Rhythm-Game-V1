using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditableHitObject : MonoBehaviour
{
    //public Color blueColor, greenColor, yellowColor, orangeColor, redColor, purpleColor, blackColor, selectedColor, unselectedColor;
    //public Image outerGlow, inner, outer;

    public TextMeshProUGUI numberText;

    //public Image previousTimelineObjectDiamondImage;

    //public GameObject timelineObject, timelineObjectDiamond; // Timeline object tied to the instantiated editor hit object
    //public Image timelineObjectDiamondImage;

    private bool followCursorPosition;

    private int objectIndex; // Timeline object index
    private int colorIndex; // Color index

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

        // Get and set the position
        //SetupEditorObject();
    }

    private void OnDisable()
    {
        // Reset index
        objectIndex = 0;
        // Turn off cursor follow
        followCursorPosition = false;
    }

    public void SetupEditorObject()
    {
        // Turn off mouse follow
        followCursorPosition = false;

        if (scriptManager.placedObject.hitObjectList.Count != 0)
        {
            this.gameObject.transform.position = scriptManager.placedObject.hitObjectList[objectIndex].hitObjectPosition;

            // Get the color index 
            colorIndex = scriptManager.placedObject.hitObjectList[objectIndex].hitObjectType;

            // Update the color of this game object
            OnEnableChangeColor();

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

    // Use this for initialization
    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
        followCursorPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (outerGlow != null && inner != null)
        {
            // Check for input on changing hit object color
            CheckInputForColorChange();
        }
        */

        if (scriptManager.cursorHitObject != null)
        {
            // Check for position change/save 
            CheckInputForPositionChange();
        }

        if (followCursorPosition == true)
        {
            // Follow the cursors position
            this.gameObject.transform.position = scriptManager.cursorHitObject.transform.position;
        }
    }

    // Update the timeline object image to change when changing the color
    public void UpdateTimelineObjectDiamondImage(Image _timelineObjectDiamondImage)
    {
        //timelineObjectDiamondImage = _timelineObjectDiamondImage;
    }

    // Change the timeline object color
    private void ChangeTimelineObjectColor(Color _color)
    {
        // Change color of the diamond iamge
        //timelineObjectDiamondImage.color = _color;
    }

    // Change the timeline object SELECTED color
    public void ChangeTimelineObjectSelectedColor(Image _timelineObjectDiamondImage)
    {
        /*
        // Reset the color of the previous selected hit object
        if (previousTimelineObjectDiamondImage != null)
        {
            previousTimelineObjectDiamondImage.color = unselectedColor;
        }

        // Change color of the selected image
        _timelineObjectDiamondImage.color = selectedColor;

        // Save the previous object diamond
        previousTimelineObjectDiamondImage = _timelineObjectDiamondImage;
        */
    }

    // Change the hit object color on enable
    private void OnEnableChangeColor()
    {
        /*
        switch (colorIndex)
        {
            case 0:
                inner.color = blueColor;
                outerGlow.color = blueColor;
                break;
            case 1:
                inner.color = purpleColor;
                outerGlow.color = purpleColor;
                break;
            case 2:
                inner.color = redColor;
                outerGlow.color = redColor;
                break;
            case 3:
                inner.color = greenColor;
                outerGlow.color = greenColor;
                break;
            case 4:
                inner.color = yellowColor;
                outerGlow.color = yellowColor;
                break;
            case 5:
                inner.color = orangeColor;
                outerGlow.color = orangeColor;
                break;
        }
        */
    }

    public void ChangeEditableHitObjectColorGreen()
    {
        /*
        // Play sound
        menuSFXAudioSource.PlayOneShot(selectedSoundClip);
        // Change color
        outerGlow.color = greenColor;
        inner.color = greenColor;
        // Change color of timeline object
        ChangeTimelineObjectColor(greenColor);
        // Update hit object color type in the list
        placedObject.editorHitObjectList[objectIndex].hitObjectType = 3;
        */
    }

    private void CheckInputForColorChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeEditableHitObjectColorGreen();
        }
    }

    private void CheckInputForPositionChange()
    {
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (followCursorPosition == false)
            {
                // Play selected sound
                menuSFXAudioSource.PlayOneShot(selectedSoundClip);
                // Enable cursor position follow
                followCursorPosition = true;
                // Enable outer selected color
                outer.color = selectedColor;
            }
            else if (followCursorPosition == true)
            {
                // Play placed sound
                menuSFXAudioSource.PlayOneShot(placedSoundClip);
                // Disable cursor position follow
                followCursorPosition = false;
                // Update the save position for the hit object index selected
                placedObject.editorHitObjectList[objectIndex].hitObjectPosition = this.gameObject.transform.position;
                // Reset selected color
                outer.color = blackColor;
            }
        }
        */
    }
}

