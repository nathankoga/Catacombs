using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStats : MonoBehaviour
{
    /*
     * This module will be in charge of all RunStats.
     * Basically, this refers to persistent information about player stats,
     * current floor, etc global "run" variables.
     */

    public DungeonFloor currentFloor = DungeonFloor.FLOOR3;
    public PlayerBattleEntityStats playerStats;


    void Start(){
        playerStats = new PlayerBattleEntityStats(3, 0);
    }

    PlayerBattleEntityStats get_stats(){
        return playerStats;
    }
}
