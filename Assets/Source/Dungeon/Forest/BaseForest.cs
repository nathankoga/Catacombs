using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseForest : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return mapTile.isGround && (floor == DungeonFloor.FLOOR1) && (!mapTile.isExit);
    }
    public override void Initialize(DungeonFloor floor)
    {
        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Tree01").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Tree02").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 40)
        {
            transform.Find("Flower01").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 40)
        {
            transform.Find("Flower02").gameObject.SetActive(false);
        }
    }
}
