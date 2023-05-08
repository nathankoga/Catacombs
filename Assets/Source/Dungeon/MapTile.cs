using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapTile : MonoBehaviour
{
    public delegate void StartBattleAction(MapTile mapTile, EnemyType enemyType);
    public static event StartBattleAction StartBattle;

    public GameObject EnemyPrefab;

    public bool isWall;
    public bool isGround;
    public bool isPath;

    public Vector2 tilePos;
    public List<TileModelBase> tileModels;
    private List<MapTile> adjacentTiles;

    // Enemy at this tile?
    private bool hasEnemy = false;
    private GameObject enemyObject;
    private EnemyType enemyType;

    private void Awake()
    {
        tileModels = new List<TileModelBase>();
    }

    /*
     * Configuration Methods
     */

    public void Initialize(Vector2 tilePos, List<MapTile> adjacentTiles)
    {
        /*
         * Called by the DungeonManager after the Tile's options are fully configured.
         */
        this.tilePos = tilePos;
        this.adjacentTiles = adjacentTiles;
        print(adjacentTiles.Count);

        if (isWall)
        {
            // BECOME WALL!
            transform.localScale = new Vector3(0, 0, 0);
        }

        // Initialize all tile children.
        foreach (var tileModel in tileModels)
        {
            tileModel.Initialize();
        }
    }
    public void SetEnemy(EnemyType enemyType)
    {
        this.enemyType = enemyType;
        hasEnemy = true;

        // Create an Enemy Prefab associated with this tile.
        enemyObject = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        enemyObject.transform.parent = this.transform;
        DungeonEnemy dungeonEnemy = enemyObject.GetComponent<DungeonEnemy>();
        dungeonEnemy.Initialize(this, enemyType);
    }

    /*
     * Active State Methods
     */

    public void ClearEnemy()
    {
        hasEnemy = false;
        Destroy(enemyObject);
    }

    /*
     * Registering 
     */

    public void RegisterTileModel(TileModelBase tmb)
    {
        tileModels.Add(tmb);
    }

    /*
     * Getters
     */

    public bool CanMoveOnto()
    {
        /*
         * Called by the Player to determine if this can be moved onto.
         */
        return isGround;
    }

    public void OnStep(PlayerLogic player)
    {
        /*
         * Called when the Player lands on this tile.
         */
        if (hasEnemy)
        {
            // Start encounter.
            StartBattle(this, this.enemyType);
        }
    }

    public MapTile GetNorthTile()
    {
        return adjacentTiles[0];
    }

    public MapTile GetSouthTile()
    {
        return adjacentTiles[1];
    }

    public MapTile GetEastTile()
    {
        return adjacentTiles[2];
    }

    public MapTile GetWestTile()
    {
        return adjacentTiles[3];
    }
}
