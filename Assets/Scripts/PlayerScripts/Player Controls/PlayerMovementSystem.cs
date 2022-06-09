using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls character's movement dependent on player's input.
/// Created by: Kane Adams
/// </summary>
public class PlayerMovementSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	public PlayerManaSystem PMS;
	public PlayerStaminaSystem PSS;
	BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	PlayerCombatSystem PCS;
	PlayerHealthSystem PHS;
	RangedAttackSystem RAS;

	private DialogueManager DM;

	GameObject eventSystem;
	GameObject Bernard;

	[Header("Movement")]
	public bool isFacingRight;
	public float moveHorizontal;
	public float moveSpeed;
	public bool isRunning;

	[Tooltip("Acceleration decreases the closer to 1")]
	[Range(0f, 1f)]
	public float horizontalDamping = 0.75f;

	[Header("Crouch")]
	public bool isCeiling;
	public bool isCrouching;
	public bool isCrouchEnter;
	public bool isCrouchExit;

	[Header("Jumping")]
	public bool isGrounded;
	public bool isJumping;
	private float moveAnimDelay;
	public bool canJump;

	public float groundedTimer = 0f;
	public float groundedTime = 0.2f;

	[Tooltip("Gap between highest and lowest jump decreases as value increases")]
	[Range(0f, 1f)]
	public float cutJumpHeight;

	[Space(10)]
	public bool canDoubleJump;
	public float jumpCount;

	[Space(10)]
	public bool isTouchingWall;
	public bool canWallJump;

	[Header("Dash")]
	public bool canDash;
	public bool isDashing;

	[SerializeField]
	Image manaCooldownUI;

	[SerializeField] private bool isManaCooldown;
	[SerializeField] private float cooldownTimer;

	[Header("Checks")]
	public float checkRadius = 0.1f;

	public float ceilingCheckWidth = 0.75f;
	public float ceilingCheckHeight = 0.6f;

	[Space(5)]
	public Transform ceilingCheck;
	[Tooltip("Objects player needs to crouch under")]
	public LayerMask ceilingLayers;

	[Space(5)]
	public Transform groundCheck;
	[Tooltip("Objects that can be jumped off")]
	public LayerMask groundLayers;

	public LayerMask enemyLayers;

	[Space(5)]
	public Transform wallCheck;
	[Tooltip("Objects that can be wall jumped")]
	public LayerMask wallLayers;

	BoxCollider2D headCollider;
	CapsuleCollider2D bodyCollider;
	Rigidbody2D rb;

	public ParticleSystem dust;

	public BoxCollider2D bossBodyCollider;
	public PolygonCollider2D bossTailCollider;

	public Collider2D bossColliderTop;
	public Collider2D bossColliderBottom;

	private bool exited = true;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
		PCS = GetComponent<PlayerCombatSystem>();
		PHS = GetComponent<PlayerHealthSystem>();
		RAS = GetComponent<RangedAttackSystem>();

		DM = FindObjectOfType<DialogueManager>();

		headCollider = GetComponent<BoxCollider2D>();
		bodyCollider = GetComponent<CapsuleCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		headCollider.enabled = true;
		isCrouching = false;
		jumpCount = 0;

		isManaCooldown = false;
		manaCooldownUI.fillAmount = 0.0f;

		isFacingRight = true;
		transform.Rotate(new Vector2(0, 180));

		// Dash ignoring collisions
		Physics2D.IgnoreLayerCollision(12, 3);  //enemies
		Physics2D.IgnoreLayerCollision(12, 14); //cannonballs
		Physics2D.IgnoreLayerCollision(12, 7);  //projectiles
		Physics2D.IgnoreLayerCollision(12, 13); //traps
		Physics2D.IgnoreLayerCollision(12, 18);
		Physics2D.IgnoreLayerCollision(6, 18);
	}

	// Update is called once per frame
	void Update()
	{
		if (DialogueManagerScript.GetInstance().IsDialoguePlaying)
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		//groundedTimer -= Time.deltaTime;
		//if (isGrounded)
		//{
		//	groundedTimer = groundedTime;
		//}
		//Debug.Log("Time grounded: " + groundedTimer);

		if (PHS.isHit)
		{
			isRunning = false;
			isJumping = false;
			if (!isCeiling)
			{
				isCrouching = false;
			}
		}

		if (/*DM.isTalking*/DialogueManagerScript.GetInstance().IsDialoguePlaying)
		{
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_IDLE);
		}
		if (!PHS.isDying /*&& !DM.isTalking*/)
		{
			if (!isGrounded && !isJumping && !PCS.isAttacking && !PHS.isHit && !isDashing && !DialogueManagerScript.GetInstance().IsDialoguePlaying && !RAS.isFireball)
			{
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_JUMPFALL);
			}

			CheckIfGrounded();
			CheckIfCeiling();
			CheckIfWall();

			if (!PHS.isHit /*&& !FindObjectOfType<BernardIntroCutscene>().isCutscene*/)
			{
				PlayerInput();
			}
		}
	}

	private void FixedUpdate()
	{
		groundedTimer -= Time.fixedDeltaTime;
		if (isGrounded)
		{
			groundedTimer = groundedTime;
			canJump = true;
		}

		if (groundedTimer <= 0 || isJumping)
		{
			canJump = false;
		}

		// Resets double jump if the player is touching the ground
		if (isGrounded)
		{
			canDoubleJump = true;
		}

		if (!isDashing && !PHS.isHit && !PHS.isDying /*&& !FindObjectOfType<BernardIntroCutscene>().isCutscene*/)
		{
			PlayerMovement();
		}
	}

	/// <summary>
	/// Prevents player getting stuck under boss after dashing
	/// </summary>
	/// <param name="collision">Checks if boss collider</param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == 6)
		{
			if (bossColliderTop.bounds.Intersects(headCollider.bounds) ||
				bossColliderTop.bounds.Intersects(bodyCollider.bounds) ||
				bossColliderBottom.bounds.Intersects(headCollider.bounds) ||
				bossColliderBottom.bounds.Intersects(bodyCollider.bounds))
			{
				gameObject.layer = 12;
			}
		}
	}

	/// <summary>
	/// Prevents player getting stuck under boss after dashing
	/// </summary>
	/// <param name="collision">Checks if boss collider</param>
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == 6)
		{
			if (!bossColliderTop.bounds.Intersects(headCollider.bounds) ||
				!bossColliderTop.bounds.Intersects(bodyCollider.bounds) ||
				!bossColliderBottom.bounds.Intersects(headCollider.bounds) ||
				!bossColliderBottom.bounds.Intersects(bodyCollider.bounds))
			{
				exited = true;
				gameObject.layer = 6;
			}
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
		if (PAM.currentAnimState != "Player_DashEnter" && PAM.currentAnimState != "Player_DashExit" && !DialogueManagerScript.GetInstance().IsDialoguePlaying && BPC.hasWalk)
		{
			if (PAM.currentAnimState != "Player_SwordAttack" && PAM.currentAnimState != "Player_HeavyAttack")
			{
				if ((moveHorizontal > 0 && !isFacingRight) || (moveHorizontal < 0 && isFacingRight))
				{
					transform.Rotate(new Vector2(0, 180f));
					isFacingRight = !isFacingRight;
				}
			}
		}

		if (!isDashing)
		{
			if (!DialogueManagerScript.GetInstance().IsDialoguePlaying)
			{
				if (Input.GetButtonDown("Jump") && jumpCount < 1 && !isCrouching && BPC.currentStam >= BPC.jumpCost && BPC.hasJump)
				{
					if (!BPC.hasDoubleJump && !isGrounded)
					{
						return;
					}
					FindObjectOfType<AudioManager>().PlayAudio("PlayerJump");
					PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_JUMPLAUNCH);
					isJumping = true;
					moveAnimDelay = 0.5f;
					Invoke(nameof(CompleteJumpAnim), moveAnimDelay);
				}

				if (Input.GetButtonDown("Jump") /*&& !isCrouching*/ && BPC.hasJump)
				{
					//Debug.Log("Jump Pressed");
					jumpCount++;
					Jump();
				}

				// Cuts off jump height when player releases jump button
				if (Input.GetButtonUp("Jump") && BPC.hasJump)
				{
					if (rb.velocity.y > 0)
					{
						rb.velocity = BPC.jumpForce * cutJumpHeight * Vector2.up;
					}
				}

				// Removes head collider if player wants to crouch
				if (Input.GetKeyDown(KeyCode.LeftControl) /*&& isGrounded */&& BPC.hasCrouch)
				{
					isCrouching = true;
					headCollider.enabled = false;

					if (isGrounded)
					{
						isCrouchEnter = true;

						PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_CROUCHENTER);
						moveAnimDelay = 0.35f;

						Invoke(nameof(CompleteCrouchAnim), moveAnimDelay);
					}
				}
				else if (!isCeiling && isCrouching)
				{
					if (!Input.GetKey(KeyCode.LeftControl) && BPC.hasCrouch)
					{
						if (!isCrouchEnter)
						{
							isCrouchExit = true;
							PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_CROUCHEXIT);
							moveAnimDelay = 0.35f;
							Invoke(nameof(CompleteCrouchAnim), moveAnimDelay);
						}
					}
					//isCrouching = false;
				}

				if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && moveHorizontal != 0 && isGrounded && BPC.currentStam > (BPC.runCost * 0.5f) && BPC.hasRun)
				{
					if (!isRunning)
					{
						CreateDustParticles();
					}
					isRunning = true;
					PSS.TakeStamina(BPC.runCost * Time.deltaTime);
				}
				else
				{
					isRunning = false;
				}

				if (BPC.currentStam < (BPC.runCost * 0.5f))
				{
					isRunning = false;
				}

				// Changes player speed depending on whether you are running, walking or crouching
				if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && BPC.currentStam >= BPC.runCost && BPC.hasRun)
				{
					moveSpeed = BPC.runSpeed;
				}
				else if (isCrouching)
				{
					moveSpeed = BPC.crouchSpeed;
				}
				else
				{
					moveSpeed = BPC.walkSpeed;
				}
			}

			if (isManaCooldown)
			{
				ApplyCooldown();
			}
			else if (!DialogueManagerScript.GetInstance().IsDialoguePlaying && !RAS.isManaCooldown)
			{
				// Dashing
				if (Input.GetButtonDown("Dash") && !isCrouching && !isDashing && canDash && BPC.hasDash && BPC.currentMP >= BPC.dashCost)
				{
					PMS.TakeMana(BPC.dashCost);

					PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DASHENTER);
					gameObject.layer = 12;
					FindObjectOfType<AudioManager>().PlayAudio("PlayerDash");

					isDashing = true;
					moveAnimDelay = 0.35f;
					Invoke(nameof(Dash), moveAnimDelay);
				}
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
		if (!PCS.isAttacking /*&& !isCrouchEnter && !isCrouchExit*/ /*&& !DM.isTalking*/&& !DialogueManagerScript.GetInstance().IsDialoguePlaying && BPC.hasWalk)
		{
			if (isGrounded || !isTouchingWall)
			{
				rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
			}
		}

		if (isCrouching && !isCrouchEnter && !isCrouchExit && !DialogueManagerScript.GetInstance().IsDialoguePlaying && isGrounded)
		{
			if (moveHorizontal != 0 && BPC.hasCrouch)
			{
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_CROUCHWALK);
				FindObjectOfType<AudioManager>().PlayAudio("PlayerCrouch");
			}
			else
			{
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_CROUCHIDLE);
			}
		}
		else if (isGrounded && !PCS.isAttacking && !isJumping && !PHS.isDying && !isCrouching && !RAS.isFireball)
		{
			if (moveHorizontal != 0 && !DialogueManagerScript.GetInstance().IsDialoguePlaying && BPC.hasWalk)
			{
				if (Input.GetKey(KeyCode.LeftShift) && BPC.hasRun)
				{
					PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_RUN);
					FindObjectOfType<AudioManager>().PlayAudio("PlayerRun");
				}
				else
				{
					PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_WALK);
				}

				if (Input.GetKeyUp(KeyCode.LeftShift))
				{
					FindObjectOfType<AudioManager>().StopAudio("PlayerRun");
				}
			}
			else
			{
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_IDLE);
			}
		}
	}

	/// <summary>
	/// Checks whether a crouched player is under a platform to see if they can stand up
	/// </summary>
	private void CheckIfCeiling()
	{
		isCeiling = Physics2D.OverlapBox(ceilingCheck.position, new Vector2(ceilingCheckWidth, ceilingCheckHeight), 0, ceilingLayers);
	}

	#region Jump Mechanics

	/// <summary>
	/// Checks player can jump and how many times
	/// </summary>
	void Jump()
	{
		//Debug.Log("canDoubleJump: " + canDoubleJump);
		//Debug.Log("hasDoubleJump: " + BPC.hasDoubleJump);
		//Debug.Log("current Stam: " + BPC.currentStam);
		//Debug.Log("jump count: " + jumpCount);

		if (BPC.hasWallJump && isTouchingWall && canWallJump && BPC.currentStam >= BPC.jumpCost)
		{
			//Debug.Log("Wall Jump");

			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
			rb.AddForce(new Vector2(BPC.wallJumpForce * -moveHorizontal, BPC.jumpForce), ForceMode2D.Impulse);
			jumpCount = 0;
			canWallJump = false;
		}

		if (isGrounded && canJump && BPC.currentStam >= BPC.jumpCost)
		{
			CreateDustParticles();

			//PSS.TakeStamina(BPC.jumpCost);
			canJump = false;

			rb.velocity = BPC.jumpForce * Vector2.up;
		}
		else if (/*isGrounded*/groundedTimer > 0 && canJump)
		{
			canJump = false;

			CreateDustParticles();

			rb.velocity = BPC.jumpForce * Vector2.up;
		}
		else if (canDoubleJump && BPC.hasDoubleJump)
		{
			CreateDustParticles();

			//if (BPC.hasDash && BPC.currentMP >= BPC.jumpCost /*&& jumpCount == 2*/)
			//{
			//	//PSS.TakeStamina(BPC.jumpCost);
			//	PMS.TakeMana(BPC.jumpCost);
			//}

			canDoubleJump = false;
			rb.velocity = new Vector2(0f, 0f);
			rb.velocity = BPC.jumpForce * Vector2.up;
		}
	}

	/// <summary>
	/// Checks whether the player is touching platform to allow jump
	/// </summary>
	private void CheckIfGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayers);

		Collider2D enemy = Physics2D.OverlapCircle(groundCheck.position, checkRadius, enemyLayers);

		if (enemy != null && (enemy.CompareTag("BossBack") || enemy.CompareTag("BernardLimb")))
		{
			PHS.isEnemyBack = true;
			isGrounded = true;
		}
		else
		{
			PHS.isEnemyBack = false;
		}

		if (isGrounded)
		{
			if (PAM.currentAnimState == "Player_Fall")
			{
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_JUMPLAND);
				isJumping = true;
				moveAnimDelay = 0.27f;
				Invoke(nameof(CompleteJumpAnim), moveAnimDelay);
			}

			canJump = true;
			canWallJump = true;
			canDash = true;
			jumpCount = 0;
			rb.gravityScale = 3f;
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

		if (jumpCount < 2)
		{
			canDoubleJump = true;
		}
		else
		{
			canDoubleJump = false;
		}
	}

	/// <summary>
	/// Allows other animations to play
	/// </summary>
	void CompleteJumpAnim()
	{
		isJumping = false;
	}

	#endregion

	#region Dash Mechanics

	/// <summary>
	/// Controls dash mechanic
	/// </summary>
	void Dash()
	{
		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DASH);

		int dir;

		if (isFacingRight)
		{
			dir = 1;
		}
		else
		{
			dir = -1;
		}

		canDash = false;
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(new Vector2(BPC.dashDist * dir, 0), ForceMode2D.Impulse);

		moveAnimDelay = 0.5f;
		Invoke(nameof(CompleteDash), moveAnimDelay);
	}

	/// <summary>
	/// Allows dash animation to play
	/// </summary>
	void CompleteDash()
	{
		if (PAM.currentAnimState == "Player_Dash")
		{
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DASHEXIT);
			moveAnimDelay = 0.35f;
			Invoke(nameof(CompleteDash), moveAnimDelay);
		}
		else
		{
			isDashing = false;
			isManaCooldown = true;
			cooldownTimer = BPC.dashCooldown;

			if (exited)
			{
				gameObject.layer = 6;
			}
		}
	}

	/// <summary>
	/// Prevents dash being spamclicked by having player wait between uses
	/// </summary>
	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;
		isDashing = false;

		if (cooldownTimer <= 0)
		{
			isManaCooldown = false;
			manaCooldownUI.fillAmount = 0.0f;
		}
		else
		{
			manaCooldownUI.fillAmount = Mathf.Clamp(cooldownTimer / BPC.dashCooldown, 0, 1);
		}
	}

	#endregion

	/// <summary>
	/// Allows player to stand
	/// </summary>
	void CompleteCrouchAnim()
	{
		if (PAM.currentAnimState == "Player_CrouchEnter")
		{
			//headCollider.enabled = false;
			isCrouchEnter = false;
		}
		//else if (PAM.currentAnimState == "Player_CrouchIdle" || PAM.currentAnimState == "Player_CrouchWalk")
		//{
		//	PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_CROUCHEXIT);
		//	moveAnimDelay = 0.517f;
		//	Invoke(nameof(CompleteCrouchAnim), moveAnimDelay);
		//}
		else if (PAM.currentAnimState == "Player_CrouchExit")
		{
			headCollider.enabled = true;
			isCrouching = false;
			isCrouchExit = false;
		}
	}

	/// <summary>
	/// Shows the Gizmo checks in editor
	/// </summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(ceilingCheck.position, new Vector2(ceilingCheckWidth, ceilingCheckHeight));
		Gizmos.DrawWireSphere(ceilingCheck.position, checkRadius * 2.5f);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
	}

	/// <summary>
	/// Plays dust particle effect when player runs
	/// </summary>
	void CreateDustParticles()
	{
		dust.Play();
	}
}
