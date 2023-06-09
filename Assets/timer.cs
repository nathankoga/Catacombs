using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerTxt;

    // Update is called once per frame
    void Awake()
    {

        string min = Mathf.Floor(Time.time / 60).ToString("00");
        string seconds = (Time.time % 60).ToString("00");
        timerTxt.text = "Finishing Time: " + min + ":" + seconds;
    }
}
