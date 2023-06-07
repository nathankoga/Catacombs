using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StatsGUI : MonoBehaviour
{
    public Canvas canvas;
    public RunStats runStats;

    public TextMeshProUGUI level;
    public TextMeshProUGUI precision;
    public TextMeshProUGUI ferocity;
    public TextMeshProUGUI stubbornness;
    public TextMeshProUGUI grace;
    public TextMeshProUGUI exp;

    public void updateGUI(){
        level.text = "Level: " + runStats.playerStats.level.ToString();
        precision.text = "Precision: " + runStats.playerStats.precision.ToString();
        ferocity.text = "Ferocity: " + runStats.playerStats.ferocity.ToString();
        stubbornness.text = "Stubbornness: " + runStats.playerStats.stubbornness.ToString();
        grace.text = "Grace: " + runStats.playerStats.grace.ToString();
        exp.text = "Exp: " + runStats.playerStats.currentExp.ToString() + "/" + runStats.playerStats.maxExp;
    }
    
    public void DisableGUI(){
        canvas.enabled = false;
    }
    
    public void EnableGUI(){
        updateGUI();

        canvas.enabled = true;
    }
}
