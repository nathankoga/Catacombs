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

    public DungeonFloor floor;
    public bool isWall;
    public bool isGround;
    public bool isPath;
    public bool isExit;

    public Vector2 tilePos;
    public List<TileModelBase> tileModels;
    private List<MapTile> adjacentTiles;

    // Enemy at this tile?
    private bool hasEnemy = false;
    private bool hasAdjEnemy= false;
    // private int numAdjEnemy= 0;
    private Vector2 adjEnemyLocation;
    
    private MapTile adjEnemyTile;
    
    
    private GameObject enemyObject;
    private EnemyType enemyType;
    private GameState gs;

    private void Awake()
    {
        tileModels = new List<TileModelBase>();
    }

    /*
     * Configuration Methods
     */

    public void Initialize(Vector2 tilePos, List<MapTile> adjacentTiles, GameState gs)
    {
        /*
         * Called by the DungeonManager after the Tile's options are fully configured.
         */
        this.gs = gs;
        this.tilePos = tilePos;
        this.adjacentTiles = adjacentTiles;

        // Initialize all tile children.
        foreach (var tileModel in tileModels)
        {
            if (tileModel.CanInitialize(floor))
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

    public GameObject referenceEnemyObject(){
        return enemyObject;
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

    public void setAdjacentEnemy(MapTile enemyTile){
        // This tile is adjacent to an enemy:
        // Therefore, when the player steps on this tile, trigger combat. 
        // Figure this is cheaper than calculating neighbors after EVERY movement input.
        hasAdjEnemy = true;
        adjEnemyTile = enemyTile;  // set reference to MapTile object that has enemy on it
    }

    /*
     * Active State Methods
     */

    public void ClearEnemy()
    {
        hasEnemy = false;
        Destroy(enemyObject);
        
        if (enemyType == EnemyType.FloorBoss){
            // if we encounter boss, find reference to player GameObject, then set flag to true
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerLogic>().bossDefeated = true;
        }
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
        
        if (hasEnemy)
        {
            // Start encounter.
            StartBattle(this, this.enemyType);
        }
        else if (hasAdjEnemy && adjEnemyTile.hasAnEnemy()){
            // pass in a reference to the tile that stores the enemy's data, and start the battle with that enemy
            // if the enemy has already been beaten, we turn off the hasAnEnemy flag so that we dont continue to "enter battle" vs nothing 
            StartBattle(adjEnemyTile, adjEnemyTile.enemyType);
           

            // // because tiles may have multiple neighbor enemies, change to a list of tiles with enemies on them so that we can continue to collide
            // MapTile poppedTile = adjEnemyTiles.Pop();
            // StartBattle(poppedTile, poppedTile.enemyType);
        }

        // If this is an exit, trigger the next floor.
        // TODO : affirm that the player has deleted the boss
        if (isExit && player.bossDefeated)
        {
            gs.RequestManager(gs.stageTransitionManager);
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
