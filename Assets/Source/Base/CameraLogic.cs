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
    public PlayerLogic pl;
    public DungeonManager dungeonManager;
    public Vector3 worldOffset;
    private void FixedUpdate()
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON || true)
        {
            LerpToWorldPos(0.08f);
        }
    }


    private void MoveToBattle(){
        // possibly implement this
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
        // Is the player in a room?
        /*
        Room room = dungeonManager.GetRoomAtPos(pl.tilePosition);
        if (room != null)
        {
            // If so, plant the camera around the whole room.
            Vector3 newWorldOffset = worldOffset * ((float)room.getHeight()) / 1.8f;
            return worldOffset + DungeonManager.GetWorldTilePosition(room.GetCenter());
        }
        */

        // Follow the player's position.
        return player.transform.position + worldOffset;
    }
}
