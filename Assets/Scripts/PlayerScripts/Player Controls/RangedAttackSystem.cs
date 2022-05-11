using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedAttackSystem : MonoBehaviour
{
	public Transform firePoint;

	public GameObject fireballPrefab;

	BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	public PlayerManaSystem PMS;

	GameObject eventSystem;

	public bool isFireball;

	[SerializeField]
	Image manaCooldownUI;

	public bool isManaCooldown;
	[SerializeField] private float cooldownTimer;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		isManaCooldown = false;
		manaCooldownUI.fillAmount = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("ManaCooldown:  " + isManaCooldown);
		if (isManaCooldown)
		{
			//Debug.Log("Open");
			ApplyCooldown();
		}
		else if (Input.GetKeyDown(KeyCode.Q) && BPC.currentMP >= BPC.fireballCost)
		{
			isFireball = true;
			//Debug.Log("manacooldown: " + isManaCooldown);
			cooldownTimer = BPC.dashCooldown;

			Shoot();
			PMS.TakeMana(BPC.fireballCost);
			PAM.ChangeAnimationState(PlayerAnimationState.PLAYER_FIREBALLLAUNCH);
			Invoke(nameof(CompleteAnim), 0.35f);
		}
	}

	void Shoot()
	{
		//GetComponent<PlayerMovementSystem>().isFacingRight
		Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
		isManaCooldown = true;
		cooldownTimer = BPC.rangeAtkCooldown;
		ApplyCooldown();
	}

	void CompleteAnim()
	{
		isFireball = false;
	}

	/// <summary>
	/// 
	/// </summary>
	void ApplyCooldown()
	{
		cooldownTimer -= Time.deltaTime;

		if (cooldownTimer <= 0)
		{
			isManaCooldown = false;
			manaCooldownUI.fillAmount = 0.0f;
		}
		else
		{
			manaCooldownUI.fillAmount = Mathf.Clamp(cooldownTimer / BPC.dashCooldown, 0, 1);
		}
	}
}
