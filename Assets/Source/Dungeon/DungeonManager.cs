using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour, IManager
{
    /*
     * The DungeonManager.
     * In charge of generating and cleaning up the dungeon.
     * (This class exists between floor changes.)
     */

    public DungeonGUI GUI;
    public MapTile[,] map;  // [,] initializes a 2D array
    public int mapSize;

    void Start()
    {
        GameState.FloorStart += GenerateDungeon;
    }

    public void StartManager()
    {
        GUI.EnableGUI();
    }

    public void StopManager()
    {
        GUI.DisableGUI();
    }

    public ManagerType GetManagerType()
    {
        return ManagerType.DUNGEON;
    }

    /*
     * Handle Dungeon Generation
     */

    void GenerateDungeon(GameState gs, DungeonFloor f)
    {
        // TODO: dungeon generation here
        map = new MapTile[mapSize, mapSize];  // 3x3 for PoC build
        
        for (int i = 0; i < mapSize; i++){            
            for (int j = 0; j < mapSize; j++){
                MapTile tile = new MapTile();  // new tile for map
                tile.isGround = true;          // instantiate as ground
                map[i,j] = tile; 
            }
        }


        // Tell the GameState to generate us.
        gs.RequestManager(this);
    }
}
