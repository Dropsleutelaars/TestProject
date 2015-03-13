using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(movingCharacters))]
    public class characterUserControl : MonoBehaviour
    {
        private movingCharacters m_Character;
        private bool m_Jump;

        public float speed = 5;
        public Transform graphics;

        public SkeletonAnimation reitseSpine;

        Quaternion goalRotation = Quaternion.identity;
        float xMovement;

        string currentAnimation = "";

        private void Awake()
        {
            m_Character = GetComponent<movingCharacters>();
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
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
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                //SetAnimation("Jump_Fall", false);
            }
        }

        void SetAnimation(string name, bool loop)
        {
            if (name == currentAnimation)
                return;

            reitseSpine.state.SetAnimation(0, name, loop);
            currentAnimation = name;
            Debug.Log(reitseSpine.state);
        }
        private void FixedUpdate()
        {
            // Read the inputs.
            //bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, m_Jump);
            m_Jump = false;

            Debug.Log("Variabele H: " + h);
        } 
    }
}
