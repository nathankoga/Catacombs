using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    /*
     * This class is in charge of the camera logic.
     * When the current manager type is set below, the camera should move
     * to the ideal position for the environment.
     */

    public GameObject player;
    public Vector3 worldOffset;
    private void FixedUpdate()
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON)
        {
            LerpToWorldPos(0.08f);
        }
    }

    /*
     * Positioning
     */

    private void LerpToWorldPos(float t)
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetWorldPos(), t);
    }

    public Vector3 GetTargetWorldPos()
    {
        return player.transform.position + worldOffset;
    }
}
