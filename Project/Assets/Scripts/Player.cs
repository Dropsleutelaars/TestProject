using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity 
{
    public Animator anim;
    private Rigidbody2D rigidbody;

    public float maxSpeed = 10f;
    public float jumpForce = 400f;
    public float groundedCheckRadius = 0.2f;
    public LayerMask groundMasks;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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

        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);

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
    }
}
