using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicService : MonoBehaviour
{
    public GameObject floor1Music;
    public GameObject floor2Music;
    public GameObject floor3Music;

    private AudioSource currentTrack;
    public void RequestFloorTheme(DungeonFloor floor)
    {
        if (currentTrack) currentTrack.Stop();
        if (floor == DungeonFloor.FLOOR1) currentTrack = floor1Music.GetComponent<AudioSource>();
        if (floor == DungeonFloor.FLOOR2) currentTrack = floor2Music.GetComponent<AudioSource>();
        if (floor == DungeonFloor.FLOOR3) currentTrack = floor3Music.GetComponent<AudioSource>();
        currentTrack.Play();
    }
}
