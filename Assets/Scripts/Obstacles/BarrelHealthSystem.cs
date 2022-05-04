using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls enemies health and required UI.
/// Created by: Kane Adams
/// </summary>
public class BarrelHealthSystem : MonoBehaviour
{
	[Header("Health")]
	private readonly int maxHP = 25;
	public float currentHP;

	public Slider healthbar;

	[Header("Explosion")]
	private SpriteRenderer sr;
	private ParticleSystem enemyParticle;
	ParticleSystem.EmissionModule em;
	private float particleDur;

	public bool isExploding;

	private void Awake()
	{
		healthbar = GetComponentInChildren<Slider>();
		enemyParticle = GetComponentInChildren<ParticleSystem>();
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	// Start is called before the first frame update
	void Start()
	{
		if (gameObject.name != "Rambleon")
		{
			currentHP = maxHP;

			healthbar.maxValue = maxHP;
			healthbar.value = currentHP;

			healthbar.enabled = true;

			isExploding = false;
		}

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

		// If enemy runs out of health, particle system activates and enemy dies
		if (currentHP <= 0)
		{
			ActivateParticle();
		}
	}

	/// <summary>
	/// Players particle explosion and hides sprite
	/// </summary>
	public void ActivateParticle()
	{
		//Debug.Log("Begins Particles");
		em.enabled = true;
		enemyParticle.Play();
		Destroy(sr);
		isExploding = true;

		Invoke(nameof(Die), particleDur);
	}

	/// <summary>
	/// Destroy the current enemy
	/// </summary>
	void Die()
	{
		//Debug.Log("Removes NPC");
		if (gameObject.CompareTag("RambleonTut"))
		{
			FindObjectOfType<RambleonSpawnController>().rambleonCounter++;
		}
		Destroy(gameObject);
	}
}
