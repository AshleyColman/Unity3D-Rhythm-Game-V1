using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridsnapManager : MonoBehaviour
{
    private bool snappingEnabled;

    // Input field
    public TMP_InputField gridSizeXInputField, gridSizeYInputField, gridSpacingXInputField, gridSpacingYInputField;

    // Grid layout group
    public GridLayoutGroup gridLayoutGroup;

    // Ints
    private int gridSizeX, gridSizeY, spacingSizeX, spacingSizeY;

    // Scripts
    private ScriptManager scriptManager;

    // Properties
    public bool SnappingEnabled
    {
        get { return snappingEnabled; }
    }

    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        snappingEnabled = true;

        // Set default layout values
        gridSizeX = 100;
        gridSizeY = 100;
        spacingSizeX = 0;
        spacingSizeY = 0;

        // Update the grid layout group
        gridLayoutGroup.cellSize = new Vector2(gridSizeX, gridSizeY);
        gridLayoutGroup.spacing = new Vector2(spacingSizeX, spacingSizeY);
    }

    // Update the grid layout with the text field values
    public void UpdateGridLayout(string _type)
    {
        switch (_type)
        {
            case "SIZEX":
                if (gridSizeXInputField.text != "" && gridSizeXInputField.text != "-")
                {
                    gridSizeX = int.Parse(gridSizeXInputField.text);

                    if (gridSizeX < 0)
                    {
                        gridSizeX = 0;
                    }
                }
                else
                {
                    gridSizeX = 0;
                }
                break;
            case "SIZEY":
                if (gridSizeYInputField.text != "" && gridSizeYInputField.text != "-")
                {
                    gridSizeY = int.Parse(gridSizeYInputField.text);

                    if (gridSizeY < 0)
                    {
                        gridSizeY = 0;
                    }
                }
                else
                {
                    gridSizeY = 0;
                }
                break;
            case "SPACINGX":
                if (gridSpacingXInputField.text != "" && gridSpacingXInputField.text != "-")
                {
                    spacingSizeX = int.Parse(gridSpacingXInputField.text);

                    if (spacingSizeX < 0)
                    {
                        spacingSizeX = 0;
                    }
                }
                else
                {
                    spacingSizeX = 0;
                }
                break;
            case "SPACINGY":
                if (gridSpacingYInputField.text != "" && gridSpacingYInputField.text != "-")
                {
                    spacingSizeY = int.Parse(gridSpacingYInputField.text);

                    if (spacingSizeY < 0)
                    {
                        spacingSizeY = 0;
                    }
                }
                else
                {
                    spacingSizeY = 0;
                }
                break;
        }

        gridLayoutGroup.cellSize = new Vector2(gridSizeX, gridSizeY);
        gridLayoutGroup.spacing = new Vector2(spacingSizeX, spacingSizeY);
    }

    public void DisableGridsnap()
    {
        snappingEnabled = false;
        scriptManager.cursorHitObject.enabled = false;
    }

    public void EnableGridsnap()
    {
        snappingEnabled = true;
        scriptManager.cursorHitObject.enabled = true;
    }
}