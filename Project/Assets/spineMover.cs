using UnityEngine;
using System.Collections;

public class spineMover : MonoBehaviour {

    public float speed = 5;
    public Transform graphics;

    public SkeletonAnimation reitseSpine;

    Quaternion goalRotation = Quaternion.identity;
    float xMovement;

    string currentAnimation = "";

	
	
	// Update is called once per frame
	void Update () {
        xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (xMovement > 0)
        {
            goalRotation = Quaternion.Euler(0, 0, 0);
            SetAnimation("Run", true);
        }
        else if (xMovement < 0)
        {
            goalRotation = Quaternion.Euler(0, 180, 0);
            SetAnimation("Run", true);
        }
        else
        {
            SetAnimation("Idle", true);
        }

        graphics.localRotation = Quaternion.Lerp(graphics.localRotation, goalRotation, 5 * Time.deltaTime);
	}

    void SetAnimation(string name, bool loop)
    {
        if (name == currentAnimation)
            return;

        reitseSpine.state.SetAnimation(0, name, loop);
            currentAnimation = name;
    }

    void FixedUpdate ()
    {
        
        //rigidbody2D.velocity = new Vector2(xMovement, rigidbody2D.velocity.y);
    }
}

