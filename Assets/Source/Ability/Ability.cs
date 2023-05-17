public enum AbilityType { TODO };

public class Ability
{
    /*
     * The base class for ability data.
     * Each ability we implement will subclass from this,
     * and an interface for this class will be developed
     * alongside the battle system.
     */

    public AbilityType type = AbilityType.TODO;

}

public class BasicAttack
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

}