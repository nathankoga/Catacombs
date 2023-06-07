using System;
using UnityEngine;

public class PlayerBattleEntityStats
{
    /*
     * A dataclass for holding and manipulating player entity data.
     */

    public int lives = 9;   // start the player at 9 lives by default
    public int currentBalance = 9;  
    public int maxBalance = 9; // balance is like armor, depleted first before taking away a life (see battle design document)

    // starting battle attributes (default at 1)
    public int ferocity = 1;
    public int stubbornness = 1;
    public int precision = 1;
    public int grace = 1; 

    public int currentExp = 0;
    public int maxExp = 10;

    public int gems = 0;
    public Amulet[] Amulets = Array.Empty<Amulet>();
    public Ability[] Abilities = Array.Empty<Ability>();

    public PlayerBattleEntityStats(int startBalance, int startGems) {
        maxBalance = startBalance;
        gems = startGems;
    }

    public void loseHP(int dmg){

        // 10 second solution, please change bc this is not sound at all - Nathan

        if (currentBalance>0){
            currentBalance -= dmg;
        }
        else{
            lives -= dmg;
        }
    }

}