using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    /*
     * This class has the logic for an enemy represented on a dungeon tile.
     */
    public EnemyBattleEntityStats statblock;
    public Vector3 worldOffset;

    GameObject selfRefObject;

    private MapTile mapTile;
    private EnemyType enemyType;

    private int health;
    private int damage;


    // private int gems = 0;  // possible later mechanic?    

    // public void Initialize(MapTile mapTile, EnemyType enemyType, health, damage)
    public void Initialize(MapTile mapTile, EnemyType enemyType)
    {
        // Set enums.
        this.mapTile = mapTile;
        this.enemyType = enemyType;

        selfRefObject = this.mapTile.referenceEnemyObject();
        //this.health = health;
        //this.damage = damage;

        // Set positioning.
        LerpToWorldPos(1.0f);

        if (enemyType == EnemyType.Floor1Boss){
            
            // set color to black
            selfRefObject.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
            // set size to be a bit taller
            selfRefObject.gameObject.transform.localScale += new Vector3(1, 3, 1);
        }
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