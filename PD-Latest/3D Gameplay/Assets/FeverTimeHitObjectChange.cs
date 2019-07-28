using UnityEngine;
using UnityEngine.UI;

public class FeverTimeHitObjectChange : MonoBehaviour {

    public Color blueColor, redColor, yellowColor, purpleColor, greenColor, orangeColor, blackColor, whiteColor;

    // Strings
    private string objectTag; // The object tag 

    // Bools
    public bool hasChangedToFeverMaterial; // Has the hit objects material changed once to fever time
    public bool changeToFeverMaterial; // Should the hit object change to fever material
    public bool hasChangedToNormalMaterial; // Has the hit objects material changed once to normal
    public bool changeToNormalMaterial; // Should the hit object change to normal material

    // Image
    public Image hitObjectOuterImage, hitObjectInnerImage, hitObjectInnerInnerImage;

    // Scripts
    private FeverTimeManager feverTimeManager; // Reference to the fever time manager

    // Reset on being activated again
    private void OnEnable()
    {
        // If the hit object components are not null
        if (hitObjectOuterImage != null & hitObjectInnerImage != null & hitObjectInnerInnerImage != null)
        {
            // Change to the normal material
            ChangeToNormalMaterial();
        }

        // Reset
        changeToFeverMaterial = false;
        changeToNormalMaterial = false;
        hasChangedToFeverMaterial = false;
        hasChangedToNormalMaterial = false;
    }

    // Use this for initialization
    void Start()
    {

        // Initialize 

        objectTag = gameObject.tag; // Get the object tag
        hasChangedToFeverMaterial = false;
        hasChangedToNormalMaterial = false;
        changeToFeverMaterial = false;
        changeToNormalMaterial = false;


        // Reference
        feverTimeManager = FindObjectOfType<FeverTimeManager>(); // Get the reference to the fever time manager
    }

    private void Update()
    {
        // Check if the hit object can change to fever material
        checkCanChangeFeverMaterial();

        // Change material to fever if allowed
        if (changeToFeverMaterial == true)
        {
            // Change the current hit object materials to fever time materials
            ChangeToFeverMaterial();

            // Set to true
            hasChangedToFeverMaterial = true;
            // Set to false
            changeToFeverMaterial = false;
        }

        // Check if the hit object can change to normal material
        checkCanChangeNormalMaterial();

        if (changeToNormalMaterial == true)
        {
            // Change the current hit object materials to fever time materials
            ChangeToNormalMaterial();

            // Set to true
            hasChangedToNormalMaterial = true;
            // Set to false
            changeToNormalMaterial = false;
        }
    }

    // Check if the hit object is allowed to change to the fever material
    private void checkCanChangeFeverMaterial()
    {
        // If is fever time
        if (feverTimeManager.FeverTimeActivated == true)
        {
            // If the hit object has not changed to fever material before
            if (hasChangedToFeverMaterial == false)
            {
                // Set the hit object to be able to change to fever material
                changeToFeverMaterial = true;
            }
        }
    }

    // Check if the hit object is allowed to change to the normal material
    private void checkCanChangeNormalMaterial()
    {

        // If is fever time
        if (feverTimeManager.FeverTimeActivated == false)
        {
            // Check if the hit object material has previously changed to fever material
            if (hasChangedToFeverMaterial == true)
            {
                // If the hit object has not changed to normal material before
                if (hasChangedToNormalMaterial == false)
                {
                    // Set the hit object to be able to change to normal material
                    changeToNormalMaterial = true;
                }
            }
        }
    }

    // Change current hit object materials to fever time hit object materials
    public void ChangeToFeverMaterial()
    {
        hitObjectOuterImage.color = whiteColor;
        hitObjectInnerInnerImage.color = whiteColor;

        // Change the fever material based off the object tag 
        switch (objectTag)
        {
            case "Blue":
                hitObjectInnerImage.color = blueColor;
                break;
            case "Green":
                hitObjectInnerImage.color = greenColor;
                break;
            case "Orange":
                hitObjectInnerImage.color = orangeColor;
                break;
            case "Yellow":
                hitObjectInnerImage.color = yellowColor;
                break;
            case "Purple":
                hitObjectInnerImage.color = purpleColor;
                break;
            case "Red":
                hitObjectInnerImage.color = redColor;
                break;
        }

        // Set to true as the material has changed
        hasChangedToFeverMaterial = true;
    }

    // Change current fever time hit object materials to normal hit object materials
    public void ChangeToNormalMaterial()
    {
        hitObjectOuterImage.color = blackColor;
        hitObjectInnerImage.color = whiteColor;

        // Change the fever material based off the object tag 
        switch (objectTag)
        {
            case "Blue":
                hitObjectInnerInnerImage.color = blueColor;
                break;
            case "Green":
                hitObjectInnerInnerImage.color = greenColor;
                break;
            case "Yellow":
                hitObjectInnerInnerImage.color = yellowColor;
                break;
            case "Orange":
                hitObjectInnerInnerImage.color = orangeColor;
                break;
            case "Red":
                hitObjectInnerInnerImage.color = redColor;
                break;
            case "Purple":
                hitObjectInnerInnerImage.color = purpleColor;
                break;
        }
    }
}

