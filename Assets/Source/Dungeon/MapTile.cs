using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public bool isWall;
    public bool isGround;

    public void Initialize()
    {
        /*
         * Called by the DungeonManager after the Tile's options are fully configured.
         */
        if (isWall)
        {
            // BECOME WALL!
            transform.localScale = new Vector3(3, 5, 3);
            transform.Translate(0, 2, 0);
        }
    }

    public bool CanMoveOnto()
    {
        /*
         * Called by the Player to determine if this can be moved onto.
         */
        return isGround;
    }
}
