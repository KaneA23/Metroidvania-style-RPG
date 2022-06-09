using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	private float healthlerpTimer;
	private float healthLerpSpeed;

	public Image healthFrontFillBar;
	public GameObject healthBarEmpty;

	public Image healthBackFillBar;

	[Header("Knockback")]
	public Rigidbody2D rb;

	[Header("Animations")]
	public bool isHit;
	public bool isDying;

	private float animDelay;

	public bool isEnemyBack;

	[Header("Hit Particles")]
	public ParticleSystem hurtParticle;


	[SerializeField] ParticleSystem leakParticles;
	ParticleSystem.EmissionModule em;

	[Header("Death animations")]
	public Animator transition;
	public float transitionTime = 1f;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
		PMS = GetComponent<PlayerMovementSystem>();

		DM = FindObjectOfType<DialogueManager>();

		rb = GetComponent<Rigidbody2D>();

		//hurtParticle = GetComponentInChildren<ParticleSystem>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.currentHP = BPC.currentMaxHP;
		BPC.maxRegenHP = (float)BPC.currentMaxHP * 0.2f;
		BPC.maxRegenHP = Mathf.RoundToInt(BPC.maxRegenHP);

		isHit = false;
		isDying = false;

		//em = leakParticles.emission;
		//em.enabled = false;

		UpdateHealthUI();
	}

	// Update is called once per frame
	void Update()
	{
		healthBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxHP, 32); // Changes size of player healthbar

		// Debugs
		//if (Input.GetKeyDown(KeyCode.Minus))
		//{
		//	TakeDamage(10, gameObject.transform.position, 0f, false);
		//}
		//if (Input.GetKeyDown(KeyCode.Equals))
		//{
		//	GainHealth(10);
		//}

		double fillF = System.Math.Round(healthFrontFillBar.fillAmount, 2);
		double fillB = System.Math.Round(healthBackFillBar.fillAmount, 2);

		// When player isn't being attacked, start regening up to 20% of max HP overtime
		if (!isHit && !isDying && fillB == fillF)
		{
			if (fillB == 1 && fillF == 1)
			{
				healthFrontFillBar.fillAmount = 1;
				healthBackFillBar.fillAmount = 1;
			}

			if (BPC.currentHP < BPC.maxRegenHP)
			{
				RegenHealth(BPC.maxRegenHP * Time.deltaTime);
			}
		}

		// If player has no more health, death animation plays
		if (BPC.currentHP <= 0 && !isDying)
		{
			isDying = true;
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_DEATH);
			animDelay = 4f;
			Invoke(nameof(KillPlayer), animDelay);

			FindObjectOfType<AudioManager>().PlayAudio("PlayerDeath");
		}

		UpdateHealthUI();
	}

	/// <summary>
	/// Player's health decreases when attacked
	/// </summary>
	/// <param name="a_damage">Amount of health lost from attack</param>
	/// <param name="a_enemyPos">player is knocked in opposite direction of enemy</param>
	/// <param name="a_knockForce">how hard the player is hit back</param>
	/// <param name="a_isKnockback">some attacks don't knock player back</param>
	public void TakeDamage(int a_damage, Vector2 a_enemyPos, float a_knockForce, bool a_isKnockback)
	{
		if (isEnemyBack || isDying)
		{
			return;
		}

		if (!PMS.isDashing && !DialogueManagerScript.GetInstance().IsDialoguePlaying /*&& !isHit && !FindObjectOfType<BernardIntroCutscene>().isCutscene*/)
		{
			PMS.isCrouching = false;    // Look at later point
			PMS.isRunning = false;

			//em.enabled = true;
			hurtParticle.Play();

			isHit = true;
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_HIT);
			FindObjectOfType<AudioManager>().PlayAudio("PlayerDamage");
			animDelay = 0.517f;
			Invoke(nameof(CompleteDaze), animDelay);

			if ((transform.position.x - a_enemyPos.x) < 0)
			{
				rb.velocity = Vector2.zero;
				//rb.AddForce(new Vector2(-1f * BPC.knockbackTaken, 250f));
				if (a_isKnockback)
				{
					rb.AddForce(new Vector2(-1f * a_knockForce, 250f));
				}
			}
			else if ((transform.position.x - a_enemyPos.x) > 0)
			{
				rb.velocity = Vector2.zero;
				//rb.AddForce(new Vector2(1f * BPC.knockbackTaken, 250f));
				if (a_isKnockback)
				{
					rb.AddForce(new Vector2(1f * a_knockForce, 250f));
				}
			}

			BPC.currentHP -= a_damage;

			healthlerpTimer = 0f;
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

		healthlerpTimer = 0f;
	}

	/// <summary>
	/// Increases amount of health player has (by regen or potion use)
	/// </summary>
	/// <param name="a_healthRegen">amount of replenished health</param>
	void RegenHealth(float a_healthRegen)
	{
		BPC.currentHP += a_healthRegen;

		// Caps amount of health
		if (BPC.currentHP > BPC.maxRegenHP)
		{
			BPC.currentHP = BPC.maxRegenHP;
		}

		healthlerpTimer = 0f;
	}

	/// <summary>
	/// Changes health bar UI by curring one of the fill GameObjects then lerping the other to the new fill amount
	/// </summary>
	public void UpdateHealthUI()
	{
		float fillF = healthFrontFillBar.fillAmount;
		float fillB = healthBackFillBar.fillAmount;
		float healthFraction = BPC.currentHP / (float)BPC.currentMaxHP;

		// Decreases healthbar UI when player takes damage
		if (fillB > healthFraction)
		{
			//leakParticles.Play();
			healthLerpSpeed = 15f;

			healthFrontFillBar.fillAmount = healthFraction;

			healthlerpTimer += Time.deltaTime;
			float percentComplete = healthlerpTimer / healthLerpSpeed;
			percentComplete *= percentComplete;

			healthBackFillBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
			//em.enabled = true;
		}
		else
		{
			//em.enabled = false;
			//leakParticles.Stop();
		}

		// Increases health UI when player regens health
		if (fillF < healthFraction)
		{
			healthLerpSpeed = 1f;
			healthBackFillBar.fillAmount = healthFraction;
			healthlerpTimer += Time.deltaTime;

			float percentComplete = healthlerpTimer / healthLerpSpeed;
			percentComplete *= percentComplete;

			healthFrontFillBar.fillAmount = Mathf.Lerp(fillF, healthBackFillBar.fillAmount, percentComplete);
		}
	}

	/// <summary>
	/// Destroys the player
	/// </summary>
	void KillPlayer()
	{
		//Destroy(gameObject);
		//gameObject.SetActive(false);
		if (SceneManager.GetActiveScene().name == "Level 1 - Temp - Play Test")
		{
			StartCoroutine(LoadLevel("StoryDeathScene"));
		}
		else
		{
			StartCoroutine(LoadLevel("ArenaDeathScene"));
		}
	}

	/// <summary>
	/// Allows player controls after hit animation ends
	/// </summary>
	void CompleteDaze()
	{
		isHit = false;
	}

	/// <summary>
	/// Changes scene to correct death scene after transition
	/// </summary>
	/// <param name="a_sceneName">Either the story or Arena death scene</param>
	/// <returns>Wait 1 second before loading new scene</returns>
	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
