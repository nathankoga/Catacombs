using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGravel : PathModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return mapTile.isGround && mapTile.isPath && (floor == DungeonFloor.FLOOR2) && (!mapTile.isExit);
    }
}
