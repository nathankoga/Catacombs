using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageTransitionGUI : MonoBehaviour, IGUIManager
{

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
