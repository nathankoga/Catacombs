using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour, IManager
{
    /*
     * The DungeonManager.
     * In charge of generating and cleaning up the dungeon.
     * (This class exists between floor changes.)
     */

    public MusicService musicService;
    public DungeonGUI GUI;
    static public GameObject[,] gomap = null;
    static public MapTile[,] map;  // [,] initializes a 2D array of map tiles
    public RoomManager roomManager;   
    public List<Room> rooms;
    public PathTree paths = new PathTree();

    public PlayerLogic player;

    // current map stats: Might want to refactor when we make new dungeons??
    static int cachedMapSize;
    static int minRoomSize = 3;
    static int maxRoomSize = 5;

    public GameObject dungeonParent;
    public GameObject TilePrefab;
    public static float xTileOffset = 5.0f;
    public static float yTileOffset = 5.0f;

    public DungeonFloor floor;
    public GameState gs;

    // Game Events
    public delegate void DungeonGeneratedAction(MapTile[,] map, int mapSize);
    public static event DungeonGeneratedAction DungeonGenerated;

    private bool firstMove = true;

    void Awake()
    {
        GameState.FloorStart += GenerateDungeon;
        player.bossDefeated = false;  // new floor, so reset boss flag
    }

    public void StartManager()
    {
        if (firstMove){
            GUI.EnableGUI();
            firstMove = false;
        }
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
        if (tilePosition.x >= cachedMapSize || tilePosition.y >= cachedMapSize) { return null; }
        return map[(int)tilePosition.x, (int)tilePosition.y];
    }

    public Room GetRoomAtPos(Vector2 tilePosition)
    {
        // Iterate over each room.
        foreach (Room room in rooms)
        {
            // Check if the tile is in the room.
            if (room.ContainsTile(tilePosition))
            {
                return room;
            }
        }
        return null;
    }

    /*
     * Dungeon Consts
     */

    int GetRoomCount()
    {
        if (floor == DungeonFloor.FLOOR2) return 7;
        if (floor == DungeonFloor.FLOOR3) return 9;
        return 5;
    }

    int GetMapSize()
    {
        if (floor == DungeonFloor.FLOOR2) return 25;
        if (floor == DungeonFloor.FLOOR3) return 50;
        return 20;

    }

    /*
     * Handle Dungeon Generation
     */

    void GenerateDungeon(GameState gs, DungeonFloor f)
    {
        // Clean up old maptiles.
        if (gomap != null)
        {
            for (int i = 0; i < cachedMapSize; i++)
            {
                for (int j = 0; j < cachedMapSize; j++)
                {
                    Destroy(gomap[i, j]);
                }
            }
        }

        // Generate bare map grid.
        floor = f;
        int mapSize = GetMapSize();
        cachedMapSize = mapSize;
        map = new MapTile[mapSize, mapSize];
        gomap = new GameObject[mapSize, mapSize];
        
        for (int i = 0; i < mapSize; i++){            
            for (int j = 0; j < mapSize; j++){
                // Create tile object.
                Vector2 tilePosition = new Vector2(i, j);
                Vector3 position = GetWorldTilePosition(tilePosition);
                GameObject currTile = Instantiate(TilePrefab, position, Quaternion.identity);
                currTile.transform.parent = dungeonParent.transform;

                // Store MapTile script.
                gomap[i, j] = currTile;
                map[i,j] = currTile.GetComponent<MapTile>();

                // Initialize this tile.
                map[i, j].floor = floor;
                map[i, j].isGround = false;
            }
        }

        /*START OF RANDOM GEN CODE */

        // take each room (7 rooms), and continue building them until they don't overlap
        // then draw paths to connect rooms together
        // in the future, might explicitly build starting room -> same tutorial room each time (like binding of isaac start)

        // for now, a quick representation of room placements in map as int vectors  

        // hacky constructor because idk how to init objects in C# 
        int roomCount = GetRoomCount();
        roomManager.initRoomManager(mapSize, roomCount, minRoomSize, maxRoomSize); 
        
        roomManager.setRooms();  // don't know how setRooms will work when generating mutliple floors??
        rooms = roomManager.getFinalRooms();  
        
        // Create paths.
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms.Count; j++)
            {
                if (i == j) continue;
                if (j < i) continue;
                DungeonPath path = new DungeonPath(rooms[i], rooms[j]);
                paths.AddPath(path);
            }
        }

        // Cull paths.
        paths.Cull(rooms, 0.0f);


        for (int curr = 0; curr < roomCount; curr++){
            int xStart = rooms[curr].getMinX();
            int xEnd= rooms[curr].getMaxX();
            int yStart = rooms[curr].getMinY();
            int yEnd= rooms[curr].getMaxY();

            // possibly an off-by one error 
            for (int i = xStart; i < xEnd; i++){
                for (int j = yStart; j < yEnd; j++){
                        MapTile t = map[i,j];
                        t.isGround = true;
                }
            }
        }

        List<DungeonPath> realPaths = paths.GetPath();
        for (int i = 0; i < realPaths.Count; i++)
        {
            List<Vector2Int> path = realPaths[i].GetPath();
            for (int j = 0; j < path.Count; j++)
            {
                Vector2Int curr = path[j];
                MapTile t = map[curr.x, curr.y];
                t.isPath = true;

                // In Floor 2/3, the path is also the ground.
                if (floor != DungeonFloor.FLOOR1)
                    t.isGround = true;

                // In floor 2, adjacent tiles become ground.
                if (floor == DungeonFloor.FLOOR2)
                {
                    int x = curr.x;
                    int y = curr.y;
                    if (x != 0) map[x - 1, y].isGround = true;
                    if (x != (mapSize - 1)) map[x + 1, y].isGround = true;
                    if (y != 0) map[x, y - 1].isGround = true;
                    if (y != (mapSize - 1)) map[x, y + 1].isGround = true;
                }
            }
        }

        // In floor 2, spawn a full gravel ring around the dungeon.
        if (floor == DungeonFloor.FLOOR2)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if ((i != 0) && (i != (mapSize - 1)) && (j != 0) && (j != (mapSize - 1)))
                        continue;
                    map[i, j].isGround = true;
                }
            }
        }

        // Random room has an exit point at its origin.
        if (floor != DungeonFloor.FLOOR3)
        {
            while (true)
            {
                int randomRoomIndex = Random.Range(1, rooms.Count);
                Room exitRoom = rooms[randomRoomIndex];
                Vector2Int exitPos = exitRoom.GetRandomInBounds();
                if (map[exitPos.x, exitPos.y].isGround == false) continue;
                map[exitPos.x, exitPos.y].isExit = true;
                map[exitPos.x, exitPos.y].isPath = false;
                map[exitPos.x, exitPos.y].isGround = true;
                break;
            }
        }

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
                map[i, j].Initialize(new Vector2(i, j), adjacentTiles, gs);
            }
        }

        // Put player in first room.
        Vector2Int playerPos = rooms[0].GetRandomInBounds();
        player.SetPosition((Vector2)playerPos);

        // before spawning enemies in other rooms, pick room (approx.) farthest from spawn, to place boss 
        int boss_room_idx = 0;
        float boss_room_dist = 0;

        for (int idx = 1; idx < roomCount; idx++){
            float dist = Vector2.Distance(playerPos, rooms[idx].GetRandomInBounds());
            if (dist > boss_room_dist){
                boss_room_dist = dist;
                boss_room_idx = idx;
            }
        }
        
        // Boss spawn
        Vector2Int bossPos = rooms[boss_room_idx].GetRandomInBounds();
        int bx = bossPos.x;
        int by = bossPos.y;

        if (floor == DungeonFloor.FLOOR1){
            map[bx,by].SetEnemy(EnemyType.Floor1Boss);
        }
        else if (floor == DungeonFloor.FLOOR2){
            map[bx,by].SetEnemy(EnemyType.Floor2Boss);
        }
        else if (floor == DungeonFloor.FLOOR3){
            map[bx,by].SetEnemy(EnemyType.Floor3Boss);
        }

        player.bossDefeated = false;  // flag not getting reset in the awake() function
        for (int row = -1; row < 2; row++){
            for (int col = -1; col < 2; col++){
                if (( 0 <= (bx + row) && (bx + row) <= mapSize -1 ) && (0 <= (by+col) && (by+col) <= mapSize -1)){
                    map[bx + row, by + col].setAdjacentEnemy();
                } 
            }
        }

        
        // spawn enemies in other rooms
        for (int idx = 1; idx < roomCount; idx++){
            // random number of enemies (2~3)
            if (idx == boss_room_idx){
                continue;
            }
            // break;  // for toggling enemy spawns
            int numEnemies = Random.Range(2,4);

            for (int z = 0; z < numEnemies; z++){
                bool enemyNotPlaced = true;
                int attempts = 5;
                
                while (enemyNotPlaced){
                    Vector2Int enemyPos = rooms[idx].GetRandomInBounds();
                    int x = enemyPos.x; 
                    int y = enemyPos.y; 
                    
                    if (!map[x,y].hasAnEnemy() && !map[x,y].adjacentToEnemy()){
                        
                        if (floor == DungeonFloor.FLOOR1){
                            map[x,y].SetEnemy(EnemyType.Floor1);
                        }
                        else if (floor == DungeonFloor.FLOOR2){
                            map[x,y].SetEnemy(EnemyType.Floor2);
                        }
                        else if (floor == DungeonFloor.FLOOR3){
                            map[x,y].SetEnemy(EnemyType.Floor3);
                        }

                        // map[x,y].SetEnemy(EnemyType.Test);
                        enemyNotPlaced = false;

                        for (int row = -1; row < 2; row++){
                            for (int col = -1; col < 2; col++){

                                // set adjacent tiles to tiles that see the enemy at location enemyPos
                                if (( 0 <= (x+row) && (x+row) <= mapSize -1) && (0 <= (y +col) && (y+col) <= mapSize -1)){
                                    // for valid positions, set the tile s.t. it has an adjacent enemy  (this prevent indexing errors)
                                    // map[x + row, y + col].setAdjacentEnemy(enemyPos);
                                    map[x + row, y + col].setAdjacentEnemy();
                                } 
                            }
                        }
                    }

                    attempts -= 1;
                    if (attempts < 0) break;
                }
            }
        }

        // Send dungeon generated event.
        musicService.RequestFloorTheme(floor);
        DungeonGenerated(map, mapSize);
    }
}


