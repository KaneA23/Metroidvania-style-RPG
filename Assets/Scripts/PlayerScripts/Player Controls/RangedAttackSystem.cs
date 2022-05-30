using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the firing of Ranged attacks from the player.
/// Created by: Kane Adams
/// </summary>
public class RangedAttackSystem : MonoBehaviour
{
	public Transform firePoint;

	public GameObject fireballPrefab;

	BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	public PlayerManaSystem PMS;

	GameObject eventSystem;

	public bool isFireball;

	[SerializeField]
	Image manaCooldownUI;

	public bool isManaCooldown;
	[SerializeField] private float cooldownTimer;

	private float knockbackAmount = 500f;

	Rigidbody2D rb;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		isManaCooldown = false;
		manaCooldownUI.fillAmount = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (isManaCooldown)
		{
			ApplyCooldown();
		}
		else if (Input.GetKeyDown(KeyCode.Q) && BPC.currentMP >= BPC.fireballCost)
		{
			if (!GetComponent<PlayerHealthSystem>().isHit && !GetComponent<PlayerHealthSystem>().isDying && !GetComponent<PlayerMovementSystem>().isDashing && !DialogueManagerScript.GetInstance().IsDialoguePlaying)
			{
				isFireball = true;
				cooldownTimer = BPC.dashCooldown;

				Shoot();
				PMS.TakeMana(BPC.fireballCost);
				PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_FIREBALLLAUNCH);
				Invoke(nameof(CompleteAnim), 0.35f);
			}
		}
	}

	/// <summary>
	/// Created instance of fireball to launch at enemies
	/// </summary>
	void Shoot()
	{
		FindObjectOfType<AudioManager>().PlayAudio("PlayerFireball");
		Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
		TakeKnockback();

		isManaCooldown = true;
		cooldownTimer = BPC.rangeAtkCooldown;
		ApplyCooldown();
	}

	/// <summary>
	/// Sends player backwards (can be used as alternative, skilled movement)
	/// </summary>
	void TakeKnockback()
	{
		if (GetComponent<PlayerMovementSystem>().isFacingRight)
		{
			rb.velocity = Vector2.zero;
			//rb.AddForce(new Vector2(0, 500f), ForceMode2D.Force);
			//rb.AddForce(new Vector2(-knockbackAmount, 0), ForceMode2D.Force);
			rb.AddForce(new Vector2(-knockbackAmount * 10, knockbackAmount), ForceMode2D.Force);
		}
		else
		{
			rb.velocity = Vector2.zero;
			//rb.AddForce(new Vector2(0, 500f), ForceMode2D.Force);
			//rb.AddForce(new Vector2(knockbackAmount, 0), ForceMode2D.Force);
			rb.AddForce(new Vector2(knockbackAmount * 10, knockbackAmount), ForceMode2D.Force);
		}
	}

	/// <summary>
	/// Prevents animation looping
	/// </summary>
	void CompleteAnim()
	{
		isFireball = false;
	}

	/// <summary>
	/// Prevents player from spamming fireball button by having them wait between shots
	/// </summary>
	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;

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
}
