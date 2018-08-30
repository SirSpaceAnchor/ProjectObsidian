using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PO
{
	public class PlayerMovement : MonoBehaviour
	{
		public float moveInputHorizontal;
		public float moveInputVertical;

		public bool isGrounded;
		private bool isJumping;


		public float speed;
		public float climbSpeed;

		public bool isClimbing;


		public Rigidbody2D rb;

		//Player Facing Direction
		public bool facingRight = true;

		//Grounding
		public Transform groundCheck;
		public float checkRadius;
		public LayerMask whatIsGround;
		public LayerMask whatisLadder;

		private int extraJumps;
		private float jumpTimeCounter;
		public int extraJumpsValue;
		public float jumpTime;
		public float jumpForce;
		public float distance;

		public void StartMovement()
		{
		extraJumps = extraJumpsValue;
		rb = GetComponent<Rigidbody2D>();
		}


		public void MovementUpdate()
		{

		}

		public void ChargeJump()
		{
			if(isGrounded == true && Input.GetButtonDown("Jump"))
			{
				isJumping = true;
				jumpTimeCounter = jumpTime;
				rb.velocity = Vector2.up * jumpForce;
			}

			if(Input.GetButton("Jump") && isJumping == true)
			{
				if(jumpTimeCounter > 0)
				{
					rb.velocity = Vector2.up * jumpForce;
					jumpTimeCounter -= Time.deltaTime;
				}
				else
				{
					isJumping = false;
				}
			}
			if(Input.GetButtonUp("Jump"))
			{
				isJumping = false;
			}
		}
		
		public void MultiJump()
		{
			if(isGrounded == true)
			{
				extraJumps = extraJumpsValue;
			}

			if(Input.GetButtonDown("Jump") && extraJumps > 0)
			{
				rb.velocity = Vector2.up * jumpForce;
				extraJumps--;
			}
			else if(Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
			{
				rb.velocity = Vector2.up * jumpForce;
			}
		}
		
		public void Climb()
		{
			RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatisLadder);

			if(hitInfo.collider != null)
			{
				if(moveInputVertical >= .25)
				{
					isClimbing = true;
					//Debug.Log("Climbing...");
				}
			}
			else
			{
				if(moveInputHorizontal >= .1 || moveInputHorizontal <= .1)
				isClimbing = false;
			}

			if(isClimbing == true && hitInfo.collider != null)
			{
				rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * climbSpeed);
				rb.gravityScale = 0;
			}
			else
			{
				rb.gravityScale = 1;
			}
		}
		
		public void Flip()
		{
			facingRight = !facingRight;
			Vector3 Scaler = transform.localScale;
			Scaler.x *= -1;
			transform.localScale = Scaler;
		}
	}
}