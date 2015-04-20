using UnityEngine;
using System.Collections;

public class camDistance : MonoBehaviour 
{
    public float hSliderValue;
    public Player playerSettings;

    public float playerJumpForce;


    void Awake()
    {
        hSliderValue = Camera.main.orthographicSize;
        Screen.SetResolution(1024, 768, true);
    }
    
    
    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(00, 00, 250, 500), "Settings Menu");

            GUI.Label(new Rect(10, 20, 250, 20), "Camera Settings:");

            GUI.Label(new Rect(10, 40, 250, 20), "Distance: " + hSliderValue);
            hSliderValue = GUI.HorizontalSlider(new Rect(10, 60, 230, 20), hSliderValue, 1.0F, 20.0F);

            GUI.Label(new Rect(10, 80, 250, 20), "Player Settings:");

            GUI.Label(new Rect(10, 100, 250, 20), "JumpPower: " + playerSettings.jumpForce);
            playerSettings.jumpForce = GUI.HorizontalSlider(new Rect(10, 120, 230, 20), playerSettings.jumpForce, 400.0F, 800.0F);

            GUI.Label(new Rect(10, 140, 250, 20), "maxSpeed: " + playerSettings.maxSpeed);
            playerSettings.maxSpeed = GUI.HorizontalSlider(new Rect(10, 160, 230, 20), playerSettings.maxSpeed, 1.0F, 20.0F);
        
    }

    void Update()
    {
        Camera.main.orthographicSize = hSliderValue;
        playerJumpForce = playerSettings.jumpForce;
        
       // Debug.Log(playerSettings.jumpForce);
    }
}