public class DungeonPath
{
    public Room roomA;
    public Room roomB;
    private List<Vector2Int> vecPath = new List<Vector2Int>();
    public DungeonPath(Room roomA, Room roomB)
    {
        this.roomA = roomA;
        this.roomB = roomB;
        this.CalculateVectorPath();
    }

    public int Distance()
    {
        return vecPath.Count;
    }

    private void CalculateVectorPath()
    {
        Vector2Int start = roomA.pathOrigin;
        Vector2Int end = roomB.pathOrigin;
        Vector2Int curr = start;
        vecPath.Add(curr);

        while (curr.x != end.x)
        {
            int direction = (end.x > curr.x) ? 1 : -1;
            curr = curr + new Vector2Int(direction, 0);
            vecPath.Add(curr);
        }

        while (curr.y != end.y)
        {
            int direction = (end.y > curr.y) ? 1 : -1;
            curr = curr + new Vector2Int(0, direction);
            vecPath.Add(curr);
        }
    }

    public List<Vector2Int> GetPath()
    {
        return vecPath;
    }
}


public class PathTree
{
    List<DungeonPath> dungeonPaths = new List<DungeonPath>();

    public void AddPath(DungeonPath path)
    {
        dungeonPaths.Add(path);
    }

    public List<DungeonPath> GetPath() { return dungeonPaths; }

