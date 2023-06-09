using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGravel : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return mapTile.isGround && (floor == DungeonFloor.FLOOR2) && (!mapTile.isExit);
    }
    public override void Initialize(DungeonFloor floor)
    {
        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Rock01").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Rock02").gameObject.SetActive(false);
        }
    }
}
