using System;
using UnityEngine;

public class PlayerBattleEntityStats
{
    /*
     * A dataclass for holding and manipulating player entity data.
     */

    public int lives = 9;   // start the player at 9 lives by default
    public int balance = 0; // balance is like armor, depleted first before taking away a life (see battle design document)

    // starting battle attributes (default at 1)
    public int ferocity = 1;
    public int stubbornness = 1;
    public int precision = 1;
    public int grace = 1; 

    public int gems = 0;
    public Amulet[] Amulets = Array.Empty<Amulet>();
    public Ability[] Abilities = Array.Empty<Ability>();

    public PlayerBattleEntityStats(int startBalance, int startGems) {
        balance = startBalance;
        gems = startGems;
    }
}