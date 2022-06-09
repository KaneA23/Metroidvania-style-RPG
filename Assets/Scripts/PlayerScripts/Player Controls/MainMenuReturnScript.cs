using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls player returning to main menu.
/// Created by: Kane Adams
/// </summary>
public class MainMenuReturnScript : MonoBehaviour
{
	public Animator transition;

	public float transitionTime = 1f;

	// Update is called once per frame
	void Update()
	{
		// Returns to main menu (will change to pause menu in future
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			StartCoroutine(LoadLevel("Main Menu Scene"));
		}

		// Resets level
		if (Input.GetKeyDown(KeyCode.F5))
		{
			StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
		}
	}

	/// <summary>
	/// Starts fade out transition
	/// </summary>
	/// <param name="a_sceneName">New scene to be entered</param>
	/// <returns>Waits 1 second before changing scene</returns>
	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
