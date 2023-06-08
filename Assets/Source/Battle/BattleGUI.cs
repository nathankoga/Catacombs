using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleGUI : MonoBehaviour, IGUIManager
{
    /*
     * This class manages the interface for the battle system.
     */

    public BattleManager battleManager;
    public TextMeshProUGUI enemyStatsText;
    public TextMeshProUGUI battleAbilitiesText;
    public Canvas canvas;

    public HealthBar healthBar;    
    public BalanceBar balanceBar;    
    public ExpBar expBar;    


    public Button buttonOne;
    public Button buttonTwo;
    public Button buttonThree;
    public Button buttonFour;
    public Button buttonFive;

    public void Start() {
        Button btn1 = buttonOne.GetComponent<Button>();
        btn1.onClick.AddListener(buttonOneEffect);

        Button btn2 = buttonTwo.GetComponent<Button>();
        btn2.onClick.AddListener(buttonTwoEffect);
        
        Button btn3 = buttonThree.GetComponent<Button>();
        btn3.onClick.AddListener(buttonThreeEffect);

        Button btn4 = buttonFour.GetComponent<Button>();
        btn4.onClick.AddListener(buttonFourEffect);
        
        Button btn5 = buttonFive.GetComponent<Button>();
        btn5.onClick.AddListener(buttonFiveEffect);
    }

    void buttonOneEffect() {
        Debug.Log("You clicked button one\n");
        battleManager.DamageEnemy(1);
    }

    void buttonTwoEffect() {
        Debug.Log("You clicked button two\n");
        battleManager.DamageEnemy(2);
    }

    void buttonThreeEffect() {
        Debug.Log("You clicked button three\n");
    }

    void buttonFourEffect() {
        Debug.Log("You clicked button four\n");
    }
    
    void buttonFiveEffect() {
        Debug.Log("You clicked button five\n");
    }

    public void Awake(){
        BattleManager.uiBattleUpdate += updateStats;
    }

    public void updateStats(string[] stats){

        // int.parse casts string to int
        // seems very wasteful to have to set max balance each time, so we might fix later
        
        healthBar.SetHealth( int.Parse(stats[0]));
        balanceBar.SetMaxBalance( int.Parse(stats[1]));
        balanceBar.SetBalance( int.Parse(stats[2]));
        expBar.SetExp(int.Parse(stats[3]),
                    int.Parse(stats[4]),
                    int.Parse(stats[5]));

        enemyStatsText.text =  stats[6] ;
        // enemyStatsText.text = stats[0] + "\n" + stats[1] + "\n" + stats[2] + "\n" + stats[3];
        // battleAbilitiesText.text = stats[4];
    }

    public void EnableGUI()
    {
       // NOTE: deleted the original "press space to kill" prompt from the canvas (in game hierarchy) 
        canvas.enabled = true;
    }

    public void DisableGUI()
    {
        canvas.enabled = false;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
