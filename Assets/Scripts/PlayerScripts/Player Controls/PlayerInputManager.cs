using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all the inputs the player 
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private BasePlayerClass BPC;
	private PlayerAnimationManager PAM;
	private PlayerCombatSystem PCS;
	private PlayerHealthSystem PHS;
	private PlayerMovementSystem PMS;

	private GameObject player;

	[Header("Input variables")]
	public float moveHorizontal;

	private void Awake()
	{
		BPC = GetComponent<BasePlayerClass>();

		player = GameObject.Find("Player");
		PAM = player.GetComponent<PlayerAnimationManager>();
		PCS = player.GetComponent<PlayerCombatSystem>();
		PHS = player.GetComponent<PlayerHealthSystem>();
		PMS = player.GetComponent<PlayerMovementSystem>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!PHS.isHit && !PHS.isDying /*&& !DialogueManagerScript.GetInstance().IsDialoguePlaying*/)
		{
			PlayerInputs();
		}
	}

	/// <summary>
	/// Collects player's inputs to allow correct actions
	/// </summary>
	public void PlayerInputs()
	{
		moveHorizontal = Input.GetAxisRaw("Horizontal");

		if (!DialogueManagerScript.GetInstance().IsDialoguePlaying)
		{
			if (PAM.currentAnimState != "Player_Dash" && PAM.currentAnimState != "Player_DashExit")
			{
				if ((moveHorizontal > 0 && !PMS.isFacingRight) || (moveHorizontal < 0 && PMS.isFacingRight))
				{
					player.transform.Rotate(new Vector2(0, 180));
					PMS.isFacingRight = !PMS.isFacingRight;
				}
			}

			if (!PMS.isDashing)
			{
				//if (Input.GetButtonDown("Jump") && PMS.jumpCount < 1 && 
			}
		}
	}
}
