using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the character's movement dependent on the player's input
/// </summary>
public class CharacterController : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed;
	public float moveHorizontal;
	public float defaultSpeed = 1f;
	public float runSpeed = 1.25f;      // Also makes jumps higher
	public float crouchSpeed = 0.75f;   // Also makes jumps shorter

	[Range(0f, 1f)]
	public float horizontalDamping = 0.75f;

	[Header("Jumping")]
	public bool isGrounded;
	public float jumpForce = 20f;
	[Range(0f, 1f)]
	public float jumpHeight;

	public float radius = 0.1f;

	public Transform groundCheck;
	public LayerMask groundLayer;

	[Header("Double jump")]
	public bool isDoubleJumpActive;
	public bool canDoubleJump;
	public float jumpCount;

	[Header("Crouch")]
	public bool isCeiling;
	public bool isCrouching;

	public Transform ceilingCheck;
	public LayerMask ceilingLayer;

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

		PlayerInput();
	}

	private void FixedUpdate()
	{
		PlayerMovement();
	}

	/// <summary>
	/// Checks player's input to move character
	/// </summary>
	void PlayerInput()
	{
		// Checks what direction the player is wanting to move
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		// Cuts off jump height when player releases jump button
		if (Input.GetButtonUp("Jump"))
		{
			if (rb.velocity.y > 0)
			{
				rb.velocity = jumpForce * jumpHeight * Vector2.up;
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
	}

	/// <summary>
	/// Moves character left and Right
	/// </summary>
	void PlayerMovement()
	{
		moveHorizontal *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

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
	}

	/// <summary>
	/// Checks player can jump and how many times
	/// </summary>
	void Jump()
	{
		if (isGrounded)
		{
			rb.velocity = jumpForce * moveSpeed * Vector2.up;
			jumpCount++;
			StartCoroutine(JumpCooldown());
		}
		else if (canDoubleJump && isDoubleJumpActive)
		{
			rb.velocity = jumpForce * moveSpeed * Vector2.up;
			jumpCount++;
			StartCoroutine(JumpCooldown());
		}
	}

	/// <summary>
	/// Checks whether the player is touching a platform to allow them to jump
	/// </summary>
	private void CheckIfGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

		// Resets double jump if the player is touching the ground
		if (isGrounded)
		{
			canDoubleJump = true;
			jumpCount = 0;
		}
	}

	/// <summary>
	/// Checks whether a crouched player is under a platform to see if they can stand up
	/// </summary>
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
