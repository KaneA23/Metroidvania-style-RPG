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
	PLAYER_CROUCHENTER,
	PLAYER_CROUCHIDLE,
	PLAYER_CROUCHWALK,
	PLAYER_CROUCHEXIT,
	PLAYER_DASHENTER,
	PLAYER_DASH,
	PLAYER_DASHEXIT,

	// Combat
	PLAYER_SWORDATTACK,
	PLAYER_HEAVYATTACK,
	PLAYER_HIT,
	PLAYER_DEATH,
	PLAYER_FIREBALLLAUNCH,
}


/// <summary>
/// Controls the change in the player's animations.
/// Created by: Kane Adams
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
	private Animator anim;

	public string currentAnimState;
	private readonly string[] animations = {
		"Player_Idle", "Player_Walk", "Player_Run", "Player_JumpLaunch", "Player_JumpFall", "Player_JumpLand", "Player_CrouchEnter", "Player_CrouchIdle", "Player_CrouchWalk", "Player_CrouchExit", "Player_DashEnter", "Player_Dash", "Player_DashExit",
		"Player_SwordAttack", "Player_HeavyAttack", "Player_Hit", "Player_Death", "Player_FireballLaunch"
	};

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
	}

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
