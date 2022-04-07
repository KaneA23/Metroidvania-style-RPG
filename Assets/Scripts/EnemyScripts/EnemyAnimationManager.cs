using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Different states for the animataion to change dependent on AI's actions
/// </summary>
public enum AIAnimationState
{
	// Earth elementals
	EARTHELEMENTAL_CALMWALK,
	EARTHELEMENTAL_ALERT,
	EARTHELEMENTAL_AGROWALK,
	EARTHELEMENTAL_FORGET,

	// Fire elementals
	FIREELEMENTAL_IDLE,
	FIREELEMENTAL_ALERT,
	FIREELEMENTAL_ATTACK,
	FIREELEMENTAL_FORGET,
}


/// <summary>
/// Controls the change in AI animations.
/// Created by: Kane Adams
/// </summary>
public class EnemyAnimationManager : MonoBehaviour
{
	private Animator anim;

	public string currentAnimState;
	private string[] animations = {
		"EarthElemental_CalmMove", "EarthElemental_Alert", "EarthElemental_AgroMove", "EarthElemental_Forget",
		"FireElemental_Idle", "FireElemental_Alert", "FireElemental_Attack", "FireElemental_Forget"
	};

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
	}

	public void ChangeAnimationState(AIAnimationState a_newAnim)
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
