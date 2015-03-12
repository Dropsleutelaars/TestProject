using UnityEngine;
using System.Collections;

public class ReitseTestScript : MonoBehaviour {

    [SpineAnimation]
    public string idleAnimation;
    [SpineAnimation]
    public string runningAnimation;

    [SerializeField]
    private bool airControl = true; 
    [Range(0f, 10f)]
    public float moveSpeed;

    private SkeletonAnimation reitseSkeleton;

    private Transform playerGroundCheck;    // A position marking where to check if the player is grounded.
    const float playerGroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool playerGrounded;            // Whether or not the player is grounded.
    private Transform playerCeilingCheck;   // A position marking where to check for ceilings
    const float playerCeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    void Awake()
    {
        reitseSkeleton = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        if((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
        {
            //move right
            reitseSkeleton.AnimationName = runningAnimation;
            reitseSkeleton.skeleton.FlipX = false;
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        else if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
        {
            //move left
            reitseSkeleton.AnimationName = runningAnimation;
            reitseSkeleton.skeleton.FlipX = true;
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            reitseSkeleton.AnimationName = idleAnimation;
        }

    }


}
