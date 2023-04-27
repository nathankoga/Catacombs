using System;

public class BattleEntityStats
{
    /*
     * A dataclass for holding and manipulating entity data.
     * Players and enemies will have their own.
     */

    public int health = 10;
    public int armor = 0;

    public int redGems = 0;
    public int blueGems = 0;
    public int greenGems = 0;

    public Amulet[] Amulets = Array.Empty<Amulet>();
    public Ability[] Abilities = Array.Empty<Ability>();

}