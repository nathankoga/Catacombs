using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    /*
     * This class is in charge of the camera logic.
     * When the current manager type is set below, the camera should move
     * to the ideal position for the environment.
     */

    public GameObject player;
    void Start()
    {
        GameState.ManagerUpdate += OnManagerUpdate;
    }

    void OnManagerUpdate(ManagerType type, IManager mgr)
    {
        switch (type)
        {
            case (ManagerType.DUNGEON):
                print("Camera: Overhead Dungeon View");
                break;
            case (ManagerType.BATTLE):
                print("Camera: Battle View");
                break;
        }
    }
}
