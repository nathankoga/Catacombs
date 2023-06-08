using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StatsGUI : MonoBehaviour
{
    public Canvas canvas;
    public RunStats runStats;

    public TextMeshProUGUI statsHeader;
    public TextMeshProUGUI balance;
    public TextMeshProUGUI level;
    public TextMeshProUGUI precision;
    public TextMeshProUGUI ferocity;
    public TextMeshProUGUI stubbornness;
    public TextMeshProUGUI grace;
    public TextMeshProUGUI exp;

    public bool leveledUp = false;

    public void updateGUI(){
        statsHeader.text = "Stats";
        level.text = "Level: " + runStats.playerStats.level.ToString();
        balance.text = "Balance: " + runStats.playerStats.maxBalance.ToString();
        precision.text = "Precision: " + runStats.playerStats.precision.ToString();
        ferocity.text = "Ferocity: " + runStats.playerStats.ferocity.ToString();
        stubbornness.text = "Stubbornness: " + runStats.playerStats.stubbornness.ToString();
        grace.text = "Grace: " + runStats.playerStats.grace.ToString();
        exp.text = "Exp: " + runStats.playerStats.currentExp.ToString() + "/" + runStats.playerStats.maxExp;
    }

    public void levelupGUI(){
        // current and bonus
        int cB = runStats.playerStats.maxBalance;    int pB = runStats.playerStats.levelStats[0];
        int cP = runStats.playerStats.precision;     int pP = runStats.playerStats.levelStats[3];
        int cF = runStats.playerStats.ferocity;      int pF = runStats.playerStats.levelStats[1];
        int cS = runStats.playerStats.stubbornness;  int pS = runStats.playerStats.levelStats[2];
        int cG = runStats.playerStats.grace;         int pG = runStats.playerStats.levelStats[4];
        int cL = runStats.playerStats.level;         int pL = runStats.playerStats.levelStats[5];


        // because of issues with referencing non monobehaviour scripts, we have to pull the updated stats and subtract it by
        //      the array which contains info for what level up bonuses occurred
        statsHeader.text = "Level Up!!!";
        level.text = "Level: " + (cL-pL).ToString()+ " + " + pL.ToString();
        balance.text = "Balance: " + (cB-pB).ToString()+ " + " + pB.ToString();
        precision.text = "Precision: " + (cP- pP).ToString() + " + " + pP.ToString();
        ferocity.text = "Ferocity: " + (cF-pF).ToString() + " + " + pF.ToString();
        stubbornness.text = "Stubbornness: " + (cS -pS).ToString() + " + " + pS.ToString();
        grace.text = "Grace: " +  (cG -pG).ToString() + " + " + pG.ToString();
        exp.text = "Exp: " + runStats.playerStats.currentExp.ToString() + "/" + runStats.playerStats.maxExp;
        canvas.enabled = true;
        leveledUp = true;
    }   
    
    public void DisableGUI(){
        canvas.enabled = false;
    }
    
    public void EnableGUI(){
        updateGUI();

        canvas.enabled = true;
    }
}
