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
	DialogueManager DM;

	GameObject eventSystem;

	public float lerpTimer;
	public float healthLerpSpeed;

	public Image healthFrontFillBar;
	public GameObject healthBarEmpty;

	public Image healthBackHealthBar;

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

		DM = FindObjectOfType<DialogueManager>();

		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.currentHP = BPC.currentMaxHP;
		BPC.maxRegenHP = (float)BPC.currentMaxHP * 0.2f;
		BPC.maxRegenHP = Mathf.RoundToInt(BPC.maxRegenHP);
		isHit = false;
		isDying = false;

		UpdateHealthUI();
	}

	// Update is called once per frame
	void Update()
	{
		healthBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxHP, 32);

		if (Input.GetKeyDown(KeyCode.Minus))
		{
			TakeDamage(Random.Range(5, 10), gameObject.transform.position, 0f, false);
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			GainHealth(1);
		}

		float fillF = Mathf.Round(healthFrontFillBar.fillAmount * 100) * 0.01f;
		float fillB = Mathf.Round(healthBackHealthBar.fillAmount * 100) * 0.01f;

		if (!isHit && !isDying && fillB == fillF)
		{
			if (BPC.currentHP < BPC.maxRegenHP)
			{
				RegenHealth(BPC.maxRegenHP * Time.deltaTime);
			}

		}

		if (BPC.currentHP <= 0 && !isHit)
		{
			isDying = true;
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DEATH);
			animDelay = 4f;
			Invoke(nameof(KillPlayer), animDelay);
		}

		UpdateHealthUI();
	}

	/// <summary>
	/// Player's health decreases when attacked
	/// </summary>
	/// <param name="a_damage">Amount of health lost from attack</param>
	public void TakeDamage(int a_damage, Vector2 a_enemyPos, float a_knockForce, bool a_isKnockback)
	{
		if (!PMS.isDashing && !DialogueManagerScript.GetInstance().IsDialoguePlaying)
		{
			isHit = true;
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HIT);
			animDelay = 0.517f;
			Invoke(nameof(CompleteDaze), animDelay);

			if ((transform.position.x - a_enemyPos.x) < 0)
			{
				Debug.Log("Left Hit");
				rb.velocity = Vector2.zero;
				//rb.AddForce(new Vector2(-1f * BPC.knockbackTaken, 250f));
				if (a_isKnockback)
				{
					rb.AddForce(new Vector2(-1f * a_knockForce, 250f));
				}
			}
			else if ((transform.position.x - a_enemyPos.x) > 0)
			{
				Debug.Log("Right Hit");
				rb.velocity = Vector2.zero;
				//rb.AddForce(new Vector2(1f * BPC.knockbackTaken, 250f));
				if (a_isKnockback)
				{
					rb.AddForce(new Vector2(1f * a_knockForce, 250f));
				}
			}

			BPC.currentHP -= a_damage;
			lerpTimer = 0f;
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
			BPC.currentHP = BPC.currentMaxHP;
		}

		lerpTimer = 0f;
	}

	void RegenHealth(float a_healthRegen)
	{
		BPC.currentHP += a_healthRegen;

		if (BPC.currentHP > BPC.maxRegenHP)
		{
			BPC.currentHP = BPC.maxRegenHP;
		}

		lerpTimer = 0f;
	}

	/// <summary>
	/// Decreases/Increases bar fills when health changes with chipping effect
	/// </summary>
	public void UpdateHealthUI()
	{
		float fillF = healthFrontFillBar.fillAmount;
		float fillB = healthBackHealthBar.fillAmount;

		float healthFraction = BPC.currentHP / (float)BPC.currentMaxHP;

		if (fillB > healthFraction)
		{
			healthLerpSpeed = 15f;

			healthFrontFillBar.fillAmount = healthFraction;

			lerpTimer += Time.deltaTime;
			float percentComplete = lerpTimer / healthLerpSpeed;
			percentComplete *= percentComplete;

			healthBackHealthBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
		}

		if (fillF < healthFraction)
		{
			healthLerpSpeed = 1f;

			healthBackHealthBar.fillAmount = healthFraction;
			lerpTimer += Time.deltaTime;
			float percentComplete = lerpTimer / healthLerpSpeed;
			percentComplete *= percentComplete;
			healthFrontFillBar.fillAmount = Mathf.Lerp(fillF, healthBackHealthBar.fillAmount, percentComplete);
		}
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
