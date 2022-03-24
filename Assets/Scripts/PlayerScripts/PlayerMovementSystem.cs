using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls character's movement dependent on player's input.
/// Created by: Kane Adams
/// </summary>
public class PlayerMovementSystem : MonoBehaviour
{
	public BasePlayerClass BPC;
	public PlayerCombatSystem PCS;
	public PlayerHealthSystem PHS;

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
	public bool isJumping;
	public float jumpForce = 25f;
	float jumpDelay;

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
	public float dashDistance = 25f;
	public bool canDash;
	public bool isDashing;

	[SerializeField]
	Image manaCooldownUI;

	public bool isManaCooldown;
	public float cooldownTimer;
	public float manaCooldownTime = 0.5f;


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

	public Animator anim;

	// Animation States
	[SerializeField]
	private string currentAnimState;

	const string PLAYER_IDLE = "Player_Idle";
	const string PLAYER_WALK = "Player_Walk";
	const string PLAYER_RUN = "Player_Run";
	const string PLAYER_JUMPLAUNCH = "Player_JumpLaunch";
	const string PLAYER_JUMPFALL = "Player_JumpFall";
	const string PLAYER_JUMPLAND = "Player_JumpLand";
	const string PLAYER_DASH = "Player_Dash";
	// string PLAYER_SWORDATTACK = "Player_SwordAttack";

	private void Awake()
	{
		PCS = GetComponent<PlayerCombatSystem>();
		PHS = GetComponent<PlayerHealthSystem>();

		headCollider = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		anim = GetComponentInChildren<Animator>();
	}

	// Start is called before the first frame update
	void Start()
	{
		headCollider.enabled = true;
		isCrouching = false;
		moveSpeed = defaultSpeed;
		jumpCount = 0;

		isManaCooldown = false;
		manaCooldownUI.fillAmount = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isGrounded && !isJumping && !PCS.isAttacking && !PHS.isHit && !isDashing)
		{
			//anim.Play(PLAYER_JUMPFALL);
			ChangeAnimationState(PLAYER_JUMPFALL);
		}

		CheckIfGrounded();
		CheckIfCeiling();
		CheckIfWall();

