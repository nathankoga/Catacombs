using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class PathModelBase : TileModelBase
{
    public override bool CanInitialize(DungeonFloor floor)
    {
        return this.mapTile.isPath = true;
    }
    public override void Initialize(DungeonFloor floor)
    {
        SetPathingVisibility();
    }

    protected void SetPathingVisibility()
    {
        if (!ShouldExist()) return;

        bool north = HasNorth();
        bool south = HasSouth();
        bool east = HasEast();
        bool west = HasWest();

        if (north || south || east || west)
        {
            // Create relevant pathing.
            transform.Find("North").transform.gameObject.SetActive(north);
            transform.Find("South").transform.gameObject.SetActive(south);
            transform.Find("East").transform.gameObject.SetActive(east);
            transform.Find("West").transform.gameObject.SetActive(west);

            // Figure out middle part.
            GameObject CORNER = transform.Find("PathCorner").transform.gameObject;
            GameObject JUNCTION = transform.Find("PathTJunction").transform.gameObject;
            GameObject END = transform.Find("PathEnd").transform.gameObject;
            GameObject FOURJUNCT = transform.Find("PathFourJunction").transform.gameObject;
            CORNER.SetActive(false);
            JUNCTION.SetActive(false);
            END.SetActive(false);
            FOURJUNCT.SetActive(false);

            FOURJUNCT.SetActive(true);
            return;

            // How many are active?
            int activeCount = Convert.ToInt16(north) + Convert.ToInt16(south) + Convert.ToInt16(east) + Convert.ToInt16(west);

            if (activeCount == 4)
            {
                FOURJUNCT.SetActive(true);
            } else if (activeCount == 3)
            {
                JUNCTION.SetActive(true);
            } else if (activeCount == 2)
            {
                CORNER.SetActive(true);
            }
            else if (activeCount == 1)
            {
                END.SetActive(true);
            }
        }
    }

    private bool ShouldExist()
    {
        return mapTile.isPath;
    }

    private bool HasNorth()
    {
        MapTile tile = mapTile.GetNorthTile();
        if (tile == null) return false;
        return tile.isPath;
    }

    private bool HasSouth()
    {
        MapTile tile = mapTile.GetSouthTile();
        if (tile == null) return false;
        return tile.isPath;
    }

    private bool HasEast()
    {
        MapTile tile = mapTile.GetEastTile();
        if (tile == null) return false;
        return tile.isPath;
    }

    private bool HasWest()
    {
        MapTile tile = mapTile.GetWestTile();
        if (tile == null) return false;
        return tile.isPath;
    }
}
