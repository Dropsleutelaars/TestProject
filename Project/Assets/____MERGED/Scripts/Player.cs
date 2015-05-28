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
    // References to some components or transforms of gameobjects.
    public Animator anim;
    private Rigidbody2D rigidBody;
    private Transform groundCheck;
    private Transform wallCheck;


#region PlayerVars

    public float dashPower = 1f;                // the strength of the Dash (not sure if its still in the game)
    public float maxSpeed = 10f;                // the maximum movement speed of the character.
    public float jumpForce = 400f;          
    //public float groundedCheckRadius = 0.2f;      Not used anymore, since we use a ray downwards the ground.
    public float dashBuffer;                    // This will indicate whether the player can dash or not.
    public float JumpSpeed = 10.0f;             // This is the strength of the jump of the player.
    public int maxInventoryItems = 3;           // This is used to store the number of maximum items that can be hold.
    private bool isWallSliding;                 // Just a boolean which represents if we are sliding against a wallslide
    private bool isJumping;                     // Just a boolean to check if we are NOT grounded. 
                                                // --- Could delete this too, and use !isGrounded
    private bool isDoubleJumping;               // Did the player used his double jump already?
    private bool isGrounded;                    // A boolean which checks if the player is grounded


    public bool isStunned = false;//are we stunned
   

#endregion

    public LayerMask groundMasks;
    public Transform inventory;
    public Vector3 inventoryOffset;


    public GameObject partSysGO;
    public ParticleSystem partSys;
    

    /// <summary>
    /// This happens when the game 'Starts'
    /// Im setting the rigidbody variable to the rigidbody2d component.
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        groundCheck = transform.FindChild("GroundCheck");
        wallCheck = transform.FindChild("WallCheck");

        partSys = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {

        if (this.transform.position.y < -10f)
            Application.LoadLevel(0);


        bool jumpKeyDown = Input.GetButtonDown("Jump");
        if (isGrounded && jumpKeyDown)
        {
            jump();
            
        }
        else if (!isGrounded && jumpKeyDown && !isDoubleJumping
            // && (anim.GetAnimatorTransitionInfo(0).IsUserName("Jumping") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Jumping"))
            )
        {
            rigidBody.velocity = new Vector2(0, 0); //Vector2.zero
            isDoubleJumping = true;

            int doubleJumpAnimation = Random.Range(0, 2);
            

            anim.SetInteger("doubleJAnimation", doubleJumpAnimation);
            jump();
            //rigidBody.AddForce(Vector2.up * -1  JumpSpeed, ForceMode2D.Impulse);
            //rigidBody.AddForce(new Vector2(0f, JumpSpeed), ForceMode2D.Impulse);
            //rigidBody.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse); 


            rigidBody.AddRelativeForce(Vector2.up * JumpSpeed, ForceMode2D.Force);
        }
    }


    /// <summary>
    /// This happens every frame in the game.
    /// Im catching the key input
    /// </summary>
    private void Update()
    {
        bool throwKeyDown = Input.GetKeyDown(KeyCode.Z);
        bool dashKeyDown = Input.GetKeyDown(KeyCode.X);
        
   
        float moveDirection = Input.GetAxis("Horizontal"); // -1 to 1
        isGrounded = false;
        
        
        #region Grounded check

        /******** Grounded check */
        // The player had a circlecast before, but i decided to just draw a ray downwards from the center of the feet.
        // Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedCheckRadius, groundMasks);

        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.transform.position, new Vector2(0, -0.1f));
       
            if (groundHit.collider != null)
            {
                if (groundHit.distance <= 0.1f)
                {
                    isGrounded = true;
                    
                }
                else
                {
                    isJumping = true;
                }
            }

        
        Debug.DrawRay(groundCheck.transform.position, new Vector2(0, -0.1f), Color.red);
        //Debug.Log(isGrounded);
     
       // Gizmos.DrawSphere()
        /********** end grounded check  */
        #endregion

        #region Animator variables
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("vSpeed", rigidBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));
        anim.SetBool("isDashing", IsInDash);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isDoubleJumping", isDoubleJumping);
        
        #endregion

        float dashMovement = (dashBuffer * 0.1f);
        dashBuffer -= dashMovement;

        float xMovement = IsInDash ? moveDirection + dashMovement * maxSpeed : moveDirection * maxSpeed;

        rigidBody.velocity = new Vector2(xMovement, rigidBody.velocity.y);

       
        // If the input is moving the player right and the player is facing left...
        if (moveDirection > 0.1f && !FacingRight)
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveDirection < -.1f && FacingRight)
            Flip();

       if (rigidBody.velocity.x == 0)
       {
           partSys.Stop();
       }
       else
       {
           partSys.Play();
       }

        if (isGrounded)
        {
            isJumping = false;
            isDoubleJumping = false;
        }

        if (!IsInDash && dashKeyDown)
        {
            // Versimpelde if-statement. 
            // Facingright == true? --> dashpower
            // Facingright == false? --> -dashpower
            // Dit gbruik ik om de juiste directie op de dashen.

            dashBuffer = FacingRight ? dashPower : -dashPower;
        }

        if (throwKeyDown)
        {
             throwItem();
        }

        //Debug.Log(isJumping);
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsTag("Dash"));
        //Debug.Log("Is In Dash?" + IsInDash + " - " +   dashBuffer.x);

