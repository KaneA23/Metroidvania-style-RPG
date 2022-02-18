using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's different attacks.
/// Created by: Kane Adams
/// </summary>
public class PlayerCombat : MonoBehaviour
{
	public bool hasHeavyAttack;

	// distance each attack can reach away from player
	public float lightRange = 0.5f;
	public float heavyRange = 1f;

	// Amount of damage each attack type does
	public float lightStrength = 10;
	public float heavyStrength = 25;

	// Time between each attack type
	public float lightRate = 2f;
	public float heavyRate = 1f;
	public float nextAttackTime = 0f;   // resets timer

	public Transform attackPoint;   // where the player attacks from

	public LayerMask enemyLayers;   // items the player can attack


	public float uiView = 5;
	double enemyDist;

	GameObject[] enemies;

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
				nextAttackTime = Time.time + 1f / lightRate;
			}
			else if (Input.GetButtonDown("Fire2") && hasHeavyAttack)
			{
				HeavyAttack();
				nextAttackTime = Time.time + 1f / heavyRate;
			}
		}

		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			enemyDist = Vector2.Distance(transform.position, enemy.transform.position);
			//Debug.Log(enemy.name + ": " + enemyDist);

			Transform enemyUI = enemy.transform.Find("Canvas");

			if (enemyDist < uiView && enemy.GetComponent<EnemyHealthSystem>().currentHP > 0)
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
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, lightRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<EnemyHealthSystem>().TakeDamage(lightStrength);
		}
	}

	/// <summary>
	/// Attacks any enemies within attack range with heavy strike
	/// </summary>
	void HeavyAttack()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, heavyRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<EnemyHealthSystem>().TakeDamage(heavyStrength);
		}
	}
}
