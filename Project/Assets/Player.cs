using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public float movementSpeed;
    float moveHorizontal;
    Vector2 movement; 

    // Use this for initialization
	void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        movement = new Vector2(moveHorizontal, 0f);
        
        rigidbody.AddForce(movement * movementSpeed * Time.deltaTime);
	}
}
