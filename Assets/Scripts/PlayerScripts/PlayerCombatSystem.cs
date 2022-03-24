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
	public PlayerMovementSystem PMS;

	[Header("Modular attacks")]
	public bool hasHeavyAtk;

	[Header("Attack Ranges")]
	// distance each attack can reach away from player
	public float lightRange = 0.75f;
	public float heavyRange = 1f;
	public float attackRangeY = 0.5f;

	public Transform attackPoint;       // where the player attacks from

	[Header("Attack Strengths")]
	// Amount of damage each attack type does
	public float lightStrength = 10;
	public float heavyStrength = 25;

	[Header("Attack cooldowns")]
	// Time between each attack type
	public float lightCooldown = 1f;
	public float heavyCooldown = 3f;
	//public float nextAttackTime = 0f;   // resets timer
	
	[SerializeField] private Image atkCooldownUI;

	[SerializeField] private bool isAtkCooldown;
	[SerializeField] private float cooldownTimer;
	[SerializeField] private float cooldownTime;

	[Header("Animation values")]
	public bool isAttacking;
	[SerializeField]
	private float attackAnimDelay = 0.8f;

	[Header("Enemy values")]
	public LayerMask enemyLayers;       // items the player can attack
	public float uiView = 5;
	
	[SerializeField] private double barrelDist;
	[SerializeField] private double enemyDist;

	GameObject[] barrels;
	GameObject[] enemies;

	Animator anim;

	const string PLAYER_SWORDATTACK = "Player_SwordAttack";

	private void Awake()
	{
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
        //else if (PMS.isGrounded)
        //{
        //	if (Input.GetButtonDown("Fire1"))
        //	{
        //		LightAttack();
        //		//Attack();
        //		//nextAttackTime = Time.time + 1f / lightCooldownTime;
        //	}
        //	else if (Input.GetButtonDown("Fire2") && hasHeavyAtk)
        //	{
        //		HeavyAttack();
        //		//nextAttackTime = Time.time + 1f / heavyCooldownTime;
        //	}
        //}

        if (Input.GetButtonDown("Fire1"))
        {
            LightAttack();
            //Attack();
            //nextAttackTime = Time.time + 1f / lightCooldownTime;
        }
        else if (Input.GetButtonDown("Fire2") && hasHeavyAtk)
        {
            HeavyAttack();
            //nextAttackTime = Time.time + 1f / heavyCooldownTime;
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
		//anim.Play(PLAYER_SWORDATTACK);
		PMS.ChangeAnimationState(PLAYER_SWORDATTACK);
		isAttacking = true;

		//attackAnimDelay = anim.GetCurrentAnimatorStateInfo(0).length;
		Invoke("CompleteAttack", attackAnimDelay);

		//Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, lightRange, enemyLayers);
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
		//ApplyCooldown();
	}

	/// <summary>
	/// Attacks any enemies within attack range with heavy strike
	/// </summary>
	void HeavyAttack()
	{ 
		PMS.ChangeAnimationState(PLAYER_SWORDATTACK);
		isAttacking = true;

		//attackAnimDelay = anim.GetCurrentAnimatorStateInfo(0).length;
		Invoke("CompleteAttack", attackAnimDelay);

		//Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, heavyRange, enemyLayers);
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
		//ApplyCooldown();
	}

	public void Attack()
	{
		if (isAtkCooldown)
		{
			return;
		}
		else
		{
			isAtkCooldown = true;
			cooldownTimer = lightCooldown;
		}
	}

	void CompleteAttack()
	{
		isAttacking = false;
	}

	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;
		//Debug.Log("Can attack in: " + cooldownTimer);

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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(heavyRange, attackRangeY));

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(attackPoint.position, new Vector2(lightRange, attackRangeY));
	}
}
