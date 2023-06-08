using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExpBar : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI expText;

    public void SetExp(int level, int exp, int maxExp){
        expSlider.value = exp;
        expSlider.maxValue = maxExp;
        
        expText.text = "Lvl " + level + 
                        ": (" + exp + "/" + maxExp + ")";
    }


}
