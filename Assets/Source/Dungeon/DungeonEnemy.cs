using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    /*
     * This class has the logic for an enemy represented on a dungeon tile.
     */
    public EnemyBattleEntityStats statblock;
    public Vector3 worldOffset;

    private GameObject selfRefObject;

    private MapTile mapTile;
    private EnemyType enemyType;

    public int hp; // This reflects current hit points, it will be modified in combat and may reach zero
    // private int gems = 0;  // possible later mechanic?    

    public int getHP()
    {
        return this.hp;
    }
    public void setHP(int newHP)
    {
        this.hp = newHP;
    }
    public void loseHP(int dmg)
    {
        this.hp -= dmg;
    }

    public void Initialize(MapTile mapTile, EnemyType enemyType)
    {
        // Set enums.
        this.mapTile = mapTile;
        this.enemyType = enemyType;

        selfRefObject = this.mapTile.referenceEnemyObject();
        
        // statblock = selfRefObject.GetComponent<EnemyBattleEntityStats>();
        // set the statblock values (hard-coded for now, but can set values differently based off of EnemyType enum)
        
        // statblock = new EnemyBattleEntityStats(); 
        // this leads to "you are trying to create a monobehaviour using the 'new' keyword error 
        
        // NOTE: Using AddComponent<...>() initializes the script that we are attaching to DungeonEnemy
        statblock = selfRefObject.AddComponent<EnemyBattleEntityStats>();
        // Deprecated, use this 
        
        // NOTE: Commented out just for the game presentation (Hard-coding health values to start at 3)
        // this.hp = statblock.health;
        
        if (enemyType == EnemyType.FloorBoss){
            this.hp = 10;
        }
        else {
            this.hp = 3;
        }

        // Set positioning.
        LerpToWorldPos(1.0f);

        if (enemyType == EnemyType.FloorBoss){
            
            // set color to magenta 
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