using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackSystem : MonoBehaviour
{
	public Transform firePoint;

	public GameObject fireballPrefab;

	BasePlayerClass BPC;
	PlayerAnimationManager PAM;
	public PlayerManaSystem PMS;

	GameObject eventSystem;

	public bool isFireball;

	private void Awake()
	{
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAM = GetComponent<PlayerAnimationManager>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q) && BPC.currentMP >= BPC.fireballCost)
		{
			isFireball = true;

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
	}

	void CompleteAnim()
	{
		isFireball = false;
	}
}
