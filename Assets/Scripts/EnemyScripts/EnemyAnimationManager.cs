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
	FIREELEMENTAL_WALK,
	FIREELEMENTAL_ALERT,
	FIREELEMENTAL_ATTACK,
	FIREELEMENTAL_FORGET,

	//Dying animations
	FIREELEMENTAL_DYING,
	EARTHELEMENTAL_DYING
}


/// <summary>
/// Controls the change in AI animations.
/// Created by: Kane Adams
/// </summary>
public class EnemyAnimationManager : MonoBehaviour
{
	public Animator anim;

	public string currentAnimState;
	private string[] animations = {
		"EarthElemental_CalmMove", "EarthElemental_Alert", "EarthElemental_AgroMove", "EarthElemental_Forget",
		"FireElemental_Idle", "FireElemental_Walk", "FireElemental_Alert", "FireElemental_Attack", "FireElemental_Forget", 
		"FireElemental_Dying", "EarthElemental_Dying"
	};

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
	}

	/// <summary>
	/// Changes current enemy's animation in relation to its actions
	/// </summary>
	/// <param name="a_newAnim">Animation for new action</param>
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
