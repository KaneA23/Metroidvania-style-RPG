using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the character's movement dependent on the player's input
/// </summary>
public class CharacterController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed;
	public float moveHorizontal;
	public float defaultSpeed = 2f;
	public float runSpeed = 3f;
	public float crouchSpeed = 1.5f;

	[Header("Jumping")]
	public bool isGrounded;
	public float jumpForce = 5f;
	public float radius = 0.1f;

	[Range(0f, 1f)]
	public float jumpHeight;

	public Transform groundCheck;
	public LayerMask groundLayer;

	[Header("Double jump")]
	public bool hasDoubleJump;
	public bool canDoubleJump;
	public float jumpCount;

	[Header("Crouch")]
	public bool isCeiling;
	public bool isCrouching;

	public Transform ceilingCheck;
	public LayerMask ceilingLayer;

	BoxCollider2D headCollider;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

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
		PlayerInput();
		CheckIfGrounded();
		CheckIfCeiling();
	}

	private void FixedUpdate()
	{
		PlayerMovement();
		//Jump();
	}

	/// <summary>
	/// Checks player's input to move character
	/// </summary>
	void PlayerInput()
	{
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		if (Input.GetKeyDown(KeyCode.W))
		{
			Jump();
		}

		if (Input.GetKeyUp(KeyCode.W))
		{
			if (rb.velocity.y > 0)
			{
				rb.velocity = jumpForce * jumpHeight * Vector2.up;
			}

			Debug.Log("Jump Magnitude: " + rb.velocity.magnitude);
		}

		if (Input.GetKey(KeyCode.LeftControl))
		{
			Crouch();
		}
		else if (!isCeiling)
		{
			Uncrouch();

		}
	}

	/// <summary>
	/// Moves character left and Right
	/// </summary>
	void PlayerMovement()
	{
		//moveHorizontal = Input.GetAxisRaw("Horizontal");

		// Checks which way the player should be facing
		if (moveHorizontal > 0.1f)
		{
			spriteRenderer.flipX = true;
		}
		else if (moveHorizontal < -0.1f)
		{
			spriteRenderer.flipX = false;
		}

		// Moves player accross X-axis
		rb.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
		//rb.velocity = moveHorizontal * moveSpeed * Vector2.right;
	}

	/// <summary>
	/// Checks player can jump and how many times
	/// </summary>
	void Jump()
	{
		if (isGrounded)
		{
			rb.velocity = jumpForce * moveSpeed * Vector2.up;
			//if (isCrouching)
			//{
			//	//rb.AddForce(new Vector2(0f, jumpForce * crouchSpeed), ForceMode2D.Impulse);
			//	rb.velocity = Vector2.up * jumpForce * crouchSpeed;
			//}
			//else
			//{
			//	//rb.AddForce(new Vector2(0f, jumpForce * defaultSpeed), ForceMode2D.Impulse);
			//	rb.velocity = Vector2.up * jumpForce * defaultSpeed;
			//}

			jumpCount++;

			StartCoroutine(JumpCooldown());
		}
		else if (canDoubleJump && hasDoubleJump)
		{
			//rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			rb.velocity = jumpForce * moveSpeed * Vector2.up;
			jumpCount++;

			StartCoroutine(JumpCooldown());
		}

		Debug.Log("Jump Height:" + rb.velocity.magnitude);
	}

	/// <summary>
	/// Disables head collider so player can fit through short gaps
	/// </summary>
	void Crouch()
	{
		headCollider.enabled = false;
		isCrouching = true;
		moveSpeed = crouchSpeed;
	}

	/// <summary>
	/// Enables head collider so player is standing up
	/// </summary>
	void Uncrouch()
	{
		headCollider.enabled = true;
		isCrouching = false;
		moveSpeed = defaultSpeed;
	}

	//void ToggleCrouch()
	//{
	//	headCollider.enabled = !headCollider.enabled;
	//	isCrouching = !isCrouching;
	//}

	///// <summary>
	///// If touching a platform then the player's jump is reset
	///// </summary>
	///// <param name="a_collision">Used to check if player is touching a platform</param>
	//private void OnTriggerEnter2D(Collider2D a_collision)
	//{
	//	if (a_collision.gameObject.tag == "Platform")
	//	{
	//		isGrounded = true;
	//		canDoubleJump = true;
	//		jumpCount = 0;
	//	}
	//}

	/// <summary>
	/// Checks whether the player is touching a jumpable surface
	/// </summary>
	/// <param name="a_collision">Is the object you are standing on a platform</param>
	//private void OnCollisionEnter2D(Collision2D a_collision)
	//{
	//	if (a_collision.gameObject.tag == "Platform")
	//	{
	//		isGrounded = true;
	//		canDoubleJump = true;
	//		jumpCount = 0;
	//	}
	//}

	///// <summary>
	///// Checks if the player is no longer jumping
	///// </summary>
	///// <param name="a_collision"></param>
	//private void OnTriggerExit2D(Collider2D a_collision)
	//{
	//	if (a_collision.gameObject.tag == "Platform")
	//	{
	//		isGrounded = false;
	//	}
	//}

	/// <summary>
	/// Checks whether the player is mid air
	/// </summary>
	/// <param name="a_collision">Checks whether the player is touching a platform</param>
	//private void OnCollisionExit2D(Collision2D a_collision)
	//{
	//	if (a_collision.gameObject.tag == "Platform")
	//	{
	//		isGrounded = false;
	//	}
	//}

	private void CheckIfGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

		if (isGrounded)
		{
			canDoubleJump = true;
			jumpCount = 0;
		}
	}

	private void CheckIfCeiling()
	{
		isCeiling = Physics2D.OverlapCircle(ceilingCheck.position, radius, ceilingLayer);
	}

	/// <summary>
	/// Player has to wait before they can jump again
	/// </summary>
	/// <returns>Waits until the end of the frame before the player can jump again</returns>
	IEnumerator JumpCooldown()
	{
		yield return new WaitForEndOfFrame();

		if (jumpCount < 0)
		{
			canDoubleJump = true;
		}
		else
		{
			canDoubleJump = false;
		}
	}
}
