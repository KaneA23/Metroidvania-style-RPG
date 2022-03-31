using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls player's Mana UI
/// Created by: Kane Adams
/// </summary>
public class PlayerManaSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	public PlayerMovementSystem PMS;
	BasePlayerClass BPC;

	[Space(5)]
	GameObject eventSystem;


	[Header("Mana bar objects")]
	public GameObject manaBarEmpty;

	public Image manaFrontFillBar;
	public Image manaBackFillBar;

	[Header("Lerping Mana decresae")]
	public float manaLerpTimer;
	public float manaLerpSpeed = 15f;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.currentMP = BPC.currentMaxMP;
	}

	// Update is called once per frame
	void Update()
	{
		manaBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxStam, 32);   // Changes size of player stamina bar

		if (!PMS.isDashing)
		{
			if (BPC.currentMP < BPC.currentMaxMP)
			{
				RegenMana(BPC.regenRateMP * Time.deltaTime);
			}
		}

		UpdateManaUI();
	}

	public void TakeMana(int a_manaCost)
	{
		BPC.currentMP -= a_manaCost;

		if (BPC.currentMP < 0)
		{
			BPC.currentMP = 0;
		}

		manaLerpTimer = 0f;
	}

	public void RegenMana(float a_manaRegen)
	{
		BPC.currentMP += a_manaRegen;

		if (BPC.currentMP > BPC.currentMaxMP)
		{
			BPC.currentMP = BPC.currentMaxMP;
		}

		manaLerpTimer = 0f;
	}

	public void UpdateManaUI()
	{
		float fillF = manaFrontFillBar.fillAmount;
		float fillB = manaBackFillBar.fillAmount;

		float manaFraction = (float)BPC.currentMP / (float)BPC.currentMaxMP;

		if (fillB > manaFraction)
		{
			manaFrontFillBar.fillAmount = manaFraction;

			manaLerpTimer += Time.deltaTime;
			float percentComplete = manaLerpTimer / manaLerpSpeed;
			percentComplete *= percentComplete;

			manaBackFillBar.fillAmount = Mathf.Lerp(fillB, manaFraction, percentComplete);
		}

		if (fillF < manaFraction)
		{
			manaBackFillBar.fillAmount = manaFraction;
			manaLerpTimer += Time.deltaTime;
			float percentComplete = manaLerpTimer / manaLerpSpeed;
			percentComplete *= percentComplete;
			manaFrontFillBar.fillAmount = Mathf.Lerp(fillF, manaBackFillBar.fillAmount, percentComplete);
		}
	}
}
