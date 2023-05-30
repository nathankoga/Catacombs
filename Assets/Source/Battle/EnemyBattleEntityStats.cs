using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleEntityStats : MonoBehaviour
{
    /*
     * A dataclass for holding and manipulating enemy entity data.
     */
    public EnemyType type;

    public int health = 1;

    // default battle attributes
    public int ferocity = 1;
    public int stubbornness = 1;
    public int precision = 1;
    public int grace = 1;

    public int gems = 0;

    public EnemyBattleEntityStats(int startHealth, int startGems)
    {
        health = startHealth;
        gems = startGems;
    }
}