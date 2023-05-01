using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour, IManager
{
    /*
     * This class is in charge of the entire battle system.
     * Um. We'll figure this out later...
     * This will be fun :) -Sheep
     */

    public GameState gameState;
    public DungeonManager dungeonManager;
    public BattleGUI GUI;

    /*
     * Battle State
     */

    bool battleActive = false;
    private MapTile mapTile;
    private EnemyType enemyType;

    /*
     * Battle Requesting
     */

    void Awake()
    {
        MapTile.StartBattle += EngageBattle;
    }

    void EngageBattle(MapTile mapTile, EnemyType enemyType)
    {
        this.mapTile = mapTile;
        this.enemyType = enemyType;
        gameState.RequestManager(this);
    }

    /*
     * Battle Input
     */

    private void OnKill(InputValue inputValue)
    {
        print(inputValue);
        if (battleActive)
        {
            mapTile.ClearEnemy();
            gameState.RequestManager(dungeonManager);
        }
    }

    /*
     * IManager Transitions
     */

    public void StartManager()
    {
        battleActive = true;
        GUI.EnableGUI();
    }

    public void StopManager()
    {
        battleActive = false;
        GUI.DisableGUI();
    }

    public ManagerType GetManagerType()
    {
        return ManagerType.BATTLE;
    }

}
