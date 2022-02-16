using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's different attacks.
/// Created by: Kane Adams
/// </summary>
public class PlayerCombat : MonoBehaviour
{
	public bool hasHeavyAttack;
	public float attackRange = 0.5f;

	public float lightAttackStrength = 10;
	public float heavyAttackStrength = 25;

	public float attackRate = 2f;
	public float nextAttackTime = 0f;

	public Transform attackPoint;

	public LayerMask enemyLayers;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				LightAttack();
				nextAttackTime = Time.time + 1f / attackRate;
			}
			else if (Input.GetButtonDown("Fire2") && hasHeavyAttack)
			{
				HeavyAttack();
				nextAttackTime = Time.time + 1f / attackRate;
			}
		}

	}

	void LightAttack()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<TestEnemyScript>().TakeDamage(lightAttackStrength);
		}
	}

	void HeavyAttack()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<TestEnemyScript>().TakeDamage(heavyAttackStrength);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
		{
			return;
		}

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
