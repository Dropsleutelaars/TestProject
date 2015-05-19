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
    private Rigidbody2D rigidBody;
    private Transform groundCheck;
    private Transform wallCheck;


#region PlayerVars

    public float dashPower = 1f;
    public float maxSpeed = 10f;
    public float jumpForce = 400f;
    public float groundedCheckRadius = 0.2f;
    public float dashBuffer;
    public float JumpSpeed = 10.0f;
    public int maxInventoryItems = 3;
    private bool isWallSliding;

#endregion

    public LayerMask groundMasks;
    public Transform inventory;
    public Vector3 inventoryOffset;
    

    /// <summary>
    /// This happens when the game 'Starts'
    /// Im setting the rigidbody variable to the rigidbody2d component.
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        groundCheck = transform.FindChild("GroundCheck");
        wallCheck = transform.FindChild("WallCheck");
    }

    /// <summary>
    /// This happens every frame in the game.
    /// Im catching the key input
    /// </summary>
    private void Update()
    {
        //Debug.Log(this.transform.position.y);
      //  Debug.Log(numberOfHoldingObjects);

        bool throwKeyDown = Input.GetKeyDown(KeyCode.Z);
        bool dashKeyDown = Input.GetKeyDown(KeyCode.X);
        bool jumpKeyDown = Input.GetKeyDown("space");
        float moveDirection = Input.GetAxis("Horizontal"); // -1 to 1
        bool grounded = false;

      

        #region Grounded check
        /******** Grounded check */
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedCheckRadius, groundMasks);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }

        Debug.Log(grounded);
       // Gizmos.DrawSphere()
        /********** end grounded check  */
        #endregion

        #region Animator variables
        anim.SetBool("Ground", grounded);
        anim.SetBool("IsGrounded", grounded);
        anim.SetFloat("vSpeed", rigidBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));
        anim.SetBool("isDashing", IsInDash);
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

        if (grounded && jumpKeyDown && anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            grounded = false;
            anim.SetBool("Ground", false);
            rigidBody.AddForce(new Vector2(0f, jumpForce));
        }
        else if (!grounded && jumpKeyDown &&
            (anim.GetAnimatorTransitionInfo(0).IsUserName("Jumping") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Jumping")))
        {
            anim.SetBool("DoubleJump", true);
            rigidBody.AddForce(new Vector2(0f, jumpForce));
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

        if (throwKeyDown)
        {
             throwItem();
        }

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsTag("Dash"));
        //Debug.Log("Is In Dash?" + IsInDash + " - " +   dashBuffer.x);

//        Debug.Log(this.transform.GetChild(4));
    }

    public void throwItem()
    {
        if (inventory.childCount == 0)
            return;

        Transform item = inventory.GetChild(0);

        item.GetComponent<Rigidbody2D>().isKinematic = false;
        item.GetComponent<Rigidbody2D>().AddForce((FacingRight ? transform.right : -transform.right) * 1000f);

        item.parent = null;

        SortInventory();
        StartCoroutine(ProjectileRoutine(item));
    }

    private IEnumerator ProjectileRoutine(Transform item)
    {
        yield return new WaitForSeconds(0.2f);
        item.GetComponent<CircleCollider2D>().enabled = true;
        item.GetComponent<CircleCollider2D>().isTrigger = false;

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
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        isWallSliding = false;
    }

    /*
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(groundCheck.transform.position, groundedCheckRadius);
    }
     * /
    
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
      

}
