using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicAttack : MonoBehaviour
{
    /*
     * (See design document)
     * Basic representation of an attack that deals damage.
     * Damage-dealing abilities will contain references to
     * an instance of BasicAttack.
     */

    public int base_dmg = 1;
    public int min_dmg = 1;
    // Critical rates stored in integer form as a percentage
    public int base_critrate = 25;
    public int min_critrate = 5;
    public int crit_dmg = 2;

    public int CalculateDamage(PlayerBattleEntityStats playerstats, EnemyBattleEntityStats enemystats) 
    {
        int dmg = base_dmg + playerstats.ferocity - enemystats.stubbornness;
        int critrate = base_critrate + (5 * playerstats.precision) - (5 * enemystats.grace);
        critrate = Mathf.Max(critrate, min_critrate);
        if (Random.Range(1, 100) < critrate) {
            dmg += crit_dmg;
        }
        dmg = Mathf.Max(dmg, min_dmg);
        return dmg;
    }
}