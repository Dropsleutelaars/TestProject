using UnityEngine;
using System.Collections;

public class playerStates : MonoBehaviour 
{

	public enum charStates
    {
        Idle,
        Run,
        Jump,
        DJump1,
        DJump2,
        Throw,
        Wallslide,
        Falling
    }
    charStates mCurrentState = charStates.Idle;

    public GameObject myPlayer;
    public Player myPlayerScript;
    
    //public EnemyChase chaseScript;

    void Start()
    {
        myPlayerScript = myPlayer.GetComponent<Player>();
        
        // chaseScript = GetComponent<chaseScript>;
        
    }
    //StartCoroutine(StateMachineBase.WaitForAnimation());
    void Update()
    {
        if (transform.position.x <= myPlayer.transform.position.x) 
        {
            //myPlayerScript.
        }
    }

    void changeState(charStates newState)
    {
        Animator myPlayerAnimator = myPlayer.GetComponent<Animator>();

        if (mCurrentState == newState)
        {
            return;
        }

        switch(newState)
        {
            case charStates.Idle:
            {
                myPlayerAnimator.Play("ReitseIdle");        
                break;
            }
            case charStates.Run:
            {
                myPlayerAnimator.Play("ReitseRun");
                break;
            }
            case charStates.Jump:
            {
                myPlayerAnimator.Play("ReitseJump");
                break;
            }
            case charStates.DJump1:
            {
                myPlayerAnimator.Play("ReitseDJ1");
                break;
            }
            case charStates.DJump2:
            {
                myPlayerAnimator.Play("ReitseDJ2");
                break;
            }
            case charStates.Falling:
            {
                myPlayerAnimator.Play("ReitseFall");
                break;
            }
            case charStates.Wallslide:
            {
                myPlayerAnimator.Play("ReitseWallSlide");
                break;
            }
            case charStates.Throw:
            {
                myPlayerAnimator.Play("ReitseThrow");
                break;
            }

            default:
            {
               return;
            }
        }

        mCurrentState = newState;
    }

    
        
        
    /*    
    IEnumerator IdleState()
    {
        Debug.Log("Idle - Entered");
        while (myState == charStates.Idle)
        {
            yield return null;
        }
        Debug.Log("Exit Idle");
        NextState();
    }

    void NextState()
    {
        string methodName = myState.ToString() + "State";
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }
     */
}
