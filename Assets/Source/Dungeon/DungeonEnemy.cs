using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    /*
     * This class has the logic for an enemy represented on a dungeon tile.
     */

    // bad hacky solution to extend old EnemyType logic
    // these are all the statblocks for game enemies to initialize
    public EnemyBattleEntityStats floor1;
    public EnemyBattleEntityStats floor2;
    public EnemyBattleEntityStats floor3;
    public EnemyBattleEntityStats floor1boss;
    public EnemyBattleEntityStats floor2boss;
    public EnemyBattleEntityStats floor3boss;

    public EnemyBattleEntityStats statblock;

    public Vector3 worldOffset;

    private GameObject selfRefObject;

    private MapTile mapTile;
    private EnemyType enemyType;


    public int hp; // This reflects current hit points, it will be modified in combat and may reach zero
    public int expGain; 
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
        
        // NOTE: Using AddComponent<...>() initializes the script that we are attaching to DungeonEnemy
        statblock = selfRefObject.AddComponent<EnemyBattleEntityStats>();
        // Deprecated, use this 
        
        // NOTE: Hard-coding health values for the Beta Presentation
        // this.hp = statblock.health;
        
        switch (enemyType) {
            case EnemyType.Floor1:
                this.statblock = floor1;
                this.hp = 3;
                expGain = 3;
                break;

            case EnemyType.Floor2:
                this.statblock = floor2;
                this.hp = 10;
                expGain = 6;
                break;

            case EnemyType.Floor3:
                this.statblock = floor3;
                this.hp = 15;
                expGain = 8;
                break;
            
            case EnemyType.Floor1Boss:
                this.statblock = floor1boss;
                this.hp = 10;
                expGain = 10;
                selfRefObject.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
                selfRefObject.gameObject.transform.localScale += new Vector3(1, 3, 1);
                break;

            case EnemyType.Floor2Boss:
                this.statblock = floor2boss;
                this.hp = 25;
                expGain = 15;
                selfRefObject.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                selfRefObject.gameObject.transform.localScale += new Vector3(1, 4, 1);
                break;

            case EnemyType.Floor3Boss:
                this.statblock = floor3boss;
                this.hp = 40;
                expGain = 20;
                selfRefObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                selfRefObject.gameObject.transform.localScale += new Vector3(1, 5, 1);
                break;

            default:
                break;


        }
        
        // Set positioning.
        LerpToWorldPos(1.0f);

        // if (enemyType == EnemyType.Floor1Boss || enemyType == EnemyType.Floor2Boss || enemyType == EnemyType.Floor3Boss){
        //     // set color to magenta 
        //     selfRefObject.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
        //     // set size to be a bit taller
        //     selfRefObject.gameObject.transform.localScale += new Vector3(1, 3, 1);
        // }
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