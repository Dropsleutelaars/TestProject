using UnityEngine;
using System.Collections;

public class ReitseTestScript : MonoBehaviour {

    [SpineAnimation]
    public string idleAnimation;

    [SpineAnimation]
    public string runningAnimation;


    [Range(0f, 10f)]
    public float moveSpeed;

    private SkeletonAnimation reitseSkeleton;

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