    public void Cull(List<Room> rooms, float bonusPathRatio)
    {
        // Sort by length.
        dungeonPaths.Sort((x, y) => x.Distance().CompareTo(y.Distance()));

        // Set up consts.
        List<DungeonPath> newPath = new List<DungeonPath>();
        List<DungeonPath> extraPaths = new List<DungeonPath>();

        // Algo consts/funcs.
        int i = 0;
        int vertices = rooms.Count;
        List<int> parent = new List<int>();
        for (int j = 0; j < vertices; j++) parent.Add(j);
        List<int> rank = new List<int>();
        for (int j = 0; j < vertices; j++) rank.Add(0);

        int Find(int i)
        {
            if (parent[i] != i) parent[i] = Find(parent[i]);
            return parent[i];
        }

        void Union(int x, int y)
        {
            int xRoot = Find(x);
            int yRoot = Find(y);
            if (xRoot == yRoot) return;
            if (rank[xRoot] < rank[yRoot]) parent[xRoot] = yRoot;
            else if (rank[yRoot] < rank[xRoot]) parent[yRoot] = xRoot;
            else
            {
                parent[yRoot] = xRoot;
                rank[xRoot] = rank[xRoot] + 1;
            }
        }

        // Kruskal's algorithm.
        while (newPath.Count < vertices - 1)
        {
            DungeonPath curr = dungeonPaths[i];
            int x = rooms.IndexOf(curr.roomA);
            int y = rooms.IndexOf(curr.roomB);
            int xRoot = Find(x);
            int yRoot = Find(y);
            if (xRoot != yRoot)
            {
                newPath.Add(curr);
                Union(xRoot, yRoot);
            } else
            {
                extraPaths.Add(curr);
            }
            i++;
        }

        // Add the rest of the extra paths.
        for (int p = i; p < dungeonPaths.Count; p++)
        {
            extraPaths.Add(dungeonPaths[p]);
        }

        int bonusPathCount = (int)(extraPaths.Count * bonusPathRatio);
        for (int p = 0; p < bonusPathCount; p++)
        {
            newPath.Add(extraPaths[p]);
        }

        // Ok its gamer time
        this.dungeonPaths = newPath;
    }
}
