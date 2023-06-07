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

    
    public void Awake(){
        PlayerLogic.MoveUpdate += RemoveTutorialText;
    }
    
    // remove the "wasd to move text"
    public void RemoveTutorialText(){
        // tutorialText.text = "";
        DisableGUI();
    }
    
    
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
