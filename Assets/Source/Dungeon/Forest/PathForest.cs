using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathForest : PathModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return mapTile.isGround && mapTile.isPath && (floor == DungeonFloor.FLOOR1) && (!mapTile.isExit);
    }
}
