using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGUI : MonoBehaviour, IGUIManager
{
    /*
     * This class manages the overworld GUI while exploring dungeons.
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
