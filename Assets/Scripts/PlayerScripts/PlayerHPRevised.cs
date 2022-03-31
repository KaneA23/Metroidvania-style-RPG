using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by: Jacob ey Kane
/// </summary>
public class PlayerHPRevised : MonoBehaviour
{
	public BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	public PlayerMovementSystem PMS;

	public Image bottleFill;	// Bottle fill always 10HP
	public Image neckFill;		// initially 5HP, increments by 5
	public Image bottleEndFill;	// End fill always 5HP

	public GameObject neck;

	public float fbottleFill = 10;
	public float fneckFill = 5;
	public float fbottleEndFill = 5;

	public float endPartA;
	public float endPartB;

	float dynamicMaxHPEnd;
	float dynamicCurrentHPEnd;

	private void Start()
	{
		PMS = GetComponent<PlayerMovementSystem>();
		Debug.Log("EndMax " + dynamicMaxHPEnd);
		Debug.Log("EndCur " + dynamicCurrentHPEnd);

	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			TakeDamage(1, gameObject.transform.position);
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
			BPC.currentHP -= a_damage;

			FillHealthBar();
		}
	}

	void FillHealthBar()
	{
		int mHP = BPC.currentMaxHP;
		int cHP = BPC.currentHP;

		endPartA = fneckFill + fbottleFill;

		Debug.Log(endPartA);

		//Debug.Log("current HP: " + cHP);

		dynamicMaxHPEnd = mHP - endPartA;
		Debug.Log("EndMax" + dynamicMaxHPEnd);
		dynamicCurrentHPEnd = cHP - endPartA;
		Debug.Log("EndCur" + dynamicCurrentHPEnd);

		float nBEFA = dynamicCurrentHPEnd / dynamicMaxHPEnd;
		//nCHP = Mathf.Clamp01(cHP);



		Debug.Log("End fill HP: " + nBEFA);
		bottleEndFill.fillAmount = nBEFA;

		if (bottleEndFill.fillAmount <= 0)
		{
			endPartB = fbottleFill;
			float dynamicMaxHPNeck = mHP - endPartB;
			float dynamicCurrentHPNeck = cHP - endPartB;

			Debug.Log("Neck Curr " + dynamicCurrentHPNeck);
			Debug.Log("Neck Max "+dynamicMaxHPNeck);

			float nBNFA = dynamicCurrentHPNeck / dynamicMaxHPNeck;
			Debug.Log("neck nBNFA " + nBNFA);
		
			neckFill.fillAmount = nBNFA;
		
			//if (neckFill.fillAmount <= 0)
			//{
			//	float dynamicMaxHPStart = mHP;
			//	float dynamicCurrentHPStart = cHP;
			//
			//	float nBSFA = (float)dynamicCurrentHPStart / (float)dynamicMaxHPStart;
			//
			//	bottleEndFill.fillAmount = nBSFA;
			//}
		}



		//if (cHP > (fbottleFill + fneckFill))
		//{
		//	Debug.Log("Called");
		//	float endAmount = mHP - (fbottleFill + fneckFill);
		//	//float endFill = Mathf.Clamp01(endAmount);
		//	float endFill = endAmount / 5f;
		//	Debug.Log("end fill, normalised: " + endFill);
		//	bottleEndFill.fillAmount = endFill;
		//}
	}
}
