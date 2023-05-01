using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUI : MonoBehaviour, IGUIManager
{
    /*
     * This class manages the interface for the battle system.
     */

    public Canvas canvas;

    public void EnableGUI()
    {
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
