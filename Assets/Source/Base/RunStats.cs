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

    public DungeonFloor currentFloor = DungeonFloor.FLOOR1;
    public BattleEntityStats playerStats;

}
