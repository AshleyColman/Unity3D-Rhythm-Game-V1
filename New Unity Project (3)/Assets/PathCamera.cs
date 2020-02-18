using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathCamera : MonoBehaviour
{
    #region Variables
    // UI
    public Image backgroundImage;

    // Integer
    private const int cameraPositionZ = -500, rhythmVisualizatorPositionZ = -450, rhythmVisualizatorPositionOffsetY = -460;

    // Scripts
    private ScriptManager scriptManager;
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        scriptManager = FindObjectOfType<ScriptManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.GameplaySceneIndex)
        {
            // Only update positions if the position has changed
            if (this.transform.position != new Vector3(scriptManager.follower.transform.localPosition.x, scriptManager.follower.transform.localPosition.y,
                cameraPositionZ))
            {
                // Set the camera position to follow the follower when moving along the line
                this.transform.position = new Vector3(scriptManager.follower.transform.localPosition.x, scriptManager.follower.transform.localPosition.y,
                    cameraPositionZ);

                // Set the background position to follow the follower when moving along the line
                backgroundImage.transform.localPosition = new Vector3(scriptManager.follower.transform.localPosition.x, scriptManager.follower.transform.localPosition.y,
                    0f);

                scriptManager.rhythmVisualizatorPro.transform.position = new Vector3(scriptManager.follower.transform.localPosition.x,
                    (scriptManager.follower.transform.localPosition.y + rhythmVisualizatorPositionOffsetY),
                    rhythmVisualizatorPositionZ);

                scriptManager.metronomePro.flashGlassAnimator.transform.position = new Vector3(scriptManager.follower.transform.localPosition.x,
                    scriptManager.follower.transform.localPosition.y,
                    0f);
            }
        }

        if (scriptManager.levelChanger.CurrentSceneIndex == scriptManager.levelChanger.EditorSceneIndex)
        {
            // Increase the camera position based on arrow keys
            MoveCameraPosition();
            ResetCameraPosition();
        }
    }

    // Increase the camera position based on arrow keys
    private void MoveCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.position = new Vector3((this.transform.position.x + 500), this.transform.position.y, cameraPositionZ);
            //backgroundImage.transform.localPosition = new Vector3((this.transform.position.x + 500), this.transform.position.y, 0f);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.position = new Vector3((this.transform.position.x - 500), this.transform.position.y, cameraPositionZ);
            //backgroundImage.transform.localPosition = new Vector3((this.transform.position.x + 500), this.transform.position.y, 0f);
        }
    }

    private void ResetCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y);
        }
    }

    #endregion
}
