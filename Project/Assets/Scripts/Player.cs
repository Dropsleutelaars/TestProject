using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script needs an RigidBody2D component attached to the same object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// Player inheritances from Entity
/// </summary>
public class Player : Entity 
{
    public Animator anim;
    private Rigidbody2D rigidbody;

#region PlayerVars

    public float dashPower = 1f;
    public float maxSpeed = 10f;
    public float jumpForce = 400f;
    public float groundedCheckRadius = 0.2f;
    public float dashBuffer;

#endregion

    public LayerMask groundMasks;

    private int numberOfHoldingObjects;

    [SerializeField]
    private List<GameObject> holdingObjects = new List<GameObject>();
    

    /// <summary>
    /// This happens when the game 'Starts'
    /// Im setting the rigidbody variable to the rigidbody2d component.
    /// </summary>
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        numberOfHoldingObjects = 0;

       
    }

    /// <summary>
    /// This happens every frame in the game.
    /// Im catching the key input
    /// </summary>
    private void Update()
    {
        //Debug.Log(this.transform.position.y);

        bool dashKeyDown = Input.GetKeyDown(KeyCode.X);
        bool jumpKeyDown = Input.GetKeyDown("space");
        float moveDirection = Input.GetAxis("Horizontal"); // -1 to 1
        bool grounded = false;

        #region Grounded check
        /******** Grounded check */
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, groundedCheckRadius, groundMasks);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }
        /********** end grounded check  */
        #endregion

        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", rigidbody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));
        anim.SetBool("isDashing", IsInDash);

        numberOfHoldingObjects = NumberOfHoldingObjects;

        float dashMovement = (dashBuffer * 0.1f);
        dashBuffer -= dashMovement;

        float xMovement = IsInDash ? moveDirection + dashMovement * maxSpeed : moveDirection * maxSpeed;

        rigidbody.velocity = new Vector2(xMovement, rigidbody.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (moveDirection > 0.1f && !FacingRight)
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveDirection < -.1f && FacingRight)
            Flip();

        if (grounded && jumpKeyDown && anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            grounded = false;
            anim.SetBool("Ground", false);
            rigidbody.AddForce(new Vector2(0f, jumpForce));
        }
        else if (!grounded && jumpKeyDown &&
            (anim.GetAnimatorTransitionInfo(0).IsUserName("Jumping") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Jumping")))
        {
            anim.SetBool("DoubleJump", true);
            rigidbody.AddForce(new Vector2(0f, jumpForce));
        }

        if (grounded)
            anim.SetBool("DoubleJump", false);

        if (!IsInDash && dashKeyDown)
        {
            // Versimpelde if-statement. 
            // Facingright == true? --> dashpower
            // Facingright == false? --> -dashpower
            // Dit gbruik ik om de juiste directie op de dashen.

            dashBuffer = FacingRight ? dashPower : -dashPower;
        }

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsTag("Dash"));
        //Debug.Log("Is In Dash?" + IsInDash + " - " +   dashBuffer.x);
    }

    /// <summary>
    /// This is an accessor.
    /// </summary>
    public bool IsInDash
    {
        get
        {
            //return !Mathf.Approximately(dashBuffer.x, 0);

            if (dashBuffer > 0.1f || dashBuffer < -0.1f)
            {
                return true;
            }
            
            else 
            {
                return false;
            }   
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            //Vector2 newPosition = new Vector2(myLocation.x, myLocation.y + 3);

            float margin = 3.5f; 

            if (NumberOfHoldingObjects > 0)
            {
                margin = (NumberOfHoldingObjects + margin);
            }
            

            other.gameObject.transform.parent = this.transform;

            other.gameObject.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + margin);

            BoxCollider2D pickupCollision = other.gameObject.GetComponent<BoxCollider2D>();

            //Stop the Collision to prevent its adding points while you double jump?!
            pickupCollision.enabled = false;

            holdingObjects.Add((GameObject)other.gameObject);
            numberOfHoldingObjects = holdingObjects.Count;          

            
        }
    }

    /// <summary>
    /// This handles the number of holding objects. When something got picked up
    /// it just adds +1 to the value.
    /// </summary>
    public int NumberOfHoldingObjects
    {
        get 
        {
            return holdingObjects.Count;
            
        }
        set
        {
            numberOfHoldingObjects = value;
        }
    }
      

}
