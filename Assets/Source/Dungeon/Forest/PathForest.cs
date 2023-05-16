using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathForest : PathModelBase
{
    public override bool CanInitialize()
    {
        return mapTile.isGround && mapTile.isPath;
    }
}
