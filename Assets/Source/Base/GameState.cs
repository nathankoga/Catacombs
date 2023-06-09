using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    /*
     * This class holds all of the information regarding the current game state.
     * It has a FSM-esque interface for swapping between Managers, which
     * manage all of the seperate "state" elements of the game.
     * 
     * For example, when the DungeonManager is engaged, dungeon exploration
     * is supported. When BattleManager is engaged, the game will be managing
     * the turn-based battle component. Each Manager has their own GUI objects
     * that they manage to support GUI transitions between states.
     */

    public RunStats runStats;
    public BattleManager battleManager;
    public DungeonManager dungeonManager;
    public StageTransitionManager stageTransitionManager;

    // Game Manager State
    static protected IManager ActiveManager = null;

    // Game Events
    public delegate void FloorStartAction(GameState gs, DungeonFloor f);
    public static event FloorStartAction FloorStart;

    public delegate void ManagerUpdateAction(ManagerType type, IManager mgr);
    public static event ManagerUpdateAction ManagerUpdate;

    void Start()
    {
        FloorStart(this, runStats.currentFloor);
        RequestManager(dungeonManager);
    }
    public void RequestManager(IManager mgr)
    {
        if (ActiveManager != null)
            ActiveManager.StopManager();
        ActiveManager = mgr;
        ActiveManager.StartManager();

        if (ManagerUpdate != null)
            ManagerUpdate(ActiveManager.GetManagerType(), ActiveManager);
    }

    static public ManagerType GetManagerType()
    {
        return ActiveManager.GetManagerType();
    }

    public void GotoNextFloor()
    {
        // Restore player lives (Can remove this depending on balance)
        //runStats.playerStats.lives = 9;

        DungeonFloor nextFloor = DungeonFloor.FLOOR1;
        if (runStats.currentFloor == DungeonFloor.FLOOR1) nextFloor = DungeonFloor.FLOOR2;
        else if (runStats.currentFloor == DungeonFloor.FLOOR2) nextFloor = DungeonFloor.FLOOR3;
        else return;
        runStats.currentFloor = nextFloor;
        FloorStart(this, nextFloor);
        RequestManager(dungeonManager);
    }
}
