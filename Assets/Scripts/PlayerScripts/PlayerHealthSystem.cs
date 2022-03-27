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
	BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	PlayerMovementSystem PMS;

	GameObject eventSystem;

	//[Header("Healthbar")]
	//public Image healthEnd;
	//public Image healthCurve;
	//public Image healthStart;

	//public Image healthBase;
	//public Image healthLongNeck;
	//public Image healthMidNeck;
	//public Image healthSmallNeck;
	//public Image healthLid;

	public Image healthBase;
	public Image[] healthNecks;
	public Image healthNeck;
	public Image healthLid;

	GameObject[] neckObjects;

	//[Space(5)]
	//[SerializeField] private float endPercentage = 0.25f;
	//[SerializeField] private float curvePercentage = 0.5f;

	//[SerializeField] private float basePercentage = 0.35f;
	//[SerializeField] private float longNeckPercentage = 0.35f;
	//[SerializeField] private float midNeckPercentage = 0.165f;
	//[SerializeField] private float smallNeckPercentage = 0.165f;
	//public float startPercentage = 0.25f;



	[Space(5)]
	[SerializeField] private float basePercentage = 0.5f;
	[SerializeField] private float neckPercentage;
	[SerializeField] private float lidPercentage = 0.25f;

	[SerializeField] private float healthPercentage;



	[Header("Knockback")]
	public Rigidbody2D rb;

	[Header("Animations")]
	public bool isHit;
	public bool isDying;

	private float animDelay;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
		PMS = GetComponent<PlayerMovementSystem>();

		rb = GetComponent<Rigidbody2D>();

		neckObjects = GameObject.FindGameObjectsWithTag("HealthbarNeck");

		healthNecks = new Image[neckObjects.Length];

		for (int i = 0; i < healthNecks.Length; i++)
		{
			healthNecks[i] = neckObjects[i].GetComponent<Image>();
			Debug.Log(healthNecks.Length);
			Debug.Log(healthNecks[i]);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		neckPercentage = 1 - (basePercentage + lidPercentage);
		neckPercentage /= healthNecks.Length;
		Debug.Log(neckPercentage);

		BPC.currentHP = BPC.currentMaxHP;
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

		if (BPC.currentHP <= 0 && !isHit)
		{
			isDying = true;
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
		if (!PMS.isDashing)
		{
			isHit = true;
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HIT);
			animDelay = 0.517f;
			Invoke(nameof(CompleteDaze), animDelay);

			if ((transform.position.x - a_enemyPos.x) < 0)
			{
				Debug.Log("Left Hit");
				rb.velocity = Vector2.zero;
				rb.AddForce(new Vector2(-1f * BPC.knockbackTaken, 250f));
			}
			else if ((transform.position.x - a_enemyPos.x) > 0)
			{
				Debug.Log("Right Hit");
				rb.velocity = Vector2.zero;
				rb.AddForce(new Vector2(1f * BPC.knockbackTaken, 250f));
			}

			BPC.currentHP -= a_damage;

			FillHealthBar();
		}
	}

	/// <summary>
	/// Player's health increases
	/// </summary>
	/// <param name="a_bonusHealth">Amount of extra HP being added</param>
	void GainHealth(int a_bonusHealth)
	{
		BPC.currentHP += a_bonusHealth;
		if (BPC.currentHP > BPC.currentMaxHP)
		{
			BPC.currentHP = 100;
		}

		FillHealthBar();
	}

	/// <summary>
	/// Alters health bar to new health level
	/// </summary>
	void FillHealthBar()
	{
		//float healthPercentage = BPC.currentHP / BPC.currentMaxHP;

		//float endFill = healthPercentage / endPercentage;
		//endFill = Mathf.Clamp(endFill, 0, 1);
		//healthEnd.fillAmount = endFill;

		//float endAmount = endPercentage * BPC.currentMaxHP;

		//float curveHealth = BPC.currentHP - endAmount;
		//float curveTotalHealth = BPC.currentMaxHP - (endAmount * 2);
		//float curveFill = curveHealth / curveTotalHealth;
		//curveFill = Mathf.Clamp(curveFill, 0, 1);
		//healthCurve.fillAmount = curveFill;

		//float curveAmount = curvePercentage * BPC.currentMaxHP;

		//float startHealth = BPC.currentHP - (curveAmount + endAmount);
		//float startTotalHealth = BPC.currentMaxHP - (curveAmount + endAmount);
		//float startFill = startHealth / startTotalHealth;
		//startFill = Mathf.Clamp(startFill, 0, 1);
		//healthStart.fillAmount = startFill;

		float baseAmount = basePercentage * BPC.currentMaxHP;
		float neckAmount = (neckPercentage / healthNecks.Length) * BPC.currentMaxHP;
		float lidAmount = lidPercentage * BPC.currentMaxHP;

		float healthPercentage = BPC.currentHP / BPC.currentMaxHP;

		float baseFill = healthPercentage / basePercentage;
		baseFill = Mathf.Clamp(baseFill, 0, 1);
		healthBase.fillAmount = baseFill;

		// Static neck health
		float neckHealth = BPC.currentHP - baseAmount;
		float neckTotalHealth = BPC.currentMaxHP - (baseAmount + lidAmount);    // calculate what health percentage is neck
		float neckFill = neckHealth / neckTotalHealth;
		neckFill = Mathf.Clamp(neckFill, 0, 1);
		healthNeck.fillAmount = neckFill;

		// Dynamic neck health
		//for (int i = 0; i < healthNecks.Length; i++)
		//{
		//	Debug.Log(healthNecks.Length);
		//	float neckHealth = BPC.currentHP - (baseAmount + (neckAmount * i));
		//	float neckTotalHealth = BPC.currentMaxHP - (baseAmount + lidAmount + (neckAmount * (healthNecks.Length - 1)));    // calculate what health percentage is neck
		//	float neckFill = neckHealth / neckTotalHealth;
		//	neckFill = Mathf.Clamp(neckFill, 0, 1);
		//	healthNecks[i].fillAmount = neckFill;
		//}

		Debug.Log("total neck amount: " + (neckAmount * (healthNecks.Length)));
		float lidHealth = BPC.currentHP - (baseAmount + (neckAmount * (healthNecks.Length)));
		float lidTotalHealth = BPC.currentMaxHP - (baseAmount + (neckAmount * (healthNecks.Length)));
		float lidFill = lidHealth / lidTotalHealth;
		lidFill = Mathf.Clamp(lidFill, 0, 1);
		healthLid.fillAmount = lidFill;
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
