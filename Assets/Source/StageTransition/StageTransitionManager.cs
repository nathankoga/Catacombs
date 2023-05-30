using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageTransitionManager : MonoBehaviour, IManager
{
    /*
     * This class is in charge of the entire battle system.
     * Um. We'll figure this out later...
     * This will be fun :) -Sheep
     */

    public GameState gameState;
    public StageTransitionGUI GUI;

    private bool active = false;
    private float timer = 1.0f;

    private void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            gameState.GotoNextFloor();
            active = false;
        }
    }

    /*
     * IManager Transitions
     */

    public void StartManager()
    {
        GUI.EnableGUI();
        timer = 1.0f;
        active = true;
    }

    public void StopManager()
    {
        GUI.DisableGUI();
    }

    public ManagerType GetManagerType()
    {
        return ManagerType.LEVELTRANSITION;
    }

}
