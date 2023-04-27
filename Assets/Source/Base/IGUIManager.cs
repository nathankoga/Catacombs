using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGUIManager
{
    /*
     * A simple interface for GUI managers.
     * Each manager may have one of these for enabling/disabling their own GUI suites.
     */

    public void EnableGUI();
    public void DisableGUI();

}
