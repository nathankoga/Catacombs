using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleManager : MonoBehaviour, IManager
{
    /*
     * This class is in charge of the entire battle system.
     * Um. We'll figure this out later...
     * This will be fun :) -Sheep
     */

    public GameState gameState;
    public DungeonManager dungeonManager;
    public BattleGUI GUI;
    public StatsGUI statsGUI;
    public BasicAttack bite;
    public BasicAttack scratch;

    public RunStats runStats;


    public delegate void uiUpdate(string[] stats);
    public static event uiUpdate uiBattleUpdate;
    // take data from runStats for player information?
    // public RunStats runStats;
    
    /*
     * Battle State
     */

    bool battleActive = false;
    bool playerTurn = true;
    private MapTile mapTile;
    private EnemyType enemyType;
    private GameObject enemyRef;

    // audio
    public AudioSource deathSound;

    /*
     * Battle Requesting
     */

    void Awake()
    {
        MapTile.StartBattle += EngageBattle;
        deathSound = GetComponent<AudioSource>();
    }

    void EngageBattle(MapTile mapTile, EnemyType enemyType)
    {
        // 'this' keyword for clarity when passing different variables of same name around
        this.mapTile = mapTile;
        this.enemyType = enemyType;
        enemyRef = this.mapTile.referenceEnemyObject();
        enemyRef.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
        runStats.playerStats.refillBalance(); // enemy still seems to go first �maya
        gameState.RequestManager(this);
    }

    /*
     * Battle Input
     */
    private void Update()
    {
        if (!battleActive) return;

        // use the string array generated by getBattleText to update the UI in BattleGUI
        
        if (!playerTurn){
            AttackPlayer();
            playerTurn = true;
        }
        
        uiBattleUpdate(getBattleText());

        // if input is a damaging move->
        //     call DamageEnemy()

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     WinBattle();
        // }
    }

    private void WinBattle()
    {
        // play enemy death sound
        // deathSound.Play();
        runStats.playerStats.gainExp(enemyRef.GetComponent<DungeonEnemy>().expGain);
        
        // can't reference StatsGUI from playerStats, so we check for level here
        if (runStats.playerStats.leveledUp){
            
            deathSound.Play();  // death sound refactored as a 'level up' sound effect
            statsGUI.levelupGUI();
            runStats.playerStats.leveledUp = false;
        }

        playerTurn = true;
        mapTile.ClearEnemy();
        gameState.RequestManager(dungeonManager);
    }

    /*
     * Battle Effects
     */
    public void DamageEnemy(int dmg)
    {
        enemyRef.GetComponent<DungeonEnemy>().loseHP(dmg);
        if (enemyRef.GetComponent<DungeonEnemy>().getHP() <= 0)
        {
            WinBattle();
        }
    }

    // this should be done somewhere else to reduce clutter but we dont have time
    public void Bite()
    {
        int attackdmg = bite.CalculateDamage(runStats.playerStats, enemyRef.GetComponent<DungeonEnemy>());
        DamageEnemy(attackdmg);
        playerTurn = false;
    }

    public void Scratch()
    {
        int attackdmg = scratch.CalculateDamage(runStats.playerStats, enemyRef.GetComponent<DungeonEnemy>());
        attackdmg += scratch.CalculateDamage(runStats.playerStats, enemyRef.GetComponent<DungeonEnemy>());
        DamageEnemy(attackdmg);
        playerTurn = false;
    }

    // should go through the basic attack system, but short on time 
    public int BasicEnemyAttack(DungeonEnemy enemystats, PlayerBattleEntityStats playerstats)
    {
        int dmg = 1 + enemystats.ferocity - playerstats.stubbornness;
        int critrate = 10 + (5 * enemystats.precision) - (5 * playerstats.grace);
        if (Random.Range(1, 100) < critrate)
        {
            dmg += enemystats.ferocity; // in place of critdmg
        }
        if (enemystats.enemyType == EnemyType.Floor1 || enemystats.enemyType == EnemyType.Floor2){
            dmg = Mathf.Max(dmg, 1); 
        }
        
        else if (enemystats.enemyType == EnemyType.Floor3Boss )
            {
                dmg = Mathf.Max(dmg, 3);
            }
        else { dmg = Mathf.Max(dmg, 2); }
        return dmg;
    }

    public void AttackPlayer()
    {
        // delays the combat slightly
        // breaks game by means of skipping their damage turns
        Invoke("delayedAttack", 0.0f);
        // int dmg = BasicEnemyAttack(enemyRef.GetComponent<DungeonEnemy>(), runStats.playerStats);
        // DamagePlayer(dmg);
    }

    public void delayedAttack(){
        int dmg = BasicEnemyAttack(enemyRef.GetComponent<DungeonEnemy>(), runStats.playerStats);
        DamagePlayer(dmg);
    }

    public void DamagePlayer(int dmg){
        runStats.playerStats.loseHP(dmg, true);
    }

    public void ExchangeHPForBalance(int dmg) {
        // refill balance bar
        runStats.playerStats.refillBalance();
        runStats.playerStats.loseHP(dmg, false);
        playerTurn = false;
    }

    public void Hiss()
    {
        // Lower the enemy's precision by 5 and grace by 3, greatly lowering the risk of critical hits and raising your own critical hit chane
        if (enemyRef.GetComponent<DungeonEnemy>().is_debuffed) {
            return;
        }
        enemyRef.GetComponent<DungeonEnemy>().is_debuffed = true;
        Color color = Color.white;
        color.r = 0.5f; color.g = 0.3f; color.b = 0.9f; color.a = 0.9f;
        enemyRef.GetComponentInChildren<MeshRenderer>().material.color = color;
        enemyRef.GetComponent<DungeonEnemy>().precision = Mathf.Max(0, enemyRef.GetComponent<DungeonEnemy>().precision - 5);
        enemyRef.GetComponent<DungeonEnemy>().grace = Mathf.Max(0, enemyRef.GetComponent<DungeonEnemy>().grace - 3);
        playerTurn = false;
    }


    public string[] getBattleText(){
        // returns an array of strings, each one associated to specific data to update in BattleGUI

        // DungeonEnemy is the reference to the enemy object, and EnemyBattleEntityStats holds a reference to it's battle data
        /*
        string[] battleText = {"Enemy HP: " + enemyRef.GetComponent<DungeonEnemy>().getHP().ToString(), 
                                "Enemy Maximum HP: " + enemyRef.GetComponent<EnemyBattleEntityStats>().health.ToString(), // TODO: fix bug causing unwanted newline here
                                "",
                                "", 
                                "\t\t\t  Your turn! \n \t\t    (Select a Move)"};
        */ 
        string[] battleText = {
            runStats.playerStats.lives.ToString(),
            runStats.playerStats.maxBalance.ToString(),
            runStats.playerStats.currentBalance.ToString(),
            runStats.playerStats.level.ToString(),
            runStats.playerStats.currentExp.ToString(),
            runStats.playerStats.maxExp.ToString(),
            "Enemy HP:" + enemyRef.GetComponent<DungeonEnemy>().getHP().ToString(),
                                
                                };
        // string[] battleText = {"Enemy HP: " + enemyRef.GetComponent<DungeonEnemy>().getHP().ToString(), 
        //                         "",  
        //                         "",
        //                         "", 
        //                         "\t\t\t  Your turn! \n \t\t    (Select a Move)"};
        return battleText;
    }
    /*
     * IManager Transitions
     */

    public void StartManager()
    {
        battleActive = true;
        GUI.EnableGUI();
    }

    public void StopManager()
    {
        battleActive = false;
        GUI.DisableGUI();
    }

    public ManagerType GetManagerType()
    {
        return ManagerType.BATTLE;
    }

}
