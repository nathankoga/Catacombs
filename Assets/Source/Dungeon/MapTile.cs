using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapTile : MonoBehaviour
{
    public delegate void StartBattleAction(MapTile mapTile, EnemyType enemyType);
    public static event StartBattleAction StartBattle;

    public GameObject EnemyPrefab;

    public bool isWall;
    public bool isGround;

    // Enemy at this tile?
    private bool hasEnemy = false;
    private GameObject enemyObject;
    private EnemyType enemyType;

    /*
     * Configuration Methods
     */

    public void Initialize()
    {
        /*
         * Called by the DungeonManager after the Tile's options are fully configured.
         */
        if (isWall)
        {
            // BECOME WALL!
            transform.localScale = new Vector3(3, 5, 3);
            transform.Translate(0, 2, 0);
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
}
