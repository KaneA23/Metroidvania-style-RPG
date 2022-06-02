using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls Death screen.
/// Created by: Kane Adams
/// </summary>
public class GameOverScript : MonoBehaviour
{
	[SerializeField] private Animator playerAnim;
	[SerializeField] private Animator deathNPCAnim;

	bool isPlayerEntrance;
	bool isDeathEntrance;

	public string currentAnimState;

	[SerializeField] private GameObject resetButton;
	[SerializeField] private GameObject menuButton;

	public Animator transition;

	public float transitionTime = 1f;

	// Start is called before the first frame update
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		resetButton.SetActive(false);
		menuButton.SetActive(false);

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

	/// <summary>
	/// Changes the animation of sprite depentend on sprite's action
	/// </summary>
	/// <param name="a_newAnim">The new action</param>
	/// <param name="a_anim">The sprite's unique animimator</param>
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

	/// <summary>
	/// Player and Death animated entering the gates
	/// </summary>
	void FirstAnimations()
	{
		ChangeAnimationState("Player_DashExit", playerAnim);
		Invoke(nameof(CompletePlayerAnim), 0.35f);

		ChangeAnimationState("Death_Enter", deathNPCAnim);
		Invoke(nameof(CompleteDeathAnim), 1.5f);
	}

	/// <summary>
	/// Player is no longer entering scene
	/// </summary>
	void CompletePlayerAnim()
	{
		isPlayerEntrance = false;
	}

	/// <summary>
	/// Death has entered scene and player can now interact
	/// </summary>
	void CompleteDeathAnim()
	{
		isDeathEntrance = false;

		resetButton.SetActive(true);
		menuButton.SetActive(true);

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	/// <summary>
	/// Respawns player at beginning of current GameMode
	/// </summary>
	public void RestartGame()
	{
		if (SceneManager.GetActiveScene().name == "StoryDeathScene")
		{
			StartCoroutine(LoadLevel("Level 1 - Temp - Play Test"));
		}
		else
		{
			StartCoroutine(LoadLevel("Arena"));
		}
	}

	/// <summary>
	/// Takes player to Main Menu Screen
	/// </summary>
	public void ReturnToMenu()
	{
		StartCoroutine(LoadLevel("Main Menu Scene"));
	}

	/// <summary>
	/// Changes scene with fade
	/// </summary>
	/// <param name="a_sceneName">New scene player is entering</param>
	/// <returns>Waits 1 second before loading new scene</returns>
	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
