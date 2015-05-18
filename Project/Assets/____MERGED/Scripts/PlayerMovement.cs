using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{

    // speler beweging
    public float Speed = 10.0f;
    public float JumpSpeed = 10.0f;
    // systeem
    private Rigidbody2D rigidbody2D;
    public LayerMask GroundLayers;
    public LayerMask WallLayers;
    // fields
    private Animator m_Animator;
    private Transform m_GroundCheck;
    private Transform m_WallCheck;



    private bool isWallSliding;


    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_GroundCheck = transform.FindChild("GroundCheck");
        m_WallCheck = transform.FindChild("WallCheck");
    }

   

    void wallJump()
    {
        rigidbody2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        isWallSliding = false;
    }

    void Update()
    {
        isWallSliding = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Debug.Log(rigidbody2D.velocity);
        
        isWallSliding = Physics2D.OverlapPoint(m_WallCheck.position, WallLayers);
        bool isGrounded = Physics2D.OverlapPoint(m_GroundCheck.position, GroundLayers);
        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                rigidbody2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        if (isWallSliding)
        {
           // Debug.Log("Is wall sliding");
            rigidbody2D.velocity = new Vector2(0f, -1f);

            if (
                (Input.GetAxis("Horizontal") > 0) && (this.transform.localScale == new Vector3(-1.0f, 1.0f, 1.0f)) 
                ||
                (Input.GetAxis("Horizontal") < 0) && (this.transform.localScale == new Vector3(1.0f, 1.0f, 1.0f))
                )
            {
                wallJump();
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                rigidbody2D.velocity = new Vector2(0f, -7f);
            }
        }
      

//         // WALLJUMP
//         if (Input.GetButton("Jump"))
//         {
//             if (isWallSliding)
//             {
//                 rigidbody2D.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
//                 isWallSliding = false;
//             }
//         }


        m_Animator.SetBool("IsGrounded", isGrounded);
        float hSpeed = (Input.GetAxis("Horizontal"));


        Debug.Log(hSpeed);

        m_Animator.SetFloat("Speed", Mathf.Abs(hSpeed));

        if (hSpeed > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (hSpeed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        this.rigidbody2D.velocity = new Vector2(hSpeed * Speed, this.rigidbody2D.velocity.y);
    }

    
}
