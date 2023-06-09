using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    public Material floor1;
    public Material floor2;
    public Material floor3;

    void Awake()
    {
        GameState.FloorStart += SwitchSkybox;
    }

    void SwitchSkybox(GameState gs, DungeonFloor f)
    {
        if (f == DungeonFloor.FLOOR1) RenderSettings.skybox = floor1;
        if (f == DungeonFloor.FLOOR2) RenderSettings.skybox = floor2;
        if (f == DungeonFloor.FLOOR3) RenderSettings.skybox = floor3;
    }
}
