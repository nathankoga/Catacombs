using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGod : PathModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return mapTile.isGround && mapTile.isPath && (floor == DungeonFloor.FLOOR3) && (!mapTile.isExit);
    }
}
