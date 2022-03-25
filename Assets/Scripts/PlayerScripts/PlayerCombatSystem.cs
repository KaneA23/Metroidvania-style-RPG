using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's different attacks.
/// Created by: Kane Adams
/// </summary>
public class PlayerCombatSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	public BasePlayerClass BPC;
	public PlayerHealthSystem PHS;
	public PlayerAnimationManager PAM;
	public PlayerMovementSystem PMS;

	[Header("Modular attacks")]
	public bool hasHeavyAtk;

	[Header("Attack Ranges")]
	// distance each attack can reach away from player
	public float lightRange = 0.75f;
	public float heavyRange = 1f;
	public float attackRangeY = 0.5f;

	public Transform attackPoint;   // where the player attacks from

	[Header("Attack Strengths")]
	// Amount of damage each attack type does
	public float lightStrength = 25;
	public float heavyStrength = 50;

	[Header("Attack cooldowns")]
	// Time between each attack type
	public float lightCooldown = 0.75f;
	public float heavyCooldown = 1.5f;

	[SerializeField] private Image atkCooldownUI;

	[SerializeField] private bool isAtkCooldown;
	[SerializeField] private float cooldownTimer;
	[SerializeField] private float cooldownTime;

	[Header("Animation values")]
	public bool isAttacking;
	[SerializeField]
	private float attackAnimDelay;

	[Header("Enemy values")]
	public LayerMask enemyLayers;   // items the player can attack
	public float uiView = 5;

	[SerializeField] private double barrelDist;
	[SerializeField] private double enemyDist;

	GameObject[] barrels;
	GameObject[] enemies;

	Animator anim;

	const string PLAYER_SWORDATTACK = "Player_SwordAttack";
	const string PLAYER_HEAVYATTACK = "Player_HeavyAttack";

	private void Awake()
	{
		PHS = GetComponent<PlayerHealthSystem>();
		PAM = GetComponent<PlayerAnimationManager>();
		PMS = GetComponent<PlayerMovementSystem>();

		anim = GetComponentInChildren<Animator>();
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
		if (isAtkCooldown)
		{
			ApplyCooldown();
		}
		else if (!PHS.isHit && !PMS.isDashing)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				LightAttack();
			}
			else if (Input.GetButtonDown("Fire2") && hasHeavyAtk)
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

			if (barrelDist < uiView && barrel.GetComponent<BarrelHealthSystem>().currentHP > 0)
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

			if (enemyDist < uiView && enemy.GetComponent<EnemyHealth>().m_CurrentHP > 0)
			{
				enemyUI.gameObject.SetActive(true);
			}
			else
			{
				enemyUI.gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Attacks any enemies within attack range with light strike
	/// </summary>
	void LightAttack()
	{
		//PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_SWORDATTACK);
		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_SWORDATTACK);
		isAttacking = true;
		attackAnimDelay = 0.35f;
		Invoke(nameof(CompleteAttack), attackAnimDelay);

		Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(lightRange, attackRangeY), 0, enemyLayers);
		foreach (Collider2D enemy in hitEnemies)
		{
			if (enemy.CompareTag("Barrel"))
			{
				enemy.GetComponent<BarrelHealthSystem>().TakeDamage(lightStrength);
			}
			else
			{
				enemy.GetComponent<EnemyHealth>().TakeDamage(lightStrength, gameObject.transform.position);
			}
		}

		isAtkCooldown = true;
		cooldownTimer = lightCooldown;
		cooldownTime = lightCooldown;
	}

	/// <summary>
	/// Attacks any enemies within attack range with heavy strike
	/// </summary>
	void HeavyAttack()
	{
		//PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HEAVYATTACK);
		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HEAVYATTACK);
		isAttacking = true;
		attackAnimDelay = 0.517f;
		Invoke(nameof(CompleteAttack), attackAnimDelay);

		Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(heavyRange, attackRangeY), 0, enemyLayers);
		foreach (Collider2D enemy in hitEnemies)
		{
			if (enemy.CompareTag("Barrel"))
			{
				enemy.GetComponent<BarrelHealthSystem>().TakeDamage(heavyStrength);
			}
			else
			{
				enemy.GetComponent<EnemyHealth>().TakeDamage(heavyStrength, gameObject.transform.position);
			}
		}

		isAtkCooldown = true;
		cooldownTimer = heavyCooldown;
		cooldownTime = heavyCooldown;
	}

	/// <summary>
	/// 
	/// </summary>
	void CompleteAttack()
	{
		isAttacking = false;
	}

	/// <summary>
	/// 
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
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(heavyRange, attackRangeY));

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(lightRange, attackRangeY));
	}
}
