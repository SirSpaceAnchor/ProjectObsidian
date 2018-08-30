using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PO
{
	public class PlayerController : MonoBehaviour
	{
		private float moveInputHorizontal;
		private float moveInputVertical;

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
		private int direction;
		private float jumpTimeCounter;
		public int extraJumpsValue;
		public float jumpTime;
		public float jumpForce;
		public float distance;
		public float dashSpeed;
		public float dashTime;
		public float startDashTime;

		void Start()
		{
			extraJumps = extraJumpsValue;
			rb = GetComponent<Rigidbody2D>();
			dashTime = startDashTime;
		}

		void FixedUpdate()
		{
			isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

			moveInputHorizontal = Input.GetAxisRaw("Horizontal");
			moveInputVertical = Input.GetAxisRaw("Vertical");

			rb.velocity = new Vector2(moveInputHorizontal * speed, rb.velocity.y);
			
			//FACING UPDATE
			if(facingRight == false && moveInputHorizontal > 0)
			{
				Flip();
			}
			else if(facingRight == true && moveInputHorizontal < 0)
			{
				Flip();
			}
			Climb();
		}

		public void Update()
		{
			MultiJump();
			ChargeJump();
		//	Dash();
		}
		public void ChargeJump()
		{
			if(isGrounded == true && Input.GetButtonDown("Jump"))
			{
				isJumping = true;
				jumpTimeCounter = jumpTime;
				rb.velocity = Vector2.up * jumpForce;
			}

			if(Input.GetButtonDown("Jump") && isJumping == true)
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
			if(Input.GetButtonDown("Jump"))
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

		/*
		public void Dash()
		{
			if(direction == 0)
			{
				if(moveInputHorizontal >= 1 && Input.GetButtonDown("Fire1"))
				{
					direction = 1;
				}
				if(moveInputHorizontal <= 1 && Input.GetButtonDown("Fire1"))
				{
					direction = 2;
				}
				if(moveInputVertical >= 1 && Input.GetButtonUp("Fire1"))
				{
					direction = 3;
				}
				if(moveInputVertical <= 1 && Input.GetButtonUp("Fire1"))
				{
					direction = 4;
				}
				
			}
			else
			{
				if(dashTime <= 0)
				{
					direction = 0;
					dashTime = startDashTime;
					rb.velocity = Vector2.zero;
				}
				else
				{
					dashTime -= Time.deltaTime;
					rb.gravityScale = 0;
					if(direction == 1)
					{
						rb.velocity = new Vector2(rb.velocity.y, moveInputHorizontal * dashSpeed);
					}
					else if(direction == 2)
					{
						rb.velocity = new Vector2(rb.velocity.y, moveInputHorizontal * dashSpeed);
					}
					/*else if(direction == 3)
					{
						rb.velocity = new Vector2(rb.velocity.y, moveInputVertical * dashSpeed);
					}
					else if(direction == 4)
					{
						rb.velocity = new Vector2(rb.velocity.y, moveInputVertical * dashSpeed);
					}
				}
			}
		}
		*/
		public void Flip()
		{
			facingRight = !facingRight;
			Vector3 Scaler = transform.localScale;
			Scaler.x *= -1;
			transform.localScale = Scaler;
		}
	}
}