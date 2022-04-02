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
		"EarthElemental_CalmWalk", "EarthElemental_Alert", "EarthElemental_AgroWalk", "EarthElemental_Forget"
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
