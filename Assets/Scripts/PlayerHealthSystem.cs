using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
	public Image healthEnd;
	public Image healthCurve;
	public Image healthStart;

	public float currentHealth;
	public float maxHealth = 100;

	public float endPercentage = 0.25f;
	public float curvePercentage = 0.5f;
	//public float startPercentage = 0.25f;

	private const float curveFillAmount = 0.75f;

	// Start is called before the first frame update
	void Start()
	{
		currentHealth = maxHealth;
		FillHealthBar();
	}

	// Update is called once per frame
	void Update()
	{
		//FillHealthBar();
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			TakeDamage(10);
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			GainHealth(10);
		}
	}

	/// <summary>
	/// Player's health decreases when attacked
	/// </summary>
	/// <param name="a_damage">Amount of health last from attack</param>
	public void TakeDamage(int a_damage)
	{
		currentHealth -= a_damage;

		if (currentHealth <= 0)
		{
			//currentHealth = 0;
			gameObject.SetActive(false);
		}
		FillHealthBar();
	}

	/// <summary>
	/// Player's health increases
	/// </summary>
	/// <param name="a_bonusHealth">Amount of extra HP being added</param>
	void GainHealth(int a_bonusHealth)
	{
		currentHealth += a_bonusHealth;
		if (currentHealth > maxHealth)
		{
			currentHealth = 100;
		}

		FillHealthBar();
	}

	/// <summary>
	/// Alters health bar to new health level
	/// </summary>
	void FillHealthBar()
	{
		float healthPercentage = currentHealth / maxHealth;

		float endFill = healthPercentage / endPercentage;
		endFill = Mathf.Clamp(endFill, 0, 1);
		healthEnd.fillAmount = endFill;

		float endAmount = endPercentage * maxHealth;

		float curveHealth = currentHealth - endAmount;
		float curveTotalHealth = maxHealth - (endAmount * 2);
		float curveFill = curveHealth / curveTotalHealth;
		curveFill = Mathf.Clamp(curveFill, 0, 1);
		healthCurve.fillAmount = curveFill;

		float curveAmount = curvePercentage * maxHealth;

		float startHealth = currentHealth - (curveAmount + endAmount);
		float startTotalHealth = maxHealth - (curveAmount + endAmount);
		float startFill = startHealth / startTotalHealth;
		startFill = Mathf.Clamp(startFill, 0, 1);
		healthStart.fillAmount = startFill;
	}
}
