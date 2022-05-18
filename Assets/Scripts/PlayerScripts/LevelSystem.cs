using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
	BasePlayerClass BPC;
	GameObject eventSystem;

	private int experienceLeft; // XP after player levelled up
	private int experienceToNextLevel;

	public Image xpFrontFillBar;
	public Image xpBackFillBar;

	[Header("Experience bar objects")]
	public GameObject xpBarEmpty;

	[Header("Lerping Experience decrease")]
	private float xpLerpTimer;
	private float xpLerpSpeed;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();
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
		//Debug.Log("CurrentXP: " + BPC.currentXP);
		//Debug.Log("Level: " + BPC.Level);
		//Debug.Log("maxXP: " + BPC.maxXP);
		//Debug.Log("Next level in: " + experienceToNextLevel);
	}

	// Update is called once per frame
	void Update()
	{
		xpBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(BPC.maxXP, 32);

		double fillF = System.Math.Round(xpFrontFillBar.fillAmount, 2);
		double fillB = System.Math.Round(xpBackFillBar.fillAmount, 2);

		if (fillF == fillB)
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				AddExperience(Random.Range(10, 150));
			}


			if (experienceLeft > 0)
			{
				AddExperience(experienceLeft);
			}



			if (fillB == 1 && fillF == 1)
			{
				xpFrontFillBar.fillAmount = 1;
				xpBackFillBar.fillAmount = 1;

				if (BPC.currentXP == BPC.maxXP && BPC.Level < BPC.maxLvl)
				{
					//Debug.Log("LEVEL UP!");
					BPC.currentXP = 0;
					BPC.Level++;
					BPC.maxXP = BPC.Level * 100;
					BPC.UpdateStats();
				}
			}
		}

		UpdateExperienceUI();
	}

	public void AddExperience(int a_amount)
	{
		//Debug.Log("CurrentXP: " + BPC.currentXP);
		//Debug.Log("Level: " + BPC.Level);
		//Debug.Log("Next level in: " + experienceToNextLevel);

		experienceLeft = a_amount - experienceToNextLevel;
		if (experienceLeft <= 0)
		{
			experienceLeft = 0;
		}

		if (a_amount >= experienceToNextLevel)
		{
			BPC.currentXP += experienceToNextLevel;
		}
		else
		{
			BPC.currentXP += a_amount;
		}

		double fillF = System.Math.Round(xpFrontFillBar.fillAmount, 2);
		double fillB = System.Math.Round(xpBackFillBar.fillAmount, 2);


		experienceToNextLevel = BPC.maxXP - BPC.currentXP;
		xpLerpTimer = 0f;

		//Debug.Log("XP left: " + experienceLeft);
		//Debug.Log("CurrentXP: " + BPC.currentXP);
		//Debug.Log("Level: " + BPC.Level);
		//Debug.Log("Next level in: " + experienceToNextLevel);
	}

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
	}
}
