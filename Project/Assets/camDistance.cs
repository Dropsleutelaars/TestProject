using UnityEngine;
using System.Collections;

public class camDistance : MonoBehaviour 
{
    public float hSliderValue;

    void Awake()
    {
        hSliderValue = Camera.main.orthographicSize;
    }
    
    
    void OnGUI()
    {
        
        GUI.Label(new Rect(10, 10, 250, 20), "Change the Camera Distance");
        GUI.Label(new Rect(30, 30, 250, 20), "Current distance: " + hSliderValue);
    
        hSliderValue = GUI.HorizontalSlider(new Rect(50, 50, 250, 20), hSliderValue, 1.0F, 15.0F);
    }

    void Update()
    {
        
        Camera.main.orthographicSize = hSliderValue;
    }
}
