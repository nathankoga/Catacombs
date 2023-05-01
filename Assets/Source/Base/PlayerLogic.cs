using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    /*
     * This class is in charge of the player logic.
     * It listens to the manager update to decide if it is in a state that can move,
     * and if it is in a movable state, it will respond to any WASD/arrow key movements.
     * 
     * It will need to interact with the DungeonManager to figure out where valid tiles are.
     * When the player moves onto a tile with an enemy, it will need to tell the
     * BattleManager about that and what that enemy is, and the BattleManager will engage combat.
     */

    public Vector2 tilePosition = Vector2.zero;
    public Vector3 worldOffset;

    public InputAction moveAction;

    private void Start()
    {
        LerpToWorldPos(1.0f);
    }

    /*
     * Update stuffs
     */

    private void FixedUpdate()
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON)
        {
            LerpToWorldPos(0.25f);
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
        return DungeonManager.GetWorldTilePosition(tilePosition) + worldOffset;
    }

    /*
     * Input Actions
     */

    private void OnMove(InputValue movementValue)
    {
        // Get movement vector (and normalize it a bit too)
        // TODO : Make the input registering less buggy.
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementVector.x = (float)Mathf.Round(movementVector.x);
        movementVector.y = (float)Mathf.Round(movementVector.y);
        if (GameState.GetManagerType() == ManagerType.DUNGEON)
        {
            // print(movementVector);

            // What is our new tile position?
            Vector2 newPos = tilePosition + movementVector;
            MapTile newTile = DungeonManager.GetTileAtPos(newPos);
            if (newTile != null)
            {
                if (newTile.CanMoveOnto()) {
                    tilePosition = newPos;
                }
            }
        }
    }
}