		if (!PHS.isHit)
		{
			PlayerInput();
		}
	}

	private void FixedUpdate()
	{
		if (!isDashing && !PHS.isHit)
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

		if (Input.GetButtonDown("Jump") && jumpCount < 1/*&& isGrounded*/)
		{
			//anim.Play(PLAYER_JUMPLAUNCH);
			ChangeAnimationState(PLAYER_JUMPLAUNCH);
			isJumping = true;

			Invoke("CompleteJumpAnim", 0.5f);
			//StartCoroutine(JumpAnimation(anim.GetCurrentAnimatorStateInfo(0).length));
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

		//if (isManaCooldown)
		//{
		//	ApplyCooldown();
		//}
		//else
		//{
		// Dashing
		if (Input.GetButtonDown("Dash") && !isCrouching && !isDashing && canDash)
		{
			if (isFacingRight)
			{
				//StartCoroutine(Dash(1));    //dash right
				Dash(1);
			}
			else
			{
				Dash(-1);
				//StartCoroutine(Dash(-1));   // dash left
			}
		}
		//}
	}

	/// <summary>
	/// Moves character left and Right
	/// </summary>
	void PlayerMovement()
	{
		moveHorizontal *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

		//if (moveHorizontal == 0 && isGrounded)
		//{
		//	anim.Play(PLAYER_IDLE);
		//}
		//else
		//{
		// Moves player across X-axis
		if (!PCS.isAttacking)
		{
			if (isGrounded || !isTouchingWall)
			{
				//anim.Play(PLAYER_RUN);
				rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
			}
		}
		//}

		if (isGrounded && !PCS.isAttacking && !isJumping && !PHS.isDying)
		{
			if (moveHorizontal != 0)
			{
				//anim.Play(PLAYER_RUN);
				if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
				{
					ChangeAnimationState(PLAYER_RUN);
				}
				else
				{
					ChangeAnimationState(PLAYER_WALK);
				}
			}
			else
			{
				//anim.Play(PLAYER_IDLE);
				ChangeAnimationState(PLAYER_IDLE);
			}
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
			//anim.Play(PLAYER_JUMPLAUNCH);
			//Debug.Log("Jump1");
			//StartCoroutine(JumpAnimation(anim.GetCurrentAnimatorStateInfo(0).length));

			rb.velocity = jumpForce * Vector2.up;//* moveSpeed
			jumpCount++;

			StartCoroutine(JumpCooldown());
		}
		else if (isWallJumpActive && isTouchingWall && canWallJump)
		{
			//anim.Play(PLAYER_JUMPLAUNCH);
			//StartCoroutine(JumpAnimation(anim.GetCurrentAnimatorStateInfo(0).length));

			//rb.velocity = new Vector2(wallJumpForce * -moveHorizontal, jumpForce);//* moveSpeed
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
			rb.AddForce(new Vector2(wallJumpForce * -moveHorizontal, jumpForce), ForceMode2D.Impulse);
			jumpCount = 0;
			canWallJump = false;

			StartCoroutine(JumpCooldown());
		}
		else if (canDoubleJump && isDoubleJumpActive)
		{
			//anim.Play(PLAYER_JUMPLAUNCH);
			//StartCoroutine(JumpAnimation(anim.GetCurrentAnimatorStateInfo(0).length));

			rb.velocity = jumpForce * Vector2.up;//* moveSpeed 
			jumpCount++;

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
			if (currentAnimState == PLAYER_JUMPFALL)
			{
				ChangeAnimationState(PLAYER_JUMPLAND);
				isJumping = true;
				Invoke("CompleteJumpAnim", 0.267f);
			}

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

	//IEnumerator JumpAnimation(float a_animLength)
	//{
	//	yield return new WaitUntil(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
	//}

	void CompleteJumpAnim()
	{
		isJumping = false;
	}

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//	if (currentAnimState == PLAYER_JUMPFALL)
	//	{
	//		Debug.Log("Landing");
	//		ChangeAnimationState(PLAYER_JUMPLAND);
	//		isJumping = true;
	//		Invoke("CompleteLaunch", 0.5f);
	//	}
	//}

	#endregion

	/// <summary>
	/// Controls dash mechanic
	/// </summary>
	/// <param name="a_direction">Direction that the player is wanted to dash</param>
	/// <returns>1 second wait between dashes</returns>
	//IEnumerator Dash(int a_direction)
	//{
	//	isDashing = true;
	//	canDash = false;
	//	rb.velocity = new Vector2(rb.velocity.x, 0);
	//	rb.AddForce(new Vector2(dashDistance * a_direction, 0), ForceMode2D.Impulse);

	//	yield return new WaitForSeconds(0.5f);
	//	isDashing = false;
	//}

	//void Dash(int a_dir)
	//{
	//	isDashing = true;
	//	canDash = false;

	//	rb.velocity = new Vector2(rb.velocity.x, 0);
	//	rb.AddForce(new Vector2(dashDistance * a_dir, 0), ForceMode2D.Impulse);

	//	isManaCooldown = true;
	//	cooldownTimer = manaCooldownTime;
	//}

	void Dash(int a_dir)
	{
		ChangeAnimationState(PLAYER_DASH);

		isDashing = true;
		canDash = false;
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(new Vector2(dashDistance * a_dir, 0), ForceMode2D.Impulse);

		Invoke("CompleteDash", 0.85f);
	}

	void CompleteDash()
	{
		isDashing = false;
	}

	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;
		isDashing = false;
		//Debug.Log("Can attack in: " + cooldownTimer);

		if (cooldownTimer <= 0)
		{
			isManaCooldown = false;
			manaCooldownUI.fillAmount = 0.0f;

			Debug.Log("Gotta Go Fast!");
		}
		else
		{
			manaCooldownUI.fillAmount = Mathf.Clamp((cooldownTimer / manaCooldownTime), 0, 1);
		}
	}

	public void ChangeAnimationState(string a_newState)
	{
		// Stops the same animation from interrupting itself
		if (currentAnimState == a_newState)
		{
			return;
		}

		// Play the animation
		anim.Play(a_newState);

		// reassign current state
		currentAnimState = a_newState;
	}
}
