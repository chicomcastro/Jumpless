using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool isAttacking;

		private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump && !isAttacking)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = Input.GetButtonDown("Jump");
			}
			
			if (!isAttacking && Input.GetKeyDown(KeyCode.E))
			{
				isAttacking = true;
				m_Character.SetAttack (isAttacking);
				m_Character.Attack();
			}
		}


        private void FixedUpdate()
        {
			// Read the inputs.
			bool crouch = false; // Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
	        m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;

			if (isAttacking && !Input.GetKeyDown(KeyCode.E))
			{
				isAttacking = false;
			}
		}
    }
}
