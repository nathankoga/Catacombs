using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleGUI : MonoBehaviour, IGUIManager
{
    /*
     * This class manages the interface for the battle system.
     */

    public BattleManager battleManager;
    public TextMeshProUGUI battleStatsText;
    public TextMeshProUGUI battleAbilitiesText;
    public Canvas canvas;

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
