using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's different attacks.
/// Created by: Kane Adams
/// </summary>
public class PlayerCombatSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	BasePlayerClass BPC;
	PlayerHealthSystem PHS;
	PlayerAnimationManager PAM;
	PlayerMovementSystem PMS;
	RangedAttackSystem RAS;
	public PlayerStaminaSystem PSS;

	private DialogueManager DM;

	GameObject eventSystem;

	[Header("Attack Ranges")]
	// distance each attack can reach away from player
	public float attackRangeY = 0.5f;//Query whether goes in stats?
	public Transform attackPoint;   // where the player attacks from

	[Header("Attack Cooldowns")]
	[SerializeField] private Image atkCooldownUI;

	[SerializeField] private bool isAtkCooldown;
	[SerializeField] private float cooldownTimer;
	[SerializeField] private float cooldownTime;

	[Header("Animation values")]
	public bool isAttacking;

	[Header("Enemy values")]
	public LayerMask enemyLayers;   // items the player can attack

	[SerializeField] private double barrelDist;
	[SerializeField] private double enemyDist;

	GameObject[] barrels;
	GameObject[] enemies;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
		PHS = GetComponent<PlayerHealthSystem>();
		PMS = GetComponent<PlayerMovementSystem>();
		RAS = GetComponent<RangedAttackSystem>();

		DM = FindObjectOfType<DialogueManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		isAtkCooldown = false;
		atkCooldownUI.fillAmount = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (!PHS.isDying /*!DM.isTalking*/)
		{
			if (isAtkCooldown)
			{
				ApplyCooldown();
			}
			else if (!PHS.isHit && !PMS.isDashing && !PMS.isCrouching && !RAS.isFireball && !DialogueManagerScript.GetInstance().IsDialoguePlaying /*&& !FindObjectOfType<BernardIntroCutscene>().isCutscene*/)
			{
				if (Input.GetButtonDown("Fire1") && BPC.currentStam > BPC.lightAtkCost && BPC.hasLightAtk)
				{
					LightAttack();
				}
				else if (Input.GetButtonDown("Fire2") && BPC.currentStam > BPC.heavyAtkCost && BPC.hasHeavyAtk)
				{
					HeavyAttack();
				}
			}

			barrels = GameObject.FindGameObjectsWithTag("Barrel");
			enemies = GameObject.FindGameObjectsWithTag("Enemy");

			foreach (GameObject barrel in barrels)
			{
				barrelDist = Vector2.Distance(transform.position, barrel.transform.position);

				Transform barrelUI = barrel.transform.Find("Canvas");

				if (barrelDist < BPC.uiViewDist && barrel.GetComponent<BarrelHealthSystem>().currentHP > 0)
				{
					barrelUI.gameObject.SetActive(true);
				}
				else
				{
					barrelUI.gameObject.SetActive(false);
				}
			}

			foreach (GameObject enemy in enemies)
			{
				enemyDist = Vector2.Distance(transform.position, enemy.transform.position);

				Transform enemyUI = enemy.transform.Find("Canvas");

				if (enemyDist < BPC.uiViewDist && enemy.GetComponent<EnemyHealth>().m_CurrentHP > 0)
				{
					enemyUI.gameObject.SetActive(true);
				}
				else
				{
					enemyUI.gameObject.SetActive(false);
				}
			}
		}
	}

	/// <summary>
	/// Attacks any enemies within attack range with light strike
	/// </summary>
	void LightAttack()
	{
		isAtkCooldown = true;
		cooldownTimer = BPC.lightAtkCooldown;
		cooldownTime = BPC.lightAtkCooldown;

		PSS.TakeStamina(BPC.lightAtkCost);

		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_SWORDATTACK);
		FindObjectOfType<AudioManager>().PlayAudio("PlayerSwing");
		isAttacking = true;
		Invoke(nameof(CompleteAttack), BPC.lightAtkSpeed);

		Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(BPC.lightAtkRange, attackRangeY), 0, enemyLayers);
		foreach (Collider2D enemy in hitEnemies)
		{
			if (enemy.CompareTag("Barrel"))
			{
				enemy.GetComponent<BarrelHealthSystem>().TakeDamage(BPC.lightAtkDamage);
			}
			else
			{
				Debug.Log(enemy.name);

				if (enemy.gameObject.CompareTag("BernardLimb"))
				{
					enemy.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(BPC.lightAtkDamage, gameObject.transform.position/*, BPC.lightKnockbackDist*/);
				}
				else
				{
					enemy.GetComponent<EnemyHealth>().TakeDamage(BPC.lightAtkDamage, gameObject.transform.position/*, BPC.lightKnockbackDist*/);
				}

			}
		}
	}

	/// <summary>
	/// Attacks any enemies within attack range with heavy strike
	/// </summary>
	void HeavyAttack()
	{
		isAtkCooldown = true;
		cooldownTimer = BPC.heavyAtkCooldown;
		cooldownTime = BPC.heavyAtkCooldown;

		PSS.TakeStamina(BPC.heavyAtkCost);

		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HEAVYATTACK);
		FindObjectOfType<AudioManager>().PlayAudio("PlayerSwing");
		isAttacking = true;
		Invoke(nameof(CompleteAttack), BPC.heavyAtkSpeed);

		Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(BPC.heavyAtkRange, attackRangeY), 0, enemyLayers);
		foreach (Collider2D enemy in hitEnemies)
		{
			if (enemy.CompareTag("Barrel"))
			{
				enemy.GetComponent<BarrelHealthSystem>().TakeDamage(BPC.heavyAtkDamage);
			}
			else
			{
				if (enemy.CompareTag("BernardLimb"))
				{
					enemy.GetComponentInParent<EnemyHealth>().TakeDamage(BPC.heavyAtkDamage, gameObject.transform.position/*, BPC.heavyKnockbackDist*/);
				}
				else
				{
					enemy.GetComponent<EnemyHealth>().TakeDamage(BPC.heavyAtkDamage, gameObject.transform.position/*, BPC.heavyKnockbackDist*/);
				}
			}
		}
	}

	/// <summary>
	/// Allows other animations to play
	/// </summary>
	void CompleteAttack()
	{
		isAttacking = false;
	}

	/// <summary>
	/// Adds timer that prevents player from attacking again until ended
	/// </summary>
	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;

		if (cooldownTimer <= 0)
		{
			isAtkCooldown = false;
			atkCooldownUI.fillAmount = 0.0f;
		}
		else
		{
			atkCooldownUI.fillAmount = Mathf.Clamp((cooldownTimer / cooldownTime), 0, 1);
		}
	}

	/// <summary>
	/// Allows to see attack ranges in editor
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		// Need to manually add Gizmo ranges to work, can't reference other script
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(2, attackRangeY));

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(1.5f, attackRangeY));
	}
}
