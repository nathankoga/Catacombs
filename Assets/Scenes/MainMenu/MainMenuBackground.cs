using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get our rect transform.
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Oscillate it up and down.
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Mathf.Sin(Time.time) * 10);
    }
}
