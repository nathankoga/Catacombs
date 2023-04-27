using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        GameState.ManagerUpdate += OnManagerUpdate;
    }

    void OnManagerUpdate(ManagerType type, IManager mgr)
    {
        if (type == ManagerType.DUNGEON)
        {
            // Enable tile movement.
        } else
        {
            // Disable tile movement.
        }
    }
}
