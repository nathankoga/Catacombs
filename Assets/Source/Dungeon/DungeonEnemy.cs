using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    /*
     * This class has the logic for an enemy represented on a dungeon tile.
     */

    public Vector3 worldOffset;

    private MapTile mapTile;
    private EnemyType enemyType;

    private int health;
    private int damage;

    private int isBoss;  // possible tag (might be unecessary if we use enemyType enum)

    // private int gems = 0;  // possible later mechanic?    

    // public void Initialize(MapTile mapTile, EnemyType enemyType, health, damage)
    public void Initialize(MapTile mapTile, EnemyType enemyType)
    {
        // Set enums.
        this.mapTile = mapTile;
        this.enemyType = enemyType;

        //this.health = health;
        //this.damage = damage;

        // Set positioning.
        LerpToWorldPos(1.0f);
    }

    private void LerpToWorldPos(float t)
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetWorldPos(), t);
    }

    public Vector3 GetTargetWorldPos()
    {
        return mapTile.transform.position + worldOffset;
    }
}