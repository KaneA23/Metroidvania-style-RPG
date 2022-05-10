using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileAnimationState
{
	// Fireballs
	FIREBALL_SHOT,
	FIREBALL_MIDAIR,
	FIREBALL_HIT,
}

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

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
