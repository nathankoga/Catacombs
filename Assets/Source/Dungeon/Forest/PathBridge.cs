using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBridge : PathModelBase
{
    public override bool CanInitialize()
    {
        return !mapTile.isGround && mapTile.isPath;
    }
}
