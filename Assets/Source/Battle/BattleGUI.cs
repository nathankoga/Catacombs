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
    public TextMeshProUGUI battleStatsText;
    public TextMeshProUGUI battleAbilitiesText;
    public Canvas canvas;

    public Button buttonOne;
    public Button buttonTwo;
    public Button buttonThree;
    public Button buttonFour;

    public void Start() {
        Button btn1 = buttonOne.GetComponent<Button>();
        btn1.onClick.AddListener(buttonOneEffect);

        Button btn2 = buttonTwo.GetComponent<Button>();
        btn2.onClick.AddListener(buttonTwoEffect);
        
        Button btn3 = buttonThree.GetComponent<Button>();
        btn3.onClick.AddListener(buttonThreeEffect);

        Button btn4 = buttonFour.GetComponent<Button>();
        btn4.onClick.AddListener(buttonFourEffect);
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

    public void Awake(){
        BattleManager.uiBattleUpdate += updateStats;
    }

    public void updateStats(string[] stats){
        battleStatsText.text = stats[0] + "\n" + stats[1] + "\n" + stats[2] + "\n" + stats[3];
        battleAbilitiesText.text = stats[4];
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
