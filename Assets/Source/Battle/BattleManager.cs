using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour, IManager
{
    /*
     * This class is in charge of the entire battle system.
     * Um. We'll figure this out later...
     * This will be fun :) -Sheep
     */

    public BattleGUI GUI;

    public void StartManager()
    {
        GUI.EnableGUI();
    }

    public void StopManager()
    {
        GUI.DisableGUI();
    }

    public ManagerType GetManagerType()
    {
        return ManagerType.BATTLE;
    }

}
