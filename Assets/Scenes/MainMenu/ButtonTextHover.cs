using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTextHover : MonoBehaviour
{

    public TextMeshProUGUI textNode;
    public string text;

    public void OnMouseEnter()
    {
        textNode.text = "> " + text;
    }

    public void OnMouseExit()
    {
        textNode.text = text;
    }
}
