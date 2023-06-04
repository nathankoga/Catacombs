using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BalanceBar : MonoBehaviour
{
    public Slider balanceSlider;
    public TextMeshProUGUI balanceText;

    public void SetMaxBalance(int balance){
        balanceSlider.maxValue = balance;
    }
    
    public void SetBalance(int balance){
        balanceSlider.value = balance;
        // TODO: add/ update maximum balance stats 
        balanceText.text = "Balance: " + balance + "/" + balanceSlider.maxValue;
    }
   
}
