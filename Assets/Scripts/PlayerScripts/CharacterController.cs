using System.Collections;
using UnityEngine;

/// <summary>
/// Controls character's movement dependent on player's input.
/// Created by: Kane Adams
/// </summary>
public class CharacterController : MonoBehaviour
{

	public BasePlayerClass BPC;

	[Header("Movement")]
	public float moveHorizontal;
	public float moveSpeed;
	public float defaultSpeed = 1f;
	public float runSpeed = 1.25f;
	public float crouchSpeed = 0.75f;

	public bool isFacingRight = false;

	[Tooltip("Acceleration decreases the closer to 1")]
	[Range(0f, 1f)]
	public float horizontalDamping = 0.75f;

	[Header("Crouch")]
	public bool isCeiling;
	public bool isCrouching;

	[Header("Jumping")]
	public bool isGrounded;
	public float jumpForce = 25f;

	[Tooltip("Gap between highest and lowest jump decreases as value increases")]
	[Range(0f, 1f)]
	public float cutJumpHeight;

	[Space(10)]
	public bool isDoubleJumpActive;
	public bool canDoubleJump;
	public float jumpCount;

	[Space(10)]
	public bool isWallJumpActive;
	public bool isTouchingWall;
	public bool canWallJump;
	public float wallJumpForce = 50f;

	[Header("Dash")]
	public bool isDashActive;
	public float dashDistance = 15f;
	public bool canDash;
	public bool isDashing;

	[Header("Checks")]
	public float checkRadius = 0.1f;

	[Space(5)]
	public Transform ceilingCheck;
	[Tooltip("Objects player needs to crouch under")]
	public LayerMask ceilingLayers;

	[Space(5)]
	public Transform groundCheck;
	[Tooltip("Objects that can be jumped off")]
	public LayerMask groundLayers;

	[Space(5)]
	public Transform wallCheck;
	[Tooltip("Objects that can be wall jumped")]
	public LayerMask wallLayers;

	BoxCollider2D headCollider;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;  // Used to switch character direction

	private void Awake()
	{
		headCollider = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Start is called before the first frame update
	void Start()
	{
		headCollider.enabled = true;
		isCrouching = false;
		moveSpeed = defaultSpeed;
		jumpCount = 0;



	}

	// Update is called once per frame
	void Update()
	{
		CheckIfGrounded();
		CheckIfCeiling();
		CheckIfWall();

		PlayerInput();
	}

	private void FixedUpdate()
	{
		if (!isDashing)
		{
			PlayerMovement();
		}
	}

	/// <summary>
	/// Checks player's input to move character
	/// </summary>
	void PlayerInput()
	{
		// Checks what direction the player is wanting to move
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		// Checks which way the player should be facing
		if ((moveHorizontal > 0 && !isFacingRight) || (moveHorizontal < 0 && isFacingRight))
		{
			//spriteRenderer.flipX = true;    //right
			transform.Rotate(new Vector2(0, 180));
			isFacingRight = !isFacingRight;
		}

		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		// Cuts off jump height when player releases jump button
		if (Input.GetButtonUp("Jump"))
		{
			if (rb.velocity.y > 0)
			{
				rb.velocity = jumpForce * cutJumpHeight * Vector2.up;
			}
		}

		// Removes head collider if player wants to crouch
		if (Input.GetKey(KeyCode.LeftControl))
		{
			headCollider.enabled = false;
			isCrouching = true;
		}
		else if (!isCeiling)
		{
			headCollider.enabled = true;
			isCrouching = false;
		}

		// Changes player speed depending on whether you are running, walking or crouching
		if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
		{
			moveSpeed = runSpeed;
		}
		else if (isCrouching)
		{
			moveSpeed = crouchSpeed;
		}
		else
		{
			moveSpeed = defaultSpeed;
		}

		// Dashing
		if (Input.GetButtonDown("Dash") && !isCrouching && !isDashing && canDash)
		{
			if (isFacingRight)
			{
				StartCoroutine(Dash(1));    //dash right
			}
			else
			{
				StartCoroutine(Dash(-1));	// dash left
			}
		}
	}

	/// <summary>
	/// Moves character left and Right
	/// </summary>
	void PlayerMovement()
	{
		moveHorizontal *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

		// Moves player across X-axis
		if (isGrounded || !isTouchingWall)
		{
			rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
		}
	}

	/// <summary>
	/// Checks whether a crouched player is under a platform to see if they can stand up
	/// </summary>
	private void CheckIfCeiling()
	{
		isCeiling = Physics2D.OverlapCircle(ceilingCheck.position, checkRadius, ceilingLayers);
	}

	#region Jump Mechanics

	/// <summary>
	/// Checks player can jump and how many times
	/// </summary>
	void Jump()
	{
		if (isGrounded)
		{
			rb.velocity = jumpForce * Vector2.up;//* moveSpeed
			jumpCount++;

			StartCoroutine(JumpCooldown());
		}
		else if (canDoubleJump && isDoubleJumpActive)
		{
			rb.velocity = jumpForce * Vector2.up;//* moveSpeed 
			jumpCount++;

			StartCoroutine(JumpCooldown());
		}
		else if (isWallJumpActive && isTouchingWall && canWallJump)
		{
			rb.velocity = new Vector2(wallJumpForce * -moveHorizontal, jumpForce);//* moveSpeed
																				  //.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
																				  //rb.AddForce(new Vector2(wallJumpForce * -moveHorizontal, jumpForce), ForceMode2D.Impulse);
			jumpCount = 0;
			canWallJump = false;

			StartCoroutine(JumpCooldown());
		}
	}

	/// <summary>
	/// Checks whether the player is touching platform to allow jump
	/// </summary>
	private void CheckIfGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayers);

		// Resets double jump if the player is touching the ground
		if (isGrounded)
		{
			canDoubleJump = true;
			canWallJump = true;
			canDash = true;
			jumpCount = 0;
		}
	}

	/// <summary>
	/// Checks whether the player is touching the wall to allow wall jump
	/// </summary>
	private void CheckIfWall()
	{
		isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayers);
	}

	/// <summary>
	/// Player has to wait before they can jump again
	/// </summary>
	/// <returns>Waits until the end of the frame before the player can jump again</returns>
	IEnumerator JumpCooldown()
	{
		yield return new WaitForEndOfFrame();

		if (jumpCount < 1)
		{
			canDoubleJump = true;
		}
		else
		{
			canDoubleJump = false;
		}
	}

	#endregion

	/// <summary>
	/// Controls dash mechanic
	/// </summary>
	/// <param name="a_direction">Direction that the player is wanted to dash</param>
	/// <returns>1 second wait between dashes</returns>
	IEnumerator Dash(int a_direction)
	{
		isDashing = true;
		canDash = false;
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(new Vector2(dashDistance * a_direction, 0), ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		isDashing = false;
	}
}
