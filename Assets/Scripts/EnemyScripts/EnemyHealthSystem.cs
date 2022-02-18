using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls enemies health and required UI
/// Created by: Kane Adams
/// </summary>
public class EnemyHealthSystem : MonoBehaviour
{
	float particleDur;
	public int maxHP = 100;
	public float currentHP;

	public Slider healthbar;
	public SpriteRenderer sr;

	public ParticleSystem enemyParticle;
	ParticleSystem.EmissionModule em;

	private void Awake()
	{
		healthbar = GetComponentInChildren<Slider>();
		enemyParticle = GetComponentInChildren<ParticleSystem>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Start is called before the first frame update
	void Start()
	{
		currentHP = maxHP;

		healthbar.maxValue = maxHP;
		healthbar.value = currentHP;

		particleDur = enemyParticle.main.duration;
		em = enemyParticle.emission;
		em.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Enemies health decreases when attacked
	/// </summary>
	/// <param name="a_damage">Amount of health lost from player's attack</param>
	public void TakeDamage(float a_damage)
	{
		currentHP -= a_damage;
		healthbar.value = currentHP;
		Debug.Log(gameObject.name + ": " + currentHP);

		if (currentHP <= 0)
		{
			Debug.Log("<color=Purple>DEATH TO THE ENEMY!</color>");
			em.enabled = true;
			enemyParticle.Play();
			Destroy(sr);
			Invoke(nameof(Die), particleDur);
		}
	}

	/// <summary>
	/// Destroy the current enemy
	/// </summary>
	void Die()
	{
		Destroy(gameObject);
	}
}
