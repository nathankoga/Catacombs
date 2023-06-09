using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGod : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return (mapTile.isGround && (floor == DungeonFloor.FLOOR3)) || (mapTile.isExit);
    }

    public override void Initialize(DungeonFloor floor)
    {
        if (floor != DungeonFloor.FLOOR3) {
            // This is an exit tile, do cool exit visuals here
            return;
        }

        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Rock01").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 70)
        {
            transform.Find("Rock02").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 40)
        {
            transform.Find("Rock03").gameObject.SetActive(false);
        }
        if (Random.Range(0, 100) < 40)
        {
            transform.Find("Rock04").gameObject.SetActive(false);
        }
    }
}
