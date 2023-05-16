using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileModelBase : MonoBehaviour
{
    public MapTile mapTile;

    void Awake()
    {
        mapTile.RegisterTileModel(this);
    }
    abstract public bool CanInitialize();
    abstract public void Initialize();
}
