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
	[Header("Referenced Scripts")]
	public PlayerAnimationManager PAM;

	[Header("Health Values")]
	public float currentHP;
	public float maxHP = 100;

	[Header("Healthbar")]
	public Image healthEnd;
	public Image healthCurve;
	public Image healthStart;

	//public Image healthBase;
	//public Image healthLongNeck;
	//public Image healthMidNeck;
	//public Image healthSmallNeck;
	//public Image healthLid;

	[Space(5)]
	[SerializeField] private float endPercentage = 0.25f;
	[SerializeField] private float curvePercentage = 0.5f;

	//[SerializeField] private float basePercentage = 0.35f;
	//[SerializeField] private float longNeckPercentage = 0.35f;
	//[SerializeField] private float midNeckPercentage = 0.165f;
	//[SerializeField] private float smallNeckPercentage = 0.165f;
	//public float startPercentage = 0.25f;

	private const float curveFillAmount = 0.75f;

	[Header("Knockback")]
	public float knockForce = 2500;
	public Rigidbody2D rb;

	public bool isHit;
	public bool isDying;

	private float animDelay;

	private void Awake()
	{
		PAM = GetComponent<PlayerAnimationManager>();

		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		currentHP = maxHP;
		isHit = false;
		isDying = false;

		FillHealthBar();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			TakeDamage(10, gameObject.transform.position);
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			GainHealth(10);
		}

		if (currentHP <= 0 && !isHit)
		{
			isDying = true;
			//PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DEATH);
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DEATH);
			animDelay = 4f;
			Invoke(nameof(KillPlayer), animDelay);
		}
	}

	/// <summary>
	/// Player's health decreases when attacked
	/// </summary>
	/// <param name="a_damage">Amount of health lost from attack</param>
	public void TakeDamage(int a_damage, Vector2 a_enemyPos)
	{
		isHit = true;
		//PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HIT);
		PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HIT);
		animDelay = 0.517f;
		Invoke(nameof(CompleteDaze), animDelay);

		if ((transform.position.x - a_enemyPos.x) < 0)
		{
			Debug.Log("Left Hit");
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(-1f * knockForce, 250f));
		}
		else if ((transform.position.x - a_enemyPos.x) > 0)
		{
			Debug.Log("Right Hit");
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(1f * knockForce, 250f));
		}

		currentHP -= a_damage;

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

		//float healthPercentage = currentHP / maxHP;

		//float baseFill = healthPercentage / basePercentage;
		//baseFill = Mathf.Clamp(baseFill, 0, 1);
		//healthBase.fillAmount = baseFill;

		//float baseAmount = basePercentage * maxHP;

		//float longNeckHealth = currentHP - baseAmount;
		//float longNeckTotalHealth = maxHP - baseAmount;
		//float longNeckFill = longNeckHealth / longNeckTotalHealth;
		//longNeckFill = Mathf.Clamp(longNeckFill, 0, 1);
		//healthLongNeck.fillAmount = longNeckFill;

		//float longNeckAmount = longNeckPercentage * maxHP;

		////float lidHealth = currentHP - longNeckAmount;
		////float lidTotalHealth = maxHP - (longNeckAmount + baseAmount);
		////float lidFill = lidHealth / lidTotalHealth;
		////lidFill = Mathf.Clamp(lidFill, 0, 1);
		////healthLid.fillAmount = lidFill;

		//float midNeckHealth = currentHP - (longNeckAmount + baseAmount);
		//float midNeckTotalHealth = maxHP - (longNeckAmount + baseAmount);
		//float midNeckFill = midNeckHealth / midNeckTotalHealth;
		//midNeckFill = Mathf.Clamp(midNeckFill, 0, 1);
		//healthMidNeck.fillAmount = midNeckFill;

		//float midNeckAmount = midNeckPercentage * maxHP;

		//float smallNeckHealth = currentHP - (midNeckAmount + longNeckAmount + baseAmount);
		//float smallNeckTotalHealth = maxHP - (midNeckAmount + longNeckAmount + baseAmount);
		//float smallNeckFill = smallNeckHealth / smallNeckTotalHealth;
		//smallNeckFill = Mathf.Clamp(smallNeckFill, 0, 1);
		//healthSmallNeck.fillAmount = smallNeckFill;

		//float smallNeckAmount = smallNeckPercentage * maxHP;

		//float lidHealth = currentHP - (smallNeckAmount + midNeckAmount + longNeckAmount + baseAmount);
		//float lidTotalHealth = maxHP - (smallNeckAmount + midNeckAmount + longNeckAmount + baseAmount);
		//float lidFill = lidHealth / lidTotalHealth;
		//lidFill = Mathf.Clamp(lidFill, 0, 1);
		//healthLid.fillAmount = lidFill;
	}

	/// <summary>
	/// Destroys the player
	/// </summary>
	void KillPlayer()
	{
		//Destroy(gameObject);
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Allows player controls after hit animation ends
	/// </summary>
	void CompleteDaze()
	{
		isHit = false;
	}
}
