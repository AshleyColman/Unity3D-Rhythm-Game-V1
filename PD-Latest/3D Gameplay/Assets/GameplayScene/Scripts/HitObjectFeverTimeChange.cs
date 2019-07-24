using UnityEngine;

public class HitObjectFeverTimeChange : MonoBehaviour {

    // FEVER MATERIALS
    public Material feverInnerMaterial; // The fever time inner material 
    public Material feverOverlayMaterial; // The fever overlay material

    // NORMAL MATERIALS
    public Material normalDiamondMaterial; // The normal diamond material
    public Material normalOverlayMaterial; // The normal overlay material

    // BOTH MATERIALS
    public Material darkBlueMaterial, darkGreenMaterial, darkOrangeMaterial, darkYellowMaterial, darkPurpleMaterial, darkRedMaterial;

    // Strings
    private string objectTag; // The object tag 

    // Bools
    public bool hasChangedToFeverMaterial; // Has the hit objects material changed once to fever time
    public bool changeToFeverMaterial; // Should the hit object change to fever material
    public bool hasChangedToNormalMaterial; // Has the hit objects material changed once to normal
    public bool changeToNormalMaterial; // Should the hit object change to normal material

    // Transforms
    private Transform hitObjectDiamond, hitObjectInner, hitObjectOverlay; // Transforms of the hit object, childs

    // Scripts
    private FeverTimeManager feverTimeManager; // Reference to the fever time manager

    // Reset on being activated again
    private void OnEnable()
    {
        // If the hit object components are not null
        if (hitObjectOverlay != null && hitObjectDiamond != null & hitObjectInner != null)
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
    void Start () {

        // Initialize 
        hitObjectDiamond = this.gameObject.transform.GetChild(1); // Get the reference to the hitobject child gameobjects
        hitObjectInner = this.gameObject.transform.GetChild(2);
        hitObjectOverlay = this.gameObject.transform.GetChild(4);
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
        // Get the renderer material attached to the child gameobjects of the hit object, change material
        hitObjectInner.GetComponent<Renderer>().material = feverInnerMaterial;
        hitObjectOverlay.GetComponent<Renderer>().material = feverOverlayMaterial;

        // Change the fever material based off the object tag 
        switch (objectTag)
        {
            case "Blue":
                hitObjectDiamond.GetComponent<Renderer>().material = darkBlueMaterial;
                break;
            case "Green":
                hitObjectDiamond.GetComponent<Renderer>().material = darkGreenMaterial;
                break;
            case "Purple":
                hitObjectDiamond.GetComponent<Renderer>().material = darkPurpleMaterial;
                break;
            case "Red":
                hitObjectDiamond.GetComponent<Renderer>().material = darkRedMaterial;
                break;
            case "Orange":
                hitObjectDiamond.GetComponent<Renderer>().material = darkOrangeMaterial;
                break;
            case "Yellow":
                hitObjectDiamond.GetComponent<Renderer>().material = darkYellowMaterial;
                break;
        }

        // Set to true as the material has changed
        hasChangedToFeverMaterial = true;
    }

    // Change current fever time hit object materials to normal hit object materials
    public void ChangeToNormalMaterial()
    {
        // Get the renderer material attached to the child gameobjects of the hit object, change material
        hitObjectDiamond.GetComponent<Renderer>().material = normalDiamondMaterial;
        hitObjectOverlay.GetComponent<Renderer>().material = normalOverlayMaterial;

        // Change the fever material based off the object tag 
        switch (objectTag)
        {
            case "Blue":
                hitObjectInner.GetComponent<Renderer>().material = darkBlueMaterial;
                break;
            case "Green":
                hitObjectInner.GetComponent<Renderer>().material = darkGreenMaterial;
                break;
            case "Purple":
                hitObjectInner.GetComponent<Renderer>().material = darkPurpleMaterial;
                break;
            case "Red":
                hitObjectInner.GetComponent<Renderer>().material = darkRedMaterial;
                break;
            case "Orange":
                hitObjectInner.GetComponent<Renderer>().material = darkOrangeMaterial;
                break;
            case "Yellow":
                hitObjectInner.GetComponent<Renderer>().material = darkYellowMaterial;
                break;
        }
    }
}