//        Debug.Log(this.transform.GetChild(4));
    }
    public void jump()
    {
        // Add a vertical force to the player.
        isGrounded = false;
        isJumping = true;

        rigidBody.AddForce(new Vector2(0f, JumpSpeed), ForceMode2D.Impulse);

        /*
            rigidBody.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
            rigidBody.AddForce(new Vector2(0f, jumpForce));
         */
    }
    public void throwItem()
    {
        if (inventory.childCount == 0)
            return;

        Transform item = inventory.GetChild(0);

        item.GetComponent<Rigidbody2D>().isKinematic = false;
        item.GetComponent<CircleCollider2D>().isTrigger = true;
        item.GetComponent<Rigidbody2D>().gravityScale = 0;
        item.GetComponent<Rigidbody2D>().AddForce((FacingRight ? transform.right : -transform.right) * 450f);

        item.parent = null;

        SortInventory();
        StartCoroutine(ProjectileRoutine(item));
    }

    private IEnumerator ProjectileRoutine(Transform item)
    {
        yield return new WaitForSeconds(0.25f);
        item.GetComponent<CircleCollider2D>().enabled = true;
        

    }

    /// <summary>
    /// This is an accessor
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
            PickupItem(other.transform);
            #region old inventory system
            /*
            float margin = 3.5f; 

            if (NumberOfHoldingObjects > 0)
            {
                margin = ((NumberOfHoldingObjects * 0.6f) + margin);
            }

            Destroy(other.gameObject);

            GameObject pickup = Instantiate(other.gameObject, this.transform.position, this.transform.rotation) as GameObject;

            pickup.transform.parent = this.transform;

            pickup.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + margin);
            */
            //holdingObjects.Add((GameObject)pickup);
            //numberOfHoldingObjects = holdingObjects.Count;        
            #endregion
        }
    }

    private void PickupItem(Transform item)
    {
        if (item.childCount >= maxInventoryItems)
            return;

        item.parent = inventory;
        //Stop the Collision to prevent its adding points while you double jump?!
        item.GetComponent<CircleCollider2D>().enabled = false;

        SortInventory();
    }

    private void SortInventory()
    {
        for(int i = 0; i < inventory.childCount; i++)
        {
            Transform item  = inventory.GetChild(i);

            item.GetComponent<Rigidbody2D>().isKinematic = true;
            item.localPosition = inventoryOffset * i;
        }
    }

// This is currently using Davids' code.
    private void wallJump()
    {
        rigidBody.AddForce(new Vector2(0f, JumpSpeed));
        //rigidBody.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        isWallSliding = false;
    }

    #region Commented (old)
    /*
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(groundCheck.transform.position, groundedCheckRadius);
    }
    */
    
// This is the end of the walljump

    /*
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
    */
    #endregion

    public void stunEnemy() //Stun an enemy with a specific time.
    {
        if (anim != null)
        {
            anim.speed = 0;
        }

        isStunned = true;

    }

   



}
