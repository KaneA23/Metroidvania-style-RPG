using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Different states for the animation to change dependent on player actions.
/// </summary>
public enum PlayerAnimationState
{
	// Movement
	PLAYER_IDLE,
	PLAYER_WALK,
	PLAYER_RUN,
	PLAYER_JUMPLAUNCH,
	PLAYER_JUMPFALL,
	PLAYER_JUMPLAND,
	PLAYER_DASH,

	// Combat
	PLAYER_SWORDATTACK,
	PLAYER_HEAVYATTACK,
	PLAYER_HIT,
	PLAYER_DEATH,
}

/// <summary>
/// Controls the change in the player's animations.
/// Created by: Kane Adams
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
	private Animator anim;

	public string currentAnimState;

	private string[] animations = { 
		"Player_Idle", "Player_Walk", "Player_Run", "Player_JumpLaunch", "Player_JumpFall", "Player_JumpLand", "Player_Dash", "Player_SwordAttack", "Player_HeavyAttack", "Player_Hit", "Player_Death"
	};

	//// Movement
	//public string PLAYER_IDLE = "Player_Idle";
	//public string PLAYER_WALK = "Player_Idle";
	//public string PLAYER_RUN
	//public string PLAYER_JUMPLAUNCH
	//public string PLAYER_JUMPFALL
	//public string PLAYER_JUMPLAND
	//public string PLAYER_DASH

	//// Combat
	//public string PLAYER_SWORDATTACK
	//public string PLAYER_HEAVYATTACK
	//public string PLAYER_HIT
	//public string PLAYER_DEATH

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	///// <summary>
	///// Changes player's current state
	///// </summary>
	///// <param name="a_newState">The new animation state</param>
	//public void ChangeAnimationState(PlayerAnimationState a_newState)
	//{
	//	switch (a_newState)
	//	{
	//		case PlayerAnimationState.PLAYER_IDLE:
	//			ChangeAnimation("Player_Idle");
	//			break;
	//		case PlayerAnimationState.PLAYER_WALK:
	//			ChangeAnimation("Player_Walk");
	//			break;
	//		case PlayerAnimationState.PLAYER_RUN:
	//			ChangeAnimation("Player_Run");
	//			break;
	//		case PlayerAnimationState.PLAYER_JUMPLAUNCH:
	//			ChangeAnimation("Player_JumpLaunch");
	//			break;
	//		case PlayerAnimationState.PLAYER_JUMPFALL:
	//			ChangeAnimation("Player_JumpFall");
	//			break;
	//		case PlayerAnimationState.PLAYER_JUMPLAND:
	//			ChangeAnimation("Player_Land");
	//			break;
	//		case PlayerAnimationState.PLAYER_DASH:
	//			ChangeAnimation("Player_Dash");
	//			break;
	//		case PlayerAnimationState.PLAYER_SWORDATTACK:
	//			ChangeAnimation("Player_SwordAttack");
	//			break;
	//		case PlayerAnimationState.PLAYER_HEAVYATTACK:
	//			ChangeAnimation("Player_HeavyAttack");
	//			break;
	//		case PlayerAnimationState.PLAYER_HIT:
	//			ChangeAnimation("Player_Hit");
	//			break;
	//		case PlayerAnimationState.PLAYER_DEATH:
	//			ChangeAnimation("Player_Death");
	//			break;
	//		default:
	//			ChangeAnimation("Player_Idle");
	//			break;
	//	}
	//}

	///// <summary>
	///// Changes player's current animation
	///// </summary>
	///// <param name="a_newAnim">The player's new animation</param>
	//void ChangeAnimation(string a_newAnim)
	//{
	//	// Stops the same animation from interrupting itself
	//	if (currentAnimState == a_newAnim)
	//	{
	//		return;
	//	}

	//	// Play the animation
	//	anim.Play(a_newAnim);

	//	// reassign current state
	//	currentAnimState = a_newAnim;
	//}

	/// <summary>
	/// Changes the animation for what the player is doing
	/// </summary>
	/// <param name="a_newAnim">New animation state</param>
	public void ChangeAnimationState(PlayerAnimationState a_newAnim)
	{
		// Stops the same animation from interrupting itself
		if (currentAnimState == animations[(int)a_newAnim])
		{
			return;
		}

		// Play the animation
		anim.Play(animations[(int)a_newAnim]);

		// reassign current state
		currentAnimState = animations[(int)a_newAnim];
	}
}
