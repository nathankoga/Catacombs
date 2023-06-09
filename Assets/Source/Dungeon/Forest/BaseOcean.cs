using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOcean : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return !mapTile.isGround && (floor == DungeonFloor.FLOOR1) && (!mapTile.isExit);
    }
    public override void Initialize(DungeonFloor floor)
    {
        transform.Find("Lilypad01").rotation.Set(0, Random.Range(0, 360), 0, 0);
        transform.Find("Lilypad02").rotation.Set(0, Random.Range(0, 360), 0, 0);
        if (Random.Range(0, 100) < 85)
        {
            transform.Find("Lilypad01").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 85)
        {
            transform.Find("Lilypad02").gameObject.SetActive(false);
        }
    }
}
