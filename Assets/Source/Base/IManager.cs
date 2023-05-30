using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerType { BATTLE, DUNGEON, LEVELTRANSITION };

public interface IManager
{
    /*
     * The base interface for the GameState manager FSM.
     * With this interface structure, we can easily develop new game states
     * as need be (for example, opening chests or having shop tiles).
     */
    public void StartManager();
    public void StopManager();
    public ManagerType GetManagerType();

}
