using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DungeonManager : MonoBehaviour, IManager
{
    /*
     * The DungeonManager.
     * In charge of generating and cleaning up the dungeon.
     * (This class exists between floor changes.)
     */

    public DungeonGUI GUI;
    static public MapTile[,] map;  // [,] initializes a 2D array
    static int mapSize = 5;

    public GameObject dungeonParent;
    public GameObject TilePrefab;
    static float xTileOffset = 5.5f;
    static float yTileOffset = 7.0f;

    // Game Events
    public delegate void DungeonGeneratedAction(MapTile[,] map, int mapSize);
    public static event DungeonGeneratedAction DungeonGenerated;

    void Awake()
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
     * Useful Dungeon Getters
     */

    static public Vector3 GetWorldTilePosition(Vector2 tilePosition)
    {
        return new Vector3(tilePosition.x * DungeonManager.xTileOffset, 0, tilePosition.y * DungeonManager.yTileOffset);
    }

    static public MapTile GetTileAtPos(Vector2 tilePosition)
    {
        if (tilePosition.x < 0 || tilePosition.y < 0) { return null; }
        if (tilePosition.x >= mapSize || tilePosition.y >= mapSize) { return null; }
        return map[(int)tilePosition.x, (int)tilePosition.y];
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

        // Generate the map tiles.
        CreateTiles(map, mapSize);

        // Send dungeon generated event.
        DungeonGenerated(map, mapSize);

        // Tell the GameState to generate us.
        gs.RequestManager(this);
    }

    void CreateTiles(MapTile[,] map, int mapSize)
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Vector3 position = GetWorldTilePosition(tilePosition);
                print(position);
                GameObject currTile = Instantiate(TilePrefab, position, Quaternion.identity);
                currTile.transform.parent = dungeonParent.transform;
                // Instantiate creates a copy of a prefab at the given location

            }
        }
    }
}
