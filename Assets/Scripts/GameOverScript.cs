using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
	[SerializeField] private Animator playerAnim;
	[SerializeField] private Animator deathNPCAnim;

	bool isPlayerEntrance;
	bool isDeathEntrance;

	public string currentAnimState;

	[SerializeField] private GameObject resetButton;

	public Animator transition;

	public float transitionTime = 1f;

	// Start is called before the first frame update
	void Start()
	{
		resetButton.SetActive(false);

		ChangeAnimationState("Player_Dash", playerAnim);
		isPlayerEntrance = true;

		ChangeAnimationState("Death_Invisible", deathNPCAnim);
		isDeathEntrance = true;

		Invoke(nameof(FirstAnimations), 1f);
	}

	// Update is called once per frame
	void Update()
	{
		if (!isPlayerEntrance)
		{
			ChangeAnimationState("Player_Idle", playerAnim);
		}

		if (!isDeathEntrance)
		{
			ChangeAnimationState("Death_Idle", deathNPCAnim);
		}
	}

	public void ChangeAnimationState(string a_newAnim, Animator a_anim)
	{
		// Stops the same animation from interrupting itself
		if (currentAnimState == a_newAnim)
		{
			return;
		}

		// Play the animation
		a_anim.Play(a_newAnim);

		// reassign current state
		currentAnimState = a_newAnim;
	}

	void FirstAnimations()
	{
		ChangeAnimationState("Player_DashExit", playerAnim);
		Invoke(nameof(CompletePlayerAnim), 0.35f);

		ChangeAnimationState("Death_Enter", deathNPCAnim);
		Invoke(nameof(CompleteDeathAnim), 1.5f);
	}

	void CompletePlayerAnim()
	{
		isPlayerEntrance = false;
	}

	void CompleteDeathAnim()
	{
		isDeathEntrance = false;

		resetButton.SetActive(true);
	}

	public void RestartGame()
	{
		if (SceneManager.GetActiveScene().name == "StoryDeathScene")
		{
			StartCoroutine(LoadLevel("Level 1 - Temp - Play Test"));
		}
		else
		{
			StartCoroutine(LoadLevel("Level 1 - Temp - Play Test 1"));
		}
	}

	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
