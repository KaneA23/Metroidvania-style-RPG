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
	public PlayerMovementSystem PMS;
	BasePlayerClass BPC;

	[Space(5)]
	GameObject eventSystem;

	[Header("Stamina bar objects")]
	public GameObject stamBarEmpty;

	public Image stamFrontFillBar;
	public Image stamBackFillBar;

	[Header("Lerping Stamina decresae")]
	public float stamLerpTimer;
	public float stamLerpSpeed/* = 15f*/;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.currentStam = BPC.currentMaxStam;
	}

	// Update is called once per frame
	void Update()
	{
		stamBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxStam, 32);   // Changes size of player stamina bar

		float fillF = Mathf.Round(stamFrontFillBar.fillAmount * 100) * 0.01f;
		float fillB = Mathf.Round(stamBackFillBar.fillAmount * 100) * 0.01f;

		if (!PMS.isRunning && !PMS.isJumping && fillF == fillB)
		{
			if (BPC.currentStam < BPC.currentMaxStam)
			{
				RegenStamina(BPC.regenRateStam * Time.deltaTime);
			}
		}

		UpdateStamUI();
	}

	public void TakeStamina(float a_stamCost)
	{
		BPC.currentStam -= a_stamCost;

		if (BPC.currentStam < 0)
		{
			BPC.currentStam = 0;
		}

		stamLerpTimer = 0f;
	}

	public void RegenStamina(float a_stamRegen)
	{
		BPC.currentStam += a_stamRegen;

		if (BPC.currentStam > BPC.currentMaxStam)
		{
			BPC.currentStam = BPC.currentMaxStam;
		}

		stamLerpTimer = 0f;
	}

	public void UpdateStamUI()
	{
		float fillF = stamFrontFillBar.fillAmount;
		float fillB = stamBackFillBar.fillAmount;

		float stamFraction = (float)BPC.currentStam / (float)BPC.currentMaxStam;

		if (fillB > stamFraction)
		{
			stamLerpSpeed = 15f;

			stamFrontFillBar.fillAmount = stamFraction;
			stamLerpTimer += Time.deltaTime;

			float percentComplete = stamLerpTimer / stamLerpSpeed;
			percentComplete *= percentComplete;

			stamBackFillBar.fillAmount = Mathf.Lerp(fillB, stamFraction, percentComplete);
		}


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
