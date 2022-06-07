using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the player Levelling Up system so the can get stronger with experience.
/// Created by: Kane Adams
/// </summary>
public class LevelSystem : MonoBehaviour
{
	BasePlayerClass BPC;
	GameObject eventSystem;

	[SerializeField] private int experienceLeft; // XP after player levelled up
	[SerializeField] private int experienceToNextLevel;

	public Image xpFrontFillBar;
	public Image xpBackFillBar;

	[Header("Experience bar objects")]
	public GameObject xpBarEmpty;
	public GameObject xpBar;

	[Header("Lerping Experience decrease")]
	private float xpLerpTimer;
	private float xpLerpSpeed;

	public TextMeshProUGUI xpText;

	Animator anim;
	public string currentAnimState;
	public float animDelay;

	[SerializeField] private int xpAmount;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		anim = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
	{
		BPC.Level = 1;      //temp
		BPC.currentXP = 0;  //temp

		xpBackFillBar.fillAmount = 0;
		xpFrontFillBar.fillAmount = 0;

		BPC.maxXP = BPC.Level * 100;

		experienceToNextLevel = BPC.maxXP - BPC.currentXP;

		ChangeAnimationState("XP_Inactive");
	}

	// Update is called once per frame
	void Update()
	{
		double fillF = System.Math.Round(xpFrontFillBar.fillAmount, 2);
		double fillB = System.Math.Round(xpBackFillBar.fillAmount, 2);

		// Debug
		//if (Input.GetKeyDown(KeyCode.L))
		//{
		//	GainExperience(Random.Range(10, 250));
		//}

		if (fillF == fillB)
		{
			if (experienceLeft > 0 && fillF == 0)
			{
				ChangeAnimationState("XP_Active");
				GainExperience(experienceLeft);
			}

			if (fillB == 1 && fillF == 1)
			{
				xpFrontFillBar.fillAmount = 1;
				xpBackFillBar.fillAmount = 1;

				// Levels up player once reached max XP (if not max level)
				if (BPC.currentXP == BPC.maxXP)
				{
					BPC.UpdateStats();

					if (BPC.Level < BPC.maxLvl)
					{
						if (experienceLeft > 0)
						{
							ChangeAnimationState("XP_Active");
							Invoke(nameof(CompleteAnim), animDelay);
						}

						BPC.currentXP = 0;
						BPC.Level++;
						BPC.maxXP = BPC.Level * 100;
					}
				}
			}
		}

		UpdateExperienceUI();
	}

	/// <summary>
	/// Called when enemy dies, Begins adding XP to player
	/// </summary>
	/// <param name="a_amount">XP earned from enemy</param>
	public void GainExperience(int a_amount)
	{
		xpAmount = a_amount;

		if (currentAnimState == "XP_Inactive")
		{
			ChangeAnimationState("XP_FadeIn");
			animDelay = 1f;
			Invoke(nameof(CompleteAnim), animDelay);
		}
		else
		{
			ChangeAnimationState("XP_Active");
			AddExperience();
		}
	}

	/// <summary>
	/// Increases player XP
	/// </summary>
	void AddExperience()
	{
		experienceLeft = xpAmount - experienceToNextLevel;

		if (experienceLeft <= 0)
		{
			experienceLeft = 0;
		}

		if (xpAmount >= experienceToNextLevel)
		{
			BPC.currentXP += experienceToNextLevel;
		}
		else
		{
			BPC.currentXP += xpAmount;
		}

		xpAmount = 0;
		experienceToNextLevel = BPC.maxXP - BPC.currentXP;
		xpLerpTimer = 0f;
		animDelay = 2f;

		// If there is more XP to add, wait before removing
		if (xpAmount <= 0)
		{
			if (experienceLeft <= 0 && currentAnimState == "XP_Active")
			{
				Invoke(nameof(CompleteAnim), animDelay);
			}
		}
	}

	/// <summary>
	/// Created lerping effect when player gains XP
	/// </summary>
	public void UpdateExperienceUI()
	{
		float fillF = xpFrontFillBar.fillAmount;
		float fillB = xpBackFillBar.fillAmount;

		float xpFraction = (float)BPC.currentXP / (float)BPC.maxXP;

		//Decreases stambar UI when player loses stamina
		if (fillB > xpFraction)
		{
			xpLerpSpeed = 15f;

			xpFrontFillBar.fillAmount = xpFraction;
			xpLerpTimer += Time.deltaTime;

			float percentComplete = xpLerpTimer / xpLerpSpeed;
			percentComplete *= percentComplete;

			xpBackFillBar.fillAmount = Mathf.Lerp(fillB, xpFraction, percentComplete);
		}

		// Incrases stambar UI when player regens stamina
		if (fillF < xpFraction)
		{
			xpLerpSpeed = 1f;
			xpBackFillBar.fillAmount = xpFraction;
			xpLerpTimer += Time.deltaTime;

			float percentComplete = xpLerpTimer / xpLerpSpeed;
			percentComplete *= percentComplete;

			xpFrontFillBar.fillAmount = Mathf.Lerp(fillF, xpBackFillBar.fillAmount, percentComplete);
		}

		xpText.text = BPC.currentXP + " / " + BPC.maxXP;

	}

	/// <summary>
	/// Waits for animation to finish before starting next animation
	/// </summary>
	void CompleteAnim()
	{
		if (currentAnimState == "XP_FadeIn")
		{
			ChangeAnimationState("XP_Active");
			AddExperience();
		}
		else if (currentAnimState == "XP_Active")
		{
			ChangeAnimationState("XP_FadeOut");
			animDelay = 1f;
			Invoke(nameof(CompleteAnim), animDelay);
		}
		else if (currentAnimState == "XP_FadeOut")
		{
			ChangeAnimationState("XP_Inactive");
		}
	}

	/// <summary>
	/// Changes animation dependent on XP bar actions
	/// </summary>
	/// <param name="a_newAnim">New animation</param>
	public void ChangeAnimationState(string a_newAnim)
	{
		// Stops the same animation from interrupting itself
		if (currentAnimState == a_newAnim)
		{
			return;
		}

		// Play the animation
		anim.Play(a_newAnim);

		// reassign current state
		currentAnimState = a_newAnim;
	}
}
