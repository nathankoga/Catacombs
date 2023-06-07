using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AbilitiesGUI : MonoBehaviour
{
    public Canvas canvas;
    public RunStats runStats;

    public void DisableGUI(){
        canvas.enabled = false;
    }
    public void EnableGUI(){
        canvas.enabled = true;
    }

}
