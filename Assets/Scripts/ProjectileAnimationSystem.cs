using UnityEngine;

/// <summary>
/// Different states for the animation to change dependent on projectile's actions.
/// </summary>
public enum ProjectileAnimationState
{
	// Fireballs
	FIREBALL_SHOT,
	FIREBALL_MIDAIR,
	FIREBALL_HIT,
}

/// <summary>
/// Controls animations of any projectile player launches.
/// Created by: Kane Adams
/// </summary>
public class ProjectileAnimationSystem : MonoBehaviour
{
	private Animator anim;

	public string currentAnimState;
	private readonly string[] animations = {
		"Fireball_Shot", "Fireball_Midair", "Fireball_Hit"
	};

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	/// <summary>
	/// Changes the animation for what the player is doing
	/// </summary>
	/// <param name="a_newAnim">New animation state</param>
	public void ChangeAnimationState(ProjectileAnimationState a_newAnim)
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
