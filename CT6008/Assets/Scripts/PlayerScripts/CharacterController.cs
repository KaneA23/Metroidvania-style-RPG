using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the character's movement dependent on the player's input
/// </summary>
public class CharacterController : MonoBehaviour
{
	[Header("Movement")]
	float moveSpeed = 3f;
	float moveHorizontal;

	[Header("Jumping")]
	bool isGrounded;
	float jumpForce = 35f;

	bool canDoubleJump;
	float jumpCount;

	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
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
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}
	}

	/// <summary>
	/// Moves character left and Right
	/// </summary>
	void PlayerMovement()
	{
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
		if (isGrounded || canDoubleJump)
		{
			rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			jumpCount++;

			StartCoroutine(JumpCooldown());
		}
	}

	/// <summary>
	/// If touching a platform then the player's jump is reset
	/// </summary>
	/// <param name="a_collision">Used to check if player is touching a platform</param>
	private void OnTriggerEnter2D(Collider2D a_collision)
	{
		if (a_collision.gameObject.tag == "Platform")
		{
			isGrounded = true;
			canDoubleJump = true;
			jumpCount = 0;
		}
	}

	/// <summary>
	/// Checks if the player is no longer jumping
	/// </summary>
	/// <param name="a_collision"></param>
	private void OnTriggerExit2D(Collider2D a_collision)
	{
		if (a_collision.gameObject.tag == "Platform")
		{
			isGrounded = false;
		}

	}

	/// <summary>
	/// Player has to wait before they can jump again
	/// </summary>
	/// <returns>Waits until the end of the frame before the player can jump again</returns>
	IEnumerator JumpCooldown()
	{
		yield return new WaitForEndOfFrame();

		if (jumpCount < 2)
		{
			canDoubleJump = true;
		}
		else
		{
			canDoubleJump = false;
		}
	}
}
