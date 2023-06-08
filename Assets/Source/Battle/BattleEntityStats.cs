using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 5;

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

        if (lives == 0)
        {
            // DIE IDIOT
            Debug.Log("you died...");
            SceneManager.LoadScene("YouDied", LoadSceneMode.Single);
        }
    }

    public void gainExp(EnemyType enemy){
        switch (enemy)
        {
            case EnemyType.Floor1:
                currentExp += 3;
                break;

            case EnemyType.Floor1Boss:
                currentExp += 10;
                break; 

            default:
                break;
        } 
        if (currentExp >= maxExp){
            levelUp();
        }
    }


    public void levelUp(){
        while (currentExp >= maxExp){
            currentExp = currentExp - maxExp;
            level += 1;
            setLevelExp();
        }

        // Modify stats 
        // probably add a "level up" text/ GUI update also
    }

    public void setLevelExp(){
        maxExp = (int) Mathf.Floor((10 + level * level)/2);
    }

}