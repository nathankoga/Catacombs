using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGod : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return (mapTile.isGround && (floor == DungeonFloor.FLOOR3)) || (mapTile.isExit);
    }

    public override void Initialize()
    {
    }
}
