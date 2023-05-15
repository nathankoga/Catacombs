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
    static public MapTile[,] map;  // [,] initializes a 2D array of map tiles
    static public RoomManager roomManager; 

    // current map stats: Might want to refactor when we make new dungeons??
    static int mapSize = 25;
    static int roomCount = 7;
    static int minRoomSize = 3;
    static int maxRoomSize = 5;

    public GameObject dungeonParent;
    public GameObject TilePrefab;
    static float xTileOffset = 5.5f;
    static float yTileOffset = 5.5f;

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

        /*START OF RANDOM GEN CODE */

        // take each room (7 rooms), and continue building them until they don't overlap
        // then draw paths to connect rooms together
        // in the future, might explicitly build starting room -> same tutorial room each time (like binding of isaac start)

        // for now, a quick representation of room placements in map as int vectors  

        // roomManager.setRooms();  // don't know how setRooms will work when generating mutliple floors??
        // roomManager.getFinalRooms();        
        
        /*END OF RANDOM DUNGEON GEN CODE*/
        
        // POC dungeon generation
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                MapTile t = map[i,j];
                t.isGround = true;
                // t.isPath = Convert.ToBoolean(i != 1);

                if (Random.Range(0.0f, 1.0f) < 0.60f)
                {
                    // setting isPath to true makes the current tile into one of path
                    t.isPath = true;
                }
            }
        }

        //map[4, 2].SetEnemy(EnemyType.Test);
        //map[2, 3].SetEnemy(EnemyType.Test);

        // Initialize all tiles.
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                List<MapTile> adjacentTiles = new List<MapTile>
                {
                    // instantiate neighbors. If at edge -> neighbor is null
                    (j != (mapSize - 1)) ? (map[i, j + 1]) : (null),
                    (j != 0) ? (map[i, j - 1]) : (null),
                    (i != (mapSize - 1)) ? (map[i + 1, j]) : (null),
                    (i != 0) ? (map[i - 1, j]) : (null)
                };
                map[i, j].Initialize(new Vector2(i, j), adjacentTiles);
            }
        }

        // Send dungeon generated event.
        DungeonGenerated(map, mapSize);
    }
}
