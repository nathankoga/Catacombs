using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public void SetHealth(int health){
        healthSlider.value = health;
        healthText.text = "Health: " + health + "/9";
    }
}
