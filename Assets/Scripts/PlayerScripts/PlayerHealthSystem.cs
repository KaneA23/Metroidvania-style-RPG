using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's health and required UI.
/// Created by: Kane Adams
/// </summary>
public class PlayerHealthSystem : MonoBehaviour
{
	public Image healthEnd;
	public Image healthCurve;
	public Image healthStart;

	public float currentHP;
	public float maxHP = 100;

	public float endPercentage = 0.25f;
	public float curvePercentage = 0.5f;
	//public float startPercentage = 0.25f;

	private const float curveFillAmount = 0.75f;

	// Start is called before the first frame update
	void Start()
	{
		currentHP = maxHP;
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
		currentHP -= a_damage;

		if (currentHP <= 0)
		{
			KillPlayer();
		}
		FillHealthBar();
	}

	/// <summary>
	/// Player's health increases
	/// </summary>
	/// <param name="a_bonusHealth">Amount of extra HP being added</param>
	void GainHealth(int a_bonusHealth)
	{
		currentHP += a_bonusHealth;
		if (currentHP > maxHP)
		{
			currentHP = 100;
		}

		FillHealthBar();
	}

	/// <summary>
	/// Alters health bar to new health level
	/// </summary>
	void FillHealthBar()
	{
		float healthPercentage = currentHP / maxHP;

		float endFill = healthPercentage / endPercentage;
		endFill = Mathf.Clamp(endFill, 0, 1);
		healthEnd.fillAmount = endFill;

		float endAmount = endPercentage * maxHP;

		float curveHealth = currentHP - endAmount;
		float curveTotalHealth = maxHP - (endAmount * 2);
		float curveFill = curveHealth / curveTotalHealth;
		curveFill = Mathf.Clamp(curveFill, 0, 1);
		healthCurve.fillAmount = curveFill;

		float curveAmount = curvePercentage * maxHP;

		float startHealth = currentHP - (curveAmount + endAmount);
		float startTotalHealth = maxHP - (curveAmount + endAmount);
		float startFill = startHealth / startTotalHealth;
		startFill = Mathf.Clamp(startFill, 0, 1);
		healthStart.fillAmount = startFill;
	}

	void KillPlayer()
	{
		Debug.Log("<color=Red>I have fallen my Lord!</color>");
		gameObject.SetActive(false);
	}
}
