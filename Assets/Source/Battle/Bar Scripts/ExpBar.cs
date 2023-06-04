using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExpBar : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI expText;

    public void SetMaxExp(int exp){
        // slider is an object that represents the width of the exp bar's colored-in section
        // by setting max value, we can accurrately represent the bar's value in terms of a ratio
        expSlider.maxValue = exp;
    }

    public void SetExp(int exp){
        expSlider.value = exp;
        
        // TODO: add/ update maximum exp stats 
        expText.text = "Exp: " + exp + "/" + expSlider.maxValue;
    }


}
