using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls player's Mana UI.
/// Created by: Kane Adams
/// </summary>
public class PlayerManaSystem : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private BasePlayerClass BPC;
	private PlayerHealthSystem PHS;
	private PlayerMovementSystem PMS;

	[Space(5)]
	GameObject eventSystem;
	GameObject player;

	[Header("Mana bar objects")]
	public GameObject manaBarEmpty;

	public Image manaFrontFillBar;
	public Image manaBackFillBar;

	[Header("Lerping Mana decresae")]
	private float manaLerpTimer;
	private float manaLerpSpeed;

	//public bool isManaCooldown;

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
		BPC.currentMP = BPC.currentMaxMP;
		BPC.maxRegenMP = (float)BPC.currentMaxMP * BPC.maxRegenMP;
		BPC.maxRegenMP = Mathf.RoundToInt(BPC.maxRegenMP);

		//isManaCooldown = false;
	}

	// Update is called once per frame
	void Update()
	{
		// Debugs
		//if (Input.GetKeyDown(KeyCode.Alpha9))
		//{
		//	TakeMana(10);
		//}
		//if (Input.GetKeyDown(KeyCode.Alpha0))
		//{
		//	RegenMana(10);
		//}

		manaBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.currentMaxMP, 32);   // Changes size of player stamina bar

		double fillF = System.Math.Round(manaFrontFillBar.fillAmount, 2);
		double fillB = System.Math.Round(manaBackFillBar.fillAmount, 2);

		// When no mana is being used, start regening up to 30% of max MP overtime
		if (!PHS.isDying)
		{
			if (!PMS.isDashing && fillF == fillB)
			{
				if (fillB == 1 && fillF == 1)
				{
					manaFrontFillBar.fillAmount = 1;
					manaBackFillBar.fillAmount = 1;
				}

				if (BPC.currentMP < BPC.maxRegenMP)
				{
					RegenMana(BPC.regenRateMP * Time.deltaTime);
				}
			}
		}

		UpdateManaUI();
	}

	/// <summary>
	/// Decreases the amount of mana player can use
	/// </summary>
	/// <param name="a_manaCost">how much mana a spell uses</param>
	public void TakeMana(int a_manaCost)
	{
		BPC.currentMP -= a_manaCost;

		// prevents negative mana
		if (BPC.currentMP < 0)
		{
			BPC.currentMP = 0;
		}

		manaLerpTimer = 0f;
	}

	/// <summary>
	/// Increases amount of mana player has (by regen or potion use)
	/// </summary>
	/// <param name="a_manaRegen">amount of replenished mana</param>
	public void RegenMana(float a_manaRegen)
	{
		BPC.currentMP += a_manaRegen;

		// Caps amount of mana
		if (BPC.currentMP > BPC.currentMaxMP)
		{
			BPC.currentMP = BPC.currentMaxMP;
		}

		manaLerpTimer = 0f;
	}

	/// <summary>
	/// Changes mana bar UI by cutting one of the fill GameObjects then lerping the other to the new fill amount
	/// </summary>
	public void UpdateManaUI()
	{
		float fillF = manaFrontFillBar.fillAmount;
		float fillB = manaBackFillBar.fillAmount;

		float manaFraction = (float)BPC.currentMP / (float)BPC.currentMaxMP;

		// Decreases manabar UI when player loses mana
		if (fillB > manaFraction)
		{
			manaLerpSpeed = 15f;
			manaFrontFillBar.fillAmount = manaFraction;

			manaLerpTimer += Time.deltaTime;
			float percentComplete = manaLerpTimer / manaLerpSpeed;
			percentComplete *= percentComplete;

			manaBackFillBar.fillAmount = Mathf.Lerp(fillB, manaFraction, percentComplete);
		}

		// Increases manabar UI when player regens mana
		if (fillF < manaFraction)
		{
			manaLerpSpeed = 1f;
			manaBackFillBar.fillAmount = manaFraction;
			manaLerpTimer += Time.deltaTime;

			float percentComplete = manaLerpTimer / manaLerpSpeed;
			percentComplete *= percentComplete;

			manaFrontFillBar.fillAmount = Mathf.Lerp(fillF, manaBackFillBar.fillAmount, percentComplete);
		}
	}
}
