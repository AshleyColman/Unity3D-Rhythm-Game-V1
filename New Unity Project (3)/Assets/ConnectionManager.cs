using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectionManager : MonoBehaviour
{
    #region Variables
    // UI
    public TMP_Dropdown connectionDropdown;

    // Bools
    private bool connection;

    // Strings
    private const string ONLINE_MESSAGE = "CONNECTED ONLINE", OFFLINE_MESSAGE = "PLAYING OFFLINE";

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Properties
    public bool Connection
    {
        get { return connection; }
        set { connection = value; }
    }
    #endregion

    #region Functions
    private void Start()
    {
        // Reference
        scriptManager = FindObjectOfType<ScriptManager>();

        // Initialize 
        connection = true;
    }

    // Toggle connection
    public void ToggleConnection()
    {
        switch (connection)
        {
            case true:
                connection = false;
                scriptManager.uiColorManager.UpdateDropdownColors(connectionDropdown, scriptManager.uiColorManager.offlineColor08,
                scriptManager.uiColorManager.offlineColor09, scriptManager.uiColorManager.offlineColorSolid, scriptManager.uiColorManager.offlineColor08);
                scriptManager.messagePanel.DisplayMessage(OFFLINE_MESSAGE, scriptManager.uiColorManager.offlineColorSolid);
                break;
            case false:
                connection = true;
                scriptManager.uiColorManager.UpdateDropdownColors(connectionDropdown, scriptManager.uiColorManager.onlineColor08,
                scriptManager.uiColorManager.onlineColor09, scriptManager.uiColorManager.onlineColorSolid, scriptManager.uiColorManager.onlineColor08);
                scriptManager.messagePanel.DisplayMessage(ONLINE_MESSAGE, scriptManager.uiColorManager.onlineColorSolid);
                break;
        }
    }
    #endregion
}
