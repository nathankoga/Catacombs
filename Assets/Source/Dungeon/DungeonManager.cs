using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour, IManager
{
    /*
     * The DungeonManager.
     * In charge of generating and cleaning up the dungeon.
     * (This class exists between floor changes.)
     */

    public DungeonGUI GUI;
    static public MapTile[,] map;  // [,] initializes a 2D array
    static int mapSize = 25;

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
        // Generate bare map grid.
        map = new MapTile[mapSize, mapSize];
        
        for (int i = 0; i < mapSize; i++){            
            for (int j = 0; j < mapSize; j++){
                // Create tile object.
                Vector2 tilePosition = new Vector2(i, j);
                Vector3 position = GetWorldTilePosition(tilePosition);
                GameObject currTile = Instantiate(TilePrefab, position, Quaternion.identity);
                currTile.transform.parent = dungeonParent.transform;

                // Store MapTile script.
                map[i,j] = currTile.GetComponent<MapTile>(); 
            }
        }

        // POC dungeon generation
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                MapTile t = map[i,j];
                if ((i == 1 || i == 3) && (j == 1 || j == 3))
                {
                    t.isWall = true;
                } else
                {
                    t.isGround = true;
                }
            }
        }

        map[4, 2].SetEnemy(EnemyType.Test);
        map[2, 3].SetEnemy(EnemyType.Test);

        // Initialize all tiles.
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                map[i, j].Initialize(new Vector2(i, j));
            }
        }

        // Send dungeon generated event.
        DungeonGenerated(map, mapSize);
    }
}
