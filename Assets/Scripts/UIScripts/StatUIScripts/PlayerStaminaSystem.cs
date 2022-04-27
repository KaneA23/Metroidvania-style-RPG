using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls player's stamina UI
/// Created by: Kane Adams
/// </summary>
public class PlayerStaminaSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private BasePlayerClass BPC;
	private PlayerHealthSystem PHS;
	private PlayerMovementSystem PMS;

	[Space(5)]
	GameObject eventSystem;
	GameObject player;

	[Header("Stamina bar objects")]
	public GameObject stamBarEmpty;

	public Image stamFrontFillBar;
	public Image stamBackFillBar;

	[Header("Lerping Stamina decresae")]
	private float stamLerpTimer;
	private float stamLerpSpeed;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		player = GameObject.Find("Player");
		PHS = player.GetComponent<PlayerHealthSystem>();
		PMS = player.GetComponent<PlayerMovementSystem>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.currentStam = BPC.currentMaxStam;
	}

	// Update is called once per frame
	void Update()
	{
		if (BPC.hasRun)
		{
			stamBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxStam, 32);   // Changes size of player stamina bar

			float fillF = Mathf.Round(stamFrontFillBar.fillAmount * 100) * 0.01f;
			float fillB = Mathf.Round(stamBackFillBar.fillAmount * 100) * 0.01f;

			//Debug.Log("FillF: " + fillF);
			//Debug.Log("FillB: " + fillB);

			// When no stamina is being used, start regening up overtime
			if (!PHS.isDying)
			{
				if (!PMS.isRunning && !PMS.isJumping && fillF == fillB)
				{
					if (BPC.currentStam < BPC.currentMaxStam)
					{
						RegenStamina(BPC.regenRateStam * Time.deltaTime);
					}
				}
			}

			UpdateStamUI();
		}
	}

	/// <summary>
	/// Decreases the amount of stamina player has left
	/// </summary>
	/// <param name="a_stamCost">Amount of stamina a movement uses</param>
	public void TakeStamina(float a_stamCost)
	{
		BPC.currentStam -= a_stamCost;

		// Prevents negative stamina
		if (BPC.currentStam < 0)
		{
			BPC.currentStam = 0;
		}

		stamLerpTimer = 0f;
	}

	/// <summary>
	/// Increases amount of player has through regen
	/// </summary>
	/// <param name="a_stamRegen">amount of replenished stamina</param>
	public void RegenStamina(float a_stamRegen)
	{
		BPC.currentStam += a_stamRegen;

		// Caps amount of stamina
		if (BPC.currentStam > BPC.currentMaxStam)
		{
			BPC.currentStam = BPC.currentMaxStam;
		}

		stamLerpTimer = 0f;
	}

	/// <summary>
	/// Changes staminabar UI by cutting one of the fill GameObjects then lerping the other to the new fill amount
	/// </summary>
	public void UpdateStamUI()
	{
		float fillF = stamFrontFillBar.fillAmount;
		float fillB = stamBackFillBar.fillAmount;

		float stamFraction = (float)BPC.currentStam / (float)BPC.currentMaxStam;

		// Decreases stambar UI when player loses stamina
		if (fillB > stamFraction)
		{
			stamLerpSpeed = 15f;

			stamFrontFillBar.fillAmount = stamFraction;
			stamLerpTimer += Time.deltaTime;

			float percentComplete = stamLerpTimer / stamLerpSpeed;
			percentComplete *= percentComplete;

			stamBackFillBar.fillAmount = Mathf.Lerp(fillB, stamFraction, percentComplete);
		}

		// Incrases stambar UI when player regens stamina
		if (fillF < stamFraction)
		{
			stamLerpSpeed = 0.5f;
			stamBackFillBar.fillAmount = stamFraction;
			stamLerpTimer += Time.deltaTime;

			float percentComplete = stamLerpTimer / stamLerpSpeed;
			percentComplete *= percentComplete;

			stamFrontFillBar.fillAmount = Mathf.Lerp(fillF, stamBackFillBar.fillAmount, percentComplete);
		}
	}
}
