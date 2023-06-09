using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleEntityStats : MonoBehaviour
{
    /*
     * A dataclass for holding and manipulating enemy entity data.
     * CURRENTLY UNUSED
     */
    public EnemyType type;

    public int health = 1; // This reflects maximum health points, it should not be changed during combat

    // default battle attributes
    public int ferocity = 1;
    public int stubbornness = 1;
    public int precision = 1;
    public int grace = 1;
    // public int expGain= 1;


    public int gems = 0;

    // public EnemyBattleEntityStats(int startHealth, int startGems)
    // Allegedly class constructors don't work for MonoBehaviour objects, therefore
    // we change to an explicit constructor 
    /* deprecated: create instances of EnemyBattleEntityStats through the object hierarchy
    public void setEnemyBattleEntityStats(int startHealth, int startGems)
    {
        health = startHealth;
        gems = startGems;
    }
    */
}