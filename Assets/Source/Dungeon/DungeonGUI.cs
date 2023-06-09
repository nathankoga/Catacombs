using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonGUI : MonoBehaviour, IGUIManager
{
    /*
     * This class manages the overworld GUI while exploring dungeons.
     */
    public PlayerLogic player;
    public TextMeshProUGUI tutorialText;
    public Canvas canvas;
    public GameObject tutGUI;

    
    public void Awake(){
        PlayerLogic.MoveUpdate += RemoveTutorialText;
    }
    
    // remove the "wasd to move text"
    public void RemoveTutorialText(){
        tutGUI.SetActive(false);
    }
    
    
    public void EnableGUI()
    {
        return;
    }

    public void DisableGUI()
    {
        return;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
