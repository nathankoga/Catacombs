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
    private bool hasAdjEnemy= false;
    private Vector2 adjEnemyLocation;
    private MapTile adjEnemyTile;
    
    
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

        // Initialize all tile children.
        foreach (var tileModel in tileModels)
        {
            if (tileModel.CanInitialize())
            {
                tileModel.Initialize();
            } else
            {
                tileModel.transform.gameObject.SetActive(false);
            }
            
        }
    }

    public bool hasAnEnemy(){
        return hasEnemy;
    }
    
    public bool adjacentToEnemy(){
        return hasAdjEnemy;

    }

    public EnemyType returnEnemy(){
        return this.enemyType;
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

    public void setAdjacentEnemy(Vector2 enemyPos){
    // public void setAdjacentEnemy(MapTile enemyTile){
        // This tile is adjacent to an enemy:
        // Therefore, when the player steps on this tile, trigger combat. 
        // Figure this is cheaper than calculating neighbors after EVERY movement input.
        hasAdjEnemy = true;
        adjEnemyLocation = enemyPos;
        // adjEnemyTile = enemyTile;
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
        return isGround || isPath;
    }

    public void OnStep(PlayerLogic player)
    {
        /*
         * Called when the Player lands on this tile.
         */
        
        if (hasEnemy )
        {
            // Start encounter.
            StartBattle(this, this.enemyType);
        }
        else if (hasAdjEnemy){

        //     StartBattle(adjEnemyTile, adjEnemyTile.enemyType);
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
