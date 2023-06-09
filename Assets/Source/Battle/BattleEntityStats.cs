using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerBattleEntityStats
{
    /*
     * A dataclass for holding and manipulating player entity data.
     */

    public int lives = 9;   // start the player at 9 lives by default
    public int currentBalance = 3;  
    public int maxBalance = 3; // balance is like armor, depleted first before taking away a life (see battle design document)

    // starting battle attributes (default at 1)
    public int ferocity = 1;
    public int stubbornness = 1;
    public int precision = 1;
    public int grace = 1; 

    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 5;

    public int[] levelStats = {0,0,0,0,0,0};
    public bool leveledUp = false;

    public int gems = 0;
    public Amulet[] Amulets = Array.Empty<Amulet>();
    public Ability[] Abilities = Array.Empty<Ability>();
        
    public PlayerBattleEntityStats(int startBalance, int startGems) {
        maxBalance = startBalance;
        gems = startGems;
    }

    public void loseHP(int dmg, bool useBalance){

        // 10 second solution, please change bc this is not sound at all - Nathan
        // useBalance is true if it's a regular hit
        // false if you just want to lose HP and not hit balance first

        if (currentBalance>0 && useBalance) {
            currentBalance -= dmg;
            if (currentBalance < 0) {
                lives += currentBalance;
                currentBalance = 0;
            }
        }
        else {
            lives -= dmg;
        }

        if (lives == 0)
        {
            // DIE IDIOT
            Debug.Log("you died...");
            SceneManager.LoadScene("YouDied", LoadSceneMode.Single);
        }
    }

    public void refillBalance() {
        // fills balance all the way up to maxBalance
        currentBalance = maxBalance;
        Debug.Log("refilled balance to " + maxBalance);
    }

    public void gainExp(int exp){
        currentExp += exp;
        if (currentExp >= maxExp){
            levelUp();
        }
    }


    public void levelUp(){
        int upgradeNum = 0;  // set the number of times to update stats
        int bal = 0; int fer = 0; int stu = 0; int pre = 0; int gra = 0;
        
        while (currentExp >= maxExp){
            currentExp = currentExp - maxExp;
            level += 1;
            upgradeNum += 1;
            setLevelExp();
        }
        
        // Modify stats (saved this way for level up text update)
        for (int i = 0; i < upgradeNum; i++){
            bal += Random.Range(0, 2);
            fer += Random.Range(0, 2);
            stu += Random.Range(0, 2);
            pre += Random.Range(0, 2);
            gra += Random.Range(0, 2);
        }
        levelStats[0]= bal;
        levelStats[1]= fer;
        levelStats[2]= stu;
        levelStats[3]= pre;
        levelStats[4]= gra;
        levelStats[5]= upgradeNum;
        leveledUp = true;

        // random bug where balance gets set one lower (MIGHT BE DUE TO TURN ORDER, EVEN THOUGH I THOUGHT I FIXED)
        maxBalance += bal;
        ferocity += fer;
        stubbornness += stu;
        precision += pre;
        grace += gra;

        
        currentBalance = maxBalance + 1;
    }

    public void setLevelExp(){
        maxExp = (int) Mathf.Floor((10 + level * level)/2);
    }

}